using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using PYO2016_Client.Sources.Capture;
using System.Threading;
using PYO2016_Client.Pages;

namespace PYO2016_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.ComponentModel.IContainer components;
        private KeyHooker hooker;


        public MainWindow()
        {
            InitializeComponent();
            this.components = new System.ComponentModel.Container();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            ////notifyIcon1.Icon = new Icon("PYO_LOGO.ico");
            ////notifyIcon1.Visible = true;
            //hooker = new KeyHooker();
            //hookThread = new Thread(hooker.SetHook);
            //hookThread.Start();
            hooker = new KeyHooker();
            hooker.SetHook();
        }
        private void SetBalloonTip()
        {
            notifyIcon1.Icon = SystemIcons.Exclamation;
            notifyIcon1.BalloonTipTitle = "Balloon Tip Title";
            notifyIcon1.BalloonTipText = "Balloon Tip Text.";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
            //this.Click += new EventHandler(Form1_Click);
        }
    }
}
