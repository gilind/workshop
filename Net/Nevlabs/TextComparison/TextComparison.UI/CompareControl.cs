using System;
using System.Windows.Forms;
using TextComparison.Modifications;

namespace TextComparison.UI
{
    public partial class CompareControl : UserControl
    {
        public CompareControl()
        {
            InitializeComponent();
        }

        public string PrimaryHeader
        {
            get { return primaryLabel.Text; }
            set { primaryLabel.Text = value; }
        }

        public string SecondaryHeader
        {
            get { return secondaryLabel.Text; }
            set { secondaryLabel.Text = value; }
        }

        public void Initialize(ModificationCollection modifications)
        {
            primaryListView.Items.Clear();
            secondaryListView.Items.Clear();
            primaryTextBox.Text = modifications.Primary.Name;
            secondaryTextBox.Text = modifications.Secondary.Name;

            int rowNumber = 1;

            foreach (var modification in modifications)
            {
                for (int index = 0; index < modification.Length; index++)
                {
                    var primaryItem = new ListViewItem(rowNumber.ToString("00000"));
                    primaryItem.BackColor = modification.Primary.Color;

                    string primaryLine = string.Empty;
                    if (index < modification.Primary.Length - 1)
                    {
                        primaryLine = modification.Primary.Lines[index];
                    }

                    primaryItem.SubItems.Add(primaryLine);
                    primaryListView.Items.Add(primaryItem);

                    var secondaryItem = new ListViewItem(rowNumber.ToString("00000"));
                    secondaryItem.BackColor = modification.Secondary.Color;

                    string secondaryLine = string.Empty;
                    if (index < modification.Secondary.Length - 1)
                    {
                        secondaryLine = modification.Secondary.Lines[index];
                    }

                    secondaryItem.SubItems.Add(secondaryLine);
                    secondaryListView.Items.Add(secondaryItem);

                    rowNumber++;
                }
            }
        }

        private void CompareControlResize(object sender, EventArgs e)
        {
            int controlWidth = (ClientRectangle.Width - splitter1.Width) / 2;

            primaryPanel.Width = controlWidth;
            secondaryPanel.Width = controlWidth;
        }

        private void PrimaryPanelResize(object sender, EventArgs e)
        {
            primaryListView.Columns[1].Width = primaryListView.ClientRectangle.Width - primaryListView.Columns[0].Width;
        }

        private void SecondaryPanelResize(object sender, EventArgs e)
        {
            secondaryListView.Columns[1].Width = secondaryListView.ClientRectangle.Width - secondaryListView.Columns[0].Width;
        }

        private static void SynchronizeCursor(ListView first, ListView second)
        {
            if (first.SelectedIndices.Count > 0)
            {
                int index = first.SelectedIndices[0];

                if (index < second.Items.Count)
                {
                    second.SelectedIndices.Add(index);
                    second.EnsureVisible(index);
                }
            }
        }

        private void PrimaryListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            SynchronizeCursor(primaryListView, secondaryListView);
        }

        private void SecondaryListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            SynchronizeCursor(secondaryListView, primaryListView);
        }
    }
}