# Техническая документация project-data-hub

## Содержание

1. [Инструкция по сборке](#1-инструкция-по-сборке)
2. [Общая архитектура](#2-общая-архитектура)
3. [Технологический стек](#3-технологический-стек)
4. [ApiGateway — API Gateway (YARP Reverse Proxy)](#4-apigateway--api-gateway-yarp-reverse-proxy)
5. [IdentityApi — Сервис аутентификации и управления пользователями](#5-identityapi--сервис-аутентификации-и-управления-пользователями)
6. [ProviderApi — Сервис управления портфолио проектов](#6-providerapi--сервис-управления-портфолио-проектов)
7. [CoreLib — Общая библиотека ядра](#7-corelib--общая-библиотека-ядра)
8. [IdentityLib — Библиотека Identity](#8-identitylib--библиотека-identity)
9. [Фронтенд](#9-фронтенд)
10. [База данных](#10-база-данных)
11. [Объектное хранилище MinIO](#11-объектное-хранилище-minio)
12. [Docker и Docker Compose](#12-docker-и-docker-compose)
13. [Полная схема API Endpoints](#13-полная-схема-api-endpoints)
14. [Модель безопасности](#14-модель-безопасности)

---

## 1. Инструкция по сборке

### Бэкенд

**Требования:**
- .NET 10.0 SDK
- PostgreSQL 16 (или Docker)
- MinIO (или Docker)

**Локальный запуск (Development):**
```bash
# 1. Запустить инфраструктуру (PostgreSQL + MinIO)
cd api && docker compose up -d postgres minio

# 2. Запустить сервисы (каждый в отдельном терминале)
cd api/Apps/IdentityApi/Api && dotnet run --environment Development
cd api/Apps/ProviderApi/Api && dotnet run --environment Development
cd api/Apps/ApiGateway/Api && dotnet run --environment Development
```

**Сборка всех проектов:**
```bash
cd api && dotnet build
```

**Запуск всех сервисов через Docker Compose:**
```bash
cd api && docker compose up --build
```

### Фронтенд

**Требования:**
- Node.js 24.15.0
- npm 11.17.0

**Локальный запуск (Development):**
```bash
cd frontend
npm install
npm start
# Приложение доступно на http://localhost:4200
```

**Сборка для production:**
```bash
cd frontend
npm run build
# Результат в директории dist/
```

**Переменные окружения:**
```bash
# frontend/.env
API_HOST=http://localhost:5000
```

---

## 2. Общая архитектура

**project-data-hub** — это web-приложение для управления портфолио проектов архитектурно-строительной компании.

```
┌──────────────────────┐
│                      │
│    Angular Client    │
│    (port 4200)       │
│                      │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐     ┌──────────────────┐
│                      │     │                  │
│     ApiGateway       │────▶│   IdentityApi    │
│   (YARP Proxy)       │     │  (Auth/Users)    │────┐
│    port 7060         │     │   port 5248      │    │
│                      │     │                  │    │
│                      │     └──────────────────┘    │
│                      │                             │
│                      │     ┌──────────────────┐    │
│                      │     │                  │    │
│                      │────▶│   ProviderApi    │    │
│                      │     │  (Projects/      │    │
│                      │     │   Portfolio)     │    │
│                      │     │   port 5185      │    │
│                      │     │                  │    │
└──────────────────────┘     └────────┬─────────┘    │
                                      ├──────────────┘
                           ┌──────────▼──────────┐   
                           │                     │
                           │     PostgreSQL      │
                           │  ┌───────────────┐  │
                           │  │   Identity    │  │
                           │  ├───────────────┤  │
                           │  │   Provider    │  │
                           │  └───────────────┘  │
                           │                     │
                           │       MinIO         │
                           │   (S3 Storage)      │
                           └─────────────────────┘
```

### Ключевые принципы архитектуры

- **Clean Architecture** — каждый сервис разделен на слои: `Domain`, `Application`, `Infrastructure`, `Api`.
- **Микросервисная архитектура** — сервисы независимы, имеют собственные базы данных.
- **API Gateway** — единая точка входа через YARP Reverse Proxy.
- **JWT-аутентификация** — сквозная аутентификация через RSA-подписанные JWT-токены.

---

## 3. Технологический стек

### Бэкенд

| Компонент | Технология |
|-----------|-----------|
| Runtime | .NET 10.0 (net10.0) |
| Язык | C# 13 |
| База данных | PostgreSQL 16 |
| ORM / Data Access | Dapper 2.1.72 |
| Миграции | FluentMigrator 8.0.1 |
| Объектное хранилище | MinIO (S3-совместимое) |
| API Gateway | Yarp.ReverseProxy 2.3.0 |
| Аутентификация | JWT Bearer (RSA-256) |
| Хэширование паролей | Argon2id (Konscious.Security.Cryptography) |
| Документы | DocumentFormat.OpenXML 3.5.1, DocxTemplater 2.7.3, ShapeCrawler 0.79.3, MiniWord 0.9.2 |
| Логирование | Serilog.AspNetCore 10.0.0 |
| Контейнеризация | Docker, Docker Compose |
| Окружение | Development / Production |

### Фронтенд

| Компонент | Технология |
|-----------|-----------|
| Фреймворк | Angular 21.2.14 (standalone components) |
| Язык | TypeScript 5.9.3 |
| UI Kit | Taiga UI 5.10.0 |
| Стилизация | SCSS, Less (Taiga UI) |
| Сборка | Angular CLI 21.2.12 (esbuild) |
| Тестирование | Vitest 4.1.5 |
| Линтер | ESLint 10.3.0 + Prettier 3.8.3 |
| HTTP клиент | Angular HttpClient + jwt-decode 4.0.0 |
| Реактивность | RxJS 7.8.2 |
| Runtime | Node.js 24.15.0 / npm 11.17.0 |

---

## 4. ApiGateway — API Gateway (YARP Reverse Proxy)

**Расположение:** `api/Apps/ApiGateway/`

### Назначение
Единая точка входа для всех клиентских запросов. Маршрутизирует запросы к соответствующим микросервисам на основе префикса пути.

### Конфигурация маршрутизации

| Путь | Кластер | Адрес назначения |
|------|---------|------------------|
| `/identity/{**catch-all}` → с удалением префикса `/identity` | `identity` | `http://localhost:5248/` |
| `/provider/{**catch-all}` → с удалением префикса `/provider` | `provider` | `http://localhost:5185/` |

### Swagger UI
В режиме Development на `/swagger` агрегирует Swagger-документацию обоих сервисов:
- `/identity/swagger/v1/swagger.json` — Identity API
- `/provider/swagger/v1/swagger.json` — Provider API

### Startup (`Startup.cs`)
- Регистрирует CORS (разрешены все источники)
- Настраивает Reverse Proxy из секции `ReverseProxy` конфигурации
- Конфигурирует Swagger UI для обоих сервисов

---

## 5. IdentityApi — Сервис аутентификации и управления пользователями

**Расположение:** `api/Apps/IdentityApi/`

### 5.1. Слои архитектуры

```
IdentityApi/
├── Api/              # ASP.NET Web API (контроллеры, Startup)
├── Application/      # Use Cases, Ports (интерфейсы репозиториев)
├── Domain/           # Сущности: User, Role, RefreshToken
└── Infrastructure/   # Репозитории, миграции, сиды
```

### 5.2. API Контроллеры

#### `AuthController` — `[Route("api/connect")]`

| Метод | Endpoint | Аутентификация | Описание |
|-------|----------|---------------|----------|
| `POST` | `/api/connect/register` | Нет | Регистрация нового пользователя |
| `POST` | `/api/connect/token` | `[AllowAnonymous]` | Авторизация, получение JWT |
| `POST` | `/api/connect/token/revoke` | `[Authorize]` | Отзыв refresh-токена |
| `POST` | `/api/connect/token/refresh` | `[AllowAnonymous]` | Обновление access-токена |
| `POST` | `/api/connect/change-password` | `[Authorize]` | Смена пароля текущим пользователем |

**Логика работы:**
- **Register:** Проверка уникальности email → хэширование пароля (Argon2id) → сохранение пользователя.
- **Login (ConnectToken):** Поиск пользователя по email → проверка пароля → генерация refresh-токена → генерация JWT access-токена.
- **Revoke:** Деактивация refresh-токена пользователя.
- **Refresh:** Проверка старого access-токена → сверка refresh-токена с хранимым → генерация новой пары токенов.
- **ChangePassword:** Хэширование нового пароля с тем же salt → обновление в БД.

#### `UserController` — `[Route("user")]`

| Метод | Endpoint | Аутентификация | Описание |
|-------|----------|---------------|----------|
| `GET` | `/user/list` | `[Authorize]` | Получить список всех пользователей |
| `GET` | `/user/{userId}` | `[Authorize]` | Получить пользователя по ID |
| `DELETE` | `/user/{userId}` | `[Authorize]` | Удалить пользователя по ID |
| `PATCH` | `/user/change-password` | `[Authorize]` | Сменить свой пароль (со старым паролем) |

#### `WellKnownController` — `[Route(".well-known")]`

| Метод | Endpoint | Описание |
|-------|----------|----------|
| `GET` | `/.well-known/jwks.json` | Публичные RSA-ключи в формате JWKS |
| `GET` | `/.well-known/openid-configuration` | OpenID Connect discovery документ |

**Наследует `SystemControllerBase`** — доступ только через заголовок `X-System-Request: proxy`.

### 5.3. Domain Entities (Domain Layer)

#### `User` (record)
| Поле | Тип | Описание |
|------|-----|----------|
| `Id` | `Guid` | Идентификатор |
| `Email` | `string` | Email (уникальный, max 50) |
| `Password` | `string` | Хэш пароля (формат: argon2id:iter:mem:par:salt:hash) |
| `HashSalt` | `string` | Salt для хэширования (Base64) |
| `IsEmailConfirmed` | `bool` | Флаг подтверждения email |
| `RoleId` | `Guid` | FK к роли |
| `FirstName` | `string?` | Имя |
| `LastName` | `string?` | Фамилия |
| `Patronymic` | `string?` | Отчество |
| `CreatedAt` | `DateTime` | Дата создания |
| `UpdatedAt` | `DateTime?` | Дата обновления |
| `RefreshToken` | `RefreshToken?` | Связанный refresh-токен |

#### `RefreshToken`
| Поле | Тип | Описание |
|------|-----|----------|
| `Id` | `Guid` | Идентификатор |
| `UserId` | `Guid` | FK к пользователю |
| `Value` | `string` | Значение токена |
| `Active` | `bool` | Активен ли |
| `ExpirationDate` | `DateTime` | Дата истечения |

#### `Role` (record)
| Поле | Тип | Описание |
|------|-----|----------|
| `Id` | `Guid` | Идентификатор |
| `RoleCode` | `string` | Код: `Viewer`, `Editor`, `Administrator` |
| `Name` | `string` | Отображаемое название |
| `PermissionsMask` | `int` | Маска прав (зарезервировано) |
| `IsSystem` | `bool` | Системная (неудаляемая) |
| `CreatedAt` | `DateTime` | Дата создания |
| `UpdatedAt` | `DateTime?` | Дата обновления |

Роль определяется кодом (`RoleCode`): `Viewer`, `Editor`, `Administrator`. Проверка прав осуществляется по claim `ClaimTypes.Role` в JWT.

### 5.4. Application Layer (Use Cases)

#### `AuthUseCaseManager` (IAuthUseCaseManager)
- `CreateUserAsync(CreateUserRequest)` — регистрация пользователя
- `ConnectTokenAsync(ConnectTokenRequest)` — логин, выдача JWT
- `RevocateRefreshTokenAsync(Guid)` — выход из системы
- `RefreshTokenAsync(RefreshTokenRequest)` — обновление access-токена
- `ChangePasswordAsync(Guid, ChangePasswordRequest)` — смена пароля

#### `UserUseCaseManager` (IUserUseCaseManager)
- `GetUserListAsync()` — список пользователей
- `GetUserByIdAsync(Guid)` — пользователь по ID
- `DeleteUserByIdAsync(Guid)` — удаление пользователя
- `ChangePasswordAsync(Guid, ChangeUserPasswordRequest)` — смена пароля (со старым)

### 5.5. Infrastructure Layer

#### Репозитории
- **`UserRepository`** — полный CRUD для пользователей (Dapper + PostgreSQL)
- **`RoleRepository`** — получение ролей по коду и ID

#### Миграции (FluentMigrator)
| Миграция | Описание |
|----------|----------|
| `Date_202604290210_AddIdentityTables` | Создание таблиц `User`, `Role`, `RefreshToken` |
| `AddSystemRolesSeed` | Seed: Viewer, Editor, Administrator |
| `AddDefaultUserSeed` | Seed: системный администратор |
| `Date_202606140100_AddAuditLog` | Таблица `AuditLog` (с использованием `EntityMapper`) |

#### Seeds
- **AddSystemRolesSeed** — создает три системные роли с предустановленными правами.
- **AddDefaultUserSeed** — создает тестового пользователя `system_user@inpad.ru`.

### 5.6. Startup (`Startup.cs`)

Порядок инициализации:
1. Регистрация контроллеров
2. `AddApplication()` — use case менеджеры
3. `AddInfrastructure(connectionString)` — Npgsql, FluentMigrator, репозитории
4. `AddCoreControllers()` — фильтры, политики авторизации, route conventions
5. `AddIdentityLib(configuration)` — JWT-сервисы, Argon2id
6. JWT Bearer Authentication (с `SystemRequestHttpHandler` для внутренних запросов). **Валидация:** используется `IRsaKeyProvider.ValidationKey` напрямую (RSA-ключ из файла). Issuer: `http://localhost:5248/identity`
7. `AddAudit(connectionString, configuration)` — регистрация `IAuditService` (DapperAuditService) и `AuditLogCleanupHostedService`
8. CORS (все разрешено)
9. Swagger c JWT Bearer security definition

Middleware pipeline:
1. Developer Exception Page / Swagger (Development)
2. `MigrateDatabase()` — авто-применение миграций FluentMigrator
3. `ErrorHandlingMiddleware` — глобальный обработчик ошибок
4. `AuditLogMiddleware` — автоматическая запись в журнал аудита
5. Routing → Authentication → Authorization → Endpoints

---

## 6. ProviderApi — Сервис управления портфолио проектов

**Расположение:** `api/Apps/ProviderApi/`

### 6.1. Слои архитектуры

```
ProviderApi/
├── Api/              # ASP.NET Web API (контроллеры, Startup, Templates)
├── Application/      # Use Cases, Services (генераторы), Ports
├── Domain/           # Сущности: ProjectCard, ProjectMetrics, ProjectImage и др.
└── Infrastructure/   # Репозитории, миграции, seeds, Storages
```

### 6.2. API Контроллеры

#### `ProjectController` — `[Route("api/project")]`

| Метод | Endpoint | Аутентификация | Описание |
|-------|----------|---------------|----------|
| `GET` | `/api/project/list` | `[Authorize(Roles = "Viewer,Editor,Administrator")]` | Список проектов |
| `GET` | `/api/project/{projectId}` | `[Authorize(Roles = "Viewer,Editor,Administrator")]` | Полная информация о проекте |
| `POST` | `/api/project/create` | `[Authorize(Roles = "Editor,Administrator")]` | Создание нового проекта |
| `POST` | `/api/project/publish` | `[Authorize(Roles = "Editor,Administrator")]` | Публикация проекта |
| `PATCH` | `/api/project/update/{projectId}` | `[Authorize(Roles = "Editor,Administrator")]` | Обновление проекта |
| `DELETE` | `/api/project/delete/{projectId}` | `[Authorize(Roles = "Administrator")]` | Удаление проекта |
| `GET` | `/api/project/draft/{projectId}` | `[Authorize(Roles = "Viewer,Editor,Administrator")]` | Получение черновика |
| `POST` | `/api/project/{projectId}/image` | `[Authorize(Roles = "Editor,Administrator")]` | Загрузка изображения |
| `GET` | `/api/project/{projectId}/images` | `[Authorize(Roles = "Viewer,Editor,Administrator")]` | Список изображений |
| `DELETE` | `/api/project/{projectId}/images/{imageId}` | `[Authorize(Roles = "Administrator")]` | Удаление изображения |

**Детали реализации `ProjectUseCaseManager`:**
- **CreateProject:** Создает `ProjectCard` со статусом `Draft`, сохраняя `ProjectDraftData` (JSON) в таблицу черновиков.
- **GetSimpleProjectList:** Возвращает упрощенный список проектов.
- **GetFullProject:** Возвращает проект со всеми полями, включая категории и участников.
- **UpdateProject:** Работает с черновиком, синхронизирует связи с категориями и участниками.
- **PublishProject:** Валидирует черновик → публикует → удаляет черновик. После публикации вызывает `IApiConnectorService.SendAsync("PublishProject", draftData)`.
- **Валидация перед публикацией:** `Title`, `CityRegion`, `ProjectStatus`, `ObjectType`, `InpadRole`, `ShortDescription`, `CategoryIdList`.

#### `ProjectMetricsController` — `[Route("api/project/metrics")]`

| Метод | Endpoint | Аутентификация | Описание |
|-------|----------|---------------|----------|
| `GET` | `/api/project/metrics/{projectId}` | `[Authorize(Roles = "Viewer,Editor,Administrator")]` | Получить метрики |
| `POST` | `/api/project/metrics/{projectId}` | `[Authorize(Roles = "Editor,Administrator")]` | Добавить метрики |
| `DELETE` | `/api/project/metrics/{projectId}` | `[Authorize(Roles = "Administrator")]` | Удалить метрики |
| `PUT` | `/api/project/metrics/{projectId}` | `[Authorize(Roles = "Administrator")]` | Полностью обновить метрики |

#### `CategoryController`

| Метод | Endpoint | Аутентификация | Описание |
|-------|----------|---------------|----------|
| `GET` | `/api/category/list` | `[Authorize(Roles = "Viewer,Editor,Administrator")]` | Список категорий |
| `GET` | `/api/category/{categoryId}` | `[Authorize(Roles = "Viewer,Editor,Administrator")]` | Категория по ID |
| `POST` | `/api/category/create` | `[Authorize(Roles = "Administrator")]` | Создать категорию |
| `PATCH` | `/api/category/{categoryId}` | `[Authorize(Roles = "Administrator")]` | Обновить категорию |
| `DELETE` | `/api/category/delete/{categoryId}` | `[Authorize(Roles = "Administrator")]` | Удалить категорию |

#### `ParticipantController`

| Метод | Endpoint | Аутентификация | Описание |
|-------|----------|---------------|----------|
| `GET` | `/api/participant/list` | `[Authorize(Roles = "Viewer,Editor,Administrator")]` | Список участников |
| `GET` | `/api/participant/{participantId}` | `[Authorize(Roles = "Viewer,Editor,Administrator")]` | Участник по ID |
| `POST` | `/api/participant/create` | `[Authorize(Roles = "Administrator")]` | Создать участника |
| `PATCH` | `/api/participant/{participantId}` | `[Authorize(Roles = "Administrator")]` | Обновить участника |
| `DELETE` | `/api/participant/delete/{participantId}` | `[Authorize(Roles = "Administrator")]` | Удалить участника |

#### `AuditController` — в CoreLib, доступен в обоих сервисах

| Метод | Endpoint | Аутентификация | Описание |
|-------|----------|---------------|----------|
| `GET` | `/api/audit/logs` | `[Authorize(Roles = "Administrator")]` | Просмотр журнала аудита |

#### `ExportFileController`

| Метод | Endpoint | Аутентификация | Описание |
|-------|----------|---------------|----------|
| `GET` | `/api/export/projects/{projectId}/presentation` | `[AllowAnonymous]` | Выгрузка .pptx |
| `POST` | `/api/export/projects/{projectId}/portfolio` | `[AllowAnonymous]` | Выгрузка .docx |

### 6.3. Domain Entities (Domain Layer)

#### `ProjectCard` (record)
| Поле | Тип | Описание |
|------|-----|----------|
| `Id` | `Guid` | Идентификатор |
| `Title` | `string` | Полное название |
| `ShortTitle` | `string?` | Краткое название |
| `CityRegion` | `string` | Город / регион |
| `Address` | `string?` | Адрес |
| `DesignYearPeriod` | `string?` | Период проектирования |
| `RealizationYear` | `string?` | Год реализации |
| `ProjectStatus` | `string` | Статус проекта |
| `ObjectType` | `string` | Тип объекта |
| `Customer` | `string?` | Заказчик |
| `InpadRole` | `string` | Роль компании |
| `DesignStage` | `string?` | Стадия проектирования |
| `ShortDescription` | `string` | Краткое описание |
| `LongDescription` | `string?` | Полное описание |
| `PublicationStatus` | `ProjectPublicationStatus` | Статус публикации |
| `CreatedAt` | `DateTime` | Дата создания |
| `UpdatedAt` | `DateTime?` | Дата обновления |
| `Publisher` | `string?` | Ответственный (ФИО) |

#### `ProjectCardDraft`
Черновик проекта. Хранит поле `ProjectData` типа `JsonObject?` (PostgreSQL JSONB). Связан 1:1 с `ProjectCard` через `ProjectId`.

#### `ProjectDraftData`
Модель, сериализуемая в JSONB поле черновика. Содержит те же поля, что и `ProjectCard`, плюс `CategoryIdList`, `ParticipantIdList`, `ProjectMetrics`.

#### `ProjectMetrics`
Технико-экономические показатели (ТЭП). Поля: `TotalArea`, `SiteArea`, `BuildingArea`, `BuildingsCount`, `Floors`, `ApartmentsCount`, `ParkingSpacesCount`, `JsonData`.

#### `ProjectImage`
Изображение проекта. Хранит путь в MinIO (`ObjectPath`), метаданные, флаги использования (`UseInSite`, `UseInPresentation`, `UseInPortfolio`, `IsMain`).

#### `ProjectPublicationStatus` (enum)
- `Draft = 1` — Черновик
- `Published = 2` — Опубликован
- `Archived = 3` — Архивный

#### `ProjectCategory` / `ProjectCategoryLink`
Справочник категорий и связь M:N с проектами.

#### `ProjectParticipant` / `ProjectParticipantLink`
Справочник ролей участников и связь M:N с проектами.

### 6.4. Application Layer

| Use Case Manager | Интерфейс | Основные методы |
|-----------------|-----------|-----------------|
| `ProjectUseCaseManager` | `IProjectUseCaseManager` | CRUD проекты, публикация, черновики |
| `ProjectMetricsUseCaseManager` | `IProjectMetricsUseCaseManager` | Управление ТЭП |
| `CategoryUseCaseManager` | `ICategoryUseCaseManager` | CRUD категории |
| `ParticipantUseCaseManager` | `IParticipantUseCaseManager` | CRUD участники |
| `ImageUseCase` | `IImageUseCase` | Загрузка/удаление/получение изображений |
| `ExportFilesUseCaseManager` | `IExportFilesUseCaseManager` | Экспорт презентаций и портфолио |

#### ImageUseCase (работа с MinIO)
- **AddProjectImageAsync:** Валидация файла (jpeg/png/svg/webp, ≤5MB) → загрузка в MinIO → сохранение метаданных в БД.
- **DeleteProjectImageAsync:** Удаление из MinIO и БД.
- **GetProjectImagesAsync:** Возвращает список с публичными URL.

#### Генераторы документов
- **PortfolioDocumentGenerator** (Word .docx) — MiniWord, шаблон `Templates/portfolio-base.docx`.
- **PresentationDocumentGenerator** (PowerPoint .pptx) — ShapeCrawler, шаблон `Templates/presentation-base.pptx`.

### 6.5. Infrastructure Layer

#### Репозитории (Dapper)
| Репозиторий | Интерфейс | Таблицы |
|-------------|-----------|---------|
| `ProjectRepository` | `IProjectRepository` | ProjectCard, ProjectCardDraft |
| `ProjectMetricsRepository` | `IProjectMetricsRepository` | ProjectMetrics |
| `ProjectImageRepository` | `IImageRepository` | ProjectImage |
| `CategoryRepository` | `ICategoryRepository` | ProjectCategory, ProjectCategoryLink |
| `ParticipantRepository` | `IParticipantRepository` | ProjectParticipant, ProjectParticipantLink |
| `ApiConnectorConfigRepository` | `IApiConnectorConfigRepository` | ApiConnectorConfig |

#### Миграции (FluentMigrator)
| Миграция | Описание |
|----------|----------|
| `Date_202605012350_AddProjectTables` | ProjectCard, ProjectCardDraft, ProjectMetrics, ProjectCategory, ProjectCategoryLink, ProjectParticipant, ProjectParticipantLink |
| `Date_202605201000_AddProjectImages` | ProjectImage |
| `Date_202606020100_AlterImages_AddIsMainColumn` | IsMain в ProjectImage |
| `Date_202606140100_AddAuditLog` | AuditLog |
| `Date_202606140200_AddApiConnectorConfig` | ApiConnectorConfig |
| `AddDefaultCategoriesAndParticipants` | Seed: 10 категорий, 8 участников |

#### Seeds
**AddDefaultCategoriesAndParticipants** — 10 категорий, 8 участников.

### 6.6. Startup (`Startup.cs`)

Порядок инициализации аналогичен IdentityApi, дополнен:
- `AddCoreDatabases()` — Dapper type handler для `JsonObject`
- `MinioClient` (singleton)
- JWT-аутентификация (RSA-ключ через `IRsaKeyProvider.ValidationKey`)
- `AddAudit(connectionString, configuration)`
- `AddApiConnectors(configuration)`
- Регистрация `IApiConnectorConfigRepository`

---

## 7. CoreLib — Общая библиотека ядра

**Расположение:** `api/Libs/CoreLib/CoreLib/`

### 7.0. Регистрация компонентов CoreLib

#### `ControllersStartUp`
`AddCoreControllers(IMvcBuilder)` — регистрирует фильтры запросов, политики авторизации (`client`, `internal`), `ControllerRouteConvention`, `AuditLogActionFilter`.

#### `AuditStartUp`
`AddAudit(IServiceCollection, string, IConfiguration)` — регистрирует `IAuditService` (DapperAuditService), `AuditLogCleanupHostedService`, биндит `AuditLogCleanupOptions`.

#### `ApiConnectorStartUp`
`AddApiConnectors(IServiceCollection, IConfiguration)` — если `ApiConnectors__Enabled = true`, регистрирует `HttpApiConnectorService`, иначе `NullApiConnectorService`.


### 7.1. ErrorHandlingMiddleware
- `BaseException` → 400, прочие → 500. Формат: `{ error, type, traceId }`.

### 7.2. Database Helpers
- **EntityMapper** — type-safe имена таблиц/колонок из C# моделей.
- **JsonObjectTypeHandler** — Dapper handler для JSONB.
- **MigrationExtensions** — `app.MigrateDatabase()`.

### 7.3. Аудит

#### `AuditLog`
| Поле | Тип | Описание |
|------|-----|----------|
| `Id` | `Guid` | Идентификатор |
| `UserId` | `Guid` | ID пользователя |
| `UserName` | `string?` | ФИО пользователя |
| `Action` | `string` | Действие |
| `EntityType` | `string?` | Тип сущности |
| `EntityId` | `string?` | ID сущности |
| `Details` | `string?` | Доп. информация (JSON) |
| `IpAddress` | `string?` | IP-адрес |
| `CreatedAt` | `DateTime` | Дата/время |

#### `IAuditService` / `DapperAuditService`
- `LogAsync(AuditLog)` — запись
- `GetLogsAsync(AuditLogFilter)` — пагинированный поиск

#### `AuditLogCleanupOptions`
- `RetentionDays` (env `AuditLogCleanup__RetentionDays`, по умолч. 14)
- `CheckIntervalHours` (env `AuditLogCleanup__CheckIntervalHours`, по умолч. 4)

#### `AuditLogCleanupHostedService`
Фоновый `BackgroundService`, удаляет записи старше `RetentionDays`.

### 7.4. CurrentUserService

#### `ICurrentUserService` / `CurrentUserService`
Извлекает из JWT: `UserId` (NameIdentifier), `UserName` (GivenName + Surname + patronymic), `Role`.

### 7.5. ApiConnectors

#### `ApiConnectorConfig` (record)
| Поле | Тип | Описание |
|------|-----|----------|
| `EventType` | `string` | Тип события |
| `Url` | `string` | URL с плейсхолдерами `{PropertyName}` |
| `HttpMethod` | `string` | GET / POST |
| `HeadersJson` | `string?` | Заголовки (JSON) |
| `BodyTemplate` | `string?` | JSON-body с плейсхолдерами |
| `IsActive` | `bool` | Активен |
| `Order` | `int` | Порядок |
| `TimeoutSeconds` | `int` | Таймаут |

#### `IApiConnectorService` / `HttpApiConnectorService`
- `SendAsync(string eventType, object data)` — находит активные коннекторы по `EventType`, подставляет свойства в URL/Headers/BodyTemplate, выполняет HTTP-запросы.

#### `NullApiConnectorService`
Заглушка при `ApiConnectors__Enabled = false`.

#### `IApiConnectorConfigRepository`
Интерфейс в CoreLib (порт), реализация в ProviderApi.

### 7.6. Exceptions
`BaseException`, `EntityNotFoundException`, `InvalidObjectException`, `InvalidProjectCardException`, `PresentationCreationException`.

### 7.7. Extensions
- `JwtClaimExtensions.GetUserId()`, `GetUserFio()`.

### 7.8. Validation
- `AllowedValuesAttribute` — проверка допустимых значений.

---

## 8. IdentityLib — Библиотека Identity

**Расположение:** `api/Libs/IdentityLib/IdentityLib/`

### 8.1. Шифрование паролей

#### `PasswordEncryptionService` (IPasswordEncryptionService)

Формат хэша: `argon2id:{iterations}:{memory}:{parallelism}:{salt_base64}:{hash_base64}`

Параметры: iterations=2, memory=19456 KB, parallelism=1, salt=16 байт, hash=32 байта.

### 8.2. JWT Generation

#### `JwtGenerationService` (IJwtGenerationService)

**Типы токенов:**
- **Access Token** — RS256, 1 час.
- **Refresh Token** — случайная строка (64 байта, Base64), 7 дней.
- **Id Token** — полная информация о пользователе.

**Claims:**
- `ClaimTypes.NameIdentifier`, `Jti`, `Email`, `GivenName`, `Surname`, `patronymic`, `Role`

**Валидация токена** — через `TokenValidationParameters` с issuer, audience, signing key.

### 8.3. RSA Key Management

#### `FileSystemRsaKeyProvider` (IRsaKeyProvider)
- Ключи: `{DirectoryPath}/jwt-private.pem`, `{DirectoryPath}/jwt-public.pem`
- Размер: 4096 бит
- Автогенерация при первом запуске

### 8.4. Error Codes

| Код | Константа | Описание |
|-----|-----------|----------|
| 40000 | `AnUnexpectedErrorOccurred` | Непредвиденная ошибка |
| 40001 | `CredentialsAreNotValid` | Неверные учетные данные |
| 40002 | `AccessTokenIsNotValid` | Невалидный access-токен |
| 40003 | `RefreshTokenIsNotActive` | Refresh-токен не активен |
| 40004 | `RefreshTokenHasExpired` | Refresh-токен истек |
| 40005 | `RefreshTokenIsNotCorrect` | Refresh-токен не совпадает |
| 40006 | `UserAlreadyExists` | Пользователь уже существует |
| 40007 | `UserDoesNotExist` | Пользователь не найден |

---

## 9. Фронтенд

**Расположение:** `frontend/`

### 9.1. Стек

| Компонент | Технология | Назначение |
|-----------|-----------|-----------|
| Фреймворк | Angular 21.2.14 | Standalone components, без NgModules |
| UI Kit | Taiga UI 5.10.0 | 30+ компонентов, темизация, i18n |
| Язык | TypeScript 5.9.3 | Strict mode |
| Сборка | Angular CLI 21.2.12 | esbuild (по умолчанию в v21) |
| Стилизация | SCSS + Less (Taiga UI) | -- |
| Тестирование | Vitest 4.1.5 | -- |
| Линтер | ESLint 10.3.0 + Prettier 3.8.3 | simple-import-sort, unused-imports |
| HTTP | Angular HttpClient + jwt-decode 4.0.0 | JWT-перехватчик с автоматическим refresh |
| Реактивность | RxJS 7.8.2 | BehaviorSubject, forkJoin, switchMap |

### 9.2. Архитектура

```
frontend/
├── src/
│   ├── main.ts                 # Точка входа, bootstrapApplication
│   ├── index.html              # HTML-хост
│   ├── styles.scss             # Глобальные стили
│   ├── app/
│   │   ├── app.ts              # Корневой компонент (App)
│   │   ├── app.config.ts       # Провайдеры: Router, HttpClient, Taiga UI
│   │   ├── app.routes.ts       # Маршруты: Login (/login), Main (/)
│   │   ├── app.html / app.scss
│   │   ├── pages/
│   │   │   ├── login/          # Страница входа (guestGuard)
│   │   │   └── main/           # Основная страница (authGuard)
│   │   │       ├── main.page.ts / .html / routes
│   │   │       ├── components/main-page-aside/  # Боковое меню
│   │   │       └── children/
│   │   │           ├── objects/      # Управление объектами (CRUD)
│   │   │           ├── media/        # Медиатека
│   │   │           └── wp-settings/  # Настройки WordPress
│   │   └── ...
│   └── libs/
│       ├── modules/
│       │   ├── auth/           # AuthService, guards, interceptor, DTO/RDO
│       │   ├── user/           # UserService, типы (UserRole)
│       │   ├── objects/        # ObjectsRequestService, формы, интерфейсы
│       │   ├── media/          # MediaRequestService, компоненты загрузки
│       │   └── wordpress/      # WordPressRequestService, типы соединения
│       └── shared/             # Утилиты, переиспользуемые сервисы, перечисления
```

### 9.3. Ключевые особенности

**Маршрутизация** — lazy-loaded страницы, два guard'а: `authGuard` (требует токен) и `guestGuard` (только для неавторизованных).

**JWT Interceptor** (`auth.interceptor.ts`):
- Добавляет `Authorization: Bearer` к каждому запросу.
- При 401 автоматически пытается обновить токен через `authService.refresh()`.
- При неудаче очищает localStorage и перенаправляет на `/login`.
- Предотвращает race condition при одновременных запросах (один refresh на несколько 401).

**UserService** — сигнал `_user`, инициализируется через `provideAppInitializer` из токена в localStorage. Декодирует JWT через `jwt-decode`.

**Роли на фронте** (`user-role.type.ts`): `'Administrator' | 'Editor' | 'Viewer'`.

**Taiga UI** — русская локализация (`TUI_RUSSIAN_LANGUAGE`), кастомные сообщения валидации.

### 9.4. Окружение

```bash
# frontend/.env
API_HOST=http://localhost:5000
```

Скрипт `scripts/set-env.script.ts` генерирует `src/environments/environment.ts` из `.env` при старте/сборке.

---

## 10. База данных

### 10.1. PostgreSQL

Две независимые базы данных:

```
Identity (база IdentityApi)
├── User
├── Role
├── RefreshToken
└── AuditLog (JSONB для Details)

Provider (база ProviderApi)
├── ProjectCard
├── ProjectCardDraft (JSONB)
├── ProjectMetrics (JSONB для JsonData)
├── ProjectImage
├── ProjectCategory
├── ProjectCategoryLink (M:N)
├── ProjectParticipant
├── ProjectParticipantLink (M:N)
├── AuditLog (JSONB для Details)
└── ApiConnectorConfig
```

### 10.2. Миграции (FluentMigrator)
Автоматически применяются при старте через `app.MigrateDatabase()`.

### 10.3. SQL инициализация (Docker)
```sql
CREATE DATABASE "Identity";
CREATE DATABASE "Provider";
```

### 10.4. EntityMapper
Type-safe генерация имен таблиц и колонок из C# record/property имен. Устраняет дублирование строковых литералов.

---

## 11. Объектное хранилище MinIO

| Параметр | Значение |
|----------|----------|
| Endpoint | `http://localhost:9000` |
| Bucket | `projects` |
| Public URL | `http://localhost:9000/projects` |

**Структура:** `projects/{projectId}/{guid}.{ext}`

**Типы файлов:** jpeg/jpg, png, svg, webp. **Макс. размер:** 5 MB.

---

## 12. Docker и Docker Compose

### 12.1. Сервисы

| Сервис | Образ | Порт (внешний) | Порт (внутренний) |
|--------|-------|----------------|-------------------|
| `postgres` | postgres:16 | 5435 | 5432 |
| `minio` | minio/minio:latest | 9000 / 9001 | 9000 / 9001 |
| `minio-init` | minio/mc:latest | — | — |
| `apigateway` | самосборка | 7060 | 8080 |
| `identityapi` | самосборка | 7061 | 8080 |
| `providerapi` | самосборка | 7062 | 8080 |

### 12.2. Переменные окружения (.env)

```
POSTGRES_USER=postgres
POSTGRES_PASSWORD=1
POSTGRES_PORT_EXTERNAL=5435
POSTGRES_IDENTITY_DB=Identity
POSTGRES_PROVIDER_DB=Provider
APIGATEWAY_PORT_EXTERNAL=7060
IDENTITY_API_PORT_EXTERNAL=7061
PROVIDER_API_PORT_EXTERNAL=7062
MINIO_ROOT_USER=minioadmin
MINIO_ROOT_PASSWORD=minioadmin123
MINIO_PORT_EXTERNAL=9000
MINIO_CONSOLE_PORT_EXTERNAL=9001
MINIO_BUCKET=projects
MINIO_ENDPOINT=http://minio:9000
API_CONNECTORS_ENABLED=false
AUDIT_LOG_RETENTION_DAYS=14
AUDIT_LOG_CHECK_INTERVAL_HOURS=4
```

### 12.3. Зависимости

```
postgres ← identityapi ← apigateway
postgres ← providerapi ← apigateway
           minio ← minio-init
```

---

## 13. Полная схема API Endpoints

### 13.1. IdentityApi (через Gateway: `/identity/...`)

```
POST   /identity/api/connect/register          # Регистрация
POST   /identity/api/connect/token              # Логин
POST   /identity/api/connect/token/revoke       # Выход
POST   /identity/api/connect/token/refresh      # Обновление токена
POST   /identity/api/connect/change-password    # Смена пароля

GET    /identity/user/list                      # Список пользователей
GET    /identity/user/{userId}                  # Пользователь по ID
DELETE /identity/user/{userId}                  # Удаление пользователя
PATCH  /identity/user/change-password           # Смена пароля (админ)

GET    /.well-known/jwks.json                   
GET    /.well-known/openid-configuration        

GET    /identity/api/audit/logs                 # Аудит (Admin)
```

### 13.2. ProviderApi (через Gateway: `/provider/...`)

```
# Проекты
GET    /provider/api/project/list               # Список проектов
GET    /provider/api/project/{projectId}        # Полный проект
POST   /provider/api/project/create             # Создать проект
POST   /provider/api/project/publish            # Опубликовать
PATCH  /provider/api/project/update/{projectId} # Обновить
DELETE /provider/api/project/delete/{projectId} # Удалить
GET    /provider/api/project/draft/{projectId}  # Черновик

# Изображения
POST   /provider/api/project/{projectId}/image  # Загрузить изображение
GET    /provider/api/project/{projectId}/images  # Список изображений
DELETE /provider/api/project/{projectId}/images/{imageId}  # Удалить

# Метрики (ТЭП)
GET    /provider/api/project/metrics/{projectId}
POST   /provider/api/project/metrics/{projectId}
PUT    /provider/api/project/metrics/{projectId}
DELETE /provider/api/project/metrics/{projectId}

# Категории
GET    /provider/api/category/list
GET    /provider/api/category/{categoryId}
POST   /provider/api/category/create
PATCH  /provider/api/category/{categoryId}
DELETE /provider/api/category/delete/{categoryId}

# Участники
GET    /provider/api/participant/list
GET    /provider/api/participant/{participantId}
POST   /provider/api/participant/create
PATCH  /provider/api/participant/{participantId}
DELETE /provider/api/participant/delete/{participantId}

# Экспорт
GET    /provider/api/export/projects/{projectId}/presentation  # PPTX
POST   /provider/api/export/projects/{projectId}/portfolio     # DOCX

# Аудит (Admin)
GET    /provider/api/audit/logs
```

### 13.3. Общие response-модели

**Успешный ответ:** DTO напрямую.

**Ошибка (400/500):**
```json
{ "error": "Текст ошибки", "type": "EntityNotFoundException", "traceId": "0HM...1" }
```

**Ошибки авторизации (IBaseErrorResponse):**
```json
{ "message": "CredentialsAreNotValid", "code": "40001" }
```

---

## 14. Модель безопасности

### 14.1. Аутентификация

1. `POST /api/connect/token` с email/паролем.
2. Сервер генерирует access-токен (RS256, 1 час) и refresh-токен (7 дней).
3. Все последующие запросы с `Authorization: Bearer {access_token}`.
4. По истечении access-токена — обновление через `/api/connect/token/refresh`.

### 14.2. JWT Payload (access token)

```json
{
  "sub": "guid-пользователя",
  "jti": "уникальный-id-токена",
  "email": "user@example.com",
  "given_name": "Иван",
  "family_name": "Иванов",
  "patronymic": "Иванович",
  "role": "Administrator",
  "iss": "http://localhost:5248/identity",
  "aud": "IdentityClient",
  "exp": 1718000000,
  "iat": 1717996400
}
```

### 14.3. RBAC (Role-Based Access Control)

Три роли: `Viewer`, `Editor`, `Administrator`. Доступ контролируется через `[Authorize(Roles = "...")]`. `PermissionList` и битовые маски удалены.

| Роль | Доступ |
|------|--------|
| `Viewer` | Все GET-запросы, экспорт |
| `Editor` | Viewer + создание, редактирование, публикация, загрузка изображений |
| `Administrator` | Editor + удаление, управление пользователями/категориями/участниками, аудит |

### 14.4. Межсервисная аутентификация

Внутренние HTTP-запросы (JWT Bearer handler для получения JWKS) помечаются `X-System-Request: proxy`.

### 14.5. JWKS Endpoint

`GET /.well-known/jwks.json` — публичный RSA-ключ для валидации JWT.

### 14.6. JWT-валидация (сервисы)

IdentityApi и ProviderApi валидируют токен через `IRsaKeyProvider.ValidationKey` (RSA-ключ из файловой системы), без OIDC Discovery (`Authority` не указывается). Issuer: `http://localhost:5248/identity`.

---

## Структура директорий проекта

```
project-data-hub/
├── api/                    # Бэкенд (.NET 10)
│   ├── Apps/
│   │   ├── ApiGateway/     # YARP Reverse Proxy
│   │   ├── IdentityApi/    # Auth & Users
│   │   └── ProviderApi/    # Portfolio & Projects
│   └── Libs/
│       ├── CoreLib/        # Shared: controllers, exceptions, audit, connectors
│       └── IdentityLib/    # Shared: JWT service, Argon2id
├── frontend/               # Angular 21
│   ├── src/
│   │   ├── app/            # Root component, config, routing, pages
│   │   └── libs/           # Modules (auth, user, objects, media, wordpress) + shared
│   ├── public/             # Static assets, icons
│   └── scripts/            # Env generation script
├── infra/                  # Database init SQL
├── docker-compose.yaml     # Backend services
└── README.md
```
