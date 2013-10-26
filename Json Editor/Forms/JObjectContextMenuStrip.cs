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
        ToolStripMenuItem objectToolStripItem;
        ToolStripMenuItem addPropertyToolStripItem;

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JObjectContextMenuStrip"/> class.
        /// </summary>
        public JObjectContextMenuStrip() :
            base()
        {
            objectToolStripItem = new ToolStripMenuItem("Json Object");
            addPropertyToolStripItem = new ToolStripMenuItem("Add Property", null, AddProperty_Click);

            objectToolStripItem.DropDownItems.Add(addPropertyToolStripItem);
            Items.Add(objectToolStripItem);
        }

        #endregion

        /// <summary>
        /// Click event handler for <see cref="addPropertyToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddProperty_Click(Object sender, EventArgs e)
        {
            JObjectTreeNode jObjectTreeNode = jTokenTreeNode as JObjectTreeNode;

            if (jObjectTreeNode == null)
            {
                return;
            }

            JProperty newJProperty = new JProperty("name" + DateTime.Now.Ticks, "v");
            jObjectTreeNode.JObjectTag.AddFirst(newJProperty);

            JPropertyTreeNode jPropertyTreeNode = (JPropertyTreeNode)JsonTreeNodeFactory.Create(newJProperty);
            jObjectTreeNode.Nodes.Insert(0, jPropertyTreeNode);

            jObjectTreeNode.TreeView.SelectedNode = jPropertyTreeNode;
        }
    }
}
