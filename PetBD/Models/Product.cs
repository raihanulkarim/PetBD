using System;
using System.Collections.Generic;

namespace PetBD.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedDate { get; set; } 
        public DateTime ModifiedDate { get; set; }
        public string Description { get; set; }
        public bool IsFeatured { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public int CatID { get; set; }
        public Category Category { get; set; }
    }
}
