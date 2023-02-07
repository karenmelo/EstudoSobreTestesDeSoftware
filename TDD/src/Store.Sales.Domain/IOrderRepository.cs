using Store.Core.Data;

namespace Store.Sales.Domain
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Add(Order order);
        void Update(Order order);
        Task<Order> GetOrderDraftByClientId(Guid clientId);
    }
}
