using System.Globalization;

namespace CodingTrackerLibrary
{
    public class CodingGoals
    {
        public long GoalId { get; set; }
        public string GoalName {get;set;}
        public int DesiredHours {get;set;}
        public DateTime ReachGoalBy {get;set;}
        public CodingGoals(long goalId, string goalName, int desiredHours, string reachGoalBy)
        {
            GoalId = goalId;
            GoalName = goalName;
            DesiredHours = desiredHours;
            ReachGoalBy = DateTime.ParseExact(reachGoalBy, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
        }
    }
}
