using ErsaCommerce.Data;
using MediatR;
using ErsaCommerce.Shared;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Application
{
    public class GetOrderByIdQuery : IRequest<Response<GetOrderDetailDto>>
    {
        public int OrderId { get; set; }

        public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Response<GetOrderDetailDto>>
        {
            private readonly IErsaDbContext _context;

            public GetOrderByIdQueryHandler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<Response<GetOrderDetailDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
            {
                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.DeletedAt == null, cancellationToken);

                if (order == null)
                    return Response<GetOrderDetailDto>.Failure(new[] { "Sipariş bulunamadı." });

                var dto = new GetOrderDetailDto
                {
                    Id = order.Id,
                    Status = order.OrderStatus,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    ItemCount = order.OrderItems.Count,

                    CustomerId = order.CustomerId,
                    CustomerName = order.Customer.FullName,
                    CustomerEmail = order.Customer.Email,
                    
                    Items = order.OrderItems.Select(i => new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                    }).ToList()
                };

                return Response<GetOrderDetailDto>.Success(dto);
            }
        }
    }
}
