﻿using System.Collections.Generic;

namespace PetBD.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}