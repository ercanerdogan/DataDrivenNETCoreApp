namespace BethanysPieShopAdmin.Models.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderDetailAsync(int? orderId);
        Task<IEnumerable<Order>> GetAllOrdersWithDetailAsync();
    }
}
