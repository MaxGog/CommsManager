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