using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tp24.Core.Domain.Entities;
using Tp24.Core.Interfaces.Repositories;
using Tp24.Infrastructure.DataAccess;
using Tp24.Infrastructure.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<List<DebtorDomainModel>> AddRangeAsync(IEnumerable<DebtorDomainModel> debtors)
        {
            var debtorEntities = _mapper.Map<List<DebtorDataModel>>(debtors);
            await _dbContext.Debtors.AddRangeAsync(debtorEntities);
            await _dbContext.SaveChangesAsync();
            
            return _mapper.Map<List<DebtorDomainModel>>(debtorEntities);
        }

        public async Task<DebtorDomainModel?> FindByReferenceAsync(string reference)
        {
            var entity = await _dbContext.Debtors
                .SingleOrDefaultAsync(x => x.Reference == reference);

            return entity != null ? _mapper.Map<DebtorDomainModel>(entity) : null;
        }

        public async Task<List<DebtorDomainModel>> FindByReferencesAsync(IEnumerable<string> references)
        {
            var entities = await _dbContext.Debtors
                .Where(x => references.Contains(x.Reference))
                .ToListAsync();

            return _mapper.Map<List<DebtorDomainModel>>(entities);
        }
    }
}
