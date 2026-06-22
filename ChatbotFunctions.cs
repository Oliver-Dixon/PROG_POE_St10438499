//Methods
using System.Media;

namespace Chatbot
{
    // Small class to group a topic's keywords and possible responses together
    // This makes it much easier to add new topics in Part 3
    public class KeywordTopic
    {
        public string[] Keywords = Array.Empty<string>();
        public string[] Responses = Array.Empty<string>();
        public string TopicName = "";
        public bool CanBeInterest = false;
    }

    // Class to group a sentiment with its trigger words and responses
    public class SentimentEntry
    {
        public string[] Keywords = Array.Empty<string>();
        public string SentimentName = "";
        public string AcknowledgeResponse = "";
        public string ResponsePrefix = "";
    }

    public class ChatbotFunctions
    {
        // Saves time by creating a global variable for the star separator used in multiple places
        static string stars = "* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *";

        // Used to pick a random response so the bot feels more varied
        static Random random = new Random();

        // Memory - stores the user's main interest so the bot can refer back to it later
        static string userInterest = "";

        // Stores the user's current sentiment so responses can adapt
        static string userSentiment = "";

        // List of all the cybersecurity topics the bot knows about
        // Add new entries here to teach the bot new topics
        static List<KeywordTopic> keywordTopics = new List<KeywordTopic>
        {
            new KeywordTopic
            {
                TopicName = "password safety",
                Keywords = new[] { "password" },
                CanBeInterest = true,
                Responses = new[]
                {
                    "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
                    "A password manager can help you create and store strong passwords for every site you use.",
                    "Strong passwords should be at least 12 characters and mix letters, numbers and symbols."
                }
            },
            new KeywordTopic
            {
                TopicName = "phishing",
                Keywords = new[] { "phishing", "scam" },
                CanBeInterest = true,
                Responses = new[]
                {
                    "Be careful with emails that create urgency or ask for personal information. Always verify the sender first.",
                    "Phishing scams often use fake links. Hover over a link before clicking to see where it really goes.",
                    "If an email looks suspicious, do not click any links. Go directly to the official website instead."
                }
            },
            new KeywordTopic
            {
                TopicName = "privacy",
                Keywords = new[] { "privacy" },
                CanBeInterest = true,
                Responses = new[]
                {
                    "Review your privacy settings on social media regularly and limit what you share publicly.",
                    "Be careful what apps you give permissions to. Only allow access that is actually needed.",
                    "Your privacy matters. Avoid sharing personal details like your address or birthday online."
                }
            },
            new KeywordTopic
            {
                TopicName = "safe browsing",
                Keywords = new[] { "browsing", "browser" },
                CanBeInterest = true,
                Responses = new[]
                {
                    "Always check for HTTPS in the URL bar before entering any personal data.",
                    "Keep your browser up to date to protect against known security flaws.",
                    "Use a reputable ad-blocker to reduce your exposure to malicious advertisements."
                }
            },
            new KeywordTopic
            {
                TopicName = "wifi",
                Keywords = new[] { "wifi", "wi-fi" },
                CanBeInterest = false,
                Responses = new[]
                {
                    "Public Wi-Fi networks are not secure. Avoid logging into important accounts when connected to them.",
                    "If you need to use public Wi-Fi, a VPN will help keep your traffic encrypted and safe.",
                    "Always make sure your home Wi-Fi is password protected with WPA2 or WPA3 encryption."
                }
            },
            new KeywordTopic
            {
                TopicName = "malware",
                Keywords = new[] { "virus", "malware" },
                CanBeInterest = false,
                Responses = new[]
                {
                    "Keep your antivirus software up to date and run regular scans to catch threats early.",
                    "Never download files or attachments from sources you do not trust as they may contain malware.",
                    "Free software downloaded from random sites often comes bundled with viruses. Stick to official sources."
                }
            },
            new KeywordTopic
            {
                TopicName = "hacking",
                Keywords = new[] { "hack" },
                CanBeInterest = false,
                Responses = new[]
                {
                    "If you think your account has been hacked, change your password immediately and enable two-factor authentication.",
                    "Hackers often target weak passwords. Strong unique passwords are your best defence.",
                    "Check haveibeenpwned.com to see if your email has been involved in any known data breaches."
                }
            }
        };

