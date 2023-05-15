using System.ComponentModel.DataAnnotations;

namespace RoadsideAssistanceBL.Models
{
    public class Vehicle
    {
        [Required]
        [MaxLength(10)]
        public string LicensePlate { get; set; }
        [MaxLength(50)]
        public string? VinNumber { get; set; }
        public int? Year { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
    }
}
