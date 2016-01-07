using PYO2016_Client.Sources.Capture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PYO2016_Client.Pages
{
    /// <summary>
    /// Interaction logic for Result.xaml
    /// </summary>
    public partial class Result : UserControl
    {
        static private System.Windows.Controls.ListView list;
        static private System.Windows.Controls.WebBrowser wb;
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
        "Html",
        typeof(string),
        typeof(Result),
        new FrameworkPropertyMetadata(OnHtmlChanged));

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser d)
        {
            return (string)d.GetValue(HtmlProperty);
        }

        public static void SetHtml(WebBrowser d, string value)
        {
            d.SetValue(HtmlProperty, value);
        }

        static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser wb = d as WebBrowser;
            if (wb != null)
                wb.NavigateToString(e.NewValue as string);
        }

        public Result()
        {
            InitializeComponent();
            // 불러오는 코드
            list= (System.Windows.Controls.ListView)this.FindName("listBox");
            wb = (System.Windows.Controls.WebBrowser)this.FindName("browser");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            list.Items.Add("hello");
            SetHtml(wb, @"<html><head></head><body><h1>hihihi</h1></body></html>");
        }

        private void addResult(string date, string value)
        {
            FileManager.getInstance().addValue(date, value);
            list.Items.Add(date);
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
            }
        }
    }
}