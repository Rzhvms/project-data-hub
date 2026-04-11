# JWT в IdentityLib.Jwt

## 1. Назначение

`IdentityLib.Jwt` — это библиотека для генерации и проверки JWT-токенов в приложении. Она решает три основные задачи:

1. Формирует `access token` для авторизации запросов к API.
2. Формирует `id token` для передачи клиенту данных об аутентифицированном пользователе.
3. Формирует `refresh token` для продления сессии без повторного ввода логина и пароля.

Дополнительно библиотека:

* хранит и загружает RSA-ключи;
* валидирует токены;
* извлекает `UserId` из токена;
* скрывает детали криптографии и подписания токенов от прикладного кода.

---

## 2. Основные сущности

### 2.1 `JwtUserData`

`JwtUserData` — это минимальная модель данных пользователя, достаточная для выпуска JWT.

```csharp
public sealed record JwtUserData
{
    public Guid UserId { get; set; }
    public string? UserName { get; set; } = null;
    public string? Email { get; set; } = null;
    public string? FirstName { get; set; } = null;
    public string? LastName { get; set; } = null;
    public IReadOnlyCollection<Claim>? Claims { get; set; } = null;
}
```

#### Назначение полей

* **UserId** — основной идентификатор пользователя. Попадает в claim `NameIdentifier`.
* **UserName** — имя пользователя для отображения и для `id token`.
* **Email** — email пользователя.
* **FirstName** — имя.
* **LastName** — фамилия.
* **Claims** — произвольные дополнительные claims, которые нужно включить в токен.

#### Почему модель отдельная

Библиотека не должна зависеть от доменной модели приложения, например от `UserModel` или сущностей ORM. Вместо этого приложение само маппит свои данные в `JwtUserData`.

---

## 3. Настройки

### 3.1 `JwtSettings`

```csharp
public sealed class JwtSettings
{
    public required AccessTokenSettings AccessTokenSettings { get; init; }
    public required RefreshTokenSettings RefreshTokenSettings { get; init; }
}
```

Это корневая модель настроек JWT.

#### Состав

* `AccessTokenSettings` — параметры access token.
* `RefreshTokenSettings` — параметры refresh token.

---

### 3.2 `AccessTokenSettings`

```csharp
public sealed class AccessTokenSettings
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public long LifeTimeInSeconds { get; init; }
}
```

#### Поля

* **Issuer** — издатель токена.
* **Audience** — получатель токена.
* **LifeTimeInSeconds** — время жизни access token в секундах.

#### Роль в системе

Эти значения используются при создании и проверке access token и id token. Если issuer или audience не совпадут при валидации, токен будет считаться недействительным.

---

### 3.3 `RefreshTokenSettings`

```csharp
public sealed class RefreshTokenSettings
{
    public int Length { get; init; }
    public int LifeTimeInMinutes { get; init; }
}
```

#### Поля

* **Length** — длина refresh token в байтах.
* **LifeTimeInMinutes** — срок жизни refresh token в минутах.

#### Комментарий

Refresh token в этой реализации — это криптографически стойкая случайная строка, а не JWT.

---

### 3.4 `JwtKeyStoreSettings`

```csharp
public sealed class JwtKeyStoreSettings
{
    public required string DirectoryPath { get; init; }
    public string PrivateKeyFileName { get; init; } = "jwt-private.pem";
    public string PublicKeyFileName { get; init; } = "jwt-public.pem";
    public int RsaKeySize { get; init; } = 4096;
}
```

#### Поля

* **DirectoryPath** — директория, где хранятся RSA-ключи.
* **PrivateKeyFileName** — имя файла приватного ключа.
* **PublicKeyFileName** — имя файла публичного ключа.
* **RsaKeySize** — размер RSA-ключа.

#### Как работает путь

Если `DirectoryPath` указан как относительный путь, он будет интерпретирован относительно `AppContext.BaseDirectory`.

Пример:

```json
{
  "JwtKeys": {
    "DirectoryPath": "keys"
  }
}
```

Это означает, что ключи будут искать или создавать в папке `keys` внутри базовой директории приложения.

---

## 4. RSA-ключи

