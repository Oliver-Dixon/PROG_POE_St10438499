//Main form

namespace Chatbot
{
    public class MainForm : Form
    {
        // Store the username for use throughout the chat
        private string userName = "";

        // Tracks if we are still waiting for the user to enter their name
        private bool waitingForName = true;

        // GUI controls
        private RichTextBox chatBox;
        private TextBox inputBox;
        private Button sendButton;
        private Panel headerPanel;
        private Label titleLabel;

        // --- Task assistant additions ---
        private bool databaseReady = false;               // Did MySQL connect successfully?
        private Panel tasksPanel = null!;                 // Right hand panel holding the task list
        private Label tasksTitle = null!;                 // Heading above the task grid
        private DataGridView taskGrid = null!;            // Shows the saved tasks in a table
        private Button addTaskButton = null!;             // Opens the add task dialog
        private Button completeButton = null!;            // Marks the selected task complete
        private Button deleteButton = null!;              // Deletes the selected task
        private Button refreshButton = null!;             // Reloads the list from the database
        private System.Windows.Forms.Timer reminderTimer = null!; // Checks for due reminders every minute

        // Layout numbers shared by the chat column and the tasks panel
        private int margin = 20;
        private int panelWidth = 360;
        private int gap = 15;

        // Form constructor
        public MainForm()
        {
            // Form settings
            this.Text = "CyberBot - Cybersecurity Assistant";
            // Wider than before to make room for the tasks panel on the right
            this.Size = new Size(1250, 720);
            this.MinimumSize = new Size(1250, 560);
            this.BackColor = Color.FromArgb(15, 15, 25);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Padding = new Padding(20);

            // Work out the width left for the chat once the panel has taken its space
            int chatWidth = this.ClientSize.Width - (margin * 2) - panelWidth - gap;

            // Header panel at the top with the title
            headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 60;
            headerPanel.BackColor = Color.FromArgb(0, 80, 100);
            this.Controls.Add(headerPanel);

            // Title label in the header
            titleLabel = new Label();
            titleLabel.Text = "ChatBot - Cybersecurity Assistant";
            titleLabel.Dock = DockStyle.Fill;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            titleLabel.ForeColor = Color.Cyan;
            headerPanel.Controls.Add(titleLabel);

            // Chat display area - sits in the left hand column
            // Anchored Top|Left|Bottom so it keeps its width and the panel can sit beside it
            chatBox = new RichTextBox();
            chatBox.Location = new Point(20, 90);
            chatBox.Size = new Size(chatWidth, this.ClientSize.Height - 175);
            chatBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            chatBox.BackColor = Color.FromArgb(10, 10, 20);
            chatBox.ForeColor = Color.Cyan;
            chatBox.Font = new Font("Consolas", 10);
            chatBox.BorderStyle = BorderStyle.FixedSingle;
            chatBox.ReadOnly = true;
            this.Controls.Add(chatBox);

            // Input box for the user to type in - sits under the chat column
            inputBox = new TextBox();
            inputBox.Location = new Point(20, this.ClientSize.Height - 70);
            inputBox.Size = new Size(chatWidth - 120, 35);
            inputBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            inputBox.Font = new Font("Consolas", 12);
            inputBox.BackColor = Color.FromArgb(25, 25, 40);
            inputBox.ForeColor = Color.White;
            inputBox.BorderStyle = BorderStyle.FixedSingle;
            // Allow Enter key to send
            inputBox.KeyDown += InputBox_KeyDown;
            this.Controls.Add(inputBox);

            // Send button on the right of the input box
            sendButton = new Button();
            sendButton.Location = new Point(20 + chatWidth - 105, this.ClientSize.Height - 72);
            sendButton.Size = new Size(105, 38);
            sendButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            sendButton.Text = "Send";
            sendButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            sendButton.BackColor = Color.FromArgb(0, 120, 150);
            sendButton.ForeColor = Color.White;
            sendButton.FlatStyle = FlatStyle.Flat;
            sendButton.FlatAppearance.BorderSize = 0;
            sendButton.Cursor = Cursors.Hand;
            sendButton.Click += SendButton_Click;
            this.Controls.Add(sendButton);

            // Build the tasks panel on the right and get the reminder timer ready
            SetUpTaskAssistant();

            // Run the start sequence when the form loads
            this.Load += MainForm_Load;
        }

