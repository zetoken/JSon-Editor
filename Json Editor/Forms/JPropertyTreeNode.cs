using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZTn.Json.Editor.Linq;

namespace ZTn.Json.Editor.Forms
{
    /// <summary>
    /// Specialized <see cref="TreeNode"/> for handling <see cref="JProperty"/> representation in a <see cref="TreeView"/>.
    /// </summary>
    sealed class JPropertyTreeNode : JTokenTreeNode
    {
        #region >> Properties

        public JProperty JPropertyTag
        {
            get { return Tag as JProperty; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JPropertyTreeNode"/> class.
        /// </summary>
        public JPropertyTreeNode(JProperty jProperty)
            : base(jProperty)
        {
            ContextMenuStrip = new JPropertyContextMenuStrip();
        }

        #endregion

        #region >> JTokenTreeNode

        /// <inheritdoc />
        public override void AfterCollapse()
        {
            base.AfterCollapse();
            if (this.TreeView != null)
            {
                NodeFont = this.TreeView.Font;
            }
        }

        /// <inheritdoc />
        public override void AfterExpand()
        {
            Text = JPropertyTag.Name;
            if (this.TreeView != null)
            {
                NodeFont = new Font(this.TreeView.Font, FontStyle.Underline);
            }
        }

        /// <inheritdoc />
        public override TreeNode AfterJsonTextChange(string jsonString)
        {
            if (CheckEmptyJsonString(jsonString))
            {
                return null;
            }

            // To allow parsing, the partial json string is first enclosed as a json object
            JTokenRoot jTokenRoot;
            try
            {
                jTokenRoot = new JTokenRoot("{" + jsonString + "}");
            }
            catch
            {
                return this;
            }

            // Extract the contained JProperties as the JObject was only a container
            // As Json.NET internally clones JToken instances having Parent!=null when inserting in a JContainer,
            // explicitly clones the new JProperties to nullify Parent and to know of the instances
            List<JProperty> jParsedProperties = ((JObject)jTokenRoot.JTokenValue).Properties()
                .Select(p => new JProperty(p))
                .ToList();

            // Update the properties of parent JObject by inserting jParsedProperties and removing edited JProperty
            JObject jObjectParent = (JObject)JPropertyTag.Parent;

            List<JProperty> jProperties = jObjectParent.Properties()
                .SelectMany(p => Object.ReferenceEquals(p, JPropertyTag) ? jParsedProperties : new List<JProperty>() { p })
                .Distinct(new Editor.Linq.JPropertyEqualityComparer())
                .ToList();
            jObjectParent.ReplaceAll(jProperties);

            // Build a new list of TreeNodes for these JProperties
            List<JPropertyTreeNode> jParsedTreeNodes = jParsedProperties
                .Select(p => JsonTreeNodeFactory.Create(p))
                .Cast<JPropertyTreeNode>()
                .ToList();

            return UpdateTreeNodes(jParsedTreeNodes);
        }

        #endregion

        /// <summary>
        /// Insert or replace a set of <paramref name="JPropertyTreeNode"/>s in current parent nodes.
        /// </summary>
        /// <param name="newNode"></param>
        /// <returns></returns>
        public TreeNode UpdateTreeNodes(IEnumerable<JPropertyTreeNode> newNodes)
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

            newNodes.ForEach(n => treeNodeCollection.Insert(nodeIndex++, n));

            CleanParentTreeNode();

            return newNodes.FirstOrDefault();
        }
    }
}
