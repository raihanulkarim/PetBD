using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetBD.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name="Status")]
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}