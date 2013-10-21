using System;
using System.Windows.Forms;
namespace ZTn.Json.Editor.Drawing
{
    interface IJsonTreeNode
    {
        /// <summary>
        /// To be called whenever the node is collapsing
        /// </summary>
        void UpdateWhenCollapsing();

        /// <summary>
        /// To be called whenever the node is expanding
        /// </summary>
        void UpdateWhenExpanding();

        /// <summary>
        /// To be called whenever the value of the json text is changed
        /// </summary>
        TreeNode UpdateWhenEditing(string newJson);
    }
}
