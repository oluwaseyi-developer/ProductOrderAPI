using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Features.Users.Dtos;

namespace ProductOrderApi.Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<Result<AuthResponse>>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public RegisterUserCommand(string email, string firstName, string lastName, string password)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }
    }
}
