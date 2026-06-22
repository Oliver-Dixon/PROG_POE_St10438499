//Task manager

namespace Chatbot
{
    
    public class TaskManager
    {
        // Holds the task currently being created through the conversation
        // null means we aren't in the middle of adding a task
        private static TaskItem? pendingTask = null;

        // Tracks what stage of adding a task we are at
        // "none" = not adding, "reminder" = waiting to hear if they want a reminder,
        // "time" = waiting for them to say when to be reminded
        private static string conversationStage = "none";

        // Keeps track of reminders we've already announced so we don't repeat them
        private static HashSet<int> remindedTaskIds = new HashSet<int>();

        // Lets the form check whether we are part way through a task conversation
        public static bool IsAddingTask()
        {
            return conversationStage != "none";
        }

        // Checks if the user typed a command to add a new task
        public static bool IsAddTaskCommand(string input)
        {
            string lower = input.ToLower().Trim();
            return lower.StartsWith("add task") || lower.StartsWith("add a task")
                || lower.StartsWith("new task") || lower.StartsWith("create task");
        }

        // Checks if the user wants to see their list of tasks
        public static bool IsViewTasksCommand(string input)
        {
            string lower = input.ToLower().Trim();
            return lower == "view tasks" || lower == "show tasks" || lower == "my tasks"
                || lower == "list tasks" || lower == "tasks"
                || lower.Contains("show my tasks") || lower.Contains("view my tasks");
        }

        // Starts adding a task that the user described in the chat
        // The task is saved straight away so it is stored even before a reminder is chosen
        public static string StartAddTask(string input)
        {
            // Pull out the part the user typed after "add task"
            string taskText = ExtractTaskText(input);

            // If they didn't actually say what the task is, ask them
            if (string.IsNullOrWhiteSpace(taskText))
            {
                return "\nChatBot: Sure! What task would you like to add? For example: add task - set up two-factor authentication\n";
            }

            // Build the new task and remember it while we ask about a reminder
            pendingTask = new TaskItem();
            pendingTask.Title = Capitalise(taskText);
            pendingTask.Description = BuildDescription(taskText);

            // Save it now so the task is stored right away
            pendingTask.Id = DatabaseHelper.AddTask(pendingTask);

            // Move the conversation on to asking about a reminder
            conversationStage = "reminder";

            return "\nChatBot: Task added with the description \"" + pendingTask.Description + "\" Would you like to set a reminder? (yes/no)\n";
        }

        // Continues the add-task conversation once it has started
        // Handles the yes/no for a reminder and works out when to remind the user
        public static string ContinueConversation(string input)
        {
            string lower = input.ToLower();

            if (conversationStage == "reminder")
            {
                // Check straight away in case they already told us when to remind them
                DateTime? maybeDate = ParseReminder(input);

                bool saysNo = ContainsWord(lower, "no") || ContainsWord(lower, "nope")
                    || lower.Contains("no thanks") || lower.Contains("don't");
                bool saysYes = ContainsWord(lower, "yes") || ContainsWord(lower, "yeah")
                    || ContainsWord(lower, "yep") || ContainsWord(lower, "sure") || lower.Contains("please");

                // If they said no and gave no time, finish without a reminder
                if (saysNo && maybeDate == null)
                {
                    conversationStage = "none";
                    pendingTask = null;
                    return "\nChatBot: No problem, I've saved the task without a reminder.\n";
                }

                // If they gave a time in the same message, set it now
                if (maybeDate != null)
                {
                    return FinishReminder(maybeDate.Value);
                }

                // They said yes but didn't say when, so ask for the timing
                if (saysYes)
                {
                    conversationStage = "time";
                    return "\nChatBot: Sure! When would you like to be reminded? For example: in 3 days, in 1 week or tomorrow.\n";
                }

                // Anything else - gently ask again
                return "\nChatBot: Would you like a reminder for this task? You can say yes or no.\n";
            }

            if (conversationStage == "time")
            {
                DateTime? reminderDate = ParseReminder(input);
                if (reminderDate != null)
                {
                    return FinishReminder(reminderDate.Value);
                }

                // Couldn't understand the timing, so ask again with examples
                return "\nChatBot: Sorry, I didn't catch that timing. Try something like 'in 3 days', 'in 2 weeks' or 'tomorrow'.\n";
            }

            return "";
        }


