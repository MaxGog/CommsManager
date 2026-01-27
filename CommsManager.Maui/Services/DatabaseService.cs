using CommsManager.Maui.Data.Models;
using CommsManager.Maui.Models;
using SQLite;
using System.Text.Json;

namespace CommsManager.Maui.Services;

public class DatabaseService : IAsyncDisposable
{
    private SQLiteAsyncConnection? _database;
    private readonly string _databasePath;
    private readonly SemaphoreSlim _databaseLock = new(1, 1);
    private bool _isInitialized = false;

    public DatabaseService()
    {
        _databasePath = Path.Combine(FileSystem.AppDataDirectory, "commsmanager.db3");
    }

    public async Task<SQLiteAsyncConnection> GetDatabaseAsync()
    {
        if (_database != null)
            return _database;

        await _databaseLock.WaitAsync();

        try
        {
            if (_database != null)
                return _database;

            _database = new SQLiteAsyncConnection(_databasePath);

            if (!_isInitialized)
            {
                await InitializeDatabaseAsync();
                _isInitialized = true;
            }

            return _database;
        }
        finally
        {
            _databaseLock.Release();
        }
    }

    private async Task InitializeDatabaseAsync()
    {
        await _database!.CreateTableAsync<LocalCustomer>();
        await _database.CreateTableAsync<LocalArtistProfile>();
        await _database.CreateTableAsync<LocalOrder>();
        await _database.CreateTableAsync<LocalCommission>();
        await _database.CreateTableAsync<LocalPhone>();
        await _database.CreateTableAsync<LocalEmail>();
        await _database.CreateTableAsync<LocalSocialLink>();

        await CreateIndexesAsync();

#if DEBUG
        await SeedTestDataAsync();
#endif
    }

    private async Task CreateIndexesAsync()
    {
        try
        {
            await _database!.ExecuteAsync(@"
                -- Индексы для Customer
                CREATE INDEX IF NOT EXISTS idx_customers_name ON Customers(Name);
                CREATE INDEX IF NOT EXISTS idx_customers_active ON Customers(IsActive);
                CREATE INDEX IF NOT EXISTS idx_customers_created ON Customers(CreatedDate);
                CREATE INDEX IF NOT EXISTS idx_customers_active_name ON Customers(IsActive, Name);
                
                -- Индексы для Order
                CREATE INDEX IF NOT EXISTS idx_orders_customer ON Orders(CustomerId);
                CREATE INDEX IF NOT EXISTS idx_orders_artist ON Orders(ArtistId);
                CREATE INDEX IF NOT EXISTS idx_orders_status ON Orders(Status);
                CREATE INDEX IF NOT EXISTS idx_orders_deadline ON Orders(Deadline);
                CREATE INDEX IF NOT EXISTS idx_orders_active ON Orders(IsActive);
                CREATE INDEX IF NOT EXISTS idx_orders_created ON Orders(CreatedDate);
                CREATE INDEX IF NOT EXISTS idx_orders_customer_status ON Orders(CustomerId, Status);
                
                -- Индексы для ArtistProfile
                CREATE INDEX IF NOT EXISTS idx_artists_name ON ArtistProfiles(Name);
                CREATE INDEX IF NOT EXISTS idx_artists_created ON ArtistProfiles(CreatedDate);
                
                -- Индексы для Commission
                CREATE INDEX IF NOT EXISTS idx_commissions_artist ON Commissions(ArtistProfileId);
                CREATE INDEX IF NOT EXISTS idx_commissions_name ON Commissions(Name);
                CREATE INDEX IF NOT EXISTS idx_commissions_type ON Commissions(TypeCommission);
                
                -- Индексы для Phone
                CREATE INDEX IF NOT EXISTS idx_phones_customer ON Phones(CustomerId);
                CREATE INDEX IF NOT EXISTS idx_phones_artist ON Phones(ArtistProfileId);
                CREATE INDEX IF NOT EXISTS idx_phones_number ON Phones(NumberPhone);
                
                -- Индексы для Email
                CREATE INDEX IF NOT EXISTS idx_emails_customer ON Emails(CustomerId);
                CREATE INDEX IF NOT EXISTS idx_emails_artist ON Emails(ArtistProfileId);
                CREATE INDEX IF NOT EXISTS idx_emails_address ON Emails(EmailAdress);
                
                -- Индексы для SocialLink
                CREATE INDEX IF NOT EXISTS idx_social_customer ON SocialLinks(CustomerId);
                CREATE INDEX IF NOT EXISTS idx_social_artist ON SocialLinks(ArtistProfileId);
                CREATE INDEX IF NOT EXISTS idx_social_type ON SocialLinks(TypeLink);
            ");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating indexes: {ex.Message}");
        }
    }

