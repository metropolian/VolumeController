using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vannatech.CoreAudio.Interfaces;
using Vannatech.CoreAudio.Constants;
using Vannatech.CoreAudio.Externals;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Media;

namespace VolumeController
{
    static class Program
    {
        public static SystemStartup Startup;
        public static AudioSessionManager AudioManager;
        public static SystemTrayIcon TrayIcon;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Startup = new SystemStartup();
            
            TrayIcon = new SystemTrayIcon(Properties.Resources.TrayIcon);
            TrayIcon.ContextMenu.Add("Run at Startup", null, TrayIcon_RunAtStartup).Checked = Startup.IsRegistered;
            TrayIcon.ContextMenu.Add("Quit", null, TrayIcon_Quit);
            TrayIcon.Start();

            AudioManager = new AudioSessionManager();
            AudioManager.GetSessionManager(AudioManager.GetDefaultDevice());

            KeyboardHook.Start();
            KeyboardHook.OnKeyboardEvent += KeyboardHook_OnKeyboardEvent;
            
            Application.Run();

            KeyboardHook.Stop();
            TrayIcon.Stop();
        }

        private static void TrayIcon_RunAtStartup(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            if (Startup.IsRegistered)
                Startup.Unregister();
            else
                Startup.Register();

            item.Checked = Startup.IsRegistered;
        }

        private static void TrayIcon_Quit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        static uint GetCurrentWindowProcessId()
        {
            uint pid = 0;
            IntPtr handle = GetForegroundWindow();
            GetWindowThreadProcessId(handle, out pid);
            return pid;
        }

        private static bool KeyboardHook_OnKeyboardEvent(KeyboardHook.KeyAction Action, KeyboardHook.KBDLLHOOKSTRUCT Data)
        {
            Keys Key = (Keys)Data.vkCode;

            switch(Key)
            {
                case Keys.VolumeMute:
                    if (Action == KeyboardHook.KeyAction.WM_KEYUP)
                    {                        
                        AudioManager.EnumSessions(
                            AudioManager.GetSessionManager(AudioManager.GetDefaultDevice()),
                            AudioSession_Muter,
                            GetCurrentWindowProcessId());
                    }                        
                    return true;

                case Keys.VolumeUp:

                    if (Action == KeyboardHook.KeyAction.WM_KEYUP)
                    {
                        AudioManager.EnumSessions(
                            AudioManager.GetSessionManager(AudioManager.GetDefaultDevice()),
                            AudioSession_Increaser,
                            GetCurrentWindowProcessId());
                    }
                    return true;

                case Keys.VolumeDown:
                    if (Action == KeyboardHook.KeyAction.WM_KEYUP)
                    {
                        AudioManager.EnumSessions(
                            AudioManager.GetSessionManager(AudioManager.GetDefaultDevice()),
                            AudioSession_Decreaser,
                            GetCurrentWindowProcessId());
                    }
                    return true;
            }
            
            Console.WriteLine(Key);
            return false;
        }

        private static bool AudioSession_Muter(AudioSessionControl session, object data)
        {
            if (session.PID != (uint)data)
                return true;

            session.SetMute(!session.IsMute);
            return false;
        }

        private static bool AudioSession_Increaser(AudioSessionControl session, object data)
        {
            if (session.PID != (uint)data)
                return true;
            float Value = session.CurrentVolume + 0.05f;
            if (Value > 1)
                Value = 1;
            session.SetVolume(Value);
            return false;
        }

        private static bool AudioSession_Decreaser(AudioSessionControl session, object data)
        {
            if (session.PID != (uint)data)
                return true;
            float Value = session.CurrentVolume - 0.05f;
            if (Value < 0)
                Value = 0;
            session.SetVolume(Value);
            return false;
        }
    }
}
