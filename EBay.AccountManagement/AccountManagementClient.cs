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
    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;

    public AccountGraphQLClient GraphQL { get; }

    /// <summary>
    /// DI-friendly constructor: StrawberryShake client injected by the container.
    /// </summary>
    internal AccountManagementClient(
        AccountManagementConfiguration config,
        HttpClient httpClient,
        IAccountManagementGraphQLClient ssGqlClient,
        ITokenContext tokenContext)
    {
        _httpClient = httpClient;
        _ownsHttpClient = false;

        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        InitializeRestClients(auth, httpClient, config, tokenContext);

        GraphQL = new AccountGraphQLClient(
            ssGqlClient,
            new DynamicGraphQLClient(httpClient, config, tokenContext),
            tokenContext);
    }

    /// <summary>
    /// Standalone constructor for use outside DI (tests, scripts).
    /// Creates its own HttpClient and builds a minimal internal service provider so that
    /// typed GraphQL operations work without requiring the caller to set up DI.
    /// </summary>
    public AccountManagementClient(AccountManagementConfiguration config)
    {
        var tokenContext = new AsyncLocalTokenContext();
        var httpClient = BuildStandaloneHttpClient(config);
        _httpClient = httpClient;
        _ownsHttpClient = true;

        var auth = new BaseBearerTokenAuthenticationProvider(new KiotaAuthProvider(config, tokenContext));
        InitializeRestClients(auth, httpClient, config, tokenContext);

        GraphQL = new AccountGraphQLClient(
            BuildStandaloneGqlClient(config, tokenContext),
            new DynamicGraphQLClient(httpClient, config, tokenContext),
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

    private static HttpClient BuildStandaloneHttpClient(AccountManagementConfiguration config)
    {
        var handler = new DiagnosticLoggingHandler(config)
        {
            InnerHandler = new SocketsHttpHandler { PooledConnectionLifetime = TimeSpan.FromMinutes(2) }
        };
        return new HttpClient(handler) { Timeout = config.HttpTimeout };
    }

    public void Dispose()
    {
        if (_ownsHttpClient)
            _httpClient.Dispose();
    }
}
