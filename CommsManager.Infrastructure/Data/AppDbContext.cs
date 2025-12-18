public class AppDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<PriceList> PriceLists { get; set; }
    public DbSet<PortfolioItem> PortfolioItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //TODO: Для разных платформ - разная конфигурация
    }
}