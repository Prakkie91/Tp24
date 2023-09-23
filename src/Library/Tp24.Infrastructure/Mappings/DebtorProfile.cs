using AutoMapper;
using Tp24.Core.Domain.Entities;
using Tp24.Infrastructure.DataAccess.Entities;

namespace Tp24.Infrastructure.Mappings;

public class DebtorProfile : Profile
{
    public DebtorProfile()
    {
        CreateMap<DebtorDataModel, DebtorDomainModel>()
            .ReverseMap();
    }
}