using ErsaCommerce.Application;
using Microsoft.AspNetCore.Mvc;

namespace ErsaCommerce.Api
{
    public class OrderController : BaseController
    {

        // siparis ekleme
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var response = await Mediator.Send(command);
            if (!response.Succeeded)
                return BadRequest(response);
            return Ok(response);
        }

        // tum siparisleri listeleme
        [HttpGet("list-all-order")]
        public async Task<IActionResult> ListAllOrder([FromQuery] ListOrderQuery request)
        {
            var result = await Mediator.Send(request);

            if (!result.Succeeded)
                return NotFound(result);

            return Ok(result);
        }

        // belirli bir musterinin tum siparislerini listeleme
        [HttpGet("list-order-by-customers")]
        public async Task<IActionResult> ListOrderByCustomer ([FromQuery] GetOrdersByCustomerQuery request)
        {
            var result = await Mediator.Send(request);

            if (!result.Succeeded)
                return NotFound(result);

            return Ok(result);
        }

        // siparis icerigi listeleme
        [HttpGet("get-order-detail")]
        public async Task<IActionResult> GetOrder([FromQuery] GetOrderByIdQuery request)
        {
            var result = await Mediator.Send(request);

            if (!result.Succeeded)
                return NotFound(result);

            return Ok(result);
        }

        // siparis durumu guncelleme
        [HttpPut("update-order-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusCommand request)
        {
            var response = await Mediator.Send(request);
            if (!response.Succeeded)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
