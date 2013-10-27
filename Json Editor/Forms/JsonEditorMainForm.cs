using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZTn.Json.Editor.Forms;
using ZTn.Json.Editor.Linq;

namespace ZTn.Json.Editor.Forms
{
    public sealed partial class JsonEditorMainForm : Form
    {
        #region >> Fields

        JTokenRoot jsonEditorItem;

        #endregion

        #region >> Constructor

        public JsonEditorMainForm()
        {
            InitializeComponent();

            jsonTypeComboBox.DataSource = Enum.GetValues(typeof(JTokenType));

            jsonTreeView.AfterCollapse += jsonTreeView_AfterCollapse;
            jsonTreeView.AfterExpand += jsonTreeView_AfterExpand;
        }

        #endregion

        #region >> Form

        /// <inheritdoc />
        /// <remarks>
        /// Optimization aiming to reduce flickering on large documents (successfully).
        /// Source: http://stackoverflow.com/a/89125/1774251
        /// </remarks>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        #endregion

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "json files (*.json)|*.json",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var stream = openFileDialog.OpenFile())
                {
                    if (stream != null)
                    {
                        jsonEditorItem = new JTokenRoot(stream);
                    }
                    else
                    {
                        jsonEditorItem = new JTokenRoot("{}");
                    }
                }

                jsonTreeView.Nodes.Clear();
                jsonTreeView.Nodes.Add(JsonTreeNodeFactory.Create(jsonEditorItem.JTokenValue));
                jsonTreeView.Nodes
                    .Cast<TreeNode>()
                    .ForEach(n => n.Expand());

                jsonTreeTabPage.Text = openFileDialog.FileName;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "json files (*.json)|*.json",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var stream = saveFileDialog.OpenFile())
                {
                    if (stream != null)
                    {
                        new JTokenRoot(((JTokenTreeNode)jsonTreeView.TopNode).JTokenTag).Save(stream);
                    }
                }
            }
        }

        private void newJsonObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jsonEditorItem = new JTokenRoot("{}");

            jsonTreeView.Nodes.Clear();
            jsonTreeView.Nodes.Add(JsonTreeNodeFactory.Create(jsonEditorItem.JTokenValue));
            jsonTreeView.Nodes
                .Cast<TreeNode>()
                .ForEach(n => n.Expand());
        }

        private void newJsonArrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jsonEditorItem = new JTokenRoot("[]");

            jsonTreeView.Nodes.Clear();
            jsonTreeView.Nodes.Add(JsonTreeNodeFactory.Create(jsonEditorItem.JTokenValue));
            jsonTreeView.Nodes
                .Cast<TreeNode>()
                .ForEach(n => n.Expand());
        }

        private void aboutJsonEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        /// <summary>
        /// For the clicked node to be selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jsonTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            jsonTreeView.SelectedNode = e.Node;
        }

        private void jsonTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            IJsonTreeNode node = e.Node as IJsonTreeNode;
            if (node != null)
            {
                node.AfterCollapse();
            }
        }

        private void jsonTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            IJsonTreeNode node = e.Node as IJsonTreeNode;
            if (node != null)
            {
                node.AfterExpand();
            }
        }

        private void jsonValueTextBox_TextChanged(object sender, EventArgs e)
        {
            IJsonTreeNode node = jsonTreeView.SelectedNode as IJsonTreeNode;
            if (node != null)
            {
                jsonTreeView.BeginUpdate();
                jsonTreeView.SelectedNode = node.AfterJsonTextChange(jsonValueTextBox.Text);
                jsonTreeView.EndUpdate();
            }
        }

        private void jsonValueTextBox_Leave(object sender, EventArgs e)
        {
            jsonValueTextBox.TextChanged -= jsonValueTextBox_TextChanged;
        }

        private void jsonValueTextBox_Enter(object sender, EventArgs e)
        {
            jsonValueTextBox.TextChanged += jsonValueTextBox_TextChanged;
        }

        #region >> Methods jsonTreeView_AfterSelect

        /// <summary>
        /// Main event handler dynamically dispatching the handling to specialized methods.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jsonTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            JsonTreeView_AfterSelectImplementation((dynamic)e.Node, e);
        }

        /// <summary>
        /// Default catcher in case of a node of unattended type.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="e"></param>
        private void JsonTreeView_AfterSelectImplementation(TreeNode node, TreeViewEventArgs e)
        {
            newtonsoftJsonTypeTextBox.Text = "";

            jsonTypeComboBox.Text = JTokenType.Undefined.ToString();

            jsonValueTextBox.ReadOnly = true;
        }

        private void JsonTreeView_AfterSelectImplementation(JTokenTreeNode node, TreeViewEventArgs e)
        {
            newtonsoftJsonTypeTextBox.Text = node.Tag.GetType().Name;

            jsonTypeComboBox.Text = node.JTokenTag.Type.ToString();

            // If jsonValueTextBox is focused then it triggers this event in the update process, so don't update it again ! (risk: infinite loop between events).
            if (!jsonValueTextBox.Focused)
            {
                jsonValueTextBox.Text = node.JTokenTag.ToString();
            }
        }

        private void JsonTreeView_AfterSelectImplementation(JValueTreeNode node, TreeViewEventArgs e)
        {
            newtonsoftJsonTypeTextBox.Text = node.Tag.GetType().Name;

            jsonTypeComboBox.Text = node.jValueTag.Type.ToString();

            switch (node.jValueTag.Type)
            {
                case JTokenType.String:
                    jsonValueTextBox.Text = "\"" + node.jValueTag.ToString() + "\"";
                    break;
                default:
                    jsonValueTextBox.Text = node.jValueTag.ToString();
                    break;
            }
        }

        #endregion
    }
}
