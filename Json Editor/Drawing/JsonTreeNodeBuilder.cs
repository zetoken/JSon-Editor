using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZTn.Json.Editor.Drawing
{
    sealed class JsonTreeNodeBuilder
    {
        #region >> Create

        /// <summary>
        /// Create a TreeNode and its subtrees for the <paramref name="obj"/> instance by dynamically dispatching to specialized overloads.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeNode Create(dynamic obj)
        {
            return Create(obj);
        }

        /// <summary>
        /// Create a TreeNode and its subtrees for the <paramref name="obj"/> instance beeing a <see cref="JArray"/> instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeNode Create(JArray obj)
        {
            var node = new Drawing.JArrayTreeNode(obj);

            node.Nodes.AddRange(obj
                .Select(o => Create((dynamic)o))
                .Cast<TreeNode>()
                .ToArray()
                );

            return node;
        }

        /// <summary>
        /// Create a TreeNode and its subtrees for the <paramref name="obj"/> instance beeing a <see cref="JObject"/> instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeNode Create(JObject obj)
        {
            var node = new Drawing.JObjectTreeNode(obj);

            node.Nodes.AddRange(obj.Properties()
                .Select(o => Create(o))
                .ToArray()
                );

            return node;
        }

        /// <summary>
        /// Create a TreeNode and its subtrees for the <paramref name="obj"/> instance beeing a <see cref="JProperty"/> instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeNode Create(JProperty obj)
        {
            var node = new Drawing.JPropertyTreeNode(obj);

            node.Nodes.AddRange(obj
                .Select(o => Create((dynamic)o))
                .Cast<TreeNode>()
                .ToArray()
                );

            return node;
        }

        /// <summary>
        /// Throw a <see cref="UnattendedJTokenTypeException"/> for the <paramref name="obj"/> instance beeing a <see cref="JToken"/> instance.
        /// This method exists only for safety in case of a new concrete <see cref="JToken"/> instance is implemented in the future.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="UnattendedJTokenTypeException">Always thrown.</exception>
        private static TreeNode Create(JToken obj)
        {
            throw new UnattendedJTokenTypeException(obj);
        }

        /// <summary>
        /// Create a TreeNode and its subtrees for the <paramref name="obj"/> instance beeing a <see cref="JValue"/> instance.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeNode Create(JValue obj)
        {
            var node = new Drawing.JValueTreeNode(obj);

            return node;
        }

        #endregion

        #region >> Wrap

        /// <summary>
        /// Create a TreeNode for the <paramref name="obj"/> instance by dynamically dispatching to specialized overloads.
        /// No subtree is created.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeNode Wrap(dynamic obj)
        {
            return Wrap(obj);
        }

        /// <summary>
        /// Create a TreeNode for the <paramref name="obj"/> instance beeing a <see cref="JArray"/> instance.
        /// No subtree is created.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeNode Wrap(JArray obj)
        {
            return new JArrayTreeNode(obj);
        }

        /// <summary>
        /// Create a TreeNode for the <paramref name="obj"/> instance beeing a <see cref="JObject"/> instance.
        /// No subtree is created.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeNode Wrap(JObject obj)
        {
            return new JObjectTreeNode(obj);
        }

        /// <summary>
        /// Create a TreeNode for the <paramref name="obj"/> instance beeing a <see cref="JProperty"/> instance.
        /// No subtree is created.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeNode Wrap(JProperty obj)
        {
            return new JPropertyTreeNode(obj);
        }

        /// <summary>
        /// Throw a <see cref="UnattendedJTokenTypeException"/> for the <paramref name="obj"/> instance beeing a <see cref="JToken"/> instance.
        /// This method exists only for safety in case of a new concrete <see cref="JToken"/> instance is implemented in the future.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="UnattendedJTokenTypeException">Always thrown.</exception>
        private static TreeNode Wrap(JToken obj)
        {
            throw new UnattendedJTokenTypeException(obj);
        }

        /// <summary>
        /// Create a TreeNode for the <paramref name="obj"/> instance beeing a <see cref="JValue"/> instance.
        /// No subtree is created.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeNode Wrap(JValue obj)
        {
            return new JValueTreeNode(obj);
        }

        #endregion
    }
}
