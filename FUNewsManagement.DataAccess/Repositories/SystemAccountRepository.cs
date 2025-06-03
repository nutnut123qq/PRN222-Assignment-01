using Microsoft.EntityFrameworkCore;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class SystemAccountRepository : Repository<SystemAccount>, ISystemAccountRepository
    {
        public SystemAccountRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<SystemAccount?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        public async Task<SystemAccount?> AuthenticateAsync(string email, string password)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.AccountEmail == email && a.AccountPassword == password);
        }

        public async Task<IEnumerable<SystemAccount>> GetByRoleAsync(short role)
        {
            return await _dbSet.Where(a => a.AccountRole == role).ToListAsync();
        }

        public async Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null)
        {
            var query = _dbSet.Where(a => a.AccountEmail == email);
            if (excludeAccountId.HasValue)
            {
                query = query.Where(a => a.AccountId != excludeAccountId.Value);
            }
            return await query.AnyAsync();
        }
    }
}
