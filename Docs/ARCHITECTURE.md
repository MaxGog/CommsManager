# Архитектура проекта CommsManager

## Общая структура

```

CommsManager/
├── CommsManager.Core/          # Доменный слой (бизнес-логика)
├── CommsManager.Infrastructure/# Инфраструктурный слой (реализация)
├── CommsManager.Web/           # Web (планируется)
├── CommsManager.Maui/          # MAUI клиент (планируется)
└── CommsManager.Shared/        # Общий код (планируется)

```

---

## 1. Доменный слой (CommsManager.Core)

### 1.1 Основные сущности (Entities)

| Сущность | Наследование | Основные свойства | Коллекции | Бизнес-методы |
|----------|--------------|-------------------|-----------|---------------|
| **Order** | `BaseEntity` | `Title`, `Price (Money)`, `Status`, `Deadline`, `CustomerId`, `ArtistId`, `IsActive`, `CreatedDate` | `Attachments (OrderAttachment[])` | `UpdateStatus()`, `Cancel()`, `Complete()`, `ExtendDeadline()`, `UpdateOrderDetails()` |
| **Customer** | `BaseEntity` | `Name`, `CreatedDate`, `IsActive`, `CustomerPicture`, `Description`, `Communication` | `Phones (Phones[])`, `Emails (Email[])`, `SocialLinks (SocialLink[])`, `Orders (Order[])` | `AddPhone()`, `RemovePhone()`, `AddEmail()`, `Activate()`, `Deactivate()`, `SetCustomerPicture()` |
| **ArtistProfile** | `BaseEntity` | `Name`, `Description`, `ArtistPicture`, `ArtistBanner`, `CreatedDate` | `Phones (Phones[])`, `Emails (Email[])`, `SocialLinks (SocialLink[])`, `Commissions (Commission[])` | `SetArtistPicture()`, `SetArtistBanner()`, `AddCommission()`, `UpdateProfile()` |
| **BaseEntity** | - | `Id (Guid)`, `DomainEvents (IDomainEvent[])` | - | `AddDomainEvent()`, `ClearDomainEvents()` |

### 1.2 Модели данных (Models)

| Модель | Назначение | Свойства | Использование |
|--------|------------|----------|---------------|
| **Commission** | Услуги художника | `Name`, `Description`, `ViewAttachment (byte[][])`, `TypeCommission`, `Price` | Используется в `ArtistProfile.Commissions` |
| **Email** | Контактная информация | `EmailAdress`, `TypeEmail`, `Description` | Используется в `Customer.Emails` и `ArtistProfile.Emails` |
| **OrderAttachment** | Вложения к заказу | `Name`, `Attachment (byte[])`, `TypeAttachment (AttachmentType)`, `Description` | Используется в `Order.Attachments` |
| **Phones** | Телефонные номера | `NumberPhone`, `TypePhone`, `RegionNumber`, `Description` | Используется в `Customer.Phones` и `ArtistProfile.Phones` |
| **SocialLink** | Социальные сети | `Link`, `TypeLink (SocialPlatform)`, `IsActive`, `IsVisible` | Используется в `Customer.SocialLinks` и `ArtistProfile.SocialLinks` |

### 1.3 Value Objects (VO)

| VO | Назначение | Свойства | Использование |
|----|------------|----------|---------------|
| **Money** | Денежная сумма | `Amount (decimal)`, `Currency (string)`, `Symbol (string)` | `Order.Price` |

### 1.4 Интерфейсы (Interfaces)

