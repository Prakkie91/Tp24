using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tp24.Core.Interfaces.Repositories;
using Tp24.Infrastructure.DataAccess;
using Tp24.Infrastructure.DataAccess.Seed.Extensions;
using Tp24.Infrastructure.Mappings;
using Tp24.Infrastructure.Repositories;

namespace Tp24.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddScoped<IDebtorRepository, DebtorRepository>()
            .AddScoped<IReceivableRepository, ReceivableRepository>()
            .AddDbContext<Tp24DbContext>(opt => opt.UseInMemoryDatabase("Tp24"))
            .RegisterDatabaseSeeders()
            .AddAutoMapper(typeof(DebtorProfile).Assembly);
        ;
    }
}