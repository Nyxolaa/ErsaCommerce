using MediatR;
using ErsaCommerce.Shared;
using ErsaCommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Application
{
    public class ListOrderQuery : IRequest<Response<List<OrderDto>>>
    {

        public class ListOrderQueryHandler : IRequestHandler<ListOrderQuery, Response<List<OrderDto>>>
        {
            private readonly IErsaDbContext _context;
            public ListOrderQueryHandler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<OrderDto>>> Handle(ListOrderQuery request, CancellationToken cancellationToken)
            {
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .Select(o => new OrderDto
                    {
                        Id = o.Id,
                        OrderDate = o.OrderDate,
                        ItemCount = o.OrderItems.Count,
                        Status = o.OrderStatus,
                        
                        CustomerId = o.CustomerId,
                        CustomerName = o.Customer.FullName,
                        CustomerEmail = o.Customer.Email,
                    })
                    .ToListAsync(cancellationToken);

                return Response<List<OrderDto>>.Success(orders);
            }
        }
    }
}