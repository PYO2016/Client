using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PYO2016_Client.Sources.Encode
{
    class PyoEncoder
    {
        public static String EncodeAndSave(String path)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();

                String newPath = Path.ChangeExtension(path, "png");

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    encoder.Save(stream);
                }
                return newPath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static byte[] Encode(String path)
        {
            byte[] encoded;
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();

                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
               
                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    encoded = stream.ToArray();
                }

                return encoded;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
