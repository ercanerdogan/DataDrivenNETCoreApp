using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopAdmin.Models.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BethanyPieShopDbContext _context;

        public OrderRepository(BethanyPieShopDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderDetailAsync(int? orderId)
        {
            if (orderId == null)
            {
                return null;
            }

            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Pie)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            return order;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersWithDetailAsync()
        {
            var orderList = await _context.Orders
                .Include(o=>o.OrderDetails)
                .ThenInclude(od=>od.Pie)
                .OrderBy(o => o.OrderId)
                .ToListAsync();

            return orderList;
        }
    }
}
