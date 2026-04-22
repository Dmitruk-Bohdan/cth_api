# CTH API

Серверная часть программного комплекса для подготовки к ЦТ и ЦЭ.  
REST API на ASP.NET Core 8 с PostgreSQL.

## Стек
- .NET 8
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
cd cth_api
docker-compose up --build
```
Swagger: http://localhost:5000/swagger

## Структура проекта

```bash
CTHelper.Domain         # сущности
CTHelper.Application   # бизнес-логика, сервисы
CTHelper.Persistence   # EF Core, контекст, миграции
CTHelper.Presentation  # контроллеры API
```