    private async Task SeedTestDataAsync()
    {
        try
        {
            var customerCount = await _database!.Table<LocalCustomer>().CountAsync();
            if (customerCount > 0)
                return;

            var artist = new LocalArtistProfile
            {
                Id = Guid.NewGuid(),
                Name = "Анна Художница",
                Description = "Профессиональный художник, специализируюсь на цифровой живописи",
                CreatedDate = DateTime.UtcNow.AddDays(-30)
            };
            await SaveArtistProfileAsync(artist);

            var commissions = new[]
            {
                new LocalCommission
                {
                    Id = Guid.NewGuid(),
                    ArtistProfileId = artist.Id,
                    Name = "Портрет в полный рост",
                    Description = "Детализированный портрет в цифровом формате",
                    TypeCommission = "Digital Art",
                    Price = "5000 руб",
                    CreatedDate = DateTime.UtcNow.AddDays(-20)
                },
                new LocalCommission
                {
                    Id = Guid.NewGuid(),
                    ArtistProfileId = artist.Id,
                    Name = "Иллюстрация для книги",
                    Description = "Черно-белая иллюстрация в векторном стиле",
                    TypeCommission = "Vector Art",
                    Price = "3000 руб",
                    CreatedDate = DateTime.UtcNow.AddDays(-15)
                }
            };

            foreach (var commission in commissions)
            {
                await SaveCommissionAsync(commission);
            }

            var customers = new[]
            {
                new LocalCustomer
                {
                    Id = Guid.NewGuid(),
                    Name = "Иван Петров",
                    Description = "Частный заказчик, заказывает арты для блога",
                    Communication = "Предпочитает Telegram",
                    CreatedDate = DateTime.UtcNow.AddDays(-45),
                    IsActive = true,
                    Phones = new List<CommsManager.Core.Models.Phones>
                    {
                        new() { NumberPhone = "+79161234567", TypePhone = "Mobile", Description = "Основной" }
                    },
                    Emails = new List<CommsManager.Core.Models.Email>
                    {
                        new() { EmailAdress = "ivan@example.com", TypeEmail = "Personal" }
                    }
                },
                new LocalCustomer
                {
                    Id = Guid.NewGuid(),
                    Name = "ООО 'Креатив'",
                    Description = "Дизайн-студия, заказывает иллюстрации для клиентов",
                    CreatedDate = DateTime.UtcNow.AddDays(-60),
                    IsActive = true,
                    Phones = new List<CommsManager.Core.Models.Phones>
                    {
                        new() { NumberPhone = "+74951234567", TypePhone = "Work", Description = "Офис" }
                    }
                }
            };

            foreach (var customer in customers)
            {
                await SaveCustomerAsync(customer);
            }

            var orders = new[]
            {
                new LocalOrder
                {
                    Id = Guid.NewGuid(),
                    Title = "Портрет для аватарки",
                    Description = "Нужен стильный портрет в цифровом стиле для соцсетей",
                    PriceAmount = 2500,
                    PriceCurrency = "RUB",
                    PriceSymbol = "₽",
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    Deadline = DateTime.UtcNow.AddDays(5),
                    Status = "InProgress",
                    IsActive = true,
                    CustomerId = customers[0].Id,
                    ArtistId = artist.Id
                },
                new LocalOrder
                {
                    Id = Guid.NewGuid(),
                    Title = "Серия иллюстраций для статьи",
                    Description = "5 иллюстраций в едином стиле для блога о путешествиях",
                    PriceAmount = 12000,
                    PriceCurrency = "RUB",
                    PriceSymbol = "₽",
                    CreatedDate = DateTime.UtcNow.AddDays(-5),
                    Deadline = DateTime.UtcNow.AddDays(15),
                    Status = "New",
                    IsActive = true,
                    CustomerId = customers[1].Id,
                    ArtistId = artist.Id
                }
            };

            foreach (var order in orders)
            {
                await SaveOrderAsync(order);
            }

            Console.WriteLine("Test data seeded successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding test data: {ex.Message}");
        }
    }

