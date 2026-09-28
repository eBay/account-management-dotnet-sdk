# EBay.AccountManagement .NET SDK

A production-ready .NET SDK for eBay Account Management APIs, combining REST and GraphQL clients with a unified facade.

## Features

- **REST API Support**: Account V1, Account V2, and Finances APIs generated with Microsoft Kiota
- **GraphQL Support**: Typed operations for auction/bidding queries plus a dynamic escape hatch
- **OAuth Authentication**: Token refresh handler interface for seamless authentication
- **Per-Call Access Tokens**: Pass access tokens directly per method call for multi-user scenarios
- **Dependency Injection**: First-class support for ASP.NET Core DI
- **Strong Typing**: Internal generated types with public abstraction layer

## Architecture

```
EBay.AccountManagement/
├── Generated/              # Kiota-generated REST clients (internal)
│   ├── AccountV1/
│   ├── AccountV2/
│   └── Finances/
├── Clients/
│   ├── Rest/              # REST client wrappers (public API)
│   │   ├── AccountV1Client.cs
│   │   ├── AccountV2Client.cs
│   │   └── FinancesClient.cs
│   └── GraphQL/           # GraphQL clients
│       ├── AccountGraphQLClient.cs     # Typed operations
│       └── DynamicGraphQLClient.cs    # Escape hatch
├── Configuration/         # SDK configuration and auth
├── Models/               # Public models
├── Exceptions/           # SDK exceptions
└── AccountManagementClient.cs  # Main SDK facade
```

## Installation

```bash
# From source
dotnet pack EBay.AccountManagement/EBay.AccountManagement.csproj
dotnet add reference path/to/EBay.AccountManagement.1.0.0.nupkg

# Or add project reference
dotnet add reference EBay.AccountManagement/EBay.AccountManagement.csproj
```

## Quick Start

### 1. Implement Token Refresh Handler

```csharp
using EBay.AccountManagement.Configuration;

public class MyTokenRefreshHandler : ITokenRefreshHandler
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private string? _accessToken;
    private DateTime _tokenExpiry;

    public MyTokenRefreshHandler(string clientId, string clientSecret)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        // Check if token is still valid
        if (_accessToken != null && DateTime.UtcNow < _tokenExpiry.AddMinutes(-5))
        {
            return _accessToken;
        }

        // Refresh token using eBay OAuth API
        using var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.ebay.com/identity/v1/oauth2/token");
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}")));

        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials"),
            new KeyValuePair<string, string>("scope", "https://api.ebay.com/oauth/api_scope https://api.ebay.com/oauth/api_scope/sell.account")
        });

        var response = await client.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(json);

        _accessToken = tokenResponse.AccessToken;
        _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

        return _accessToken;
    }

    private class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
```

### 2. Configure and Use the SDK

```csharp
using EBay.AccountManagement;
using EBay.AccountManagement.Configuration;

// Create configuration
var config = new AccountManagementConfiguration
{
    TokenRefreshHandler = new MyTokenRefreshHandler("your-client-id", "your-client-secret"),
    AccountV1BaseUrl = "https://api.ebay.com/sell/account/v1",
    AccountV2BaseUrl = "https://api.ebay.com/sell/account/v2",
    FinancesBaseUrl = "https://apiz.ebay.com/sell/finances/v1",
    GraphQLBaseUrl = "https://api.ebay.com/graphql"
};

// Create SDK client
using var client = new AccountManagementClient(config);

// Use REST APIs
var policies = await client.AccountV1.GetFulfillmentPoliciesAsync("EBAY_US");
var privileges = await client.AccountV1.GetPrivilegesAsync();
var userPrefs = await client.AccountV2.GetUserPreferencesAsync();
var transactions = await client.Finances.GetTransactionsAsync(
    filter: "transactionDate:[2024-01-01T00:00:00.000Z..2024-01-31T23:59:59.999Z]",
    limit: 50);

// Use GraphQL APIs
var biddingActivity = await client.GraphQL.GetUserBiddingActivityAsync(
    new UserBiddingActivityInput
    {
        Filter = new BiddingActivityFilter { Limit = 20 }
    });

// Use GraphQL escape hatch for arbitrary queries
var rawResult = await client.GraphQL.ExecuteRawAsync(@"
    query {
      userBiddingActivity(input: {}) {
        auctions {
          itemId
          auctionDataSummary {
            currentPrice { value currency }
          }
        }
      }
    }");
```

### Per-Call Access Tokens

All methods accept an optional `accessToken` parameter for multi-user scenarios:

```csharp
using EBay.AccountManagement;
using EBay.AccountManagement.Configuration;

// No global token needed — pass per call
var config = new AccountManagementConfiguration();
using var client = new AccountManagementClient(config);

// Each call uses the caller's own token
var policies = await client.AccountV1.GetFulfillmentPoliciesAsync("EBAY_US", accessToken: userToken);
var privileges = await client.AccountV1.GetPrivilegesAsync(accessToken: userToken);
var transactions = await client.Finances.GetTransactionsAsync(accessToken: userToken);
```

### 3. Dependency Injection (ASP.NET Core)

