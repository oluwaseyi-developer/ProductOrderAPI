using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Application.Common.Interfaces.Services;
using ProductOrderApi.Application.Features.Users.Dtos;

namespace ProductOrderApi.Application.Features.Users.Queries
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result<AuthResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public LoginUserQueryHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<Result<AuthResponse>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                    return Result<AuthResponse>.Failure("Invalid email or password");

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
                return Result<AuthResponse>.Failure($"Failed to login: {ex.Message}");
            }
        }
    }
}
