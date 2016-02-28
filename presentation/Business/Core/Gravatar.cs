using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Cryptography;
using System.Text;
using System.Drawing;
using System.IO;

namespace presentation.Business.Core
{
    public class Gravatar
    {
        private int pixelSize = 16;
        private int tiles = 5;

        public MemoryStream Render(string id)
        {
            byte[] hash = Hash(id);

            int length = hash.Length;

            Color color = Color.FromArgb(hash[length - 3], hash[length - 2], hash[length - 1], 0);

            int width = pixelSize * tiles;
            int height = width;
            using (Image img = new Bitmap(width, height))
            {
                MemoryStream stream = new MemoryStream();
                
                using (Graphics g = Graphics.FromImage(img))
                {
                    //设置位图的背景颜色，默认是黑色
                    g.Clear(Color.White);

                    var brush = new SolidBrush(color);

                    bool[] flagArr = GetPixelFlag(hash);
                    for (int i = 0; i < tiles; i++)
                    {
                        for (int j = 0; j < tiles; j++)
                        {
                            if (flagArr[Reflact(i, j)])
                            {
                                g.FillRectangle(brush, i * pixelSize, j * pixelSize, pixelSize, pixelSize);
                            }
                        }
                    }

                    img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);//保存验证码对象，指定是Jpeg格式
                    return stream;
                }
            }
        }

        /// <summary>
        /// 得到指定id的哈希值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static byte[] Hash(string id)
        {
            byte[] source;
            byte[] hash;

            //Create a byte array from source data.
            source = ASCIIEncoding.ASCII.GetBytes(id);

            //Compute hash based on source data.
            hash = new MD5CryptoServiceProvider().ComputeHash(source);
            return hash;
        }

        /// <summary>
        /// 得到哈希值对应的格子图中每个格子是否不为空白的标志
        /// </summary>
        /// <param name="hashArray"></param>
        /// <returns></returns>
        private static bool[] GetPixelFlag(byte[] hash)
        {
            if (hash == null)
            {
                throw new ArgumentNullException("hashArray");
            }

            int length = hash.Length;
            bool[] flagArr = new bool[length];

            for (int i = 0; i < length; i++)
            {
                flagArr[i] = (hash[i] & 0x1) == 0;
            }

            return flagArr;
        }

        /// <summary>
        /// 得到当前坐标在方格的位置，左侧不变，右侧会镜像到左侧。
        /// </summary>
        /// <param name="x">col</param>
        /// <param name="y">row</param>
        /// <returns></returns>
        private int Reflact(int x, int y)
        {
            int mid = this.tiles / 2;
            if (x > mid)
            {
                x = 2 * mid - x;
            }
            return x + mid * y;
        }
    }
}






