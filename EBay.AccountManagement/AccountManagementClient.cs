// Copyright 2026 eBay Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using EBay.AccountManagement.Clients.GraphQL;
using EBay.AccountManagement.Clients.Rest;
using EBay.AccountManagement.Configuration;
using EBay.AccountManagement.Extensions;
using EBay.AccountManagement.Generated.AccountV1;
using EBay.AccountManagement.Generated.AccountV2;
using EBay.AccountManagement.Generated.Finances;
using EBay.AccountManagement.Generated.GraphQL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace EBay.AccountManagement;

public class AccountManagementClient : IDisposable
{
    internal const string AccountV1ClientName = "AccountManagementClient.AccountV1";
    internal const string AccountV2ClientName = "AccountManagementClient.AccountV2";
    internal const string FinancesClientName = "AccountManagementClient.Finances";
    internal const string GraphQLClientName = "AccountManagementGraphQLClient";

    private readonly HttpClient[] _ownedClients;

    public AccountV1Client AccountV1 { get; private set; } = null!;
    public AccountV2Client AccountV2 { get; private set; } = null!;
    public FinancesClient Finances { get; private set; } = null!;

    public AccountGraphQLClient GraphQL { get; }

    /// <summary>
    /// DI-friendly constructor. Named HttpClients come from <see cref="IHttpClientFactory"/>.
    /// </summary>
    internal AccountManagementClient(
        AccountManagementConfiguration config,
        IHttpClientFactory factory,
        IAccountManagementGraphQLClient graphQLClient,
        ITokenContext tokenContext)
    {
        _ownedClients = [];
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        InitializeRestClients(auth, config, tokenContext, endpoint => factory.CreateClient(endpoint.HttpClientName));
        GraphQL = new AccountGraphQLClient(
            graphQLClient,
            new DynamicGraphQLClient(factory.CreateClient(GraphQLClientName), config, tokenContext),
            tokenContext);
    }

    /// <summary>
    /// Standalone constructor for use outside DI (tests, scripts).
    /// Creates its own HttpClients with DiagnosticLoggingHandler.
    /// Typed GraphQL operations are unavailable; use GraphQL.ExecuteRawUnsafeAsync instead.
    /// </summary>
    public AccountManagementClient(AccountManagementConfiguration config)
    {
        var tokenContext = new AsyncLocalTokenContext();
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        var owned = new List<HttpClient>();
        InitializeRestClients(auth, config, tokenContext, endpoint =>
        {
            var client = BuildStandaloneHttpClient(config, endpoint.BaseUrl);
            owned.Add(client);
            return client;
        });
        var graphQLHttp = BuildStandaloneHttpClient(config, config.GraphQLBaseUrl);
        owned.Add(graphQLHttp);
        _ownedClients = [.. owned];
        GraphQL = new AccountGraphQLClient(
            BuildStandaloneGqlClient(config, tokenContext),
            new DynamicGraphQLClient(graphQLHttp, config, tokenContext),
            tokenContext);
    }

    private static IAccountManagementGraphQLClient BuildStandaloneGqlClient(
        AccountManagementConfiguration config, ITokenContext tokenContext)
    {
        var services = new ServiceCollection();
        services.AddAccountManagementSdk(config);
        services.AddSingleton<ITokenContext>(tokenContext);
        return services.BuildServiceProvider().GetRequiredService<IAccountManagementGraphQLClient>();
    }

    private static HttpClient BuildStandaloneHttpClient(AccountManagementConfiguration config, string baseUrl)
    {
        var handler = new DiagnosticLoggingHandler(config)
        {
            InnerHandler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) }
        };
        return new HttpClient(handler) { BaseAddress = new Uri(baseUrl), Timeout = config.HttpTimeout };
    }

    internal static IReadOnlyList<(string ApiId, string HttpClientName)> RestEndpointNames { get; } =
    [
        ("account-v1", AccountV1ClientName),
        ("account-v2", AccountV2ClientName),
        ("finances", FinancesClientName)
    ];

    internal static IReadOnlyList<RestEndpoint> RestEndpoints(AccountManagementConfiguration config) =>
        [.. RestEndpointNames.Select(name => new RestEndpoint(name.ApiId, name.HttpClientName, config.RestBaseUrl(name.ApiId)))];

    private void InitializeRestClients(
        IAuthenticationProvider auth,
        AccountManagementConfiguration config,
        ITokenContext tokenContext,
        RestHttpClientProvider httpClientFor)
    {
        var endpoints = RestEndpoints(config);
        var accountV1Adapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "account-v1")));
        AccountV1 = new AccountV1Client(new AccountV1ApiClient(accountV1Adapter), config, tokenContext, accountV1Adapter);
        var accountV2Adapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "account-v2")));
        AccountV2 = new AccountV2Client(new AccountV2ApiClient(accountV2Adapter), config, tokenContext, accountV2Adapter);
        var financesAdapter = new HttpClientRequestAdapter(auth, httpClient: httpClientFor(endpoints.First(endpoint => endpoint.ApiId == "finances")));
        Finances = new FinancesClient(new FinancesApiClient(financesAdapter), config, tokenContext, financesAdapter);
    }

    public void Dispose()
    {
        foreach (var client in _ownedClients)
        {
            client.Dispose();
        }
        GC.SuppressFinalize(this);
    }
}

internal sealed record RestEndpoint(string ApiId, string HttpClientName, string BaseUrl);

internal delegate HttpClient RestHttpClientProvider(RestEndpoint endpoint);
