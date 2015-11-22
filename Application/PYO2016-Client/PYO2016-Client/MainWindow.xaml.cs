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

namespace PYO2016_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.ComponentModel.IContainer components;
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        private static bool printScreenKeyState = false;
        private static bool shiftKeyState = false;
        private static Object keyLock = new Object();

        private LowLevelKeyboardProc _proc = hookProc;

        private static IntPtr hhook = IntPtr.Zero;

        public void SetHook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
        }

        public static void UnHook()
        {
            UnhookWindowsHookEx(hhook);
        }

        //Hook process
        public static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            lock (keyLock)
            {
                if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN && vkCode == 44)
                {
                    if (!printScreenKeyState)
                    {
                        printScreenKeyState = true;
                        if (shiftKeyState)
                        {
                            CaptureTool.getInstance().capture("C:\\Users\\KGWANGMIN\\Documents");
                            System.Windows.MessageBox.Show("Capture Activated");
                        }
                    }
                    return (IntPtr)1;
                }
                else if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN && vkCode == 160)
                {
                    if (!shiftKeyState)
                    {
                        shiftKeyState = true;
                        if (printScreenKeyState)
                        {
                            CaptureTool.getInstance().capture("C:\\");
                            System.Windows.MessageBox.Show("Capture Activated");
                        }
                    }
                    return (IntPtr)1;
                }
                else if (code >= 0 && wParam == (IntPtr)WM_KEYUP && vkCode == 44)
                {
                    if (printScreenKeyState)
                        printScreenKeyState = false;
                    return (IntPtr)1;
                }
                else if (code >= 0 && wParam == (IntPtr)WM_KEYUP && vkCode == 160)
                {
                    if (shiftKeyState)
                        shiftKeyState = false;
                    return (IntPtr)1;
                }
                else
                    return CallNextHookEx(hhook, code, (int)wParam, lParam);
            }
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UnHook();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetHook();
        }





        public MainWindow()
        {
            InitializeComponent();
            this.components = new System.ComponentModel.Container();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            notifyIcon1.Icon = new Icon("PYO_LOGO.ico");
            notifyIcon1.Visible = true;
            SetHook();
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