        // Builds the tasks panel on the right: a title, the task grid and the action buttons
        private void SetUpTaskAssistant()
        {
            int topOfContent = 90;                              // line up with the chat box
            int panelHeight = this.ClientSize.Height - 175;     // same height as the chat box
            int panelLeft = this.ClientSize.Width - margin - panelWidth;

            // The panel itself
            tasksPanel = new Panel();
            tasksPanel.Location = new Point(panelLeft, topOfContent);
            tasksPanel.Size = new Size(panelWidth, panelHeight);
            tasksPanel.BackColor = Color.FromArgb(20, 20, 35);
            tasksPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            // Heading above the grid
            tasksTitle = new Label();
            tasksTitle.Text = "Your Cybersecurity Tasks";
            tasksTitle.ForeColor = Color.Cyan;
            tasksTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            tasksTitle.Location = new Point(10, 10);
            tasksTitle.Size = new Size(panelWidth - 20, 26);

            // The grid that lists the tasks
            taskGrid = new DataGridView();
            taskGrid.Location = new Point(10, 45);
            taskGrid.Size = new Size(panelWidth - 20, panelHeight - 130);
            taskGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            taskGrid.BackgroundColor = Color.FromArgb(15, 15, 25);
            taskGrid.ForeColor = Color.White;
            taskGrid.GridColor = Color.FromArgb(0, 80, 100);
            taskGrid.BorderStyle = BorderStyle.None;
            taskGrid.ReadOnly = true;
            taskGrid.AllowUserToAddRows = false;
            taskGrid.AllowUserToResizeRows = false;
            taskGrid.RowHeadersVisible = false;
            taskGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            taskGrid.MultiSelect = false;
            taskGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            taskGrid.EnableHeadersVisualStyles = false;
            taskGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 80, 100);
            taskGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            taskGrid.DefaultCellStyle.BackColor = Color.FromArgb(15, 15, 25);
            taskGrid.DefaultCellStyle.ForeColor = Color.White;
            taskGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 150);

            // Four columns: id, title, reminder and status
            taskGrid.Columns.Add("Id", "Id");
            taskGrid.Columns.Add("Title", "Task");
            taskGrid.Columns.Add("Reminder", "Reminder");
            taskGrid.Columns.Add("Status", "Status");
            taskGrid.Columns["Id"].FillWeight = 15;
            taskGrid.Columns["Title"].FillWeight = 45;
            taskGrid.Columns["Reminder"].FillWeight = 25;
            taskGrid.Columns["Status"].FillWeight = 20;

            // The action buttons, laid out two by two at the bottom of the panel
            addTaskButton  = MakeTaskButton("Add Task",  10,  panelHeight - 75);
            completeButton = MakeTaskButton("Complete", 185,  panelHeight - 75);
            deleteButton   = MakeTaskButton("Delete",    10,  panelHeight - 40);
            refreshButton  = MakeTaskButton("Refresh",  185,  panelHeight - 40);

            addTaskButton.Click  += AddTaskButton_Click;
            completeButton.Click += CompleteButton_Click;
            deleteButton.Click   += DeleteButton_Click;
            refreshButton.Click  += RefreshButton_Click;

            // Add everything to the panel, then the panel to the form
            tasksPanel.Controls.Add(tasksTitle);
            tasksPanel.Controls.Add(taskGrid);
            tasksPanel.Controls.Add(addTaskButton);
            tasksPanel.Controls.Add(completeButton);
            tasksPanel.Controls.Add(deleteButton);
            tasksPanel.Controls.Add(refreshButton);
            this.Controls.Add(tasksPanel);

