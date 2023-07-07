namespace DeTInYarneStaatVoorTiming.Models
{
    public class Appointment
    {
        public int ID { get; private set; }
        public string activityTitle { get; set; }
        public string location { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string submitterName { get; set; }
        public int importance { get; set; }
    }
}
