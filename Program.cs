// Program.cs
// This is the main file that runs the CyberBot chatbot

namespace Chatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Show the welcome screen first
            ChatbotFunctions.ShowGreeting();

            // Ask the user for their name and store it
            string userName = ChatbotFunctions.GetUserName();

            // This variable controls when the chatbot stops running
            bool running = true;

            // Keep looping until the user types exit
            while (running)
            {
                Console.WriteLine("");
                Console.Write("[You]: ");

                // Read what the user typed
                string? rawInput = Console.ReadLine();

                // Check if the user typed nothing
                if (string.IsNullOrWhiteSpace(rawInput))
                {
                    Console.WriteLine("CyberBot: I didn't quite understand that. Could you rephrase?");
                    Console.WriteLine("");
                    continue;
                }

                // Convert input to lowercase so it matches easier
                string userInput = rawInput.Trim().ToLower();

                // Check what the user typed and call the right function
                if (userInput == "how are you")
                {
                    ChatbotFunctions.RespondHowAreYou(userName);
                }
                else if (userInput == "what's your purpose" || userInput == "purpose")
                {
                    ChatbotFunctions.RespondPurpose(userName);
                }
                else if (userInput == "what can i ask you about" || userInput == "help")
                {
                    ChatbotFunctions.ShowHelp();
                }
                else if (userInput == "safe browsing")
                {
                    ChatbotFunctions.RespondSafeBrowsing();
                }
                else if (userInput == "phishing")
                {
                    ChatbotFunctions.RespondPhishing();
                }
                else if (userInput == "password safety")
                {
                    ChatbotFunctions.RespondPasswordSafety();
                }
                else if (userInput == "exit")
                {
                    // Say goodbye and stop the loop
                    ChatbotFunctions.ShowGoodbye(userName);
                    running = false;
                }
                else
                {
                    // If nothing matched tell the user
                    ChatbotFunctions.RespondUnknown();
                }
            }
        }
    }
}