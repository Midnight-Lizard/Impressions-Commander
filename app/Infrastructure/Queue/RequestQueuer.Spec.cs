using Confluent.Kafka;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using MidnightLizard.Impressions.Commander.Infrastructure.Authentication;
using MidnightLizard.Impressions.Commander.Infrastructure.Serialization;
using MidnightLizard.Impressions.Commander.Requests.AddLike;
using MidnightLizard.Impressions.Commander.Requests.Common;
using MidnightLizard.Testing.Utilities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Queue
{
    public class RequestQueuerSpec : RequestQueuer<LIKES_QUEUE_CONFIG>
    {
        private readonly DomainRequest testRequest = new AddLikeRequest
        {
            AggregateId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
            ObjectType = "test"
        };
        private readonly UserId testUserId = new UserId("test-user-id");
        private readonly Message<string, string> errorMessage =
            new Message<string, string>("", 0, 0, "", "", new Timestamp(),
                new Error(ErrorCode.Unknown, "test"));

        public RequestQueuerSpec() : base(
            new LIKES_QUEUE_CONFIG
            {
                TopicName = "test",
                ProducerSettings = new Dictionary<string, object>
                {
                    ["bootstrap.servers"] = "test:123"
                }
            },
            Substitute.For<ILogger<RequestQueuer<LIKES_QUEUE_CONFIG>>>(),
            Substitute.For<IRequestSerializer>())
        {
            this.producer = Substitute.For<ISerializingProducer<string, string>>();
            this.producer.ProduceAsync("test", this.testRequest.AggregateId.ToString(), Arg.Any<string>())
                .Returns(new Message<string, string>("", 0, 0, "", "", new Timestamp(), new Error(ErrorCode.NoError)));
        }

        public class QueueRequestSpec : RequestQueuerSpec
        {
            [It(nameof(QueueRequest))]
            public async Task Should_call_KafkaProducer__ProduceAsync()
            {
                await this.QueueRequest(this.testRequest, this.testUserId);

                await this.producer.Received(1).ProduceAsync("test", this.testRequest.AggregateId.ToString(), Arg.Any<string>());
            }

            [It(nameof(QueueRequest))]
            public void Should_throw_ApplicationException_when_KafkaProducer__ProduceAsync_returns_Error()
            {
                this.producer.ProduceAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                    .Returns(this.errorMessage);

                Func<Task> act = async () => await this.QueueRequest(this.testRequest, this.testUserId);

                act.Should().Throw<ApplicationException>();
            }

            [It(nameof(QueueRequest))]
            public async Task Should_log_Error_when_KafkaProducer__ProduceAsync_returns_Error()
            {
                this.producer.ProduceAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                    .Returns(this.errorMessage);

                try
                {
                    await this.QueueRequest(this.testRequest, this.testUserId);
                }
                catch { }

                this.logger.Received(1).Log(LogLevel.Error, 0, Arg.Any<FormattedLogValues>(), null, Arg.Any<Func<object, Exception, string>>());
            }
        }
    }
}
