using System;
using System.Drawing;

namespace AHNet.Web.Util
{
    public class Imaging
    {
        public static Bitmap ResizeImage(Bitmap originalImage, int height, int width)
        {
            return ResizeImage(originalImage, height, width, false, "");
        }

        public static Bitmap ResizeImage(Bitmap originalImage, int height, int width, bool letterbox, string lbxColor)
        {
            int srcWidth = originalImage.Width;
            int srcHeight = originalImage.Height;

            Size newSize;

            if (height != 0 && width != 0)
            {
                newSize = GetOutSize(originalImage, width, height);
            }
            else
            {
                if (srcWidth > srcHeight)
                    newSize = GetOutSize(originalImage, width, height);
                else
                    newSize = GetOutSize(originalImage, height, width);
            }
            //Decimal sizeRatio = ((Decimal)srcHeight / srcWidth);
            //int thumbHeight = Decimal.ToInt32(sizeRatio * resizeWidth);

            Bitmap bmp = null;

            if (!letterbox)
                bmp = new Bitmap(newSize.Width, newSize.Height);
            else
                bmp = new Bitmap(width, height);

            using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp))
            {
                Color cl = Color.Black;

                if (!String.IsNullOrEmpty(lbxColor))
                {
                    cl = ColorTranslator.FromHtml(lbxColor);
                }
                else
                {
                    cl = ColorTranslator.FromHtml("transparent");
                }

                using (Brush brBG = new SolidBrush(cl))
                {
                    gr.FillRectangle(brBG, new Rectangle(0, 0, width, height));
                }

                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                Point newPos = new Point(0, 0);

                if (letterbox)
                    newPos = new Point((width / 2) - (newSize.Width / 2), (height / 2) - (newSize.Height / 2));

                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(newPos, newSize);

                gr.DrawImage(originalImage, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);
            }
            return bmp;
        }

        private static Size GetOutSize(Bitmap originalImage, int size)
        {
            int HSize = 0;
            int VSize = 0;
            int w = originalImage.Width;
            int h = originalImage.Height;
            int ArSize = size;

            if (ArSize != 0)
            {
                //Se ha especificado un valor mediante la propiedad "size":

                /*Nótese que si se ha especificado el alto y/o el ancho, esta propiedad
                    no tendrá efecto alguno*/

                if (w > h)
                {
                    //Si la imágen es mas ancha que alta:
                    HSize = ArSize;
                    VSize = Convert.ToInt32(h * (100f / w * ArSize) / 100);
                }
                else
                {
                    //Si la imágen es mas alta que ancha:
                    VSize = ArSize;
                    HSize = Convert.ToInt32(w * (100f / h * ArSize) / 100);
                }
            }

            return new Size(HSize, VSize);
        }

        private static Size GetOutSize(Bitmap originalImage, int fwidth, int fheight)
        {
            int HSize = 0;
            int VSize = 0;
            int w = originalImage.Width;
            int h = originalImage.Height;
            int ParamW = fwidth;
            int ParamH = fheight;

            if (w > h)
            {
                //Si la imágen es mas ancha que alta:
                HSize = fwidth;
                VSize = Convert.ToInt32(h * (100f / w * HSize) / 100);
                if (VSize > fheight)
                {
                    VSize = fheight;
                    HSize = Convert.ToInt32(w * (100f / h * VSize) / 100);
                }
            }
            else
            {
                //Si la imágen es mas alta que ancha:
                VSize = fheight;
                HSize = Convert.ToInt32(w * (100f / h * VSize) / 100);
                if (HSize > fheight)
                {
                    HSize = fwidth;
                    VSize = Convert.ToInt32(h * (100f / w * HSize) / 100);
                }
            }

            return new Size(HSize, VSize);
        }

        internal static Bitmap ResizeImage(Bitmap _out, int size)
        {
            return ResizeImage(_out, size, false);
        }

        internal static Bitmap ResizeImage(Bitmap _out, int size, bool letterbox)
        {
            int srcHeight = _out.Height;
            int srcWidth = _out.Width;

            if (srcWidth > srcHeight)
            {
                decimal sizeRatio = ((decimal)srcHeight / srcWidth);
                int thumbHeight = Decimal.ToInt32(sizeRatio * size);

                return ResizeImage(_out, thumbHeight, size, letterbox, "");
            }
            else
            {
                decimal sizeRatio = ((Decimal)srcWidth / srcHeight);
                int thumbWidth = Decimal.ToInt32(sizeRatio * size);

                return ResizeImage(_out, size, thumbWidth, letterbox, "");
            }
        }
    }
}
