using FirstFloor.ModernUI.Windows.Controls;
using Newtonsoft.Json.Linq;
using PYO2016_Client.Sources.Capture;
using PYO2016_Client.Sources.HttpGetter;
using PYO2016_Client.Sources.Model;
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
                if (e == null)
                    ModernDialog.ShowMessage("Table Parse Error (Did you send non-tablize image?)", FirstFloor.ModernUI.Resources.Ok, MessageBoxButton.OK);
                else
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
            string result = HttpGetter.HttpGet("http://210.118.74.141:25430/api/ParsedTables?pk=" + AccessTokenManager.getInstance().getToken());
            JArray a = JArray.Parse(result);
            FileManager.getInstance().clear();

            for (int i = list.Items.Count - 1; i >= 0; --i)
            {
                list.Items.RemoveAt(i);
            }

            foreach (JObject o in a.Children<JObject>())
            {
                if (o["isProccessed"].ToString() == "False")
                {
                    string s = FileManager.getInstance().addValue("running " + o["time"].ToString(), o["pk"].ToString());
                    list.Items.Add(s);
                }
                else
                {
                    if (o["result"].ToString() != "NULL")
                    {
                        string s = FileManager.getInstance().addValue("finished " + o["time"].ToString(), o["pk"].ToString());
                        list.Items.Add(s);
                    }
                }
            }
        }

        private void addResult(string date, string value)
        {
            FileManager.getInstance().addValue(date, value);
            list.Items.Add(date);
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                string content = (string)item.Content;
                string call = "http://210.118.74.141:25430/api/ParsedTables/" + FileManager.getInstance().getValue(content) + "?pk=" + AccessTokenManager.getInstance().getToken();
                string result = HttpGetter.HttpGet(call);

                JObject obj = JObject.Parse(result);

                if (obj["isProccessed"].ToString() == "False")
                {
                    SetHtml(wb, @"This job is running... wait please...");
                }
                else
                {
                    if (obj["result"].ToString() != "")
                        SetHtml(wb, @obj["result"].ToString());
                    else
                    {
                        //ModernDialog.ShowMessage("Table Parse Error (Did you send non-tablize image?)", FirstFloor.ModernUI.Resources.Ok, MessageBoxButton.OK);
                        SetHtml(wb, @"Table Parse Error (Did you send non-tablize image?)");
                    }
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            for (int i = list.SelectedItems.Count - 1; i >= 0; --i)
            {
                string content = (string)list.SelectedItems[i];
                string call = "http://210.118.74.141:25430/api/ParsedTables/" + FileManager.getInstance().getValue(content) + "?pk=" + AccessTokenManager.getInstance().getToken();
                string res = HttpGetter.HttpDelete(call);
                if ( res == "BAD" )
                {
                    ModernDialog.ShowMessage("Table didn't processed yet", FirstFloor.ModernUI.Resources.Ok, MessageBoxButton.OK);
                    return;
                }
                list.Items.RemoveAt(i);
                FileManager.getInstance().removeValue(i);
            }

            string result = HttpGetter.HttpGet("http://210.118.74.141:25430/api/ParsedTables?pk=" + AccessTokenManager.getInstance().getToken());
            JArray a = JArray.Parse(result);
            FileManager.getInstance().clear();

            for (int i = list.Items.Count - 1; i >= 0; --i)
            {
                list.Items.RemoveAt(i);
            }

            foreach (JObject o in a.Children<JObject>())
            {
                if (o["isProccessed"].ToString() == "False")
                {
                    string s = FileManager.getInstance().addValue("running " + o["time"].ToString(), o["pk"].ToString());
                    list.Items.Add(s);
                }
                else
                {
                    if (o["result"].ToString() != "NULL")
                    {
                        string s = FileManager.getInstance().addValue("finished " + o["time"].ToString(), o["pk"].ToString());
                        list.Items.Add(s);
                    }
                }
            }
        }
    }
}