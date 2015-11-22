using System;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace PYO2016_Client.Sources.Capture
{
    class CaptureTool
    {
        private static CaptureTool instance = null;
        private static Window mainWindow;

        private CaptureTool()
        {}
        public static CaptureTool getInstance()
        {
            if (instance == null)
                instance = new CaptureTool();
            return instance;
        }

        public void capture(String path)
        {
            mainWindow = new Window();


            //BitmapImage theImage = new BitmapImage
            //    (new Uri(path+"\\sunoh.jpg", UriKind.Relative));
            BitmapImage theImage = System.Windows.Forms.Clipboard.GetImage();

            ImageBrush myImageBrush = new ImageBrush(theImage);

            Canvas myCanvas = new Canvas();
            myCanvas.Width = 300;
            myCanvas.Height = 200;
            myCanvas.Background = myImageBrush;
            
            mainWindow.Content = myCanvas;
            mainWindow.Title = "Canvas Sample";
            mainWindow.Show();
        }
    }
}
