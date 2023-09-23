using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tp24.Application.Features.Receivable.Commands;
using Tp24.Infrastructure.DataAccess;
using Tp24.Infrastructure.Mappings;
using Tp24.Infrastructure.Repositories;

namespace Tp24.UnitTest;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var dbOptions = InMemoryDatabaseHelper.CreateNewContextOptions();
        services.AddSingleton(new Tp24DbContext(dbOptions));
        services.AddTransient<DebtorRepository>();

        // Add FluentValidation Validators
        services.AddValidatorsFromAssemblyContaining<AddReceivablesCommand>();

        services.AddAutoMapper(typeof(ReceivableProfile).Assembly);
    }
}