using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniza.Namedays
{
    public record struct DayMonth
    {
        public int Day { get; init; }
        public int Month { get; init; }

        public DayMonth()
            : this(1, 1)
        {
        }

        public DayMonth(int day, int month)
        {
            Day = day;
            Month = month;
        }

        public DateTime ToDateTime()
        {
            int currentYear = DateTime.Now.Year;
            return new DateTime(currentYear, Month, Day);
        }
    }
}
