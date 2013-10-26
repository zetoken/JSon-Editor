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
        ToolStripMenuItem insertPropertyToolStripItem;

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JObjectContextMenuStrip"/> class.
        /// </summary>
        public JObjectContextMenuStrip() :
            base()
        {
            objectToolStripItem = new ToolStripMenuItem("Json Object");
            insertPropertyToolStripItem = new ToolStripMenuItem("Insert Property", null, InsertProperty_Click);

            objectToolStripItem.DropDownItems.Add(insertPropertyToolStripItem);
            Items.Add(objectToolStripItem);
        }

        #endregion

        /// <summary>
        /// Click event handler for <see cref="insertPropertyToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertProperty_Click(Object sender, EventArgs e)
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
