using ErsaCommerce.Data;
using MediatR;
using ErsaCommerce.Shared;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Application
{
    public class GetOrderByIdQuery : IRequest<Response<GetOrderDto>>
    {
        public int OrderId { get; set; }

        public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Response<GetOrderDto>>
        {
            private readonly IErsaDbContext _context;

            public GetOrderByIdQueryHandler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<Response<GetOrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.DeletedAt == null, cancellationToken);

                if (order == null)
                    return Response<GetOrderDto>.Failure(new[] { "Sipariş bulunamadı." });

                var dto = new GetOrderDto
                {
                    Id = order.Id,
                    Status = order.OrderStatus,
                    CreatedAt = order.CreatedAt,
                    CustomerId = order.CustomerId,
                    OrderItems = order.OrderItems.Select(i => new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity
                    }).ToList()
                };

                return Response<GetOrderDto>.Success(dto);
            }
        }
    }
}
