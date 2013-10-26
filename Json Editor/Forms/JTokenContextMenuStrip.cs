using Newtonsoft.Json.Linq;
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

        protected ToolStripMenuItem editToolStripItem;

        protected ToolStripItem copyNodeToolStripItem;
        protected ToolStripItem cutNodeToolStripItem;
        protected ToolStripItem deleteNodeToolStripItem;
        protected ToolStripItem pasteNodeAfterToolStripItem;
        protected ToolStripItem pasteNodeBeforeToolStripItem;
        protected ToolStripItem pasteNodeReplaceToolStripItem;

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JTokenContextMenuStrip"/> class.
        /// </summary>
        public JTokenContextMenuStrip()
            : base()
        {
            collapseAllToolStripItem = new ToolStripMenuItem("Collapse All", null, CollapseAll_Click);
            expandAllToolStripItem = new ToolStripMenuItem("Expand All", null, ExpandAll_Click);

            editToolStripItem = new ToolStripMenuItem("Edit");
            copyNodeToolStripItem = new ToolStripMenuItem("Copy", null, CopyNode_Click);
            cutNodeToolStripItem = new ToolStripMenuItem("Cut", null, CutNode_Click);
            deleteNodeToolStripItem = new ToolStripMenuItem("Delete Node", null, DeleteNode_Click);
            pasteNodeAfterToolStripItem = new ToolStripMenuItem("Paste Node After", null, PasteNodeAfter_Click);
            pasteNodeBeforeToolStripItem = new ToolStripMenuItem("Paste Node Before", null, PasteNodeBefore_Click);
            pasteNodeReplaceToolStripItem = new ToolStripMenuItem("Replace", null, PasteNodeReplace_Click);
            editToolStripItem.DropDownItems.Add(copyNodeToolStripItem);
            editToolStripItem.DropDownItems.Add(cutNodeToolStripItem);
            editToolStripItem.DropDownItems.Add(pasteNodeBeforeToolStripItem);
            editToolStripItem.DropDownItems.Add(pasteNodeAfterToolStripItem);
            editToolStripItem.DropDownItems.Add(new ToolStripSeparator());
            editToolStripItem.DropDownItems.Add(pasteNodeReplaceToolStripItem);
            editToolStripItem.DropDownItems.Add(new ToolStripSeparator());
            editToolStripItem.DropDownItems.Add(deleteNodeToolStripItem);

            Items.Add(collapseAllToolStripItem);
            Items.Add(expandAllToolStripItem);
            Items.Add(editToolStripItem);
        }

        #endregion

        #region >> ContextMenuStrip

        /// <inheritdoc />
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                jTokenTreeNode = FindSourceTreeNode() as JTokenTreeNode;

                // Collapse item shown if node is expanded and has children
                collapseAllToolStripItem.Visible = jTokenTreeNode.IsExpanded
                    && jTokenTreeNode.Nodes.Cast<TreeNode>().Any();

                // Expand item shown if node if not expanded or has a children not expanded
                expandAllToolStripItem.Visible = !jTokenTreeNode.IsExpanded
                    || jTokenTreeNode.Nodes.Cast<TreeNode>().Any(t => !t.IsExpanded);

                // Remove item enabled if it is not the root or the value of a property
                deleteNodeToolStripItem.Enabled = (jTokenTreeNode.Parent != null)
                    && !(jTokenTreeNode.Parent is JPropertyTreeNode);

                // Cut item enabled if delete is
                cutNodeToolStripItem.Enabled = deleteNodeToolStripItem.Enabled;

                // Paste items enabled only when a copy or cut operation is pending
                pasteNodeAfterToolStripItem.Enabled = !EditorClipboard<JTokenTreeNode>.IsEmpty()
                    && (jTokenTreeNode.Parent != null)
                    && !(jTokenTreeNode.Parent is JPropertyTreeNode);

                pasteNodeBeforeToolStripItem.Enabled = !EditorClipboard<JTokenTreeNode>.IsEmpty()
                    && (jTokenTreeNode.Parent != null)
                    && !(jTokenTreeNode.Parent is JPropertyTreeNode);

                pasteNodeReplaceToolStripItem.Enabled = !EditorClipboard<JTokenTreeNode>.IsEmpty();

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
        /// Click event handler for <see cref="copyNodeToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CopyNode_Click(Object sender, EventArgs e)
        {
            EditorClipboard<JTokenTreeNode>.Set(jTokenTreeNode);
        }

        /// <summary>
        /// Click event handler for <see cref="cutNodeToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CutNode_Click(Object sender, EventArgs e)
        {
            EditorClipboard<JTokenTreeNode>.Set(jTokenTreeNode, false);
        }

        /// <summary>
        /// Click event handler for <see cref="deleteNodeToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteNode_Click(Object sender, EventArgs e)
        {
            DeleteNode(jTokenTreeNode);
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
        /// Click event handler for <see cref="pasteNodeAfterToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PasteNodeAfter_Click(Object sender, EventArgs e)
        {
            JTokenTreeNode sourceJTokenTreeNode = EditorClipboard<JTokenTreeNode>.Get();

            JToken jTokenSource = sourceJTokenTreeNode.JTokenTag.DeepClone();

            try
            {
                jTokenTreeNode.JTokenTag.AddAfterSelf(jTokenSource);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            TreeView treeView = jTokenTreeNode.TreeView;
            treeView.BeginUpdate();

            jTokenTreeNode.UpdateParentTreeNode(JsonTreeNodeFactory.Create(jTokenSource), false);

            treeView.EndUpdate();

            // If cut was asked, the clipboard is now empty and source should be removed from treeview
            if (EditorClipboard<JTokenTreeNode>.IsEmpty())
            {
                DeleteNode(sourceJTokenTreeNode);
            }
        }

        /// <summary>
        /// Click event handler for <see cref="pasteNodeBeforeToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PasteNodeBefore_Click(Object sender, EventArgs e)
        {
            JTokenTreeNode sourceJTokenTreeNode = EditorClipboard<JTokenTreeNode>.Get();

            JToken jTokenSource = sourceJTokenTreeNode.JTokenTag.DeepClone();

            try
            {
                jTokenTreeNode.JTokenTag.AddBeforeSelf(jTokenSource);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            TreeView treeView = jTokenTreeNode.TreeView;
            treeView.BeginUpdate();

            jTokenTreeNode.UpdateParentTreeNode(JsonTreeNodeFactory.Create(jTokenSource), true);

            treeView.EndUpdate();

            // If cut was asked, the clipboard is now empty and source should be removed from treeview
            if (EditorClipboard<JTokenTreeNode>.IsEmpty())
            {
                DeleteNode(sourceJTokenTreeNode);
            }
        }

        /// <summary>
        /// Click event handler for <see cref="pasteNodeReplaceToolStripItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PasteNodeReplace_Click(Object sender, EventArgs e)
        {
            JTokenTreeNode sourceJTokenTreeNode = EditorClipboard<JTokenTreeNode>.Get();

            JToken jTokenSource = sourceJTokenTreeNode.JTokenTag.DeepClone();

            try
            {
                jTokenTreeNode.JTokenTag.Replace(jTokenSource);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            TreeView treeView = jTokenTreeNode.TreeView;
            treeView.BeginUpdate();

            jTokenTreeNode.UpdateParentTreeNode(JsonTreeNodeFactory.Create(jTokenSource), true);

            treeView.EndUpdate();

            // If cut was asked, the clipboard is now empty and source should be removed from treeview
            if (EditorClipboard<JTokenTreeNode>.IsEmpty())
            {
                DeleteNode(sourceJTokenTreeNode);
            }
        }

        /// <summary>
        /// Identify the Source <see cref="TreeNode"/> at the origin of this <see cref="ContextMenuStrip"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        TreeNode FindSourceTreeNode()
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

        void DeleteNode(JTokenTreeNode node)
        {
            if (node != null)
            {
                TreeView treeView = node.TreeView;
                treeView.BeginUpdate();

                node.JTokenTag.Remove();
                node.CleanParentTreeNode();

                treeView.EndUpdate();
            }
        }

    }
}
