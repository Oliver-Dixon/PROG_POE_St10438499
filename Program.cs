//Main program

namespace Chatbot
{
    class Program
    {
        static void Main(string[] args)
        {

            //voice greeting 
            ChatbotFunctions.VoiceGreeting();

            // Welcome screen
            ChatbotFunctions.Greeting();
            
            // Ask the user for their name and store it
            string userName = ChatbotFunctions.UserName();

            // Help menu
            ChatbotFunctions.Help();

            // Keeps the program running until the user types 0 to exit
            bool running = true;
            while (running)
            {
                Console.WriteLine("");
                Console.Write("[" + userName + "]: ");

                // Read what the user typed
                string? rawInput = Console.ReadLine();

                // Check if the user typed nothing
                if (string.IsNullOrWhiteSpace(rawInput))
                {
                    Console.WriteLine("ChatBot: Please enter a valid option from the menu.");
                    continue;
                }

                // Store the input to compare later and trim any extra spaces
                string userInput = rawInput.Trim();

                // Check which number the user entered
                if (userInput == "1")
                {
                    ChatbotFunctions.Help();
                }
                else if (userInput == "2")
                {
                    ChatbotFunctions.Purpose(userName);
                }
                else if (userInput == "3")
                {
                    ChatbotFunctions.HowAreYou(userName);
                }
                else if (userInput == "4")
                {
                    ChatbotFunctions.SafeBrowsing();
                }
                else if (userInput == "5")
                {
                    ChatbotFunctions.Phishing();
                }
                else if (userInput == "6")
                {
                    ChatbotFunctions.PasswordSafety();
                }
                else if (userInput == "0")
                {
                    // Ends the program
                    ChatbotFunctions.Exit(userName);
                    running = false;
                }
                else
                {
                    // If nothing matched tell the user
                    ChatbotFunctions.Validation();
                }
            }
        }
    }
}