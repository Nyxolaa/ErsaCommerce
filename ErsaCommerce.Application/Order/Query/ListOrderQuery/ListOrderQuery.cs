using MediatR;
using ErsaCommerce.Shared;
using ErsaCommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Application
{
    public class ListOrderQuery : IRequest<Response<List<GetOrderDto>>>
    {

        public class ListOrderQueryHandler : IRequestHandler<ListOrderQuery, Response<List<GetOrderDto>>>
        {
            private readonly IErsaDbContext _context;
            public ListOrderQueryHandler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<GetOrderDto>>> Handle(ListOrderQuery request, CancellationToken cancellationToken)
            {
                var orders = await _context.Orders
                    .Select(o => new GetOrderDto
                    {
                        Id = o.Id,
                        OrderDate = o.OrderDate,
                        ItemCount = o.OrderItems.Count,
                        Status = o.OrderStatus,
                        TotalAmount = o.TotalAmount,
                        CustomerId = o.CustomerId,
                        CustomerName = o.Customer.FullName,
                        CustomerEmail = o.Customer.Email
                    })
                    .ToListAsync(cancellationToken);

                return Response<List<GetOrderDto>>.Success(orders);
            }
        }
    }
}