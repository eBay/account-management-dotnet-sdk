# ChangeLog

## 0.3.0 (09/25/2026)

### Added

- Target frameworks `net8.0` and `net9.0`, in addition to `net10.0`.

## 0.2.0 (09/14/2026)

### Breaking changes

#### Renamed operations

| Client | Previous | Current |
| --- | --- | --- |
| `AccountV1Client` | `GetKycAsync` | `GetKYCAsync` |
| `AccountV1Client` | `GetSalesTaxAsync` | `GetSalesTaxesAsync` |
| `AccountV1Client` | `GetSalesTaxForJurisdictionAsync` | `GetSalesTaxAsync` |
| `AccountV1Client` | `GetSubscriptionsAsync` | `GetSubscriptionAsync` |
| `AccountV2Client` | `UpdateRateTableShippingCostAsync` | `UpdateShippingCostAsync` |
| `AccountV2Client` | `UpdateUserPreferencesAsync` | `SetUserPreferencesAsync` |
| `FinancesClient` | `GetBillingActivityAsync` | `GetBillingActivitiesAsync` |
| `FinancesClient` | `GetOrderEarningAsync` | `GetOrderEarningsByIdAsync` |

#### Changed signatures

`AccountV1Client`:

| Operation | Previous parameters | Current parameters |
| --- | --- | --- |
| `GetAdvertisingEligibilityAsync` | `(string? programTypes, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string? programTypes, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetSubscriptionAsync` | `(string? continuationToken, int? limit, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string? limit, string? continuationToken, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |

`AccountV2Client`:

| Operation | Previous parameters | Current parameters |
| --- | --- | --- |
| `CreateCalculatedShippingRulesAsync` | `(CreateCalculatedShippingRulesRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, CreateCalculatedShippingRulesRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `CreateFlatShippingRulesAsync` | `(CreateFlatShippingRulesRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, CreateFlatShippingRulesRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `CreatePromotionalShippingRuleAsync` | `(CreatePromotionalShippingRuleRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, CreatePromotionalShippingRuleRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetCombinedShippingRulesAsync` | `(string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetUserPreferencesAsync` | `(string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string? fieldgroups, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `SetUserPreferencesAsync` | `(UntypedNode body, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, SetUserPreferencesRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `UpdateCalculatedShippingRulesAsync` | `(UpdateCalculatedShippingRulesRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, UpdateCalculatedShippingRulesRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `UpdateCombinedPaymentsAsync` | `(UpdateCombinedPaymentsRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, UpdateCombinedPaymentsRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `UpdateFlatShippingRulesAsync` | `(UpdateFlatShippingRulesRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, UpdateFlatShippingRulesRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `UpdatePromotionalShippingRuleAsync` | `(UpdatePromotionalShippingRuleRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, UpdatePromotionalShippingRuleRequest body, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |

`FinancesClient`:

| Operation | Previous parameters | Current parameters |
| --- | --- | --- |
| `GetBillingActivitiesAsync` | `(string? filter, int? limit, string? offset, string? sort, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string? acceptLanguage, string? filter, string? limit, string? offset, string? sort, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetOrderEarningsByIdAsync` | `(string orderId, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string orderId, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetOrderEarningsAsync` | `(string? filter, int? limit, string? offset, string? sort, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string[]? filter, int? limit, int? offset, string[]? sort, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetOrderEarningsSummaryAsync` | `(string? filter, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string[]? filter, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetPayoutAsync` | `(string payoutId, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string payoutId, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetPayoutsAsync` | `(string? filter, int? limit, string? offset, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string[]? filter, int? limit, int? offset, string[]? sort, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetPayoutSummaryAsync` | `(string? filter, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string[]? filter, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetSellerFundsSummaryAsync` | `(string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetTransactionsAsync` | `(string? filter, int? limit, string? offset, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string[]? filter, int? limit, int? offset, string[]? sort, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetTransactionSummaryAsync` | `(string? filter, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string[] filter, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
| `GetTransferAsync` | `(string transferId, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string xEBAYCMARKETPLACEID, string transferId, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |
