using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PetBD.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name="Status")]
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        [Display(Name = "Created on")]
        public DateTime CreatedDate { get; set; } 
        [Display(Name = "Modified on")]
        public DateTime ModifiedDate { get; set; }
        public string Description { get; set; }
        public bool IsFeatured { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        [Display(Name="Category")]
        public int CatID { get; set; }
        public Category Category { get; set; }
    }
}