### 4.1 Назначение

RSA-ключи нужны для асимметричного подписания JWT:

* **private key** — используется только для подписи токенов;
* **public key** — используется только для проверки подписи.

Это означает, что сервис, который выпускает токены, хранит приватный ключ, а сервисы, которые только проверяют токены, могут хранить публичный ключ.

---

### 4.2 `IRsaKeyProvider`

`IRsaKeyProvider` скрывает способ получения ключей.

В текущей реализации используется файловый провайдер:

```csharp
public sealed class FileSystemRsaKeyProvider : IRsaKeyProvider
```

---

### 4.3 `FileSystemRsaKeyProvider`

Этот класс отвечает за:

1. Проверку существования директории.
2. Создание директории при необходимости.
3. Проверку наличия файлов ключей.
4. Генерацию ключей, если их нет.
5. Загрузку ключей из файлов.
6. Создание `RsaSecurityKey` для подписи и валидации.

#### Алгоритм работы

1. Берутся настройки `JwtKeyStoreSettings`.
2. Вычисляется физический путь к директории.
3. Директория создается, если ее нет.
4. Формируются пути к private/public PEM-файлам.
5. Если файлов нет — создаётся новый RSA-ключ.
6. Private и public ключ экспортируются в PEM.
7. Затем ключи загружаются обратно из файлов.
8. Создаются `SigningKey` и `ValidationKey`.


Такой подход позволяет один раз создать ключи при первом запуске, а потом использовать их повторно между перезапусками приложения.

---

### 4.4 Рекомендация по использованию

В production важно хранить каталог с ключами в защищенном месте и контролировать права доступа. Private key не должен быть доступен всем пользователям системы.

---

## 5. Генерация JWT

### 5.1 `JwtGenerationService`

`JwtGenerationService` — это главный сервис библиотеки. Он отвечает за создание и проверку токенов.

```csharp
public sealed class JwtGenerationService : IJwtGenerationService
```

#### Зависимости

* `IOptions<JwtSettings>` — настройки токенов.
* `IRsaKeyProvider` — источник RSA-ключей.
* `TimeProvider` — источник текущего времени.

---

## 6. Типы токенов

### 6.1 Access Token

Access token используется для доступа к защищенным API-методам.

#### Особенности

* содержит `UserId`;
* может содержать дополнительные claims;
* имеет ограниченное время жизни;
* подписывается RSA-ключом.

#### Метод генерации

```csharp
string GenerateAccessToken(JwtUserData user, bool isRestoringPassword = false)
```

#### Сценарий восстановления пароля

Если `isRestoringPassword = true`, токен получает короткое время жизни — 5 минут. Это нужно, чтобы токен для сброса пароля был краткоживущим и менее опасным.

---

### 6.2 ID Token

ID token нужен для передачи клиенту информации о пользователе после аутентификации.

#### Что содержит

* `UserId`;
* `UserName`;
* `Email`;
* `FirstName`;
* `LastName`;
* дополнительные claims.

#### Метод генерации

```csharp
string GenerateIdToken(JwtUserData user)
```

#### Когда нужен

ID token нужен в сценариях, где есть клиентское приложение, которому важно получить информацию о пользователе после входа. Если система работает только как backend API, этот токен может быть не нужен.

---

### 6.3 Refresh Token

Refresh token — Случайная строка, сгенерированная криптографически стойким генератором.

#### Метод генерации

```csharp
string GenerateRefreshToken()
```

#### Назначение

Refresh token используется для получения нового access token без повторного логина пользователя.

---

## 7. Как формируются claims

### 7.1 Claims для access token

Метод `BuildAccessTokenClaims` создаёт базовый набор claims:

* `ClaimTypes.NameIdentifier` — идентификатор пользователя;
* `jti` — уникальный идентификатор токена;
* дополнительные claims из `JwtUserData.Claims`.

---

### 7.2 Claims для id token

Метод `BuildIdTokenClaims` добавляет:

* `NameIdentifier`;
* `jti`;
* `Name`;
* `GivenName`;
* `Email`;
* `Surname`;
* дополнительные claims.

#### Замечание

