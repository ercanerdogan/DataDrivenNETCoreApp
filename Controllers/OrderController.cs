using BethanysPieShopAdmin.Models;
using BethanysPieShopAdmin.Models.Repositories;
using BethanysPieShopAdmin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopAdmin.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index(int? orderId, int? orderDetailId)
        {
            var orderIndexViewModel = new OrderIndexViewModel
            {
                Orders = await _orderRepository.GetAllOrdersWithDetailAsync()
            };

            if (orderId != null)
            {
                var selectedOrder = orderIndexViewModel.Orders.SingleOrDefault(o => o.OrderId == orderId);
                if (selectedOrder != null)
                {
                    orderIndexViewModel.OrderDetails = selectedOrder.OrderDetails;
                    orderIndexViewModel.SelectedOrderId = orderId.Value; 
                }
            }

            if (orderDetailId != null)
            {
                var selectedOrderDetail =
                    orderIndexViewModel.OrderDetails.SingleOrDefault(od => od.OrderDetailId == orderDetailId);
                if (selectedOrderDetail != null)
                {
                    orderIndexViewModel.SelectedOrderDetailId = orderDetailId.Value;
                    orderIndexViewModel.Pies = new List<Pie>() { selectedOrderDetail.Pie };
                }

            }

            return View(orderIndexViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.GetOrderDetailAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
