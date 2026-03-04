using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BancoKRT.Api.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace BancoKRT.Api.Infrastructure.Persistence
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BancoKrtContext _context;

        public AccountRepository(BancoKrtContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Account>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var accounts = await _context.Accounts.AsNoTracking().ToListAsync(cancellationToken);
            return accounts;
        }

        public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
        {
            await _context.Accounts.AddAsync(account, cancellationToken);
        }

        public Task UpdateAsync(Account account, CancellationToken cancellationToken = default)
        {
            _context.Accounts.Update(account);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Account account, CancellationToken cancellationToken = default)
        {
            _context.Accounts.Remove(account);
            return Task.CompletedTask;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
