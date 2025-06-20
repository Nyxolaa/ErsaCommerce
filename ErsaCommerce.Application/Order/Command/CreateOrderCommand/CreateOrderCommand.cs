using ErsaCommerce.Data;
using ErsaCommerce.Domain;
using ErsaCommerce.Domain.Enum;
using ErsaCommerce.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Application
{
    public class CreateOrderCommand : IRequest<Response<OrderDto>>
    {
        public long CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerEmail { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;
        public string BillingAddress { get; set; } = null!;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public string PaymentMethod { get; set; } = "CreditCard"; // varsayılan: CreditCard
        public string ShippingMethod { get; set; } = "Standard"; // varsayılan: Standard
        public string Notes { get; set; } // isteğe bağlı notlar

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

                if (customer == null)
                    return Response<OrderDto>.Failure(new[] { "Müşteri bulunamadı." });

                if (request.Items == null || !request.Items.Any())
                    return Response<OrderDto>.Failure(new[] { "Sipariş için en az bir ürün gereklidir." });

                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    CreatedAt = DateTime.UtcNow,
                    OrderStatus = OrderStatus.Pending,
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
                    // bunlar detay
                    TotalAmount = request.TotalAmount,
                    ShippingAddress = request.ShippingAddress,
                    BillingAddress = request.BillingAddress,                   
                    PaymentMethod = request.PaymentMethod,
                    ShippingMethod = request.ShippingMethod,
                    Notes = request.Notes
                };

                return Response<OrderDto>.Success(orderDto, "Sipariş başarıyla oluşturuldu.");
            }
        }

    }
}
