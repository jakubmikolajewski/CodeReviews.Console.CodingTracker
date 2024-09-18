using System.Globalization;

namespace CodingTrackerLibrary
{
    public class Validator
    {
        public static readonly List<string> mainMenuChoices = ["v", "u", "d", "i", "r", "m", "s", "g", "exit"];
        public static readonly List<string> reportsMenuChoices = ["t", "a", "menu"];
        public static readonly List<string> periodMenuChoices = ["days", "weeks", "months", "years", "all"];
        public static readonly List<string> columns = ["StartDate", "EndDate"];
        public static string ValidateMenuChoice(List<string> menuChoices)
        {
            string? menuChoice = Console.ReadLine();
            while (menuChoice is null || !menuChoices.Contains(menuChoice))
            {
                Console.WriteLine($"Please enter a valid choice from the menu ({String.Join(", ", menuChoices)}).\n");
                menuChoice = Console.ReadLine();
            }
            return menuChoice;
        }
        public static string ValidateDate()
        {
            if (UserInput.ChooseNowAsDateTime())
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
            string? date = Console.ReadLine();
            while (!DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime dateResult))
            {
                Console.WriteLine("Please enter a date in the correct format: (yyyy-MM-dd HH:mm)\n");
                date = Console.ReadLine();
            }
            return date;            
        }
        public static void ValidateColumns()
        {
            bool validInput = false;
            while (!validInput)
            {
                List<string> input = SplitAndTrimMultipleValues();
                foreach (string column in input)
                {
                    if (columns.Contains(column))
                        CodingController.valuesToUpdate.Add(column, "");
                }
                if (CodingController.valuesToUpdate.Keys.Count == 0)
                    Console.WriteLine($"Please enter at least one column name correctly. ({String.Join(", ", columns)})");
                else
                    validInput = true;
            }
        }
        public static int ValidateRowChoice()
        {
            int input;
            List<long> ids= CodingController.SelectIdsQuery();
            string? readResult = Console.ReadLine();
            while ((!int.TryParse(readResult, out input)) || (!ids.Contains(input)))
            {
                Console.WriteLine($"Please enter a valid Id:\n");
                readResult = Console.ReadLine();
            }
            return input;
        }
        public static List<string> SplitAndTrimMultipleValues()
        {
            List<string> inputList = [];

            string? readResult = Console.ReadLine();
            if (readResult is not null)
                inputList = readResult.Split(",").ToList();

            for (int i = 0; i < inputList.Count; i++)
            {
                inputList[i] = inputList[i].Trim();
            }
            return inputList;
        }
        public static int ValidateInt()
        {
            int result;
            string? input = Console.ReadLine();
            while (input is null || (!int.TryParse(input, out result)))
            {
                Console.WriteLine("Please provide a number.");
                input = Console.ReadLine();
            }
            return result;
        }
        public static string ValidateString()
        {
            string? input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Incorrect input. Try again.");
                input = Console.ReadLine();
            }
            return input;
        }
    }
}
