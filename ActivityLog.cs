namespace Chatbot
{
    public class ActivityLog
    {
        private static List<string> entries = new List<string>();
        private static string filePath = "activity_log.txt";
        private static bool loaded = false;

        public static void Load()
        {
            if (loaded) return;
            loaded = true;
            try
            {
                if (File.Exists(filePath))
                {
                    foreach (string line in File.ReadAllLines(filePath))
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                            entries.Add(line);
                    }
                }
            }
            catch
            {
            }
        }

        public static void Add(string action)
        {
            string entry = DateTime.Now.ToString("dd MMM yyyy HH:mm") + " - " + action;
            entries.Add(entry);
            try
            {
                File.AppendAllText(filePath, entry + Environment.NewLine);
            }
            catch
            {
            }
        }

        public static List<string> GetNewestFirst()
        {
            List<string> copy = new List<string>(entries);
            copy.Reverse();
            return copy;
        }

        public static int Count()
        {
            return entries.Count;
        }
    }
}