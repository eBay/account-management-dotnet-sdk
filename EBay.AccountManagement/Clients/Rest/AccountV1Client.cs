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
using EBay.AccountManagement.Generated.AccountV1;
using EBay.AccountManagement.Generated.AccountV1.Models;
using Microsoft.Kiota.Abstractions;

namespace EBay.AccountManagement.Clients.Rest;

public sealed class AccountV1Client
{
    private readonly AccountV1ApiClient _kiota;
    private readonly AccountManagementConfiguration _config;
    private readonly ITokenContext _tokenContext;

    internal AccountV1Client(AccountV1ApiClient kiota, AccountManagementConfiguration config, ITokenContext tokenContext)
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

    // ── Fulfillment Policy ──────────────────────────────────────────────────

    public async Task<string?> GetFulfillmentPoliciesAsync(
        string marketplaceId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Fulfillment_policy.GetAsync(cfg =>
                {
                    cfg.QueryParameters.MarketplaceId = marketplaceId;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> CreateFulfillmentPolicyAsync(
        FulfillmentPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Fulfillment_policy.EmptyPathSegment.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> GetFulfillmentPolicyAsync(
        string policyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Fulfillment_policy[policyId].GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> UpdateFulfillmentPolicyAsync(
        string policyId,
        FulfillmentPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Fulfillment_policy[policyId].PutAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task DeleteFulfillmentPolicyAsync(
        string policyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Fulfillment_policy[policyId].DeleteAsync(
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    public async Task<string?> GetFulfillmentPolicyByNameAsync(
        string marketplaceId,
        string name,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Fulfillment_policy.Get_by_policy_name.GetAsync(cfg =>
                {
                    cfg.QueryParameters.MarketplaceId = marketplaceId;
                    cfg.QueryParameters.Name = name;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Payment Policy ──────────────────────────────────────────────────────

    public async Task<string?> GetPaymentPoliciesAsync(
        string marketplaceId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Payment_policy.GetAsync(cfg =>
                {
                    cfg.QueryParameters.MarketplaceId = marketplaceId;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> CreatePaymentPolicyAsync(
        PaymentPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Payment_policy.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> GetPaymentPolicyAsync(
        string policyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Payment_policy[policyId].GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> UpdatePaymentPolicyAsync(
        string policyId,
        PaymentPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Payment_policy[policyId].PutAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task DeletePaymentPolicyAsync(
        string policyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Payment_policy[policyId].DeleteAsync(
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    public async Task<string?> GetPaymentPolicyByNameAsync(
        string marketplaceId,
        string name,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Payment_policy.Get_by_policy_name.GetAsync(cfg =>
                {
                    cfg.QueryParameters.MarketplaceId = marketplaceId;
                    cfg.QueryParameters.Name = name;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Return Policy ───────────────────────────────────────────────────────

    public async Task<string?> GetReturnPoliciesAsync(
        string marketplaceId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Return_policy.GetAsync(cfg =>
                {
                    cfg.QueryParameters.MarketplaceId = marketplaceId;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> CreateReturnPolicyAsync(
        ReturnPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Return_policy.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> GetReturnPolicyAsync(
        string policyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Return_policy[policyId].GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> UpdateReturnPolicyAsync(
        string policyId,
        ReturnPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Return_policy[policyId].PutAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task DeleteReturnPolicyAsync(
        string policyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Return_policy[policyId].DeleteAsync(
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    public async Task<string?> GetReturnPolicyByNameAsync(
        string marketplaceId,
        string name,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Return_policy.Get_by_policy_name.GetAsync(cfg =>
                {
                    cfg.QueryParameters.MarketplaceId = marketplaceId;
                    cfg.QueryParameters.Name = name;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Custom Policy ───────────────────────────────────────────────────────

    public async Task<string?> GetCustomPoliciesAsync(
        string? policyTypes = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Custom_policy.EmptyPathSegment.GetAsync(cfg =>
                {
                    if (!string.IsNullOrEmpty(policyTypes))
                        cfg.QueryParameters.PolicyTypes = policyTypes;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> CreateCustomPolicyAsync(
        CustomPolicyCreateRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Custom_policy.EmptyPathSegment.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> GetCustomPolicyAsync(
        string customPolicyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Custom_policy[customPolicyId].GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task UpdateCustomPolicyAsync(
        string customPolicyId,
        CustomPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Custom_policy[customPolicyId].PutAsync(body,
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    // ── KYC ─────────────────────────────────────────────────────────────────

    public async Task<string?> GetKycAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Kyc.GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Advertising Eligibility ─────────────────────────────────────────────

    public async Task<string?> GetAdvertisingEligibilityAsync(
        string? programTypes = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Advertising_eligibility.GetAsync(cfg =>
                {
                    if (!string.IsNullOrEmpty(programTypes))
                        cfg.QueryParameters.ProgramTypes = programTypes;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Bulk Sales Tax ──────────────────────────────────────────────────────

    public async Task<string?> BulkCreateOrReplaceSalesTaxAsync(
        BulkSalesTaxInput body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Bulk_create_or_replace_sales_tax.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Rate Table ──────────────────────────────────────────────────────────

    public async Task<string?> GetRateTablesAsync(
        string? countryCode = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Rate_table.GetAsync(cfg =>
                {
                    if (!string.IsNullOrEmpty(countryCode))
                        cfg.QueryParameters.CountryCode = countryCode;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Sales Tax ───────────────────────────────────────────────────────────

    public async Task<string?> GetSalesTaxAsync(
        string countryCode,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Sales_tax.GetAsync(cfg =>
                {
                    cfg.QueryParameters.CountryCode = countryCode;
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> GetSalesTaxForJurisdictionAsync(
        string countryCode,
        string jurisdictionId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Sales_tax[countryCode][jurisdictionId].GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task CreateOrReplaceSalesTaxAsync(
        string countryCode,
        string jurisdictionId,
        SalesTaxBase body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Sales_tax[countryCode][jurisdictionId].PutAsync(body,
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    public async Task DeleteSalesTaxAsync(
        string countryCode,
        string jurisdictionId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _tokenContext.RunAsync(accessToken, () => _kiota.Sales_tax[countryCode][jurisdictionId].DeleteAsync(
                cfg => ApplyHeaders(cfg.Headers, requestOptions),
                cancellationToken: cancellationToken));
        }
        catch (ApiException ex) { Rethrow401<object>(ex); }
    }

    // ── Subscription ────────────────────────────────────────────────────────

    public async Task<string?> GetSubscriptionsAsync(
        string? continuationToken = null,
        int? limit = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Subscription.GetAsync(cfg =>
                {
                    if (!string.IsNullOrEmpty(continuationToken))
                        cfg.QueryParameters.ContinuationToken = continuationToken;
                    if (limit.HasValue)
                        cfg.QueryParameters.Limit = limit.Value.ToString(CultureInfo.InvariantCulture);
                    ApplyHeaders(cfg.Headers, requestOptions);
                }, cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Payments Program ────────────────────────────────────────────────────

    public async Task<string?> GetPaymentsProgramAsync(
        string marketplaceId,
        string paymentsProgramType,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Payments_program[marketplaceId][paymentsProgramType].GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> GetPaymentsProgramOnboardingAsync(
        string marketplaceId,
        string paymentsProgramType,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Payments_program[marketplaceId][paymentsProgramType].Onboarding.GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Program Opt-In / Opt-Out ────────────────────────────────────────────

    public async Task<string?> GetOptedInProgramsAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Program.Get_opted_in_programs.GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> OptInToProgramAsync(
        Program body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Program.Opt_in.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    public async Task<string?> OptOutOfProgramAsync(
        Program body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Program.Opt_out.PostAsync(body,
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }

    // ── Seller Privileges ───────────────────────────────────────────────────

    public async Task<string?> GetPrivilegesAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _tokenContext.RunAsync(accessToken, async () =>
            {
                var r = await _kiota.Privilege.GetAsync(
                    cfg => ApplyHeaders(cfg.Headers, requestOptions),
                    cancellationToken: cancellationToken);
                return r != null ? JsonSerializer.Serialize(r) : null;
            });
            return result;
        }
        catch (ApiException ex) { return Rethrow401<string?>(ex); }
    }
}
