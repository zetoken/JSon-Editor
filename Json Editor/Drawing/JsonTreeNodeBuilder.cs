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
        /// <summary>
        /// Visitor allowing dynamic dispatch to specialized overloads.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TreeNode JsonVisitor(dynamic obj)
        {
            return JsonVisitor(obj);
        }

        /// <summary>
        /// Visit a JSON array.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static TreeNode JsonVisitor(JArray obj)
        {
            var node = new Drawing.JArrayTreeNode(obj);

            node.Nodes.AddRange(obj
                .Select(o => JsonVisitor((dynamic)o))
                .Cast<TreeNode>()
                .ToArray()
                );

            return node;
        }

        /// <summary>
        /// Visit a JSON object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static TreeNode JsonVisitor(JObject obj)
        {
            var node = new Drawing.JObjectTreeNode(obj);

            node.Nodes.AddRange(obj.Properties()
                .Select(o => JsonVisitor(o))
                .ToArray()
                );

            return node;
        }

        /// <summary>
        /// Visit a JSON property.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static TreeNode JsonVisitor(JProperty obj)
        {
            var node = new Drawing.JPropertyTreeNode(obj);

            node.Nodes.AddRange(obj
                .Select(o => JsonVisitor((dynamic)o))
                .Cast<TreeNode>()
                .ToArray()
                );

            return node;
        }

        /// <summary>
        /// Visit an abstract JSON token.
        /// This method exists only for safety in case of a new concrete <see cref="JToken"/> instance is implemented in the future.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static TreeNode JsonVisitor(JToken obj)
        {
            var node = new Drawing.JTokenTreeNode(obj);

            return node;
        }

        /// <summary>
        /// Visit a JSON value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static TreeNode JsonVisitor(JValue obj)
        {
            var node = new Drawing.JValueTreeNode(obj);

            return node;
        }
    }
}
