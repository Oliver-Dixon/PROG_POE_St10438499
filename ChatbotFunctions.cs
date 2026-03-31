// ChatbotFunctions.cs
// This file holds all the functions that the chatbot uses

namespace Chatbot
{
    public class ChatbotFunctions
    {
        // This is the line that gets printed between sections
        static string line = "--------------------------------------------";

        // This function shows the welcome banner when the program starts
        public static void ShowGreeting()
        {
            Console.WriteLine(line);
            Console.WriteLine("   ___      _              ___       _   ");
            Console.WriteLine("  / __\\_  _| |__   ___ _ _| _ ) ___ | |_ ");
            Console.WriteLine(" / /  | || | '_ \\ / -_) '_| _ \\/ _ \\|  _|");
            Console.WriteLine(" \\/    \\_, |_.__/ \\___|_| |___/\\___/ \\__|");
            Console.WriteLine("       |__/                                ");
            Console.WriteLine(line);
            Console.WriteLine("  Welcome to CyberBot - Cybersecurity Assistant");
            Console.WriteLine(line);
            Console.WriteLine("");
        }

        // This function asks the user for their name
        public static string GetUserName()
        {
            Console.Write("CyberBot: Before we begin, what is your name? ");

            string? name = Console.ReadLine();

            // Keep asking if they leave the name blank
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("CyberBot: Please enter a valid name: ");
                name = Console.ReadLine();
            }

            Console.WriteLine("");
            Console.WriteLine("CyberBot: Nice to meet you, " + name + "!");
            Console.WriteLine("CyberBot: Type 'help' to see what I can help you with.");

            return name.Trim();
        }

        // This function shows all the topics the user can ask about
        public static void ShowHelp()
        {
            Console.WriteLine("");
            Console.WriteLine(line);
            Console.WriteLine("  Topics you can ask me about:");
            Console.WriteLine(line);
            Console.WriteLine("  how are you           - Check on the bot");
            Console.WriteLine("  what's your purpose   - Learn what CyberBot does");
            Console.WriteLine("  what can i ask you about - See this menu");
            Console.WriteLine("  safe browsing         - Safe browsing tips");
            Console.WriteLine("  phishing              - How to spot phishing");
            Console.WriteLine("  password safety       - Password best practices");
            Console.WriteLine("  exit                  - Quit the chatbot");
            Console.WriteLine(line);
        }

        // This function responds when the user asks how the bot is doing
        public static void RespondHowAreYou(string name)
        {
            Console.WriteLine("");
            Console.WriteLine("CyberBot: I am running perfectly, thanks for asking " + name + "!");
            Console.WriteLine("CyberBot: Ready to help you stay safe online.");
        }

        // This function explains what the chatbot is for
        public static void RespondPurpose(string name)
        {
            Console.WriteLine("");
            Console.WriteLine("CyberBot: Great question " + name + "!");
            Console.WriteLine("CyberBot: I am here to help teach you about cybersecurity.");
            Console.WriteLine("CyberBot: You can ask me about safe browsing, phishing and password safety.");
        }

        // This function gives the user safe browsing tips
        public static void RespondSafeBrowsing()
        {
            // Store all the tips in an array
            string tip1 = "Always check for HTTPS in the URL before entering data.";
            string tip2 = "Avoid clicking suspicious links in emails or messages.";
            string tip3 = "Keep your browser and extensions up to date.";
            string tip4 = "Use an ad-blocker to reduce exposure to malicious ads.";
            string tip5 = "Avoid using public Wi-Fi for banking without a VPN.";

            Console.WriteLine("");
            Console.WriteLine(line);
            Console.WriteLine("  Safe Browsing Tips");
            Console.WriteLine(line);

            // Print each tip with a number
            Console.WriteLine("  1. " + tip1);
            Console.WriteLine("  2. " + tip2);
            Console.WriteLine("  3. " + tip3);
            Console.WriteLine("  4. " + tip4);
            Console.WriteLine("  5. " + tip5);

            Console.WriteLine(line);
        }

        // This function explains how to spot a phishing attempt
        public static void RespondPhishing()
        {
            string flag1 = "Urgent language like - Your account will be closed!";
            string flag2 = "Sender addresses that look similar but are slightly off.";
            string flag3 = "Links where the hover URL does not match the displayed text.";
            string flag4 = "Requests for passwords or card details via email.";
            string flag5 = "Poor grammar, odd formatting or unexpected attachments.";

            Console.WriteLine("");
            Console.WriteLine(line);
            Console.WriteLine("  Phishing Warning Signs");
            Console.WriteLine(line);

            Console.WriteLine("  1. " + flag1);
            Console.WriteLine("  2. " + flag2);
            Console.WriteLine("  3. " + flag3);
            Console.WriteLine("  4. " + flag4);
            Console.WriteLine("  5. " + flag5);

            Console.WriteLine(line);
            Console.WriteLine("  When in doubt do not click. Go directly to the official website.");
            Console.WriteLine(line);
        }

        // This function shares password safety advice
        public static void RespondPasswordSafety()
        {
            string advice1 = "Use at least 12 characters with letters, numbers and symbols.";
            string advice2 = "Never reuse the same password across different accounts.";
            string advice3 = "Use a password manager like Bit warden or 1Password.";
            string advice4 = "Enable two-factor authentication wherever possible.";
            string advice5 = "Avoid using personal info like birthdays or pet names.";

            Console.WriteLine("");
            Console.WriteLine(line);
            Console.WriteLine("  Password Safety Tips");
            Console.WriteLine(line);

            Console.WriteLine("  1. " + advice1);
            Console.WriteLine("  2. " + advice2);
            Console.WriteLine("  3. " + advice3);
            Console.WriteLine("  4. " + advice4);
            Console.WriteLine("  5. " + advice5);

            Console.WriteLine(line);
        }

        // This function handles input that the chatbot does not recognize
        public static void RespondUnknown()
        {
            Console.WriteLine("");
            Console.WriteLine("CyberBot: I didn't quite understand that. Could you rephrase?");
            Console.WriteLine("CyberBot: Type 'help' to see the list of available topics.");
        }

        // This function says goodbye when the user exits
        public static void ShowGoodbye(string name)
        {
            Console.WriteLine("");
            Console.WriteLine(line);
            Console.WriteLine("  Goodbye " + name + "! Stay safe online.");
            Console.WriteLine(line);
        }
    }
}