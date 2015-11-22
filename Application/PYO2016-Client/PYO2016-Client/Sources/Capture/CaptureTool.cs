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
using System.Windows.Input;
using System.IO;

namespace PYO2016_Client.Sources.Capture
{
    class CaptureTool
    {
        private static CaptureTool instance = null;
        // is clicking??
        private bool isInClick;
        private System.Windows.Point clickPoint;
        private static Window mainWindow;
        private Canvas canvas;
        private String path;
        private BitmapSource bitmapSource;
        System.Windows.Shapes.Rectangle rect;

        private CaptureTool()
        {
            isInClick = false;
        }
        public static CaptureTool getInstance()
        {
            if (instance == null)
                instance = new CaptureTool();
            return instance;
        }

        public void capture(String path)
        {
            mainWindow = new Window();
            this.path = path;
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            
            mainWindow.WindowState = WindowState.Maximized;
            mainWindow.WindowStyle = WindowStyle.None;
            mainWindow.BorderThickness = new Thickness(0);

            canvas = new Canvas();
            canvas.Width = resolution.Width;
            canvas.Height = resolution.Height; 
            canvas.Background = getImageBrush();
            
            mainWindow.Content = canvas;
            mainWindow.MouseDown += new System.Windows.Input.MouseButtonEventHandler(mouseDown);
            mainWindow.MouseMove += new System.Windows.Input.MouseEventHandler(mouseMove);
            mainWindow.MouseUp += new System.Windows.Input.MouseButtonEventHandler(mouseDown);
            mainWindow.Show();
        }

        private ImageBrush getImageBrush()
        {
            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(printscreen as System.Drawing.Image);

            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            

            //var bitmap = new System.Drawing.Bitmap(img);
            bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(printscreen.GetHbitmap(),
                                                                        IntPtr.Zero,
                                                                        Int32Rect.Empty,
                                                                        BitmapSizeOptions.FromEmptyOptions()
            );
            printscreen.Dispose();
            var brush = new ImageBrush(bitmapSource);
            return brush;
        }

        private void mouseDown(object sender, MouseButtonEventArgs e)
        {
            //System.Windows.MessageBox.Show("Event Catch");
            if (isInClick == false) { 
                isInClick = true;
                clickPoint = e.GetPosition(canvas);
                rect = new System.Windows.Shapes.Rectangle();
                rect.Stroke = new SolidColorBrush(Colors.Aqua);
                rect.Fill = new SolidColorBrush(Colors.Aqua);
                rect.Opacity = 0.5;
                rect.Width = 0;
                rect.Height = 0;
                Canvas.SetLeft(rect, this.clickPoint.X);
                Canvas.SetTop(rect, this.clickPoint.Y);
                canvas.Children.Add(rect);
            }
        }
        private void mouseMove(object sender, EventArgs e)
        {
            if(Mouse.LeftButton == MouseButtonState.Pressed)
            {
                System.Windows.Point p = Mouse.GetPosition(canvas);
                rect.Width = Math.Abs(this.clickPoint.X - p.X);
                rect.Height = Math.Abs(this.clickPoint.Y - p.Y);
                if (this.clickPoint.X - p.X > 0)
                    Canvas.SetLeft(rect, this.clickPoint.X - Math.Abs(this.clickPoint.X - p.X));
                else
                    Canvas.SetLeft(rect, this.clickPoint.X);
                if (this.clickPoint.Y - p.Y > 0)
                    Canvas.SetTop(rect, this.clickPoint.Y - Math.Abs(this.clickPoint.Y - p.Y));
                else
                    Canvas.SetTop(rect, this.clickPoint.Y);
            }
            else if (Mouse.LeftButton == MouseButtonState.Released)
            {
                if (isInClick)
                {
                    System.Windows.Point p = Mouse.GetPosition(canvas);
                    isInClick = false;
                    canvas.Children.Remove(rect);
                    mainWindow.Hide();
                    double x, y;
                    if (this.clickPoint.X - p.X > 0)
                        x = this.clickPoint.X - Math.Abs(this.clickPoint.X - p.X);
                    else
                        x = this.clickPoint.X;
                    if (this.clickPoint.Y - p.Y > 0)
                        y = this.clickPoint.Y - Math.Abs(this.clickPoint.Y - p.Y);
                    else
                        y = this.clickPoint.Y;

                    SaveBitmapSourceImageToFile(x, y
                        , Math.Abs(this.clickPoint.X - p.X)
                        , Math.Abs(this.clickPoint.Y - p.Y));

                    mainWindow.Close();
                }
            }
        }
        public void SaveBitmapSourceImageToFile(double x1, double y1, double x2, double y2)
        {
            var image = this.bitmapSource;
            var cI = new CroppedBitmap(image, new Int32Rect((int)x1, (int)y1, (int)x2, (int)y2));

            this.path += "\\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(cI));
                encoder.Save(fileStream);
            }
        }

        private void mouseUp(object sender, MouseButtonEventArgs e)
        {
            //System.Windows.MessageBox.Show("Event Catch");
            if (isInClick)
                isInClick = false;
        }

    }
}
