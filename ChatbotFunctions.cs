//Methods
using System.Media;

namespace Chatbot
{
    public class ChatbotFunctions
    {
        // Saves time by creating a global variable for the star separator used in multiple places
        static string stars = "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *";

        // Voice greeting
        public static void PlayVoiceGreeting()
        {
            try
            {
                string audioPath = "greeting.wav";

                // Check if the file is there before playing it
                if (File.Exists(audioPath))
                {
                    using var player = new SoundPlayer(audioPath);
                    // Use Play so the audio plays without freezing the GUI
                    player.Play();
                }
            }
            catch (Exception error)
            {
                // Silent fail so the GUI still loads if audio is missing
                Console.WriteLine("Audio error: " + error.Message);
            }
        }

        // Greeting method
        public static string Greeting()
        {
            string output = "";
            output += stars + "\n";
            output += @"
   ___  _   _   ___   ____  ___   ___   ____ 
  /  _|| |_| | / _ \ |_  _|| _ ) / _ \ |_  _|
  | |_ |  _  || |_| | | |  | _ || |_| | | |  
  \___||_| |_||_| |_| |_|  |___/ \___/  |_|  

        Cybersecurity Awareness Bot  v1.0
" + "\n";
            output += stars + "\n";
            output += "       CyberBot - Your Cybersecurity Assistant\n";
            output += stars + "\n\n";
            return output;
        }

        // Welcome message after the user enters their name
        public static string Hello(string name)
        {
            string output = "";
            output += "ChatBot: Hello " + name + "!\n";
            output += "ChatBot: I'm here to help you learn about cybersecurity.\n";
            return output;
        }

        // This shows the user the list of topics they can ask about
        public static string Help()
        {
            string output = "";
            output += "\n" + stars + "\n";
            output += "   What do you want to learn about cybersecurity?\n";
            output += stars + "\n";
            output += "  1 - What can I ask you about\n";
            output += "  2 - What's your purpose\n";
            output += "  3 - How are you\n";
            output += "  4 - Safe browsing tips\n";
            output += "  5 - Phishing warning signs\n";
            output += "  6 - Password safety tips\n";
            output += "  0 - Exit the chatbot\n";
            output += stars + "\n";
            return output;
        }

        // Responds to the user asking how the chatbot is doing
        public static string HowAreYou(string name)
        {
            return "\nChatBot: I'm doing well and ready to help you learn about cybersecurity, " + name + "!\n";
        }

        // Explains what the chatbot is for when the user asks about its purpose
        public static string Purpose(string name)
        {
            string output = "";
            output += "\nChatBot: Great question " + name + "!\n";
            output += "ChatBot: I am here to help teach you about cybersecurity.\n";
            output += "ChatBot: You can ask me about safe browsing, phishing and password safety.\n";
            return output;
        }

        // Safe browsing tips when the user asks about them
        public static string SafeBrowsing()
        {
            // Store all the tips in variables
            string tip1 = "Always check for HTTPS in the URL before entering data.";
            string tip2 = "Avoid clicking suspicious links in emails or messages.";
            string tip3 = "Keep your browser and extensions up to date.";
            string tip4 = "Use an ad-blocker to reduce exposure to malicious ads.";
            string tip5 = "Avoid using public Wi-Fi for banking without a VPN.";

            string output = "";
            output += "\n" + stars + "\n";
            output += "   Safe Browsing Tips\n";
            output += stars + "\n";
            // Print each tip with a number
            output += "  1: " + tip1 + "\n";
            output += "  2: " + tip2 + "\n";
            output += "  3: " + tip3 + "\n";
            output += "  4: " + tip4 + "\n";
            output += "  5: " + tip5 + "\n";
            output += stars + "\n";
            return output;
        }

        // This explains how to spot a phishing attempt
        public static string Phishing()
        {
            string flag1 = "Urgent language like - Your account will be closed!";
            string flag2 = "Sender addresses that look similar but are slightly off.";
            string flag3 = "Links where the hover URL does not match the displayed text.";
            string flag4 = "Requests for passwords or card details via email.";
            string flag5 = "Poor grammar, odd formatting or unexpected attachments.";

            string output = "";
            output += "\n" + stars + "\n";
            output += "   Phishing Warning Signs\n";
            output += stars + "\n";
            output += "  1: " + flag1 + "\n";
            output += "  2: " + flag2 + "\n";
            output += "  3: " + flag3 + "\n";
            output += "  4: " + flag4 + "\n";
            output += "  5: " + flag5 + "\n";
            output += "\n  * You should never click suspicious links! *\n";
            output += stars + "\n";
            return output;
        }

        // This shares password safety advice
        public static string PasswordSafety()
        {
            string advice1 = "Use at least 12 characters with letters, numbers and symbols.";
            string advice2 = "Never reuse the same password across different accounts.";
            string advice3 = "Enable two-factor authentication wherever possible.";
            string advice4 = "Change your passwords regularly.";
            string advice5 = "Avoid using personal info like birthdays or pet names.";

            string output = "";
            output += "\n" + stars + "\n";
            output += "   Password Safety Tips\n";
            output += stars + "\n";
            output += "  1: " + advice1 + "\n";
            output += "  2: " + advice2 + "\n";
            output += "  3: " + advice3 + "\n";
            output += "  4: " + advice4 + "\n";
            output += "  5: " + advice5 + "\n";
            output += stars + "\n";
            return output;
        }

        // This handles input that the chatbot does not recognize
        public static string Validation()
        {
            string output = "";
            output += "\nChatBot: Please enter a valid option from the menu.\n";
            output += "ChatBot: Type 1 to see the list of available topics.\n";
            return output;
        }

        // Quit method
        public static string Exit(string name)
        {
            string output = "";
            output += "\n" + stars + "\n";
            output += "  Goodbye " + name + "! Stay safe online.\n";
            output += stars + "\n";
            return output;
        }
    }
}