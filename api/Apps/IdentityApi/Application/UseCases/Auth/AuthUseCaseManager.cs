using System.Security.Claims;
using Application.Ports.Repositories;
using Application.UseCases.Auth.Dto.Request;
using Application.UseCases.Auth.Dto.Response.Base;
using Application.UseCases.Auth.Dto.Response.ConnectToken;
using Application.UseCases.Auth.Dto.Response.CreateUser;
using Application.UseCases.Auth.Dto.Response.RefreshToken;
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
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IJwtGenerationService jwtGenerationService)
    : IAuthUseCaseManager
{
    /// <inheritdoc />
    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        // Проверяем, существует ли пользователь
        var existingUser = await userRepository.GetUserByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new CreateUserErrorResponse
            {
                Message = "Пользователь с такой почтой уже существует.",
                Code = ErrorCodes.UserAlreadyExists.ToString("D")
            };
        }

        var salt = encryptionService.GenerateSalt();
        var role = await roleRepository.GetRoleByRoleCode(request.RoleName);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Password = encryptionService.HashPassword(request.Password, salt),
            HashSalt = Convert.ToBase64String(salt),
            RoleId = role.Id,
            CreatedAt = DateTime.UtcNow,
        };

        await userRepository.CreateUserAsync(user);
        
        logger.LogInformation("Пользователь {Email} успешно создан", user.Email);

        return new CreateUserSuccessResponse
        {
            UserId = user.Id
        };
    }
    
    /// <inheritdoc />
    public async Task<ConnectTokenResponse> ConnectTokenAsync(ConnectTokenRequest request)
    {
        var user = await userRepository.GetUserByEmailAsync(request.Email);

        if (user == null)
        {
            return CreateErrorResponse<ConnectTokenErrorResponse>(ErrorCodes.UserDoesNotExist);
        }
        
        if (!encryptionService.VerifyPassword(request.Password, user.Password))
        {
            return CreateErrorResponse<ConnectTokenErrorResponse>(ErrorCodes.CredentialsAreNotValid);
        }
        
        user.RefreshToken = CreateRefreshTokenAsync();

        await userRepository.UpdateUserAsync(user);

        var jwtUserData = await CreateJwtUserDataAsync(user);
        
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
        var user = await userRepository.GetUserByUserIdAsync(userId);
        if (user == null) return CreateErrorResponse<RevocateTokenErrorResponse>(ErrorCodes.UserDoesNotExist);

        if (user.RefreshToken != null)
        {
            user.RefreshToken.Active = false;
            await userRepository.UpdateUserAsync(user);
        }

        logger.LogInformation("Пользователь {UserId} успешно вышел из системы.", user.Id);
        
        return new RevocateTokenSuccessResponse()
        {
            Message = $"Пользователь вышел из системы в {DateTime.UtcNow:O} (UTC)."
        };
    }
    
    /// <inheritdoc />
    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var isValid = jwtGenerationService.IsTokenValid(request.AccessToken, false);
        if (!isValid) return CreateErrorResponse<RefreshTokenErrorResponse>(ErrorCodes.AccessTokenIsNotValid);

        var userId = jwtGenerationService.GetUserIdFromToken(request.AccessToken);
        var user = await userRepository.GetUserByUserIdAsync(userId);

        if (user == null) return CreateErrorResponse<RefreshTokenErrorResponse>(ErrorCodes.UserDoesNotExist);

        var token = user.RefreshToken;
        ArgumentNullException.ThrowIfNull(token);
        
        if (!token.Active) return CreateErrorResponse<RefreshTokenErrorResponse>(ErrorCodes.RefreshTokenIsNotActive);
        if (token.ExpirationDate < DateTime.UtcNow) return CreateErrorResponse<RefreshTokenErrorResponse>(ErrorCodes.RefreshTokenHasExpired);
        if (token.Value != request.RefreshToken) return CreateErrorResponse<RefreshTokenErrorResponse>(ErrorCodes.RefreshTokenIsNotCorrect);

        var jwtUserData = await CreateJwtUserDataAsync(user);
        
        var newAccessToken = jwtGenerationService.GenerateAccessToken(jwtUserData);
        var newRefreshTokenValue = jwtGenerationService.GenerateRefreshToken();

        user.RefreshToken!.Value = newRefreshTokenValue;
        user.RefreshToken.Active = true;
        user.RefreshToken.ExpirationDate = DateTime.UtcNow.AddMinutes(jwtGenerationService.GetRefreshTokenLifetimeInMinutes());

        await userRepository.UpdateUserAsync(user);

        logger.LogInformation("Refresh-токен обновлён для пользователя {UserId}", user.Id);

        return new RefreshTokenSuccessResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshTokenValue,
            RefreshTokenExpirationDate = user.RefreshToken.ExpirationDate
        };
    }

    /// <inheritdoc />
    public async Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request)
    {
        var user = await userRepository.GetUserByUserIdAsync(userId);
        ArgumentNullException.ThrowIfNull(user);
        
        var newPasswordHash = encryptionService.HashPassword(request.Password, Convert.FromBase64String(user.HashSalt));

        await userRepository.ChangeUserPasswordAsync(user.Id, newPasswordHash, user.HashSalt);
    }
    
    /// <summary>
    /// Формирование модели для генерации Jwt
    /// </summary>
    private async Task<JwtUserData> CreateJwtUserDataAsync(User user)
    {
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
            Email = user.Email,
            Claims = claims
        };

        return jwtUserData;
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