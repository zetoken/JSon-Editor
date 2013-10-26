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
        protected ToolStripItem collapseAllToolStripItem = new ToolStripMenuItem("Collapse All", null, CollapseAll_Click);
        protected ToolStripItem expandAllToolStripItem = new ToolStripMenuItem("Expand All", null, ExpandAll_Click);
        protected ToolStripItem removeNodeToolStripItem = new ToolStripMenuItem("Remove All", null, RemoveNode_Click);

       #region >> Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JTokenContextMenuStrip"/> class.
        /// </summary>
        public JTokenContextMenuStrip()
            : base()
        {
            Items.Add(collapseAllToolStripItem);
            Items.Add(expandAllToolStripItem);
            Items.Add(new ToolStripSeparator());
            Items.Add(removeNodeToolStripItem);
        }

        #endregion

        #region >> ContextMenuStrip

        /// <inheritdoc />
        protected override void OnVisibleChanged(EventArgs e)
        {
            JTokenTreeNode jTokenTreeNode = GetSourceTreeNode(this) as JTokenTreeNode;

            if (Visible)
            {
                collapseAllToolStripItem.Visible = jTokenTreeNode.IsExpanded && jTokenTreeNode.JTokenTag.HasValues;
                expandAllToolStripItem.Visible = !jTokenTreeNode.IsExpanded && jTokenTreeNode.JTokenTag.HasValues;
                removeNodeToolStripItem.Visible = (jTokenTreeNode.Parent != null) && !(jTokenTreeNode.Parent is JPropertyTreeNode);
            }

            base.OnVisibleChanged(e);
        }

        #endregion

        #region >> Methods GetSourceTreeNode

        /// <summary>
        /// Identify the <see cref="TreeNode"/> that generated a <see cref="ContextMenuStrip"/> instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        protected static TreeNode GetSourceTreeNode(ContextMenuStrip contextMenuStrip)
        {
            if (contextMenuStrip == null)
            {
                return null;
            }

            TreeView treeView = contextMenuStrip.SourceControl as TreeView;
            if (treeView.SelectedNode == null)
            {
                return null;
            }

            return treeView.SelectedNode;
        }

        /// <summary>
        /// Identify the <see cref="TreeNode"/> calling a <see cref="ToolStripItem"/> instance.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        protected static TreeNode GetSourceTreeNode(ToolStripItem toolStripItem)
        {
            if (toolStripItem == null)
            {
                return null;
            }

            return GetSourceTreeNode(toolStripItem.Owner as ContextMenuStrip);
        }

        #endregion

        private static void CollapseAll_Click(Object sender, EventArgs e)
        {
            TreeNode treeNode = GetSourceTreeNode(sender as ToolStripItem);
            if (treeNode != null)
            {
                treeNode.TreeView.BeginUpdate();
                treeNode.Collapse(false);
                treeNode.TreeView.EndUpdate();
            }
        }

        private static void ExpandAll_Click(Object sender, EventArgs e)
        {
            TreeNode treeNode = GetSourceTreeNode(sender as ToolStripItem);
            if (treeNode != null)
            {
                treeNode.TreeView.BeginUpdate();
                treeNode.ExpandAll();
                treeNode.TreeView.EndUpdate();
            }
        }

        private static void RemoveNode_Click(Object sender, EventArgs e)
        {
            JTokenTreeNode treeNode = GetSourceTreeNode(sender as ToolStripItem) as JTokenTreeNode;
            if (treeNode != null)
            {
                TreeView treeView = treeNode.TreeView;
                treeView.BeginUpdate();
                treeNode.JTokenTag.Remove();
                treeNode.CleanParentTreeNode();
                treeView.EndUpdate();
            }
        }
    }
}
