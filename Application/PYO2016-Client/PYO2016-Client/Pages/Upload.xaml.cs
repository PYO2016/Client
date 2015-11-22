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

namespace PYO2016_Client.Pages
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class BasicPage1 : UserControl
    {
        public BasicPage1()
        {
            InitializeComponent();
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {

        }
        private void captureButton_Click_1(object sender, RoutedEventArgs e)
        {
            CaptureTool.getInstance().capture("C:\\Users\\KGWANGMIN\\Documents");
        }
    }
}
