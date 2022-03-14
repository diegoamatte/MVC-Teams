using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace Teams.ViewModels
{
    public class PlayerCreateViewModel
    {
        public string? Name { get; set; }

        public ushort Age { get; set; }

        public string? ImgUrl { get; set; }

        public string? Nationality { get; set; }

        [DisplayName("Team")]
        public Guid TeamId { get; set; }

        public List<SelectListItem> TeamList { get; set; }
    }
}
