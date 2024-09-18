using CodingTrackerLibrary;

public static class Program
{
    private static bool exit;
    private static DatabaseActions current = new DatabaseActions();
    public static void Main(string[] args)
    {
        while (!exit)
        {
            current.CreateDatabaseAndTableIfNotExists();
            TableVisualizationEngine.ShowMenu();
            exit = UserInput.SwitchMenuChoice(Validator.ValidateMenuChoice(Validator.mainMenuChoices));
        }
    }
}