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
using EBay.AccountManagement.Generated.AccountV1;
using EBay.AccountManagement.Generated.AccountV2;
using EBay.AccountManagement.Generated.Finances;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace EBay.AccountManagement;

/// <summary>
/// Entry point for the Account Management APIs. The client owns its HTTP connections:
/// create one per application (or register it with <c>AddAccountManagementSdk</c>, which adds it
/// as a singleton) and dispose it on shutdown. Reach the individual APIs through its properties.
/// </summary>
public class AccountManagementClient : IDisposable
{
    private readonly HttpClient[] _ownedClients;

    public AccountV1Client AccountV1 { get; private set; } = null!;
    public AccountV2Client AccountV2 { get; private set; } = null!;
    public FinancesClient Finances { get; private set; } = null!;

    public AccountGraphQLClient GraphQL { get; }

    /// <summary>Creates a client with its own HTTP connections.</summary>
    public AccountManagementClient(AccountManagementConfiguration config)
        : this(config, CreateDefaultPrimaryHandler, disposeHandlers: true)
    {
    }

    /// <summary>
    /// Creates a client that sends every request through <paramref name="primaryHandler"/>, for example to
    /// configure a proxy or client certificates, or to substitute a test double. The handler is shared by
    /// all APIs and is not disposed by this client.
    /// </summary>
    public AccountManagementClient(AccountManagementConfiguration config, HttpMessageHandler primaryHandler)
        : this(config, RequireHandler(primaryHandler), disposeHandlers: false)
    {
    }

    private AccountManagementClient(
        AccountManagementConfiguration config,
        Func<HttpMessageHandler> primaryHandler,
        bool disposeHandlers)
    {
        config.ValidateTrustedHosts();
        var tokenContext = new AsyncLocalTokenContext();
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        var owned = new List<HttpClient>();
        HttpClient Build(DelegatingHandler logging, string baseUrl)
        {
            logging.InnerHandler = primaryHandler();
            var client = new HttpClient(logging, disposeHandlers)
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = config.HttpTimeout
            };
            owned.Add(client);
            return client;
        }
        InitializeRestClients(auth, config, tokenContext, endpoint => Build(new DiagnosticLoggingHandler(config), endpoint.BaseUrl));
        var graphQLHttp = Build(new GraphQLHttpHandler(config), config.GraphQLBaseUrl);
        GraphQL = new AccountGraphQLClient(
            new GraphQLExecutor(graphQLHttp, config, tokenContext),
            new DynamicGraphQLClient(graphQLHttp, config, tokenContext),
            tokenContext);
        _ownedClients = [.. owned];
    }

    private static HttpMessageHandler CreateDefaultPrimaryHandler() =>
#if NETFRAMEWORK
        // .NET Framework defaults to 2 connections per host outside ASP.NET.
        new HttpClientHandler { MaxConnectionsPerServer = 64 };
#else
        new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) };
#endif

    private static Func<HttpMessageHandler> RequireHandler(HttpMessageHandler primaryHandler)
    {
#if NETFRAMEWORK
        if (primaryHandler is null)
        {
            throw new ArgumentNullException(nameof(primaryHandler));
        }
#else
        ArgumentNullException.ThrowIfNull(primaryHandler);
#endif
        return () => primaryHandler;
    }

    internal static IReadOnlyList<RestEndpoint> RestEndpoints(AccountManagementConfiguration config) =>
    [
        new RestEndpoint("account-v1", config.RestBaseUrl("account-v1")),
        new RestEndpoint("account-v2", config.RestBaseUrl("account-v2")),
        new RestEndpoint("finances", config.RestBaseUrl("finances"))
    ];

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

internal sealed record RestEndpoint(string ApiId, string BaseUrl);

internal delegate HttpClient RestHttpClientProvider(RestEndpoint endpoint);
