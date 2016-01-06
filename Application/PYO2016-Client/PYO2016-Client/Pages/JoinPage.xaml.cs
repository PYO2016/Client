using FirstFloor.ModernUI.Windows.Controls;
using PYO2016_Client.Sources.HttpGetter;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for JoinPage.xaml
    /// </summary>
    public partial class JoinPage : System.Windows.Controls.UserControl
    {
        private static string[] paramName = { "Email", "Password", "ConfirmPassword" };
        public JoinPage()
        {
            InitializeComponent();
        }

        private void JoinBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] param = new string[3];
            param[0] = ((System.Windows.Controls.TextBox)(FindName("idText"))).Text;
            param[1] = ((System.Windows.Controls.PasswordBox)(FindName("pwText"))).Password;
            param[2] = ((System.Windows.Controls.PasswordBox)(FindName("confirmText"))).Password;
            if (param[1] != param[2])
            {
                System.Windows.Forms.MessageBox.Show("Confirmed Password does not match to the Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string result = HttpGetter.HttpPost("http://pyoserver.azurewebsites.net/api/Account/Register", paramName, param);
                //string result = HttpGetter.HttpPost("http://localhost:25430/api/Account/Register", paramName, param);
                if(result.Equals(""))
                {
                    BBCodeBlock bs = new BBCodeBlock();
                    try
                    {
                        bs.LinkNavigator.Navigate(new Uri("/Pages/Home.xaml", UriKind.Relative), this);
                    }
                    catch (Exception error)
                    {
                        ModernDialog.ShowMessage(error.Message, FirstFloor.ModernUI.Resources.NavigationFailed, MessageBoxButton.OK);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception error)
            {
                ModernDialog.ShowMessage(error.Message, FirstFloor.ModernUI.Resources.NavigationFailed, MessageBoxButton.OK);
            }
        }
    }
}
