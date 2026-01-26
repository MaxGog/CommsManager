# Архитектура сущностей CommsManager.Core

## Основные сущности (Entities)

| Сущность | Наследование | Основные свойства | Коллекции | Связи |
|----------|--------------|-------------------|-----------|--------|
| **Order** | `BaseEntity` | `Title`, `Price (Money)`, `Status`, `Deadline`, `CustomerId`, `ArtistId`, `IsActive` | `Attachments (OrderAttachment[])` | Связан с **Customer** (CustomerId) и **ArtistProfile** (ArtistId) |
| **Customer** | `BaseEntity` | `Name`, `CreatedDate`, `IsActive`, `CustomerPicture` | `Phones (Phones[])`, `Emails (Email[])`, `SocialLinks (SocialLink[])`, `Orders (Order[])` | Имеет много **Order** (через коллекцию Orders) |
| **ArtistProfile** | `BaseEntity` | `Name`, `Description`, `ArtistPicture`, `ArtistBanner` | `Phones (Phones[])`, `Emails (Email[])`, `SocialLinks (SocialLink[])`, `Commissions (Commission[])` | Связан с **Order** через ArtistId |
| **BaseEntity** | - | `Id`, `DomainEvents` | - | Базовый класс для всех сущностей |

## Модели данных (Models)

| Модель | Используется в | Основные свойства |
|--------|----------------|-------------------|
| **Commission** | `ArtistProfile` | `Name`, `Description`, `ViewAttachment`, `TypeCommission`, `Price` |
| **Email** | `Customer`, `ArtistProfile` | `EmailAdress`, `TypeEmail`, `Description` |
| **OrderAttachment** | `Order` | `Name`, `Attachment`, `TypeAttachment (AttachmentType)`, `Description` |
| **Phones** | `Customer`, `ArtistProfile` | `NumberPhone`, `TypePhone`, `RegionNumber`, `Description` |
| **SocialLink** | `Customer`, `ArtistProfile` | `Link`, `TypeLink (SocialPlatform)`, `IsActive`, `IsVisible` |

## Value Objects (VO)

| VO | Используется в | Назначение |
|----|----------------|------------|
| **Money** | `Order` (Price) | Представляет денежную сумму |

## Enums (Перечисления)

| Enum | Используется в | Значения |
|------|----------------|----------|
| **OrderStatus** | `Order` (Status) | New, InProgress, Completed, Cancelled и др. |
| **AttachmentType** | `OrderAttachment` (TypeAttachment) | Типы вложений (Image, Document, Audio и др.) |
| **SocialPlatform** | `SocialLink` (TypeLink) | Платформы социальных сетей (Telegram, VK, Instagram и др.) |

## Связи между сущностями

### 1. **Customer ↔ Order**

- **Customer** имеет коллекцию `Orders`
- **Order** содержит `CustomerId` (FK)
- **Связь**: Один ко многим (1 Customer → N Orders)

### 2. **ArtistProfile ↔ Order**

- **Order** содержит `ArtistId` (FK)
- **ArtistProfile** не имеет прямой коллекции Orders (но можно добавить при необходимости)
- **Связь**: Один ко многим (1 Artist → N Orders)

### 3. **Вспомогательные модели**

- **Email**, **Phones**, **SocialLink** используются и в `Customer`, и в `ArtistProfile`
- **OrderAttachment** используется только в `Order`
- **Commission** используется только в `ArtistProfile`