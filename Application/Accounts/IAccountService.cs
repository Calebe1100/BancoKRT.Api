using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BancoKRT.Api.Application.Accounts
{
    public interface IAccountService
    {
        Task<AccountDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<AccountDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<AccountDto> CreateAsync(CreateAccountRequest request, CancellationToken cancellationToken = default);
        Task<AccountDto?> UpdateAsync(Guid id, UpdateAccountRequest request, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
