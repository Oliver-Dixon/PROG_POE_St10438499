namespace Chatbot
{
    public class ActivityLogForm : Form
    {
        private List<string> entries;
        private int shownCount = 0;
        private int pageSize = 5;

        private Label headerLabel = null!;
        private Label countLabel = null!;
        private ListBox logList = null!;
        private Button showMoreButton = null!;
        private Button closeButton = null!;

        public ActivityLogForm()
        {
            entries = ActivityLog.GetNewestFirst();

            this.Text = "Activity Log";
            this.Size = new Size(620, 560);
            this.MinimumSize = new Size(620, 560);
            this.BackColor = Color.FromArgb(15, 15, 25);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(20);

            BuildControls();
            ShowMore();
        }

        private void BuildControls()
        {
            headerLabel = new Label();
            headerLabel.Text = "Activity Log";
            headerLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            headerLabel.ForeColor = Color.Cyan;
            headerLabel.Location = new Point(20, 18);
            headerLabel.Size = new Size(560, 36);
            this.Controls.Add(headerLabel);

            countLabel = new Label();
            countLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            countLabel.ForeColor = Color.White;
            countLabel.Location = new Point(20, 58);
            countLabel.Size = new Size(560, 22);
            this.Controls.Add(countLabel);

            logList = new ListBox();
            logList.Location = new Point(20, 88);
            logList.Size = new Size(560, 360);
            logList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            logList.BackColor = Color.FromArgb(10, 10, 20);
            logList.ForeColor = Color.Cyan;
            logList.Font = new Font("Consolas", 11);
            logList.BorderStyle = BorderStyle.FixedSingle;
            logList.SelectionMode = SelectionMode.None;
            this.Controls.Add(logList);

            showMoreButton = new Button();
            showMoreButton.Text = "Show More";
            showMoreButton.Location = new Point(20, 462);
            showMoreButton.Size = new Size(150, 40);
            showMoreButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            showMoreButton.FlatStyle = FlatStyle.Flat;
            showMoreButton.FlatAppearance.BorderSize = 0;
            showMoreButton.BackColor = Color.FromArgb(0, 120, 150);
            showMoreButton.ForeColor = Color.White;
            showMoreButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            showMoreButton.Cursor = Cursors.Hand;
            showMoreButton.Click += (s, e) => ShowMore();
            this.Controls.Add(showMoreButton);

            closeButton = new Button();
            closeButton.Text = "Close";
            closeButton.Location = new Point(475, 462);
            closeButton.Size = new Size(105, 40);
            closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.BackColor = Color.FromArgb(60, 60, 75);
            closeButton.ForeColor = Color.White;
            closeButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            closeButton.Cursor = Cursors.Hand;
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);
        }

        private void ShowMore()
        {
            if (entries.Count == 0)
            {
                countLabel.Text = "No activity recorded yet.";
                showMoreButton.Enabled = false;
                return;
            }

            int next = Math.Min(shownCount + pageSize, entries.Count);
            for (int i = shownCount; i < next; i++)
                logList.Items.Add((i + 1) + ".  " + entries[i]);
            shownCount = next;

            countLabel.Text = "Showing " + shownCount + " of " + entries.Count + " actions (newest first)";
            showMoreButton.Enabled = shownCount < entries.Count;
        }
    }
}