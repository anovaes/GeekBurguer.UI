using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurguer.UI.Contract
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string StoreName { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public List<Item> Items { get; set; }
    }
}
