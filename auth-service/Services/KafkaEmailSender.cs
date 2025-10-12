using System.Text.Json;
using System.Threading.Tasks;
using auth_service.Configuration;
using auth_service.Models;
using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.Producers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace auth_service.Services
{
    // Stub email sender for development and testing purposes
    // TODO: Implement a real email sender service
    public class KafkaEmailSender : IEmailSender<ApplicationUser>
    {
        private readonly IMessageProducer _producer;
        private readonly KafkaSettings _kafkaOptions;

        public KafkaEmailSender(
            IProducerAccessor producerAccessor,
            IOptions<KafkaSettings> kafkaOptions
        )
        {
            _kafkaOptions = kafkaOptions.Value;
            _producer = producerAccessor.GetProducer(_kafkaOptions.ProducerName);
        }

        public async Task SendConfirmationLinkAsync(
            ApplicationUser user,
            string email,
            string confirmationLink
        )
        {
            var message = new
            {
                user_id = user.Id,
                email,
                confirmationLink,
                type = EAccountMessageTypes.EmailConfirmation,
            };
            var kafkaMessage = new Message<string, string>
            {
                Key = user.Id,
                Value = JsonSerializer.Serialize(message),
            };

            await _producer.ProduceAsync(_kafkaOptions.TopicNames.AccountManagement, kafkaMessage);
        }

        public async Task SendPasswordResetCodeAsync(
            ApplicationUser user,
            string email,
            string resetCode
        )
        {
            var message = new
            {
                user_id = user.Id,
                email,
                resetCode,
                type = EAccountMessageTypes.PasswordResetCode,
            };
            var kafkaMessage = new Message<string, string>
            {
                Key = user.Id,
                Value = JsonSerializer.Serialize(message),
            };

            await _producer.ProduceAsync(_kafkaOptions.TopicNames.AccountManagement, kafkaMessage);
        }

        public async Task SendPasswordResetLinkAsync(
            ApplicationUser user,
            string email,
            string resetLink
        )
        {
            var message = new
            {
                user_id = user.Id,
                email,
                resetLink,
                type = EAccountMessageTypes.PasswordResetLink,
            };
            var kafkaMessage = new Message<string, string>
            {
                Key = user.Id,
                Value = JsonSerializer.Serialize(message),
            };

            await _producer.ProduceAsync(_kafkaOptions.TopicNames.AccountManagement, kafkaMessage);
        }
    }
}
