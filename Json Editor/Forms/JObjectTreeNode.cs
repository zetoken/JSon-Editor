using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZTn.Json.Editor.Forms
{
    /// <summary>
    /// Specialized <see cref="TreeNode"/> for handling <see cref="JObject"/> representation in a <see cref="TreeView"/>.
    /// </summary>
    sealed class JObjectTreeNode : JTokenTreeNode
    {
        #region >> Properties

        public JObject JObjectTag
        {
            get { return Tag as JObject; }
        }

        #endregion

        #region >> Constructor

        public JObjectTreeNode(JObject jObject)
            : base(jObject)
        {
            ContextMenuStrip = new JObjectContextMenuStrip();
        }

        public JObjectTreeNode(JObject jObject, Action<JObjectTreeNode> callBack)
            : this(jObject)
        {
            if (callBack != null)
            {
                callBack(this);
            }
        }

        #endregion

        #region >> JTokenTreeNode

        /// <inheritdoc />
        public override void AfterCollapse()
        {
            Text = "{" + JObjectTag.Type + "} " + JObjectTag;
        }

        /// <inheritdoc />
        public override void AfterExpand()
        {
            Text = "{" + JObjectTag.Type + "}";
        }

        #endregion
    }
}
