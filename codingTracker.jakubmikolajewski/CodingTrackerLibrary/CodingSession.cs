using System.Globalization;

namespace CodingTrackerLibrary
{
    public class CodingSession
    {
        private long _id;
        private string _startDate;
        private string _endDate;
        private int _duration;

        public CodingSession(string startDate, string endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
            _duration = CalculateDuration(startDate, endDate);
        }
        public CodingSession(long id, string startDate, string endDate, int duration)
        {
            _id = id;
            _startDate = startDate;
            _endDate = endDate;
            _duration = duration;
        }
        public long Id
        {
            get => _id;
        }
        public DateTime StartDate
        {
            get => DateTime.ParseExact(_startDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
        }
        public DateTime EndDate
        {
            get => DateTime.ParseExact(_endDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
        }
        public int Duration
        {
            get => _duration;
        }
        private static int CalculateDuration(string startDate, string endDate)
        {
            DateTime start = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            DateTime end = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);

            TimeSpan dateDifference = (end - start);
            if (dateDifference <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(endDate, "End date cannot be earlier than start date.");
            }          
            return Convert.ToInt32(dateDifference.TotalMinutes);
        }
    }
}
