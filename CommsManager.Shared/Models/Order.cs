using System.ComponentModel.DataAnnotations;

namespace CommsManager.Shared.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Название обязательно")]
    [StringLength(100, ErrorMessage = "Максимум 100 символов")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Описание обязательно")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Клиент обязателен")]
    public string CustomerName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Некорректный email")]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Цена обязательна")]
    [Range(1, 1000000, ErrorMessage = "Цена от 1 до 1 000 000")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Срок выполнения обязателен")]
    public DateTime Deadline { get; set; } = DateTime.Now.AddDays(7);

    public OrderStatus Status { get; set; } = OrderStatus.New;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? CompletedDate { get; set; }
}

public enum OrderStatus
{
    New,
    InProgress,
    ReadyForReview,
    Completed,
    Cancelled
}