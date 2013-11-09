using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZTn.Json.Editor.Forms
{
    public partial class TreeViewWithoutTooltips : TreeView
    {
        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public TreeViewWithoutTooltips()
            : base()
        {
            InitializeComponent();
        }

        #endregion

        #region >> TreeView

        /// <inheritdoc />
        /// <remarks>
        /// Style change disabling automatic creation of tooltip on each node of the TreeView (no other C# way of doing this).
        /// </remarks>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.Style |= 0x80;    // Turn on TVS_NOTOOLTIPS
                return cp;
            }
        }

        #endregion
    }
}
