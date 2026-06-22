using System.Text.RegularExpressions;

namespace Chatbot
{
    public class NlpProcessor
    {
        public static void LogAction(string action)
        {
            ActivityLog.Add(action);
        }

        public static bool IsSummaryRequest(string input)
        {
            string lower = input.ToLower().Trim();
            return lower.Contains("what have you done")
                || lower.Contains("what did you do")
                || lower.Contains("what you have done")
                || lower.Contains("what have you helped")
                || lower.Contains("recent actions")
                || lower.Contains("show me a summary")
                || lower == "summary";
        }

        public static string GetActionSummary()
        {
            var entries = ActivityLog.GetNewestFirst();
            if (entries.Count == 0)
                return "\nChatBot: I haven't done anything for you yet. Try adding a task or setting a reminder!\n";

            string output = "\nChatBot: Here's a summary of recent actions:\n";
            int show = Math.Min(5, entries.Count);
            for (int i = 0; i < show; i++)
                output += "  " + (i + 1) + ". " + entries[i] + "\n";
            if (entries.Count > show)
                output += "  ...type 9 or 'activity log' to see the full history.\n";
            return output;
        }

        public static string Respond(string input, bool databaseReady)
        {
            string lower = input.ToLower().Trim();

            bool reminderIntent = lower.Contains("remind me") || lower.Contains("set a reminder")
                || lower.Contains("set reminder") || lower.Contains("add a reminder")
                || lower.StartsWith("remind ") || lower.StartsWith("reminder ")
                || lower.Contains("reminder to") || lower.Contains("reminder for");

            bool addTaskIntent = lower.Contains("add a task") || lower.Contains("add task")
                || lower.Contains("create a task") || lower.Contains("create task")
                || lower.Contains("make a task") || lower.Contains("new task")
                || lower.Contains("set up a task");

            if (reminderIntent)
            {
                string action = StripCommandWords(input);
                DateTime? when = ExtractWhen(ref action);
                action = TidyAction(action);

                if (string.IsNullOrWhiteSpace(action))
                    return "";

                if (when != null)
                {
                    if (!databaseReady)
                        return DatabaseDownMessage();

                    TaskItem task = new TaskItem();
                    task.Title = action;
                    task.Description = action;
                    task.Id = DatabaseHelper.AddTask(task);
                    DatabaseHelper.UpdateReminder(task.Id, when.Value);

                    LogAction("Reminder set for '" + action + "' on " + when.Value.ToString("dd MMM yyyy HH:mm"));
                    return "\nChatBot: Reminder set for '" + action + "' on "
                         + when.Value.ToString("dddd, dd MMMM yyyy 'at' HH:mm") + ".\n";
                }

                return AddTaskThroughManager(action, databaseReady);
            }

            if (addTaskIntent)
            {
                string action = StripCommandWords(input);
                action = TidyAction(action);

                if (string.IsNullOrWhiteSpace(action))
                    return "";

                return AddTaskThroughManager(action, databaseReady);
            }

            return "";
        }

        private static string AddTaskThroughManager(string action, bool databaseReady)
        {
            if (!databaseReady)
                return DatabaseDownMessage();

            string response = TaskManager.StartAddTask("add task - " + action);
            LogAction("Added a task: " + Capitalise(action));
            return response;
        }

        private static string StripCommandWords(string input)
        {
            string text = input.Trim();
            string[] prefixes =
            {
                "hey ", "hi ", "please ", "can you ", "could you ", "would you ",
                "i want to ", "i need to ", "i'd like to ", "i would like to ",
                "set a reminder to ", "set a reminder for ", "set a reminder ", "set reminder to ", "set reminder ",
                "add a reminder to ", "add a reminder for ", "add a reminder ",
                "remind me to ", "remind me ", "remind to ", "reminder to ", "reminder for ", "remind ",
                "add a task to ", "add a task for ", "add a task ", "add task to ", "add task for ", "add task ",
                "create a task to ", "create a task ", "create task ", "make a task to ", "make a task ",
                "new task ", "set up a task to ", "set up a task ", "to "
            };

            bool changed = true;
            while (changed)
            {
                changed = false;
                string lower = text.ToLower();
                foreach (string prefix in prefixes)
                {
                    if (lower.StartsWith(prefix))
                    {
                        text = text.Substring(prefix.Length).Trim();
                        changed = true;
                        break;
                    }
                }
            }

            return text;
        }

        private static DateTime? ExtractWhen(ref string action)
        {
            string lower = action.ToLower();

            if (lower.Contains("tomorrow"))
            {
                action = RemovePhrase(action, "tomorrow");
                return DateTime.Now.AddDays(1);
            }
            if (lower.Contains("tonight"))
            {
                action = RemovePhrase(action, "tonight");
                return DateTime.Today.AddHours(20);
            }
            if (lower.Contains("next week"))
            {
                action = RemovePhrase(action, "next week");
                return DateTime.Now.AddDays(7);
            }
            if (lower.Contains("today"))
            {
                action = RemovePhrase(action, "today");
                return DateTime.Now.AddHours(1);
            }

            Match match = Regex.Match(lower, @"in\s+(\d+)\s+(day|days|week|weeks|hour|hours|month|months)");
            if (match.Success)
            {
                int amount = int.Parse(match.Groups[1].Value);
                string unit = match.Groups[2].Value;
                action = RemovePhrase(action, match.Value);

                if (unit.StartsWith("day")) return DateTime.Now.AddDays(amount);
                if (unit.StartsWith("week")) return DateTime.Now.AddDays(amount * 7);
                if (unit.StartsWith("hour")) return DateTime.Now.AddHours(amount);
                if (unit.StartsWith("month")) return DateTime.Now.AddMonths(amount);
            }

            return null;
        }

        private static string RemovePhrase(string text, string phrase)
        {
            int index = text.ToLower().IndexOf(phrase.ToLower());
            if (index < 0) return text;
            return (text.Substring(0, index) + text.Substring(index + phrase.Length)).Trim();
        }

        private static string TidyAction(string action)
        {
            string text = action.Trim();
            text = text.Trim('-', ':', ',', '.', ' ').Trim();

            string[] leadWords = { "me to ", "to ", "that ", "for ", "about " };
            bool changed = true;
            while (changed)
            {
                changed = false;
                string lower = text.ToLower();
                foreach (string word in leadWords)
                {
                    if (lower.StartsWith(word))
                    {
                        text = text.Substring(word.Length).Trim();
                        changed = true;
                        break;
                    }
                }
            }

            text = text.Trim();
            if (text.ToLower().EndsWith(" on"))
                text = text.Substring(0, text.Length - 3).Trim();
            if (text.ToLower().EndsWith(" at"))
                text = text.Substring(0, text.Length - 3).Trim();

            return Capitalise(text);
        }

        private static string Capitalise(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        private static string DatabaseDownMessage()
        {
            return "\nChatBot: I understood that, but the task database isn't available right now. "
                 + "Please make sure MySQL is running and try again.\n";
        }
    }
}