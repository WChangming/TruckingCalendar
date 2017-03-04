using System.ComponentModel.DataAnnotations;

namespace TruckingCalendar.Models
{
    public enum TruckStatus : byte
    {
        [Display(Name = "Planned")]
        orange,
        [Display(Name = "Confirmed")]
        green,
        [Display(Name = "Changed")]
        red,
        [Display(Name = "Loaded")]
        darkcyan
    }
}