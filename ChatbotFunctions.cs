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

        // Memory - stores the user's main interest so the bot can refer back to it later
        static string userInterest = "";

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
            output += "  Tip: tell me what you're interested in and I'll remember it!\n";
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

        // Returns a single random phishing tip
        public static string RandomPhishingTip()
        {
            string[] tips = {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "Always check the sender's email address carefully. Phishers use addresses that look almost real but are slightly off.",
                "Never click links in suspicious emails. Type the website address directly into your browser instead.",
                "Look out for urgent language like 'Your account will be closed!' as this is a common phishing tactic.",
                "If an email has poor grammar or unexpected attachments, it is likely a phishing attempt. Delete it."
            };

            return "\nChatBot: " + GetRecallPrefix("phishing") + tips[random.Next(tips.Length)] + "\n";
        }

        // Returns a single random safe browsing tip
        public static string RandomBrowsingTip()
        {
            string[] tips = {
                "Always check for HTTPS in the URL bar before entering any personal data.",
                "Keep your browser and all extensions up to date to protect against security flaws.",
                "Use a reputable ad-blocker to reduce your exposure to malicious advertisements.",
                "Avoid using public Wi-Fi for banking or shopping unless you are using a VPN.",
                "Bookmark important websites so you don't accidentally visit a fake version through a search engine."
            };

            return "\nChatBot: " + GetRecallPrefix("safe browsing") + tips[random.Next(tips.Length)] + "\n";
        }

        // Returns a single random password safety tip
        public static string RandomPasswordTip()
        {
            string[] tips = {
                "Use a password manager to generate and store strong, unique passwords for every account.",
                "Make passwords at least 12 characters long with a mix of letters, numbers and symbols.",
                "Enable two-factor authentication on every account that supports it for extra security.",
                "Never reuse passwords across multiple sites because one breach could compromise all your accounts.",
                "Change your passwords straight away if a service you use reports a data breach."
            };

            return "\nChatBot: " + GetRecallPrefix("password safety") + tips[random.Next(tips.Length)] + "\n";
        }

        // Checks if the user is expressing interest in a topic and stores it for later
        public static string CheckInterest(string input)
        {
            string lowerInput = input.ToLower();

            // Phrases that signal the user is sharing what they care about
            bool sharingInterest = lowerInput.Contains("interested in") ||
                                   lowerInput.Contains("want to learn about") ||
                                   lowerInput.Contains("favourite") ||
                                   lowerInput.Contains("favorite") ||
                                   lowerInput.Contains("i like");

            if (!sharingInterest)
                return "";

            // Figure out which topic they mentioned and save it
            if (lowerInput.Contains("password"))
            {
                userInterest = "password safety";
                return "\nChatBot: Great! I'll remember that you're interested in password safety. It's a crucial part of staying safe online.\n";
            }
            if (lowerInput.Contains("phishing") || lowerInput.Contains("scam"))
            {
                userInterest = "phishing";
                return "\nChatBot: Great! I'll remember that you're interested in phishing awareness. It's a crucial part of staying safe online.\n";
            }
            if (lowerInput.Contains("privacy"))
            {
                userInterest = "privacy";
                return "\nChatBot: Great! I'll remember that you're interested in privacy. It's a crucial part of staying safe online.\n";
            }
            if (lowerInput.Contains("browsing") || lowerInput.Contains("browser"))
            {
                userInterest = "safe browsing";
                return "\nChatBot: Great! I'll remember that you're interested in safe browsing. It's a crucial part of staying safe online.\n";
            }

            return "";
        }

        // Returns a recall phrase if the topic matches what the user is interested in
        // For example "As someone interested in privacy, "
        private static string GetRecallPrefix(string topic)
        {
            if (string.Equals(userInterest, topic, StringComparison.OrdinalIgnoreCase))
            {
                string[] prefixes = {
                    "As someone interested in " + topic + ", ",
                    "Since you mentioned you're into " + topic + ", ",
                    "Remembering your interest in " + topic + ", "
                };
                return prefixes[random.Next(prefixes.Length)];
            }
            return "";
        }

        // Looks for cybersecurity keywords in the user's input and returns a relevant response
        // Returns an empty string if no keyword is found
        public static string CheckKeywords(string input)
        {
            // First check if the user is sharing an interest
            string interestResponse = CheckInterest(input);
            if (!string.IsNullOrEmpty(interestResponse))
                return interestResponse;

            // Convert to lowercase so we catch words regardless of how they were typed
            string lowerInput = input.ToLower();

            // Check if the user is asking for a tip or advice on something
            bool askingForTip = lowerInput.Contains("tip") || lowerInput.Contains("advice");

            // If they want a tip on a specific topic give them one random tip from that topic
            if (askingForTip)
            {
                if (lowerInput.Contains("phishing") || lowerInput.Contains("scam"))
                    return RandomPhishingTip();
                if (lowerInput.Contains("browsing") || lowerInput.Contains("browser"))
                    return RandomBrowsingTip();
                if (lowerInput.Contains("password"))
                    return RandomPasswordTip();
            }

            // General keyword matching for when the user is not asking for a tip directly

            // Password keyword with varied responses
            if (lowerInput.Contains("password"))
            {
                string[] responses = {
                    "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
                    "A password manager can help you create and store strong passwords for every site you use.",
                    "Strong passwords should be at least 12 characters and mix letters, numbers and symbols."
                };
                return "\nChatBot: " + GetRecallPrefix("password safety") + responses[random.Next(responses.Length)] + "\n";
            }

            // Phishing or scam keyword
            if (lowerInput.Contains("phishing") || lowerInput.Contains("scam"))
            {
                string[] responses = {
                    "Be careful with emails that create urgency or ask for personal information. Always verify the sender first.",
                    "Phishing scams often use fake links. Hover over a link before clicking to see where it really goes.",
                    "If an email looks suspicious, do not click any links. Go directly to the official website instead."
                };
                return "\nChatBot: " + GetRecallPrefix("phishing") + responses[random.Next(responses.Length)] + "\n";
            }

            // Privacy keyword
            if (lowerInput.Contains("privacy"))
            {
                string[] responses = {
                    "Review your privacy settings on social media regularly and limit what you share publicly.",
                    "Be careful what apps you give permissions to. Only allow access that is actually needed.",
                    "Your privacy matters. Avoid sharing personal details like your address or birthday online."
                };
                return "\nChatBot: " + GetRecallPrefix("privacy") + responses[random.Next(responses.Length)] + "\n";
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