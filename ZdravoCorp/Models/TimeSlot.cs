using System;

namespace ZdravoCorp.Models
{
    public class TimeSlot
    {
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
        

        public TimeSlot() {}

        public TimeSlot(DateTime dateTime, int duration)
        {
            DateTime = dateTime;
            Duration = duration;
        }
        public bool IsOverlappingWith(TimeSlot timeSlot)
        {
            
            if(timeSlot.DateTime <= DateTime && timeSlot.DateTime.AddMinutes(timeSlot.Duration) > DateTime)
            {
                return true;
            }
            if(timeSlot.DateTime >= DateTime && DateTime.AddMinutes(Duration) > timeSlot.DateTime)
            {
                return true;
            }
            return false;
        }
    }
}
