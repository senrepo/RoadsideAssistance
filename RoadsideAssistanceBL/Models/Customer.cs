using System.ComponentModel.DataAnnotations;

namespace RoadsideAssistanceBL.Models
{
    public class Customer
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public ServiceRequest? serviceRequest { get; set; }
    }
}
