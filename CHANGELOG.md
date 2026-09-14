# ChangeLog

## 1.0.0 (09/14/2026)

**Breaking changes**
- The following operations were removed from `AccountV1Client`:
  - `GetKycAsync(string?, RequestOptions?, CancellationToken)`
  - `GetSalesTaxForJurisdictionAsync(string, string, string?, RequestOptions?, CancellationToken)`
  - `GetSubscriptionsAsync(string?, int?, string?, RequestOptions?, CancellationToken)`
- The following operations were removed from `AccountV2Client`:
  - `UpdateRateTableShippingCostAsync(string, RateTableUpdate, string?, RequestOptions?, CancellationToken)`
  - `UpdateUserPreferencesAsync(UntypedNode, string?, RequestOptions?, CancellationToken)`
- The following operations were removed from `FinancesClient`:
  - `GetBillingActivityAsync(string?, int?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetOrderEarningAsync(string, string?, RequestOptions?, CancellationToken)`
- The following operations changed signature on `AccountV1Client`:
  - `GetAdvertisingEligibilityAsync`: `(string?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, RequestOptions?, CancellationToken)`
  - `GetSalesTaxAsync`: `(string, string?, RequestOptions?, CancellationToken)` → `(string, string, string?, RequestOptions?, CancellationToken)`
- The following operations changed signature on `AccountV2Client`:
  - `CreateCalculatedShippingRulesAsync`: `(CreateCalculatedShippingRulesRequest, string?, RequestOptions?, CancellationToken)` → `(string, CreateCalculatedShippingRulesRequest, string?, RequestOptions?, CancellationToken)`
  - `CreateFlatShippingRulesAsync`: `(CreateFlatShippingRulesRequest, string?, RequestOptions?, CancellationToken)` → `(string, CreateFlatShippingRulesRequest, string?, RequestOptions?, CancellationToken)`
  - `CreatePromotionalShippingRuleAsync`: `(CreatePromotionalShippingRuleRequest, string?, RequestOptions?, CancellationToken)` → `(string, CreatePromotionalShippingRuleRequest, string?, RequestOptions?, CancellationToken)`
  - `GetCombinedShippingRulesAsync`: `(string?, RequestOptions?, CancellationToken)` → `(string, string?, RequestOptions?, CancellationToken)`
  - `GetUserPreferencesAsync`: `(string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, RequestOptions?, CancellationToken)`
  - `UpdateCalculatedShippingRulesAsync`: `(UpdateCalculatedShippingRulesRequest, string?, RequestOptions?, CancellationToken)` → `(string, UpdateCalculatedShippingRulesRequest, string?, RequestOptions?, CancellationToken)`
  - `UpdateCombinedPaymentsAsync`: `(UpdateCombinedPaymentsRequest, string?, RequestOptions?, CancellationToken)` → `(string, UpdateCombinedPaymentsRequest, string?, RequestOptions?, CancellationToken)`
  - `UpdateFlatShippingRulesAsync`: `(UpdateFlatShippingRulesRequest, string?, RequestOptions?, CancellationToken)` → `(string, UpdateFlatShippingRulesRequest, string?, RequestOptions?, CancellationToken)`
  - `UpdatePromotionalShippingRuleAsync`: `(UpdatePromotionalShippingRuleRequest, string?, RequestOptions?, CancellationToken)` → `(string, UpdatePromotionalShippingRuleRequest, string?, RequestOptions?, CancellationToken)`
- The following operations changed signature on `FinancesClient`:
  - `GetOrderEarningsAsync`: `(string?, int?, string?, string?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetOrderEarningsSummaryAsync`: `(string?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, RequestOptions?, CancellationToken)`
  - `GetPayoutAsync`: `(string, string?, RequestOptions?, CancellationToken)` → `(string, string, string?, RequestOptions?, CancellationToken)`
  - `GetPayoutsAsync`: `(string?, int?, string?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetPayoutSummaryAsync`: `(string?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, RequestOptions?, CancellationToken)`
  - `GetSellerFundsSummaryAsync`: `(string?, RequestOptions?, CancellationToken)` → `(string, string?, RequestOptions?, CancellationToken)`
  - `GetTransactionsAsync`: `(string?, int?, string?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetTransactionSummaryAsync`: `(string?, string?, RequestOptions?, CancellationToken)` → `(string, string?, string?, RequestOptions?, CancellationToken)`
  - `GetTransferAsync`: `(string, string?, RequestOptions?, CancellationToken)` → `(string, string, string?, RequestOptions?, CancellationToken)`

- The following operations were added to `AccountV1Client`:
  - `GetKYCAsync(string?, RequestOptions?, CancellationToken)`
  - `GetSalesTaxesAsync(string, string?, RequestOptions?, CancellationToken)`
  - `GetSubscriptionAsync(string?, string?, string?, RequestOptions?, CancellationToken)`
- The following operations were added to `AccountV2Client`:
  - `SetUserPreferencesAsync(string, UntypedNode, string?, RequestOptions?, CancellationToken)`
  - `UpdateShippingCostAsync(string, RateTableUpdate, string?, RequestOptions?, CancellationToken)`
- The following operations were added to `FinancesClient`:
  - `GetBillingActivitiesAsync(string?, string?, string?, string?, string?, string?, RequestOptions?, CancellationToken)`
  - `GetOrderEarningsByIdAsync(string, string, string?, RequestOptions?, CancellationToken)`
