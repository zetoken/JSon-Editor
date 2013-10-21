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
    /// Specialized <see cref="TreeNode"/> for handling <see cref="JValue"/> representation in a <see cref="TreeView"/>.
    /// </summary>
    sealed class JValueTreeNode : JTokenTreeNode
    {
        #region >> Properties

        public JValue jValueTag
        {
            get { return Tag as JValue; }
        }

        #endregion

        #region >> Constructor

        public JValueTreeNode(JToken jValue)
            : base(jValue)
        {
        }

        public JValueTreeNode(JToken jValue, Action<JValueTreeNode> callBack)
            : this(jValue)
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
            Text = (Tag as JValue).ToString();
        }

        /// <inheritdoc />
        public override void UpdateWhenExpanding()
        {
            Text = (Tag as JValue).ToString();
        }

        #endregion
    }
}
