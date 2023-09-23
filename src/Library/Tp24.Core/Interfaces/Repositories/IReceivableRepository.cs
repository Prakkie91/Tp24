using Tp24.Core.Domain.Entities;

namespace Tp24.Core.Interfaces.Repositories;

public interface IReceivableRepository
{
    Task<ReceivableDomainModel> AddAsync(ReceivableDomainModel receivable);
    Task<ReceivableDomainModel?> FindByReferenceAsync(string reference);
    Task<ReceivablesSummaryDomainModel> GetReceivablesSummaryAsync();

}