    public async Task<List<LocalCustomer>> GetCustomersAsync(bool includeInactive = false)
    {
        var db = await GetDatabaseAsync();
        var query = db.Table<LocalCustomer>();

        if (!includeInactive)
            query = query.Where(c => c.IsActive);

        return await query
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<LocalCustomer?> GetCustomerAsync(Guid id)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<LocalCustomer>()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<LocalCustomer>> SearchCustomersAsync(string searchTerm)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<LocalCustomer>()
            .Where(c => c.Name.Contains(searchTerm) ||
                       (c.Description != null && c.Description.Contains(searchTerm)) ||
                       (c.Communication != null && c.Communication.Contains(searchTerm)))
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<int> SaveCustomerAsync(LocalCustomer customer)
    {
        var db = await GetDatabaseAsync();

        if (customer.Id == Guid.Empty)
            customer.Id = Guid.NewGuid();

        if (customer.CreatedDate == default)
            customer.CreatedDate = DateTime.UtcNow;

        var existing = await db.Table<LocalCustomer>()
            .FirstOrDefaultAsync(c => c.Id == customer.Id);

        if (existing == null)
        {
            return await db.InsertAsync(customer);
        }
        else
        {
            return await db.UpdateAsync(customer);
        }
    }

    public async Task<int> DeleteCustomerAsync(LocalCustomer customer)
    {
        var db = await GetDatabaseAsync();
        return await db.DeleteAsync(customer);
    }

    public async Task<int> GetCustomerCountAsync(bool activeOnly = true)
    {
        var db = await GetDatabaseAsync();
        var query = db.Table<LocalCustomer>();

        if (activeOnly)
            query = query.Where(c => c.IsActive);

        return await query.CountAsync();
    }

    public async Task<List<LocalOrder>> GetOrdersAsync(OrderFilter? filter = null)
    {
        var db = await GetDatabaseAsync();
        var query = db.Table<LocalOrder>();

        if (filter != null)
        {
            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(o => o.Status == filter.Status);

            if (filter.CustomerId.HasValue)
                query = query.Where(o => o.CustomerId == filter.CustomerId.Value);

            if (filter.ArtistId.HasValue)
                query = query.Where(o => o.ArtistId == filter.ArtistId.Value);

            if (filter.IsActive.HasValue)
                query = query.Where(o => o.IsActive == filter.IsActive.Value);

            if (filter.FromDate.HasValue)
                query = query.Where(o => o.CreatedDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(o => o.CreatedDate <= filter.ToDate.Value);

            if (filter.DeadlineFrom.HasValue)
                query = query.Where(o => o.Deadline >= filter.DeadlineFrom.Value);

            if (filter.DeadlineTo.HasValue)
                query = query.Where(o => o.Deadline <= filter.DeadlineTo.Value);
        }

        return await query
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();
    }

    public async Task<LocalOrder?> GetOrderAsync(Guid id)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<LocalOrder>()
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<LocalOrder>> GetOrdersByStatusAsync(string status)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<LocalOrder>()
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();
    }

    public async Task<List<LocalOrder>> GetOverdueOrdersAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<LocalOrder>()
            .Where(o => o.Deadline < DateTime.UtcNow &&
                       o.Status != "Completed" &&
                       o.Status != "Cancelled" &&
                       o.IsActive)
            .OrderBy(o => o.Deadline)
            .ToListAsync();
    }

