using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetBD.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Display(Name="Sub-Title")]
        public string SubTitle { get; set; }
        [Display(Name="Status")]
        public bool IsActive { get; set; }
        public string Image { get; set; }
        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile UploadImage { get; set; }
    }
}
