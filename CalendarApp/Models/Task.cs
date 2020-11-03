using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCoreSqlDb.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title/Location")]
        public string Title { get; set; }
        public string Description { get; set; }

        [Display(Name = "Color")]
        public TaskColor TaskColor { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int ResourceId { get; set; }
        public Resource Resource { get; set; }

        public string GetTooltip()
        {
            var tooltip = Title;
            if (!string.IsNullOrWhiteSpace(Description))
            {
                tooltip += "\n\n" + Description;
            }
            return tooltip;
        }

        public string GetHtmlTooltip()
        {
            return GetTooltip().Replace("\n", "<br>");
        }
    }
}
