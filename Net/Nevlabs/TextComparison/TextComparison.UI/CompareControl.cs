using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

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

        private readonly Color _noChangedColor = SystemColors.Window;
        private readonly Color _removedColor = Color.MistyRose;
        private readonly Color _replacedColor = Color.MistyRose;
        private readonly Color _addedColor = Color.LightGreen;
        private readonly Color _lightGrayColor = SystemColors.ActiveCaption;

        public void Initialize(TextFile primaryFile, TextFile secondaryFile, IList<Modification> modifications)
        {
            primaryListView.Items.Clear();
            secondaryListView.Items.Clear();
            primaryTextBox.Text = primaryFile.Name;
            secondaryTextBox.Text = secondaryFile.Name;

            int rowNumber = 1;

            //Font strikeoutFont = new Font(Font, FontStyle.Strikeout);

            foreach (var modification in modifications)
            {
                ListViewItem primaryItem;
                ListViewItem secondaryItem;
                int index;

                switch (modification.Type)
                {
                    case ModificationType.Removed:
                        for (index = 0; index < modification.Length; index++)
                        {
                            primaryItem = new ListViewItem(rowNumber.ToString("00000"));
                            secondaryItem = new ListViewItem(rowNumber.ToString("00000"));
                            primaryItem.BackColor = _removedColor;
                            primaryItem.SubItems.Add(primaryFile[modification.PrimaryIndex + index].Line);
                            secondaryItem.BackColor = _lightGrayColor;

                            primaryListView.Items.Add(primaryItem);
                            secondaryListView.Items.Add(secondaryItem);
                            rowNumber++;
                        }
                        break;

                    case ModificationType.NoChanged:
                        for (index = 0; index < modification.Length; index++)
                        {
                            primaryItem = new ListViewItem(rowNumber.ToString("00000"));
                            secondaryItem = new ListViewItem(rowNumber.ToString("00000"));
                            primaryItem.BackColor = _noChangedColor;
                            primaryItem.SubItems.Add(primaryFile[modification.PrimaryIndex + index].Line);
                            secondaryItem.BackColor = _noChangedColor;
                            secondaryItem.SubItems.Add(secondaryFile[modification.SecondaryIndex + index].Line);

                            primaryListView.Items.Add(primaryItem);
                            secondaryListView.Items.Add(secondaryItem);
                            rowNumber++;
                        }
                        break;

                    case ModificationType.Added:
                        for (index = 0; index < modification.Length; index++)
                        {
                            primaryItem = new ListViewItem(rowNumber.ToString("00000"));
                            secondaryItem = new ListViewItem(rowNumber.ToString("00000"));
                            primaryItem.BackColor = _lightGrayColor;
                            primaryItem.SubItems.Add("");
                            secondaryItem.BackColor = _addedColor;
                            secondaryItem.SubItems.Add(secondaryFile[modification.SecondaryIndex + index].Line);

                            primaryListView.Items.Add(primaryItem);
                            secondaryListView.Items.Add(secondaryItem);
                            rowNumber++;
                        }
                        break;

                    case ModificationType.Replaced:
                        for (index = 0; index < modification.Length; index++)
                        {
                            primaryItem = new ListViewItem(rowNumber.ToString("00000"));
                            secondaryItem = new ListViewItem(rowNumber.ToString("00000"));
                            primaryItem.BackColor = _replacedColor;
                            //primaryItem.Font = strikeoutFont;
                            primaryItem.SubItems.Add(primaryFile[modification.PrimaryIndex + index].Line);
                            secondaryItem.BackColor = _addedColor;
                            secondaryItem.SubItems.Add(secondaryFile[modification.SecondaryIndex + index].Line);

                            primaryListView.Items.Add(primaryItem);
                            secondaryListView.Items.Add(secondaryItem);
                            rowNumber++;
                        }
                        break;
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

        private void SynchronizeCursor(ListView first, ListView second)
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