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

namespace PYO2016_Client.Pages
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class BasicPage1 : System.Windows.Controls.UserControl
    {
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
                                var fileContent = new ByteArrayContent(File.ReadAllBytes(listView.Items[i].ToString()));//(System.IO.File.ReadAllBytes(fileName));
                                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                                {
                                    FileName = fileInfo.Name.Substring(0, fileInfo.Name.IndexOf(fileInfo.Extension)) + DateTime.Now.ToString("__yyyyMMdd_HHmmss") + fileInfo.Extension
                                };
                                content.Add(fileContent);
                            }
                            catch (Exception ex)
                            {
                                //Log the exception
                            }
                        }
                        
                        //var result = client.PostAsync("http://localhost:25430/api/Upload?pk=" + Convert.ToString(Attributes.getInstance().getPk()), content).Result;
                        var result = client.PostAsync("http://pyoserver.azurewebsites.net/api/Upload?pk=" + Convert.ToString(AccessTokenManager.getInstance().getToken()), content).Result;
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
