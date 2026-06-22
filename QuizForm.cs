namespace Chatbot
{
    public class QuizForm : Form
    {
        private class QuizQuestion
        {
            public string Text = "";
            public string[] Options = new string[0];
            public int CorrectIndex = 0;
            public string Explanation = "";
        }

        private List<QuizQuestion> questions = new List<QuizQuestion>();
        private int currentIndex = 0;
        private int score = 0;
        private bool answered = false;

        private Label headerLabel = null!;
        private Label progressLabel = null!;
        private Label questionLabel = null!;
        private Button[] answerButtons = null!;
        private Label feedbackLabel = null!;
        private Button nextButton = null!;

        public QuizForm()
        {
            this.Text = "Cybersecurity Mini-Game";
            this.Size = new Size(660, 580);
            this.MinimumSize = new Size(660, 580);
            this.BackColor = Color.FromArgb(15, 15, 25);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(20);

            BuildQuestions();
            BuildControls();
            ShowQuestion();
        }

        private void BuildControls()
        {
            headerLabel = new Label();
            headerLabel.Text = "Cybersecurity Quiz";
            headerLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            headerLabel.ForeColor = Color.Cyan;
            headerLabel.Location = new Point(20, 18);
            headerLabel.Size = new Size(600, 36);
            this.Controls.Add(headerLabel);

            progressLabel = new Label();
            progressLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            progressLabel.ForeColor = Color.White;
            progressLabel.Location = new Point(20, 58);
            progressLabel.Size = new Size(600, 24);
            this.Controls.Add(progressLabel);

            questionLabel = new Label();
            questionLabel.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            questionLabel.ForeColor = Color.White;
            questionLabel.Location = new Point(20, 92);
            questionLabel.Size = new Size(600, 80);
            this.Controls.Add(questionLabel);

            answerButtons = new Button[4];
            for (int i = 0; i < 4; i++)
            {
                Button b = new Button();
                b.Location = new Point(20, 182 + i * 52);
                b.Size = new Size(600, 44);
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.BackColor = Color.FromArgb(0, 80, 100);
                b.ForeColor = Color.White;
                b.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                b.TextAlign = ContentAlignment.MiddleLeft;
                b.Padding = new Padding(15, 0, 0, 0);
                b.Cursor = Cursors.Hand;
                int index = i;
                b.Click += (s, e) => AnswerChosen(index);
                answerButtons[i] = b;
                this.Controls.Add(b);
            }

            feedbackLabel = new Label();
            feedbackLabel.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            feedbackLabel.ForeColor = Color.White;
            feedbackLabel.Location = new Point(20, 398);
            feedbackLabel.Size = new Size(600, 70);
            this.Controls.Add(feedbackLabel);

            nextButton = new Button();
            nextButton.Text = "Next";
            nextButton.Location = new Point(515, 480);
            nextButton.Size = new Size(105, 42);
            nextButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            nextButton.FlatStyle = FlatStyle.Flat;
            nextButton.FlatAppearance.BorderSize = 0;
            nextButton.BackColor = Color.FromArgb(0, 120, 150);
            nextButton.ForeColor = Color.White;
            nextButton.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            nextButton.Cursor = Cursors.Hand;
            nextButton.Visible = false;
            nextButton.Click += NextButton_Click;
            this.Controls.Add(nextButton);
        }

        private void ShowQuestion()
        {
            answered = false;
            QuizQuestion q = questions[currentIndex];
            progressLabel.Text = "Question " + (currentIndex + 1) + " of " + questions.Count + "          Score: " + score;
            questionLabel.Text = q.Text;
            feedbackLabel.Text = "";
            nextButton.Visible = false;

            for (int i = 0; i < answerButtons.Length; i++)
            {
                if (i < q.Options.Length)
                {
                    answerButtons[i].Text = q.Options[i];
                    answerButtons[i].Visible = true;
                    answerButtons[i].Enabled = true;
                    answerButtons[i].BackColor = Color.FromArgb(0, 80, 100);
                }
                else
                {
                    answerButtons[i].Visible = false;
                }
            }
        }

        private void AnswerChosen(int index)
        {
            if (answered) return;
            answered = true;
            QuizQuestion q = questions[currentIndex];

            for (int i = 0; i < q.Options.Length; i++)
                answerButtons[i].Enabled = false;

            answerButtons[q.CorrectIndex].BackColor = Color.FromArgb(0, 140, 70);

            if (index == q.CorrectIndex)
            {
                score++;
                feedbackLabel.ForeColor = Color.LightGreen;
                feedbackLabel.Text = "Correct! " + q.Explanation;
            }
            else
            {
                answerButtons[index].BackColor = Color.FromArgb(150, 40, 40);
                feedbackLabel.ForeColor = Color.Salmon;
                feedbackLabel.Text = "Not quite. " + q.Explanation;
            }

            progressLabel.Text = "Question " + (currentIndex + 1) + " of " + questions.Count + "          Score: " + score;
            nextButton.Text = currentIndex == questions.Count - 1 ? "Finish" : "Next";
            nextButton.Visible = true;
        }

        private void NextButton_Click(object? sender, EventArgs e)
        {
            if (currentIndex < questions.Count - 1)
            {
                currentIndex++;
                ShowQuestion();
            }
            else
            {
                ShowResult();
            }
        }

        private void ShowResult()
        {
            foreach (Button b in answerButtons)
                b.Visible = false;

            progressLabel.Text = "";
            questionLabel.Text = "You scored " + score + " out of " + questions.Count + "!";

            double ratio = (double)score / questions.Count;
            string message;
            if (ratio >= 0.8)
                message = "Great job! You're a cybersecurity pro!";
            else if (ratio >= 0.5)
                message = "Nice work! A little more practice and you'll be an expert.";
            else
                message = "Keep learning to stay safe online!";

            feedbackLabel.ForeColor = Color.Cyan;
            feedbackLabel.Text = message;

            nextButton.Text = "Close";
            nextButton.Visible = true;
            nextButton.Click -= NextButton_Click;
            nextButton.Click += (s, e) => this.Close();
        }

        private void BuildQuestions()
        {
            Add("What should you do if you receive an email asking for your password?",
                new[] { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                2, "Reporting phishing emails helps prevent scams and protects others too.");

            Add("Which of these is the strongest password?",
                new[] { "password123", "Your pet's name", "A long mix of letters, numbers and symbols", "123456" },
                2, "Long, random passwords with mixed characters are far harder to crack.");

            Add("True or False: It is safe to reuse the same password across many accounts.",
                new[] { "True", "False" },
                1, "Reusing passwords means one breach can unlock all of your accounts.");

            Add("What does the padlock icon in your browser's address bar mean?",
                new[] { "The site is government owned", "The connection is encrypted (HTTPS)", "The site has no ads", "The site is free" },
                1, "The padlock shows the connection is encrypted, but always check the address too.");

            Add("A caller claims to be IT support and urgently wants your login details. What should you do?",
                new[] { "Give them the details", "Refuse and verify through official channels", "Read out your password slowly", "Email them your password" },
                1, "This is a classic social engineering trick. Always verify before sharing anything.");

            Add("True or False: Public Wi-Fi is always safe for online banking.",
                new[] { "True", "False" },
                1, "Public Wi-Fi can be intercepted. Use mobile data or a VPN for sensitive tasks.");

            Add("Which is a common sign of a phishing email?",
                new[] { "Perfect spelling and grammar", "A sender you fully trust", "Urgent threats and suspicious links", "No links at all" },
                2, "Phishing often uses urgency and dodgy links to rush you into mistakes.");

            Add("How often should you install software and app updates?",
                new[] { "Never", "Only when something breaks", "Regularly, as they are released", "Once every five years" },
                2, "Updates patch security flaws, so installing them promptly keeps you safer.");

            Add("True or False: Two-factor authentication adds an extra layer of account security.",
                new[] { "True", "False" },
                0, "Even if your password is stolen, the second factor blocks most attackers.");

            Add("What is 'malware'?",
                new[] { "A type of healthy food", "Software designed to harm or exploit devices", "A web browser", "A strong password" },
                1, "Malware is malicious software like viruses, ransomware and spyware.");

            Add("True or False: You should click links in unexpected messages to see where they lead.",
                new[] { "True", "False" },
                1, "Unexpected links can lead to scams or malware. When in doubt, don't click.");

            Add("What is the safest way to store many strong, unique passwords?",
                new[] { "On a sticky note on your monitor", "Use the same one everywhere", "Use a reputable password manager", "In a public document" },
                2, "A trusted password manager keeps unique passwords safe behind one master key.");
        }

        private void Add(string text, string[] options, int correct, string explanation)
        {
            QuizQuestion q = new QuizQuestion();
            q.Text = text;
            q.Options = options;
            q.CorrectIndex = correct;
            q.Explanation = explanation;
            questions.Add(q);
        }
    }
}