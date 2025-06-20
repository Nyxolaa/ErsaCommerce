using ErsaCommerce.Application;
using Microsoft.AspNetCore.Mvc;

namespace ErsaCommerce.Api
{
    public class OrderController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
        {
            var response = await Mediator.Send(command);
            if (!response.Succeeded)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var result = await Mediator.Send(new GetOrderByIdQuery { OrderId = id });

            if (!result.Succeeded)
                return NotFound(result);

            return Ok(result);
        }
    }
}
