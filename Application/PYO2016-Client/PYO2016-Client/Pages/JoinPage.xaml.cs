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
            param[1] = ((System.Windows.Controls.TextBox)(FindName("pwText"))).Text;
            param[2] = ((System.Windows.Controls.TextBox)(FindName("confirmText"))).Text;
            if(param[1] != param[2])
            {
                System.Windows.Forms.MessageBox.Show("hihihi", "hell",
                                     MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question);
                return;
            }
            string result = HttpGetter.HttpPost("http://pyoserver.azurewebsites.net/api/Account/Register", paramName, param);
        }
    }
}
