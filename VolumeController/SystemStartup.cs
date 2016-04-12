using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeController
{
    class SystemStartup
    {
        public string StartupRoot = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        public string AppName = "";
        public string AppExecutable = "";

        public SystemStartup()
        {
            Process current = Process.GetCurrentProcess();
            AppName = current.ProcessName;
            AppExecutable = current.MainModule.FileName;
        }

        public SystemStartup(string name, string exe)
        {
            AppName = name;
            AppExecutable = exe;
        }

        public bool IsRegistered
        {
            get
            {
                object res = RegistryStartupKey.GetValue(AppName);
                return res != null;

            }
        }

        RegistryKey RegistryStartupKey
        {
            get
            {
                RegistryKey registry = Registry.CurrentUser.OpenSubKey(StartupRoot, true);
                return registry;
            }
        }

        public bool Register()
        {
            if (AppName == "")
                return false;

            RegistryStartupKey.SetValue(AppName, AppExecutable, RegistryValueKind.String);
            return true;
        }

        public bool Unregister()
        {
            if (AppName == "")
                return false;

            RegistryStartupKey.DeleteValue(AppName);
            return true;
        }
    }
}
