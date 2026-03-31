// ChatbotFunctions.cs
namespace Chatbot
{
    public class ChatbotFunctions
    {
        public static void ShowGreeting()
        {
            Console.WriteLine("Welcome to CyberBot - your personal cybersecurity assistant.");
            Console.WriteLine("Type 'help' to see available topics, or 'exit' to quit.\n");
        }
        public static void ShowHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  how are you     - Check on the bot");
            Console.WriteLine("  purpose         - Learn what CyberBot does");
            Console.WriteLine("  safe browsing   - Tips for browsing safely online");
            Console.WriteLine("  phishing        - How to spot phishing attempts");
            Console.WriteLine("  password safety - Best practices for strong passwords");
            Console.WriteLine("  help            - Show this menu");
            Console.WriteLine("  exit            - Quit the chatbot\n");
        }
        public static void RespondHowAreYou()
        {
            Console.WriteLine("CyberBot: I am running smoothly, thanks for asking!");
            Console.WriteLine("CyberBot: How can I help keep you safe online today?\n");
        }
        public static void RespondPurpose()
        {
            Console.WriteLine("CyberBot: My purpose is to educate you on cybersecurity best practices.");
            Console.WriteLine("CyberBot: Ask me about safe browsing, phishing, or password safety.\n");
        }
        public static void RespondSafeBrowsing()
        {
            Console.WriteLine("CyberBot: Here are some safe browsing tips:\n");
            string[] tips = {
                "Always check for HTTPS in the URL bar before entering any data.",
                "Avoid clicking shortened or suspicious links in emails.",
                "Keep your browser and extensions up to date.",
                "Use a reputable ad-blocker to reduce exposure to malicious ads.",
                "Avoid using public Wi-Fi for banking without a VPN."
            };
            for (int i = 0; i < tips.Length; i++)
                Console.WriteLine((i + 1) + ". " + tips[i]);
            Console.WriteLine();
        }
        public static void RespondPhishing()
        {
            Console.WriteLine("CyberBot: Watch out for these phishing warning signs:\n");
            string[] redFlags = {
                "Urgent language such as Your account will be closed!",
                "Sender addresses that almost match a real company but are slightly off.",
                "Links where the hover URL differs from the displayed text.",
                "Requests for passwords or card details via email.",
                "Poor grammar, odd formatting, or unexpected attachments."
            };
            for (int i = 0; i < redFlags.Length; i++)
                Console.WriteLine((i + 1) + ". " + redFlags[i]);
            Console.WriteLine("\nWhen in doubt, do not click. Go directly to the official website.\n");
        }
        public static void RespondPasswordSafety()
        {
            Console.WriteLine("CyberBot: Follow these password safety tips:\n");
            string[] advice = {
                "Use at least 12 characters with a mix of letters, numbers, and symbols.",
                "Never reuse the same password across different accounts.",
                "Use a trusted password manager such as Bitwarden or 1Password.",
                "Enable two-factor authentication wherever possible.",
                "Avoid personal information like birthdays or pet names in passwords."
            };
            for (int i = 0; i < advice.Length; i++)
                Console.WriteLine((i + 1) + ". " + advice[i]);
            Console.WriteLine();
        }
        public static void RespondUnknown(string input)
        {
            Console.WriteLine("CyberBot: Sorry, I did not understand " + input);
            Console.WriteLine("CyberBot: Type 'help' to see what I can assist with.\n");
        }
    }
}
