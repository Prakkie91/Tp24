using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tp24.Core.Domain.Entities;
using Tp24.Core.Interfaces.Repositories;
using Tp24.Infrastructure.DataAccess;
using Tp24.Infrastructure.DataAccess.Entities;

namespace Tp24.Infrastructure.Repositories;

public class ReceivableRepository : IReceivableRepository
{
    private readonly Tp24DbContext _dbContext;
    private readonly IMapper _mapper;

    public ReceivableRepository(Tp24DbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ReceivableDomainModel> AddAsync(ReceivableDomainModel receivable)
    {
        var receivableEntity = _mapper.Map<ReceivableDataModel>(receivable);

        _dbContext.Receivables.Add(receivableEntity);

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ReceivableDomainModel>(receivableEntity);
    }

    public async Task<List<ReceivableDomainModel>> AddRangeAsync(IEnumerable<ReceivableDomainModel> receivables)
    {
        var receivableEntities = _mapper.Map<List<ReceivableDataModel>>(receivables);
        await _dbContext.Receivables.AddRangeAsync(receivableEntities);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<List<ReceivableDomainModel>>(receivableEntities);
    }


    public async Task<ReceivableDomainModel?> FindByReferenceAsync(string reference)
    {
        var entity = await _dbContext.Receivables
            .SingleOrDefaultAsync(x => x.Reference == reference);

        return entity != null ? _mapper.Map<ReceivableDomainModel>(entity) : null;
    }

    public async Task<ReceivablesSummaryDomainModel> GetReceivablesSummaryAsync()
    {
        var today = DateTime.Today;

        var openInvoices = _dbContext.Receivables
            .Where(r => r.ClosedDate == null && !r.Cancelled);

        var closedInvoices = _dbContext.Receivables
            .Where(r => r.ClosedDate != null);

        var overdueInvoices = openInvoices
            .Where(r => r.DueDate < today);

        var totalReceivables = await _dbContext.Receivables.CountAsync();

        var openInvoiceCount = await openInvoices.CountAsync();

        var closedInvoiceCount = await closedInvoices.CountAsync();

        var totalOpeningValue = await _dbContext.Receivables
            .SumAsync(r => r.OpeningValue);

        var totalPaidValue = await _dbContext.Receivables
            .SumAsync(r => r.PaidValue);

        var numOfOverdueInvoices = await overdueInvoices.CountAsync();
        var totalOverdueAmount = await overdueInvoices.SumAsync(r => r.OpeningValue - r.PaidValue);

        var uniqueDebtors = await _dbContext.Receivables.Select(r => r.DebtorId).Distinct().CountAsync();

        return new ReceivablesSummaryDomainModel
        {
            TotalReceivables = totalReceivables,
            OpenInvoiceCount = openInvoiceCount,
            ClosedInvoiceCount = closedInvoiceCount,
            TotalOpeningValue = totalOpeningValue,
            TotalPaidValue = totalPaidValue,
            NumOfOverdueInvoices = numOfOverdueInvoices,
            TotalOverdueAmount = totalOverdueAmount,
            UniqueDebtors = uniqueDebtors
        };
    }
}