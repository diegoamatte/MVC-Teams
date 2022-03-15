using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teams.ViewModels.Player
{
    public class PlayerEditViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [Range(1, 99)]
        public ushort Age { get; set; }

        [Required]
        [Url]
        [DisplayName("Image URL")]
        public string? ImgUrl { get; set; }

        [Required]
        public string? Nationality { get; set; }

        [DisplayName("Team")]
        public Guid TeamId { get; set; }

        public SelectList? TeamList { get; set; }
    }
}
