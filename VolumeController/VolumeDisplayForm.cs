using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VolumeController
{
    public partial class VolumeDisplayForm : Form
    {
        public Timer Toaster;

        public int HideMillis = 5000;

        private DateTime StartDateTime;
        public float Value;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams baseParams = base.CreateParams;

                const int WS_EX_NOACTIVATE = 0x08000000;
                const int WS_EX_TOOLWINDOW = 0x00000080;
                baseParams.ExStyle |= (int)(WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);

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
            //BringToFront();
            Invalidate();
        }



        public new void Hide()
        {
            Toaster.Stop();
            base.Hide();
        }

        private void VolumeDisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void VolumeDisplayForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics Gfx = e.Graphics;

            StringFormat Format = new StringFormat();
            Format.LineAlignment = StringAlignment.Center;
            Format.Alignment = StringAlignment.Center;

            RectangleF Bounds = this.ClientRectangle;

            Font CurFont = new Font("Terminal", 72);
            Brush BrushColor = new SolidBrush(Color.Lime);            
            Gfx.DrawString(String.Format("{0:0}", Value), CurFont, BrushColor, Bounds, Format);

            CurFont.Dispose();
            BrushColor.Dispose();

        }
    }    
}
