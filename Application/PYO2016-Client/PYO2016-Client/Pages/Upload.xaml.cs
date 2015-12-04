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

namespace PYO2016_Client.Pages
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class BasicPage1 : System.Windows.Controls.UserControl
    {
        private System.Windows.Controls.ListView listView;
        public BasicPage1()
        {
            InitializeComponent();
            listView = (System.Windows.Controls.ListView)this.FindName("fileList");
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }
        private void captureButton_Click_1(object sender, RoutedEventArgs e)
        {
            CaptureTool.getInstance().capture(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\pyo-capture");
            CaptureTool.captureResetEvent.WaitOne();
            for (int i = 0; i < listView.Items.Count; i++)
            {
                if (listView.Items[0].ToString() == CaptureTool.getInstance().getPath())
                    return;
            }
            fileAddex(CaptureTool.getInstance().getPath());
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
            if(listView.SelectedIndex != -1)
                listView.Items.RemoveAt(listView.SelectedIndex);
        }
    }
}
