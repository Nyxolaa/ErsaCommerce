using ErsaCommerce.Data;
using MediatR;
using ErsaCommerce.Domain.Enum;
using ErsaCommerce.Shared;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Application
{
    public class GetOrdersByCustomerQuery : IRequest<Response<List<OrderDto>>>
    {
        public int CustomerId { get; set; }

        public class Handler : IRequestHandler<GetOrdersByCustomerQuery, Response<List<OrderDto>>>
        {
            private readonly IErsaDbContext _context;

            public Handler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<OrderDto>>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
            {
                var customerExists = await _context.Customers
                    .AnyAsync(c => c.Id == request.CustomerId && c.DeletedAt == null, cancellationToken);

                if (!customerExists)
                    return Response<List<OrderDto>>.Failure(new[] { "Müşteri bulunamadı." });

                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.CustomerId == request.CustomerId)
                    .Select(o => new OrderDto
                    {
                        Id = o.Id,
                        Status = o.OrderStatus,
                        OrderDate = o.CreatedAt,
                        ItemCount = o.OrderItems.Sum(i => i.Quantity)
                    })
                    .ToListAsync(cancellationToken);

                return Response<List<OrderDto>>.Success(orders);
            }
        }
    }
}
