using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Application.Common.Interfaces.Services;
using ProductOrderApi.Application.Features.Users.Dtos;
using ProductOrderApi.Domain.Entities;

namespace ProductOrderApi.Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<AuthResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<Result<AuthResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user already exists
                if (await _unitOfWork.UserRepository.UserExistsAsync(request.Email))
                    return Result<AuthResponse>.Failure("User with this email already exists");

                // Hash password
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // Create user
                var user = new User(request.Email, request.FirstName, request.LastName, passwordHash);
                user.AddRole("Customer"); // Default role

                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Generate token
                var token = _jwtService.GenerateToken(user);

                return Result<AuthResponse>.Success(new AuthResponse
                {
                    Token = token,
                    Expiration = DateTime.UtcNow.AddHours(1),
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Roles = user.Roles.ToList()
                    }
                });
            }
            catch (Exception ex)
            {
                return Result<AuthResponse>.Failure($"Failed to register user: {ex.Message}");
            }
        }
    }

   
}
