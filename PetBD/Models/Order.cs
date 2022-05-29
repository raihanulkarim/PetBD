using System;
using System.Collections.Generic;

namespace PetBD.Models
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetails>();
        }
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
        public Boolean? Status { get; set; }
        public virtual List<OrderDetails> OrderDetails { get; set; }
    }
}
