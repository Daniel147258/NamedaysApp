using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniza.Namedays
{
    public record struct Nameday
    {
        public DayMonth DayMonth { get; init; }

        public string date
        {
            get
            {
                return DayMonth.ToDateTime().ToString("d.M.");
            }
        } 
        public string Name { get; init; }


        public Nameday(string name, DayMonth dayMonth)
        {
            Name = name;
            DayMonth = dayMonth;
        }

        public Nameday() : this("", new DayMonth())
        {
        }
    }
}