    public async Task<List<LocalOrder>> GetOrdersDueSoonAsync(int days = 3)
    {
        var db = await GetDatabaseAsync();
        var dateLimit = DateTime.UtcNow.AddDays(days);

        return await db.Table<LocalOrder>()
            .Where(o => o.Deadline >= DateTime.UtcNow &&
                       o.Deadline <= dateLimit &&
                       o.Status != "Completed" &&
                       o.Status != "Cancelled" &&
                       o.IsActive)
            .OrderBy(o => o.Deadline)
            .ToListAsync();
    }

    public async Task<int> SaveOrderAsync(LocalOrder order)
    {
        var db = await GetDatabaseAsync();

        if (order.Id == Guid.Empty)
            order.Id = Guid.NewGuid();

        if (order.CreatedDate == default)
            order.CreatedDate = DateTime.UtcNow;

        var existing = await db.Table<LocalOrder>()
            .FirstOrDefaultAsync(o => o.Id == order.Id);

        if (existing == null)
        {
            return await db.InsertAsync(order);
        }
        else
        {
            return await db.UpdateAsync(order);
        }
    }

    public async Task<int> DeleteOrderAsync(LocalOrder order)
    {
        var db = await GetDatabaseAsync();
        return await db.DeleteAsync(order);
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? fromDate = null, DateTime? toDate = null)
    {
        var db = await GetDatabaseAsync();
        var query = db.Table<LocalOrder>()
            .Where(o => o.Status == "Completed");

        if (fromDate.HasValue)
            query = query.Where(o => o.CreatedDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(o => o.CreatedDate <= toDate.Value);

        var result = await query.ToListAsync();
        return result.Sum(o => o.PriceAmount);
    }

    public async Task<List<LocalArtistProfile>> GetArtistProfilesAsync()
    {
        var db = await GetDatabaseAsync();
        return await db.Table<LocalArtistProfile>()
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<LocalArtistProfile?> GetArtistProfileAsync(Guid id)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<LocalArtistProfile>()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<LocalArtistProfile>> SearchArtistsAsync(string searchTerm)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<LocalArtistProfile>()
            .Where(a => a.Name.Contains(searchTerm) ||
                       (a.Description != null && a.Description.Contains(searchTerm)))
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<int> SaveArtistProfileAsync(LocalArtistProfile artist)
    {
        var db = await GetDatabaseAsync();

        if (artist.Id == Guid.Empty)
            artist.Id = Guid.NewGuid();

        if (artist.CreatedDate == default)
            artist.CreatedDate = DateTime.UtcNow;

        var existing = await db.Table<LocalArtistProfile>()
            .FirstOrDefaultAsync(a => a.Id == artist.Id);

        if (existing == null)
        {
            return await db.InsertAsync(artist);
        }
        else
        {
            return await db.UpdateAsync(artist);
        }
    }

    public async Task<int> DeleteArtistProfileAsync(LocalArtistProfile artist)
    {
        var db = await GetDatabaseAsync();
        return await db.DeleteAsync(artist);
    }

    public async Task<List<LocalCommission>> GetCommissionsByArtistAsync(Guid artistId)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<LocalCommission>()
            .Where(c => c.ArtistProfileId == artistId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<LocalCommission?> GetCommissionAsync(Guid id)
    {
        var db = await GetDatabaseAsync();
        return await db.Table<LocalCommission>()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<int> SaveCommissionAsync(LocalCommission commission)
    {
        var db = await GetDatabaseAsync();

        if (commission.Id == Guid.Empty)
            commission.Id = Guid.NewGuid();

        if (commission.CreatedDate == default)
            commission.CreatedDate = DateTime.UtcNow;

        var existing = await db.Table<LocalCommission>()
            .FirstOrDefaultAsync(c => c.Id == commission.Id);

        if (existing == null)
        {
            return await db.InsertAsync(commission);
        }
        else
        {
            commission.UpdatedDate = DateTime.UtcNow;
            return await db.UpdateAsync(commission);
        }
    }

    public async Task<int> DeleteCommissionAsync(LocalCommission commission)
    {
        var db = await GetDatabaseAsync();
        return await db.DeleteAsync(commission);
    }

    public async Task<List<LocalPhone>> GetPhonesByEntityAsync(Guid? customerId, Guid? artistId)
    {
        var db = await GetDatabaseAsync();

        if (customerId.HasValue)
        {
            return await db.Table<LocalPhone>()
                .Where(p => p.CustomerId == customerId.Value)
                .OrderBy(p => p.TypePhone)
                .ThenBy(p => p.NumberPhone)
                .ToListAsync();
        }
        else if (artistId.HasValue)
        {
            return await db.Table<LocalPhone>()
                .Where(p => p.ArtistProfileId == artistId.Value)
                .OrderBy(p => p.TypePhone)
                .ThenBy(p => p.NumberPhone)
                .ToListAsync();
        }

        return new List<LocalPhone>();
    }

    public async Task<int> SavePhoneAsync(LocalPhone phone)
    {
        var db = await GetDatabaseAsync();

        if (phone.Id == 0)
        {
            phone.CreatedDate = DateTime.UtcNow;
            return await db.InsertAsync(phone);
        }
        else
        {
            return await db.UpdateAsync(phone);
        }
    }

    public async Task<int> DeletePhoneAsync(LocalPhone phone)
    {
        var db = await GetDatabaseAsync();
        return await db.DeleteAsync(phone);
    }

    public async Task<AppStatistics> GetStatisticsAsync()
    {
        var db = await GetDatabaseAsync();

        var stats = new AppStatistics
        {
            TotalCustomers = await db.Table<LocalCustomer>().CountAsync(),
            ActiveCustomers = await db.Table<LocalCustomer>().Where(c => c.IsActive).CountAsync(),
            TotalOrders = await db.Table<LocalOrder>().CountAsync(),
            ActiveOrders = await db.Table<LocalOrder>().Where(o => o.IsActive).CountAsync(),
            TotalArtists = await db.Table<LocalArtistProfile>().CountAsync(),
            TotalCommissions = await db.Table<LocalCommission>().CountAsync(),
            NewOrders = await db.Table<LocalOrder>().Where(o => o.Status == "New").CountAsync(),
            InProgressOrders = await db.Table<LocalOrder>().Where(o => o.Status == "InProgress").CountAsync(),
            CompletedOrders = await db.Table<LocalOrder>().Where(o => o.Status == "Completed").CountAsync(),
            OverdueOrders = await db.Table<LocalOrder>()
                .Where(o => o.Deadline < DateTime.UtcNow &&
                           o.Status != "Completed" &&
                           o.Status != "Cancelled" &&
                           o.IsActive)
                .CountAsync()
        };

        var completedOrders = await db.Table<LocalOrder>()
            .Where(o => o.Status == "Completed")
            .ToListAsync();

        stats.TotalRevenue = completedOrders.Sum(o => o.PriceAmount);

        return stats;
    }

    public async Task BackupDatabaseAsync(string backupPath)
    {
        var db = await GetDatabaseAsync();
        await db.BackupAsync(backupPath);
    }

    public async Task<int> ExecuteQueryAsync(string query, params object[] args)
    {
        var db = await GetDatabaseAsync();
        return await db.ExecuteAsync(query, args);
    }

    public async Task<List<T>> QueryAsync<T>(string query, params object[] args) where T : new()
    {
        var db = await GetDatabaseAsync();
        return await db.QueryAsync<T>(query, args);
    }

    public async Task VacuumDatabaseAsync()
    {
        var db = await GetDatabaseAsync();
        await db.ExecuteAsync("VACUUM");
    }

    public async Task ResetDatabaseAsync()
    {
        await _databaseLock.WaitAsync();

        try
        {
            if (_database != null)
            {
                await _database.CloseAsync();
                _database = null;
            }

            if (File.Exists(_databasePath))
            {
                File.Delete(_databasePath);
            }

            _isInitialized = false;
            await GetDatabaseAsync();
        }
        finally
        {
            _databaseLock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_database != null)
        {
            await _database.CloseAsync();
            _database = null;
        }
        _databaseLock.Dispose();
    }
}