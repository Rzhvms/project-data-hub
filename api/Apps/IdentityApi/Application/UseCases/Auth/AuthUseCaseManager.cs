using System.Security.Claims;
using Application.Ports.Repositories;
using Application.UseCases.Auth.Dto.Request;
using Application.UseCases.Auth.Dto.Response.Base;
using Application.UseCases.Auth.Dto.Response.ConnectToken;
using Application.UseCases.Auth.Dto.Response.CreateUser;
using Application.UseCases.Auth.Dto.Response.RevocateToken;
using Application.UseCases.Auth.interfaces;
using Domain.Entities.IdentityUser;
using IdentityLib.Encryption.Interfaces;
using IdentityLib.ErrorCodes;
using IdentityLib.Jwt.Entities;
using IdentityLib.Jwt.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Auth;

/// <inheritdoc />
public class AuthUseCaseManager(
    IPasswordEncryptionService encryptionService,
    ILogger<AuthUseCaseManager> logger,
    IAuthRepository authRepository,
    IRoleRepository roleRepository,
    IJwtGenerationService jwtGenerationService)
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

        var salt = encryptionService.GenerateSalt();
        var role = await roleRepository.GetRoleByRoleCode(request.RoleName);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            Phone = request.Phone,
            Password = encryptionService.HashPassword(request.Password, salt),
            HashSalt = Convert.ToBase64String(salt),
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
    
    /// <inheritdoc />
    public async Task<ConnectTokenResponse> ConnectTokenAsync(ConnectTokenRequest request)
    {
        var user = await authRepository.GetUserByEmailOrUsernameAsync(request.Login);

        if (user == null)
        {
            return CreateErrorResponse<ConnectTokenErrorResponse>(ErrorCodes.UserDoesNotExist);
        }
        
        if (!encryptionService.VerifyPassword(request.Password, user.Password))
        {
            return CreateErrorResponse<ConnectTokenErrorResponse>(ErrorCodes.CredentialsAreNotValid);
        }
        
        user.RefreshToken = CreateRefreshTokenAsync();

        await authRepository.UpdateUserAsync(user);

        var role = await roleRepository.GetRoleByIdAsync(user.RoleId);
        
        // Добавляем информацию о роли и правах в клеймы
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, role.RoleCode),
            new("permissions", ((int)role.PermissionsMask).ToString())
        };

        var jwtUserData = new JwtUserData
        {
            UserId = user.Id,
            UserName = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Claims = claims
        };
        
        var accessToken = jwtGenerationService.GenerateAccessToken(jwtUserData);
        
        return new ConnectTokenSuccessResponse
        {
            AccessToken = accessToken,
            RefreshToken = user.RefreshToken.Value
        };
    }

    /// <inheritdoc />
    public async Task<RevocateTokenResponse> RevocateRefreshTokenAsync(Guid userId)
    {
        var user = await authRepository.GetUserByUserIdAsync(userId);
        if (user == null) return CreateErrorResponse<RevocateTokenErrorResponse>(ErrorCodes.UserDoesNotExist);

        if (user.RefreshToken != null)
        {
            user.RefreshToken.Active = false;
            await authRepository.UpdateUserAsync(user);
        }

        logger.LogInformation("Пользователь {UserId} успешно вышел из системы.", user.Id);
        
        return new RevocateTokenSuccessResponse()
        {
            Message = $"Пользователь вышел из системы в {DateTime.UtcNow:O} (UTC)."
        };
    }
    
    /// <summary>
    /// Создает новый refresh token с параметрами из jwtGenerationService.
    /// </summary>
    private RefreshToken CreateRefreshTokenAsync()
    {
        var lifetimeMinutes = jwtGenerationService.GetRefreshTokenLifetimeInMinutes();
        return new RefreshToken
        {
            Value = jwtGenerationService.GenerateRefreshToken(),
            Active = true,
            ExpirationDate = DateTime.UtcNow.AddMinutes(lifetimeMinutes)
        };
    }

    /// <summary>
    /// Универсальный метод для формирования ошибки с кодом и описанием.
    /// </summary>
    private static TResponse CreateErrorResponse<TResponse>(ErrorCodes errorCode) where TResponse : IBaseErrorResponse, new()
    {
        return new TResponse
        {
            Message = Enum.GetName(typeof(ErrorCodes), errorCode),
            Code = ((int)errorCode).ToString()
        };
    }
}