        // List of all sentiments the bot can detect
        // Add new entries here to teach the bot new emotions
        static List<SentimentEntry> sentiments = new List<SentimentEntry>
        {
            new SentimentEntry
            {
                SentimentName = "worried",
                Keywords = new[] { "worried", "scared", "anxious", "nervous" },
                AcknowledgeResponse = "I understand this can feel worrying. Don't stress - I'm here to help you stay safe step by step.",
                ResponsePrefix = "Don't worry, "
            },
            new SentimentEntry
            {
                SentimentName = "frustrated",
                Keywords = new[] { "frustrated", "angry", "annoyed", "confused" },
                AcknowledgeResponse = "I get that this can be frustrating. Let's break it down together so it feels easier to handle.",
                ResponsePrefix = "Hang in there, "
            },
            new SentimentEntry
            {
                SentimentName = "curious",
                Keywords = new[] { "curious", "wonder", "interesting" },
                AcknowledgeResponse = "It's great that you're curious! Asking questions is the best way to learn about cybersecurity.",
                ResponsePrefix = "Great question - "
            },
            new SentimentEntry
            {
                SentimentName = "overwhelmed",
                Keywords = new[] { "overwhelmed", "too much", "don't understand" },
                AcknowledgeResponse = "It's okay to feel overwhelmed - cybersecurity covers a lot. We can take it one small piece at a time.",
                ResponsePrefix = "Take it one step at a time. "
            }
        };

        // Dictionary of random tip lists for specific topics, used when the user asks for a tip
        static Dictionary<string, string[]> randomTips = new Dictionary<string, string[]>
        {
            { "phishing", new[]
                {
                    "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                    "Always check the sender's email address carefully. Phishers use addresses that look almost real but are slightly off.",
                    "Never click links in suspicious emails. Type the website address directly into your browser instead.",
                    "Look out for urgent language like 'Your account will be closed!' as this is a common phishing tactic.",
                    "If an email has poor grammar or unexpected attachments, it is likely a phishing attempt. Delete it."
                }
            },
            { "safe browsing", new[]
                {
                    "Always check for HTTPS in the URL bar before entering any personal data.",
                    "Keep your browser and all extensions up to date to protect against security flaws.",
                    "Use a reputable ad-blocker to reduce your exposure to malicious advertisements.",
                    "Avoid using public Wi-Fi for banking or shopping unless you are using a VPN.",
                    "Bookmark important websites so you don't accidentally visit a fake version through a search engine."
                }
            },
            { "password safety", new[]
                {
                    "Use a password manager to generate and store strong, unique passwords for every account.",
                    "Make passwords at least 12 characters long with a mix of letters, numbers and symbols.",
                    "Enable two-factor authentication on every account that supports it for extra security.",
                    "Never reuse passwords across multiple sites because one breach could compromise all your accounts.",
                    "Change your passwords straight away if a service you use reports a data breach."
                }
            }
        };

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
            output += "  7 - View and manage your cybersecurity tasks\n";
            output += "  8 - Play the cybersecurity quiz\n";
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
            // Pull the tip list from the dictionary
            string[] tips = randomTips["safe browsing"];

            string output = "";
            output += "\n" + stars + "\n";
            output += "   Safe Browsing Tips\n";
            output += stars + "\n";
            // Print each tip with a number
            for (int i = 0; i < tips.Length; i++)
            {
                output += "  " + (i + 1) + ": " + tips[i] + "\n";
            }
            output += stars + "\n";
            return output;
        }

        // This explains how to spot a phishing attempt
        public static string Phishing()
        {
            string[] tips = randomTips["phishing"];

            string output = "";
            output += "\n" + stars + "\n";
            output += "   Phishing Warning Signs\n";
            output += stars + "\n";
            for (int i = 0; i < tips.Length; i++)
            {
                output += "  " + (i + 1) + ": " + tips[i] + "\n";
            }
            output += "\n  * You should never click suspicious links! *\n";
            output += stars + "\n";
            return output;
        }

        // This shares password safety advice
        public static string PasswordSafety()
        {
            string[] tips = randomTips["password safety"];

            string output = "";
            output += "\n" + stars + "\n";
            output += "   Password Safety Tips\n";
            output += stars + "\n";
            for (int i = 0; i < tips.Length; i++)
            {
                output += "  " + (i + 1) + ": " + tips[i] + "\n";
            }
            output += stars + "\n";
            return output;
        }

        // Explains the task assistant commands in the same friendly menu style
        public static string TaskMenu()
        {
            // A divider line to match the look of the rest of the chatbot
            string line = "* * * * * * * * * * * * * * * * * * * * * * * * * * * *";

            string text = "\n" + line + "\n";
            text += "   CYBERSECURITY TASK ASSISTANT\n";
            text += line + "\n";
            text += "You can manage your tasks straight from the chat:\n";
            text += "  - Add a task:      add task - review my privacy settings\n";
            text += "  - View your tasks: view tasks\n";
            text += "  - Complete a task: complete task 2\n";
            text += "  - Delete a task:   delete task 2\n";
            text += "Or use the buttons in the Tasks panel on the right.\n";
            text += "When adding a task I'll ask if you'd like a reminder, e.g. 'in 3 days'.\n";
            return text;
        }

        // Returns one random tip from any of the random tip lists
        // Used when the user asks for a tip on a specific topic
        public static string GetRandomTip(string topicName)
        {
            if (!randomTips.ContainsKey(topicName))
                return "";

            string[] tips = randomTips[topicName];
            string chosenTip = tips[random.Next(tips.Length)];
            return "\nChatBot: " + GetSentimentPrefix() + GetRecallPrefix(topicName) + chosenTip + "\n";
        }