| Интерфейс | Назначение | Методы |
|-----------|------------|--------|
| **IRepository<T>** | Базовый репозиторий | `GetByIdAsync()`, `GetAllAsync()`, `FindAsync()`, `AddAsync()`, `UpdateAsync()`, `DeleteAsync()`, `ExistsAsync()`, `CountAsync()` |
| **ICustomerRepository** | Репозиторий клиентов | `GetActiveCustomersAsync()`, `SearchByNameAsync()`, `HasOrdersAsync()`, `GetCustomersWithOrdersAsync()` |
| **IOrderRepository** | Репозиторий заказов | `GetByCustomerIdAsync()`, `GetByArtistIdAsync()`, `GetActiveOrdersAsync()`, `GetOverdueOrdersAsync()`, `GetOrdersByStatusAsync()`, `GetTotalRevenueByArtistAsync()` |
| **IArtistProfileRepository** | Репозиторий художников | `GetProfileWithCommissionsAsync()`, `SearchByNameAsync()`, `HasActiveCommissionsAsync()`, `GetPopularArtistsAsync()` |
| **IUnitOfWork** | Unit of Work паттерн | `Customers`, `Orders`, `ArtistProfiles`, `SaveChangesAsync()`, `BeginTransactionAsync()`, `CommitTransactionAsync()`, `RollbackTransactionAsync()` |
| **ICustomerService** | Сервис клиентов | `CreateCustomerAsync()`, `AddCustomerContactAsync()`, `GetCustomerOrdersAsync()`, `DeactivateInactiveCustomersAsync()` |
| **IOrderService** | Сервис заказов | `CreateOrderAsync()`, `UpdateOrderStatusAsync()`, `DuplicateOrderAsync()`, `GetOrdersDueSoonAsync()`, `ProcessOverdueOrdersAsync()` |
| **IFileStorageService** | Сервис файлов | `SaveAttachmentAsync()`, `GetAttachmentAsync()`, `DeleteAttachmentAsync()`, `SaveCustomerPictureAsync()`, `SaveArtistPictureAsync()` |
| **ICacheService** | Сервис кеширования | `GetAsync()`, `SetAsync()`, `RemoveAsync()`, `ExistsAsync()`, `GetOrSetAsync()` |

### 1.5 Enums (Перечисления)

| Enum | Используется в | Значения |
|------|----------------|----------|
| **OrderStatus** | `Order.Status` | `New`, `InProgress`, `Completed`, `Cancelled` |
| **AttachmentType** | `OrderAttachment.TypeAttachment` | `Image`, `Document`, `Audio`, `Video`, `Other` |
| **SocialPlatform** | `SocialLink.TypeLink` | `Telegram`, `VK`, `Instagram`, `Twitter`, `Facebook`, `YouTube`, `Other` |

### 1.6 Связи между сущностями

#### 1.6.1 Customer ↔ Order

- **Навигация**: `Customer.Orders` → `Order[]`
- **Внешний ключ**: `Order.CustomerId` (Guid)
- **Тип связи**: Один ко многим (1 Customer → N Orders)
- **Удаление**: `DeleteBehavior.Restrict`

#### 1.6.2 ArtistProfile ↔ Order

- **Навигация**: Нет прямой навигации в ArtistProfile
- **Внешний ключ**: `Order.ArtistId` (Guid)
- **Тип связи**: Один ко многим (1 Artist → N Orders)

#### 1.6.3 ArtistProfile ↔ Commission

- **Навигация**: `ArtistProfile.Commissions` → `Commission[]`
- **Внешний ключ**: `Commission.ArtistProfileId` (Guid)
- **Тип связи**: Один ко многим (1 Artist → N Commissions)
- **Удаление**: `DeleteBehavior.Cascade`

---

## 2. Инфраструктурный слой (CommsManager.Infrastructure)

### 2.1 Конфигурации Entity Framework Core

| Конфигурация | Назначение | Основные настройки |
|--------------|------------|-------------------|
| **ApplicationDbContext** | Основной DbContext | `DbSet<Customer>`, `DbSet<Order>`, `DbSet<ArtistProfile>`, `DbSet<Commission>` |
| **CustomerConfiguration** | Конфигурация Customer | Owned Types: `Phones`, `Emails`, `SocialLinks`. Индексы: `Name`, `IsActive`, `CreatedDate` |
| **OrderConfiguration** | Конфигурация Order | Owned Types: `Attachments`. Конвертация `Money`. Индексы: `CustomerId`, `ArtistId`, `Status`, `Deadline` |
| **ArtistProfileConfiguration** | Конфигурация ArtistProfile | Owned Types: `Phones`, `Emails`, `SocialLinks`, `Commissions`. JSON-сериализация `ViewAttachment` |
| **CommissionConfiguration** | Конфигурация Commission | JSON-сериализация `ViewAttachment`. Связь с `ArtistProfile`. Индексы: `Name`, `TypeCommission`, `ArtistProfileId` |
| **DesignTimeDbContextFactory** | Фабрика для миграций | Создание DbContext для времени разработки |

### 2.2 Репозитории (Реализации)

| Репозиторий | Реализует интерфейс | Особенности |
|-------------|---------------------|-------------|
| **CustomerRepository** | `ICustomerRepository` | Включает связанные данные (`Phones`, `Emails`, `Orders`). Фильтрация по `IsActive` |
| **OrderRepository** | `IOrderRepository` | Включает `Attachments`. Специальные запросы: `GetOverdueOrdersAsync()`, `GetTotalRevenueByArtistAsync()` |
| **ArtistProfileRepository** | `IArtistProfileRepository` | Включает `Commissions`. Метод `GetPopularArtistsAsync()` с сортировкой по количеству комиссий |
| **UnitOfWork** | `IUnitOfWork` | Управление транзакциями. Паттерн Unit of Work для согласованности данных |

