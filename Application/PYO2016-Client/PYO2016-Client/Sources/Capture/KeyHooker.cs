using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PYO2016_Client.Sources.Capture
{
    class KeyHooker
    {
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
                            return (IntPtr)1;
                        }
                    }
                }
                else if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN && vkCode == 160)
                {
                    if (!shiftKeyState)
                    {
                        shiftKeyState = true;
                        if (printScreenKeyState)
                        {
                            CaptureTool.getInstance().capture("C:\\Users\\KGWANGMIN\\Documents");
                            return (IntPtr)1;
                        }
                    }
                }
                else if (code >= 0 && wParam == (IntPtr)WM_KEYUP && vkCode == 44)
                {
                    if (printScreenKeyState)
                        printScreenKeyState = false;
                }
                else if (code >= 0 && wParam == (IntPtr)WM_KEYUP && vkCode == 160)
                {
                    if (shiftKeyState)
                        shiftKeyState = false;
                }
            }            
            return CallNextHookEx(hhook, code, (int)wParam, lParam);
        }
    }
}
