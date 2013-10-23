using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZTn.Json.Editor.Linq;

namespace ZTn.Json.Editor.Drawing
{
    /// <summary>
    /// Specialized <see cref="TreeNode"/> for handling <see cref="JToken"/> representation in a <see cref="TreeView"/>.
    /// </summary>
    class JTokenTreeNode : TreeNode, IJsonTreeNode
    {
        #region >> Properties

        public JToken JTokenTag
        {
            get { return Tag as JToken; }
        }

        #endregion

        #region >> Constructor

        public JTokenTreeNode(JToken jToken)
        {
            Tag = jToken;
            UpdateWhenCollapsing();
        }

        public JTokenTreeNode(JToken jToken, Action<JTokenTreeNode> callBack)
            : this(jToken)
        {
            if (callBack != null)
            {
                callBack(this);
            }
        }

        #endregion

        #region >> IJsonTreeNode

        /// <inheritdoc />
        public virtual void UpdateWhenCollapsing()
        {
            Text = Tag.ToString();
        }

        /// <inheritdoc />
        public virtual void UpdateWhenExpanding()
        {
            Text = Tag.ToString();
        }

        /// <inheritdoc />
        public virtual TreeNode UpdateWhenEditing(string jsonString)
        {
            JsonEditorSource jsonEditorSource;
            try
            {
                jsonEditorSource = new JsonEditorSource(jsonString);
            }
            catch
            {
                return this;
            }

            if (JTokenTag.Parent != null)
            {
                JTokenTag.Replace(jsonEditorSource.RootJToken);
            }

            return UpdateParentTreeNode(jsonEditorSource.RootTreeNode);
        }

        #endregion

        /// <summary>
        /// Remove JTokenTag from its parent if <paramref name="jsonString"/> is empty or null.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns><value>true</value> if <paramref name="jsonString"/> is empty or null.</returns>
        protected bool CheckEmptyJsonString(string jsonString)
        {
            if (String.IsNullOrWhiteSpace(jsonString))
            {
                JTokenTag.Remove();
                CleanParentTreeNode();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove <see cref="JTokenTreeNode"/>s from the parent of current <see cref="TreeNode"/> having a detached JTokenTag property.
        /// </summary>
        /// <returns>First available <see cref="TreeNode"/> or null if the parent has no children.</returns>
        protected TreeNode CleanParentTreeNode()
        {
            TreeNode parent = Parent;

            parent.Nodes
                 .OfType<JTokenTreeNode>()
                 .Where(n => n != null && n.JTokenTag.Parent == null)
                 .ToList()  // Mandatory because working list will be modified next step
                 .ForEach(n => parent.Nodes.Remove(n));

            return parent.Nodes.Cast<TreeNode>().FirstOrDefault();
        }

        /// <summary>
        /// Insert or replace a <paramref name="TreeNode"/> in current parent nodes.
        /// </summary>
        /// <param name="newNode"></param>
        /// <returns></returns>
        protected TreeNode UpdateParentTreeNode(TreeNode newNode)
        {
            if (newNode != this)
            {
                TreeNodeCollection treeNodeCollection;
                if (Parent != null)
                {
                    treeNodeCollection = Parent.Nodes;
                }
                else
                {
                    treeNodeCollection = TreeView.Nodes;
                }
                int nodeIndex = treeNodeCollection.IndexOf(this);

                treeNodeCollection.Insert(nodeIndex, newNode);

                CleanParentTreeNode();
            }

            if (IsExpanded)
            {
                newNode.Expand();
            }
            else
            {
                newNode.Collapse();
            }

            return newNode;
        }
    }
}
