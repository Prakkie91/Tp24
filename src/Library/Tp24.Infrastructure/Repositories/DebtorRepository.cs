using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tp24.Core.Domain.Entities;
using Tp24.Core.Interfaces.Repositories;
using Tp24.Infrastructure.DataAccess;
using Tp24.Infrastructure.DataAccess.Entities;

namespace Tp24.Infrastructure.Repositories
{
    public class DebtorRepository : IDebtorRepository
    {
        private readonly Tp24DbContext _dbContext;
        private readonly IMapper _mapper;

        public DebtorRepository(Tp24DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<DebtorDomainModel> AddAsync(DebtorDomainModel debtor)
        {
            var debtorEntity = _mapper.Map<DebtorDataModel>(debtor);
            await _dbContext.Debtors.AddAsync(debtorEntity);
            await _dbContext.SaveChangesAsync();
            
            return _mapper.Map<DebtorDomainModel>(debtorEntity);
        }

        public async Task<DebtorDomainModel?> FindByReferenceAsync(string reference)
        {
            var entity = await _dbContext.Debtors
                .SingleOrDefaultAsync(x => x.Reference == reference);

            return entity != null ? _mapper.Map<DebtorDomainModel>(entity) : null;
        }
    }
}