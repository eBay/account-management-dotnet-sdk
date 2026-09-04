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
using EBay.AccountManagement.Configuration;
using EBay.AccountManagement.Extensions;
using EBay.AccountManagement.Generated.GraphQL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions.Authentication;

namespace EBay.AccountManagement;

public partial class AccountManagementClient : IDisposable
{
    internal const string AccountV1ClientName = "AccountManagementClient.AccountV1";
    internal const string AccountV2ClientName = "AccountManagementClient.AccountV2";
    internal const string FinancesClientName = "AccountManagementClient.Finances";

    private readonly HttpClient[] _ownedClients;

    public AccountGraphQLClient GraphQL { get; }

    /// <summary>
    /// DI-friendly constructor: each HttpClient is a named client from IHttpClientFactory
    /// with DiagnosticLoggingHandler and BaseAddress already configured.
    /// </summary>
    internal AccountManagementClient(
        AccountManagementConfiguration config,
        HttpClient accountV1Client,
        HttpClient accountV2Client,
        HttpClient financesClient,
        IAccountManagementGraphQLClient ssGqlClient,
        ITokenContext tokenContext)
    {
        _ownedClients = [];

        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        InitializeRestClients(auth, accountV1Client, accountV2Client, financesClient, config, tokenContext);

        GraphQL = new AccountGraphQLClient(
            ssGqlClient,
            new DynamicGraphQLClient(accountV1Client, config, tokenContext),
            tokenContext);
    }

    /// <summary>
    /// Standalone constructor for use outside DI (tests, scripts).
    /// Creates its own HttpClients with DiagnosticLoggingHandler and builds a minimal
    /// internal service provider so that typed GraphQL operations work without requiring
    /// the caller to set up DI.
    /// </summary>
    public AccountManagementClient(AccountManagementConfiguration config)
    {
        var tokenContext = new AsyncLocalTokenContext();
        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));

        var accountV1Client = BuildStandaloneHttpClient(config, config.AccountV1BaseUrl);
        var accountV2Client = BuildStandaloneHttpClient(config, config.AccountV2BaseUrl);
        var financesClient = BuildStandaloneHttpClient(config, config.FinancesBaseUrl);
        _ownedClients = [accountV1Client, accountV2Client, financesClient];

        InitializeRestClients(auth, accountV1Client, accountV2Client, financesClient, config, tokenContext);

        GraphQL = new AccountGraphQLClient(
            BuildStandaloneGqlClient(config, tokenContext),
            new DynamicGraphQLClient(accountV1Client, config, tokenContext),
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

    public void Dispose()
    {
        foreach (var client in _ownedClients)
            client.Dispose();
    }
}
