using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZTn.Json.Editor.Forms
{
    sealed class JObjectContextMenuStrip : JTokenContextMenuStrip
    {
        ToolStripItem addPropertyToolStripItem = new ToolStripMenuItem("Add Property", null, AddProperty_Click);

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JObjectContextMenuStrip"/> class.
        /// </summary>
        public JObjectContextMenuStrip() :
            base()
        {
            Items.Add(new ToolStripSeparator());
            Items.Add(addPropertyToolStripItem);
        }

        #endregion

        private static void AddProperty_Click(Object sender, EventArgs e)
        {
            JObjectTreeNode jObjectTreeNode = GetSourceTreeNode(sender as ToolStripItem) as JObjectTreeNode;
            if (jObjectTreeNode == null)
            {
                return;
            }

            JProperty newJProperty = new JProperty("new" + DateTime.Now.Ticks, "v");
            jObjectTreeNode.JObjectTag.AddFirst(newJProperty);

            JPropertyTreeNode jPropertyTreeNode = (JPropertyTreeNode)JsonTreeNodeFactory.Create(newJProperty);
            jObjectTreeNode.Nodes.Insert(0, jPropertyTreeNode);

            jObjectTreeNode.TreeView.SelectedNode = jPropertyTreeNode;
        }
    }
}
