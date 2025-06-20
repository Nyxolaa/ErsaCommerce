using ErsaCommerce.Data;
using ErsaCommerce.Domain.Enum;
using ErsaCommerce.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Application
{
    public class UpdateOrderStatusCommand : IRequest<Response<string>>
    {
        public long OrderId { get; set; }
        public int Status { get; set; }

        public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Response<string>>
        {
            private readonly IErsaDbContext _context;

            public UpdateOrderStatusCommandHandler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<Response<string>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(c => c.Id == request.OrderId , cancellationToken);

                // order in status unu kontrol et
                if (order.OrderStatus != OrderStatus.Pending) // eger status pending degilse islem yapma
                    return Response<string>.Failure(new[] { "Yalnızca `Pending` durumundaki siparişlerin durumu güncellenebilir." });

                // status u guncelle
                order.OrderStatus = (OrderStatus)request.Status;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                var responseStatus = (OrderStatus)request.Status;

                return Response<string>.Success( $"Sipariş durumu {responseStatus} olarak başarıyla güncellendi.");
            }
        }
    }
}
