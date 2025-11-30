using System.ComponentModel.DataAnnotations;

namespace TouristAgency.Entities
{
    public class Destination
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(100)]
        public string Country { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
        public string? ImageUrl { get; set; }  


        public DateTime CreatedOn { get; set; }

        public bool IsActive { get; set; }
    }
}
