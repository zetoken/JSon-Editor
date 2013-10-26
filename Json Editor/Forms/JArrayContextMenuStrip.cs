using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZTn.Json.Editor.Forms
{
    class JArrayContextMenuStrip : JTokenContextMenuStrip
    {
        ToolStripMenuItem arrayToolStripItem;
        ToolStripMenuItem addArrayToolStripItem;
        ToolStripMenuItem addObjectToolStripItem;
        ToolStripMenuItem addValueToolStripItem;

        #region >> Constructors

        public JArrayContextMenuStrip()
            : base()
        {
            arrayToolStripItem = new ToolStripMenuItem("Json Array");
            addArrayToolStripItem = new ToolStripMenuItem("Add Array", null, AddArray_Click);
            addObjectToolStripItem = new ToolStripMenuItem("Add Object", null, AddObject_Click);
            addValueToolStripItem = new ToolStripMenuItem("Add Value", null, AddValue_Click);

            arrayToolStripItem.DropDownItems.Add(addArrayToolStripItem);
            arrayToolStripItem.DropDownItems.Add(addObjectToolStripItem);
            arrayToolStripItem.DropDownItems.Add(addValueToolStripItem);
            Items.Add(arrayToolStripItem);
        }

        #endregion

        /// <summary>
        /// Click event handler for <see cref="addValueToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddArray_Click(Object sender, EventArgs e)
        {
            AddJToken(JArray.Parse("[]"));
        }

        /// <summary>
        /// Click event handler for <see cref="addValueToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddObject_Click(Object sender, EventArgs e)
        {
            AddJToken(JObject.Parse("{}"));
        }

        /// <summary>
        /// Click event handler for <see cref="addValueToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddValue_Click(Object sender, EventArgs e)
        {
            AddJToken(JValue.Parse("null"));
        }

        /// <summary>
        /// Add a new <see cref="JToken"/> instance in current <see cref="JArrayTreeNode"/>
        /// </summary>
        /// <param name="newJToken"></param>
        private void AddJToken(JToken newJToken)
        {
            JArrayTreeNode jArrayTreeNode = jTokenTreeNode as JArrayTreeNode;

            if (jArrayTreeNode == null)
            {
                return;
            }

            jArrayTreeNode.jArrayTag.AddFirst(newJToken);

            TreeNode newTreeNode = JsonTreeNodeFactory.Create(newJToken);
            jArrayTreeNode.Nodes.Insert(0, newTreeNode);

            jArrayTreeNode.TreeView.SelectedNode = newTreeNode;
        }
    }
}
