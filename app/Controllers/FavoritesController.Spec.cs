using Autofac.Extensions.DependencyInjection;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MidnightLizard.Impressions.Commander.Configuration;
using MidnightLizard.Impressions.Commander.Infrastructure.Authentication;
using MidnightLizard.Impressions.Commander.Infrastructure.Queue;
using MidnightLizard.Impressions.Commander.Requests.AddToFavorites;
using MidnightLizard.Impressions.Commander.Requests.Common;
using MidnightLizard.Impressions.Commander.Requests.RemoveFromFavorites;
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
    public class FavoritesControllerSpec
    {
        private readonly IRequestQueuer<FAVORITES_QUEUE_CONFIG> testQueuer;
        private readonly TestServer testServer;
        private readonly HttpClient testClient;

        public FavoritesControllerSpec()
        {
            var queueConfig = JsonConvert.SerializeObject(
                new LIKES_QUEUE_CONFIG
                {
                    TopicName = "test",
                    ProducerSettings = new Dictionary<string, object>
                    {
                        ["bootstrap.servers"] = "test:123"
                    }
                });
            this.testQueuer = Substitute.For<IRequestQueuer<FAVORITES_QUEUE_CONFIG>>();
            this.testServer = new TestServer(new WebHostBuilder()
                .ConfigureServices(x => x.AddAutofac())
                .ConfigureTestServices(services => services
                    .AddSingleton<IRequestQueuer<FAVORITES_QUEUE_CONFIG>>(this.testQueuer))
                .UseSetting(nameof(LIKES_QUEUE_CONFIG), queueConfig)
                .UseSetting(nameof(FAVORITES_QUEUE_CONFIG), queueConfig)
                .UseSetting(nameof(CorsConfig.ALLOWED_ORIGINS), JsonConvert.SerializeObject(new CorsConfig
                {
                    ALLOWED_ORIGINS = "localhost"
                }))
                .UseStartup<StartupStub>());
            this.testClient = this.testServer.CreateClient();
            this.testClient.DefaultRequestHeaders.Add("api-version", "1.0");
            this.testClient.DefaultRequestHeaders.Add("schema-version", SchemaVersion.Latest.ToString());
        }

        public class PublishSpec : FavoritesControllerSpec
        {
            private readonly AddToFavoritesRequest correctRequest = new AddToFavoritesRequest
            {
                AggregateId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                ObjectType = "test"
            };

            [It(nameof(FavoritesController.Add))]
            public async Task Should_successfuly_process_correct_request()
            {
                var json = JsonConvert.SerializeObject(this.correctRequest);
                HttpContent jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                var result = await this.testClient.PostAsync("favorites/add", jsonContent);

                result.StatusCode.Should().Be(HttpStatusCode.Accepted, await result.Content.ReadAsStringAsync());

                await this.testQueuer.Received(1).QueueRequest(
                    Arg.Is<DomainRequest>(x => x.DeserializerType == typeof(AddToFavoritesRequestDeserializer_v1)),
                    Arg.Any<UserId>());
            }

            [It(nameof(FavoritesController.Add))]
            public async Task Should_return_BadRequest_response_when_request_is_incorrect()
            {
                var json = JsonConvert.SerializeObject(new AddToFavoritesRequest());
                HttpContent jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                var result = await this.testClient.PostAsync("favorites/add", jsonContent);

                result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }

            [It(nameof(FavoritesController.Add))]
            public async Task Should_return_BadRequest_response_when_SchemaVersion_is_missing()
            {
                this.testClient.DefaultRequestHeaders.Remove("schema-version");
                var json = JsonConvert.SerializeObject(this.correctRequest);
                HttpContent jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                var result = await this.testClient.PostAsync("favorites/add", jsonContent);
                result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        public class RemoveSpec : FavoritesControllerSpec
        {
            private readonly RemoveFromFavoritesRequest correctRequest = new RemoveFromFavoritesRequest
            {
                AggregateId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                ObjectType = "test"
            };

            [It(nameof(FavoritesController.Remove))]
            public async Task Should_successfuly_process_correct_request()
            {
                var json = JsonConvert.SerializeObject(this.correctRequest);
                HttpContent jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                var result = await this.testClient.PostAsync("favorites/remove", jsonContent);

                result.StatusCode.Should().Be(HttpStatusCode.Accepted, await result.Content.ReadAsStringAsync());

                await this.testQueuer.Received(1).QueueRequest(
                    Arg.Is<DomainRequest>(x => x.DeserializerType == typeof(RemoveFromFavoritesRequestDeserializer_v1)),
                    Arg.Any<UserId>());
            }

            [It(nameof(FavoritesController.Remove))]
            public async Task Should_return_BadRequest_response_when_request_is_incorrect()
            {
                var json = JsonConvert.SerializeObject(new RemoveFromFavoritesRequest());
                HttpContent jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                var result = await this.testClient.PostAsync("favorites/remove", jsonContent);

                result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }

            [It(nameof(FavoritesController.Remove))]
            public async Task Should_return_BadRequest_response_when_SchemaVersion_is_missing()
            {
                this.testClient.DefaultRequestHeaders.Remove("schema-version");
                var json = JsonConvert.SerializeObject(this.correctRequest);
                HttpContent jsonContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                var result = await this.testClient.PostAsync("favorites/remove", jsonContent);
                result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }
    }
}
