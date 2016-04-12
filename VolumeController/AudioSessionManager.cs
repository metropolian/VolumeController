using System;
using Vannatech.CoreAudio.Interfaces;
using Vannatech.CoreAudio.Constants;
using Vannatech.CoreAudio.Externals;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace VolumeController
{
    class AudioSessionManager
    {
        private IMMDeviceEnumerator device_enumerator;
        //private IMMDevice device;

        public AudioSessionManager()
        {
            device_enumerator = Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(ComCLSIDs.MMDeviceEnumeratorCLSID))) as IMMDeviceEnumerator;
        }

        public IMMDevice GetDefaultDevice()
        {
            IMMDevice device = null;
            device_enumerator.GetDefaultAudioEndpoint(
                Vannatech.CoreAudio.Enumerations.EDataFlow.eRender,
                Vannatech.CoreAudio.Enumerations.ERole.eMultimedia, out device);
            return device;
        }

        public string GetDeviceId(IMMDevice device)
        {
            string device_id = "";
            device.GetId(out device_id);
            return device_id;
        }

        public string GetDeviceName(IMMDevice device)
        {
            IPropertyStore device_prop;
            device.OpenPropertyStore(0, out device_prop);
            PROPERTYKEY prop_key = new PROPERTYKEY();
            prop_key.pid = 2;
            prop_key.fmtid = PropertyKeys.PKEY_DeviceInterface_FriendlyName;
            PROPVARIANT prop_value;
            device_prop.GetValue(ref prop_key, out prop_value);
            string device_name = Marshal.PtrToStringAuto(prop_value.Data.AsStringPtr);
            return device_name;
        }

        public delegate bool AudioSessionProc(AudioSessionControl session, object data);

        public IAudioSessionManager2 GetSessionManager(IMMDevice device)
        {
            Guid iid = Guid.Empty;
            object session_manager_object = null;
            IAudioSessionManager2 session_manager;
            iid = new Guid(ComIIDs.IAudioSessionManager2IID);
            device.Activate(iid, (uint)CLSCTX.CLSCTX_INPROC_SERVER, IntPtr.Zero, out session_manager_object);
            session_manager = session_manager_object as IAudioSessionManager2;
            return session_manager;
        }

        public int EnumSessions(IAudioSessionManager2 session_manager, AudioSessionProc session_proc, object data)
        {   
            IAudioSessionEnumerator sessionList = null;
            session_manager.GetSessionEnumerator(out sessionList);
            if (sessionList == null)
                return 0;
            
            int cnt = 0;
            sessionList.GetCount(out cnt);

            for (int index = 0; index < cnt; index++)
            {
                IAudioSessionControl session = null;
                sessionList.GetSession(index, out session);
                if (session == null)
                    continue;

                bool quit = false;
                AudioSessionControl control = null;
                try
                {
                    control = new AudioSessionControl(session);
                    quit = (session_proc(control, data) == false);
                }
                catch { }
                finally
                {
                    control.Dispose();
                }

                Marshal.Release(Marshal.GetIUnknownForObject(session));
                if (quit)
                    break;
            }
            GC.WaitForPendingFinalizers();
            return cnt;
        }
    }

    class AudioSessionControl : IDisposable
    {
        private IAudioSessionControl session;
        private IAudioSessionControl2 session2;
        private ISimpleAudioVolume volume;

        public AudioSessionControl(object inp)
        {
            session = inp as IAudioSessionControl;
            session2 = inp as IAudioSessionControl2;
            volume = session as ISimpleAudioVolume;
        }

        public string DisplayName
        {
            get
            {
                string res = "";
                if (session == null)
                    return res;

                session.GetDisplayName(out res);
                if (res == "")
                {
                    if (session2 == null)
                        return res;

                    uint pid = 0;
                    session2.GetProcessId(out pid);
                    Process proc = Process.GetProcessById((int)pid);
                    res = proc.MainWindowTitle;
                }
                return res;
            }
        }

        public uint PID
        {
            get
            {
                if (session2 == null)
                    return 0;
                uint pid = 0;
                session2.GetProcessId(out pid);
                return pid;
            }
        }

        public bool SetVolume(float Value)
        {
            if (volume == null)
                return false;            
            return volume.SetMasterVolume(Value, Guid.Empty) == 0;
        }

        public bool SetMute(bool Value)
        {
            if (volume == null)
                return false;
            return volume.SetMute(Value, Guid.Empty) == 0;
        }

        public void Dispose()
        {
            Marshal.Release(Marshal.GetIUnknownForObject(session));
        }

        public float CurrentVolume
        {
            get
            {
                if (volume == null)
                    return 0;
                float res = 0;
                volume.GetMasterVolume(out res);
                return res;
            }
        }

        public bool IsMute
        {
            get
            {
                if (volume == null)
                    return false;

                bool res = false;
                volume.GetMute(out res);
                return res;
            }
        }
    }
}
