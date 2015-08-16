namespace TextComparison.UI
{
    partial class CompareControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.primaryPanel = new System.Windows.Forms.Panel();
            this.primaryListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel4 = new System.Windows.Forms.Panel();
            this.primaryLabel = new System.Windows.Forms.Label();
            this.primaryTextBox = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.secondaryPanel = new System.Windows.Forms.Panel();
            this.secondaryListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel5 = new System.Windows.Forms.Panel();
            this.secondaryLabel = new System.Windows.Forms.Label();
            this.secondaryTextBox = new System.Windows.Forms.TextBox();
            this.primaryPanel.SuspendLayout();
            this.panel4.SuspendLayout();
            this.secondaryPanel.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // primaryPanel
            // 
            this.primaryPanel.Controls.Add(this.primaryListView);
            this.primaryPanel.Controls.Add(this.panel4);
            this.primaryPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.primaryPanel.Location = new System.Drawing.Point(0, 0);
            this.primaryPanel.Name = "primaryPanel";
            this.primaryPanel.Size = new System.Drawing.Size(200, 197);
            this.primaryPanel.TabIndex = 3;
            this.primaryPanel.Resize += new System.EventHandler(this.PrimaryPanelResize);
            // 
            // primaryListView
            // 
            this.primaryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.primaryListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.primaryListView.FullRowSelect = true;
            this.primaryListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.primaryListView.HideSelection = false;
            this.primaryListView.Location = new System.Drawing.Point(0, 45);
            this.primaryListView.MultiSelect = false;
            this.primaryListView.Name = "primaryListView";
            this.primaryListView.Size = new System.Drawing.Size(200, 152);
            this.primaryListView.TabIndex = 1;
            this.primaryListView.UseCompatibleStateImageBehavior = false;
            this.primaryListView.View = System.Windows.Forms.View.Details;
            this.primaryListView.SelectedIndexChanged += new System.EventHandler(this.PrimaryListViewSelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "#";
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Текст";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.primaryLabel);
            this.panel4.Controls.Add(this.primaryTextBox);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(200, 45);
            this.panel4.TabIndex = 0;
            // 
            // primaryLabel
            // 
            this.primaryLabel.AutoSize = true;
            this.primaryLabel.Location = new System.Drawing.Point(3, 3);
            this.primaryLabel.Name = "primaryLabel";
            this.primaryLabel.Size = new System.Drawing.Size(41, 13);
            this.primaryLabel.TabIndex = 1;
            this.primaryLabel.Text = "Primary";
            // 
            // primaryTextBox
            // 
            this.primaryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.primaryTextBox.Location = new System.Drawing.Point(3, 20);
            this.primaryTextBox.Name = "primaryTextBox";
            this.primaryTextBox.ReadOnly = true;
            this.primaryTextBox.Size = new System.Drawing.Size(194, 20);
            this.primaryTextBox.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(200, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 197);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // secondaryPanel
            // 
            this.secondaryPanel.Controls.Add(this.secondaryListView);
            this.secondaryPanel.Controls.Add(this.panel5);
            this.secondaryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.secondaryPanel.Location = new System.Drawing.Point(203, 0);
            this.secondaryPanel.Name = "secondaryPanel";
            this.secondaryPanel.Size = new System.Drawing.Size(202, 197);
            this.secondaryPanel.TabIndex = 6;
            this.secondaryPanel.Resize += new System.EventHandler(this.SecondaryPanelResize);
            // 
            // secondaryListView
            // 
            this.secondaryListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.secondaryListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.secondaryListView.FullRowSelect = true;
            this.secondaryListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.secondaryListView.HideSelection = false;
            this.secondaryListView.Location = new System.Drawing.Point(0, 45);
            this.secondaryListView.MultiSelect = false;
            this.secondaryListView.Name = "secondaryListView";
            this.secondaryListView.Size = new System.Drawing.Size(202, 152);
            this.secondaryListView.TabIndex = 2;
            this.secondaryListView.UseCompatibleStateImageBehavior = false;
            this.secondaryListView.View = System.Windows.Forms.View.Details;
            this.secondaryListView.SelectedIndexChanged += new System.EventHandler(this.SecondaryListViewSelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "#";
            this.columnHeader3.Width = 50;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Текст";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.secondaryLabel);
            this.panel5.Controls.Add(this.secondaryTextBox);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(202, 45);
            this.panel5.TabIndex = 1;
            // 
            // secondaryLabel
            // 
            this.secondaryLabel.AutoSize = true;
            this.secondaryLabel.Location = new System.Drawing.Point(3, 3);
            this.secondaryLabel.Name = "secondaryLabel";
            this.secondaryLabel.Size = new System.Drawing.Size(58, 13);
            this.secondaryLabel.TabIndex = 1;
            this.secondaryLabel.Text = "Secondary";
            // 
            // secondaryTextBox
            // 
            this.secondaryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.secondaryTextBox.Location = new System.Drawing.Point(3, 20);
            this.secondaryTextBox.Name = "secondaryTextBox";
            this.secondaryTextBox.ReadOnly = true;
            this.secondaryTextBox.Size = new System.Drawing.Size(196, 20);
            this.secondaryTextBox.TabIndex = 0;
            // 
            // CompareControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.secondaryPanel);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.primaryPanel);
            this.Name = "CompareControl";
            this.Size = new System.Drawing.Size(405, 197);
            this.Resize += new System.EventHandler(this.CompareControlResize);
            this.primaryPanel.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.secondaryPanel.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel primaryPanel;
        private System.Windows.Forms.ListView primaryListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label primaryLabel;
        private System.Windows.Forms.TextBox primaryTextBox;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel secondaryPanel;
        private System.Windows.Forms.ListView secondaryListView;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label secondaryLabel;
        private System.Windows.Forms.TextBox secondaryTextBox;
    }
}
