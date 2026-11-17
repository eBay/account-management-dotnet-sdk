# eBay Account Management SDK for .NET

Manage business policies, custom policies, rate tables, sales tax, program opt-ins and seller funds, payouts and transactions, plus the user's watch list and bidding activity. This SDK wraps the eBay Account API v1, Account API v2, Finances API and eBay GraphQL behind one client, `AccountManagementClient`.

- **Target frameworks:** .NET Framework 4.7.2, .NET 8, .NET 9 and .NET 10
- **C#:** usable from C# 7.3 and later

## Installation

The package `EBay.AccountManagement` is not on nuget.org yet. Reference the project, or build the package and add it to a local feed:

```bash
dotnet pack EBay.AccountManagement -c Release -o artifacts/packages
```

## Quick start

```csharp
using EBay.AccountManagement;
using EBay.AccountManagement.Configuration;

var config = new AccountManagementConfiguration
{
    // A user or application access token from your own OAuth flow (see "Authentication").
    AccessToken = Environment.GetEnvironmentVariable("EBAY_ACCESS_TOKEN")
};

using var client = new AccountManagementClient(config);

var privileges = await client.AccountV1.GetPrivilegesAsync();
Console.WriteLine($"Registration completed: {privileges?.SellerRegistrationCompleted}");
```

The GraphQL API is on the same client:

```csharp
using EBay.AccountManagement.Generated.GraphQL;

var result = await client.GraphQL.UserWatchListAsync(new UserWatchListInput
{
    Pagination = new PageInfoInput { MaxPageSize = 10 }
});

Console.WriteLine($"Watching {result?.WatchListCount} of {result?.WatchListMaximum} items");
foreach (var entry in result?.Items ?? [])
{
    Console.WriteLine($"{entry.Item.ItemId} since {entry.StartedWatchingAt:d}");
}
```

## APIs

| `AccountManagementClient` property | API | Default base URL | Access token |
|---|---|---|---|
| `AccountV1` | Account API v1 | `https://api.ebay.com/sell/account/v1` | User |
| `AccountV2` | Account API v2 | `https://api.ebay.com/sell/account/v2` | User |
| `Finances` | Finances API | `https://apiz.ebay.com/sell/finances/v1` | User |
| `GraphQL` | eBay GraphQL | `https://api.ebay.com/graphql` | See eBay's GraphQL docs |

"Access token" is what eBay's OpenAPI spec declares: an application token (client credentials) or a user token (authorization code). Scopes are listed per API below.

<details>
<summary><b>AccountV1</b>: Account API v1, 37 operations</summary>

Scopes: `https://api.ebay.com/oauth/api_scope/sell.account`, `https://api.ebay.com/oauth/api_scope/sell.account.readonly`

