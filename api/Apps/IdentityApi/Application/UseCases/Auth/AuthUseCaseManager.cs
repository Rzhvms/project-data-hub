using Application.Ports.Repositories;
using Application.UseCases.Auth.Dto.Request;
using Application.UseCases.Auth.Dto.Response;
using Application.UseCases.Auth.interfaces;
using Domain.Entities.IdentityUser;
using IdentityLib.Encryption.Interfaces;
using IdentityLib.ErrorCodes;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Auth;

/// <inheritdoc />
public class AuthUseCaseManager(
    IPasswordEncryptionService encryptionService,
    ILogger<AuthUseCaseManager> logger,
    IAuthRepository authRepository,
    IRoleRepository roleRepository)
    : IAuthUseCaseManager
{
    /// <inheritdoc />
    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        // Проверяем, существует ли пользователь
        var existingUser = await authRepository.GetUserByEmailOrUsernameAsync(request.Email, request.Username);
        if (existingUser != null)
        {
            return new CreateUserErrorResponse
            {
                Message = "Пользователь с таким логином / почтой уже существует.",
                Code = ErrorCodes.UserDoesNotExist.ToString("D")
            };
        }

        var role = await roleRepository.GetRoleByRoleCode(request.RoleName);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            Phone = request.Phone,
            Password = encryptionService.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            RoleId = role.Id
        };

        await authRepository.CreateUserAsync(user);
        
        logger.LogInformation("Пользователь {UserName} {Email} успешно создан", user.Username, user.Email);

        return new CreateUserSuccessResponse
        {
            UserId = user.Id
        };
    }
}