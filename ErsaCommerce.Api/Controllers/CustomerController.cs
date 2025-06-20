using ErsaCommerce.Application;
using Microsoft.AspNetCore.Mvc;

namespace ErsaCommerce.Api
{
    public class CustomerController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerCommand command)
        {
            var response = await Mediator.Send(command);
            if (!response.Succeeded)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> ListCustomers(ListCustomerQuery request)
        {
            var result = await Mediator.Send(request);

            if (!result.Succeeded)
                return NotFound(result);

            return Ok(result);

        }
    }
}
