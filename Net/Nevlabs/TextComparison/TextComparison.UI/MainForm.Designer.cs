using System.Windows.Forms;

namespace TextComparison.UI
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mergePanel = new System.Windows.Forms.Panel();
            this.compareControl3 = new TextComparison.UI.CompareControl();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.compareControl1 = new TextComparison.UI.CompareControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.openServerButton = new System.Windows.Forms.ToolStripButton();
            this.openUser1Button = new System.Windows.Forms.ToolStripButton();
            this.openUser2Button = new System.Windows.Forms.ToolStripButton();
            this.mergeButton = new System.Windows.Forms.ToolStripButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.compareControl2 = new TextComparison.UI.CompareControl();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.mergePanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mergePanel
            // 
            this.mergePanel.Controls.Add(this.compareControl3);
            this.mergePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mergePanel.Location = new System.Drawing.Point(0, 335);
            this.mergePanel.Name = "mergePanel";
            this.mergePanel.Size = new System.Drawing.Size(617, 148);
            this.mergePanel.TabIndex = 4;
            this.mergePanel.Resize += new System.EventHandler(this.MergePanelResize);
            // 
            // compareControl3
            // 
            this.compareControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compareControl3.Location = new System.Drawing.Point(0, 0);
            this.compareControl3.Name = "compareControl3";
            this.compareControl3.PrimaryHeader = "Server";
            this.compareControl3.SecondaryHeader = "Merged";
            this.compareControl3.Size = new System.Drawing.Size(617, 148);
            this.compareControl3.TabIndex = 9;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // compareControl1
            // 
            this.compareControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.compareControl1.Location = new System.Drawing.Point(0, 25);
            this.compareControl1.Name = "compareControl1";
            this.compareControl1.PrimaryHeader = "Server";
            this.compareControl1.SecondaryHeader = "User1";
            this.compareControl1.Size = new System.Drawing.Size(617, 152);
            this.compareControl1.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openServerButton,
            this.openUser1Button,
            this.openUser2Button,
            this.mergeButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(617, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // openServerButton
            // 
            this.openServerButton.Image = ((System.Drawing.Image)(resources.GetObject("openServerButton.Image")));
            this.openServerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openServerButton.Name = "openServerButton";
            this.openServerButton.Size = new System.Drawing.Size(59, 22);
            this.openServerButton.Text = "Server";
            this.openServerButton.Click += new System.EventHandler(this.OpenServerClick);
            // 
            // openUser1Button
            // 
            this.openUser1Button.Image = ((System.Drawing.Image)(resources.GetObject("openUser1Button.Image")));
            this.openUser1Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openUser1Button.Name = "openUser1Button";
            this.openUser1Button.Size = new System.Drawing.Size(56, 22);
            this.openUser1Button.Text = "User1";
            this.openUser1Button.Click += new System.EventHandler(this.OpenUser1Click);
            // 
            // openUser2Button
            // 
            this.openUser2Button.Image = ((System.Drawing.Image)(resources.GetObject("openUser2Button.Image")));
            this.openUser2Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openUser2Button.Name = "openUser2Button";
            this.openUser2Button.Size = new System.Drawing.Size(56, 22);
            this.openUser2Button.Text = "User2";
            this.openUser2Button.Click += new System.EventHandler(this.OpenUser2Click);
            // 
            // mergeButton
            // 
            this.mergeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.mergeButton.Image = ((System.Drawing.Image)(resources.GetObject("mergeButton.Image")));
            this.mergeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mergeButton.Name = "mergeButton";
            this.mergeButton.Size = new System.Drawing.Size(74, 22);
            this.mergeButton.Text = "Auto Merge";
            this.mergeButton.Click += new System.EventHandler(this.MergeButtonClick);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 177);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(617, 3);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // compareControl2
            // 
            this.compareControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.compareControl2.Location = new System.Drawing.Point(0, 180);
            this.compareControl2.Name = "compareControl2";
            this.compareControl2.PrimaryHeader = "Server";
            this.compareControl2.SecondaryHeader = "User2";
            this.compareControl2.Size = new System.Drawing.Size(617, 152);
            this.compareControl2.TabIndex = 8;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter2.Location = new System.Drawing.Point(0, 332);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(617, 3);
            this.splitter2.TabIndex = 9;
            this.splitter2.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(617, 483);
            this.Controls.Add(this.mergePanel);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.compareControl2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.compareControl1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Resize += new System.EventHandler(this.MainFormResize);
            this.mergePanel.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel mergePanel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private CompareControl compareControl1;
        private ToolStrip toolStrip1;
        private ToolStripButton openServerButton;
        private ToolStripButton openUser1Button;
        private ToolStripButton openUser2Button;
        private Splitter splitter1;
        private CompareControl compareControl2;
        private Splitter splitter2;
        private ToolStripButton mergeButton;
        private CompareControl compareControl3;
    }
}

