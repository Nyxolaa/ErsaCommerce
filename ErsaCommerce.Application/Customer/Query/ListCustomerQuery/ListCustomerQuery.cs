using ErsaCommerce.Data;
using ErsaCommerce.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Application
{
    public class ListCustomerQuery : IRequest<Response<List<CustomerDto>>>
    {
        public class ListCustomerQueryHandler : IRequestHandler<ListCustomerQuery, Response<List<CustomerDto>>>
        {
            private readonly IErsaDbContext _context;
            public ListCustomerQueryHandler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<CustomerDto>>> Handle(ListCustomerQuery request, CancellationToken cancellationToken)
            {
                var customers = await _context.Customers
                    .Where(c => c.DeletedAt == null)
                    .Select(c => new CustomerDto
                    {
                        Id = c.Id,
                        FullName = c.FullName,
                        Email = c.Email
                    })
                    .ToListAsync(cancellationToken);

                return Response<List<CustomerDto>>.Success(customers);
            }
        }
    }
}
