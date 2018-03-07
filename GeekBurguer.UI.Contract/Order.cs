using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurguer.UI.Contract
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
