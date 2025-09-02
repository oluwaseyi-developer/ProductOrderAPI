using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Features.Users.Dtos;

namespace ProductOrderApi.Application.Features.Users.Queries
{
    public class LoginUserQuery : IRequest<Result<AuthResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginUserQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
