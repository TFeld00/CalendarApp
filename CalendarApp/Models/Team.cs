using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
    public class Team
    {
        [Key]
        public string Name { get; set; }

        public string Badge { get; set; }

        public Color Color { get; set; }
    }
}
