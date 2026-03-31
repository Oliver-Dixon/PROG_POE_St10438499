// Program.cs
namespace Chatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatbotFunctions.ShowGreeting();
            bool running = true;
            while (running)
            {
                Console.Write("You: ");
                string? rawInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(rawInput))
                {
                    Console.WriteLine("CyberBot: Please type something before pressing Enter.\n");
                    continue;
                }
                string input = rawInput.Trim().ToLower();
                switch (input)
                {
                    case "how are you": ChatbotFunctions.RespondHowAreYou(); break;
                    case "purpose": ChatbotFunctions.RespondPurpose(); break;
                    case "safe browsing": ChatbotFunctions.RespondSafeBrowsing(); break;
                    case "phishing": ChatbotFunctions.RespondPhishing(); break;
                    case "password safety": ChatbotFunctions.RespondPasswordSafety(); break;
                    case "help": ChatbotFunctions.ShowHelp(); break;
                    case "exit":
                        Console.WriteLine("CyberBot: Stay safe out there. Goodbye.\n");
                        running = false;
                        break;
                    default: ChatbotFunctions.RespondUnknown(rawInput.Trim()); break;
                }
            }
        }
    }
}
