using EBay.AccountManagement.Clients.GraphQL;
using EBay.AccountManagement.Clients.Rest;
using EBay.AccountManagement.Configuration;
using EBay.AccountManagement.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EBay.AccountManagement.Tests;

public sealed class DependencyInjectionTests
{
    [Fact]
    public void Registers_one_shared_entry_client_and_no_individual_api_clients()
    {
        var services = new ServiceCollection();
        services.AddAccountManagementSdk(new AccountManagementConfiguration { AccessToken = "t" });

        using var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<AccountManagementClient>();

        Assert.Same(client, provider.CreateScope().ServiceProvider.GetRequiredService<AccountManagementClient>());
        Assert.NotNull(client.AccountV1);
        Assert.NotNull(client.GraphQL);
        Assert.Null(provider.GetService<AccountV1Client>());
        Assert.Null(provider.GetService<AccountGraphQLClient>());
    }

    [Fact]
    public void Rejects_a_null_primary_handler()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new AccountManagementClient(new AccountManagementConfiguration(), (HttpMessageHandler)null!));
    }
}
