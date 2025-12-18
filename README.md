# CommsManager 🎨📱

**Профессиональная система управления заказами для творческих профессионалов**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![MAUI](https://img.shields.io/badge/MAUI-Blazor_Hybrid-0078D4?logo=xamarin)](https://learn.microsoft.com/dotnet/maui/)
[![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-5C2D91?logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)
[![Code Style](https://img.shields.io/badge/code_style-C%23_12-239120?logo=csharp)](https://learn.microsoft.com/dotnet/csharp/)

## 🌟 О проекте

CommsManager — это многоплатформенное решение для художников, крафтеров, фотографов и других творческих профессионалов, которое помогает управлять заказами, клиентами и демонстрировать работы через персонализированную страницу-визитку.

**Ключевые возможности:**

- 📋 Управление заказами с трекингом статусов
- 👥 База клиентов и история взаимодействий
- 🖼️ Портфолио с примерами работ
- 💰 Гибкая система прайс-листов
- 🌐 Личная страница-визитка (а-ля Linktree)
- 📱 QR-код для быстрого доступа к профилю
- 🔄 Онлайн/офлайн синхронизация
- 📊 Аналитика и отчетность

## 🚀 Быстрый старт

### Предварительные требования

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) или новее
- [Visual Studio Code](https://code.visualstudio.com) с компонентами:
  - .NET Multi-platform App UI development
  - ASP.NET and web development
- [SQL Server 2022](https://www.microsoft.com/sql-server)
- [Git](https://git-scm.com/)

### Установка

1. **Клонируйте репозиторий**

```bash
git clone https://github.com/MaxGog/CommsManager.git
cd CommsManager
```

1. **Восстановите зависимости**

```bash
dotnet restore
```

1. **Настройте базу данных**

```bash
cd src/CommsManager.Infrastructure
dotnet ef database update --startup-project ../CommsManager.Web
```

1. **Запустите веб-приложение**

```bash
cd src/CommsManager.Web
dotnet run
```

1. **Запустите мобильное приложение**
   - Выберите проект `CommsManager.Maui`
   - Выберите целевую платформу (Android/iOS/Windows)

### Альтернативный запуск через Docker

```bash
# Соберите образ
docker build -t commsmanager -f Dockerfile .

# Запустите контейнер
docker run -p 8080:80 --name commsmanager-app commsmanager
```

## 📁 Структура решения

```
CommsManager/
├── 📁 CommsManager.Core/           # Ядро приложения
│   ├── Entities/                   # Доменные сущности
│   ├── Interfaces/                 # Абстракции
│   ├── Services/                   # Доменные сервисы
│   ├── Specifications/             # Спецификации
│   ├── ValueObjects/               # Value Objects
│   └── Events/                     # Доменные события
├── 📁 CommsManager.Infrastructure/ # Репозитории, БД
├── 📁 CommsManager.Application/    # Сценарии использования
├── 📁 CommsManager.Web/            # Web-приложение (Blazor WASM)
├── 📁 CommsManager.Maui/           # Мобильное приложение
├── 📁 CommsManager.API/            # Web API (опционально)
├── 📁 tests/                       # Тесты
│   ├── CommsManager.Core.Tests/
│   ├── CommsManager.Application.Tests/
│   └── CommsManager.Integration.Tests/
├── 📁 docs/                        # Документация
├── 📁 scripts/                     # Скрипты сборки/развертывания
├── 📁 assets/                      # Изображения, иконки
├── 📄 LICENSE                      # Лицензия
├── 📄 README.md                    # Этот файл
├── 📄 .gitignore                   # Git игнорирование
├── 📄 Directory.Build.props        # Общие настройки сборки
└── 📄 CommsManager.sln             # Файл решения
```

## 🛠️ Технологический стек

| Технология | Назначение | Версия |
|------------|------------|---------|
| **.NET 10** | Основная платформа | 10.0+ |
| **MAUI Blazor Hybrid** | Мобильные приложения | 10.0 |
| **Blazor WebAssembly** | Веб-приложение | 10.0 |
| **Entity Framework Core** | ORM и работа с БД | 10.0 |
| **SQL Server** | Базы данных | 2022 |

## 🚀 Развертывание

### Веб-приложение (Azure)

```bash
# Публикация
dotnet publish src/CommsManager.Web -c Release -o ./publish

# Развертывание через Azure CLI
az webapp deployment source config-zip \
    --resource-group commsmanager-rg \
    --name commsmanager-app \
    --src ./publish.zip
```

### Мобильное приложение

- **Android**: Сборка через Visual Studio -> Publish -> Create Android Bundle
- **iOS**: Требуется Mac с Xcode для сборки
- **Windows**: Публикация в Microsoft Store

## 🙏 Благодарности

- [.NET Foundation](https://dotnetfoundation.org/) за прекрасную платформу
- [Microsoft MAUI Team](https://github.com/dotnet/maui) за фреймворк
- Сообществу Blazor за вдохновение

## 📞 Контакты

**Автор:** [Гоглов Максим Алексеевич]  
**Email:** [max.gog2005@outlook.com]  
**Telegram:** [@maxgog]  
**Issues:** [GitHub Issues](https://github.com/MaxGog/CommsManager/issues)

---

⭐ **Если проект вам понравился, поставьте звезду на GitHub!** ⭐
