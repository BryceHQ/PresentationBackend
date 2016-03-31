using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Business.Utility
{
    /// <summary>
    /// 字符串(即相应进制的数字)加法
    /// </summary>
    public class StringAddtion
    {
        //进制 
        private int length;

        public int Length
        {
            get
            {
                return length;
            }
        }

        public StringAddtion()
        {
            length = 10;
        }

        public StringAddtion(int len)
        {
            if (len < 0)
            {
                len = 0 - len;
            }
            else if (len > 82)
            {
                len = 82;
            }
            length = len;
        }

        /// <summary>
        /// 对字符串（即某个进制格式的数字）进行加1操作
        /// </summary>
        /// <param name="number">需要进行增加操作的字符串</param>
        /// <param name="scale">该字符串被当做多少进制的数字,范围[2,62]</param>
        /// <param name="throwExceptionWhenOverflow">当字符串的长度变长时，是否抛出溢出异常</param>
        /// <returns>返回增加后的字符串</returns>
        public static string Add(string number, short scale, bool throwExceptionWhenOverflow)
        {
            if (scale < 2 || scale > 62)//最多使用全部的数字和大小写字母，62个字符
            {
                throw new ArgumentOutOfRangeException("scale", "支持的进制范围为[2, 62]");
            }
            char[] cs = number.ToCharArray();
            int numIndex = cs.Length - 1;
            int maxIndex = scale - 1;
            int index = 0;
            bool carryDigitFlag = false;
            for (int i = numIndex; i > -1; i--)
            {
                index = GetIndex(cs[i]);

                if (index > maxIndex)
                {
                    throw new ArgumentOutOfRangeException("number", "参数包含的字符超出了当前进制所使用的字符范围");
                }
                if (index == maxIndex)
                {
                    if (i == 0)
                    {
                        if (throwExceptionWhenOverflow)
                            throw new OverflowException();
                        carryDigitFlag = true;
                    }
                    cs[i] = GetChar(0);
                }
                else
                {
                    cs[i] = GetChar(index + 1);
                    break;
                }
            }
            if (carryDigitFlag)//进位，字符串长度会加1
            {
                return string.Format("1{0}", new string(cs));
            }
            return new string(cs);
        }

        private static int GetIndex(char c)
        {
            int ascCode = (int)c;
            if (ascCode > 47 && ascCode < 58)
            {
                return ascCode - 48;
            }
            else
            {
                if (ascCode > 64 && ascCode < 91)
                {
                    return ascCode - 55;
                }
                else
                {
                    return ascCode - 61;
                }
            }
        }
        private static char GetChar(int index)
        {
            if (index > -1 && index < 10)
            {
                return (char)(index + 48);
            }
            else
            {
                if (index > 9 && index < 36)
                {
                    return (char)(index + 55);
                }
                else
                {
                    return (char)(index + 61);
                }
            }
        }
    }

    public class AttachmentUtility
    {
        private const int NUMBER_LENGTH = 36;
        private const int MAX_FILE_COUNT = 1000;
        private const string FOLDER_NAME_START = "00000001";//folderName 为标准格式 00000001 ~ ZZZZZZZZ
        private static object syncRoot = new object();

        public static string GetAttachmentSaveFolder(string saveDir)
        {
            string folderName = string.Empty;
            string year = DateTime.Now.Year.ToString();
            string serverAttachmentDir = Path.Combine(saveDir, year);
            string fullPath = string.Empty;
            string partialPath = string.Empty;
            lock (syncRoot)
            {
                if (!Directory.Exists(serverAttachmentDir))
                    Directory.CreateDirectory(serverAttachmentDir);
                string[] dirs = Directory.GetDirectories(serverAttachmentDir);
                string maxDir = dirs.OrderBy(s => s).LastOrDefault();//返回的是完整的物理路径
                if (string.IsNullOrEmpty(maxDir))
                {
                    //没有一个目录，创建第一个目录
                    folderName = FOLDER_NAME_START;
                }
                else
                {
                    folderName = new DirectoryInfo(maxDir).Name;	//获取文件夹名
                    if (folderName.Length > FOLDER_NAME_START.Length)
                        throw new Exception("文件夹名称长度大于" + FOLDER_NAME_START.Length + "位");
                    else if (Directory.GetFiles(maxDir).Count() >= MAX_FILE_COUNT)
                    {
                        //目录内的文件达到上限MAX_FILE_COUNT，创建一个新的目录
                        folderName = StringAddtion.Add(folderName, NUMBER_LENGTH, true);
                    }
                }

                fullPath = Path.Combine(serverAttachmentDir, folderName);

                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
                partialPath = Path.Combine(year, folderName);
            }
            return partialPath;
        }

    }
}
