using System;
using System.Net.Http;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using MassTransit;
using Newtonsoft.Json;

namespace AuthClient
{
    internal class Program
    {
        private static async Task Main()
        {
            const string auth0Domain = @"mova-user-id-test.auth0.com";

            var httpClient = new HttpClient();

            var auth = new AuthenticationApiClient(
                domain: auth0Domain,
                connection: new HttpClientAuthenticationConnection(httpClient: httpClient)
            );

            var token = await auth.GetTokenAsync(request: new ClientCredentialsTokenRequest
            {
                ClientId = "Z7gyil733avwkafs5Aeky3XMmLtBURqy",

                // ReSharper disable StringLiteralTypo
                ClientSecret = "wISWqDR-LN-vjAuux10w-lTYayKsUDrMwfee2QtNBLFrEianzertZjZrUaj26P-n",
                // ReSharper restore StringLiteralTypo

                Audience = "https://mova-user-id-test.auth0.com/api/v2/",
                SigningAlgorithm = JwtSignatureAlgorithm.RS256
            });

            var api = new ManagementApiClient(
                token: token.AccessToken,
                domain: auth0Domain,
                connection: new HttpClientManagementConnection(httpClient: httpClient)
            );

            foreach (
                var u in await api.Users.GetAllAsync(
                    request: new GetUsersRequest(),
                    pagination: new PaginationInfo())
            )
                await api.Users.DeleteAsync(id: u.UserId);


            var users = new[]
            {
                (
                    id: NewId.NextGuid(),
                    email: "vmo@ciklum.com"
                ),
                (
                    id: NewId.NextGuid(),
                    email: $"test-{NewId.NextGuid():N}@test.com"
                )
            };

            foreach (var (id, email) in users)
            {
                var createdUser = await CreateUser(api: api, email: email, id: id)
                    .ConfigureAwait(continueOnCapturedContext: false);
                await Console.Out.WriteLineAsync(value: "created user:")
                    .ConfigureAwait(continueOnCapturedContext: false);
                await Console.Out.WriteLineAsync(value: JsonConvert.SerializeObject(
                        value: createdUser,
                        formatting: Formatting.Indented,
                        settings: new JsonSerializerSettings
                        {
                            DefaultValueHandling = DefaultValueHandling.Ignore
                        }
                    ))
                    .ConfigureAwait(continueOnCapturedContext: false);

                var gotUser = await GetUser(
                        api: api,
                        userId: createdUser.UserId
                        //userId: wspUserId
                    )
                    .ConfigureAwait(continueOnCapturedContext: false);
                await Console.Out.WriteLineAsync(value: "got user:")
                    .ConfigureAwait(continueOnCapturedContext: false);
                await Console.Out.WriteLineAsync(value: JsonConvert.SerializeObject(
                        value: gotUser,
                        formatting: Formatting.Indented,
                        settings: new JsonSerializerSettings
                        {
                            DefaultValueHandling = DefaultValueHandling.Ignore
                        }
                    ))
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        private static async Task<User> CreateUser(ManagementApiClient api, string email, Guid id)
        {
            var request = new UserCreateRequest
            {
                Connection = "Username-Password-Authentication",
                Email = email,
                Password = NewId.NextGuid().ToString(format: "N"),
                AppMetadata = new UserMetadata
                {
                    UserId = id.ToString(format: "N")
                }
            };

            await Console.Out.WriteLineAsync(value: $"pwd: {request.Password}")
                .ConfigureAwait(continueOnCapturedContext: false);


            return await api.Users.CreateAsync(request: request);
        }

        private static Task<User> GetUser(ManagementApiClient api, string userId)
        {
            return api.Users.GetAsync(id: userId);
        }
    }

    public class UserMetadata
    {
        [JsonProperty(propertyName: "debriefit-user-id")]
        public string UserId { get; set; }
    }
}