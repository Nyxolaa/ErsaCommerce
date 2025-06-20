using ErsaCommerce.Application;
using Microsoft.AspNetCore.Mvc;

namespace ErsaCommerce.Api
{
    public class CustomerController : BaseController
    {

        // musteri ekleme
        [HttpPost("create-customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand request)
        {
            var response = await Mediator.Send(request);
            if (!response.Succeeded)
                return BadRequest(response);
            return Ok(response);
        }

        // musteri listeleme
        [HttpGet("list-customer")]
        public async Task<IActionResult> ListCustomers([FromQuery] ListCustomerQuery request)
        {
            var result = await Mediator.Send(request);

            if (!result.Succeeded)
                return NotFound(result);

            return Ok(result);

        }
    }
}
