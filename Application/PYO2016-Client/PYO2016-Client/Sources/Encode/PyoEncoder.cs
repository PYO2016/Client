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
        public static String Encode(String path)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();

                String newPath = Path.ChangeExtension(path, "png");

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    encoder.Save(stream);
                }
                return newPath;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
