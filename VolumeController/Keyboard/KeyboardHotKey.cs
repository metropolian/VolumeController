using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace System.Windows.Keyboard
{
    class KeyboardHotKey : IMessageFilter
    {
        public delegate bool KeyboardHookEvent(KeyAction Action, Keys Key);
        public event KeyboardHook.KeyboardHookEvent OnKeyboardEvent;

        #region fields
        public static int MOD_ALT = 0x1;
        public static int MOD_CONTROL = 0x2;
        public static int MOD_SHIFT = 0x4;
        public static int MOD_WIN = 0x8;
        public static int WM_HOTKEY = 0x312;
        #endregion

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private IntPtr Handle = IntPtr.Zero;
        private int Hash_Id;
        private List<Keys> RegisteredKeys;

        public KeyboardHotKey(Form host)
        {
            if (host != null)
            this.Handle = host.Handle;
            RegisteredKeys = new List<Keys>();
        }


        public void RegisterKey(Keys key)
        {
            int modifiers = 0;

            if ((key & Keys.Alt) == Keys.Alt)
                modifiers = modifiers | MOD_ALT;

            if ((key & Keys.Control) == Keys.Control)
                modifiers = modifiers | MOD_CONTROL;

            if ((key & Keys.Shift) == Keys.Shift)
                modifiers = modifiers | MOD_SHIFT;

            Keys vk = key & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;
            Hash_Id = this.GetHashCode(); 
            
            RegisterHotKey((IntPtr)IntPtr.Zero, Hash_Id, modifiers, (uint)vk);
        }

        public void Start()
        {
            Hash_Id = this.GetHashCode();
            Application.AddMessageFilter(this);
        }


        public void Stop()
        {
            Application.RemoveMessageFilter(this);
            UnregisterHotKey(this.Handle, Hash_Id);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                int vk = m.LParam.ToInt32() >> 16;
                OnKeyboardEvent(KeyAction.WM_KEYDOWN, (Keys)vk);
                OnKeyboardEvent(KeyAction.WM_KEYUP, (Keys)vk);
                return true;
            }
            return false;
        }
    }
}
