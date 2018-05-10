using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageTrimming
{
    public static class ImageTrimming
    {
        /// <summary>
        /// トリミング条件
        /// この色を削除してもいいならtrue、駄目ならfalse
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public delegate bool IsTrimming(Color color);

        /// <summary>
        /// 条件に従って画像を矩形にトリミングします
        /// </summary>
        /// <param name="img">対象の画像</param>
        /// <param name="isTrimming">トリミング条件</param>
        /// <returns>トリミング済み画像</returns>
        public static Bitmap Trimming(Bitmap img,IsTrimming isTrimming)
        {
            //必要な範囲
            int l = 0;
            int t = 0;
            int r = img.Size.Width - 1;
            int b = img.Size.Height - 1;

            /*範囲を求める*/
            //左
            for (int x = 0; x < r; x++)
            {
                if (NeedH(img, x,isTrimming))
                {
                    l = x;
                    break;
                }
            }

            //t
            for (int y = 0; y < b; y++)
            {
                if (NeedW(img, y, isTrimming))
                {
                    t = y;
                    break;
                }
            }

            //r
            for (int x = img.Size.Width - 1; x >= 0; x--)
            {
                if (NeedH(img, x, isTrimming))
                {
                    r = x;
                    break;
                }
            }

            //b
            for (int y = img.Size.Height - 1; y >= 0; y--)
            {
                if (NeedW(img, y, isTrimming))
                {
                    b = y;
                    break;
                }
            }

            //トリミング
            Bitmap newImg = img.Clone(new Rectangle(new Point(l, t), new Size(r - l + 1, b - t + 1)), img.PixelFormat);
            img.Dispose();
            return newImg;
        }

        /// <summary>
        /// 指定された横行は必要か
        /// </summary>
        /// <param name="img">画像</param>
        /// <param name="y">調べたい行のy座標</param>
        /// <returns>必要か</returns>
        private static bool NeedW(Bitmap img, int y, IsTrimming isTrimming)
        {
            for (int x = 0; x < img.Size.Width; x++)
            {
                if (isTrimming(img.GetPixel(x, y))) return true;//一つでも必要なピクセルがあるなら
            }
            return false;
        }

        /// <summary>
        /// 指定された縦列は必要か
        /// </summary>
        /// <param name="img">画像</param>
        /// <param name="x">調べたい列のx座標</param>
        /// <returns>必要か</returns>
        private static bool NeedH(Bitmap img, int x, IsTrimming isTrimming)
        {
            for (int y = 0; y < img.Size.Height; y++)
            {
                if (isTrimming(img.GetPixel(x, y))) return true;//一つでも必要なピクセルがあるなら
            }
            return false;
        }
    }
}
