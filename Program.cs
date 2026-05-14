//Main program

namespace Chatbot
{
    class Program
    {
        // Required for WinForms applications
        [STAThread]
        static void Main()
        {
            // Plays the voice greeting before the GUI loads
            ChatbotFunctions.PlayVoiceGreeting();

            // Starts the WinForms application
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}