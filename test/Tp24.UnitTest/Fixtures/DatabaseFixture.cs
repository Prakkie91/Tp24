using AutoMapper;
using Tp24.Infrastructure.DataAccess;
using Tp24.Infrastructure.Mappings;
using Tp24.Infrastructure.Repositories;

namespace Tp24.UnitTest.Fixtures;

public class DatabaseFixture
{
    private readonly IMapper _mapper;

    public DatabaseFixture()
    {
        _mapper = ConfigureAutoMapper();

        var dbOptions = InMemoryDatabaseHelper.CreateNewContextOptions();
        Context = new Tp24DbContext(dbOptions);
        DebtorRepository = new DebtorRepository(Context, _mapper);
        ReceivableRepository = new ReceivableRepository(Context, _mapper);
        Context.SaveChanges();
    }

    public Tp24DbContext Context { get; }
    public DebtorRepository DebtorRepository { get; }
    public ReceivableRepository ReceivableRepository { get; }

    private IMapper ConfigureAutoMapper()
    {
        var configuration = new MapperConfiguration(cfg => { cfg.AddMaps(typeof(DebtorProfile).Assembly); });

        return configuration.CreateMapper();
    }

    ~DatabaseFixture()
    {
        Context.Dispose();
    }
}