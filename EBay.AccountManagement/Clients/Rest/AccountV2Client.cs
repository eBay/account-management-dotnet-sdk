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

using EBay.AccountManagement.Configuration;
using EBay.AccountManagement.Generated.AccountV2.Models;
using EBay.AccountManagement.Generated.AccountV2;
using Microsoft.Kiota.Abstractions;

namespace EBay.AccountManagement.Clients.Rest;

public sealed class AccountV2Client
{
    private readonly AccountV2ApiClient _kiota;
    private readonly AccountManagementConfiguration _config;
    private readonly ITokenContext _tokenContext;
    private readonly IRequestAdapter _adapter;

    internal AccountV2Client(AccountV2ApiClient kiota, AccountManagementConfiguration config, ITokenContext tokenContext, IRequestAdapter adapter)
    {
        _kiota = kiota;
        _config = config;
        _tokenContext = tokenContext;
        _adapter = adapter;
    }

    /// <summary>
    /// Retrieves details of a specific shipping rate table.
    /// </summary>
    public Task<RateTableDetails?> GetRateTableAsync(
        string rateTableId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Rate_table[rateTableId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Updates one or more shipping rates for a specific shipping rate table.
    /// </summary>
    public Task UpdateShippingCostAsync(
        string rateTableId,
        RateTableUpdate body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Rate_table[rateTableId].Update_shipping_cost.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Retrieves payout percentages and unique IDs for accounts configured to receive seller payouts.
    /// </summary>
    public Task<PayoutSettingsResponse?> GetPayoutSettingsAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payout_settings.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Updates the split-payout percentage for two payout instruments for sellers in mainland China.
    /// </summary>
    public Task UpdatePayoutPercentageAsync(
        UpdatePayoutPercentageRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payout_settings.Update_percentage.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Creates calculated shipping rules that determine combined shipping costs based on item attributes, such as weight and dimensions, for the seller's account.
    /// </summary>
    public Task CreateCalculatedShippingRulesAsync(
        string xEBAYCMARKETPLACEID,
        CreateCalculatedShippingRulesRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Combined_shipping_rules.Create_calculated_shipping_rules.PostAsync(body, cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Creates flat-rate rules that apply standard combined shipping costs for a seller's listings.
    /// </summary>
    public Task CreateFlatShippingRulesAsync(
        string xEBAYCMARKETPLACEID,
        CreateFlatShippingRulesRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Combined_shipping_rules.Create_flat_shipping_rules.PostAsync(body, cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Creates promotional shipping rules, such as discounts or free-shipping thresholds.
    /// </summary>
    public Task CreatePromotionalShippingRuleAsync(
        string xEBAYCMARKETPLACEID,
        CreatePromotionalShippingRuleRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Combined_shipping_rules.Create_promotional_shipping_rule.PostAsync(body, cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Retrieves all combined shipping rule configurations defined for the seller.
    /// </summary>
    public Task<GetCombinedShippingRulesPublicApiResponse?> GetCombinedShippingRulesAsync(
        string xEBAYCMARKETPLACEID,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Combined_shipping_rules.GetAsync(cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Updates previously defined calculated shipping rules.
    /// </summary>
    public Task UpdateCalculatedShippingRulesAsync(
        string xEBAYCMARKETPLACEID,
        UpdateCalculatedShippingRulesRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Combined_shipping_rules.Update_calculated_shipping_rules.PostAsync(body, cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Updates combined payment settings that determine how unpaid orders can be merged.
    /// </summary>
    public Task UpdateCombinedPaymentsAsync(
        string xEBAYCMARKETPLACEID,
        UpdateCombinedPaymentsRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Combined_shipping_rules.Update_combined_payments.PostAsync(body, cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Updates existing flat-rate shipping rules.
    /// </summary>
    public Task UpdateFlatShippingRulesAsync(
        string xEBAYCMARKETPLACEID,
        UpdateFlatShippingRulesRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Combined_shipping_rules.Update_flat_shipping_rules.PostAsync(body, cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Updates a promotional shipping rule to adjust discount thresholds, eligibility criteria, or duration for the seller.
    /// </summary>
    public Task UpdatePromotionalShippingRuleAsync(
        string xEBAYCMARKETPLACEID,
        UpdatePromotionalShippingRuleRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Combined_shipping_rules.Update_promotional_shipping_rule.PostAsync(body, cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Retrieves the seller's preferences for a specific eBay marketplace.
    /// </summary>
    public Task<GetUserPreferencesResponse?> GetUserPreferencesAsync(
        string xEBAYCMARKETPLACEID,
        string? fieldgroups = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.User_preferences.GetAsync(cfg =>
        {
            if (fieldgroups is not null)
            {
                cfg.QueryParameters.Fieldgroups = fieldgroups;
            }
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Modifies one or more preferences for a seller on a specific marketplace.
    /// </summary>
    public Task SetUserPreferencesAsync(
        string xEBAYCMARKETPLACEID,
        SetUserPreferencesRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.User_preferences.PatchAsync(body, cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));
}
