using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using System.Linq;

namespace PYO2016_Client.Pages
{
    internal class Attributes
    {
        private int pk;
        private ModernWindow window;
        private LinkGroup group0, group1;
        private Attributes()
        {
            pk = -1;
            window = App.Current.MainWindow as ModernWindow;
            group0 = window.MenuLinkGroups.ElementAt(0);
            group1 = window.MenuLinkGroups.ElementAt(1);

            window.MenuLinkGroups.RemoveAt(1);
        }

        private static Attributes instance = new Attributes();

        public static Attributes getInstance()
        {
            return instance;
        }

        public int getPk()
        {
            return pk;
        }

        public ModernWindow getWindow()
        {
            return window;
        }

        public LinkGroup getGroup0()
        {
            return group0;
        }
        public LinkGroup getGroup1()
        {
            return group1;
        }

        public void removeLinkGroup(int index)
        {
            if(index == 0)
                window.MenuLinkGroups.Remove(group0);
            else
                window.MenuLinkGroups.Remove(group1);
        }

        public void addLinkGroup(int index)
        {
            if (index == 0)
            {
                window.MenuLinkGroups.Add(group0);
            }
            else
            {
                window.MenuLinkGroups.Add(group1);
            }
        }
    }
}