        public static string HandleManagementCommand(string input)
        {
            string lower = input.ToLower();

            bool wantsComplete = lower.Contains("complete") || lower.Contains("mark done")
                || lower.Contains("finished") || lower.Contains("done with");
            bool wantsDelete = lower.Contains("delete") || lower.Contains("remove");

            // Only treat it as a command if it mentions a task and one of the actions
            if (!lower.Contains("task") || (!wantsComplete && !wantsDelete))
                return "";

            // Find the task number in the message
            int id = FindNumber(lower);
            if (id < 0)
                return "\nChatBot: Which task number do you mean? For example: complete task 2\n";

            if (wantsComplete)
            {
                DatabaseHelper.MarkComplete(id);
                return "\nChatBot: Nice work! I've marked task #" + id + " as completed.\n";
            }

            DatabaseHelper.DeleteTask(id);
            return "\nChatBot: Done, I've removed task #" + id + " from your list.\n";
        }

  
        public static string ListTasksText()
        {
            var tasks = DatabaseHelper.GetAllTasks();

            if (tasks.Count == 0)
                return "\nChatBot: You don't have any tasks yet. Add one by typing: add task - set up two-factor authentication\n";

            string output = "\nChatBot: Here are your current cybersecurity tasks:\n";
            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "[Done] " : "[To do] ";
                string reminder = task.ReminderDate.HasValue
                    ? " (Reminder: " + task.ReminderDate.Value.ToString("dd MMM yyyy HH:mm") + ")"
                    : "";
                output += "  #" + task.Id + " " + status + task.Title + reminder + "\n";
            }
            return output;
        }

     
        public static string GetDueReminders()
        {
            var tasks = DatabaseHelper.GetAllTasks();
            string body = "";

            foreach (var task in tasks)
            {
                if (!task.IsCompleted
                    && task.ReminderDate.HasValue
                    && task.ReminderDate.Value <= DateTime.Now
                    && !remindedTaskIds.Contains(task.Id))
                {
                    
                    remindedTaskIds.Add(task.Id);
                    body += "  Task #" + task.Id + ": " + task.Title + " - " + task.Description + "\n";
                }
            }

            if (body == "")
                return "";

            string stars = "* * * * * * * * * * * * * * * * * * * * * * * * * * * *";
            return "\n" + stars + "\n   Reminder! These tasks are due:\n" + body + stars + "\n";
        }

        
        private static string FinishReminder(DateTime reminderDate)
        {
            if (pendingTask != null)
            {
                DatabaseHelper.UpdateReminder(pendingTask.Id, reminderDate);
            }

            
            string friendly = reminderDate.ToString("dddd, dd MMMM yyyy 'at' HH:mm");

            conversationStage = "none";
            pendingTask = null;

            return "\nChatBot: Got it! I'll remind you on " + friendly + ".\n";
        }

        
        private static DateTime? ParseReminder(string input)
        {
            string lower = input.ToLower();

            // Handle the simple word based options first
            if (lower.Contains("tomorrow"))
                return DateTime.Now.AddDays(1);
            if (lower.Contains("next week"))
                return DateTime.Now.AddDays(7);

            // Split the text into words so we can look for a number and its unit
            string[] words = lower.Split(new[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                // Try to read the current word as a number like the "3" in "in 3 days"
                if (int.TryParse(words[i], out int amount))
                {
                    // Look at the next word to find out the unit of time
                    if (i + 1 < words.Length)
                    {
                        string unit = words[i + 1];
                        if (unit.StartsWith("day"))
                            return DateTime.Now.AddDays(amount);
                        if (unit.StartsWith("week"))
                            return DateTime.Now.AddDays(amount * 7);
                        if (unit.StartsWith("hour"))
                            return DateTime.Now.AddHours(amount);
                        if (unit.StartsWith("month"))
                            return DateTime.Now.AddMonths(amount);
                    }
                }
            }

            // As a last resort try to read an actual date the user may have typed
            if (DateTime.TryParse(input, out DateTime exactDate))
                return exactDate;

            // Nothing matched so there is no reminder date
            return null;
        }

        // Pulls out the task wording from a message like "add task - review privacy settings"
        private static string ExtractTaskText(string input)
        {
            string text = input.Trim();
            string lower = text.ToLower();

            // The phrases a user might start with when adding a task
            string[] prefixes = { "add a task", "add task", "create a task", "create task", "new task", "add" };

            // Remove the first matching prefix from the start of the message only
            foreach (var prefix in prefixes)
            {
                if (lower.StartsWith(prefix))
                {
                    text = text.Substring(prefix.Length);
                    break;
                }
            }

            
            text = text.Trim().TrimStart('-', ':').Trim();
            return text;
        }

        
        private static string BuildDescription(string taskText)
        {
            string lower = taskText.ToLower();

            if (lower.Contains("privacy"))
                return "Review account privacy settings to ensure your data is protected.";
            if (lower.Contains("two-factor") || lower.Contains("2fa") || lower.Contains("authentication"))
                return "Set up two-factor authentication to add an extra layer of security to your account.";
            if (lower.Contains("password"))
                return "Update your passwords to strong, unique ones for each account.";
            if (lower.Contains("backup") || lower.Contains("back up"))
                return "Back up your important files to protect against ransomware and hardware failure.";
            if (lower.Contains("update") || lower.Contains("software"))
                return "Install the latest software updates to patch known security flaws.";

            // Nothing special matched so just use what the user typed, tidied up
            return Capitalise(taskText);
        }

        // Makes the first letter of a piece of text a capital so it reads nicely
        private static string Capitalise(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        // Looks for the first whole word that is a number, used to find a task id
        private static int FindNumber(string text)
        {
            string[] words = text.Split(new[] { ' ', '#', ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (int.TryParse(word, out int number))
                    return number;
            }
            return -1;
        }

        // Checks if a whole word appears in the text, avoiding accidental matches inside other words
        private static bool ContainsWord(string text, string word)
        {
            string[] parts = text.Split(new[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Any(p => p == word);
        }
    }
}