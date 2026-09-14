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
using EBay.AccountManagement.Generated.AccountV1.Models;
using EBay.AccountManagement.Generated.AccountV1;
using Microsoft.Kiota.Abstractions;

namespace EBay.AccountManagement.Clients.Rest;

public sealed class AccountV1Client
{
    private readonly AccountV1ApiClient _kiota;
    private readonly AccountManagementConfiguration _config;
    private readonly ITokenContext _tokenContext;
    private readonly IRequestAdapter _adapter;

    internal AccountV1Client(AccountV1ApiClient kiota, AccountManagementConfiguration config, ITokenContext tokenContext, IRequestAdapter adapter)
    {
        _kiota = kiota;
        _config = config;
        _tokenContext = tokenContext;
        _adapter = adapter;
    }

    /// <summary>
    /// This method retrieves the list of custom policies defined for a seller's account. To limit the returned custom policies, specify the <b>policy_types</b> query parameter.
    /// </summary>
    public Task<CustomPolicyResponse?> GetCustomPoliciesAsync(
        string? policyTypes = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Custom_policy.EmptyPathSegment.GetAsync(cfg =>
        {
            if (policyTypes is not null)
            {
                cfg.QueryParameters.PolicyTypes = policyTypes;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method creates a new custom policy that specifies the seller's terms for complying with local governmental regulations. Each Custom Policy targets a <b>policyType</b>. Multiple policies may be created as using the following custom policy types:
    /// - PRODUCT_COMPLIANCE: Product Compliance policies disclose product information as required for regulatory compliance.
    /// <b>Note:</b> A maximum of 60 Product Compliance policies per seller may be created.
    /// - TAKE_BACK: Takeback policies describe the seller's legal obligation to take back a previously purchased item when the buyer purchases a new one.
    /// <b>Note:</b> A maximum of 18 Takeback policies per seller may be created.
    /// A successful create policy call returns an HTTP status code of <b>201 Created</b> with the system-generated policy ID included in the Location response header.
    /// </summary>
    public Task CreateCustomPolicyAsync(
        CustomPolicyCreateRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Custom_policy.EmptyPathSegment.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves the custom policy specified by the <b>custom_policy_id</b> path parameter.
    /// </summary>
    public Task<CustomPolicy?> GetCustomPolicyAsync(
        string customPolicyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Custom_policy[customPolicyId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method updates an existing custom policy specified by the <b>custom_policy_id</b> path parameter. Since this method overwrites the policy's <b>name</b>, <b>label</b>, and <b>description</b> fields, always include the complete and current text of all three policy fields in the request payload, even if they are not being updated.
    /// For example, the value for the <b>label</b> field is to be updated, but the <b>name</b> and <b>description</b> values will remain unchanged. The existing <b>name</b> and <b>description</b> values, as they are defined in the current policy, must also be passed in.
    /// A successful policy update call returns an HTTP status code of <b>204 No Content</b>.
    /// </summary>
    public Task UpdateCustomPolicyAsync(
        string customPolicyId,
        CustomPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Custom_policy[customPolicyId].PutAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method creates a new fulfillment policy for an eBay marketplace where the policy encapsulates seller's terms for fulfilling item purchases. Fulfillment policies include the shipment options that the seller offers to buyers.
    /// A successful request returns the <b>getFulfillmentPolicy</b> URI to the new policy in the <b>Location</b> response header and the ID for the new policy is returned in the response payload.
    /// <b>Tip:</b> For details on creating and using the business policies supported by the Account API, see <see href="/api-docs/sell/static/seller-accounts/business-policies.html">eBay business policies</see>.
    /// </summary>
    public Task<SetFulfillmentPolicyResponse?> CreateFulfillmentPolicyAsync(
        FulfillmentPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Fulfillment_policy.EmptyPathSegment.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves the complete details of a fulfillment policy. Supply the ID of the policy you want to retrieve using the <b>fulfillmentPolicyId</b> path parameter.
    /// </summary>
    public Task<FulfillmentPolicy?> GetFulfillmentPolicyAsync(
        string fulfillmentPolicyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Fulfillment_policy[fulfillmentPolicyId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method updates an existing fulfillment policy. Specify the policy you want to update using the <b>fulfillment_policy_id</b> path parameter. Supply a complete policy payload with the updates you want to make; this call overwrites the existing policy with the new details specified in the payload.
    /// </summary>
    public Task<SetFulfillmentPolicyResponse?> UpdateFulfillmentPolicyAsync(
        string fulfillmentPolicyId,
        FulfillmentPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Fulfillment_policy[fulfillmentPolicyId].PutAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method deletes a fulfillment policy. Supply the ID of the policy you want to delete in the <b>fulfillmentPolicyId</b> path parameter.
    /// </summary>
    public Task DeleteFulfillmentPolicyAsync(
        string fulfillmentPolicyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Fulfillment_policy[fulfillmentPolicyId].DeleteAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves all the fulfillment policies configured for the marketplace you specify using the <c>marketplace_id</c> query parameter.
    /// </summary>
    public Task<FulfillmentPolicyResponse?> GetFulfillmentPoliciesAsync(
        string marketplaceId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Fulfillment_policy.GetAsync(cfg =>
        {
            cfg.QueryParameters.MarketplaceId = marketplaceId;
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves the details for a specific fulfillment policy. In the request, supply both the policy <c>name</c> and its associated <c>marketplace_id</c> as query parameters.
    /// </summary>
    public Task<FulfillmentPolicy?> GetFulfillmentPolicyByNameAsync(
        string marketplaceId,
        string name,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Fulfillment_policy.Get_by_policy_name.GetAsync(cfg =>
        {
            cfg.QueryParameters.MarketplaceId = marketplaceId;
            cfg.QueryParameters.Name = name;
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves all the payment business policies configured for the marketplace you specify using the <c>marketplace_id</c> query parameter.
    /// </summary>
    public Task<PaymentPolicyResponse?> GetPaymentPoliciesAsync(
        string marketplaceId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_policy.GetAsync(cfg =>
        {
            cfg.QueryParameters.MarketplaceId = marketplaceId;
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method creates a new payment policy where the policy encapsulates seller's terms for order payments.
    /// A successful request returns the <b>getPaymentPolicy</b> URI to the new policy in the <b>Location</b> response header and the ID for the new policy is returned in the response payload.
    /// <b>Tip:</b> For details on creating and using the business policies supported by the Account API, see <see href="/api-docs/sell/static/seller-accounts/business-policies.html">eBay business policies</see>.
    /// </summary>
    public Task<SetPaymentPolicyResponse?> CreatePaymentPolicyAsync(
        PaymentPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_policy.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves the complete details of a payment policy. Supply the ID of the policy you want to retrieve using the <b>paymentPolicyId</b> path parameter.
    /// </summary>
    public Task<PaymentPolicy?> GetPaymentPolicyAsync(
        string paymentPolicyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_policy[paymentPolicyId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method updates an existing payment policy. Specify the policy you want to update using the <b>payment_policy_id</b> path parameter. Supply a complete policy payload with the updates you want to make; this call overwrites the existing policy with the new details specified in the payload.
    /// </summary>
    public Task<SetPaymentPolicyResponse?> UpdatePaymentPolicyAsync(
        string paymentPolicyId,
        PaymentPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_policy[paymentPolicyId].PutAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method deletes a payment policy. Supply the ID of the policy you want to delete in the <b>paymentPolicyId</b> path parameter.
    /// </summary>
    public Task DeletePaymentPolicyAsync(
        string paymentPolicyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_policy[paymentPolicyId].DeleteAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves the details of a specific payment policy. Supply both the policy <c>name</c> and its associated <c>marketplace_id</c> in the request query parameters.
    /// </summary>
    public Task<PaymentPolicy?> GetPaymentPolicyByNameAsync(
        string marketplaceId,
        string name,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payment_policy.Get_by_policy_name.GetAsync(cfg =>
        {
            cfg.QueryParameters.MarketplaceId = marketplaceId;
            cfg.QueryParameters.Name = name;
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// <b>Note:</b> This method is no longer applicable, as all seller accounts globally have been enabled for the new eBay payment and checkout flow.
    /// This method returns whether or not the user is opted-in to the specified payments program. Sellers opt-in to payments programs by marketplace and you use the <b>marketplace_id</b> path parameter to specify the marketplace of the status flag you want returned.
    /// </summary>
    public Task<PaymentsProgramResponse?> GetPaymentsProgramAsync(
        string marketplaceId,
        string paymentsProgramType,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payments_program[marketplaceId][paymentsProgramType].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// <b>Note:</b> This method is no longer applicable, as all seller accounts globally have been enabled for the new eBay payment and checkout flow.
    /// This method retrieves a seller's onboarding status for a payments program for a specified marketplace. The overall onboarding status of the seller and the status of each onboarding step is returned.
    /// </summary>
    public Task<PaymentsProgramOnboardingResponse?> GetPaymentsProgramOnboardingAsync(
        string marketplaceId,
        string paymentsProgramType,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payments_program[marketplaceId][paymentsProgramType].Onboarding.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves the seller's current set of privileges, including whether or not the seller's eBay registration has been completed, as well as the details of their site-wide <b>sellingLimit</b> (the amount and quantity they can sell on a given day).
    /// </summary>
    public Task<SellingPrivileges?> GetPrivilegesAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Privilege.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method gets a list of the seller programs that the seller has opted-in to.
    /// </summary>
    public Task<Programs?> GetOptedInProgramsAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Program.Get_opted_in_programs.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method opts the seller in to an eBay seller program. Refer to the <see href="/develop/api/sell/account_api_v1#sell-account_api_v1-program-optintoprogram.programtypeenum">ProgramTypeEnum</see> for information about available eBay seller programs.
    /// <b>Note:</b> It can take up to 24-hours for eBay to process your request to opt-in to a Seller Program. Use the <see href="/develop/api/sell/account_api_v1#sell-account_api_v1-program-getoptedinprograms">getOptedInPrograms</see> call to check the status of your request after the processing period has passed.
    /// </summary>
    public Task OptInToProgramAsync(
        Program body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Program.Opt_in.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method opts the seller out of a seller program in which they are currently opted in to. A seller can retrieve a list of the seller programs they are opted-in to using the <b>getOptedInPrograms</b> method.
    /// </summary>
    public Task OptOutOfProgramAsync(
        Program body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Program.Opt_out.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves a seller's <i>shipping rate tables</i> for the country specified in the <b>country_code</b> query parameter. If you call this method without specifying a country code, the call returns all of the seller's shipping rate tables.
    /// The method's response includes a <b>rateTableId</b> for each table defined by the seller. This <b>rateTableId</b> value is used in add/revise item call or in create/update fulfillment business policy call to specify the shipping rate table to use for that policy's domestic or international shipping options.
    /// This call currently supports getting rate tables related to the following marketplaces: United States, Canada, United Kingdom, Germany, Australia, France, Italy, and Spain. <b>Note:</b> Rate tables created with the Trading API might not have been assigned a <b>rateTableId</b> at the time of their creation. This method can assign and return <b>rateTableId</b> values for rate tables with missing IDs if you make a request using the <b>country_code</b> where the seller has defined rate tables.
    /// Sellers can define up to 40 shipping rate tables for their account, which lets them set up different rate tables for each of the marketplaces they sell into. Go to <see href="https://www.ebay.com/ship/rt">Shipping rate tables</see> in <b>My eBay</b> to create and update rate tables.
    /// </summary>
    public Task<RateTableResponse?> GetRateTablesAsync(
        string? countryCode = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Rate_table.GetAsync(cfg =>
        {
            if (countryCode is not null)
            {
                cfg.QueryParameters.CountryCode = countryCode;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves all the return policies configured for the marketplace you specify using the <c>marketplace_id</c> query parameter.
    /// </summary>
    public Task<ReturnPolicyResponse?> GetReturnPoliciesAsync(
        string marketplaceId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Return_policy.GetAsync(cfg =>
        {
            cfg.QueryParameters.MarketplaceId = marketplaceId;
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method creates a new return policy where the policy encapsulates seller's terms for returning items.
    /// Each policy targets a specific marketplace, and you can create multiple policies for each marketplace. Return policies are not applicable to motor-vehicle listings.
    /// A successful request returns the <b>getReturnPolicy</b> URI to the new policy in the <b>Location</b> response header and the ID for the new policy is returned in the response payload.
    /// <b>Tip:</b> For details on creating and using the business policies supported by the Account API, see <see href="/api-docs/sell/static/seller-accounts/business-policies.html">eBay business policies</see>.
    /// </summary>
    public Task<SetReturnPolicyResponse?> CreateReturnPolicyAsync(
        ReturnPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Return_policy.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves the complete details of the return policy specified by the <b>returnPolicyId</b> path parameter.
    /// </summary>
    public Task<ReturnPolicy?> GetReturnPolicyAsync(
        string returnPolicyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Return_policy[returnPolicyId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method updates an existing return policy. Specify the policy you want to update using the <b>return_policy_id</b> path parameter. Supply a complete policy payload with the updates you want to make; this call overwrites the existing policy with the new details specified in the payload.
    /// </summary>
    public Task<SetReturnPolicyResponse?> UpdateReturnPolicyAsync(
        string returnPolicyId,
        ReturnPolicyRequest body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Return_policy[returnPolicyId].PutAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method deletes a return policy. Supply the ID of the policy you want to delete in the <b>returnPolicyId</b> path parameter.
    /// </summary>
    public Task DeleteReturnPolicyAsync(
        string returnPolicyId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Return_policy[returnPolicyId].DeleteAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves the details of a specific return policy. Supply both the policy <c>name</c> and its associated <c>marketplace_id</c> in the request query parameters.
    /// </summary>
    public Task<ReturnPolicy?> GetReturnPolicyByNameAsync(
        string marketplaceId,
        string name,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Return_policy.Get_by_policy_name.GetAsync(cfg =>
        {
            cfg.QueryParameters.MarketplaceId = marketplaceId;
            cfg.QueryParameters.Name = name;
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method creates or updates multiple sales-tax table entries.
    /// <i>Sales-tax tables</i> can be set up for countries that support different <i>tax jurisdictions</i>.
    /// <b>Note:</b> Sales-tax tables are only available for the US (EBAY_US) and Canada (EBAY_CA) marketplaces.
    /// Each sales-tax table entry comprises the following parameters:
    /// - <c>countryCode</c>
    /// - <c>jurisdictionId</c>
    /// - <c>salesTaxPercentage</c>
    /// - <c>shippingAndHandlingTaxed</c>
    /// Valid jurisdiction IDs are retrieved using <b><see href="/develop/api/sell/metadata_api#sell-metadata_api-country-getsalestaxjurisdictions">getSalesTaxJurisdictions</see></b> in the Metadata API.
    /// For details about using this call, refer to <see href="/api-docs/sell/static/seller-accounts/tax-tables.html">Establishing sales-tax tables</see>.
    /// Important! </b>"&gt;<b>Important!</b> In the US, eBay now calculates, collects, and remits sales tax to the proper taxing authorities in all 50 states and Washington, DC. Sellers can no longer specify sales-tax rates for these jurisdictions using a tax table.
    /// However, sellers may continue to use a sales-tax table to set rates for the following US territories:
    /// - American Samoa (AS)
    /// - Guam (GU)
    /// - Northern Mariana Islands (MP)
    /// - Palau (PW)
    /// - US Virgin Islands (VI)
    /// For additional information, refer to <see href="https://www.ebay.com/help/selling/fees-credits-invoices/taxes-import-charges?id=4121">Taxes and import charges</see>.
    /// </summary>
    public Task<UpdatedSalesTaxResponse?> BulkCreateOrReplaceSalesTaxAsync(
        BulkSalesTaxInput body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Bulk_create_or_replace_sales_tax.PostAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This call retrieves the current sales-tax table entry for a specific tax jurisdiction. Specify the jurisdiction to retrieve using the <b>countryCode</b> and <b>jurisdictionId</b> path parameters. All four response fields will be returned if a sales-tax entry exists for the tax jurisdiction. Otherwise, the response will be returned as empty.
    /// <b>Note:</b> Sales-tax tables are only available for the US (EBAY_US) and Canada (EBAY_CA) marketplaces.
    /// Important! </b>"&gt;<b>Important!</b> In the US, eBay now calculates, collects, and remits sales tax to the proper taxing authorities in all 50 states and Washington, DC. Sellers can no longer specify sales-tax rates for these jurisdictions using a tax table.
    /// However, sellers may continue to use a sales-tax table to set rates for the following US territories:
    /// - American Samoa (AS)
    /// - Guam (GU)
    /// - Northern Mariana Islands (MP)
    /// - Palau (PW)
    /// - US Virgin Islands (VI)
    /// For additional information, refer to <see href="https://www.ebay.com/help/selling/fees-credits-invoices/taxes-import-charges?id=4121">Taxes and import charges</see>.
    /// </summary>
    public Task<SalesTax?> GetSalesTaxAsync(
        string countryCode,
        string jurisdictionId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Sales_tax[countryCode][jurisdictionId].GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method creates or updates a sales-tax table entry for a jurisdiction. Specify the tax table entry you want to configure using the two path parameters: <b>countryCode</b> and <b>jurisdictionId</b>.
    /// A tax table entry for a jurisdiction is comprised of two fields: one for the jurisdiction's sales-tax rate and another that's a boolean value indicating whether or not shipping and handling are taxed in the jurisdiction.
    /// You can set up <i>sales-tax tables</i> for countries that support different <i>tax jurisdictions</i>.
    /// <b>Note:</b> Sales-tax tables are only available for the US (EBAY_US) and Canada (EBAY_CA) marketplaces.
    /// Retrieve valid jurisdiction IDs using <b><see href="/develop/api/sell/metadata_api#sell-metadata_api-country-getsalestaxjurisdictions">getSalesTaxJurisdictions</see></b> in the Metadata API.
    /// For details about using this call, refer to <see href="/api-docs/sell/static/seller-accounts/tax-tables.html">Establishing sales-tax tables</see>.
    /// Important! </b>"&gt;<b>Important!</b> In the US, eBay now calculates, collects, and remits sales tax to the proper taxing authorities in all 50 states and Washington, DC. Sellers can no longer specify sales-tax rates for these jurisdictions using a tax table.
    /// However, sellers may continue to use a sales-tax table to set rates for the following US territories:
    /// - American Samoa (AS)
    /// - Guam (GU)
    /// - Northern Mariana Islands (MP)
    /// - Palau (PW)
    /// - US Virgin Islands (VI)
    /// For additional information, refer to <see href="https://www.ebay.com/help/selling/fees-credits-invoices/taxes-import-charges?id=4121">Taxes and import charges</see>.
    /// </summary>
    public Task CreateOrReplaceSalesTaxAsync(
        string countryCode,
        string jurisdictionId,
        SalesTaxBase body,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Sales_tax[countryCode][jurisdictionId].PutAsync(body, cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This call deletes a sales-tax table entry for a jurisdiction. Specify the jurisdiction to delete using the <b>countryCode</b> and <b>jurisdictionId</b> path parameters.
    /// <b>Note:</b> Sales-tax tables are only available for the US (EBAY_US) and Canada (EBAY_CA) marketplaces.
    /// </summary>
    public Task DeleteSalesTaxAsync(
        string countryCode,
        string jurisdictionId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Sales_tax[countryCode][jurisdictionId].DeleteAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Use this call to retrieve all sales tax table entries that the seller has defined for a specific country. All four response fields will be returned for each tax jurisdiction that matches the search criteria. If no sales tax rates are defined for the specified, a <c>204 No Content</c> status code is returned with no response payload.
    /// <b>Note:</b> Sales-tax tables are only available for the US (EBAY_US) and Canada (EBAY_CA) marketplaces.
    /// Important! </b>"&gt;<b>Important!</b> In the US, eBay now calculates, collects, and remits sales tax to the proper taxing authorities in all 50 states and Washington, DC. Sellers can no longer specify sales-tax rates for these jurisdictions using a tax table.
    /// However, sellers may continue to use a sales-tax table to set rates for the following US territories:
    /// - American Samoa (AS)
    /// - Guam (GU)
    /// - Northern Mariana Islands (MP)
    /// - Palau (PW)
    /// - US Virgin Islands (VI)
    /// For additional information, refer to <see href="https://www.ebay.com/help/selling/fees-credits-invoices/taxes-import-charges?id=4121">Taxes and import charges</see>.
    /// </summary>
    public Task<SalesTaxes?> GetSalesTaxesAsync(
        string countryCode,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Sales_tax.GetAsync(cfg =>
        {
            cfg.QueryParameters.CountryCode = countryCode;
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method retrieves a list of subscriptions associated with the seller account.
    /// </summary>
    public Task<SubscriptionResponse?> GetSubscriptionAsync(
        string? limit = null,
        string? continuationToken = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Subscription.GetAsync(cfg =>
        {
            if (limit is not null)
            {
                cfg.QueryParameters.Limit = limit;
            }
            if (continuationToken is not null)
            {
                cfg.QueryParameters.ContinuationToken = continuationToken;
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// <b>Note:</b> This method was originally created to see which onboarding requirements were still pending for sellers being onboarded for eBay managed payments, but now that all seller accounts are onboarded globally, this method should now just return an empty payload with a <c>204 No Content</c> HTTP status code.
    /// </summary>
    public Task<KycResponse?> GetKYCAsync(
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Kyc.GetAsync(cfg =>
        {
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method allows developers to check the seller eligibility status for eBay advertising programs.
    /// </summary>
    public Task<SellerEligibilityMultiProgramResponse?> GetAdvertisingEligibilityAsync(
        string xEBAYCMARKETPLACEID,
        string? programTypes = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Advertising_eligibility.GetAsync(cfg =>
        {
            if (programTypes is not null)
            {
                cfg.QueryParameters.ProgramTypes = programTypes;
            }
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));
}
