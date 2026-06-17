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


using System.Net;
using System.Text.Json;
using EBay.AccountManagement.Configuration;
using EBay.AccountManagement.Exceptions;
using EBay.AccountManagement.Generated.AccountV2;
using EBay.AccountManagement.Generated.AccountV2.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;

namespace EBay.AccountManagement.Clients.Rest;

public sealed class AccountV2Client
{
    private readonly AccountV2ApiClient _kiota;
    private readonly AccountManagementConfiguration _config;
    private readonly ITokenContext _tokenContext;

    internal AccountV2Client(AccountV2ApiClient kiota, AccountManagementConfiguration config, ITokenContext tokenContext)
    {
        _kiota = kiota;
        _config = config;
        _tokenContext = tokenContext;
    }

    private void ApplyHeaders(RequestHeaders headers, RequestOptions? callOptions)
    {
        foreach (var (k, v) in _config.DefaultRequestOptions.Headers)
            headers.TryAdd(k, v);
        if (callOptions is null) return;
        foreach (var (k, v) in callOptions.Headers)
            headers[k] = new[] { v };
    }

    private static T Rethrow401<T>(ApiException ex)
    {
        if (ex.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
            throw new UnauthorizedException(ex);
        throw new SdkException($"REST API error ({ex.ResponseStatusCode})", ex);
    }

    // ── User Preferences ────────────────────────────────────────────────────

    public async Task<string?> GetUserPreferencesAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.User_preferences.GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task UpdateUserPreferencesAsync(
        UntypedNode body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.User_preferences.PatchAsync(body,
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    // ── Payout Settings ─────────────────────────────────────────────────────

    public async Task<string?> GetPayoutSettingsAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Payout_settings.GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task UpdatePayoutPercentageAsync(
        UpdatePayoutPercentageRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Payout_settings.Update_percentage.PostAsync(body,
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    // ── Rate Table ──────────────────────────────────────────────────────────

    public async Task<string?> GetRateTableAsync(
        string rateTableId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Rate_table[rateTableId].GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task UpdateRateTableShippingCostAsync(
        string rateTableId,
        RateTableUpdate body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Rate_table[rateTableId].Update_shipping_cost.PostAsync(body,
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    // ── Combined Shipping Rules ─────────────────────────────────────────────

    public async Task<string?> GetCombinedShippingRulesAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Combined_shipping_rules.GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task CreateCalculatedShippingRulesAsync(
        CreateCalculatedShippingRulesRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Combined_shipping_rules.Create_calculated_shipping_rules.PostAsync(body,
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    public async Task CreateFlatShippingRulesAsync(
        CreateFlatShippingRulesRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Combined_shipping_rules.Create_flat_shipping_rules.PostAsync(body,
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    public async Task CreatePromotionalShippingRuleAsync(
        CreatePromotionalShippingRuleRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Combined_shipping_rules.Create_promotional_shipping_rule.PostAsync(body,
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    public async Task UpdateCalculatedShippingRulesAsync(
        UpdateCalculatedShippingRulesRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Combined_shipping_rules.Update_calculated_shipping_rules.PostAsync(body,
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    public async Task UpdateCombinedPaymentsAsync(
        UpdateCombinedPaymentsRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Combined_shipping_rules.Update_combined_payments.PostAsync(body,
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    public async Task UpdateFlatShippingRulesAsync(
        UpdateFlatShippingRulesRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Combined_shipping_rules.Update_flat_shipping_rules.PostAsync(body,
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    public async Task UpdatePromotionalShippingRuleAsync(
        UpdatePromotionalShippingRuleRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Combined_shipping_rules.Update_promotional_shipping_rule.PostAsync(body,
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }
}
