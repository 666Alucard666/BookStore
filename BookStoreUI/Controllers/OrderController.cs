using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using DAL.Models;
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
        public async Task<ActionResult> Create([FromBody]OrderDTO order)
        {
            if (order == null)
            {
                return BadRequest("Wrong order data!");
            }
            if (order.ProductsList.Count == 0)
            {
                return BadRequest("0 books in basket");
            }
            var o = await _orderService.MakeOrder(order);
            if (!o)
            {
                return BadRequest("Order cannot be created!");
            }

            return Ok("Order was created!");
        }

        [HttpPut("DeleteOrder")]
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
        public async Task<ActionResult<Order>> GetOrderByNumber(Guid orderNumber)
        {
            var h = Request.Headers;
            if (orderNumber == Guid.Empty)
            {
                return BadRequest("Wrong orderNumber");
            }
            var b = await _orderService.GetOrderByNumber(orderNumber);
            if (b == null)
            {
                return BadRequest("None orders with this number");
            }
            return Ok(b);
        }

        [HttpGet("GetOrdersByUser")]
        public async Task<ActionResult<List<Order>>> GetOrdersByUserId([FromQuery]Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("Wrong user");
            }

            var b = await _orderService.GetAllOrdersByCustomer(userId);
            if (b == null || !b.Any())
            {
                return BadRequest("None orders for this user");
            }
            return Ok(b);
        }
        
        [HttpGet("GetOrdersByShop")]
        public async Task<ActionResult<List<Order>>> GetOrdersByShop([FromQuery]Guid shopId)
        {
            if (shopId == Guid.Empty)
            {
                return BadRequest("Wrong shop id");
            }

            var b = await _orderService.GetAllOrdersByShop(shopId);
            if (b == null || !b.Any())
            {
                return BadRequest("None orders for this shop");
            }
            return Ok(b);
        }
        
        [HttpGet("GetOrdersReceipt")]
        public async Task<ActionResult<bool>> GetOrdersReceipt([FromQuery]Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Wrong id!");
            }
            var b = await _orderService.CreateAndSendReceipt(id);
            if (b == null)
            {
                return BadRequest("None orders with this number");
            }
            return Ok(true);
        }

        [HttpGet("GetPopularRecipientCities")]
        public async Task<ActionResult<IEnumerable<OrderCities>>> GetPopularRecipientCities()
        {
            var res = await _orderService.GetPopularRecipientCities();
            if (!res.Any())
            {
                return BadRequest("Error");
            }

            return Ok(res);
        }

    }
}
