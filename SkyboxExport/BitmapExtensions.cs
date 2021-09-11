using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace SkyboxExport
{
    public static class BitmapExtensions
    {
        public static void ValidateImport(this Bitmap bitmap, int resolution)
        {
            if (bitmap.Width != resolution*4)
            {
                throw new Exception("Width isnt multiple of 4");
            }

            if (bitmap.Height != resolution * 3)
            {
                throw new Exception("Width isnt multiple of 4");
            }
        }

        public static void ValidateExport(this Bitmap bitmap, int resolution)
        {
            if (bitmap.Width != resolution)
            {
                throw new Exception(FormattableString.Invariant($"Width isnt {resolution} but {bitmap.Width}"));
            }

            if (bitmap.Height != resolution)
            {
                throw new Exception(FormattableString.Invariant($"Height isnt {resolution} but {bitmap.Height}"));
            }
        }

        public static BitmapImage ToImageSource(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