| Method | Description |
|---|---|
| `GetCustomPoliciesAsync` | This method retrieves the list of custom policies defined for a seller's account. |
| `CreateCustomPolicyAsync` | This method creates a new custom policy that specifies the seller's terms for complying with local governmental regulations. |
| `GetCustomPolicyAsync` | This method retrieves the custom policy specified by the custom_policy_id path parameter. |
| `UpdateCustomPolicyAsync` | This method updates an existing custom policy specified by the custom_policy_id path parameter. |
| `CreateFulfillmentPolicyAsync` | This method creates a new fulfillment policy for an eBay marketplace where the policy encapsulates seller's terms for fulfilling item purchases. |
| `GetFulfillmentPolicyAsync` | This method retrieves the complete details of a fulfillment policy. |
| `UpdateFulfillmentPolicyAsync` | This method updates an existing fulfillment policy. |
| `DeleteFulfillmentPolicyAsync` | This method deletes a fulfillment policy. |
| `GetFulfillmentPoliciesAsync` | This method retrieves all the fulfillment policies configured for the marketplace you specify using the marketplace_id query parameter. |
| `GetFulfillmentPolicyByNameAsync` | This method retrieves the details for a specific fulfillment policy. |
| `GetPaymentPoliciesAsync` | This method retrieves all the payment business policies configured for the marketplace you specify using the marketplace_id query parameter. |
| `CreatePaymentPolicyAsync` | This method creates a new payment policy where the policy encapsulates seller's terms for order payments. |
| `GetPaymentPolicyAsync` | This method retrieves the complete details of a payment policy. |
| `UpdatePaymentPolicyAsync` | This method updates an existing payment policy. |
| `DeletePaymentPolicyAsync` | This method deletes a payment policy. |
| `GetPaymentPolicyByNameAsync` | This method retrieves the details of a specific payment policy. |
| `GetPaymentsProgramAsync` | This method returns whether or not the user is opted-in to the specified payments program. |
| `GetPaymentsProgramOnboardingAsync` | This method retrieves a seller's onboarding status for a payments program for a specified marketplace. |
| `GetPrivilegesAsync` | This method retrieves the seller's current set of privileges, including whether or not the seller's eBay registration has been completed, as well as the details of their site-wide sellingLimit (the amount and quantity th |
| `GetOptedInProgramsAsync` | This method gets a list of the seller programs that the seller has opted-in to. |
| `OptInToProgramAsync` | This method opts the seller in to an eBay seller program. |
| `OptOutOfProgramAsync` | This method opts the seller out of a seller program in which they are currently opted in to. |
| `GetRateTablesAsync` | This method retrieves a seller's shipping rate tables for the country specified in the country_code query parameter. |
| `GetReturnPoliciesAsync` | This method retrieves all the return policies configured for the marketplace you specify using the marketplace_id query parameter. |
| `CreateReturnPolicyAsync` | This method creates a new return policy where the policy encapsulates seller's terms for returning items. |
| `GetReturnPolicyAsync` | This method retrieves the complete details of the return policy specified by the returnPolicyId path parameter. |
| `UpdateReturnPolicyAsync` | This method updates an existing return policy. |
| `DeleteReturnPolicyAsync` | This method deletes a return policy. |
| `GetReturnPolicyByNameAsync` | This method retrieves the details of a specific return policy. |
| `BulkCreateOrReplaceSalesTaxAsync` | This method creates or updates multiple sales-tax table entries. |
| `GetSalesTaxAsync` | This call retrieves the current sales-tax table entry for a specific tax jurisdiction. |
| `CreateOrReplaceSalesTaxAsync` | This method creates or updates a sales-tax table entry for a jurisdiction. |
| `DeleteSalesTaxAsync` | This call deletes a sales-tax table entry for a jurisdiction. |
| `GetSalesTaxesAsync` | Use this call to retrieve all sales tax table entries that the seller has defined for a specific country. |
| `GetSubscriptionAsync` | This method retrieves a list of subscriptions associated with the seller account. |
| `GetKYCAsync` |  |
| `GetAdvertisingEligibilityAsync` | This method allows developers to check the seller eligibility status for eBay advertising programs. |

</details>

<details>
<summary><b>AccountV2</b>: Account API v2, 14 operations</summary>

Scopes: `https://api.ebay.com/oauth/api_scope/sell.account`, `https://api.ebay.com/oauth/api_scope/sell.account.readonly`, `https://api.ebay.com/oauth/api_scope/sell.finances`

| Method | Description |
|---|---|
| `GetRateTableAsync` | Retrieves details of a specific shipping rate table. |
| `UpdateShippingCostAsync` | Updates one or more shipping rates for a specific shipping rate table. |
| `GetPayoutSettingsAsync` | Retrieves payout percentages and unique IDs for accounts configured to receive seller payouts. |
| `UpdatePayoutPercentageAsync` | Updates the split-payout percentage for two payout instruments for sellers in mainland China. |
| `CreateCalculatedShippingRulesAsync` | Creates calculated shipping rules that determine combined shipping costs based on item attributes, such as weight and dimensions, for the seller's account. |
| `CreateFlatShippingRulesAsync` | Creates flat-rate rules that apply standard combined shipping costs for a seller's listings. |
| `CreatePromotionalShippingRuleAsync` | Creates promotional shipping rules, such as discounts or free-shipping thresholds. |
| `GetCombinedShippingRulesAsync` | Retrieves all combined shipping rule configurations defined for the seller. |
| `UpdateCalculatedShippingRulesAsync` | Updates previously defined calculated shipping rules. |
| `UpdateCombinedPaymentsAsync` | Updates combined payment settings that determine how unpaid orders can be merged. |
| `UpdateFlatShippingRulesAsync` | Updates existing flat-rate shipping rules. |
| `UpdatePromotionalShippingRuleAsync` | Updates a promotional shipping rule to adjust discount thresholds, eligibility criteria, or duration for the seller. |
| `GetUserPreferencesAsync` | Retrieves the seller's preferences for a specific eBay marketplace. |
| `SetUserPreferencesAsync` | Modifies one or more preferences for a seller on a specific marketplace. |

</details>

<details>
<summary><b>Finances</b>: Finances API, 11 operations</summary>

Scopes: `eBayUser`, `https://api.ebay.com/oauth/api_scope/sell.finances`, `https://api.ebay.com/oauth/api_scope/sell.finances.earnings.read`

> **Note:** Due to EU & UK Payments regulatory requirements, an additional security verification via Digital Signatures is required for certain API calls that are made on behalf of EU/UK sellers, including all Finances API methods. Please refer to Digital Signatures for APIs to learn more on the impacted APIs and the process to create signatures to be included in the HTTP payload.

> **Note:** The Finances API does not support Team Access. Financial information, such as payouts or transactions, is only returned for the user that makes the call. You cannot use any of the methods in this API to return financial information for another user.

| Method | Description |
|---|---|
| `GetOrderEarningsAsync` | This method returns detailed order-level financial data for each order associated with a seller account. |
| `GetOrderEarningsByIdAsync` | This method returns detailed order-level financial data including order earnings, gross amount, expenses, and refunds. |
| `GetOrderEarningsSummaryAsync` | This method returns a summarized view of order earnings information for one or more orders associated with a seller account. |
| `GetPayoutAsync` | This method retrieves details on a specific seller payout. |
| `GetPayoutsAsync` | This method is used to retrieve the details of one or more seller payouts. |
| `GetPayoutSummaryAsync` | This method is used to retrieve cumulative values for payouts in a particular state, or all states. |
| `GetSellerFundsSummaryAsync` | This method retrieves all pending funds that have not yet been distributed through a seller payout. |
| `GetTransactionsAsync` | The getTransactions method allows a seller to retrieve information about one or more of their monetary transactions. |
| `GetTransactionSummaryAsync` | The getTransactionSummary method retrieves cumulative information for monetary transactions. |
| `GetTransferAsync` | This method retrieves detailed information regarding a TRANSFER transaction type. |
| `GetBillingActivitiesAsync` | This method retrieves filtered billing activities of the seller. |

</details>

<details>
<summary><b>GraphQL</b>: eBay GraphQL, 11 operations</summary>

Scopes (any operation may need any of these): `https://api.ebay.com/oauth/api_scope/buy.watchlist`, `https://api.ebay.com/oauth/api_scope/commerce.usernote`

| Method | Description |
|---|---|
| `AddToWatchListAsync` | The **addToWatchList** mutation can be used to add one or more items to a user's Watchlist. |
| `CreateOrReplaceUserNoteAsync` | This mutation is used to create a new user note or replace an existing user note for an item or order line item. |
| `DeleteUserNoteAsync` | This mutation is used to delete an existing user note for an item or order line item. |
| `ItemBiddingActivityAsync` | Query to return auction and bid information for a specified listing. |
| `RemoveAllFromWatchListAsync` | The **removeAllFromWatchList** mutation can be used to remove all items from the user's Watchlist. |
| `RemoveFromWatchListAsync` | The **removeFromWatchList** mutation can be used to remove one or more items from a user's Watchlist. |
| `UserAsync` | Retrieves the user information for the current authenticated user. |
| `UserBiddingActivityAsync` | Query to return a list of all items on which the user is currently bidding, as well as those that they have previously won or purchased. |
| `UserCustomListsAsync` | The **userCustomLists** query can be used to retrieve one, multiple, or all of a user's custom Watchlists. |
| `UserWatchListAsync` | The **userWatchList** query can be used to retrieve a user's default Watchlist. |
| `ExecuteRawUnsafeAsync` | Runs any GraphQL query or mutation and returns the raw JSON response. |

</details>

## GraphQL

`client.GraphQL` has a typed method per GraphQL operation the SDK ships (see the table above). Inputs are classes with settable properties; only the properties you set are sent, so leaving a property unset is different from setting it to `null`. Results are interfaces named after their path in the query, and GraphQL unions surface as one interface per possible type: use `is`/`as` to pick the concrete type, as in the quick start.

For anything else, `ExecuteRawUnsafeAsync` sends any query and returns the response as a `JsonDocument`:

```csharp
using var document = await client.GraphQL.ExecuteRawUnsafeAsync("query { __typename }");
Console.WriteLine(document.RootElement.GetRawText());
```

## Authentication

The SDK does not obtain or refresh tokens: get them with your own OAuth flow ([eBay OAuth guide](https://developer.ebay.com/develop/guides/sell/authorization)) and give them to the SDK in one of two ways:

- `AccountManagementConfiguration.AccessToken`: used for every call that doesn't pass its own token.
- The `accessToken` parameter on every method: used for that call only, for example a user token where the configuration holds an application token.

```csharp
string userToken = Environment.GetEnvironmentVariable("EBAY_USER_TOKEN")!;
var privileges = await client.AccountV1.GetPrivilegesAsync(accessToken: userToken);
```

A token is only ever sent to trusted hosts, see below.

## Configuration

| Property | Type | Default | Description |
|---|---|---|---|
| `AccountV1BaseUrl` | `string` | `https://api.ebay.com/sell/account/v1` | Base URL of the Account API v1. |
| `AccountV2BaseUrl` | `string` | `https://api.ebay.com/sell/account/v2` | Base URL of the Account API v2. |
| `FinancesBaseUrl` | `string` | `https://apiz.ebay.com/sell/finances/v1` | Base URL of the Finances API. |
| `GraphQLBaseUrl` | `string` | `https://api.ebay.com/graphql` | Base URL of the GraphQL endpoint. |
| `AccessToken` | `string?` |  | Fallback bearer token used when no per-call accessToken is supplied. |
| `DiagnosticLogging` | `bool` | `false` | When true, logs the HTTP method, URL, status code, and response reason phrase for all non-success responses to stderr. |
| `HttpTimeout` | `TimeSpan` | 30 seconds | Timeout applied to all HTTP requests. |
| `DefaultRequestOptions` | `RequestOptions` | no headers | Default headers applied to every REST request. |
| `AllowedHosts` | `IList<string>` | empty | Additional hosts trusted with the access token, for example a proxy (see below). |

`AccountManagementConfiguration.DefaultAllowedHosts` lists the hosts trusted by default.

## Sandbox and trusted hosts

The access token is only sent to trusted hosts: the hosts in `AccountManagementConfiguration.DefaultAllowedHosts`, which are the eBay production hosts and their `*.sandbox.ebay.com` counterparts, plus any in `AllowedHosts`. Switching to the sandbox needs only the base URLs:

```csharp
var sandboxConfig = new AccountManagementConfiguration
{
    AccessToken = Environment.GetEnvironmentVariable("EBAY_SANDBOX_ACCESS_TOKEN"),
    AccountV1BaseUrl = "https://api.sandbox.ebay.com/sell/account/v1",
    AccountV2BaseUrl = "https://api.sandbox.ebay.com/sell/account/v2",
    FinancesBaseUrl = "https://apiz.sandbox.ebay.com/sell/finances/v1",
    GraphQLBaseUrl = "https://graphqlapi.sandbox.ebay.com/graphql",
};
```

Any other host, for example a proxy that forwards to eBay, has to be added to `AllowedHosts`; the client constructor throws `InvalidOperationException` otherwise:

```csharp
var proxied = new AccountManagementConfiguration
{
    AccountV1BaseUrl = "https://ebay-proxy.example.com/sell/account/v1",
    AllowedHosts = { "ebay-proxy.example.com" },
};
```

## Dependency injection

`AddAccountManagementSdk` registers the configuration and one shared `AccountManagementClient` as a singleton. It owns its HTTP connections and is disposed with the container. Inject the client and use its properties for the individual APIs:

```csharp
using EBay.AccountManagement;
using EBay.AccountManagement.Configuration;
using EBay.AccountManagement.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAccountManagementSdk(new AccountManagementConfiguration
{
    AccessToken = builder.Configuration["EBay:AccessToken"]
});

var app = builder.Build();
app.MapGet("/example", async (AccountManagementClient client) =>
{
    var privileges = await client.AccountV1.GetPrivilegesAsync();
    return Results.Ok();
});
app.Run();
```

## Custom HTTP handler

To send requests through your own handler, for example for a proxy, client certificates or a test double, pass it to the constructor. The handler is shared by all APIs and is not disposed by the client:

```csharp
var handler = new HttpClientHandler { Proxy = new WebProxy("http://corporate-proxy:8080") };
using var proxiedClient = new AccountManagementClient(config, handler);
```

## Error handling

```csharp
try
{
    var privileges = await client.AccountV1.GetPrivilegesAsync();
}
catch (UnauthorizedException)
{
    // HTTP 401: the token is missing, expired, for the other environment, or lacks a scope.
}
catch (ApiException ex)
{
    // Any other REST error.
    Console.WriteLine($"HTTP {ex.ResponseStatusCode}");
}
catch (GraphQLException ex)
{
    // GraphQL "errors" in the response.
    Console.WriteLine(string.Join("; ", ex.Errors));
}
```

- `UnauthorizedException` (HTTP 401) and `GraphQLException` derive from `SdkException`.
- Other REST errors throw Kiota's `ApiException`, or a generated subclass named in eBay's spec for that operation, with `ResponseStatusCode`. eBay's error text is only available where the spec describes the error body correctly; set `DiagnosticLogging = true` to log failed requests to stderr.

## .NET Framework

The `net472` build is a classic package that `packages.config` projects can use. Such projects must list every dependency themselves. At the minimum versions the SDK supports, these are:

| Package | Version |
|---|---|
| Microsoft.Kiota.Abstractions, .Bundle, .Http.HttpClientLibrary, .Serialization.Form, .Serialization.Json, .Serialization.Multipart, .Serialization.Text | 1.22.0 |
| Std.UriTemplate | 2.0.8 |
| Microsoft.IO.RecyclableMemoryStream | 3.0.1 |
| Microsoft.Extensions.DependencyInjection.Abstractions | 6.0.0 |
| System.Diagnostics.DiagnosticSource, System.Net.Http.WinHttpHandler | 6.0.0 |
| System.Text.Json, System.Text.Encodings.Web, System.IO.Pipelines, Microsoft.Bcl.AsyncInterfaces | 10.0.0 |
| System.Runtime.CompilerServices.Unsafe | 6.1.2 |
| System.Memory, System.Threading.Tasks.Extensions | 4.6.3 |
| System.Buffers, System.Numerics.Vectors, System.ValueTuple | 4.6.1 |

With these versions the app needs binding redirects for Microsoft.Bcl.AsyncInterfaces (10.0.0.0), System.Buffers (4.0.5.0), System.Memory (4.0.5.0), System.Runtime.CompilerServices.Unsafe (6.0.3.0), System.Text.Encodings.Web (10.0.0.0), System.Text.Json (10.0.0.0) and System.Threading.Tasks.Extensions (4.2.4.0). There must be no redirect for System.ValueTuple: .NET Framework 4.7.1+ provides it. Newer package versions work too and change the redirect versions.

## Development

- .NET 10 SDK, plus the .NET 8 and .NET 9 SDKs so every target framework builds. `net472` builds on any OS; its tests run on Windows only.
- Build: `dotnet build EBay.AccountManagement`
- Test: `dotnet test tests/EBay.AccountManagement.Tests`
- Most of this repository is generated from eBay's API specs by the SDK generator (`api-sdk-definitions`): the API clients, `Configuration/`, `Extensions/`, `AccountManagementClient.cs`, `specs/` and `generate.sh`. Change the generator, not the generated files. `./generate.sh` reruns Kiota for the specs in `specs/openapi`.

See [CONTRIBUTING.adoc](CONTRIBUTING.adoc).

## License

Apache 2.0, see [LICENSE](LICENSE).
