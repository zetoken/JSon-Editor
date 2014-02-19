using System.Drawing;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZTn.Json.Editor.Linq;

namespace ZTn.Json.Editor.Forms
{
    public sealed partial class JsonEditorMainForm : Form
    {
        private const string DefaultFileFilters = @"json files (*.json)|*.json";

        #region >> Fields

        private JTokenRoot jsonEditorItem;

        private string internalOpenedFileName;

        #endregion

        #region >> Properties

        /// <summary>
        /// Accessor to file name of opened file.
        /// </summary>
        string OpenedFileName
        {
            get { return internalOpenedFileName; }
            set
            {
                internalOpenedFileName = value;
                saveToolStripMenuItem.Enabled = internalOpenedFileName != null;
                Text = (internalOpenedFileName ?? "") + @" - Json Editor by ZTn";
            }
        }

        #endregion

        #region >> Constructor

        public JsonEditorMainForm()
        {
            InitializeComponent();

            jsonTypeComboBox.DataSource = Enum.GetValues(typeof(JTokenType));

            jsonTreeView.AfterCollapse += jsonTreeView_AfterCollapse;
            jsonTreeView.AfterExpand += jsonTreeView_AfterExpand;

            OpenedFileName = null;
            SetActionStatus(@"Empty document.", true);


            var commandLineArgs = Environment.GetCommandLineArgs();
            if (commandLineArgs.Skip(1).Any())
            {
                OpenedFileName = commandLineArgs[1];
                try
                {
                    using (var stream = new FileStream(commandLineArgs[1], FileMode.Open))
                    {
                        SetJsonSourceStream(stream, commandLineArgs[1]);
                    }
                }
                catch
                {
                    OpenedFileName = null;
                }
            }
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
            var openFileDialog = new OpenFileDialog
            {
                Filter = @"json files (*.json)|*.json",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var stream = openFileDialog.OpenFile())
                {
                    SetJsonSourceStream(stream, openFileDialog.FileName);
                }

            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenedFileName == null)
            {
                return;
            }

            try
            {
                using (var stream = new FileStream(OpenedFileName, FileMode.Open))
                {
                    jsonEditorItem.Save(stream);
                }
            }
            catch
            {
                MessageBox.Show(this, string.Format("An error occured when saving file as \"{0}\".", OpenedFileName), @"Save As...");

                OpenedFileName = null;
                SetActionStatus(@"Document NOT saved.", true);

                return;
            }

            SetActionStatus(@"Document successfully saved.", false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = DefaultFileFilters,
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                OpenedFileName = saveFileDialog.FileName;
                using (var stream = saveFileDialog.OpenFile())
                {
                    if (stream.CanWrite)
                    {
                        jsonEditorItem.Save(stream);
                    }
                }
            }
            catch
            {
                MessageBox.Show(this, string.Format("An error occured when saving file as \"{0}\".", OpenedFileName), @"Save As...");

                OpenedFileName = null;
                SetActionStatus(@"Document NOT saved.", true);

                return;
            }

            SetActionStatus(@"Document successfully saved.", false);
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
            var node = e.Node as IJsonTreeNode;
            if (node != null)
            {
                node.AfterCollapse();
            }
        }

        private void jsonTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var node = e.Node as IJsonTreeNode;
            if (node != null)
            {
                node.AfterExpand();
            }
        }

        private void jsonValueTextBox_TextChanged(object sender, EventArgs e)
        {
            var node = jsonTreeView.SelectedNode as IJsonTreeNode;
            if (node == null)
            {
                return;
            }

            jsonTreeView.BeginUpdate();
            jsonTreeView.SelectedNode = node.AfterJsonTextChange(jsonValueTextBox.Text);
            jsonTreeView.EndUpdate();
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
        // ReSharper disable once UnusedParameter.Local
        private void JsonTreeView_AfterSelectImplementation(TreeNode node, TreeViewEventArgs e)
        {
            newtonsoftJsonTypeTextBox.Text = "";

            jsonTypeComboBox.Text = String.Format("{0}: {1}", JTokenType.Undefined, node.GetType().FullName);

            jsonValueTextBox.ReadOnly = true;
        }

        // ReSharper disable once UnusedParameter.Local
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

        // ReSharper disable once UnusedParameter.Local
        private void JsonTreeView_AfterSelectImplementation(JValueTreeNode node, TreeViewEventArgs e)
        {
            newtonsoftJsonTypeTextBox.Text = node.Tag.GetType().Name;

            jsonTypeComboBox.Text = node.JValueTag.Type.ToString();

            switch (node.JValueTag.Type)
            {
                case JTokenType.String:
                    jsonValueTextBox.Text = @"""" + node.JValueTag + @"""";
                    break;
                default:
                    jsonValueTextBox.Text = node.JValueTag.ToString();
                    break;
            }
        }

        #endregion

        private void SetJsonSourceStream(Stream stream, string fileName)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            OpenedFileName = fileName;

            try
            {
                jsonEditorItem = new JTokenRoot(stream);
            }
            catch
            {
                MessageBox.Show(this, string.Format("An error occured when reading \"{0}\"", OpenedFileName), @"Open...");

                OpenedFileName = null;
                SetActionStatus(@"Document NOT loaded.", true);

                return;
            }

            SetActionStatus(@"Document successfully loaded.", false);

            jsonTreeView.Nodes.Clear();
            jsonTreeView.Nodes.Add(JsonTreeNodeFactory.Create(jsonEditorItem.JTokenValue));
            jsonTreeView.Nodes
                .Cast<TreeNode>()
                .ForEach(n => n.Expand());
        }

        private void SetActionStatus(string text, bool isError)
        {
            actionStatusLabel.Text = text;
            actionStatusLabel.ForeColor = isError ? Color.OrangeRed : Color.Black;
        }
    }
}
