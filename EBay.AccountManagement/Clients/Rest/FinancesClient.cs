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


using System.Globalization;
using System.Net;
using System.Text.Json;
using EBay.AccountManagement.Configuration;
using EBay.AccountManagement.Exceptions;
using EBay.AccountManagement.Generated.Finances;
using Microsoft.Kiota.Abstractions;

namespace EBay.AccountManagement.Clients.Rest;

public sealed class FinancesClient
{
    private readonly FinancesApiClient _kiota;
    private readonly AccountManagementConfiguration _config;
    private readonly ITokenContext _tokenContext;

    internal FinancesClient(FinancesApiClient kiota, AccountManagementConfiguration config, ITokenContext tokenContext)
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

    // ── Transactions ────────────────────────────────────────────────────────

    public async Task<string?> GetTransactionsAsync(
        string? filter = null,
        int? limit = null,
        string? offset = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Transaction.GetAsync(cfg =>
                {
                    if (!string.IsNullOrEmpty(filter))
                        cfg.QueryParameters.Filter = filter;
                    if (limit.HasValue)
                        cfg.QueryParameters.Limit = limit.Value.ToString(CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(offset))
                        cfg.QueryParameters.Offset = offset;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> GetTransactionSummaryAsync(
        string? filter = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Transaction_summary.GetAsync(cfg =>
                {
                    if (!string.IsNullOrEmpty(filter))
                        cfg.QueryParameters.Filter = filter;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Payouts ─────────────────────────────────────────────────────────────

    public async Task<string?> GetPayoutsAsync(
        string? filter = null,
        int? limit = null,
        string? offset = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Payout.GetAsync(cfg =>
                {
                    if (!string.IsNullOrEmpty(filter))
                        cfg.QueryParameters.Filter = filter;
                    if (limit.HasValue)
                        cfg.QueryParameters.Limit = limit.Value.ToString(CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(offset))
                        cfg.QueryParameters.Offset = offset;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> GetPayoutAsync(
        string payoutId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Payout[payoutId].GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> GetPayoutSummaryAsync(
        string? filter = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Payout_summary.GetAsync(cfg =>
                {
                    if (!string.IsNullOrEmpty(filter))
                        cfg.QueryParameters.Filter = filter;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Billing Activity ─────────────────────────────────────────────────────

    public async Task<string?> GetBillingActivityAsync(
        string? filter = null,
        int? limit = null,
        string? offset = null,
        string? sort = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Billing_activity.GetAsync(cfg =>
                {
                    if (!string.IsNullOrEmpty(filter))
                        cfg.QueryParameters.Filter = filter;
                    if (limit.HasValue)
                        cfg.QueryParameters.Limit = limit.Value.ToString(CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(offset))
                        cfg.QueryParameters.Offset = offset;
                    if (!string.IsNullOrEmpty(sort))
                        cfg.QueryParameters.Sort = sort;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Order Earnings ──────────────────────────────────────────────────────

    public async Task<string?> GetOrderEarningsAsync(
        string? filter = null,
        int? limit = null,
        string? offset = null,
        string? sort = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Order_earnings.GetAsync(cfg =>
                {
                    if (!string.IsNullOrEmpty(filter))
                        cfg.QueryParameters.Filter = filter;
                    if (limit.HasValue)
                        cfg.QueryParameters.Limit = limit.Value.ToString(CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(offset))
                        cfg.QueryParameters.Offset = offset;
                    if (!string.IsNullOrEmpty(sort))
                        cfg.QueryParameters.Sort = sort;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> GetOrderEarningAsync(
        string orderId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Order_earnings[orderId].GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> GetOrderEarningsSummaryAsync(
        string? filter = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Order_earnings_summary.GetAsync(cfg =>
                {
                    if (!string.IsNullOrEmpty(filter))
                        cfg.QueryParameters.Filter = filter;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Seller Funds Summary ────────────────────────────────────────────────

    public async Task<string?> GetSellerFundsSummaryAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Seller_funds_summary.GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Transfer ────────────────────────────────────────────────────────────

    public async Task<string?> GetTransferAsync(
        string transferId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Transfer[transferId].GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }
}
