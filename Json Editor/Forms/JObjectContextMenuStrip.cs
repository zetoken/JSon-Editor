using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;
using ZTn.Json.Editor.Properties;

namespace ZTn.Json.Editor.Forms
{
    class JObjectContextMenuStrip : JTokenContextMenuStrip
    {
        protected ToolStripMenuItem ObjectToolStripItem;
        protected ToolStripMenuItem InsertPropertyToolStripItem;

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JObjectContextMenuStrip"/> class.
        /// </summary>
        public JObjectContextMenuStrip()
        {
            ObjectToolStripItem = new ToolStripMenuItem(Resources.JsonObject);
            InsertPropertyToolStripItem = new ToolStripMenuItem(Resources.InsertProperty, null, InsertProperty_Click);

            ObjectToolStripItem.DropDownItems.Add(InsertPropertyToolStripItem);
            Items.Add(ObjectToolStripItem);
        }

        #endregion

        /// <summary>
        /// Click event handler for <see cref="InsertPropertyToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertProperty_Click(Object sender, EventArgs e)
        {
            var jObjectTreeNode = JTokenNode as JObjectTreeNode;

            if (jObjectTreeNode == null)
            {
                return;
            }

            var newJProperty = new JProperty("name" + DateTime.Now.Ticks, "v");
            jObjectTreeNode.JObjectTag.AddFirst(newJProperty);

            var jPropertyTreeNode = (JPropertyTreeNode)JsonTreeNodeFactory.Create(newJProperty);
            jObjectTreeNode.Nodes.Insert(0, jPropertyTreeNode);

            jObjectTreeNode.TreeView.SelectedNode = jPropertyTreeNode;
        }
    }
}
