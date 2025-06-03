using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface ISystemAccountRepository : IRepository<SystemAccount>
    {
        Task<SystemAccount?> GetByEmailAsync(string email);
        Task<SystemAccount?> AuthenticateAsync(string email, string password);
        Task<IEnumerable<SystemAccount>> GetByRoleAsync(short role);
        Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null);
    }
}
