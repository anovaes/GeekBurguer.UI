using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurguer.UI.Contract
{
    public class Store
    {
        public Guid StoreId { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
