using System;
using System.Text;

using System.IO;
using System.Drawing;
using System.Security.Cryptography;

namespace Web.Core
{
    class Gravatar
    {
        private int pixelSize = 15;
        private int width = 6;
        private int height = 6;

        public MemoryStream Render(string id)
        {
            byte[] hash = Hash(id);

            int length = hash.Length;
            Brush[] brushes = new Brush[3];
            for (int m = 0; m < 3; m++)
            {
                brushes[m] = new SolidBrush(
                    Color.FromArgb(
                        hash[length - 4 * m - 4],
                        hash[length - 4 * m - 3],
                        hash[length - 4 * m - 2],
                        hash[length - 4 * m - 1]
                    )
                );
            }

            int width = pixelSize * this.width;
            int height = pixelSize * this.height;
            using (Image img = new Bitmap(width, height))
            {
                MemoryStream stream = new MemoryStream();

                using (Graphics g = Graphics.FromImage(img))
                {
                    //设置位图的背景颜色，默认是黑色
                    g.Clear(Color.White);

                    int mid = this.width / 2;
                    for (int i = 0; i < this.width; i++)
                    {
                        for (int j = 0; j < this.height; j++)
                        {
                            int col = i >= mid ? 2 * mid - 1 - i : i;
                            if (Check(hash, col, j))
                            {
                                g.FillRectangle(brushes[col], i * pixelSize, j * pixelSize, pixelSize, pixelSize);
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
        /// 
        /// </summary>
        /// <param name="hashArray"></param>
        /// <returns></returns>
        private bool Check(byte[] hash, int x, int y)
        {
            int index = y * this.width + x;
            int row = index / 8;
            int col = index - row * 8;

            return (hash[row] & Convert.ToInt32(Math.Pow(2, col))) != 0;
        }

        /// <summary>
        /// 得到当前坐标在方格的位置，左侧不变，右侧会镜像到左侧。
        /// </summary>
        /// <param name="x">col</param>
        /// <param name="y">row</param>
        /// <returns></returns>
        private int Reflact(int x, int y)
        {
            int mid = this.width / 2;
            if (x > mid)
            {
                x = 2 * mid - x;
            }
            return x + mid * y;
        }
    }
}
