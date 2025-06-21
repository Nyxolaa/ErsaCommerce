using ErsaCommerce.Data;
using ErsaCommerce.Domain.Enum;
using ErsaCommerce.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ErsaCommerce.Application
{
    public class GetOrdersByCustomerSP : IRequest<Response<List<GetOrderDto>>>
    {
        public long CustomerId { get; set; }

        public class Handler : IRequestHandler<GetOrdersByCustomerSP, Response<List<GetOrderDto>>>
        {
            private readonly IErsaDbContext _context;

            public Handler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<Response<List<GetOrderDto>>> Handle(GetOrdersByCustomerSP request, CancellationToken cancellationToken)
            {
                var result = new List<GetOrderDto>();
                var dbContext = _context as DbContext;

                using (var command = dbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "sp_GetOrdersByCustomerId";
                    command.CommandType = CommandType.StoredProcedure;

                    var param = command.CreateParameter();
                    param.ParameterName = "@CustomerId";
                    param.Value = request.CustomerId;
                    command.Parameters.Add(param);

                    await dbContext.Database.OpenConnectionAsync(cancellationToken);

                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Add(new GetOrderDto
                            {
                                Id = reader.GetInt64(0),
                                Status = (OrderStatus)reader.GetInt32(1),
                                OrderDate = reader.GetDateTime(2),
                                ItemCount = reader.GetInt32(3),
                                TotalAmount = reader.GetDecimal(4)
                            });
                        }
                    }
                }

                return Response<List<GetOrderDto>>.Success(result);
            }
        }
    }
}

//
//CREATE PROCEDURE sp_GetOrdersByCustomerId
//    @CustomerId INT
//AS
//BEGIN
//    SELECT 
//        o.Id,
//        o.OrderStatus AS Status,
//        o.CreatedAt AS OrderDate,
//        SUM(oi.Quantity) AS ItemCount,
//        o.TotalAmount
//    FROM Orders o
//    INNER JOIN OrderItems oi ON o.Id = oi.OrderId
//    WHERE o.CustomerId = @CustomerId
//    GROUP BY o.Id, o.OrderStatus, o.CreatedAt, o.TotalAmount
//END