        // Checks if the user is expressing interest in a topic and stores it for later
        public static string CheckInterest(string input)
        {
            string lowerInput = input.ToLower();

            // Phrases that signal the user is sharing what they care about
            string[] interestPhrases = { "interested in", "want to learn about", "favourite", "favorite", "i like" };

            bool sharingInterest = interestPhrases.Any(phrase => lowerInput.Contains(phrase));
            if (!sharingInterest)
                return "";

            // Look through all topics that can be remembered as interests
            foreach (var topic in keywordTopics.Where(t => t.CanBeInterest))
            {
                if (topic.Keywords.Any(keyword => lowerInput.Contains(keyword)))
                {
                    userInterest = topic.TopicName;
                    return "\nChatBot: Great! I'll remember that you're interested in " + topic.TopicName +
                           ". It's a crucial part of staying safe online.\n";
                }
            }

            return "";
        }

        // Detects how the user is feeling based on words they used
        // Updates the userSentiment so other responses can adjust their tone
        public static string CheckSentiment(string input)
        {
            string lowerInput = input.ToLower();

            // Loop through every sentiment and see if any keywords match
            foreach (var sentiment in sentiments)
            {
                if (sentiment.Keywords.Any(keyword => lowerInput.Contains(keyword)))
                {
                    userSentiment = sentiment.SentimentName;
                    return "\nChatBot: " + sentiment.AcknowledgeResponse + "\n";
                }
            }

            return "";
        }

        // Returns a small encouraging phrase based on the user's current sentiment
        private static string GetSentimentPrefix()
        {
            // Find the matching sentiment in the list and return its prefix
            var match = sentiments.FirstOrDefault(s => s.SentimentName == userSentiment);
            return match?.ResponsePrefix ?? "";
        }

        // Returns a recall phrase if the topic matches what the user is interested in
        private static string GetRecallPrefix(string topic)
        {
            if (string.Equals(userInterest, topic, StringComparison.OrdinalIgnoreCase))
            {
                string[] prefixes =
                {
                    "as someone interested in " + topic + ", ",
                    "since you mentioned you're into " + topic + ", ",
                    "remembering your interest in " + topic + ", "
                };
                return prefixes[random.Next(prefixes.Length)];
            }
            return "";
        }

        // Looks for cybersecurity keywords in the user's input and returns a relevant response
        // Returns an empty string if no keyword is found
        public static string CheckKeywords(string input)
        {
            // First check if the user is expressing a feeling - this updates the stored sentiment
            string sentimentResponse = CheckSentiment(input);

            // Then check if the user is sharing an interest
            string interestResponse = CheckInterest(input);
            if (!string.IsNullOrEmpty(interestResponse))
                return interestResponse;

            // Convert to lowercase so we catch words regardless of how they were typed
            string lowerInput = input.ToLower();

            // Check if the user is asking for a tip or advice on something
            bool askingForTip = lowerInput.Contains("tip") || lowerInput.Contains("advice");

            if (askingForTip)
            {
                // Loop through tip topics and return the matching random tip
                foreach (var topicKey in randomTips.Keys)
                {
                    // Find a keyword topic that maps to this random tip key
                    var matchingTopic = keywordTopics.FirstOrDefault(t => t.TopicName == topicKey);
                    if (matchingTopic != null && matchingTopic.Keywords.Any(k => lowerInput.Contains(k)))
                    {
                        return GetRandomTip(topicKey);
                    }
                }
            }

            // Loop through all keyword topics and return a response for the first match
            foreach (var topic in keywordTopics)
            {
                if (topic.Keywords.Any(keyword => lowerInput.Contains(keyword)))
                {
                    string chosenResponse = topic.Responses[random.Next(topic.Responses.Length)];
                    return "\nChatBot: " + GetSentimentPrefix() + GetRecallPrefix(topic.TopicName) + chosenResponse + "\n";
                }
            }

            // If no keyword matched but a sentiment was detected, return the sentiment response on its own
            if (!string.IsNullOrEmpty(sentimentResponse))
                return sentimentResponse;

            // No keyword or sentiment matched
            return "";
        }

        // Default response shown when the input is not recognised at all
        public static string Validation()
        {
            string[] responses =
            {
                "I'm not sure I understand. Can you try rephrasing?",
                "Hmm, I didn't quite catch that. Could you ask in a different way?",
                "Sorry, that one went over my head. Could you rephrase your question?",
                "I'm not sure what you mean. Try mentioning a cybersecurity topic like passwords, phishing or privacy."
            };

            string output = "";
            output += "\nChatBot: " + responses[random.Next(responses.Length)] + "\n";
            output += "ChatBot: You can also type 1 to see the list of available topics.\n";
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