using ErsaCommerce.Data;
using MediatR;
using ErsaCommerce.Shared;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Application
{
    public class GetOrdersByCustomerQuery : IRequest<Response<List<GetOrderDto>>>
    {
        public long CustomerId { get; set; }

        public class Handler : IRequestHandler<GetOrdersByCustomerQuery, Response<List<GetOrderDto>>>
        {
            private readonly IErsaDbContext _context;

            public Handler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<GetOrderDto>>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
            {
                var customerExists = await _context.Customers
                    .AnyAsync(c => c.Id == request.CustomerId && c.DeletedAt == null, cancellationToken);

                if (!customerExists)
                    return Response<List<GetOrderDto>>.Failure(new[] { "Müşteri bulunamadı." });

                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.CustomerId == request.CustomerId)
                    .Select(o => new GetOrderDto
                    {
                        Id = o.Id,
                        Status = o.OrderStatus,
                        OrderDate = o.CreatedAt,
                        ItemCount = o.OrderItems.Sum(i => i.Quantity),
                        TotalAmount = o.TotalAmount
                    })
                    .ToListAsync(cancellationToken);

                return Response<List<GetOrderDto>>.Success(orders);
            }
        }
    }
}
