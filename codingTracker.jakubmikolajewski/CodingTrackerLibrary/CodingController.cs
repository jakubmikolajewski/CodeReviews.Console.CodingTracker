using Dapper;

namespace CodingTrackerLibrary
{   
    public class CodingController
    {
        public static Dictionary<string, string> valuesToUpdate = [];
        public static void SelectAllSessionsQuery()
        {        
            string command = $"SELECT * FROM 'Coding sessions';";

            DatabaseActions current = new DatabaseActions();
            using (current.Connection)
            {
                try
                {
                    using (var reader = current.Connection.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            CodingSession session = new CodingSession(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
                            TableVisualizationEngine.selectAllSessions.Add(session);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed update.\nDetails: {ex.Message}");
                }
            }
        }
        public static void UpdateQuery()
        {
            int rowChoice = UserInput.ChooseRow();
            List<CodingSession> currentRow = SelectSingleRowQuery(rowChoice);
            Console.WriteLine($"Updating row:\n" +
                $"Id: {currentRow.ElementAt(0).Id}, " +
                $"StartDate: {currentRow.ElementAt(0).StartDate}, " +
                $"EndDate: {currentRow.ElementAt(0).EndDate}, " +
                $"Duration: {currentRow.ElementAt(0).Duration}\n");

            UserInput.ChooseColumns();
            UserInput.ChooseValues();

            if (!valuesToUpdate.ContainsKey("StartDate"))
            {
                valuesToUpdate.Add("StartDate", currentRow.ElementAt(0).StartDate.ToString());
            }
            if (!valuesToUpdate.ContainsKey("EndDate"))
            {
                valuesToUpdate.Add("EndDate", currentRow.ElementAt(0).EndDate.ToString());
            }

            try
            {
                CodingSession session = new CodingSession(valuesToUpdate["StartDate"], valuesToUpdate["EndDate"]);

                string command = "UPDATE 'Coding sessions' SET StartDate = @StartDate, EndDate = @EndDate, Duration = @Duration WHERE Id = @Id ";
                object[] parameters = { new { Id = rowChoice,
                                          StartDate = valuesToUpdate["StartDate"],
                                          EndDate = valuesToUpdate["EndDate"],
                                          Duration = session.Duration } };
                DatabaseActions current = new DatabaseActions();
                using (current.Connection)
                {
                    try
                    {
                        current.Connection.Execute(command, parameters);
                        Console.WriteLine("Update successful!\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed update.\nDetails: {ex.Message}\n");
                    }
                }        
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                valuesToUpdate.Clear();
            }
        }
        public static void InsertQuery()
        {
            string startDate = UserInput.ChooseStartDate();
            string endDate = UserInput.ChooseEndDate();
            try
            {
                CodingSession session = new CodingSession(startDate, endDate);
                string command = "INSERT INTO 'Coding sessions' (StartDate, EndDate, Duration) VALUES (@StartDate, @EndDate, @Duration);";
                object[] parameters = { new { StartDate = session.StartDate, EndDate = session.EndDate, Duration = session.Duration } };

                DatabaseActions current = new DatabaseActions();
                using (current.Connection)
                {
                    try
                    {
                        current.Connection.Execute(command, parameters);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed insert.\nDetails: {ex.Message}");
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Insert successful.");
        }
        public static void DeleteQuery()
        {
            int rowChoice = UserInput.ChooseRow();
            string command = "DELETE FROM 'Coding sessions' WHERE Id = @Id;";
            object[] parameters = { new { Id = rowChoice } };

            DatabaseActions current = new DatabaseActions();
            using (current.Connection)
            {
                try
                {
                    current.Connection.Execute(command, parameters);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed deletion.\nDetails: {ex.Message}");
                }
            }
            Console.WriteLine("Deletion successful.");
        }

        public static List<CodingSession> SelectSingleRowQuery(int rowChoice)
        {     
            List<CodingSession> sessions = [];

            string command = $"SELECT * FROM 'Coding sessions' WHERE Id = '{rowChoice}';";
            Console.WriteLine(command);
            DatabaseActions current = new DatabaseActions();
            using (current.Connection)
            {
                try
                {
                    CodingSession session = current.Connection.QuerySingle<CodingSession>(command);
                    sessions.Add(session);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed single row query.\nDetails: {ex.Message}");
                }
            }
            return sessions;
        }
        public static List<long> SelectIdsQuery()
        {
            List<long> ids = [];
            string command = "SELECT Id FROM 'Coding sessions';";

            DatabaseActions current = new DatabaseActions();
            using (current.Connection)
            {
                try
                {
                    using (var reader = current.Connection.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            ids.Add(reader.GetInt64(0));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed select query.\nDetails: {ex.Message}");
                }
            }
            return ids;
        }
        public static void InsertMeasuredSession(string startDate, string endDate, string duration)
        {
            DatabaseActions current = new DatabaseActions();
            string command = "INSERT INTO 'Coding sessions' (StartDate, EndDate, Duration) VALUES (@StartDate, @EndDate, @Duration);";
            object[] parameters = {new {StartDate = startDate, EndDate = endDate, Duration = duration}};
            using (current.Connection)
            {
                try
                {
                    current.Connection.Execute(command, parameters);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed insert.\nDetails: {ex.Message}");
                }
            }
            Console.WriteLine("Insert successful.");
        }

        public static void CreateCodingGoals()
        {
            DatabaseActions current = new DatabaseActions();
            string command = "CREATE TABLE 'Coding goals' (GoalId INTEGER PRIMARY KEY, GoalName TEXT, DesiredHours INT, ReachGoalBy TEXT);";
            using (current.Connection)
            {
                try
                {
                    current.Connection.Execute(command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Creation failed.\nDetails: {ex.Message}");
                }
            }
        }
        public static bool CheckIfCodingGoalsExists()
        {
            DatabaseActions current = new DatabaseActions();
            string command = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'Coding goals'";
            using (current.Connection)
            {
                try
                {
                    var result = current.Connection.ExecuteScalar(command);
                    if (Convert.ToInt32(result) == 1)
                        return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Creation failed.\nDetails: {ex.Message}");
                }
            }
            return false;
        }
        public static void InsertCodingGoal(string goalName, int desiredHours, string reachGoalBy)
        {
            string command = "INSERT INTO 'Coding goals' (GoalName, DesiredHours, ReachGoalBy) VALUES (@GoalName, @DesiredHours, @ReachGoalBy);";
            object[] parameters = { new { GoalName = goalName, DesiredHours = desiredHours, ReachGoalBy = reachGoalBy } };

            DatabaseActions current = new DatabaseActions();
            using (current.Connection)
            {
                try
                {
                    current.Connection.Execute(command, parameters);
                    Console.WriteLine("Insert succeeded.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Insert failed.\nDetails: {ex.Message}");
                }
            }
        }
        public static void SelectAllGoalsQuery()
        {
            string command = "SELECT * FROM 'Coding goals';";
            DatabaseActions current = new DatabaseActions();
            using (current.Connection)
            {
                try
                {
                    using (var reader = current.Connection.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            CodingGoals goal = new CodingGoals(reader.GetInt64(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3));
                            TableVisualizationEngine.selectAllGoals.Add(goal);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Select failed.\nDetails: {ex.Message}");
                }
            }
        }        
    }
}
