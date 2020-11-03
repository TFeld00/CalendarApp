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

        public string Team { get; set; }

        [NotMapped]
        public Dictionary<DateTime, Task> TasksByDate
        {
            get
            {
                if (tasksByDate == null)
                {
                    tasksByDate = Tasks.ToDictionary(t => t.Date, t => t);
                }
                return tasksByDate;
            }
            set => tasksByDate = value;
        }
    }
}
