//Methods
using System.Media;

namespace Chatbot
{
    public class ChatbotFunctions
    {
        // Saves time by creating a global variable for the star separator used in multiple places
        static string stars = "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *";

        // Used to pick a random response so the bot feels more varied
        static Random random = new Random();

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
            output += "ChatBot: You can pick a number from the menu or just ask me about a topic.\n";
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
            output += "  Or type a question with words like password, phishing, privacy, wifi, virus or hack.\n";
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

        // Looks for cybersecurity keywords in the user's input and returns a relevant response
        // Returns an empty string if no keyword is found
        public static string CheckKeywords(string input)
        {
            // Convert to lowercase so we catch words regardless of how they were typed
            string lowerInput = input.ToLower();

            // Password keyword with varied responses
            if (lowerInput.Contains("password"))
            {
                string[] responses = {
                    "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
                    "A password manager can help you create and store strong passwords for every site you use.",
                    "Strong passwords should be at least 12 characters and mix letters, numbers and symbols."
                };
                return "\nChatBot: " + responses[random.Next(responses.Length)] + "\n";
            }

            // Phishing or scam keyword
            if (lowerInput.Contains("phishing") || lowerInput.Contains("scam"))
            {
                string[] responses = {
                    "Be careful with emails that create urgency or ask for personal information. Always verify the sender first.",
                    "Phishing scams often use fake links. Hover over a link before clicking to see where it really goes.",
                    "If an email looks suspicious, do not click any links. Go directly to the official website instead."
                };
                return "\nChatBot: " + responses[random.Next(responses.Length)] + "\n";
            }

            // Privacy keyword
            if (lowerInput.Contains("privacy"))
            {
                string[] responses = {
                    "Review your privacy settings on social media regularly and limit what you share publicly.",
                    "Be careful what apps you give permissions to. Only allow access that is actually needed.",
                    "Your privacy matters. Avoid sharing personal details like your address or birthday online."
                };
                return "\nChatBot: " + responses[random.Next(responses.Length)] + "\n";
            }

            // Wi-Fi keyword
            if (lowerInput.Contains("wifi") || lowerInput.Contains("wi-fi"))
            {
                string[] responses = {
                    "Public Wi-Fi networks are not secure. Avoid logging into important accounts when connected to them.",
                    "If you need to use public Wi-Fi, a VPN will help keep your traffic encrypted and safe.",
                    "Always make sure your home Wi-Fi is password protected with WPA2 or WPA3 encryption."
                };
                return "\nChatBot: " + responses[random.Next(responses.Length)] + "\n";
            }

            // Virus or malware keyword
            if (lowerInput.Contains("virus") || lowerInput.Contains("malware"))
            {
                string[] responses = {
                    "Keep your antivirus software up to date and run regular scans to catch threats early.",
                    "Never download files or attachments from sources you do not trust as they may contain malware.",
                    "Free software downloaded from random sites often comes bundled with viruses. Stick to official sources."
                };
                return "\nChatBot: " + responses[random.Next(responses.Length)] + "\n";
            }

            // Hack keyword
            if (lowerInput.Contains("hack"))
            {
                string[] responses = {
                    "If you think your account has been hacked, change your password immediately and enable two-factor authentication.",
                    "Hackers often target weak passwords. Strong unique passwords are your best defence.",
                    "Check haveibeenpwned.com to see if your email has been involved in any known data breaches."
                };
                return "\nChatBot: " + responses[random.Next(responses.Length)] + "\n";
            }

            // No keyword matched
            return "";
        }

        // This handles input that the chatbot does not recognize
        public static string Validation()
        {
            string output = "";
            output += "\nChatBot: Please enter a valid option from the menu or ask about a cybersecurity topic.\n";
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