// ChatbotFunctions.cs
// This file holds all the functions that the chatbot uses

namespace Chatbot
{
    public class ChatbotFunctions
    {
        // Saves time by creating a global variable for the star separator used in multiple places
        static string stars = "* * * * * * * * * * * * * * * * * * * * *";

        // Greeting method
        public static void Greeting()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("");
            Console.WriteLine(stars);
            Console.WriteLine(" CyberBot - Your Cybersecurity Assistant ");
            Console.WriteLine(stars);
            Console.WriteLine("");
            Console.ResetColor();
        }

        // This asks the user for their name
        public static string UserName()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("ChatBot: What is your name? ");
            Console.ResetColor();

            string? name = Console.ReadLine();

            // Keep asking if they leave the name blank
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("CyberBot: Please enter a valid name: ");
                Console.ResetColor();
                name = Console.ReadLine();
            }

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("ChatBot: Hello " + name + "!");
            Console.WriteLine("ChatBot: I'm here to help you learn about cybersecurity.");
            Console.ResetColor();

            return name.Trim();
        }

        // This shows the user the list of topics they can ask about
        public static void Help()
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(stars);
            Console.WriteLine("   What do you want to learn about cybersecurity?");
            Console.WriteLine(stars);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  1 - What can I ask you about");
            Console.WriteLine("  2 - What's your purpose");
            Console.WriteLine("  3 - How are you");
            Console.WriteLine("  4 - Safe browsing tips");
            Console.WriteLine("  5 - Phishing warning signs");
            Console.WriteLine("  6 - Password safety tips");
            Console.WriteLine("  0 - Exit the chatbot");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(stars);
            Console.ResetColor();
        }

        // Responds to the user asking how the chatbot is doing
        public static void HowAreYou(string name)
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("ChatBot: I'm doing well and ready to help you learn about cybersecurity, " + name + "!");
            Console.ResetColor();
        }

        // Explains what the chatbot is for when the user asks about its purpose
        public static void Purpose(string name)
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("ChatBot: Great question " + name + "!");
            Console.WriteLine("ChatBot: I am here to help teach you about cybersecurity.");
            Console.WriteLine("ChatBot: You can ask me about safe browsing, phishing and password safety.");
            Console.ResetColor();
        }

        // Safe browsing tips when the user asks about them
        public static void SafeBrowsing()
        {
            // Store all the tips in variables
            string tip1 = "Always check for HTTPS in the URL before entering data.";
            string tip2 = "Avoid clicking suspicious links in emails or messages.";
            string tip3 = "Keep your browser and extensions up to date.";
            string tip4 = "Use an ad-blocker to reduce exposure to malicious ads.";
            string tip5 = "Avoid using public Wi-Fi for banking without a VPN.";

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(stars);
            Console.WriteLine("   Safe Browsing Tips");
            Console.WriteLine(stars);
            Console.ResetColor();

            // Print each tip with a number
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  1: " + tip1);
            Console.WriteLine("  2: " + tip2);
            Console.WriteLine("  3: " + tip3);
            Console.WriteLine("  4: " + tip4);
            Console.WriteLine("  5: " + tip5);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(stars);
            Console.ResetColor();
        }

        // This explains how to spot a phishing attempt
        public static void Phishing()
        {
            string flag1 = "Urgent language like - Your account will be closed!";
            string flag2 = "Sender addresses that look similar but are slightly off.";
            string flag3 = "Links where the hover URL does not match the displayed text.";
            string flag4 = "Requests for passwords or card details via email.";
            string flag5 = "Poor grammar, odd formatting or unexpected attachments.";

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(stars);
            Console.WriteLine("   Phishing Warning Signs");
            Console.WriteLine(stars);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  1: " + flag1);
            Console.WriteLine("  2: " + flag2);
            Console.WriteLine("  3: " + flag3);
            Console.WriteLine("  4: " + flag4);
            Console.WriteLine("  5: " + flag5);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("  * You should never click suspicious links! *");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(stars);
            Console.ResetColor();
        }

        // This shares password safety advice
        public static void PasswordSafety()
        {
            string advice1 = "Use at least 12 characters with letters, numbers and symbols.";
            string advice2 = "Never reuse the same password across different accounts.";
            string advice3 = "Enable two-factor authentication wherever possible.";
            string advice4 = "Change your passwords regularly.";
            string advice5 = "Avoid using personal info like birthdays or pet names.";

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(stars);
            Console.WriteLine("   Password Safety Tips");
            Console.WriteLine(stars);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  1: " + advice1);
            Console.WriteLine("  2: " + advice2);
            Console.WriteLine("  3: " + advice3);
            Console.WriteLine("  4: " + advice4);
            Console.WriteLine("  5: " + advice5);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(stars);
            Console.ResetColor();
        }

        // This handles input that the chatbot does not recognize
        public static void Validation()
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ChatBot: Please enter a valid option from the menu.");
            Console.WriteLine("ChatBot: Type 1 to see the list of available topics.");
            Console.ResetColor();
        }

        // Quit method
        public static void Exit(string name)
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(stars);
            Console.WriteLine("  Goodbye " + name + "! Stay safe online.");
            Console.WriteLine(stars);
            Console.ResetColor();
        }
    }
}