### 2.3 Настройки базы данных

#### 2.3.1 Строка подключения

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CommsManagerDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  }
}
```

#### 2.3.2 Типы хранения данных

| Тип данных C# | Тип SQL Server | Особенности |
|---------------|----------------|-------------|
| `Guid` | `uniqueidentifier` | Первичные ключи |
| `DateTime` | `datetime2` | Дата и время с высокой точностью |
| `byte[]` | `varbinary(max)` | Изображения и файлы |
| `string` | `nvarchar(max)` или `nvarchar(N)` | Строки с указанием максимальной длины |
| `enum` | `nvarchar(50)` | Хранение как строки |
| `List<byte[]>` | `nvarchar(max)` | JSON-сериализация для сложных структур |

#### 2.3.3 Индексы (для производительности)

| Таблица | Индексируемые поля | Тип индекса |
|---------|-------------------|-------------|
| **Customers** | `Name`, `IsActive`, `CreatedDate`, `(IsActive, Name)` | Неуникальный |
| **Orders** | `CustomerId`, `ArtistId`, `Status`, `Deadline`, `IsActive` | Неуникальный |
| **ArtistProfiles** | `Name`, `CreatedDate` | Неуникальный |
| **Commissions** | `Name`, `TypeCommission`, `ArtistProfileId` | Неуникальный |

### 2.4 Dependency Injection

#### 2.4.1 Регистрация сервисов

```csharp
// В методе AddInfrastructure
services.AddDbContext<ApplicationDbContext>(options => ...);
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<ICustomerRepository, CustomerRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IArtistProfileRepository, ArtistProfileRepository>();
```

#### 2.4.2 Время жизни объектов

| Сервис | Lifetime | Причина |
|--------|----------|---------|
| `ApplicationDbContext` | Scoped | Один контекст на HTTP-запрос |
| Репозитории | Scoped | Согласованность с DbContext |
| `IUnitOfWork` | Scoped | Управление транзакциями в рамках запроса |

---

## 3. Принципы проектирования

### 3.1 Используемые паттерны

| Паттерн | Реализация | Преимущества |
|---------|------------|--------------|
| **Repository** | `IRepository<T>`, `CustomerRepository` | Абстракция доступа к данным, легкое тестирование |
| **Unit of Work** | `IUnitOfWork`, `UnitOfWork` | Согласованность транзакций, отслеживание изменений |
| **Dependency Injection** | Внедрение через конструктор | Гибкость, тестируемость, слабая связанность |
| **Value Object** | `Money` | Неизменяемость, семантическое значение |
| **Owned Types** | `Phones`, `Emails` в `Customer` | Группировка связанных данных в таблице владельца |
| **Domain Events** | `IDomainEvent` в `BaseEntity` | Реакция на изменения в домене |

### 3.2 Принципы SOLID

| Принцип | Реализация |
|---------|------------|
| **Single Responsibility** | Каждый класс имеет одну причину для изменения |
| **Open/Closed** | Расширение через новые реализации интерфейсов |
| **Liskov Substitution** | Наследование от `BaseEntity`, реализация `IRepository<T>` |
| **Interface Segregation** | Специализированные интерфейсы репозиториев |
| **Dependency Inversion** | Зависимость от абстракций (`IRepository`), а не от реализаций |

### 3.3 Принципы Domain-Driven Design (DDD)

| Концепция DDD | Реализация |
|---------------|------------|
| **Aggregate Root** | `Customer`, `Order`, `ArtistProfile` как корни агрегатов |
| **Value Objects** | `Money` как иммутабельный объект |
| **Entities** | Сущности с идентификатором (`Id`) |
| **Repositories** | Абстракции для персистентности агрегатов |
| **Domain Events** | События в `BaseEntity.DomainEvents` |

---

## 4. Миграции базы данных

### 4.1 Создание миграций

```bash
# Создание новой миграции
dotnet ef migrations add MigrationName --project CommsManager.Infrastructure

# Обновление базы данных
dotnet ef database update --project CommsManager.Infrastructure

# Откат миграции
dotnet ef migrations remove --project CommsManager.Infrastructure
```

### 4.2 Существующие миграции

| Миграция | Изменения |
|----------|-----------|
| `InitialCreate` | Создание таблиц: Customers, Orders, ArtistProfiles, Commissions |
| `FixCommissionEntity` | Исправление конфигурации Commission, добавление первичного ключа |

---