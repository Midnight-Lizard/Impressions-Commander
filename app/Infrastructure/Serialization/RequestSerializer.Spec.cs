using FluentAssertions;
using MidnightLizard.Impressions.Commander.Infrastructure.Authentication;
using MidnightLizard.Impressions.Commander.Requests.AddLike;
using MidnightLizard.Testing.Utilities;
using System;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Serialization
{
    public class RequestSerializerSpec
    {
        private readonly RequestSerializer serialiser;
        private readonly UserId testUserId = new UserId("test-user-id");
        private readonly Guid id = Guid.NewGuid(), aggregateId = Guid.NewGuid();
        private readonly string testObkectType = "test-type";
        private readonly string snapshot;

        public RequestSerializerSpec()
        {
            this.serialiser = new RequestSerializer(SchemaVersion.Latest);
            this.snapshot =
                $@"{{" +
                    $@"""CorrelationId"":""{this.id}""," +
                    $@"""Type"":""{nameof(AddLikeRequest)}""," +
                    $@"""Version"":""{SchemaVersion.Latest}""," +
                    $@"""UserId"":""{this.testUserId.Value}""," +
                    $@"""Payload"":" +
                    $@"{{" +
                        $@"""AggregateId"":""{this.aggregateId}""," +
                        $@"""Id"":""{this.id}""," +
                        $@"""ObjectType"":""{this.testObkectType}""" +
                    $@"}}" +
                $@"}}";
        }

        [It(nameof(RequestSerializer))]
        public void Should_serialize_request_the_same_way_as_in_snapshot()
        {
            var testRequest = new AddLikeRequest
            {
                AggregateId = this.aggregateId,
                Id = this.id,
                ObjectType = this.testObkectType
            };

            var json = this.serialiser.Serialize(testRequest, this.testUserId);
            json.Should().Be(this.snapshot);
        }
    }
}
