using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.Services
{
    public interface ISystemAccountService
    {
        Task<IEnumerable<SystemAccount>> GetAllAccountsAsync();
        Task<SystemAccount?> GetAccountByIdAsync(short id);
        Task<SystemAccount?> GetAccountByEmailAsync(string email);
        Task<SystemAccount?> AuthenticateAsync(string email, string password);
        Task<SystemAccount> CreateAccountAsync(SystemAccount account);
        Task<SystemAccount> UpdateAccountAsync(SystemAccount account);
        Task DeleteAccountAsync(short id);
        Task<IEnumerable<SystemAccount>> GetAccountsByRoleAsync(short role);
        Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null);
        Task<IEnumerable<SystemAccount>> SearchAccountsAsync(string searchTerm);
    }
}
