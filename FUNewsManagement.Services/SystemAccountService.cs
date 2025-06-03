using FUNewsManagement.DataAccess.Models;
using FUNewsManagement.DataAccess.Repositories;

namespace FUNewsManagement.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _accountRepository;

        public SystemAccountService(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        public async Task<SystemAccount?> GetAccountByIdAsync(short id)
        {
            return await _accountRepository.GetByIdAsync(id);
        }

        public async Task<SystemAccount?> GetAccountByEmailAsync(string email)
        {
            return await _accountRepository.GetByEmailAsync(email);
        }

        public async Task<SystemAccount?> AuthenticateAsync(string email, string password)
        {
            return await _accountRepository.AuthenticateAsync(email, password);
        }

        public async Task<SystemAccount> CreateAccountAsync(SystemAccount account)
        {
            // Validate email uniqueness
            if (await _accountRepository.EmailExistsAsync(account.AccountEmail))
            {
                throw new InvalidOperationException("Email already exists.");
            }

            await _accountRepository.AddAsync(account);
            return account;
        }

        public async Task<SystemAccount> UpdateAccountAsync(SystemAccount account)
        {
            // Validate email uniqueness (excluding current account)
            if (await _accountRepository.EmailExistsAsync(account.AccountEmail, account.AccountId))
            {
                throw new InvalidOperationException("Email already exists.");
            }

            await _accountRepository.UpdateAsync(account);
            return account;
        }

        public async Task DeleteAccountAsync(short id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            if (account != null)
            {
                await _accountRepository.DeleteAsync(account);
            }
        }

        public async Task<IEnumerable<SystemAccount>> GetAccountsByRoleAsync(short role)
        {
            return await _accountRepository.GetByRoleAsync(role);
        }

        public async Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null)
        {
            return await _accountRepository.EmailExistsAsync(email, excludeAccountId);
        }

        public async Task<IEnumerable<SystemAccount>> SearchAccountsAsync(string searchTerm)
        {
            return await _accountRepository.FindAsync(a => 
                a.AccountName.Contains(searchTerm) || 
                a.AccountEmail.Contains(searchTerm));
        }
    }
}
