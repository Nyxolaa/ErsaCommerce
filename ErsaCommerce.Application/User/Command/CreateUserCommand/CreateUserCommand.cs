using ErsaCommerce.Data;
using ErsaCommerce.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ErsaCommerce.Application
{
    public class CreateUserCommand : IRequest<long> // kullanici id'si donecek
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User"; // varsayılan: User

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, long>
        {
            private readonly IErsaDbContext _context;

            public CreateUserCommandHandler(IErsaDbContext context)
            {
                _context = context;
            }

            public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                // Kullanıcı var mı kontrolü
                var existingUser = await _context.Users
                    .AnyAsync(u => u.Username == request.Username, cancellationToken);
                if (existingUser)
                    throw new Exception("Bu kullanıcı adı zaten mevcut.");

                string passwordHash = Convert.ToBase64String(
                    SHA256.HashData(Encoding.UTF8.GetBytes(request.Password)));

                var user = new User
                {
                    Username = request.Username,
                    Password = passwordHash,
                    Role = request.Role
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);

                return user.Id;
            }
        }
    }
}