Если `UserName` и `FirstName` отличаются, важно понимать, как они интерпретируются в вашем приложении. В текущей реализации `UserName` добавляется в `Name` и `GivenName`.

---

## 8. Подписание токена

### 8.1 `CreateToken`

Метод `CreateToken` создаёт JWT с помощью `JwtSecurityTokenHandler`.

#### Параметры

* `issuer` — издатель токена;
* `audience` — аудитория токена;
* `claims` — список claims;
* `notBefore` — время, раньше которого токен не должен использоваться;
* `expires` — время истечения токена;
* `signingKey` — RSA-ключ для подписи.

#### Подпись

Используется алгоритм:

```csharp
SecurityAlgorithms.RsaSha256
```

Это RSA с SHA-256.

---

## 9. Проверка токена

### 9.1 `ValidateToken`

Этот метод проверяет:

* что токен не пустой;
* что подпись корректна;
* что issuer совпадает;
* что audience совпадает;
* что срок действия токена валиден, если включена проверка lifetime.

#### Валидационные параметры

```csharp
ValidateIssuer = true
ValidateAudience = true
ValidateLifetime = validateLifetime
ValidateIssuerSigningKey = true
RequireSignedTokens = true
RequireExpirationTime = validateLifetime
ClockSkew = TimeSpan.Zero
```

#### Почему `ClockSkew = TimeSpan.Zero`

Это убирает допустимое окно расхождения времени между серверами. Поведение становится более строгим и предсказуемым.

---

### 9.2 `GetUserIdFromToken`

Метод извлекает `UserId` из claim `NameIdentifier`.

#### Поведение

1. Токен валидируется без проверки lifetime.
2. Извлекается claim `NameIdentifier`.
3. Значение парсится как `Guid`.
4. Если значение отсутствует или некорректно, выбрасывается `InvalidTokenException`.

---

### 9.3 `IsTokenValid`

Метод возвращает `true`, если токен успешно проходит валидацию, иначе `false`.

---

## 10. Жизненный цикл токенов

### Access token

* короткий срок жизни;
* используется для запросов к API;
* должен регулярно обновляться.

### ID token

* предназначен для клиента;
* содержит данные о личности пользователя;
* не должен использоваться как токен доступа к API.

### Refresh token

* длинный срок жизни;
* используется для обновления access token;
* должен храниться особенно аккуратно.

---

## 11. Регистрация в DI

Библиотека предполагает регистрацию через `IServiceCollection`.

Типичная схема:

1. Забиндить `JwtSettings` из секции `Jwt`.
2. Забиндить `JwtKeyStoreSettings` из секции `JwtKeys`.
3. Зарегистрировать `TimeProvider.System`.
4. Зарегистрировать `IRsaKeyProvider`.
5. Зарегистрировать `IJwtGenerationService`.

---

## 12. Пример конфигурации `appsettings.json`

```json
{
  "Jwt": {
    "AccessTokenSettings": {
      "Issuer": "IdentityApi",
      "Audience": "IdentityClient",
      "LifeTimeInSeconds": 3600
    },
    "RefreshTokenSettings": {
      "Length": 64,
      "LifeTimeInMinutes": 10080
    }
  },
  "JwtKeys": {
    "DirectoryPath": "keys",
    "PrivateKeyFileName": "jwt-private.pem",
    "PublicKeyFileName": "jwt-public.pem",
    "RsaKeySize": 4096
  }
}
```

---

## 13. Как использовать библиотеку в приложении

### Шаг 1. Маппинг пользователя

Данные доменного пользователя преобразуются в `JwtUserData`.

### Шаг 2. Генерация access token

Вызов `GenerateAccessToken(user)`.

### Шаг 3. Генерация id token

Вызов `GenerateIdToken(user)`.

### Шаг 4. Генерация refresh token

Вызов `GenerateRefreshToken()`.

---

## 14. Краткая схема потока

1. Приложение получает данные пользователя.
2. Преобразует их в `JwtUserData`.
3. `JwtGenerationService` формирует claims.
4. JWT подписывается RSA private key.
5. Токен возвращается клиенту.
6. При проверке токена используется RSA public key.
