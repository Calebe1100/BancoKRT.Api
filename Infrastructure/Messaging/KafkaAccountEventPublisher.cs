using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BancoKRT.Api.Domain.Accounts;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace BancoKRT.Api.Infrastructure.Messaging
{
    public class KafkaAccountEventPublisher : IAccountEventPublisher
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public KafkaAccountEventPublisher(IConfiguration configuration)
        {
            var bootstrapServers = configuration["Kafka:BootstrapServers"];
            _topic = configuration["Kafka:AccountTopic"] ?? "accounts-events";

            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public Task PublishCreatedAsync(Account account, CancellationToken cancellationToken = default)
        {
            return PublishAsync("created", account, cancellationToken);
        }

        public Task PublishUpdatedAsync(Account account, CancellationToken cancellationToken = default)
        {
            return PublishAsync("updated", account, cancellationToken);
        }

        public Task PublishDeletedAsync(Account account, CancellationToken cancellationToken = default)
        {
            return PublishAsync("deleted", account, cancellationToken);
        }

        private Task PublishAsync(string operation, Account account, CancellationToken cancellationToken)
        {
            var payload = JsonSerializer.Serialize(new
            {
                operation,
                account.Id,
                account.HolderName,
                account.Cpf,
                Status = account.Status.ToString()
            });

            return _producer.ProduceAsync(_topic, new Message<string, string>
            {
                Key = account.Id.ToString(),
                Value = payload
            }, cancellationToken);
        }
    }
}
