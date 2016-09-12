using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Keyboard;

namespace VolumeController
{
    static class Program
    {
        public static SystemStartup Startup;
        public static AudioSessionManager AudioManager;
        public static SystemTrayIcon TrayIcon;
        public static VolumeDisplayForm VolumeStatusWindow;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Startup = new SystemStartup();

                TrayIcon = new SystemTrayIcon(Properties.Resources.TrayIcon);
                TrayIcon.ContextMenu.Add("VolumeController", null, null);
                TrayIcon.ContextMenu.Add("-", null, null);
                TrayIcon.ContextMenu.Add("Run at Startup", null, TrayIcon_RunAtStartup).Checked = Startup.IsRegistered;
                TrayIcon.ContextMenu.Add("Quit", null, TrayIcon_Quit);
                TrayIcon.Start();

                AudioManager = new AudioSessionManager();
                AudioManager.GetSessionManager(AudioManager.GetDefaultDevice());

                VolumeStatusWindow = new VolumeDisplayForm();

                KeyboardHotKey hotkey = new KeyboardHotKey(null);
                hotkey.OnKeyboardEvent += KeyboardHook_OnKeyboardEvent;
                hotkey.RegisterKey(Keys.VolumeUp);
                hotkey.RegisterKey(Keys.VolumeDown);
                hotkey.RegisterKey(Keys.VolumeMute);
                
                hotkey.Start();

                Application.Run();

                hotkey.Stop();
                TrayIcon.Stop();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            MessageBox.Show("AppExit");
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
            Console.WriteLine(pid);
            return pid;
        }

        private static bool KeyboardHook_OnKeyboardEvent(KeyAction Action, Keys Key)
        {
            switch(Key)
            {
                case Keys.VolumeMute:
                    if (Action == KeyAction.WM_KEYUP)
                    {
                        AudioSessionControl session = AudioSession_GetCurrentForeground();
                        AudioSession_MuteToggle(session);
                    }                        
                    return true;

                case Keys.VolumeUp:

                    if (Action == KeyAction.WM_KEYUP)
                    {
                        AudioSessionControl session = AudioSession_GetCurrentForeground();
                        AudioSession_Increaser(session );
                    }
                    return true;

                case Keys.VolumeDown:
                    if (Action == KeyAction.WM_KEYUP)
                    {
                        AudioSessionControl session = AudioSession_GetCurrentForeground();
                        
                        AudioSession_Decreaser(session);
                    }
                    return true;
            }
            
            //Console.WriteLine(Key);
            return false;
        }

        private static AudioSessionControl AudioSession_GetCurrentForeground()
        {
            if (VolumeStatusWindow.CurrentSession == null)
            {
                VolumeStatusWindow.HostProcessId = GetCurrentWindowProcessId();
                VolumeStatusWindow.CurrentSession = AudioManager.FindSession(
                    AudioManager.GetSessionManager(AudioManager.GetDefaultDevice()),
                    AudioMatch_Session,
                    VolumeStatusWindow.HostProcessId
                    );
            }
            return (AudioSessionControl) VolumeStatusWindow.CurrentSession;
        }

        private static bool AudioMatch_Session(AudioSessionControl session, object data)
        {
            if (session.PID != (uint)data)
                return true;
            return false;
        }

        private static bool AudioSession_MuteToggle(AudioSessionControl session)
        {
            if (session == null)
                return false;

            bool Muted = !session.IsMute;
            session.SetMute(Muted);

            VolumeStatusWindow.Status = Muted ? VolumeDisplayForm.DisplayStatus.Muted : VolumeDisplayForm.DisplayStatus.Normal;
            VolumeStatusWindow.Toast();
            return true;
        }

        private static bool AudioSession_Increaser(AudioSessionControl session)
        {
            if (session == null)
                return false;

            float Value = session.CurrentVolume + 0.01f;
            if (Value > 1)
                Value = 1;
            session.SetVolume(Value);
            VolumeStatusWindow.Value = Value * 100f;
            VolumeStatusWindow.Toast();
            return true;
        }

        private static bool AudioSession_Decreaser(AudioSessionControl session)
        {
            if (session == null)
                return false;
            float Value = session.CurrentVolume - 0.01f;
            if (Value < 0)
                Value = 0;
            session.SetVolume(Value);

            VolumeStatusWindow.Value = Value * 100f;
            VolumeStatusWindow.Toast();
            return true;
        }
    }
}
