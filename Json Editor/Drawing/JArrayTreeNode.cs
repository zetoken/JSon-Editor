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
    /// Specialized <see cref="TreeNode"/> for handling <see cref="JArray"/> representation in a <see cref="TreeView"/>.
    /// </summary>
    sealed class JArrayTreeNode : JTokenTreeNode
    {
        #region >> Properties

        public JArray jArrayTag
        {
            get { return Tag as JArray; }
        }

        #endregion

        #region >> Constructor

        public JArrayTreeNode(JArray jArray)
            : base(jArray)
        {
            Tag = jArray;
            AfterCollapse();
        }

        public JArrayTreeNode(JArray jArray, Action<JArrayTreeNode> callBack)
            : this(jArray)
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
            Text = "[" + jArrayTag.Type + "] " + Tag;
        }

        /// <inheritdoc />
        public override void AfterExpand()
        {
            Text = "[" + jArrayTag.Type + "]";
        }

        #endregion
    }
}
