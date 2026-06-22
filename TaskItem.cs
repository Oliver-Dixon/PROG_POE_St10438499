//Task model

namespace Chatbot
{
    
    public class TaskItem
    {
        public int Id = 0;                          // Database id for the task
        public string Title = "";                   // Short title for the task
        public string Description = "";              // Fuller description of what needs doing
        public DateTime? ReminderDate = null;       // Optional reminder - null means no reminder set
        public bool IsCompleted = false;            // Tracks whether the task is done
        public DateTime CreatedDate = DateTime.Now; // When the task was first created
    }
}