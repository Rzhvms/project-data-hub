namespace IdentityLib.ErrorCodes;

/// <summary>
/// Перечисление кодов ошибок, используемых в приложении для авторизации и работы с токенами.
/// </summary>
public enum ErrorCodes
{
    /// <summary>
    /// Произошла непредвиденная ошибка на сервере.
    /// </summary>
    AnUnexpectedErrorOccurred = 40000,

    /// <summary>
    /// Учетные данные пользователя неверны (неправильный пароль или email).
    /// </summary>
    CredentialsAreNotValid = 40001,

    /// <summary>
    /// Access-токен недействителен (невалидный, просрочен или некорректный).
    /// </summary>
    AccessTokenIsNotValid = 40002,

    /// <summary>
    /// Refresh-токен не активен (был отозван или заблокирован).
    /// </summary>
    RefreshTokenIsNotActive = 40003,

    /// <summary>
    /// Срок действия refresh-токена истек.
    /// </summary>
    RefreshTokenHasExpired = 40004,

    /// <summary>
    /// Refresh-токен некорректный (не совпадает с ожидаемым значением).
    /// </summary>
    RefreshTokenIsNotCorrect = 40005,

    /// <summary>
    /// Пользователь с указанными учетными данными уже существует.
    /// </summary>
    UserAlreadyExists = 40006,
    
    /// <summary>
    /// Пользователь с указанными учетными данными не существует.
    /// </summary>
    UserDoesNotExist = 40007
}