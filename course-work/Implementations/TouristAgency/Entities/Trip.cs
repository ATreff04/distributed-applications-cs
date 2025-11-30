using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TouristAgency.Entities
{
    public class Trip
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; }

        public DateTime DepartureDate { get; set; }

        [Required]
        public int DestinationId { get; set; }   
        
        [ValidateNever]
        public Destination Destination { get; set; }

        public decimal Price { get; set; }

        public int Seats { get; set; }
    }
}
