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

        // Form constructor
        public MainForm()
        {
            // Form settings
            this.Text = "CyberBot - Cybersecurity Assistant";
            this.Size = new Size(900, 700);
            this.MinimumSize = new Size(700, 500);
            this.BackColor = Color.FromArgb(15, 15, 25);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Padding = new Padding(20);

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

            // Chat display area - stretches with window
            chatBox = new RichTextBox();
            chatBox.Location = new Point(20, 90);
            chatBox.Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 175);
            chatBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            chatBox.BackColor = Color.FromArgb(10, 10, 20);
            chatBox.ForeColor = Color.Cyan;
            chatBox.Font = new Font("Consolas", 10);
            chatBox.BorderStyle = BorderStyle.FixedSingle;
            chatBox.ReadOnly = true;
            this.Controls.Add(chatBox);

            // Input box for the user to type in - stretches with window
            inputBox = new TextBox();
            inputBox.Location = new Point(20, this.ClientSize.Height - 70);
            inputBox.Size = new Size(this.ClientSize.Width - 145, 35);
            inputBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            inputBox.Font = new Font("Consolas", 12);
            inputBox.BackColor = Color.FromArgb(25, 25, 40);
            inputBox.ForeColor = Color.White;
            inputBox.BorderStyle = BorderStyle.FixedSingle;
            // Allow Enter key to send
            inputBox.KeyDown += InputBox_KeyDown;
            this.Controls.Add(inputBox);

            // Send button on the right of the input box
            sendButton = new Button();
            sendButton.Location = new Point(this.ClientSize.Width - 120, this.ClientSize.Height - 72);
            sendButton.Size = new Size(105, 38);
            sendButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            sendButton.Text = "Send";
            sendButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            sendButton.BackColor = Color.FromArgb(0, 120, 150);
            sendButton.ForeColor = Color.White;
            sendButton.FlatStyle = FlatStyle.Flat;
            sendButton.FlatAppearance.BorderSize = 0;
            sendButton.Cursor = Cursors.Hand;
            sendButton.Click += SendButton_Click;
            this.Controls.Add(sendButton);

            // Run the start sequence when the form loads
            this.Load += MainForm_Load;
        }

        // What happens when the form opens
        private void MainForm_Load(object? sender, EventArgs e)
        {
            // Display the greeting and ASCII art
            chatBox.AppendText(ChatbotFunctions.Greeting());

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
        private void ProcessInput()
        {
            // Check if the user typed nothing
            if (string.IsNullOrWhiteSpace(inputBox.Text))
            {
                chatBox.AppendText("\nChatBot: Please enter a valid option from the menu.\n");
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

                // Clear the input box for next entry
                inputBox.Clear();
                ScrollToBottom();
                return;
            }

            // Show what the user typed
            chatBox.AppendText("\n[" + userName + "]: " + userInput + "\n");

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
            else if (userInput == "0")
            {
                // Ends the program
                chatBox.AppendText(ChatbotFunctions.Exit(userName));
                // Close the form after 2 seconds so the user sees the goodbye message
                Task.Delay(2000).ContinueWith(t => this.Invoke(() => this.Close()));
            }
            else
            {
                // If nothing matched tell the user
                chatBox.AppendText(ChatbotFunctions.Validation());
            }

            // Clear the input box for next entry
            inputBox.Clear();

            // Scroll to the bottom so the latest message is visible
            ScrollToBottom();
        }

        // Helper method to keep the chat scrolled to the bottom
        private void ScrollToBottom()
        {
            chatBox.SelectionStart = chatBox.Text.Length;
            chatBox.ScrollToCaret();
        }
    }
}