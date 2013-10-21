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
using ZTn.Json.Editor.Drawing;

namespace ZTn.Json.Editor
{
    public sealed partial class JsonEditorMainForm : Form
    {
        #region >> Fields

        JsonEditorSource jsonEditorItem;

        #endregion

        public static JsonEditorMainForm JsonEditorForm { get; private set; }

        #region >> Constructor

        public JsonEditorMainForm()
        {
            InitializeComponent();

            JsonEditorForm = this;

            jsonTypeComboBox.DataSource = Enum.GetValues(typeof(JTokenType));

            jsonTreeView.AfterCollapse += JsonTreeView_AfterCollapse;
            jsonTreeView.AfterExpand += JsonTreeView_AfterExpand;
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
                        jsonEditorItem = new JsonEditorSource(stream);
                    }
                    else
                    {
                        jsonEditorItem = new JsonEditorSource();
                    }
                }

                jsonTreeView.Nodes.Clear();
                jsonTreeView.Nodes.Add(jsonEditorItem.RootTreeNode);
                foreach (TreeNode node in jsonTreeView.Nodes)
                {
                    node.Expand();
                }

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
                        new JsonEditorSource(((JTokenTreeNode)jsonTreeView.TopNode).JTokenTag).Save(stream);
                    }
                }
            }
        }

        private void newJsonObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jsonEditorItem = new JsonEditorSource("{}");

            jsonTreeView.Nodes.Clear();
            jsonTreeView.Nodes.Add(jsonEditorItem.RootTreeNode);
            foreach (TreeNode node in jsonTreeView.Nodes)
            {
                node.Expand();
            }
        }

        private void newJsonArrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jsonEditorItem = new JsonEditorSource("[]");

            jsonTreeView.Nodes.Clear();
            jsonTreeView.Nodes.Add(jsonEditorItem.RootTreeNode);
            foreach (TreeNode node in jsonTreeView.Nodes)
            {
                node.Expand();
            }
        }

        #region >> JsonTreeView_NodeMouseClick

        /// <summary>
        /// Main event handler dynamically dispatching the handling to specialized methods.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JsonTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            jsonTreeView.SelectedNode = e.Node;
            JsonTreeView_NodeMouseClick((dynamic)e.Node, e);
        }

        private void JsonTreeView_NodeMouseClick(TreeNode node, TreeNodeMouseClickEventArgs e)
        {
            newtonsoftJsonTypeTextBox.Text = "";

            jsonTypeComboBox.Text = "";

            jsonValueTextBox.ReadOnly = true;
        }

        private void JsonTreeView_NodeMouseClick(JArrayTreeNode node, TreeNodeMouseClickEventArgs e)
        {
            newtonsoftJsonTypeTextBox.Text = "JArray";

            jsonTypeComboBox.Text = node.jArrayTag.Type.ToString();

            jsonValueTextBox.Text = node.jArrayTag.ToString();
        }

        private void JsonTreeView_NodeMouseClick(JObjectTreeNode node, TreeNodeMouseClickEventArgs e)
        {
            newtonsoftJsonTypeTextBox.Text = "JObject";

            jsonTypeComboBox.Text = node.JObjectTag.Type.ToString();

            jsonValueTextBox.Text = node.JObjectTag.ToString();
        }

        private void JsonTreeView_NodeMouseClick(JPropertyTreeNode node, TreeNodeMouseClickEventArgs e)
        {
            newtonsoftJsonTypeTextBox.Text = "JProperty";

            jsonTypeComboBox.Text = node.JPropertyTag.Type.ToString();

            jsonValueTextBox.Text = node.JPropertyTag.ToString();
        }

        private void JsonTreeView_NodeMouseClick(JTokenTreeNode node, TreeNodeMouseClickEventArgs e)
        {
            newtonsoftJsonTypeTextBox.Text = "JToken";

            jsonTypeComboBox.Text = node.JTokenTag.Type.ToString();

            jsonValueTextBox.Text = node.JTokenTag.ToString();
        }

        private void JsonTreeView_NodeMouseClick(JValueTreeNode node, TreeNodeMouseClickEventArgs e)
        {
            newtonsoftJsonTypeTextBox.Text = "JValue";

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

        private void JsonTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            IJsonTreeNode node = e.Node as IJsonTreeNode;
            if (node != null)
            {
                node.UpdateWhenCollapsing();
            }
        }

        private void JsonTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            IJsonTreeNode node = e.Node as IJsonTreeNode;
            if (node != null)
            {
                node.UpdateWhenExpanding();
            }
        }

        private void JsonValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!jsonValueTextBox.ReadOnly)
            {
                IJsonTreeNode node = jsonTreeView.SelectedNode as IJsonTreeNode;
                if (node != null)
                {
                    jsonTreeView.SuspendLayout();
                    jsonTreeView.SelectedNode = node.UpdateWhenEditing(jsonValueTextBox.Text);
                    jsonTreeView.ResumeLayout();
                }
            }
        }

        private void jsonValueTextBox_Leave(object sender, EventArgs e)
        {
            jsonValueTextBox.TextChanged -= JsonValueTextBox_TextChanged;
        }

        private void jsonValueTextBox_Enter(object sender, EventArgs e)
        {
            jsonValueTextBox.TextChanged += JsonValueTextBox_TextChanged;

        }
    }
}
