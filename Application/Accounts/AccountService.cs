using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BancoKRT.Api.Domain.Accounts;
using BancoKRT.Api.Infrastructure.Messaging;
using Microsoft.Extensions.Caching.Distributed;

namespace BancoKRT.Api.Application.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IAccountEventPublisher _eventPublisher;
        private readonly IDistributedCache _cache;

        public AccountService(IAccountRepository repository, IAccountEventPublisher eventPublisher, IDistributedCache cache)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _cache = cache;
        }

        public async Task<AccountDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var cacheKey = GetCacheKey(id);
            var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (cached != null)
            {
                var cachedDto = JsonSerializer.Deserialize<AccountDto>(cached);
                if (cachedDto != null)
                {
                    return cachedDto;
                }
            }

            var account = await _repository.GetByIdAsync(id, cancellationToken);
            if (account == null)
            {
                return null;
            }

            var dto = MapToDto(account);
            await SetCacheAsync(cacheKey, dto, cancellationToken);
            return dto;
        }

        public async Task<IReadOnlyCollection<AccountDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var accounts = await _repository.GetAllAsync(cancellationToken);
            return accounts.Select(MapToDto).ToArray();
        }

        public async Task<AccountDto> CreateAsync(CreateAccountRequest request, CancellationToken cancellationToken = default)
        {
            var account = new Account(Guid.NewGuid(), request.HolderName, request.Cpf, request.Status);

            await _repository.AddAsync(account, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            await _eventPublisher.PublishCreatedAsync(account, cancellationToken);

            var dto = MapToDto(account);
            await SetCacheAsync(GetCacheKey(account.Id), dto, cancellationToken);

            return dto;
        }

        public async Task<AccountDto?> UpdateAsync(Guid id, UpdateAccountRequest request, CancellationToken cancellationToken = default)
        {
            var account = await _repository.GetByIdAsync(id, cancellationToken);
            if (account == null)
            {
                return null;
            }

            account.Update(request.HolderName, request.Cpf, request.Status);

            await _repository.UpdateAsync(account, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            await _eventPublisher.PublishUpdatedAsync(account, cancellationToken);

            var dto = MapToDto(account);
            await SetCacheAsync(GetCacheKey(account.Id), dto, cancellationToken);

            return dto;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var account = await _repository.GetByIdAsync(id, cancellationToken);
            if (account == null)
            {
                return false;
            }

            await _repository.DeleteAsync(account, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            await _eventPublisher.PublishDeletedAsync(account, cancellationToken);

            await _cache.RemoveAsync(GetCacheKey(id), cancellationToken);

            return true;
        }

        private static AccountDto MapToDto(Account account)
        {
            return new AccountDto
            {
                Id = account.Id,
                HolderName = account.HolderName,
                Cpf = account.Cpf,
                Status = account.Status
            };
        }

        private static string GetCacheKey(Guid id)
        {
            var today = DateTime.UtcNow.Date.ToString("yyyyMMdd");
            return $"account:{today}:{id}";
        }

        private Task SetCacheAsync(string key, AccountDto dto, CancellationToken cancellationToken)
        {
            var json = JsonSerializer.Serialize(dto);
            var expiration = GetTodayExpiration();

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            return _cache.SetStringAsync(key, json, options, cancellationToken);
        }

        private static TimeSpan GetTodayExpiration()
        {
            var now = DateTimeOffset.UtcNow;
            var endOfDay = now.Date.AddDays(1);
            return endOfDay - now;
        }
    }
}
