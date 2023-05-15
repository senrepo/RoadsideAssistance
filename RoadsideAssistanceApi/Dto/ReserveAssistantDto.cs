
using RoadsideAssistanceBL.Models;
using System.ComponentModel.DataAnnotations;

namespace RoadsideAssistanceApi.Dto
{
    public class ReserveAssistantDto
    {
        [Required]
        public Customer Customer { get; set; }
        [Required]
        public Geolocation Geolocation { get; set; }
    }
}
