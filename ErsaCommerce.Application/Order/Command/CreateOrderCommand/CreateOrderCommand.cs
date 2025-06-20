using ErsaCommerce.Data;
using ErsaCommerce.Domain;
using ErsaCommerce.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Application
{
    public class CreateOrderCommand : IRequest<Response<CreateOrderDto>>
    {
        public long CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();


        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreateOrderDto>>
        {
            private readonly IErsaDbContext _context;

            public CreateOrderCommandHandler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<Response<CreateOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Id == request.CustomerId && c.DeletedAt == null, cancellationToken);

                // `Yeni sipariş oluşturulurken, müşteri veritabanında kayıtlı değilse hata döndürmeli.`
                if (customer == null)
                    return Response<CreateOrderDto>.Failure(new[] { "Müşteri bulunamadı." });

                // product eklenmediyse ?
                if (request.Items == null || !request.Items.Any())
                    return Response<CreateOrderDto>.Failure(new[] { "Lütfen en az bir ürün ekleyin." });

                // product larin price verisi ile total amount hesapla
                var productIds = request.Items.Select(x => x.ProductId).ToList();

                var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync(cancellationToken);

                if (products.Count != productIds.Count)
                    return Response<CreateOrderDto>.Failure(new[] { "Bazı ürünler bulunamadı veya silinmiş." });

                // Total amount hesapla
                var totalAmount = request.Items.Sum(item =>
                {
                    var product = products.First(p => p.Id == item.ProductId);
                    return product.Price * item.Quantity;
                });

                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    CreatedAt = DateTime.UtcNow,
                    OrderItems = request.Items.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity
                    }).ToList(),
                    OrderDate = DateTime.UtcNow,
                    OrderStatus = Domain.Enum.OrderStatus.Pending,
                    TotalAmount = totalAmount
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                var orderDto = new CreateOrderDto
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

                return Response<CreateOrderDto>.Success(orderDto, "Sipariş başarıyla oluşturuldu.");
            }
        }

    }
}
