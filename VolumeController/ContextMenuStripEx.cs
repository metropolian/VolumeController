using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VolumeController
{
    class ContextMenuStripEx : ContextMenuStrip
    {
        public ContextMenuStripEx()
        {
        }

        public ToolStripMenuItem Add(string text, Image icon, EventHandler onClick)
        {
            if (text == "-")
            {
                this.Items.Add(new ToolStripSeparator());
                return null;
            }

            ToolStripMenuItem res = new ToolStripMenuItem(text, icon, onClick);
            this.Items.Add(res);
            return res;
        }
    }
}
