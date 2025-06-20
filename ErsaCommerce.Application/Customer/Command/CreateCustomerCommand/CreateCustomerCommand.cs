using ErsaCommerce.Data;
using ErsaCommerce.Domain;
using ErsaCommerce.Shared;
using MediatR;

namespace ErsaCommerce.Application
{
    public class CreateCustomerCommand : IRequest<Response<CustomerDto>>
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Response<CustomerDto>>
        {
            private readonly IErsaDbContext _context;

            public CreateCustomerCommandHandler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<Response<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                var customer = new Customer
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Address = request.Address,
                    CreatedAt = request.CreatedAt
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync(cancellationToken);

                var customerDto = new CustomerDto
                {
                    Id = customer.Id,
                    FullName = customer.FullName,
                    Email = customer.Email,
                    Address = customer.Address
                };

                return Response<CustomerDto>.Success(customerDto);
            }
        }
    }
}
