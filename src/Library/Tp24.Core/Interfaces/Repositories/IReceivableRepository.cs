using Tp24.Core.Domain.Entities;

namespace Tp24.Core.Interfaces.Repositories;

public interface IReceivableRepository
{
    Task<ReceivableDomainModel> AddAsync(ReceivableDomainModel receivable);
    Task<List<ReceivableDomainModel>> AddRangeAsync(IEnumerable<ReceivableDomainModel> receivables);
    Task<ReceivableDomainModel?> FindByReferenceAsync(string reference);
    Task<List<ReceivableDomainModel>> FindByReferencesAsync(IEnumerable<string> references);
    Task<ReceivablesSummaryDomainModel> GetReceivablesSummaryAsync();
}