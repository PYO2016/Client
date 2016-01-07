using FirstFloor.ModernUI.Windows.Controls;
using Newtonsoft.Json.Linq;
using PYO2016_Client.Sources.HttpGetter;
using PYO2016_Client.Sources.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PYO2016_Client.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : System.Windows.Controls.UserControl
    {
        private static string[] paramName = { "Email", "Password" };
        public string url = "";

        public Home()
        {
            InitializeComponent();
            Attributes instance = Attributes.getInstance();
        }

        private void LoginBtnClick(object sender, RoutedEventArgs e)
        {
            string[] param = new string[2];
            param[0] = ((System.Windows.Controls.TextBox)(FindName("idText"))).Text;
            param[1] = ((System.Windows.Controls.PasswordBox)(FindName("pwText"))).Password;

            try
            {    
                string result = HttpGetter.HttpPost("http://pyoserver.azurewebsites.net/api/Account/Login", paramName, param);
                JObject obj = JObject.Parse(result);
                //JArray array = JArray.Parse(obj["pk"].ToString());
                //string res = array[0].ToString();
                string res = obj["pk"].ToString();

                AccessTokenManager.getInstance().setToken(res);

                Attributes instance = Attributes.getInstance();
                instance.addLinkGroup(1);

                BBCodeBlock bs = new BBCodeBlock();
                try
                {    
                    bs.LinkNavigator.Navigate(new Uri("/Pages/Upload.xaml", UriKind.Relative), this);
                    instance.removeLinkGroup(0);
                }
                catch (Exception error)
                {
                    ModernDialog.ShowMessage(error.Message, FirstFloor.ModernUI.Resources.NavigationFailed, MessageBoxButton.OK);
                }
            }
            catch (Exception error)
            {
                ModernDialog.ShowMessage(error.Message, FirstFloor.ModernUI.Resources.NavigationFailed, MessageBoxButton.OK);
            }
        }
    }
}
