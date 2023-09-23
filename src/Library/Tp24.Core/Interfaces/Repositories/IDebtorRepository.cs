using Tp24.Core.Domain.Entities;

namespace Tp24.Core.Interfaces.Repositories;

public interface IDebtorRepository
{
    Task<DebtorDomainModel> AddAsync(DebtorDomainModel debtor);
    Task<List<DebtorDomainModel>> AddRangeAsync(IEnumerable<DebtorDomainModel> debtors);
    Task<DebtorDomainModel?> FindByReferenceAsync(string reference);
    Task<List<DebtorDomainModel>> FindByReferencesAsync(IEnumerable<string> references);
}