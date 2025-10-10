# Сервис Авторизации для ShoppingList

## Предварительная настройка

Зависимости: Docker, Docker Compose

### Настройка репозитория

```bash
git clone https://github.com/IKIT-Command-Project-VAD/auth-service.git

cd auth-service
```

### Настройка Docker

#### Windows

`your_password` заменить на придуманный вами пароль.
```pwsh
docker volume create shoppinglist-auth-postgres-data

docker run --name shoppingList-authService-postgres `
  -e POSTGRES_USER=postgres `
  -e POSTGRES_PASSWORD=your_password `
  -e POSTGRES_DB=shoppinglist_auth `
  -v shoppinglist-authService-postgres-data:/var/lib/postgresql/data `
  -p 9100:5432 `
  -d postgres:latest
```

#### MacOS
`your_password` заменить на придуманный вами пароль.
```bash
docker volume create shoppinglist-auth-postgres-data

docker run --name shoppingList-authService-postgres \
  -e POSTGRES_USER=your_username \
  -e POSTGRES_PASSWORD=your_password \
  -e POSTGRES_DB=shoppinglist_auth \
  -v shoppinglist-authService-postgres-data:/var/lib/postgresql/data \
  -p 9100:5432 \
  -d postgres:latest
```

### Настройка проекта

1. Заходим в appsettings и все поля, у которых нету значений, добавляем через `user-secrets`

### Запуск проекта

Запускаем проект пока-что только через http или https
