using Application.Ports.Repositories;
using Application.UseCases.Users.Dto.Request;
using Application.UseCases.Users.Dto.Response;
using Application.UseCases.Users.Interfaces;
using IdentityLib.Encryption.Interfaces;

namespace Application.UseCases.Users;

/// <inheritdoc />
internal class UserUseCaseManager(IUserRepository userRepository, IPasswordEncryptionService encryptionService) : IUserUseCaseManager
{
    /// <inheritdoc />
    public async Task<ChangeUserPasswordResponse> ChangePasswordAsync(Guid id, ChangeUserPasswordRequest request)
    {
        var user = await userRepository.GetUserByUserIdAsync(id);
        if (user == null) return new ChangeUserPasswordResponse { IsSuccess = false, Message = "Пользователь не найден" };
        
        var oldPasswordHash = encryptionService.HashPassword(request.OldPassword, Convert.FromBase64String(user.HashSalt));
        if (oldPasswordHash != user.Password) return new ChangeUserPasswordResponse { IsSuccess = false, Message = "Старый пароль неверный" };
        
        var newPasswordHash = encryptionService.HashPassword(request.NewPassword, Convert.FromBase64String(user.HashSalt));
        await userRepository.ChangeUserPasswordAsync(user.Id, newPasswordHash, user.HashSalt);

        return new ChangeUserPasswordResponse { IsSuccess = true, Message = "Пароль успешно изменен" };
    }
}