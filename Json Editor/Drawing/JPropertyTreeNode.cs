using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            // To be parseable, the partial json string is first constructed as a json object
            JsonEditorSource jsonEditorSource;
            try
            {
                jsonEditorSource = new JsonEditorSource("{" + jsonString + "}");
            }
            catch
            {
                return this;
            }

            // Extract the contained JProperty as the JObject was only a container
            jsonEditorSource.Load(
                ((JObject)jsonEditorSource.RootJToken)
                .Properties()
                .First()
                );

            // Newtonsoft.json does not internally use the JProperty instance added to it;
            // Build a fresh JPropertyTreeNode to keep track of fresh JProperty instance.
            if (JPropertyTag.Parent != null)
            {
                JContainer parent = JPropertyTag.Parent;
                int index = ((IList<JToken>)parent).IndexOf(JPropertyTag);

                JPropertyTag.Replace(jsonEditorSource.RootJToken);

                jsonEditorSource.Load(((IList<JToken>)parent)[index]);
            }

            return UpdateTreeNodes(jsonEditorSource.RootTreeNode);
        }

        #endregion
    }
}
