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
using PYO2016_Client.Sources.Capture;
using System.Windows.Forms;
using PYO2016_Client.Sources.HttpGetter;
using System.IO;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using PYO2016_Client.Sources.Model;
using FirstFloor.ModernUI.Windows.Controls;
using PYO2016_Client.Sources.Encode;

namespace PYO2016_Client.Pages
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class BasicPage1 : System.Windows.Controls.UserControl
    {
        static private int ITEM_COUNT_MAX = 10;
        static private string MAX_WARNING_TITLE = "Failed";
        static private string MAX_WRANING_TEXT = "한 번에 분석 가능한 이미지는 최대 10개 입니다.";

        static private System.Windows.Controls.ListView listView;
        static private object thisLock = new object();

        public BasicPage1()
        {
            InitializeComponent();
            listView = (System.Windows.Controls.ListView)this.FindName("fileList");
            KeyHooker.getInstance().SetHook();
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        static public void static_captureButton_Click()
        {
            if (listView.Items.Count >= ITEM_COUNT_MAX)
            {
                ModernDialog.ShowMessage(MAX_WRANING_TEXT, MAX_WARNING_TITLE, MessageBoxButton.OK);
                return;
            }

            lock (thisLock)
            {
                CaptureTool.getInstance().capture(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\pyo-capture");
                if (CaptureTool.getInstance().getPath() == null)
                    return;
                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (listView.Items[0].ToString() == CaptureTool.getInstance().getPath())
                        return;
                }
                listView.Items.Add(CaptureTool.getInstance().getPath());
            }
        }

        private void captureButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (listView.Items.Count >= ITEM_COUNT_MAX)
            {
                ModernDialog.ShowMessage(MAX_WRANING_TEXT, MAX_WARNING_TITLE, MessageBoxButton.OK);
                return;
            }

            lock (thisLock)
            {
                CaptureTool.getInstance().capture(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\pyo-capture");
                if (CaptureTool.getInstance().getPath() == null)
                    return;
                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (listView.Items[0].ToString() == CaptureTool.getInstance().getPath())
                        return;
                }
                fileAddex(CaptureTool.getInstance().getPath());
            }
        }

        private void fileAdd(object sender, RoutedEventArgs e)
        {
            if (listView.Items.Count >= ITEM_COUNT_MAX)
            {
                ModernDialog.ShowMessage(MAX_WRANING_TEXT, MAX_WARNING_TITLE, MessageBoxButton.OK);
                return;
            }

            OpenFileDialog openImgFileDialog = new OpenFileDialog();
            openImgFileDialog.Filter = "Cursor Files|*.jpg;*.png;*.bmp";
            openImgFileDialog.Title = "Select a ImageFile";
            if (openImgFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (listView.Items[0].ToString() == openImgFileDialog.FileName)
                        return;
                }
                fileAddex(openImgFileDialog.FileName);
            }
        }

        private void fileAddex(string filename)
        {
            if (listView.Items.Count >= ITEM_COUNT_MAX)
            {
                return;
            }

            listView.Items.Add(filename);
        }

        private void fileRemove(object sender, RoutedEventArgs e)
        {
            for (int i = listView.SelectedItems.Count - 1; i >= 0; --i)
            {
                listView.Items.RemoveAt(i);
            }
        }

        private void analysisButton_Click(object sender, RoutedEventArgs e)
        {
            if (listView.Items.Count > 0)
            {
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        for (int i = 0; i < listView.Items.Count; i++)
                        {
                            FileInfo fileInfo = new FileInfo(listView.Items[i].ToString());
                            try
                            {
                                //var fileContent = new ByteArrayContent(File.ReadAllBytes(listView.Items[i].ToString()));//(System.IO.File.ReadAllBytes(fileName));
                                var fileContent = new ByteArrayContent(PyoEncoder.Encode(listView.Items[i].ToString()));
                                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                                {
                                    FileName = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf(fileInfo.Extension)) + DateTime.Now.ToString("__yyyyMMdd_HHmmss") + "_" + i + ".png"
                                };
                                content.Add(fileContent);
                            }
                            catch (Exception ex)
                            {
                                //Log the exception
                            }
                        }
                        
                        
                        var result = client.PostAsync(HttpGetter.getURL() + "api/Upload?pk=" + Convert.ToString(AccessTokenManager.getInstance().getToken()), content).Result;
                        
                        ModernDialog.ShowMessage("Send finished", FirstFloor.ModernUI.Resources.Ok, MessageBoxButton.OK);

                        for (int i = listView.Items.Count - 1; i >= 0; --i)
                        {
                            listView.Items.RemoveAt(i);
                        }
                    }
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Nothing is added");
            }
        }
    }
}
