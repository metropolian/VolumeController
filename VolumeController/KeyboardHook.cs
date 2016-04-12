using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VolumeController
{

    class KeyboardHook
    {

        private const int WH_KEYBOARD_LL = 13;

        public enum KeyAction
        {
            None = 0,
            WM_KEYDOWN = 0x0100,
            WM_KEYUP = 0x0101
        }

        public delegate bool KeyboardHookEvent(KeyAction Action, KBDLLHOOKSTRUCT Data );
        public static event KeyboardHookEvent OnKeyboardEvent;

        private static IntPtr HHook = IntPtr.Zero;
        public static IntPtr Start()
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                HHook = SetWindowsHookEx(WH_KEYBOARD_LL, HookCallback, GetModuleHandle(curModule.ModuleName), 0);
            }
            return HHook;
        }

        public static void Stop()
        {
            UnhookWindowsHookEx(HHook);
        }


        private delegate IntPtr LowLevelKeyboardProc(int nCode, int wParam, int lParam);

        private static IntPtr HookCallback(int nCode, int wParam, int lParam)
        {
            if ((nCode >= 0) && (OnKeyboardEvent != null))
            {
                try
                {
                    KBDLLHOOKSTRUCT Data = new KBDLLHOOKSTRUCT();
                    Marshal.PtrToStructure((IntPtr)lParam, Data);
                    if (OnKeyboardEvent((KeyAction)wParam, Data))
                        return (IntPtr)1;
                }
                catch
                {
                }
            }

            return CallNextHookEx(HHook, nCode, wParam, lParam);

        }

        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [Flags]
        public enum KBDLLHOOKSTRUCTFlags : uint
        {
            LLKHF_EXTENDED = 0x01,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80,
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            int wParam, int lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

    }
}

