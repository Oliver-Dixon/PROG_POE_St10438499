//Database helper

using MySql.Data.MySqlClient;

namespace Chatbot
{
    // Handles all the MySQL database work for storing and managing tasks
    // Keeping every bit of database code in one place makes it much easier to maintain
    public class DatabaseHelper
    {
        // Connection settings - change these to match your own MySQL setup
        // The server is usually localhost when MySQL is running on your own machine
        private static string server = "localhost";
        private static string user = "root";
        private static string password = "xxj00lsxx";         
        private static string databaseName = "cyberbot";

        
        private static string ServerConnection()
        {
            return $"Server={server};Uid={user};Pwd={password};";
        }

        
        private static string DatabaseConnection()
        {
            return $"Server={server};Database={databaseName};Uid={user};Pwd={password};";
        }

       
        public static void Initialise()
        {
            // First connect to the server and make the database if it's missing
            using (var connection = new MySqlConnection(ServerConnection()))
            {
                connection.Open();
                string createDb = "CREATE DATABASE IF NOT EXISTS " + databaseName + ";";
                using var command = new MySqlCommand(createDb, connection);
                command.ExecuteNonQuery();
            }

            // Now connect to the database and make the Tasks table if it's missing
            using (var connection = new MySqlConnection(DatabaseConnection()))
            {
                connection.Open();
                string createTable = @"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Title VARCHAR(255) NOT NULL,
                        Description TEXT,
                        ReminderDate DATETIME NULL,
                        IsCompleted BOOLEAN NOT NULL DEFAULT FALSE,
                        CreatedDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                    );";
                using var command = new MySqlCommand(createTable, connection);
                command.ExecuteNonQuery();
            }
        }

        // Adds a new task to the database and returns the new task's id
        public static int AddTask(TaskItem task)
        {
            using var connection = new MySqlConnection(DatabaseConnection());
            connection.Open();

            string insert = @"INSERT INTO Tasks (Title, Description, ReminderDate, IsCompleted)
                              VALUES (@title, @description, @reminder, @completed);";

            using var command = new MySqlCommand(insert, connection);
            // Parameters are used instead of joining strings so the input is safe from SQL injection
            command.Parameters.AddWithValue("@title", task.Title);
            command.Parameters.AddWithValue("@description", task.Description);
            // Store a proper NULL when there is no reminder set
            command.Parameters.AddWithValue("@reminder", (object?)task.ReminderDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@completed", task.IsCompleted);

            command.ExecuteNonQuery();

            // After an insert the command knows the new auto-increment id
            return (int)command.LastInsertedId;
        }

        // Gets every task from the database and returns them as a list
        public static List<TaskItem> GetAllTasks()
        {
            var tasks = new List<TaskItem>();

            using var connection = new MySqlConnection(DatabaseConnection());
            connection.Open();

            string select = "SELECT Id, Title, Description, ReminderDate, IsCompleted, CreatedDate FROM Tasks ORDER BY Id;";
            using var command = new MySqlCommand(select, connection);
            using var reader = command.ExecuteReader();

            // Read each row and turn it back into a TaskItem object
            while (reader.Read())
            {
                var task = new TaskItem();
                task.Id = reader.GetInt32("Id");
                task.Title = reader.GetString("Title");
                // Description and reminder are optional so guard against null values
                task.Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description");
                task.ReminderDate = reader.IsDBNull(reader.GetOrdinal("ReminderDate")) ? (DateTime?)null : reader.GetDateTime("ReminderDate");
                task.IsCompleted = reader.GetBoolean("IsCompleted");
                task.CreatedDate = reader.GetDateTime("CreatedDate");
                tasks.Add(task);
            }

            return tasks;
        }

        // Saves a reminder date against an existing task
        public static void UpdateReminder(int id, DateTime reminderDate)
        {
            using var connection = new MySqlConnection(DatabaseConnection());
            connection.Open();
            string update = "UPDATE Tasks SET ReminderDate = @reminder WHERE Id = @id;";
            using var command = new MySqlCommand(update, connection);
            command.Parameters.AddWithValue("@reminder", reminderDate);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        // Marks a task as completed in the database
        public static void MarkComplete(int id)
        {
            using var connection = new MySqlConnection(DatabaseConnection());
            connection.Open();
            string update = "UPDATE Tasks SET IsCompleted = TRUE WHERE Id = @id;";
            using var command = new MySqlCommand(update, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        // Deletes a task from the database using its id
        public static void DeleteTask(int id)
        {
            using var connection = new MySqlConnection(DatabaseConnection());
            connection.Open();
            string delete = "DELETE FROM Tasks WHERE Id = @id;";
            using var command = new MySqlCommand(delete, connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}