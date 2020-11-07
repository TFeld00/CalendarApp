using System;
using System.Collections.Generic;

namespace DotNetCoreSqlDb.Models
{
    public class CalendarViewModel
    {
        public List<Resource> Resources { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Team { get; set; }
        public bool NoTeam { get; internal set; }
        public bool ShowAll { get; internal set; }

        public List<MonthViewModel> GetMonths()
        {
            var list = new List<MonthViewModel>();
            var current = new MonthViewModel
            {
                Month = StartDate,
                Days = 0
            };
            for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                if (date.Month == current.Month.Month) { current.Days++; }
                else
                {
                    list.Add(current);
                    current = new MonthViewModel
                    {
                        Month = date,
                        Days = 1
                    };
                }
            }
            list.Add(current);
            return list;
        }
    }

    public class MonthViewModel
    {
        public DateTime Month { get; set; }
        public int Days { get; set; }
    }
}
