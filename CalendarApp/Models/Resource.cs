using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DotNetCoreSqlDb.Models
{
    public class Resource
    {
        private Dictionary<DateTime, Task> tasksByDate;

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Task> Tasks { get; set; } = new List<Task>();

        public string TeamName { get; set; }

        [NotMapped]
        public Team Team { get; set; }

        [NotMapped]
        public Dictionary<DateTime, Task> TasksByDate
        {
            get
            {
                if (tasksByDate == null)
                {
                    tasksByDate = new Dictionary<DateTime, Task>();
                    foreach (var t in Tasks)
                    {
                        if (!tasksByDate.ContainsKey(t.Date))
                            tasksByDate.Add(t.Date, t);
                    }
                }
                return tasksByDate;
            }
            set => tasksByDate = value;
        }
    }
}