```csharp
using EBay.AccountManagement.Extensions;
using EBay.AccountManagement.Configuration;

// In Program.cs or Startup.cs
builder.Services.AddSingleton<ITokenRefreshHandler, MyTokenRefreshHandler>();

builder.Services.AddAccountManagementSdk(config =>
{
    config.TokenRefreshHandler = sp.GetRequiredService<ITokenRefreshHandler>();
});

// Inject the SDK or sub-clients
public class MyController : ControllerBase
{
    private readonly AccountManagementClient _sdk;
    private readonly AccountV1Client _accountV1;

    public MyController(AccountManagementClient sdk, AccountV1Client accountV1)
    {
        _sdk = sdk;
        _accountV1 = accountV1;
    }

    [HttpGet("policies")]
    public async Task<IActionResult> GetPolicies()
    {
        var policies = await _accountV1.GetFulfillmentPoliciesAsync("EBAY_US");
        return Ok(policies);
    }
}
```

## REST API Examples

### Account V1 API

```csharp
// Get fulfillment policies
var policies = await client.AccountV1.GetFulfillmentPoliciesAsync("EBAY_US");

// Get seller privileges
var privileges = await client.AccountV1.GetPrivilegesAsync();

// Get opted-in programs
var programs = await client.AccountV1.GetOptedInProgramsAsync();
```

### Account V2 API

```csharp
// Get user preferences
var userPrefs = await client.AccountV2.GetUserPreferencesAsync();

// Get payout settings
var payoutSettings = await client.AccountV2.GetPayoutSettingsAsync();
```

### Finances API

```csharp
// Get transactions with filter
var transactions = await client.Finances.GetTransactionsAsync(
    filter: "transactionType:{SALE}",
    limit: 100,
    offset: "0");

// Get payouts
var payouts = await client.Finances.GetPayoutsAsync(
    filter: "payoutDate:[2024-01-01T00:00:00.000Z..]",
    limit: 50);
```

## GraphQL API Examples

### Typed Operations

```csharp
using EBay.AccountManagement.Models.GraphQL;

// Get user bidding activity
var userActivity = await client.GraphQL.GetUserBiddingActivityAsync(
    new UserBiddingActivityInput
    {
        Filter = new BiddingActivityFilter
        {
            Limit = 20,
            Offset = 0
        }
    });

foreach (var auction in userActivity?.Auctions ?? Array.Empty<AuctionItem>())
{
    Console.WriteLine($"Item {auction.ItemId}: " +
                      $"{auction.AuctionDataSummary?.CurrentPrice?.Value} " +
                      $"{auction.AuctionDataSummary?.CurrentPrice?.Currency}");
}
```

### Dynamic Queries (Escape Hatch)

```csharp
// Execute any arbitrary GraphQL query
var result = await client.GraphQL.ExecuteRawAsync(
    "query { userBiddingActivity(input: {}) { auctions { itemId } } }");

// Parse the JSON response
var data = result.RootElement.GetProperty("data");
// ... process the response
```

## Regenerating the SDK

When OpenAPI specs or GraphQL schemas change:

```bash
# Run the regeneration script
./generate.sh

# Or manually:
dotnet tool restore
dotnet kiota generate -l csharp -d specs/openapi/account-v1.yaml ...
dotnet build EBay.AccountManagement/EBay.AccountManagement.csproj
```

## Error Handling

```csharp
using EBay.AccountManagement.Exceptions;

try
{
    var result = await client.GraphQL.GetUserBiddingActivityAsync(input);
}
catch (GraphQLException ex)
{
    foreach (var error in ex.Errors)
    {
        Console.WriteLine($"GraphQL Error: {error.Message}");
        if (error.Path != null)
        {
            Console.WriteLine($"Path: {string.Join(" > ", error.Path)}");
        }
    }
}
catch (SdkException ex)
{
    Console.WriteLine($"SDK Error: {ex.Message}");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"HTTP Error: {ex.Message}");
}
```

## Configuration Options

| Property | Default | Description |
|----------|---------|-------------|
| `AccountV1BaseUrl` | `https://api.ebay.com/sell/account/v1` | Account V1 API base URL |
| `AccountV2BaseUrl` | `https://api.ebay.com/sell/account/v2` | Account V2 API base URL |
| `FinancesBaseUrl` | `https://apiz.ebay.com/sell/finances/v1` | Finances API base URL |
| `GraphQLBaseUrl` | `https://api.ebay.com/graphql` | GraphQL API endpoint |
| `AccessToken` | `null` | Static access token (fallback) |
| `TokenRefreshHandler` | `null` | OAuth token refresh handler |

## Project Structure

- **Generated/**: Kiota-generated REST clients (internal, committed to git)
- **Clients/Rest/**: Hand-crafted wrappers exposing public API
- **Clients/GraphQL/**: GraphQL typed client + dynamic escape hatch
- **Configuration/**: SDK config, auth providers
- **Models/**: Public models and GraphQL types
- **Exceptions/**: SDK exceptions
- **Extensions/**: DI extension methods

## Development

### Prerequisites

- .NET 10 SDK, plus the .NET 8 and .NET 9 SDKs so every target framework can build
- Kiota CLI tool (auto-installed via dotnet tools)

### Building

```bash
dotnet build EBay.AccountManagement/EBay.AccountManagement.csproj
```

### Packaging

```bash
dotnet pack EBay.AccountManagement/EBay.AccountManagement.csproj
```

## Dependencies

- **Microsoft.Kiota.Bundle**: REST client generation and runtime
- **Microsoft.Extensions.DependencyInjection.Abstractions**: DI support
- **Microsoft.Extensions.Http**: HttpClient factory integration

## License

[Apache 2.0](LICENSE) © 2026 eBay Inc.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run `./generate.sh` if specs changed
5. Ensure `dotnet build` succeeds
6. Submit a pull request
