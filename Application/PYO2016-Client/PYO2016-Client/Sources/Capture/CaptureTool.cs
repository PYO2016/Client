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
using System.Windows.Interop;

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
            
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            
            mainWindow.WindowState = WindowState.Maximized;
            mainWindow.WindowStyle = WindowStyle.None;
            mainWindow.BorderThickness = new Thickness(0);

            Canvas myCanvas = new Canvas();
            myCanvas.Width = resolution.Width;
            myCanvas.Height = resolution.Height; 
            myCanvas.Background = getImageBrush();
            
            mainWindow.Content = myCanvas;
            mainWindow.MouseDown += new System.Windows.Input.MouseButtonEventHandler(mouseDown);
            mainWindow.Show();
        }

        private ImageBrush getImageBrush()
        {
            System.Drawing.Image img = System.Windows.Forms.Clipboard.GetImage();

            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(printscreen as System.Drawing.Image);

            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            

            //var bitmap = new System.Drawing.Bitmap(img);
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(printscreen.GetHbitmap(),
                                                                        IntPtr.Zero,
                                                                        Int32Rect.Empty,
                                                                        BitmapSizeOptions.FromEmptyOptions()
            );
            printscreen.Dispose();
            var brush = new ImageBrush(bitmapSource);
            return brush;
        }

        private void mouseDown(object sender, EventArgs e)
        {
            System.Windows.MessageBox.Show("Event Catch");
        }

    }
}
