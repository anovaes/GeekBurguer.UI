using System;

namespace GeekBurguer.UI.Contract
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
