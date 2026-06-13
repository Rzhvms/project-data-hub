using Application.Ports.Repositories;
using Application.UseCases.Users.Dto.Request;
using Application.UseCases.Users.Dto.Response;
using Application.UseCases.Users.Interfaces;
using CoreLib.Audit;
using CoreLib.Exceptions;
using CoreLib.User;
using IdentityLib.Encryption.Interfaces;

namespace Application.UseCases.Users;

/// <inheritdoc />
internal class UserUseCaseManager(
    IUserRepository userRepository,
    IPasswordEncryptionService encryptionService,
    IAuditService auditService,
    ICurrentUserService currentUser) : IUserUseCaseManager
{
    /// <inheritdoc />
    public async Task<GetUserListResponse> GetUserListAsync()
    {
        var response = await userRepository.GetAllUsersAsync();
        return new GetUserListResponse
        {
            UserList = response.Select(user => new GetUserResponse
            {
                Id = user.Id,
                Email = user.Email,
                RoleId = user.RoleId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Patronymic = user.Patronymic
            }).ToList()
        };
    }

    /// <inheritdoc />
    public async Task<GetUserResponse> GetUserByIdAsync(Guid userId)
    {
        var user = await userRepository.GetUserByUserIdAsync(userId);

        if (user is null) throw new EntityNotFoundException("Пользователь не найден");

        return new GetUserResponse()
        {
            Id = user.Id,
            Email = user.Email,
            RoleId = user.RoleId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Patronymic = user.Patronymic,
        };
    }

    /// <inheritdoc />
    public async Task DeleteUserByIdAsync(Guid userId)
    {
        await userRepository.DeleteUserByIdAsync(userId);
        await auditService.LogAsync("DeleteUser", "User", userId, currentUser.UserId, currentUser.UserName, $"Удален пользователь {userId}");
    }
    
    /// <inheritdoc />
    public async Task<ChangeUserPasswordResponse> ChangePasswordAsync(Guid id, ChangeUserPasswordRequest request)
    {
        var user = await userRepository.GetUserByUserIdAsync(id);
        if (user == null) return new ChangeUserPasswordResponse { IsSuccess = false, Message = "Пользователь не найден" };
        
        var oldPasswordHash = encryptionService.HashPassword(request.OldPassword, Convert.FromBase64String(user.HashSalt));
        if (oldPasswordHash != user.Password) return new ChangeUserPasswordResponse { IsSuccess = false, Message = "Старый пароль неверный" };
        
        var newPasswordHash = encryptionService.HashPassword(request.NewPassword, Convert.FromBase64String(user.HashSalt));
        await userRepository.ChangeUserPasswordAsync(user.Id, newPasswordHash, user.HashSalt);
        await auditService.LogAsync("ChangePassword", "User", user.Id, currentUser.UserId, currentUser.UserName, $"Смена пароля пользователем");

        return new ChangeUserPasswordResponse { IsSuccess = true, Message = "Пароль успешно изменен" };
    }
}