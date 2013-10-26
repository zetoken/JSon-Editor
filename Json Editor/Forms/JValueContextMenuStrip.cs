using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTn.Json.Editor.Forms
{
    class JValueContextMenuStrip:JTokenContextMenuStrip
    {
        public JValueContextMenuStrip()
            : base()
        {
            removeNodeToolStripItem.Visible = false;
        }
    }
}
