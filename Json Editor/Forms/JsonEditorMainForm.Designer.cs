namespace ZTn.Json.Editor.Forms
{
    partial class JsonEditorMainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JsonEditorMainForm));
            this.formMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newJsonObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newJsonArrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jsonDataTabControl = new System.Windows.Forms.TabControl();
            this.jsonTreeTabPage = new System.Windows.Forms.TabPage();
            this.jsonTreeViewSplitContainer = new System.Windows.Forms.SplitContainer();
            this.jsonTreeView = new System.Windows.Forms.TreeView();
            this.jsonTypeComboBox = new System.Windows.Forms.ComboBox();
            this.jsonValueTextBox = new System.Windows.Forms.TextBox();
            this.jsonValueLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.newtonsoftJsonTypeTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutJsonEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formMenuStrip.SuspendLayout();
            this.jsonDataTabControl.SuspendLayout();
            this.jsonTreeTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jsonTreeViewSplitContainer)).BeginInit();
            this.jsonTreeViewSplitContainer.Panel1.SuspendLayout();
            this.jsonTreeViewSplitContainer.Panel2.SuspendLayout();
            this.jsonTreeViewSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // formMenuStrip
            // 
            this.formMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.formMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.formMenuStrip.Name = "formMenuStrip";
            this.formMenuStrip.Size = new System.Drawing.Size(1008, 24);
            this.formMenuStrip.TabIndex = 0;
            this.formMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newJsonObjectToolStripMenuItem,
            this.newJsonArrayToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // newJsonObjectToolStripMenuItem
            // 
            this.newJsonObjectToolStripMenuItem.Name = "newJsonObjectToolStripMenuItem";
            this.newJsonObjectToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.newJsonObjectToolStripMenuItem.Text = "New Json Object";
            this.newJsonObjectToolStripMenuItem.Click += new System.EventHandler(this.newJsonObjectToolStripMenuItem_Click);
            // 
            // newJsonArrayToolStripMenuItem
            // 
            this.newJsonArrayToolStripMenuItem.Name = "newJsonArrayToolStripMenuItem";
            this.newJsonArrayToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.newJsonArrayToolStripMenuItem.Text = "New Json Array";
            this.newJsonArrayToolStripMenuItem.Click += new System.EventHandler(this.newJsonArrayToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // jsonDataTabControl
            // 
            this.jsonDataTabControl.Controls.Add(this.jsonTreeTabPage);
            this.jsonDataTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jsonDataTabControl.Location = new System.Drawing.Point(0, 24);
            this.jsonDataTabControl.Name = "jsonDataTabControl";
            this.jsonDataTabControl.SelectedIndex = 0;
            this.jsonDataTabControl.Size = new System.Drawing.Size(1008, 577);
            this.jsonDataTabControl.TabIndex = 1;
            // 
            // jsonTreeTabPage
            // 
            this.jsonTreeTabPage.Controls.Add(this.jsonTreeViewSplitContainer);
            this.jsonTreeTabPage.Location = new System.Drawing.Point(4, 22);
            this.jsonTreeTabPage.Name = "jsonTreeTabPage";
            this.jsonTreeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.jsonTreeTabPage.Size = new System.Drawing.Size(1000, 551);
            this.jsonTreeTabPage.TabIndex = 0;
            this.jsonTreeTabPage.Text = "Json Tree View";
            this.jsonTreeTabPage.UseVisualStyleBackColor = true;
            // 
            // jsonTreeViewSplitContainer
            // 
            this.jsonTreeViewSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jsonTreeViewSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.jsonTreeViewSplitContainer.Name = "jsonTreeViewSplitContainer";
            // 
            // jsonTreeViewSplitContainer.Panel1
            // 
            this.jsonTreeViewSplitContainer.Panel1.Controls.Add(this.jsonTreeView);
            this.jsonTreeViewSplitContainer.Panel1MinSize = 200;
            // 
            // jsonTreeViewSplitContainer.Panel2
            // 
            this.jsonTreeViewSplitContainer.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.jsonTreeViewSplitContainer.Panel2.Controls.Add(this.jsonTypeComboBox);
            this.jsonTreeViewSplitContainer.Panel2.Controls.Add(this.jsonValueTextBox);
            this.jsonTreeViewSplitContainer.Panel2.Controls.Add(this.jsonValueLabel);
            this.jsonTreeViewSplitContainer.Panel2.Controls.Add(this.label2);
            this.jsonTreeViewSplitContainer.Panel2.Controls.Add(this.newtonsoftJsonTypeTextBox);
            this.jsonTreeViewSplitContainer.Panel2.Controls.Add(this.label1);
            this.jsonTreeViewSplitContainer.Panel2MinSize = 320;
            this.jsonTreeViewSplitContainer.Size = new System.Drawing.Size(1000, 551);
            this.jsonTreeViewSplitContainer.SplitterDistance = 667;
            this.jsonTreeViewSplitContainer.TabIndex = 8;
            // 
            // jsonTreeView
            // 
            this.jsonTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jsonTreeView.Location = new System.Drawing.Point(0, 0);
            this.jsonTreeView.Name = "jsonTreeView";
            this.jsonTreeView.Size = new System.Drawing.Size(661, 545);
            this.jsonTreeView.TabIndex = 0;
            this.jsonTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.jsonTreeView_AfterSelect);
            this.jsonTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.jsonTreeView_NodeMouseClick);
            // 
            // jsonTypeComboBox
            // 
            this.jsonTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.jsonTypeComboBox.Enabled = false;
            this.jsonTypeComboBox.FormattingEnabled = true;
            this.jsonTypeComboBox.Location = new System.Drawing.Point(3, 59);
            this.jsonTypeComboBox.Name = "jsonTypeComboBox";
            this.jsonTypeComboBox.Size = new System.Drawing.Size(154, 21);
            this.jsonTypeComboBox.TabIndex = 7;
            // 
            // jsonValueTextBox
            // 
            this.jsonValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jsonValueTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.jsonValueTextBox.Location = new System.Drawing.Point(3, 97);
            this.jsonValueTextBox.Multiline = true;
            this.jsonValueTextBox.Name = "jsonValueTextBox";
            this.jsonValueTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.jsonValueTextBox.Size = new System.Drawing.Size(320, 448);
            this.jsonValueTextBox.TabIndex = 6;
            this.jsonValueTextBox.Enter += new System.EventHandler(this.jsonValueTextBox_Enter);
            this.jsonValueTextBox.Leave += new System.EventHandler(this.jsonValueTextBox_Leave);
            // 
            // jsonValueLabel
            // 
            this.jsonValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.jsonValueLabel.AutoSize = true;
            this.jsonValueLabel.Location = new System.Drawing.Point(3, 81);
            this.jsonValueLabel.Name = "jsonValueLabel";
            this.jsonValueLabel.Size = new System.Drawing.Size(65, 13);
            this.jsonValueLabel.TabIndex = 5;
            this.jsonValueLabel.Text = "JSON Value";
            this.jsonValueLabel.TextChanged += new System.EventHandler(this.jsonValueTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "NewtonSoft.Json Type";
            // 
            // newtonsoftJsonTypeTextBox
            // 
            this.newtonsoftJsonTypeTextBox.Location = new System.Drawing.Point(3, 19);
            this.newtonsoftJsonTypeTextBox.Name = "newtonsoftJsonTypeTextBox";
            this.newtonsoftJsonTypeTextBox.ReadOnly = true;
            this.newtonsoftJsonTypeTextBox.Size = new System.Drawing.Size(154, 20);
            this.newtonsoftJsonTypeTextBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "JSON Type";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutJsonEditorToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(24, 20);
            this.aboutToolStripMenuItem.Text = "?";
            // 
            // aboutJsonEditorToolStripMenuItem
            // 
            this.aboutJsonEditorToolStripMenuItem.Name = "aboutJsonEditorToolStripMenuItem";
            this.aboutJsonEditorToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.aboutJsonEditorToolStripMenuItem.Text = "About Json Editor";
            this.aboutJsonEditorToolStripMenuItem.Click += new System.EventHandler(this.aboutJsonEditorToolStripMenuItem_Click);
            // 
            // JsonEditorMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.jsonDataTabControl);
            this.Controls.Add(this.formMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.formMenuStrip;
            this.Name = "JsonEditorMainForm";
            this.Text = "ZTn Json Editor";
            this.formMenuStrip.ResumeLayout(false);
            this.formMenuStrip.PerformLayout();
            this.jsonDataTabControl.ResumeLayout(false);
            this.jsonTreeTabPage.ResumeLayout(false);
            this.jsonTreeViewSplitContainer.Panel1.ResumeLayout(false);
            this.jsonTreeViewSplitContainer.Panel2.ResumeLayout(false);
            this.jsonTreeViewSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jsonTreeViewSplitContainer)).EndInit();
            this.jsonTreeViewSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip formMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.TabControl jsonDataTabControl;
        private System.Windows.Forms.TabPage jsonTreeTabPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox newtonsoftJsonTypeTextBox;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label jsonValueLabel;
        private System.Windows.Forms.SplitContainer jsonTreeViewSplitContainer;
        private System.Windows.Forms.TextBox jsonValueTextBox;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newJsonObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newJsonArrayToolStripMenuItem;
        private System.Windows.Forms.ComboBox jsonTypeComboBox;
        public System.Windows.Forms.TreeView jsonTreeView;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutJsonEditorToolStripMenuItem;
    }
}

