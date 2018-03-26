using System;

namespace GeekBurguer.UI.Controllers.Configuration
{
    internal class StoreCatalogApiConfiguration
    {
        public Guid StoreId { get; set; }
        public string StoreApi { get; set; }
        public string ProductsApi { get; set; }
    }
}