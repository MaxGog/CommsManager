using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CommsManager.Models;
using CommsManager.Data;

namespace CommsManager.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DashboardModel> _logger;

        public DashboardModel(ApplicationDbContext context, ILogger<DashboardModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public int TotalClients { get; set; }
        public int TotalServices { get; set; }
        public int ActiveOrders { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Profit => TotalIncome - TotalExpenses;

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                TotalClients = await _context.Clients
                    .Where(c => c.UserId == userId)
                    .CountAsync();

                TotalServices = await _context.Services
                    .Where(s => s.UserId == userId && s.IsActive)
                    .CountAsync();

                ActiveOrders = await _context.Orders
                    .Where(o => o.UserId == userId &&
                               o.Status != OrderStatus.Completed &&
                               o.Status != OrderStatus.Cancelled)
                    .CountAsync();

                TotalIncome = await _context.Incomes
                    .Where(i => i.UserId == userId)
                    .SumAsync(i => i.Amount);

                TotalExpenses = await _context.Expenses
                    .Where(e => e.UserId == userId)
                    .SumAsync(e => e.Amount);
            }
        }
    }
}