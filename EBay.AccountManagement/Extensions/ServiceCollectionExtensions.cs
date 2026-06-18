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
using EBay.AccountManagement.Generated.GraphQL;
using Microsoft.Extensions.DependencyInjection;

namespace EBay.AccountManagement.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountManagementSdk(
        this IServiceCollection services,
        AccountManagementConfiguration config)
    {
        services.AddSingleton(config);
        services.AddSingleton<ITokenContext, AsyncLocalTokenContext>();

        // StrawberryShake: register generated client
        services.AddAccountManagementGraphQLClient();

        // Wire auth into the named HTTP client that StrawberryShake uses
        services.AddTransient<BearerTokenHandler>();
        services.AddTransient(_ => new GraphQLHttpHandler(config));
        services
            .AddHttpClient("AccountManagementGraphQLClient", client =>
                client.BaseAddress = new Uri(config.GraphQLBaseUrl))
            .ConfigureHttpClient(c => c.Timeout = config.HttpTimeout)
            .AddHttpMessageHandler<BearerTokenHandler>()
            .AddHttpMessageHandler(_ => new GraphQLHttpHandler(config));

        // Named REST client: DiagnosticLoggingHandler wired in so DI policies are honoured
        services
            .AddHttpClient(nameof(AccountManagementClient))
            .ConfigureHttpClient(c => c.Timeout = config.HttpTimeout)
            .AddHttpMessageHandler(sp =>
                new DiagnosticLoggingHandler(sp.GetRequiredService<AccountManagementConfiguration>()));

        // Dynamic escape hatch
        services.AddScoped<DynamicGraphQLClient>(sp =>
            new DynamicGraphQLClient(
                sp.GetRequiredService<IHttpClientFactory>().CreateClient(),
                sp.GetRequiredService<AccountManagementConfiguration>(),
                sp.GetRequiredService<ITokenContext>()));

        // Register facade
        services.AddScoped<AccountManagementClient>(sp =>
            new AccountManagementClient(
                sp.GetRequiredService<AccountManagementConfiguration>(),
                sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(AccountManagementClient)),
                sp.GetRequiredService<IAccountManagementGraphQLClient>(),
                sp.GetRequiredService<ITokenContext>()));

        // Register sub-clients for direct injection
        services.AddScoped(sp => sp.GetRequiredService<AccountManagementClient>().AccountV1);
        services.AddScoped(sp => sp.GetRequiredService<AccountManagementClient>().AccountV2);
        services.AddScoped(sp => sp.GetRequiredService<AccountManagementClient>().Finances);
        services.AddScoped(sp => sp.GetRequiredService<AccountManagementClient>().GraphQL);

        return services;
    }
}
