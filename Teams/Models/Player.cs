using System.ComponentModel.DataAnnotations;

namespace Teams.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1,99)]
        public ushort Age { get; set; }

        [Required]
        public string ImgUrl { get; set; }

        [Required]
        public string Nationality { get; set; }

        public Guid TeamId { get; set; }
        public virtual Team? Team { get; set; }
    }
}
