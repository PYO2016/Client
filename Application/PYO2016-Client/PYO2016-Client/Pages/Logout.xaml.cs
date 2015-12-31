using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PYO2016_Client.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Logout : System.Windows.Controls.UserControl
    {

        public Logout()
        {
            Attributes instance = Attributes.getInstance();
            instance.addLinkGroup(0);
            Task.Factory.StartNew(() => Thread.Sleep(100)).ContinueWith((t) =>
            {
                logout();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        
        public void logout()
        {
            Attributes instance = Attributes.getInstance();
            //instance.removeLinkGroup(1);
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
    }
}
