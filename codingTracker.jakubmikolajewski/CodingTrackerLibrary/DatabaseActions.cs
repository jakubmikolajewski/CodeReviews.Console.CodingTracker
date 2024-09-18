using System.Data.SQLite;

namespace CodingTrackerLibrary
{
    public class DatabaseActions
    {
        private const string _connectionString = "Data Source=CodingTrackerDatabase.sqlite;Version=3;New=False";
        private readonly SQLiteConnection _connection;

        public DatabaseActions()
        {
            _connection = new SQLiteConnection(_connectionString);
        }
        public SQLiteConnection Connection
        {
            get => _connection;
        }

        public void CreateDatabaseAndTableIfNotExists()
        {
            if (!File.Exists("CodingTrackerDatabase.sqlite"))
            {
                try
                {
                    SQLiteConnection.CreateFile("CodingTrackerDatabase.sqlite");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception during database creation.\nDetails: {ex.Message}");
                }
                CreateTableQuery();
            }
        }
        private void CreateTableQuery()
        {
            DatabaseActions current = new DatabaseActions();
            using (current.Connection)
            {     
                try
                {
                    current.Connection.Open();
                    using (SQLiteCommand command = current.Connection.CreateCommand())
                    {
                        command.CommandText = $"CREATE TABLE 'Coding sessions' (Id INTEGER PRIMARY KEY, StartDate TEXT, EndDate TEXT, Duration INT)";
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred during table creation.\nDetails: {ex.Message}");
                }
                finally
                {
                    current.Connection.Close();
                }   
            }
        }

    }
}
