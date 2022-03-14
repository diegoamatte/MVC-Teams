using System.ComponentModel.DataAnnotations;

namespace Teams.Models
{
    public class Team : Entity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string League { get; set; }

        [Required]
        [Url]
        public string? TeamLogoUrl { get; set; }

    }
}