            // The timer that quietly checks for due reminders once a minute
            reminderTimer = new System.Windows.Forms.Timer();
            reminderTimer.Interval = 60000;
            reminderTimer.Tick += ReminderTimer_Tick;
        }

        // Small helper so all the task buttons share the same cyber styling
        private Button MakeTaskButton(string text, int x, int y)
        {
            var button = new Button();
            button.Text = text;
            button.Location = new Point(x, y);
            button.Size = new Size(165, 30);
            button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.FromArgb(0, 80, 100);
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            return button;
        }

        // What happens when the form opens
        private void MainForm_Load(object? sender, EventArgs e)
        {
            // Display the greeting and ASCII art
            chatBox.AppendText(ChatbotFunctions.Greeting());

            // Get the task database ready (the chat still works even if this fails)
            try
            {
                DatabaseHelper.Initialise();
                databaseReady = true;
                RefreshTaskList();
            }
            catch
            {
                databaseReady = false;
            }

            // Start checking for due reminders from now on
            reminderTimer.Start();

            // Ask the user for their name in the chat
            chatBox.AppendText("ChatBot: What is your name?\n");

            // Set focus to the input box so the user can type straight away
            inputBox.Focus();
        }

        // Handles when the send button is clicked
        private void SendButton_Click(object? sender, EventArgs e)
        {
            ProcessInput();
        }

        // Handles when the Enter key is pressed inside the input box
        private void InputBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ProcessInput();
                e.SuppressKeyPress = true;
            }
        }

        // Reads input and shows the matching response
        // Wrapped in try catch so unexpected errors don't crash the program
        private void ProcessInput()
        {
            try
            {
                // Check if the user typed nothing
                if (string.IsNullOrWhiteSpace(inputBox.Text))
                {
                    chatBox.AppendText("\nChatBot: I didn't catch that. Please type something or pick a number from the menu.\n");
                    return;
                }

                // Store the input to compare later and trim any extra spaces
                string userInput = inputBox.Text.Trim();

                // If we are still waiting for the name, save it and show the menu
                if (waitingForName)
                {
                    userName = userInput;
                    waitingForName = false;

                    // Show the name back to the user
                    chatBox.AppendText("\n[" + userName + "]: " + userInput + "\n");

                    // Show welcome message and the help menu
                    chatBox.AppendText(ChatbotFunctions.Hello(userName));
                    chatBox.AppendText(ChatbotFunctions.Help());

                    // Let the user know about the task assistant, or show any reminders already due
                    if (!databaseReady)
                        chatBox.AppendText(TaskUnavailableMessage());
                    else
                        chatBox.AppendText(TaskManager.GetDueReminders());

                    // Clear the input box for next entry
                    inputBox.Clear();
                    ScrollToBottom();
                    return;
                }

                // Show what the user typed
                chatBox.AppendText("\n[" + userName + "]: " + userInput + "\n");

                // If we are part way through adding a task, keep that conversation going
                if (TaskManager.IsAddingTask())
                {
                    chatBox.AppendText(TaskManager.ContinueConversation(userInput));
                    RefreshTaskList();
                    inputBox.Clear();
                    ScrollToBottom();
                    return;
                }

                // Check which number the user entered
                if (userInput == "1")
                {
                    chatBox.AppendText(ChatbotFunctions.Help());
                }
                else if (userInput == "2")
                {
                    chatBox.AppendText(ChatbotFunctions.Purpose(userName));
                }
                else if (userInput == "3")
                {
                    chatBox.AppendText(ChatbotFunctions.HowAreYou(userName));
                }
                else if (userInput == "4")
                {
                    chatBox.AppendText(ChatbotFunctions.SafeBrowsing());
                }
                else if (userInput == "5")
                {
                    chatBox.AppendText(ChatbotFunctions.Phishing());
                }
                else if (userInput == "6")
                {
                    chatBox.AppendText(ChatbotFunctions.PasswordSafety());
                }
                else if (userInput == "7")
                {
                    // Show the task assistant menu and the current list of tasks
                    chatBox.AppendText(ChatbotFunctions.TaskMenu());
                    ListTasksInChat();
                }
                else if (userInput == "8")
                {
                    StartQuiz();
                }
                else if (userInput == "0")
                {
                    // Ends the program
                    chatBox.AppendText(ChatbotFunctions.Exit(userName));
                    // Close the form after 2 seconds so the user sees the goodbye message
                    Task.Delay(2000).ContinueWith(t => this.Invoke(() => this.Close()));
                }
                else if (TaskManager.IsAddTaskCommand(userInput))
                {
                    // The user typed something like "add task - review my privacy settings"
                    if (!databaseReady)
                        chatBox.AppendText(TaskUnavailableMessage());
                    else
                    {
                        chatBox.AppendText(TaskManager.StartAddTask(userInput));
                        RefreshTaskList();
                    }
                }
                else if (TaskManager.IsViewTasksCommand(userInput))
                {
                    // The user asked to see their tasks
                    ListTasksInChat();
                }
                else if (databaseReady && TaskManager.HandleManagementCommand(userInput) != "")
                {
                    // The user typed something like "complete task 2" or "delete task 3"
                    chatBox.AppendText(TaskManager.HandleManagementCommand(userInput));
                    RefreshTaskList();
                }
                else if (IsQuizCommand(userInput))
                {
                    StartQuiz();
                }
                else
                {
                    // If it was not a menu number, try to find a keyword in the input
                    string keywordResponse = ChatbotFunctions.CheckKeywords(userInput);

                    // If a keyword was found show the keyword response
                    if (!string.IsNullOrEmpty(keywordResponse))
                    {
                        chatBox.AppendText(keywordResponse);
                    }
                    else
                    {
                        // Otherwise tell the user the input was not understood
                        chatBox.AppendText(ChatbotFunctions.Validation());
                    }
                }

                // Clear the input box for next entry
                inputBox.Clear();

                // Scroll to the bottom so the latest message is visible
                ScrollToBottom();
            }
            catch (Exception error)
            {
                // If anything unexpected goes wrong, show a friendly message and keep the program running
                chatBox.AppendText("\nChatBot: Something unexpected happened but I'm still here. Please try again.\n");
                Console.WriteLine("Error in ProcessInput: " + error.Message);
                inputBox.Clear();
            }
        }

        // Reloads the grid from the database, keeping it in step with any changes
        private void RefreshTaskList()
        {
            if (!databaseReady) return;
            try
            {
                taskGrid.Rows.Clear();
                var tasks = DatabaseHelper.GetAllTasks();
                foreach (var task in tasks)
                {
                    string reminder = task.ReminderDate.HasValue
                        ? task.ReminderDate.Value.ToString("dd MMM HH:mm")
                        : "-";
                    string status = task.IsCompleted ? "Done" : "To do";
                    taskGrid.Rows.Add(task.Id, task.Title, reminder, status);
                }
            }
            catch
            {
                // If the database drops out we just leave the grid as it is
            }
        }

        // Reads the task id out of the currently selected row, or -1 if nothing is selected
        private int GetSelectedTaskId()
        {
            if (taskGrid.SelectedRows.Count == 0) return -1;
            return Convert.ToInt32(taskGrid.SelectedRows[0].Cells["Id"].Value);
        }

        // Opens the add task dialog and saves the new task if the user confirms
        private void AddTaskButton_Click(object? sender, EventArgs e)
        {
            if (!databaseReady) { chatBox.AppendText(TaskUnavailableMessage()); return; }

            using (var dialog = new AddTaskForm())
            {
                if (dialog.ShowDialog() == DialogResult.OK && dialog.CreatedTask != null)
                {
                    DatabaseHelper.AddTask(dialog.CreatedTask);
                    chatBox.AppendText("\nChatBot: Task added: " + dialog.CreatedTask.Title + "\n");
                    RefreshTaskList();
                    ScrollToBottom();
                }
            }
        }

        // Marks the selected task as complete
        private void CompleteButton_Click(object? sender, EventArgs e)
        {
            int id = GetSelectedTaskId();
            if (id < 0) { chatBox.AppendText("\nChatBot: Please select a task first.\n"); return; }
            DatabaseHelper.MarkComplete(id);
            chatBox.AppendText("\nChatBot: Nice work! Task #" + id + " is marked complete.\n");
            RefreshTaskList();
            ScrollToBottom();
        }

        // Deletes the selected task, asking for confirmation first
        private void DeleteButton_Click(object? sender, EventArgs e)
        {
            int id = GetSelectedTaskId();
            if (id < 0) { chatBox.AppendText("\nChatBot: Please select a task first.\n"); return; }

            var answer = MessageBox.Show("Delete task #" + id + "?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes)
            {
                DatabaseHelper.DeleteTask(id);
                chatBox.AppendText("\nChatBot: Task #" + id + " has been deleted.\n");
                RefreshTaskList();
                ScrollToBottom();
            }
        }

        // Just reloads the list from the database
        private void RefreshButton_Click(object? sender, EventArgs e)
        {
            RefreshTaskList();
        }

        // Fired once a minute - shows any reminders that have now fallen due
        private void ReminderTimer_Tick(object? sender, EventArgs e)
        {
            if (!databaseReady) return;
            string due = TaskManager.GetDueReminders();
            if (due != "")
            {
                chatBox.AppendText(due);
                ScrollToBottom();
            }
        }

        // Friendly message shown when MySQL isn't available
        private string TaskUnavailableMessage()
        {
            return "\nChatBot: The task assistant needs a MySQL database, but I couldn't connect "
                 + "to one. Please make sure MySQL is running and check the settings at the top of "
                 + "DatabaseHelper.cs. Everything else still works fine!\n";
        }

        // Lists the tasks in the chat window, guarding against the database being down
        private void ListTasksInChat()
        {
            if (!databaseReady) { chatBox.AppendText(TaskUnavailableMessage()); return; }
            try { chatBox.AppendText(TaskManager.ListTasksText()); }
            catch { chatBox.AppendText(TaskUnavailableMessage()); }
        }

        private bool IsQuizCommand(string input)
        {
            string lower = input.ToLower().Trim();
            return lower == "quiz" || lower == "play" || lower == "play quiz"
                || lower == "mini game" || lower == "minigame" || lower == "game"
                || lower.Contains("play the quiz") || lower.Contains("start quiz")
                || lower.Contains("play the game") || lower.Contains("mini-game");
        }

        private void StartQuiz()
        {
            chatBox.AppendText("\nChatBot: Let's test your cybersecurity knowledge! Opening the quiz...\n");
            ScrollToBottom();
            using (var quiz = new QuizForm())
            {
                quiz.ShowDialog(this);
            }
            chatBox.AppendText("\nChatBot: Thanks for playing! Type 8 any time to play again.\n");
            ScrollToBottom();
        }

        // Helper method to keep the chat scrolled to the bottom
        private void ScrollToBottom()
        {
            try
            {
                chatBox.SelectionStart = chatBox.Text.Length;
                chatBox.ScrollToCaret();
            }
            catch
            {
                // Ignore scroll errors - they should never crash the chat
            }
        }
    }
}