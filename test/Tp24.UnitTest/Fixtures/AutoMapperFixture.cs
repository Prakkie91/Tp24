using AutoMapper;
using Tp24.Infrastructure.Mappings;

namespace Tp24.UnitTest.Fixtures;

public class AutoMapperFixture
{
    public AutoMapperFixture()
    {
        Configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(DebtorProfile).Assembly);
        });

        Mapper = Configuration.CreateMapper();
    }

    public IConfigurationProvider Configuration { get; }
    public IMapper Mapper { get; }
}