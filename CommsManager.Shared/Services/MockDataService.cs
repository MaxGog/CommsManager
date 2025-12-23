using CommsManager.Shared.Models;

namespace CommsManager.Shared.Services;

public class MockDataService
{
    private List<Order> _orders = new();
    private List<PortfolioItem> _portfolio = new();

    public MockDataService()
    {
        _orders = new List<Order>
        {
            new Order
            {
                Id = Guid.NewGuid(),
                Title = "Портрет маслом",
                Description = "Портрет по фотографии 30x40 см",
                CustomerName = "Анна Иванова",
                CustomerEmail = "anna@example.com",
                Price = 5000,
                Deadline = DateTime.Now.AddDays(14),
                Status = OrderStatus.InProgress
            },
            new Order
            {
                Id = Guid.NewGuid(),
                Title = "Иллюстрация для книги",
                Description = "Цветная иллюстрация в стиле фэнтези",
                CustomerName = "Издательство 'Весна'",
                CustomerEmail = "info@vesna.ru",
                Price = 12000,
                Deadline = DateTime.Now.AddDays(30),
                Status = OrderStatus.New
            }
        };

        _portfolio = new List<PortfolioItem>
        {
            new PortfolioItem
            {
                Id = Guid.NewGuid(),
                Title = "Портрет кота",
                Description = "Акварель, 20x30 см",
                ImageUrl = "https://via.placeholder.com/300x200",
                Price = 3000,
                Tags = new List<string> { "акварель", "портрет", "животные" }
            }
        };
    }

    public List<Order> GetOrders() => _orders;
    public Order? GetOrder(Guid id) => _orders.FirstOrDefault(o => o.Id == id);
    public void AddOrder(Order order) => _orders.Add(order);
    public void UpdateOrder(Order order)
    {
        var index = _orders.FindIndex(o => o.Id == order.Id);
        if (index != -1) _orders[index] = order;
    }

    public List<PortfolioItem> GetPortfolio() => _portfolio;
    public void AddPortfolioItem(PortfolioItem item) => _portfolio.Add(item);

    public decimal GetTotalRevenue() =>
        _orders.Where(o => o.Status == OrderStatus.Completed).Sum(o => o.Price);

    public int GetActiveOrdersCount() =>
        _orders.Count(o => o.Status == OrderStatus.New || o.Status == OrderStatus.InProgress);
}