//Add task dialog

namespace Chatbot
{
  
    public class AddTaskForm : Form
    {
        // GUI controls for the dialog
        private TextBox titleBox;
        private TextBox descriptionBox;
        private CheckBox reminderCheck;
        private DateTimePicker reminderPicker;
        private Button okButton;
        private Button cancelButton;

       
        public TaskItem? CreatedTask = null;

        public AddTaskForm()
        {
            // Dialog window settings
            this.Text = "Add a Cybersecurity Task";
            this.Size = new Size(440, 380);
            this.BackColor = Color.FromArgb(15, 15, 25);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Title label and box
            this.Controls.Add(MakeLabel("Task title:", 20, 20));

            titleBox = new TextBox();
            titleBox.Location = new Point(20, 45);
            titleBox.Size = new Size(380, 28);
            titleBox.Font = new Font("Segoe UI", 11);
            titleBox.BackColor = Color.FromArgb(25, 25, 40);
            titleBox.ForeColor = Color.White;
            titleBox.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(titleBox);

            // Description label and box
            this.Controls.Add(MakeLabel("Description:", 20, 85));

            descriptionBox = new TextBox();
            descriptionBox.Location = new Point(20, 110);
            descriptionBox.Size = new Size(380, 80);
            descriptionBox.Multiline = true;
            descriptionBox.Font = new Font("Segoe UI", 11);
            descriptionBox.BackColor = Color.FromArgb(25, 25, 40);
            descriptionBox.ForeColor = Color.White;
            descriptionBox.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(descriptionBox);

            
            reminderCheck = new CheckBox();
            reminderCheck.Text = "Remind me on:";
            reminderCheck.Location = new Point(20, 205);
            reminderCheck.Size = new Size(130, 28);
            reminderCheck.ForeColor = Color.Cyan;
            reminderCheck.Font = new Font("Segoe UI", 11);
            // Only let the date picker be used when the box is ticked
            reminderCheck.CheckedChanged += (s, e) => reminderPicker.Enabled = reminderCheck.Checked;
            this.Controls.Add(reminderCheck);

            // Date and time picker - starts a week ahead as a sensible default
            reminderPicker = new DateTimePicker();
            reminderPicker.Location = new Point(150, 207);
            reminderPicker.Size = new Size(250, 28);
            reminderPicker.Format = DateTimePickerFormat.Custom;
            reminderPicker.CustomFormat = "dd MMMM yyyy  HH:mm";
            reminderPicker.Value = DateTime.Now.AddDays(7);
            reminderPicker.Enabled = false;
            this.Controls.Add(reminderPicker);

            // OK button - builds and saves the task
            okButton = MakeButton("Add Task", 150, 280);
            okButton.Click += OkButton_Click;
            this.Controls.Add(okButton);

            // Cancel button - closes the dialog without saving anything
            cancelButton = MakeButton("Cancel", 290, 280);
            cancelButton.BackColor = Color.FromArgb(60, 60, 75);
            cancelButton.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
            this.Controls.Add(cancelButton);
        }

       
        private void OkButton_Click(object? sender, EventArgs e)
        {
            // Make sure there is at least a title before saving
            if (string.IsNullOrWhiteSpace(titleBox.Text))
            {
                MessageBox.Show("Please enter a task title.", "Missing title",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CreatedTask = new TaskItem();
            CreatedTask.Title = titleBox.Text.Trim();
            // If no description was typed, fall back to the title so it is never empty
            CreatedTask.Description = string.IsNullOrWhiteSpace(descriptionBox.Text)
                ? titleBox.Text.Trim()
                : descriptionBox.Text.Trim();
            // Only attach a reminder if the user ticked the box
            CreatedTask.ReminderDate = reminderCheck.Checked ? reminderPicker.Value : (DateTime?)null;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Helper to make a label in the right style so we don't repeat ourselves
        private Label MakeLabel(string text, int x, int y)
        {
            var label = new Label();
            label.Text = text;
            label.Location = new Point(x, y);
            label.Size = new Size(200, 22);
            label.ForeColor = Color.Cyan;
            label.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            return label;
        }

        // Helper to make a button in the right style to match the rest of the app
        private Button MakeButton(string text, int x, int y)
        {
            var button = new Button();
            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(120, 38);
            button.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            button.BackColor = Color.FromArgb(0, 120, 150);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            return button;
        }
    }
}