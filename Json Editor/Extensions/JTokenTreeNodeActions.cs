using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using ZTn.Json.Editor.Forms;
using ZTn.Json.Editor.Generic;

namespace ZTn.Json.Editor.Extensions
{
    public static class JTokenTreeNodeActions
    {
        /// <summary>
        /// Implementation of "copy" action
        /// </summary>
        /// <param name="node"></param>
        public static JTokenTreeNode ClipboardCopy(this JTokenTreeNode node)
        {
            EditorClipboard<JTokenTreeNode>.Set(node);

            return node;
        }

        /// <summary>
        /// Implementation of "cut" action
        /// </summary>
        /// <param name="node"></param>
        public static JTokenTreeNode ClipboardCut(this JTokenTreeNode node)
        {
            EditorClipboard<JTokenTreeNode>.Set(node, false);

            return node;
        }

        /// <summary>
        /// Implementation of "paste after" action.
        /// </summary>
        /// <param name="node">Reference node for the paste.</param>
        public static void ClipboardPasteAfter(this JTokenTreeNode node)
        {
            node.ClipboardPaste(
                jt => node.JTokenTag.AddAfterSelf(jt),
                n => node.UpdateParentTreeNode(n, false)
                );
        }

        /// <summary>
        /// Implementation of "paste before" action.
        /// </summary>
        /// <param name="node"></param>
        public static void ClipboardPasteBefore(this JTokenTreeNode node)
        {
            node.ClipboardPaste(
                jt => node.JTokenTag.AddBeforeSelf(jt),
                n => node.UpdateParentTreeNode(n, true)
                );
        }

        /// <summary>
        /// Implementation of "paste and replace" action.
        /// </summary>
        /// <param name="node"></param>
        public static void ClipboardPasteReplace(this JTokenTreeNode node)
        {
            node.ClipboardPaste(
                jt => node.JTokenTag.Replace(jt),
                n => node.UpdateParentTreeNode(n, true)
                );
        }

        /// <summary>
        /// Implementation of "paste" action using 2 delegates for the concrete action on JToken tree and TreeView.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="pasteJTokenImplementation">Implementation of paste action in the JToken tree.</param>
        /// <param name="pasteJTokenImplementation">Implementation of paste action in the treeView.</param>
        private static void ClipboardPaste(this JTokenTreeNode node, Action<JToken> pasteJTokenImplementation, Action<TreeNode> pasteTreeNodeImplementation)
        {
            JTokenTreeNode sourceJTokenTreeNode = EditorClipboard<JTokenTreeNode>.Get();

            JToken jTokenSource = sourceJTokenTreeNode.JTokenTag.DeepClone();

            try
            {
                pasteJTokenImplementation(jTokenSource);
            }
            catch (Exception exception)
            {
                // If cut was asked, the clipboard is now empty and source should be inserted again in clipboard
                if (EditorClipboard<JTokenTreeNode>.IsEmpty())
                {
                    EditorClipboard<JTokenTreeNode>.Set(sourceJTokenTreeNode, false);
                }

                throw new JTokenTreeNodePasteException(exception);
            }

            TreeView treeView = node.TreeView;
            treeView.BeginUpdate();

            pasteTreeNodeImplementation(JsonTreeNodeFactory.Create(jTokenSource));

            treeView.EndUpdate();

            // If cut was asked, the clipboard is now empty and source should be removed from treeview
            if (EditorClipboard<JTokenTreeNode>.IsEmpty())
            {
                sourceJTokenTreeNode.EditDelete();
            }
        }

        /// <summary>
        /// Implementation of "delete" action.
        /// </summary>
        /// <param name="jTokenTreeNode"></param>
        /// <param name="node"></param>
        public static void EditDelete(this JTokenTreeNode node)
        {
            if (node != null)
            {
                try
                {
                    node.JTokenTag.Remove();
                }
                catch (Exception exception)
                {
                    throw new JTokenTreeNodeDeleteException(exception);
                }

                TreeView treeView = node.TreeView;
                treeView.BeginUpdate();

                node.CleanParentTreeNode();

                treeView.EndUpdate();
            }
        }
    }
}
