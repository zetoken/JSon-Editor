using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZTn.Json.Editor.Forms
{
    class JTokenContextMenuStrip : ContextMenuStrip
    {
        /// <summary>
        /// Source <see cref="TreeNode"/> at the origin of this <see cref="ContextMenuStrip"/>
        /// </summary>
        protected JTokenTreeNode jTokenTreeNode;

        protected ToolStripItem collapseAllToolStripItem;
        protected ToolStripItem expandAllToolStripItem;
        protected ToolStripItem removeNodeToolStripItem;

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JTokenContextMenuStrip"/> class.
        /// </summary>
        public JTokenContextMenuStrip()
            : base()
        {
            collapseAllToolStripItem = new ToolStripMenuItem("Collapse All", null, CollapseAll_Click);
            expandAllToolStripItem = new ToolStripMenuItem("Expand All", null, ExpandAll_Click);
            removeNodeToolStripItem = new ToolStripMenuItem("Remove All", null, RemoveNode_Click);

            Items.Add(collapseAllToolStripItem);
            Items.Add(expandAllToolStripItem);
            Items.Add(removeNodeToolStripItem);
        }

        #endregion

        #region >> ContextMenuStrip

        /// <inheritdoc />
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                jTokenTreeNode = GetSourceTreeNode() as JTokenTreeNode;

                collapseAllToolStripItem.Visible = jTokenTreeNode.IsExpanded && jTokenTreeNode.JTokenTag.HasValues;
                expandAllToolStripItem.Visible = !jTokenTreeNode.IsExpanded && jTokenTreeNode.JTokenTag.HasValues;
                removeNodeToolStripItem.Visible = (jTokenTreeNode.Parent != null) && !(jTokenTreeNode.Parent is JPropertyTreeNode);
            }

            base.OnVisibleChanged(e);
        }

        #endregion

        /// <summary>
        /// Click event handler for <see cref="collapseAllToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CollapseAll_Click(Object sender, EventArgs e)
        {
            if (jTokenTreeNode != null)
            {
                jTokenTreeNode.TreeView.BeginUpdate();
                jTokenTreeNode.Collapse(false);
                jTokenTreeNode.TreeView.EndUpdate();
            }
        }

        /// <summary>
        /// Click event handler for <see cref="expandAllToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExpandAll_Click(Object sender, EventArgs e)
        {
            if (jTokenTreeNode != null)
            {
                jTokenTreeNode.TreeView.BeginUpdate();
                jTokenTreeNode.ExpandAll();
                jTokenTreeNode.TreeView.EndUpdate();
            }
        }

        /// <summary>
        /// Click event handler for <see cref="removeNodeToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RemoveNode_Click(Object sender, EventArgs e)
        {
            if (jTokenTreeNode != null)
            {
                TreeView treeView = jTokenTreeNode.TreeView;
                treeView.BeginUpdate();
                jTokenTreeNode.JTokenTag.Remove();
                jTokenTreeNode.CleanParentTreeNode();
                treeView.EndUpdate();
            }
        }

        /// <summary>
        /// Identify the Source <see cref="TreeNode"/> at the origin of this <see cref="ContextMenuStrip"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        TreeNode GetSourceTreeNode()
        {
            if (SourceControl == null)
            {
                return null;
            }

            TreeView treeView = SourceControl as TreeView;
            if (treeView == null)
            {
                return null;
            }

            return treeView.SelectedNode;
        }
    }
}
