using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
    public enum TaskColor
    {
        [Display(Name = "Primary", Order = 2)]
        Primary,
        [Display(Name = "Secondary", Order = 4)]
        Secondary,
        [Display(Name = "Color", Order = 1)]
        Success,
        [Display(Name = "Color", Order = 5)]
        Danger,
        [Display(Name = "Color", Order = 6)]
        Warning,
        [Display(Name = "Color", Order = 3)]
        Info,
        [Display(Name = "Color", Order = 7)]
        Light,
        [Display(Name = "Color", Order = 8)]
        Dark
    }
}
