using ErsaCommerce.Data;
using ErsaCommerce.Domain;
using ErsaCommerce.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Application
{
    public class CreateOrderCommand : IRequest<Response<OrderDto>>
    {
        public long CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();


        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<OrderDto>>
        {
            private readonly IErsaDbContext _context;

            public CreateOrderCommandHandler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<Response<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == request.CustomerId && c.DeletedAt == null, cancellationToken);

                // `Yeni sipariş oluşturulurken, müşteri veritabanında kayıtlı değilse hata döndürmeli.`
                if (customer == null)
                    return Response<OrderDto>.Failure(new[] { "Müşteri bulunamadı." });

                // product eklenmediyse ?
                if (request.Items == null || !request.Items.Any())
                    return Response<OrderDto>.Failure(new[] { "Lütfen en az bir ürün ekleyin." });

                // product larin price verisi ile total amount hesapla (STORED PROCEDURE)
                var totalAmount = 0;
                foreach (var item in request.Items)
                {
                   
                }

                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    CreatedAt = DateTime.UtcNow,
                    OrderItems = request.Items.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity
                    }).ToList()
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync(cancellationToken);

                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    CustomerName = customer.FullName,
                    CustomerEmail = customer.Email,
                    Status = order.OrderStatus,
                    Items = request.Items,
                    OrderDate = order.CreatedAt,
                    TotalAmount = order.TotalAmount
                };

                return Response<OrderDto>.Success(orderDto, "Sipariş başarıyla oluşturuldu.");
            }
        }

    }
}
