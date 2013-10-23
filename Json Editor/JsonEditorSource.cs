using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZTn.Json.Editor.Drawing;

namespace ZTn.Json.Editor
{
    sealed class JsonEditorSource
    {
        private JToken jTokenRoot;
        private JTokenTreeNode treeNodeRoot;

        #region >> Properties

        /// <summary>
        /// Raw Json text.
        /// </summary>
        public string Text
        {
            get { return jTokenRoot.ToString(); }
        }

        /// <summary>
        /// Root <see cref="JToken"/> node
        /// </summary>
        public JToken RootJToken
        {
            get { return jTokenRoot; }
            set { jTokenRoot = value; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public JsonEditorSource()
        {
            jTokenRoot = JToken.Parse("{}");
        }

        /// <summary>
        /// Constructor using an existing stream to populate the instance.
        /// </summary>
        /// <param name="jsonStream">Source stream.</param>
        public JsonEditorSource(Stream jsonStream)
            : this()
        {
            Load(jsonStream);
        }

        /// <summary>
        /// Constructor using an existing json string to populate the instance.
        /// </summary>
        /// <param name="jsonString">Source string.</param>
        public JsonEditorSource(string jsonString)
            : this()
        {
            Load(jsonString);
        }

        /// <summary>
        /// Constructor using an existing json string to populate the instance.
        /// </summary>
        /// <param name="jsonString">Source string.</param>
        public JsonEditorSource(JToken jToken)
            : this()
        {
            Load(jToken);
        }

        #endregion

        public string BeautifyJsonText()
        {
            return jTokenRoot.ToString();
        }

        public void Load(Stream jsonStream)
        {
            using (var streamReader = new StreamReader(jsonStream))
            {
                Load(streamReader.ReadToEnd());
            }
        }

        public void Load(string jsonString)
        {
            Load(JToken.Parse(jsonString));
        }

        public void Load(JToken jToken)
        {
            jTokenRoot = jToken;
            treeNodeRoot = JsonTreeNodeBuilder.JsonVisitor((dynamic)jTokenRoot);
        }

        public void Save(Stream jsonStream)
        {
            using (var streamWriter = new StreamWriter(jsonStream))
            {
                streamWriter.Write(jTokenRoot.ToString());
            }
        }

        public JTokenTreeNode RootTreeNode
        {
            get
            {
                return treeNodeRoot;
            }
        }
    }
}
