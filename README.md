# CTH API

Серверная часть программного комплекса для подготовки к ЦТ и ЦЭ.  
REST API на ASP.NET Core 9 с PostgreSQL.

## Стек
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- JWT аутентификация
- Swagger/OpenAPI

## Основной функционал (API)
- Регистрация / логин (JWT)
- Управление тестами (список, фильтрация)
- Генерация смешанных тестов по темам и сложности
- Прохождение теста и сохранение попыток
- Статистика и аналитика по результатам
- Рекомендации по повторению тем
- Роли: ученик / преподаватель
- Привязка учеников к преподавателю
- Назначение тестов ученикам

## Запуск

```bash
git clone https://github.com/Dmitruk-Bohdan/cth_api.git
cd cth_api/CTHelper.Presentation
dotnet restore
dotnet ef database update
dotnet run
```
Swagger: http://localhost:5000/swagger

## Структура проекта

```bash
CTHelper.Domain         # сущности
CTHelper.Application   # бизнес-логика, сервисы
CTHelper.Persistence   # EF Core, контекст, миграции
CTHelper.Presentation  # контроллеры API
```


