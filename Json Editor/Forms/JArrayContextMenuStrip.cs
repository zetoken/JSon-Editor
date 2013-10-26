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
        ToolStripMenuItem insertArrayToolStripItem;
        ToolStripMenuItem insertObjectToolStripItem;
        ToolStripMenuItem insertValueToolStripItem;

        #region >> Constructors

        public JArrayContextMenuStrip()
            : base()
        {
            arrayToolStripItem = new ToolStripMenuItem("Json Array");
            insertArrayToolStripItem = new ToolStripMenuItem("Insert Array", null, InsertArray_Click);
            insertObjectToolStripItem = new ToolStripMenuItem("Insert Object", null, InsertObject_Click);
            insertValueToolStripItem = new ToolStripMenuItem("Insert Value", null, InsertValue_Click);

            arrayToolStripItem.DropDownItems.Add(insertArrayToolStripItem);
            arrayToolStripItem.DropDownItems.Add(insertObjectToolStripItem);
            arrayToolStripItem.DropDownItems.Add(insertValueToolStripItem);
            Items.Add(arrayToolStripItem);
        }

        #endregion

        /// <summary>
        /// Click event handler for <see cref="insertValueToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertArray_Click(Object sender, EventArgs e)
        {
            InsertJToken(JArray.Parse("[]"));
        }

        /// <summary>
        /// Click event handler for <see cref="insertValueToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertObject_Click(Object sender, EventArgs e)
        {
            InsertJToken(JObject.Parse("{}"));
        }

        /// <summary>
        /// Click event handler for <see cref="insertValueToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertValue_Click(Object sender, EventArgs e)
        {
            InsertJToken(JValue.Parse("null"));
        }

        /// <summary>
        /// Add a new <see cref="JToken"/> instance in current <see cref="JArrayTreeNode"/>
        /// </summary>
        /// <param name="newJToken"></param>
        private void InsertJToken(JToken newJToken)
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
