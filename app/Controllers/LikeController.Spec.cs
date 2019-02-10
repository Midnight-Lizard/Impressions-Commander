using Autofac.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MidnightLizard.Impressions.Commander.Configuration;
using MidnightLizard.Impressions.Commander.Infrastructure.Authentication;
using MidnightLizard.Impressions.Commander.Infrastructure.Queue;
using MidnightLizard.Impressions.Commander.Requests.AddLike;
using MidnightLizard.Impressions.Commander.Requests.Common;
using MidnightLizard.Impressions.Commander.Requests.RemoveLike;
using MidnightLizard.Testing.Utilities;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Controllers
{
    public class LikeControllerSpec
    {
        private readonly IRequestQueuer<LIKES_QUEUE_CONFIG> testQueuer;
        private readonly TestServer testServer;
        private readonly HttpClient testClient;

        public LikeControllerSpec()
        {
            this.testQueuer = Substitute.For<IRequestQueuer<LIKES_QUEUE_CONFIG>>();
            this.testServer = new TestServer(new WebHostBuilder()
                .ConfigureServices(x => x.AddAutofac())
                .ConfigureTestServices(services => services
                    .AddSingleton<IRequestQueuer<LIKES_QUEUE_CONFIG>>(this.testQueuer))
                .UseSetting(nameof(LIKES_QUEUE_CONFIG), JsonConvert.SerializeObject(
                    new LIKES_QUEUE_CONFIG
                    {
                        TopicName = "test",
                        ProducerSettings = new Dictionary<string, object>
                        {
                            ["bootstrap.servers"] = "test:123"
                        }
                    }))
                .UseSetting(nameof(CorsConfig.ALLOWED_ORIGINS), JsonConvert.SerializeObject(new CorsConfig
                {
                    ALLOWED_ORIGINS = "localhost"
                }))
                .UseStartup<StartupStub>());
            this.testClient = this.testServer.CreateClient();
            this.testClient.DefaultRequestHeaders.Add("api-version", "1.0");
            this.testClient.DefaultRequestHeaders.Add("schema-version", SchemaVersion.Latest.ToString());
        }

        public class PublishSpec : LikeControllerSpec
        {
            private readonly AddLikeRequest correctRequest = new AddLikeRequest
            {
                AggregateId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                ObjectType = "test"
            };

            [It(nameof(LikeController.Add))]
            public async Task Should_successfuly_process_correct_request()
            {
                var json = JsonConvert.SerializeObject(this.correctRequest);
                HttpContent jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                var result = await this.testClient.PostAsync("like/add", jsonContent);

                result.StatusCode.Should().Be(HttpStatusCode.Accepted, await result.Content.ReadAsStringAsync());

                await this.testQueuer.Received(1).QueueRequest(
                    Arg.Is<DomainRequest>(x => x.DeserializerType == typeof(AddLikeRequestDeserializer_v1)),
                    Arg.Any<UserId>());
            }

            [It(nameof(LikeController.Add))]
            public async Task Should_return_BadRequest_response_when_request_is_incorrect()
            {
                var json = JsonConvert.SerializeObject(new AddLikeRequest());
                HttpContent jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                var result = await this.testClient.PostAsync("like/add", jsonContent);

                result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }

            [It(nameof(LikeController.Add))]
            public async Task Should_return_BadRequest_response_when_SchemaVersion_is_missing()
            {
                this.testClient.DefaultRequestHeaders.Remove("schema-version");
                var json = JsonConvert.SerializeObject(this.correctRequest);
                HttpContent jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                var result = await this.testClient.PostAsync("like/add", jsonContent);
                result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        public class RemoveSpec : LikeControllerSpec
        {
            private readonly RemoveLikeRequest correctRequest = new RemoveLikeRequest
            {
                AggregateId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                ObjectType = "test"
            };

            [It(nameof(LikeController.Remove))]
            public async Task Should_successfuly_process_correct_request()
            {
                var json = JsonConvert.SerializeObject(this.correctRequest);
                HttpContent jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                var result = await this.testClient.PostAsync("like/remove", jsonContent);

                result.StatusCode.Should().Be(HttpStatusCode.Accepted, await result.Content.ReadAsStringAsync());

                await this.testQueuer.Received(1).QueueRequest(
                    Arg.Is<DomainRequest>(x => x.DeserializerType == typeof(RemoveLikeRequestDeserializer_v1)),
                    Arg.Any<UserId>());
            }

            [It(nameof(LikeController.Remove))]
            public async Task Should_return_BadRequest_response_when_request_is_incorrect()
            {
                var json = JsonConvert.SerializeObject(new RemoveLikeRequest());
                HttpContent jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                var result = await this.testClient.PostAsync("like/remove", jsonContent);

                result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }

            [It(nameof(LikeController.Remove))]
            public async Task Should_return_BadRequest_response_when_SchemaVersion_is_missing()
            {
                this.testClient.DefaultRequestHeaders.Remove("schema-version");
                var json = JsonConvert.SerializeObject(this.correctRequest);
                HttpContent jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                var result = await this.testClient.PostAsync("like/remove", jsonContent);
                result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }
    }
}
