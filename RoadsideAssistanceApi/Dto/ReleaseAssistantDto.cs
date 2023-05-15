
using RoadsideAssistanceBL.Models;
using System.ComponentModel.DataAnnotations;

namespace RoadsideAssistanceApi.Dto
{
    public class ReleaseAssistantDto
    {
        [Required]
        public Customer Customer { get; set; }
        [Required]
        public Assistant Assistant { get; set; }

    }
}
