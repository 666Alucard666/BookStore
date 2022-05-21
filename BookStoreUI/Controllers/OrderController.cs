using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreUI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/order")]
    [Produces("application/json")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult> Create(OrderDTO order)
        {
            if (order == null)
            {
                return BadRequest("Wrong order data!");
            }
            if (order.Books.Count == 0)
            {
                return BadRequest("0 books in basket");
            }
            var o = await _orderService.MakeOrder(order);

            if (!o)
            {
                return BadRequest("Order cannot be created!");
            }

            return Ok();
        }

        [HttpDelete("DeleteOrder")]
        public async Task<ActionResult> Delete(DeleteOrderRequest order)
        {
            if (order == null)
            {
                return BadRequest("Wrong order data!");
            }
            var o = await _orderService.DeleteOrder(order);
            if (!o)
            {
                return BadRequest("Order cannot be deleted!");
            }

            return Ok();
        }
        [HttpGet("GetOrderByNumber")]
        public ActionResult<Order> GetOrderByNumber(int orderNumber)
        {
            if (orderNumber == 0)
            {
                return BadRequest("Wrong orderNumber");
            }
            var b = _orderService.GetOrderByNumber(orderNumber);
            if (b == null)
            {
                return BadRequest("None orders with this number");
            }
            return Ok(b);
        }

        [HttpGet("GetOrdersByUser")]
        public ActionResult<List<Order>> GetOrdersByUserId([FromQuery]int userId)
        {
            if (userId == 0)
            {
                return BadRequest("Wrong user");
            }

            var b = _orderService.GetAllOrdersByUser(userId).ToList();
            if (b == null || b.Count == 0)
            {
                return BadRequest("None orders for this user");
            }
            return Ok(b);
        }
    }
}
