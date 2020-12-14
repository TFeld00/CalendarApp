using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
    public class Holiday
    {
        [Key]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
