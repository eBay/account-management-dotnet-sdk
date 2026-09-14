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
using Microsoft.Kiota.Abstractions.Serialization;
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
    /// This method retrieves an existing rate table identified by the <b>rate_table_id</b> path parameter.
    /// Shipping rate tables are currently supported by the following marketplaces: United States, Canada, United Kingdom, Germany, Australia, France, Italy, and Spain. A successful call returns detailed information for the specified shipping rate table.
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
    /// This method allows sellers to update <b>shippingCost</b> and/or <b>additionalCost</b> information for an existing shipping rate table identified by the <b>rate_table_id</b> path parameter.
    /// A successful call returns an HTTP status code of <b>204 No Content</b>.
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
    /// Important! </b>"&gt;<b>Important!</b> Split-payout functionality is <b>only</b> available to mainland China sellers, who can split payouts between their Payoneer account and bank account. Card payouts are not currently available for sellers in mainland China.
    /// This method returns details on two payment instruments defined on a seller's account, including the ID, type, status, nickname, last four digits of the account number, and payout percentage for the instruments.
    /// Using the <b>instrumentId</b> returned with this method, sellers can makes changes to the payout split of the instruments with the <b>updatePayoutPercentage</b> method. Note that the <b>instrumentStatus</b> of a payment instrument, also returned using this method, must be <c>ACTIVE</c> in order to do split payouts on that payment instrument.
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
    /// Important! </b>"&gt;<b>Important!</b> Split-payout functionality is <b>only</b> available to mainland China sellers, who can split payouts between their Payoneer account and bank account. Card payouts are not currently available for sellers in mainland China.
    /// This method allows sellers in mainland China to configure the split-payout percentage for two payout instruments available for seller payouts. For example, a seller can split payouts to have 70% of the payout go to a bank account and 30% go to a Payoneer account.
    /// <b>Note:</b> The split-payout percentage <b>must</b> always add up to 100%. If the values do not equal 100, the call will fail. Instruments cannot be added/removed using Finance and Account APIs.
    /// Users can specify the payout instruments being updated by using the <b>instrumentId</b> associated with each payment instrument in the request payload. This value is returned by using the <b>getPayoutSettings</b> method. Users can specify the percentage of the payout allocated to each instrument using the <b>payoutPercentage</b> request field. This value must be a whole number and cannot exceed 100.
    /// For more details on configuring split-payout percentages, see <see href="/api-docs/split-payout/playbook.html">Mainland China Split Payout Playbook.</see>
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
    /// This method creates or registers calculated shipping rules that determine combined shipping costs based on weight, item count, or cost parameters for an authenticated seller.
    /// This shipping rule will apply to eBay listings that use the calculated shipping model.
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
    /// This method is used to create fixed-rate (flat) shipping rules that apply standard combined shipping costs for a seller’s listings.
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
    /// This method defines promotional shipping rules such as discounts or free-shipping thresholds, configurable by marketplace for the seller.
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
    /// This method retrieves all existing combined shipping rule configurations defined by the authenticated seller, including calculated, flat, and promotional types.
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
    /// This method updates previously defined calculated shipping rules to modify discount percentages, weight offsets, or amount parameters for the seller.
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
    /// This method configures or modifies combined payment settings that determine how unpaid orders can be merged for a single invoice within a defined duration for the seller.
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
    /// This method updates existing shipping rules.
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
    /// This method updates an existing promotional shipping rule to adjust discount thresholds, eligibility criteria, or duration for the seller.
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
    /// This method retrieves the seller's preferences for a specific eBay marketplace, such as combined payment preferences, same-day shipping cutoff time, excluded shipping locations, and opt-in status for Business Policies and Out-of-Stock control. The <b>fieldgroups</b> query parameter specifies the type of seller preferences to retrieve. If <b>fieldgroups</b> = <c>ALL</c> or the parameter is omitted, all the supported seller preferences are returned. To retrieve only specific preferences, include the <b>fieldgroups</b> parameter with one or more supported values, delimited by commas.
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
    /// This method is used to modify one or more preferences for a seller on a specific marketplace. The preferences that can be modified include combined payment preferences, same-day shipping cutoff time, and opt-in status for Business Policies and Out-of-stock control. This is a PATCH operation, so you only need to include the fields that correspond to the preferences/settings that you are changing.
    /// </summary>
    public Task SetUserPreferencesAsync(
        string xEBAYCMARKETPLACEID,
        UntypedNode body,
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
