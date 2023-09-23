using AutoMapper;
using Tp24.Core.Domain.Entities;
using Tp24.Infrastructure.DataAccess.Entities;

namespace Tp24.Infrastructure.Mappings;

public class ReceivableProfile : Profile
{
    public ReceivableProfile()
    {
        CreateMap<ReceivableDataModel, ReceivableDomainModel>()
            .ReverseMap()
            .ForMember(dest => dest.Debtor, opt => opt.Ignore());
    }
}