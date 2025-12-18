namespace CommsManager.Core.Enums;

public enum OrderStatus
{
    Draft = 0,          // Черновик
    New = 1,            // Новый
    Confirmed = 2,      // Подтвержден
    InProgress = 3,     // В работе
    ReadyForReview = 4, // Готов к проверке
    Completed = 5,      // Завершен
    Cancelled = 6,      // Отменен
    OnHold = 7         // На паузе
}

public enum PaymentStatus
{
    Pending = 0,
    PartiallyPaid = 1,
    Paid = 2,
    Overdue = 3,
    Refunded = 4
}

public enum SocialPlatform
{
    Instagram,
    Facebook,
    TikTok,
    Twitter,
    YouTube,
    Pinterest,
    LinkedIn,
    VKontakte,
    Telegram,
    WhatsApp,
    Other
}

public enum AttachmentType
{
    Image,
    Document,
    Video,
    Audio,
    Other
}