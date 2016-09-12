using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VolumeController
{
    public partial class VolumeDisplayForm : Form
    {
        public enum DisplayStatus
        {
            Normal = 1,
            Muted
        }

        public Timer Toaster;

        public int HideMillis = 5000;

        private DateTime StartDateTime;
        private float VolumeValue;

        public float Value
        {
            set
            {
                VolumeValue = value;
                LbValue.Text = String.Format("{0:0}", VolumeValue);
            }

            get { return VolumeValue;  }
        }

        public DisplayStatus Status;

        public uint HostProcessId = 0;
        public object CurrentSession = null;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams baseParams = base.CreateParams;

                const int WS_EX_NOACTIVATE = 0x08000000;
                const int WS_EX_TOOLWINDOW = 0x00000080;
                baseParams.ExStyle |= (int)(WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);
                Opacity = 0.75f;
                return baseParams;
            }
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }


        public VolumeDisplayForm()
        {
            InitializeComponent();
        }

        private void VolumeDisplay_Shown(object sender, EventArgs e)
        {

        }

        private void Toaster_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - StartDateTime).TotalMilliseconds > HideMillis)
            {
                Hide();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }


        public void Toast()
        {
            StartDateTime = DateTime.Now;
            this.Left = Screen.PrimaryScreen.Bounds.Left + 60;
            this.Top = Screen.PrimaryScreen.Bounds.Top + 60;

            if (!this.Visible)
            {

                Toaster = new Timer();
                Toaster.Interval = 1000;
                Toaster.Tick += Toaster_Tick;
                Toaster.Start();

                Show();
            }

            try
            {
                Process HostProcess = Process.GetProcessById((int)HostProcessId);
                ImIcon.Image = Icon.ExtractAssociatedIcon(HostProcess.MainModule.FileName).ToBitmap();
            }
            catch
            {
                ImIcon.Image = null;
            }

            //BringToFront();
            Invalidate();
        }



        public new void Hide()
        {
            CurrentSession = null;
            HostProcessId = 0;

            Toaster.Stop();
            base.Hide();
        }

        private void VolumeDisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        /*
        private void VolumeDisplayForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics Gfx = e.Graphics;

            StringFormat Format = new StringFormat();
            Format.LineAlignment = StringAlignment.Center;
            Format.Alignment = StringAlignment.Center;

            RectangleF Bounds = this.ClientRectangle;

            Font CurFont = new Font("Terminal", 72);
            Color TextColor = Color.Lime;
            if (Status == DisplayStatus.Muted)
                TextColor = Color.Red;
            
            Brush BrushColor = new SolidBrush(TextColor);
            Gfx.DrawString(String.Format("{0:0}", Value), CurFont, BrushColor, Bounds, Format);

            BrushColor.Dispose();
            CurFont.Dispose();

        } */
    }    
}
