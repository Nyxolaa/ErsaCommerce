using MediatR;
using ErsaCommerce.Data;
using Microsoft.EntityFrameworkCore;
using ErsaCommerce.Infrastructure.Jwt;

namespace ErsaCommerce.Application.Auth.Command.LoginCommand
{
    public class LoginCommand : IRequest<string>
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public class Handler : IRequestHandler<LoginCommand, string>
        {
            private readonly IErsaDbContext _context;
            private readonly ITokenService _tokenService;

            public Handler(IErsaDbContext context, ITokenService tokenService)
            {
                _context = context;
                _tokenService = tokenService;
            }

            public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

                if (user == null || user.Password != request.Password)
                    throw new UnauthorizedAccessException("Invalid credentials");

                var token = _tokenService.CreateToken(user);

                return token;
            }
        }
    }
}
