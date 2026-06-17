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
using EBay.AccountManagement.Generated.GraphQL;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace EBay.AccountManagement;

public sealed class AccountManagementClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly bool _ownsHttpClient;

    public AccountV1Client AccountV1 { get; }
    public AccountV2Client AccountV2 { get; }
    public FinancesClient Finances { get; }
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

        AccountV1 = new AccountV1Client(new AccountV1ApiClient(
            new HttpClientRequestAdapter(auth, httpClient: httpClient)),
            config, tokenContext);

        AccountV2 = new AccountV2Client(new AccountV2ApiClient(
            new HttpClientRequestAdapter(auth, httpClient: httpClient)),
            config, tokenContext);

        Finances = new FinancesClient(new FinancesApiClient(
            new HttpClientRequestAdapter(auth, httpClient: httpClient)),
            config, tokenContext);

        GraphQL = new AccountGraphQLClient(
            ssGqlClient,
            new DynamicGraphQLClient(httpClient, config, tokenContext),
            tokenContext);
    }

    /// <summary>
    /// Standalone constructor for use outside DI (tests, scripts).
    /// Creates its own HttpClient and does not use StrawberryShake DI.
    /// Typed GraphQL operations are unavailable; use GraphQL.ExecuteRawUnsafeAsync instead.
    /// </summary>
    public AccountManagementClient(AccountManagementConfiguration config)
        : this(config, BuildStandaloneHttpClient(config), null!, new AsyncLocalTokenContext())
    {
        _ownsHttpClient = true;
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
