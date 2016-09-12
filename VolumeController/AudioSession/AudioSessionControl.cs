using System;
using Vannatech.CoreAudio.Interfaces;
using Vannatech.CoreAudio.Constants;
using Vannatech.CoreAudio.Externals;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace VolumeController
{
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
