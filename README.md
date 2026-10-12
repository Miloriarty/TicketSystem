# PracticeProject - API Управления Заявками (Ticketing System)

Бэкенд на базе **ASP.NET Core Web API** и **Entity Framework Core**, предназначенное для управления заявками (tickets), сотрудниками, ролями и категориями

## Технологический стек
- **Platform:** .NET 10
- **Framework:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Database:** PostgreSQL

## Структура проекта
- `Controllers/` - REST API контроллеры (`Categories`, `Employees`, `Tickets`, `Roles`, остальные пока не работают)
- `Data/` - Контекст базы данных (`AppDbContext.cs`) с настройками связей через Fluent API.
- `Dto/` - Модели передачи данных (DTO) и мапперы для сущностей.
- `Entity` - Модели базы данных.
- `Migrations/` - Миграции Entity Framework Core.

## Системные требования
- [.NET SDK](https://dotnet.microsoft.com/download) (версии 8.0 или выше)
- Любая IDE для разработки на .NET

## Установка и локальный запуск
1. Клонируйте репозиторий / перейдите в папку проекта:
   ```bash  
   cd PracticeProject
   ```

2. Восстановите зависимости:
    ```bash
    dotnet restore
    ```

Настройте подключение к базе данных:
- Проверьте строку подключения в файле appsettings.json или appsettings.Development.json.
- Примените миграции (создание базы данных):
    ```bash
    dotnet ef database update
    ```
- Запуск:
    ```bash
    dotnet run
    ```

Приложение запустится локально (http://localhost:5042)

## Основные эндпоинты API
- `/api/categories` - Управление категориями
- `/api/employees` - Управление сотрудниками
- `/api/tickets` - Управление заявками
- `/api/roles` - Управление ролями
