using Spectre.Console;

namespace CodingTrackerLibrary
{
    public class TableVisualizationEngine
    {
        public static List<CodingSession> selectAllSessions = [];
        public static List<CodingGoals> selectAllGoals = [];
        public static CodingSession? selectSingleRow;
        public static void ShowMenu()
        {
            var table = new Table();
            AnsiConsole.Markup("\n[underline green]Welcome to the Coding Tracker app![/]\n");
            table.AddColumn("[bold red]Please choose any of the following options:[/]");
            table.AddRow("'v' - View current entries");
            table.AddRow("'u' - Update current entries");
            table.AddRow("'i' - Insert new entry");
            table.AddRow("'d' - Delete current entries");
            table.AddRow("'r' - Generate report");
            table.AddRow("'m' - Measure current session");
            table.AddRow("'s' - Set a coding goal");
            table.AddRow("'g' - View current goals");
            table.AddRow("'exit' - Close the app.");
            AnsiConsole.Write(table);
        }
        public static void ShowReportsMenu()
        {
            var table = new Table();
            table.AddColumn("[bold red]Which report would you like to generate:[/]");
            table.AddRow("'t' - Total hours of coding");
            table.AddRow("'a' - Average lenght of session");
            table.AddRow("'menu' - Go back to the main menu.");         
            AnsiConsole.Write(table);
        }
        public static void ShowPeriodMenu()
        {
            var table = new Table();
            table.AddColumn("[bold red]Choose a period of time the query will return results for:[/]");
            table.AddRow("'days' - Number of days");
            table.AddRow("'weeks' - Number of weeks");
            table.AddRow("'months' - Number of months");
            table.AddRow("'years' - Number of years'");
            table.AddRow("'all' - All time");
            AnsiConsole.Write(table);
        }

        public static void ShowSingleRow()
        {
            var table = new Table();
            table.Title = new TableTitle("Updating row", style: "yellow");
            string[] columns = ["Id", "StartDate", "EndDate", "Duration"];
            table.AddColumns(columns);
            if (selectSingleRow is not null)
            {
                table.AddRow(selectSingleRow.Id.ToString(), selectSingleRow.StartDate.ToString("yyyy-MM-dd HH:mm"), selectSingleRow.EndDate.ToString("yyyy-MM-dd HH:mm"), selectSingleRow.Duration.ToString());
            }
            AnsiConsole.Write(table);
        }
        public static void ShowCurrentGoalsStatus()
        {
            string remainingFormatted;
            string perDayFormatted;
            var table = new Table();
            string[] columns = ["GoalId", "GoalName", "DesiredHours", "ReachGoalBy", "RemainingToGoal", "HoursADay"];
            table.AddColumns(columns);
            int currentTotalMinutes = selectAllSessions.Sum(x => x.Duration);

            foreach (CodingGoals goal in selectAllGoals)
            {
                int remainingToGoal = (goal.DesiredHours * 60) - currentTotalMinutes;
                if (remainingToGoal > 0)
                {
                    remainingFormatted = GetHoursMinutesFormat(remainingToGoal);

                    TimeSpan ts = goal.ReachGoalBy - DateTime.Now;
                    double hoursPerDay = remainingToGoal / ts.Days;
                    perDayFormatted = GetHoursMinutesFormat(Convert.ToInt32(hoursPerDay));
                }
                else
                {
                    remainingFormatted = "Congrats! You have reached your goal!";
                    perDayFormatted = "0";
                }
                table.AddRow(goal.GoalId.ToString(), goal.GoalName, goal.DesiredHours.ToString(), goal.ReachGoalBy.ToString("yyyy-MM-dd HH:mm"), remainingFormatted, perDayFormatted);
            }
            AnsiConsole.Write(table);
            selectAllGoals.Clear();
            selectAllSessions.Clear();
        }

        public static void SelectTotalHoursQuery(DateTime range)
        {
            var table = new Table();
            table.AddColumn("Total hours for the given period");
            int sum = selectAllSessions.Where(x => x.StartDate >= range)
                               .Sum(x => x.Duration);
            string formatted = GetHoursMinutesFormat(Convert.ToInt32(sum));
            table.AddRow($"{formatted}");
            AnsiConsole.Write(table);
            selectAllSessions.Clear();
        }
        public static void SelectAverageSessionLength(DateTime range)
        {
            var table = new Table();
            table.AddColumn("Average session time for the given period");
            double average = selectAllSessions.Where(x => x.StartDate >= range)
                                      .Average(x => x.Duration);
            string formatted = GetHoursMinutesFormat(Convert.ToInt32(average));
            table.AddRow($"{formatted}");
            AnsiConsole.Write(table);
            selectAllSessions.Clear();
        }
        public static void SelectGivenPeriodQuery(DateTime range)
        {
            IEnumerable<CodingSession> filtered = selectAllSessions.Where(x => x.StartDate >= range)
                                                           .OrderByDescending(x => x.Duration);
            var table = new Table();
            string[] columns = ["Id", "StartDate", "EndDate", "Duration"];
            table.AddColumns(columns);

            foreach (CodingSession x in filtered)
            {
                table.AddRow(x.Id.ToString(), x.StartDate.ToString("yyyy-MM-dd HH:mm"), x.EndDate.ToString("yyyy-MM-dd HH:mm"), x.Duration.ToString());
            }
            AnsiConsole.Write(table);
            selectAllSessions.Clear();     
        }
        private static string GetHoursMinutesFormat(int input)
        {
            int hours = input / 60;
            int minutes = input % 60;
            string formatted = $"{hours} hours, {minutes} minutes";
            return formatted;
        }
    }
}
