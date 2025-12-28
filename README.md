# Candidate Validation MVP (монолит, .NET 9, РФ)

MVP для проверки кандидатов: регистрация соискателей, загрузка выписки (PDF) из Госуслуг/ЭТК, разбор опыта, расчёт стажа и просмотр списка кандидатов HR.

## Быстрый старт

### Запуск через Docker

```bash
docker compose up --build
```

- Приложение: `http://localhost:8080`
- Swagger UI: `http://localhost:8080/swagger`
- Логин/регистрация (простые HTML):
  - `http://localhost:8080/login.html`
  - `http://localhost:8080/register.html`

### Локальный запуск (без Docker)

```bash
cd goida
export ConnectionStrings__DefaultConnection='Host=localhost;Port=5432;Database=goida;Username=postgres;Password=postgres'
export Jwt__Key='dev_super_secret_key_change_me'
export Jwt__Issuer='goida'
export Jwt__Audience='goida'
export FileStorage__RootPath='/appdata'
export HR_EMAIL='hr@example.com'
export HR_PASSWORD='ChangeMe123!'

dotnet run
```

## Тесты

```bash
dotnet test
```

## Swagger

1. Откройте `http://localhost:8080/swagger`.
2. Вызовите `POST /api/auth/login` и получите JWT.
3. Нажмите **Authorize** и введите: `Bearer {token}`.

## Создание HR пользователя

При старте приложения создаётся HR пользователь из переменных окружения:
- `HR_EMAIL` (по умолчанию `hr@example.com`)
- `HR_PASSWORD` (по умолчанию `ChangeMe123!`)

## Основные API

- `POST /api/auth/register` — регистрация соискателя.
- `POST /api/auth/login` — логин и выдача JWT.
- `GET /api/me` — профиль соискателя.
- `PUT /api/me` — обновление профиля/стека.
- `POST /api/me/upload-extract` — загрузка PDF выписки.
- `GET /api/me/experiences` — список опыта.
- `GET /api/hr/candidates` — список кандидатов (HR).
- `GET /api/hr/candidates/{id}` — детали кандидата.
- `GET /api/files/{fileId}` — скачивание выписки.

## Примеры curl

### Регистрация соискателя
```bash
curl -X POST http://localhost:8080/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"applicant1@example.com","password":"Password123!","displayName":"Анна Сидорова"}'
```

### Логин
```bash
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"applicant1@example.com","password":"Password123!"}'
```

### Загрузка PDF
```bash
curl -X POST http://localhost:8080/api/me/upload-extract \
  -H "Authorization: Bearer <TOKEN>" \
  -F "file=@/path/to/extract.pdf;type=application/pdf"
```

### Список кандидатов (HR)
```bash
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"hr@example.com","password":"ChangeMe123!"}'
```

```bash
curl -X GET "http://localhost:8080/api/hr/candidates?sort=experienceYears&filter=c%23" \
  -H "Authorization: Bearer <HR_TOKEN>"
```

## Переменные окружения

| Переменная | Значение | Описание |
| --- | --- | --- |
| `ConnectionStrings__DefaultConnection` | Host=... | строка подключения к Postgres |
| `Jwt__Issuer` | goida | issuer для JWT |
| `Jwt__Audience` | goida | audience для JWT |
| `Jwt__Key` | secret | ключ подписи |
| `Jwt__ExpiresMinutes` | 120 | срок жизни JWT |
| `FileStorage__RootPath` | /appdata | путь к volume для PDF |
| `HR_EMAIL` | hr@example.com | логин HR |
| `HR_PASSWORD` | ChangeMe123! | пароль HR |
| `Database__ApplyMigrations` | true | применять миграции при старте |

