using GeekBurguer.UI.Contract;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace GeekBurguer.Products.Service
{
    public interface IUIChangedService
    {
        void SendMessagesAsync();
        void AddToMessageList(IEnumerable<EntityEntry<Product>> changes);
    }
}