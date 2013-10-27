using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZTn.Json.Editor.Generic;
using ZTn.Json.Editor.Linq;

namespace ZTn.Json.Editor.Forms
{
    /// <summary>
    /// Specialized <see cref="TreeNode"/> for handling <see cref="JToken"/> representation in a <see cref="TreeView"/>.
    /// </summary>
    abstract class JTokenTreeNode : TreeNode, IJsonTreeNode
    {
        #region >> Properties

        public JToken JTokenTag
        {
            get { return Tag as JToken; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JTokenTreeNode"/> class.
        /// </summary>
        /// <param name="jToken"></param>
        public JTokenTreeNode(JToken jToken)
        {
            Tag = jToken;
            ContextMenuStrip = SingleInstanceProvider<JTokenContextMenuStrip>.Value;
            AfterCollapse();
        }

        #endregion

        #region >> IJsonTreeNode

        /// <inheritdoc />
        /// <remarks>Default simple implementation to be overriden if needed.</remarks>
        public virtual void AfterCollapse()
        {
            Text = Tag.ToString();
        }

        /// <inheritdoc />
        /// <remarks>Default simple implementation to be overriden if needed.</remarks>
        public virtual void AfterExpand()
        {
            Text = Tag.ToString();
        }

        /// <inheritdoc />
        /// <remarks>Default simple implementation to be overriden if needed.</remarks>
        public virtual TreeNode AfterJsonTextChange(string jsonString)
        {
            JTokenRoot jTokenRoot;
            try
            {
                jTokenRoot = new JTokenRoot(jsonString);
            }
            catch
            {
                return this;
            }

            if (JTokenTag.Parent != null)
            {
                JTokenTag.Replace(jTokenRoot.JTokenValue);
            }

            return UpdateParentTreeNode(JsonTreeNodeFactory.Create(jTokenRoot.JTokenValue));
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
        public TreeNode CleanParentTreeNode()
        {
            TreeNode parent = Parent;

            // ToList() is mandatory before ForEach because working list will be modified
            parent.Nodes
                .OfType<JTokenTreeNode>()
                .Where(n => n != null && n.JTokenTag.Parent == null)
                .ToList()
                .ForEach(n => parent.Nodes.Remove(n));

            return parent.Nodes
                .Cast<TreeNode>()
                .FirstOrDefault();
        }

        /// <summary>
        /// Insert or replace a <paramref name="TreeNode"/> in current parent nodes.
        /// </summary>
        /// <param name="newNode"></param>
        /// <returns></returns>
        public TreeNode UpdateParentTreeNode(TreeNode newNode)
        {
            return UpdateParentTreeNode(newNode, true);
        }

        /// <summary>
        /// Insert or replace a <paramref name="TreeNode"/> in current parent nodes.
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="insertBefore">
        /// Set to <c>true</c> to insert <paramref name="newNode"/> before current node.
        /// Set to <c>fase</c> to insert <paramref name="newNode"/> after current node.
        /// </param>
        /// <returns></returns>
        public TreeNode UpdateParentTreeNode(TreeNode newNode, bool insertBefore)
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

                if (insertBefore)
                {
                    treeNodeCollection.Insert(nodeIndex, newNode);
                }
                else
                {
                    treeNodeCollection.Insert(nodeIndex + 1, newNode);
                }


                CleanParentTreeNode();

                if (IsExpanded)
                {
                    newNode.Expand();
                }
                else
                {
                    newNode.Collapse();
                }
            }

            return newNode;
        }
    }
}
