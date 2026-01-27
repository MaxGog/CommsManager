using CommsManager.Core.Enums;
using CommsManager.Core.ValueObjects;
using CommsManager.Core.Models;

namespace CommsManager.Core.Entities;

public class Order : BaseEntity
{
    private string _title;
    private string? _description;
    private Money _price;
    private DateTime _deadline;
    private OrderStatus _status;
    private bool _isActive;
    private readonly List<OrderAttachment> _attachments = [];

    public Order(string title, Money price, Guid customerId, Guid artistId, DateTime deadline)
    {
        ArgumentNullException.ThrowIfNull(price);
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Имя заказа не может быть пустым.", nameof(title));
        if (customerId == Guid.Empty)
            throw new ArgumentException("ID заказчика не может быть пустым.", nameof(customerId));
        if (artistId == Guid.Empty)
            throw new ArgumentException("ID артиста не может быть пустым.", nameof(artistId));
        if (deadline <= DateTime.UtcNow)
            throw new ArgumentException("Дедлайн не может быть в прошлом или в настоящем.", nameof(deadline));

        Id = Guid.NewGuid();
        _title = title;
        _price = price;
        CustomerId = customerId;
        ArtistId = artistId;
        _deadline = deadline;
        _status = OrderStatus.New;
        CreatedDate = DateTime.UtcNow;
        _isActive = true;
    }

    public string Title
    {
        get => _title;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Имя заказа не может быть пустым.", nameof(value));
            _title = value;
        }
    }

    public string? Description
    {
        get => _description;
        private set => _description = value;
    }

    public Money Price
    {
        get => _price;
        private set
        {
            ArgumentNullException.ThrowIfNull(value);
            _price = value;
        }
    }

    public DateTime CreatedDate { get; private set; }

    public DateTime Deadline
    {
        get => _deadline;
        private set
        {
            if (value <= DateTime.UtcNow)
                throw new ArgumentException("Дедлайн не может быть в настоящем или прошлом.", nameof(value));
            _deadline = value;
        }
    }

    public OrderStatus Status
    {
        get => _status;
        private set => _status = value;
    }

    public bool IsActive
    {
        get => _isActive;
        private set => _isActive = value;
    }

    public Guid CustomerId { get; private set; }
    public Guid ArtistId { get; private set; }

    public IReadOnlyCollection<OrderAttachment> Attachments => _attachments.AsReadOnly();

    public void AddAttachment(string fileName, byte[] file, AttachmentType contentType)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Имя файла нек может быть пустым.", nameof(fileName));

        ArgumentNullException.ThrowIfNull(file);
        if (file.Length == 0)
            throw new ArgumentException("Данные файла не могут быть пустыми.", nameof(file));

        _attachments.Add(new OrderAttachment
        {
            Name = fileName,
            Attachment = file,
            TypeAttachment = contentType
        });
    }

    public bool RemoveAttachment(OrderAttachment attachment) => _attachments.Remove(attachment);

    public void ClearAttachments() => _attachments.Clear();

    public void UpdateTitle(string title)
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Невозможно обновить название завершенного или отмененного заказа.");

        Title = title;
    }

    public void UpdateDescription(string? description)
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Невозможно обновить описание завершенного или отмененного заказа.");

        Description = description;
    }

    public void UpdateStatus(OrderStatus newStatus)
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Невозможно изменить статус выполненного или отмененного заказа.");

        if (newStatus == OrderStatus.Completed || newStatus == OrderStatus.Cancelled) IsActive = false;
        Status = newStatus;
    }

    public void UpdatePrice(Money newPrice)
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Невозможно изменить цену выполненного заказа.");
        ArgumentNullException.ThrowIfNull(newPrice);

        Price = newPrice;
    }

    public void UpdateDeadline(DateTime newDeadline)
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Невозможно обновить крайний срок выполнения или отмены заказа.");
        if (newDeadline <= DateTime.UtcNow)
            throw new ArgumentException("Крайний срок должен быть в будущем.", nameof(newDeadline));

        Deadline = newDeadline;
    }

    public void Cancel(string? reason = null)
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Невозможно отменить выполненный заказ.");

        Status = OrderStatus.Cancelled;
        IsActive = false;
    }

    public void Complete()
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Не удается выполнить отмененный заказ.");

        Status = OrderStatus.Completed;
        IsActive = false;
    }

    public void Activate()
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Не удается активировать завершенный или отмененный заказ.");

        IsActive = true;
    }

    public void Deactivate() => IsActive = false;

    public bool IsOverdue => DateTime.UtcNow > Deadline && Status != OrderStatus.Completed && Status != OrderStatus.Cancelled;

    public TimeSpan TimeUntilDeadline => Deadline - DateTime.UtcNow;

    public void ExtendDeadline(TimeSpan extension)
    {
        if (extension <= TimeSpan.Zero)
            throw new ArgumentException("Расширение должно быть положительным.", nameof(extension));

        UpdateDeadline(Deadline.Add(extension));
    }

    public void UpdateOrderDetails(string title, string? description, Money price, DateTime deadline)
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Невозможно обновить информацию о завершенном или отмененном заказе.");

        UpdateTitle(title);
        UpdateDescription(description);
        UpdatePrice(price);
        UpdateDeadline(deadline);
    }
}