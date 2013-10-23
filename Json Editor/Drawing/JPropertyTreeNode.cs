using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZTn.Json.Editor.Linq;

namespace ZTn.Json.Editor.Drawing
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

        #region >> Constructor

        public JPropertyTreeNode(JProperty jProperty)
            : base(jProperty)
        {
        }

        public JPropertyTreeNode(JProperty jProperty, Action<JPropertyTreeNode> callBack)
            : this(jProperty)
        {
            if (callBack != null)
            {
                callBack(this);
            }
        }

        #endregion

        #region >> JTokenTreeNode

        /// <inheritdoc />
        public override void UpdateWhenCollapsing()
        {
            Text = (Tag as JProperty).ToString();
            NodeFont = JsonEditorMainForm.JsonEditorForm.Font;
        }

        /// <inheritdoc />
        public override void UpdateWhenExpanding()
        {
            Text = (Tag as JProperty).Name;
            NodeFont = new Font(JsonEditorMainForm.JsonEditorForm.Font, FontStyle.Underline);
        }

        /// <inheritdoc />
        public override TreeNode UpdateWhenEditing(string jsonString)
        {
            if (CheckEmptyJsonString(jsonString))
            {
                return null;
            }

            // To allow parsing, the partial json string is first enclosed as a json object
            JsonEditorSource jsonEditorSource;
            try
            {
                jsonEditorSource = new JsonEditorSource("{" + jsonString + "}");
            }
            catch
            {
                return this;
            }

            // Extract the contained JProperties as the JObject was only a container
            // As Json.NET internally clones JToken instances having Parent!=null when inserting in a JContainer,
            // explicitly clones the new JProperties nullify Parent and to know the instances 
            List<JProperty> jParsedProperties = ((JObject)jsonEditorSource.RootJToken)
                .Properties()
                .Select(p => new JProperty(p))
                .ToList();

            // Build a new list of TreeNodes for these JProperties
            List<JPropertyTreeNode> jParsedTreeNode = jParsedProperties
                .Select(p => JsonTreeNodeBuilder.JsonVisitor(p))
                .Cast<JPropertyTreeNode>()
                .ToList();

            // Update the properties of parent JObject by inserting jParsedProperties and removing edited JProperty
            JObject jObjectParent = (JObject)JPropertyTag.Parent;
            List<JProperty> jProperties = jObjectParent.Properties()
                .SelectMany(p => Object.ReferenceEquals(p, JPropertyTag) ? jParsedProperties : new List<JProperty>() { p })
                .Distinct(new Editor.Linq.JPropertyEqualityComparer())
                .ToList();
            jObjectParent.ReplaceAll(jProperties);

            return UpdateTreeNodes(jParsedTreeNode);
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
