using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VolumeController
{
    class SystemTrayIcon 
    {
        public NotifyIcon TrayIcon;
        public event EventHandler Click;
        public event EventHandler MenuOpening;
        public event ToolStripItemClickedEventHandler ClickMenuItem;
        public ContextMenuStripEx ContextMenu;

        public SystemTrayIcon(Icon icon)
        {
            TrayIcon = new NotifyIcon();
            TrayIcon.Icon = icon;
            TrayIcon.Click += TrayIcon_Click;

            ContextMenu = new ContextMenuStripEx();
            ContextMenu.ItemClicked += ContextMenu_ItemClicked;
            TrayIcon.ContextMenuStrip = ContextMenu;
            
        }

        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (ClickMenuItem != null)
                ClickMenuItem(sender, e);


        }

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            if (Click != null)
                Click(sender, e);
            else
            {
                if (MenuOpening != null)
                    MenuOpening(sender, e);
                //TrayIcon.ContextMenuStrip.Show(Control.MousePosition);
            }
        }

        public void Stop()
        {
            TrayIcon.Visible = false;
        }

        public Icon Icon
        {
            get { return TrayIcon.Icon; }
            set {TrayIcon.Icon = value; }
        }

        public void Start()
        {
            TrayIcon.Visible = true;
        }

    }
}
