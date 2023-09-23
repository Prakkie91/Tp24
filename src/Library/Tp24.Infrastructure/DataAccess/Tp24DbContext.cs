using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Tp24.Infrastructure.DataAccess.Entities;

namespace Tp24.Infrastructure.DataAccess;

public class Tp24DbContext : DbContext
{
    public Tp24DbContext(DbContextOptions<Tp24DbContext> options) : base(options)
    {
    }

    public Tp24DbContext()
    {
    }

    public DbSet<DebtorDataModel> Debtors { get; set; }
    public DbSet<ReceivableDataModel> Receivables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.UseInMemoryDatabase("Tp24", b => b.EnableNullChecks(false));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}