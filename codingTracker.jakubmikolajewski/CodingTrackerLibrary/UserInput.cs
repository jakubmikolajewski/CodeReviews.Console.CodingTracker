using System.Diagnostics;

namespace CodingTrackerLibrary
{
    public class UserInput
    {
        public static Stopwatch timer = new Stopwatch();
        public static bool SwitchMenuChoice(string menuChoice)
        {
            switch (menuChoice)
            {
                case "v":
                    TableVisualizationEngine.ShowPeriodMenu();
                    DateTime range = SwitchPeriodMenuChoices(Validator.ValidateMenuChoice(Validator.periodMenuChoices));
                    CodingController.SelectAllSessionsQuery();
                    TableVisualizationEngine.SelectGivenPeriodQuery(range);
                    break;
                case "u":
                    CodingController.UpdateQuery();
                    break;
                case "d":
                    CodingController.DeleteQuery();
                    break;
                case "i":
                    CodingController.InsertQuery();
                    break;
                case "r":
                    TableVisualizationEngine.ShowReportsMenu();
                    SwitchReportsMenuChoice(Validator.ValidateMenuChoice(Validator.reportsMenuChoices));
                    break;
                case "m":
                    string startDate = MeasureSessionTime();
                    (string endDate, int duration) = StopTimer();
                    CodingController.InsertMeasuredSession(startDate, endDate, duration);
                    break;
                case "s":
                    if (!CodingController.CheckIfCodingGoalsExists())
                        CodingController.CreateCodingGoals();
                    (string goalName, int desiredHours, string reachGoalBy) = ChooseGoal();
                    CodingController.InsertCodingGoal(goalName, desiredHours, reachGoalBy);
                    break;
                case "g":
                    CodingController.SelectAllSessionsQuery();
                    CodingController.SelectAllGoalsQuery();
                    TableVisualizationEngine.ShowCurrentGoalsStatus();
                    break;
                case "exit":
                    return true;
            }
            return false;
        }

        private static void SwitchReportsMenuChoice(string reportsMenuChoice)
        {
            switch (reportsMenuChoice)
            {
                case "t":
                    TableVisualizationEngine.ShowPeriodMenu();
                    DateTime range = SwitchPeriodMenuChoices(Validator.ValidateMenuChoice(Validator.periodMenuChoices));
                    CodingController.SelectAllSessionsQuery();
                    TableVisualizationEngine.SelectTotalHoursQuery(range);
                    break;
                case "a":
                    TableVisualizationEngine.ShowPeriodMenu();
                    range = SwitchPeriodMenuChoices(Validator.ValidateMenuChoice(Validator.periodMenuChoices));
                    CodingController.SelectAllSessionsQuery();
                    TableVisualizationEngine.SelectAverageSessionLength(range);
                    break;
                case "menu":
                    break;
            }
        }
        public static int ChooseRow()
        {
            Console.WriteLine("Which row would you like to affect? Please enter Id:\n");
            return Validator.ValidateRowChoice();
        }
        public static void ChooseColumns()
        {
            Console.WriteLine("Which columns would you like to edit (comma separated):\n");
            Validator.ValidateColumns();
        }
        public static void ChooseValues()
        {
            foreach (string column in CodingController.valuesToUpdate.Keys)
            {
                Console.WriteLine($"Please enter value for column {column}:\n");
                CodingController.valuesToUpdate[column] = Validator.ValidateDate();
            }
        }
        public static string ChooseStartDate()
        {
            Console.WriteLine("Please choose a start date (yyyy-MM-dd HH:mm): ");
            string startDate = Validator.ValidateDate();
            return startDate;
        }
        public static string ChooseEndDate()
        {
            Console.WriteLine("Please choose a start date (yyyy-MM-dd HH:mm): ");
            string endDate = Validator.ValidateDate();
            return endDate;
        }
        public static string ChooseNowAsDateTime()
        {
            Console.WriteLine("If you wish to use the current datetime as an entry, type 'now'. Else, enter a date (yyyy-MM-dd HH:mm):\n");
            string? input = Console.ReadLine();
            if (input is not null)
            {
                return input;
            }
            else return "";
        }
        public static string MeasureSessionTime()
        {
            string startDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            timer = Stopwatch.StartNew();
            Console.WriteLine("Timer has started.\n" +
                "\nPress any key to stop timer.");
            Console.ReadLine();
            return startDate;
        }
        public static (string, int) StopTimer()
        {
            timer.Stop();
            string endDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            int duration = Convert.ToInt32(timer.Elapsed.TotalMinutes);
            return (endDate, duration);
        }
        public static DateTime SwitchPeriodMenuChoices(string periodMenuChoice)
        {
            switch (periodMenuChoice)
            {
                case "days":
                    int days = ChoosePeriod();
                    return DateTime.Now.AddDays(-days);
                case "weeks":
                    int weeks = ChoosePeriod();
                    return DateTime.Now.AddDays(-weeks*7);
                case "months":
                    int months = ChoosePeriod();
                    return DateTime.Now.AddMonths(-months);
                case "years":
                    int years = ChoosePeriod();
                    return DateTime.Now.AddYears(-years);
                case "all":
                    return DateTime.MinValue;
            }
            return DateTime.MinValue;
        }
        private static int ChoosePeriod()
        {
            Console.WriteLine("How many?\n");
            return Validator.ValidateInt();
        }
        private static (string, int, string) ChooseGoal()
        {
            Console.WriteLine("Choose the goal's name:\n");
            string goalName = Validator.ValidateString();
            Console.WriteLine("Choose desired hours to reach:\n");
            int desiredHours = Validator.ValidateInt();
            Console.WriteLine("Choose date to reach goal by:\n");
            string reachGoalBy = Validator.ValidateDate();
            return (goalName, desiredHours, reachGoalBy);
        }
    }
}
