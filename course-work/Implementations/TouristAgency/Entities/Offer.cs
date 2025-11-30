using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TouristAgency.Entities
{
    public class Offer
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Title { get; set; }

        [Required]
        public int DestinationId { get; set; }   // FK към Destination
        
        [ValidateNever]
        public Destination Destination { get; set; }

        public decimal Price { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; }

        public DateTime StartDate { get; set; }
    }
}
