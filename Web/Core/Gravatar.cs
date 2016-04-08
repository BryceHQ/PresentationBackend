using System;
using System.Text;

using System.IO;
using System.Drawing;
using System.Security.Cryptography;

namespace Web.Core
{
    public class Gravatar
    {
        private int _pixelSize = 15;
        private int _width = 6;
        private int _height = 6;

        private static Gravatar _default = new Gravatar();

        /// <summary>
        /// 默认的Gravatar
        /// </summary>
        public static Gravatar Default
        {
            get
            {
                return _default;
            }
        }

        public Stream Render(string id, Stream stream)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }
            if(stream == null)
            {
                stream = new MemoryStream();
            }
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

            int width = _pixelSize * this._width;
            int height = _pixelSize * this._height;
            using (Image img = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(img))
                {
                    //设置位图的背景颜色，默认是黑色
                    g.Clear(Color.White);

                    int mid = this._width / 2;
                    for (int i = 0; i < this._width; i++)
                    {
                        for (int j = 0; j < this._height; j++)
                        {
                            int col = i >= mid ? 2 * mid - 1 - i : i;
                            if (Check(hash, col, j))
                            {
                                g.FillRectangle(brushes[col], i * _pixelSize, j * _pixelSize, _pixelSize, _pixelSize);
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
        /// 判断某一坐标位置处是否有内容
        /// </summary>
        /// <param name="hashArray"></param>
        /// <returns></returns>
        private bool Check(byte[] hash, int x, int y)
        {
            int index = y * this._width + x;
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
            int mid = this._width / 2;
            if (x > mid)
            {
                x = 2 * mid - x;
            }
            return x + mid * y;
        }
    }
}
