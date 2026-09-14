# ChangeLog

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
| `GetAdvertisingEligibilityAsync` | `(string?, string?, RequestOptions?, CancellationToken)` | `(string, string?, string?, RequestOptions?, CancellationToken)` |
| `GetSubscriptionAsync` | `(string? continuationToken, int? limit, string? accessToken, RequestOptions? requestOptions, CancellationToken cancellationToken)` | `(string? limit, string? continuationToken, string? accessToken, RequestOptions? requestOptions, CancellationToken ct)` |

`AccountV2Client`:

| Operation | Previous parameters | Current parameters |
| --- | --- | --- |
| `CreateCalculatedShippingRulesAsync` | `(CreateCalculatedShippingRulesRequest, string?, RequestOptions?, CancellationToken)` | `(string, CreateCalculatedShippingRulesRequest, string?, RequestOptions?, CancellationToken)` |
| `CreateFlatShippingRulesAsync` | `(CreateFlatShippingRulesRequest, string?, RequestOptions?, CancellationToken)` | `(string, CreateFlatShippingRulesRequest, string?, RequestOptions?, CancellationToken)` |
| `CreatePromotionalShippingRuleAsync` | `(CreatePromotionalShippingRuleRequest, string?, RequestOptions?, CancellationToken)` | `(string, CreatePromotionalShippingRuleRequest, string?, RequestOptions?, CancellationToken)` |
| `GetCombinedShippingRulesAsync` | `(string?, RequestOptions?, CancellationToken)` | `(string, string?, RequestOptions?, CancellationToken)` |
| `GetUserPreferencesAsync` | `(string?, RequestOptions?, CancellationToken)` | `(string, string?, string?, RequestOptions?, CancellationToken)` |
| `SetUserPreferencesAsync` | `(UntypedNode, string?, RequestOptions?, CancellationToken)` | `(string, SetUserPreferencesRequest, string?, RequestOptions?, CancellationToken)` |
| `UpdateCalculatedShippingRulesAsync` | `(UpdateCalculatedShippingRulesRequest, string?, RequestOptions?, CancellationToken)` | `(string, UpdateCalculatedShippingRulesRequest, string?, RequestOptions?, CancellationToken)` |
| `UpdateCombinedPaymentsAsync` | `(UpdateCombinedPaymentsRequest, string?, RequestOptions?, CancellationToken)` | `(string, UpdateCombinedPaymentsRequest, string?, RequestOptions?, CancellationToken)` |
| `UpdateFlatShippingRulesAsync` | `(UpdateFlatShippingRulesRequest, string?, RequestOptions?, CancellationToken)` | `(string, UpdateFlatShippingRulesRequest, string?, RequestOptions?, CancellationToken)` |
| `UpdatePromotionalShippingRuleAsync` | `(UpdatePromotionalShippingRuleRequest, string?, RequestOptions?, CancellationToken)` | `(string, UpdatePromotionalShippingRuleRequest, string?, RequestOptions?, CancellationToken)` |

`FinancesClient`:

| Operation | Previous parameters | Current parameters |
| --- | --- | --- |
| `GetBillingActivitiesAsync` | `(string?, int?, string?, string?, string?, RequestOptions?, CancellationToken)` | `(string?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)` |
| `GetOrderEarningsByIdAsync` | `(string, string?, RequestOptions?, CancellationToken)` | `(string, string, string?, RequestOptions?, CancellationToken)` |
| `GetOrderEarningsAsync` | `(string?, int?, string?, string?, string?, RequestOptions?, CancellationToken)` | `(string, string[]?, int?, int?, string[]?, string?, RequestOptions?, CancellationToken)` |
| `GetOrderEarningsSummaryAsync` | `(string?, string?, RequestOptions?, CancellationToken)` | `(string, string[]?, string?, RequestOptions?, CancellationToken)` |
| `GetPayoutAsync` | `(string, string?, RequestOptions?, CancellationToken)` | `(string, string, string?, RequestOptions?, CancellationToken)` |
| `GetPayoutsAsync` | `(string?, int?, string?, string?, RequestOptions?, CancellationToken)` | `(string, string[]?, int?, int?, string[]?, string?, RequestOptions?, CancellationToken)` |
| `GetPayoutSummaryAsync` | `(string?, string?, RequestOptions?, CancellationToken)` | `(string, string[]?, string?, RequestOptions?, CancellationToken)` |
| `GetSellerFundsSummaryAsync` | `(string?, RequestOptions?, CancellationToken)` | `(string, string?, RequestOptions?, CancellationToken)` |
| `GetTransactionsAsync` | `(string?, int?, string?, string?, RequestOptions?, CancellationToken)` | `(string, string[]?, int?, int?, string[]?, string?, RequestOptions?, CancellationToken)` |
| `GetTransactionSummaryAsync` | `(string?, string?, RequestOptions?, CancellationToken)` | `(string, string[], string?, RequestOptions?, CancellationToken)` |
| `GetTransferAsync` | `(string, string?, RequestOptions?, CancellationToken)` | `(string, string, string?, RequestOptions?, CancellationToken)` |
