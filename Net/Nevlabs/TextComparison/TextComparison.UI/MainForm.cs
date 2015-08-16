using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace TextComparison.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            _merger = new Merger();
            _merger.StateChanged += MergerStateChanged;

            InitializeComponent();
        }

        private void MergerStateChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private readonly Merger _merger;
        private void MergePanelResize(object sender, EventArgs e)
        {
        }

        private void UpdateControls()
        {
            compareControl1.Initialize(_merger.ServerFile, _merger.User1File, _merger.ServerUser1Modifications);
            compareControl2.Initialize(_merger.ServerFile, _merger.User2File, _merger.ServerUser2Modifications);
        }

        private string GetFileName()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            openFileDialog.InitialDirectory = location;
            openFileDialog.Filter = @"All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : null;
        }

        private void OpenServerClick(object sender, EventArgs e)
        {
            string filename = GetFileName();

            if (!File.Exists(filename)) return;

            _merger.ServerFile.Load(filename);
            _merger.ExecuteComapare();
        }

        private void OpenUser1Click(object sender, EventArgs e)
        {
            string filename = GetFileName();

            if (!File.Exists(filename)) return;

            _merger.User1File.Load(filename);
            _merger.ExecuteComapare();
        }

        private void OpenUser2Click(object sender, EventArgs e)
        {
            string filename = GetFileName();

            if (!File.Exists(filename)) return;

            _merger.User2File.Load(filename);
            _merger.ExecuteComapare();
        }

        private void MainFormResize(object sender, EventArgs e)
        {
            int height = (ClientRectangle.Height - splitter1.Height - splitter2.Height)/3;

            compareControl1.Height = height;
            compareControl2.Height = height;
        }

        private void MergeButtonClick(object sender, EventArgs e)
        {
            _merger.ExecuteMerge();
        }
    }
}