using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            return UpdateTreeNodes(jsonEditorSource.RootTreeNode);
        }

        #endregion

        protected TreeNode UpdateTreeNodes(TreeNode newNode)
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
                    treeNodeCollection = JsonEditorMainForm.JsonEditorForm.jsonTreeView.Nodes;
                }
                int nodeIndex = treeNodeCollection.IndexOf(this);

                treeNodeCollection.Insert(nodeIndex, newNode);
                treeNodeCollection.Remove(this);
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
