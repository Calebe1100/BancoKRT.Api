using System.Threading;
using System.Threading.Tasks;
using BancoKRT.Api.Domain.Accounts;

namespace BancoKRT.Api.Infrastructure.Messaging
{
    public interface IAccountEventPublisher
    {
        Task PublishCreatedAsync(Account account, CancellationToken cancellationToken = default);
        Task PublishUpdatedAsync(Account account, CancellationToken cancellationToken = default);
        Task PublishDeletedAsync(Account account, CancellationToken cancellationToken = default);
    }
}
