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
using EBay.AccountManagement.Generated.Finances.Models;
using EBay.AccountManagement.Generated.Finances;
using Microsoft.Kiota.Abstractions;

namespace EBay.AccountManagement.Clients.Rest;

public sealed class FinancesClient
{
    private readonly FinancesApiClient _kiota;
    private readonly AccountManagementConfiguration _config;
    private readonly ITokenContext _tokenContext;
    private readonly IRequestAdapter _adapter;

    internal FinancesClient(FinancesApiClient kiota, AccountManagementConfiguration config, ITokenContext tokenContext, IRequestAdapter adapter)
    {
        _kiota = kiota;
        _config = config;
        _tokenContext = tokenContext;
        _adapter = adapter;
    }

    /// <summary>
    /// This method returns detailed order-level financial data for each order associated with a seller account. The returned order-level financial data includes order earnings, gross amount, expenses, and refunds. Order earnings includes earnings after deducting expenses and refunds from the gross amount.
    /// <b>Note: </b>Expenses include fees, shipping labels, and donations. Refunds include gross refunds, gross claims, and gross payment disputes.
    /// <b>Note: </b>Only charges and credits tied to the order are shown, and they appear in near real time for orders created within the selected order creation time window.
    /// The financial data for orders will be returned as filtered based on the order's creation date.
    /// <b>Note: </b>Only charges and credits tied to the order are shown, and they appear in near real time for orders created within the selected order creation time window.
    /// Pagination is supported through the <c>limit</c> and <c>offset</c> parameters. These parameters can be used to control the number of records returned in a single response and to retrieve subsequent pages of results when the result set spans multiple pages.
    /// The response can be filtered by using the <c>filter</c> parameter. This parameter allows consumers to restrict the results returned by the method based on supported filtering criteria.
    /// <b>Note:</b> Access to the <b>order_earnings</b> resource is currently limited to only US, China, or Hong Kong sellers who meet the following criteria and also request access:
    /// - US sellers with the country of residence set to <c>US</c> and having a payout currency in <c>USD</c>.
    /// - Hong Kong or China sellers having the country of residence set to <c>HK</c> or <c>CN</c> and the payout currency set to <c>USD</c> have access.
    /// To request access, submit an <see href="/api-docs/static/gs_request-an-application-growth.html">application growth check</see> to have the required <see href="#h3-oauth-scope">OAuth scope</see> added to your app. After submission, you can use the same <see href="/api-docs/static/gs_request-an-application-growth.html">growth check</see> link to track the status of the request.
    /// eBay plans on expanding this to other markets in the future.
    /// Important! </b>"&gt;<b>Important!</b> Due to EU &amp; UK Payments regulatory requirements, an additional security verification via Digital Signatures is required for certain API calls that are made on behalf of EU/UK sellers, including all Finances API methods. Please refer to <see href="/develop/guides/digital-signatures-for-apis">Digital Signatures for APIs</see> to learn more on the impacted APIs and the process to create signatures to be included in the HTTP payload.
    /// <b>Note: </b> The Finances API does not support <see href="https://www.ebay.com/sellercenter/ebay-for-business/multi-user-account-access">Team Access</see>. Financial information, such as payouts or transactions, is only returned for the user that makes the call. You cannot use any of the methods in this API to return financial information for another user.
    /// </summary>
    public Task<OrderEarnings?> GetOrderEarningsAsync(
        string xEBAYCMARKETPLACEID,
        string[]? filter = null,
        int? limit = null,
        int? offset = null,
        string[]? sort = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Order_earnings.GetAsync(cfg =>
        {
            if (filter is not null)
            {
                cfg.QueryParameters.Filter = filter;
            }
            if (limit is not null)
            {
                cfg.QueryParameters.Limit = limit;
            }
            if (offset is not null)
            {
                cfg.QueryParameters.Offset = offset;
            }
            if (sort is not null)
            {
                cfg.QueryParameters.Sort = sort;
            }
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method returns detailed order-level financial data including order earnings, gross amount, expenses, and refunds. Order earnings includes earnings after deducting expenses and refunds from the gross amount.
    /// <b>Note: </b>Expenses include fees, shipping labels, and donations. Refunds include gross refunds, gross claims, and gross payment disputes.
    /// <b>Note: </b>Only charges and credits tied to the order are shown, and they appear in near real time for orders created within the selected order creation time window.
    /// The response returns earnings information only for the order identified by the <c>order_id</c> path parameter.
    /// <b>Note:</b> Access to the <b>order_earnings</b> resource is currently limited to only US, China, or Hong Kong sellers who meet the following criteria and also request access:
    /// - US sellers with the country of residence set to <c>US</c> and having a payout currency in <c>USD</c>.
    /// - Hong Kong or China sellers having the country of residence set to <c>HK</c> or <c>CN</c> and the payout currency set to <c>USD</c> have access.
    /// To request access, submit an <see href="/api-docs/static/gs_request-an-application-growth.html">application growth check</see> to have the required <see href="#h3-oauth-scope">OAuth scope</see> added to your app. After submission, you can use the same <see href="/api-docs/static/gs_request-an-application-growth.html">growth check</see> link to track the status of the request.
    /// eBay plans on expanding this to other markets in the future.
    /// Important! </b>"&gt;<b>Important!</b> Due to EU &amp; UK Payments regulatory requirements, an additional security verification via Digital Signatures is required for certain API calls that are made on behalf of EU/UK sellers, including all Finances API methods. Please refer to <see href="/develop/guides/digital-signatures-for-apis">Digital Signatures for APIs</see> to learn more on the impacted APIs and the process to create signatures to be included in the HTTP payload.
    /// <b>Note: </b> The Finances API does not support <see href="https://www.ebay.com/sellercenter/ebay-for-business/multi-user-account-access">Team Access</see>. Financial information, such as payouts or transactions, is only returned for the user that makes the call. You cannot use any of the methods in this API to return financial information for another user.
    /// </summary>
    public Task<OrderEarning?> GetOrderEarningsByIdAsync(
        string xEBAYCMARKETPLACEID,
        string orderId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Order_earnings[orderId].GetAsync(cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// This method returns a summarized view of order earnings information for one or more orders associated with a seller account. The method retrieves aggregated data for order earnings after deducting expenses and refunds from the gross amount. You can use this method for high-level financial reporting workflows.
    /// <b>Note: </b>Expenses include fees, shipping labels, and donations. Refunds include gross refunds, gross claims, and gross payment disputes.
    /// <b>Note: </b>Only charges and credits tied to the order are shown, and they appear in near real time for orders created within the selected order creation time window.
    /// The response can be filtered by using the <c>filter</c> parameter. This parameter allows consumers to restrict the summary results returned by the method based on supported filtering criteria.
    /// <b>Note:</b> Access to the <b>order_earnings</b> resource is currently limited to only US, China, or Hong Kong sellers who meet the following criteria and also request access:
    /// - US sellers with the country of residence set to <c>US</c> and having a payout currency in <c>USD</c>.
    /// - Hong Kong or China sellers having the country of residence set to <c>HK</c> or <c>CN</c> and the payout currency set to <c>USD</c> have access.
    /// To request access, submit an <see href="/api-docs/static/gs_request-an-application-growth.html">application growth check</see> to have the required <see href="#h3-oauth-scope">OAuth scope</see> added to your app. After submission, you can use the same <see href="/api-docs/static/gs_request-an-application-growth.html">growth check</see> link to track the status of the request.
    /// eBay plans on expanding this to other markets in the future.
    /// Important! </b>"&gt;<b>Important!</b> Due to EU &amp; UK Payments regulatory requirements, an additional security verification via Digital Signatures is required for certain API calls that are made on behalf of EU/UK sellers, including all Finances API methods. Please refer to <see href="/develop/guides/digital-signatures-for-apis">Digital Signatures for APIs</see> to learn more on the impacted APIs and the process to create signatures to be included in the HTTP payload.
    /// <b>Note: </b> The Finances API does not support <see href="https://www.ebay.com/sellercenter/ebay-for-business/multi-user-account-access">Team Access</see>. Financial information, such as payouts or transactions, is only returned for the user that makes the call. You cannot use any of the methods in this API to return financial information for another user.
    /// </summary>
    public Task<OrderEarningsSummary?> GetOrderEarningsSummaryAsync(
        string xEBAYCMARKETPLACEID,
        string[]? filter = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Order_earnings_summary.GetAsync(cfg =>
        {
            if (filter is not null)
            {
                cfg.QueryParameters.Filter = filter;
            }
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> Due to EU &amp; UK Payments regulatory requirements, an additional security verification via Digital Signatures is required for certain API calls that are made on behalf of EU/UK sellers, including all <b>Finances API</b> methods. Please refer to <see href="/develop/guides/digital-signatures-for-apis">Digital Signatures for APIs</see> to learn more on the impacted APIs and the process to create signatures to be included in the HTTP payload.
    /// <b>Note:</b> The <b>Finances API</b> does not support <see href="https://www.ebay.com/sellercenter/ebay-for-business/multi-user-account-access">Team Access</see>. Financial information, such as payouts or transactions, is only returned for the user that makes the call. You cannot use any of the methods in this API to return financial information for another user.
    /// This method retrieves details on a specific seller payout. The unique identifier of the payout is passed in as a path parameter at the end of the call URI.
    /// The <b>getPayouts</b> method can be used to retrieve the unique identifier of a payout, or the user can check Seller Hub.
    /// For split-payout cases, which are <b>only</b> available to sellers in mainland China, this method will return the <b>payoutPercentage</b> for the specified payout. This value indicates the current payout percentage allocated to a payment instrument. This method will also return the <b>convertedToCurrency</b> and <b>convertedToValue</b> response fields in CNY value.
    /// <b>Note:</b> In split-payout cases, this method will only return details on an individual payout, also known as a true(actual) payoutid. If a user inputs a <b>payoutReference</b> id as a path parameter, the call will fail and the <b>404 not found</b> status code will be returned.
    /// For more information on split payouts, see <see href="/api-docs/split-payout/playbook.html">Mainland China Split Payout Playbook</see>.
    /// </summary>
    public Task<Payout?> GetPayoutAsync(
        string xEBAYCMARKETPLACEID,
        string payoutId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payout[payoutId].GetAsync(cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> Due to EU &amp; UK Payments regulatory requirements, an additional security verification via Digital Signatures is required for certain API calls that are made on behalf of EU/UK sellers, including all <b>Finances API</b> methods. Please refer to <see href="/develop/guides/digital-signatures-for-apis">Digital Signatures for APIs</see> to learn more on the impacted APIs and the process to create signatures to be included in the HTTP payload.
    /// <b>Note:</b> The <b>Finances API</b> does not support <see href="https://www.ebay.com/sellercenter/ebay-for-business/multi-user-account-access">Team Access</see>. Financial information, such as payouts or transactions, is only returned for the user that makes the call. You cannot use any of the methods in this API to return financial information for another user.
    /// This method is used to retrieve the details of one or more seller payouts. By using the <b>filter</b> query parameter, users can retrieve payouts processed within a specific date range, and/or they can retrieve payouts in a specific state.
    /// There are also pagination and sort query parameters that allow users to control the payouts that are returned in the response.
    /// If no payouts match the input criteria, an empty payload is returned.
    /// For split-payout cases, which are <b>only</b> available to sellers in mainland China, this method will return the <b>payoutPercentage</b> for the specified payout. This value indicates the current payout percentage allocated to a payout instrument. This method will also return the <b>convertedToCurrency</b> and <b>convertedTo</b> response fields set to CNY value and the <b>payoutReference</b>, the unique identifier reference (not true payout).
    /// By using the <b>filter</b> query parameter, users can retrieve the two true(actual) payouts associated with a <b>payoutReference</b>.
    /// <b>Note:</b> For more information on split payouts, see <see href="/api-docs/split-payout/playbook.html">Mainland China Split Payout Playbook</see>.
    /// <b>Note:</b> Only payouts less than 5 years in the past can be retrieved.
    /// </summary>
    public Task<Payouts?> GetPayoutsAsync(
        string xEBAYCMARKETPLACEID,
        string[]? filter = null,
        int? limit = null,
        int? offset = null,
        string[]? sort = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payout.GetAsync(cfg =>
        {
            if (filter is not null)
            {
                cfg.QueryParameters.Filter = filter;
            }
            if (limit is not null)
            {
                cfg.QueryParameters.Limit = limit;
            }
            if (offset is not null)
            {
                cfg.QueryParameters.Offset = offset;
            }
            if (sort is not null)
            {
                cfg.QueryParameters.Sort = sort;
            }
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> Due to EU &amp; UK Payments regulatory requirements, an additional security verification via Digital Signatures is required for certain API calls that are made on behalf of EU/UK sellers, including all <b>Finances API</b> methods. Please refer to <see href="/develop/guides/digital-signatures-for-apis">Digital Signatures for APIs</see> to learn more on the impacted APIs and the process to create signatures to be included in the HTTP payload.
    /// <b>Note:</b> The <b>Finances API</b> does not support <see href="https://www.ebay.com/sellercenter/ebay-for-business/multi-user-account-access">Team Access</see>. Financial information, such as payouts or transactions, is only returned for the user that makes the call. You cannot use any of the methods in this API to return financial information for another user.
    /// This method is used to retrieve cumulative values for payouts in a particular state, or all states. The metadata in the response includes total payouts, the total number of monetary transactions (sales, refunds, credits) associated with those payouts, and the total dollar value of all payouts.
    /// If the <b>filter</b> query parameter is used to filter by payout status, only one payout status value may be used. If the <b>filter</b> query parameter is not used to filter by a specific payout status, cumulative values for payouts in all states are returned.
    /// The user can also use the <b>filter</b> query parameter to specify a date range, and then only payouts that were processed within that date range are considered.
    /// <b>Note:</b> <b>getPayoutSummary</b> will only return data on payouts that occurred less than five years in the past.
    /// </summary>
    public Task<PayoutSummaryResponse?> GetPayoutSummaryAsync(
        string xEBAYCMARKETPLACEID,
        string[]? filter = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Payout_summary.GetAsync(cfg =>
        {
            if (filter is not null)
            {
                cfg.QueryParameters.Filter = filter;
            }
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> Due to EU &amp; UK Payments regulatory requirements, an additional security verification via Digital Signatures is required for certain API calls that are made on behalf of EU/UK sellers, including all <b>Finances API</b> methods. Please refer to <see href="/develop/guides/digital-signatures-for-apis">Digital Signatures for APIs</see> to learn more on the impacted APIs and the process to create signatures to be included in the HTTP payload.
    /// <b>Note:</b> The <b>Finances API</b> does not support <see href="https://www.ebay.com/sellercenter/ebay-for-business/multi-user-account-access">Team Access</see>. Financial information, such as payouts or transactions, is only returned for the user that makes the call. You cannot use any of the methods in this API to return financial information for another user.
    /// This method retrieves all pending funds that have not yet been distributed through a seller payout.
    /// There are no input parameters for this method. The response payload includes available funds, funds being processed, funds on hold, and also an aggregate count of all three of these categories.
    /// If there are no funds that are pending, on hold, or being processed for the seller's account, no response payload is returned, and an http status code of <c>204 - No Content</c> is returned instead.
    /// </summary>
    public Task<SellerFundsSummaryResponse?> GetSellerFundsSummaryAsync(
        string xEBAYCMARKETPLACEID,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Seller_funds_summary.GetAsync(cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> Due to EU &amp; UK Payments regulatory requirements, an additional security verification via Digital Signatures is required for certain API calls that are made on behalf of EU/UK sellers, including all <b>Finances API</b> methods. Please refer to <see href="/develop/guides/digital-signatures-for-apis">Digital Signatures for APIs</see> to learn more on the impacted APIs and the process to create signatures to be included in the HTTP payload.
    /// <b>Note:</b> The <b>Finances API</b> does not support <see href="https://www.ebay.com/sellercenter/ebay-for-business/multi-user-account-access">Team Access</see>. Financial information, such as payouts or transactions, is only returned for the user that makes the call. You cannot use any of the methods in this API to return financial information for another user.
    /// The <b>getTransactions</b> method allows a seller to retrieve information about one or more of their monetary transactions.
    /// <b>Note:</b> For a complete list of transaction types, refer to <see href="/develop/api/sell/finances_api#sell-finances_api-transaction-gettransactions.transactiontypeenum">TransactionTypeEnum</see>.
    /// Numerous input filters are available which can be used individually or combined to refine the data that are returned. For example:
    /// - <c>SALE</c> transactions for August 15, 2022;
    /// - <c>RETURN</c> transactions for the month of January, 2021;
    /// - Transactions currently in a <c>transactionStatus</c> equal to <c>FUNDS_ON_HOLD</c>.
    /// Refer to the <see href="/develop/api/sell/finances_api#sell-finances_api-transaction-gettransactions-param-filter">filter</see> field for additional information about each filter and its use.
    /// Pagination and sort query parameters are also provided that allow users to further control how monetary transactions are displayed in the response.
    /// If no monetary transactions match the input criteria, an http status code of <i>204 No Content</i> is returned with no response payload.
    /// <b>Note:</b> Only monetary transactions that have occurred within the last five years can be retrieved.
    /// </summary>
    public Task<Transactions?> GetTransactionsAsync(
        string xEBAYCMARKETPLACEID,
        string[]? filter = null,
        int? limit = null,
        int? offset = null,
        string[]? sort = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Transaction.GetAsync(cfg =>
        {
            if (filter is not null)
            {
                cfg.QueryParameters.Filter = filter;
            }
            if (limit is not null)
            {
                cfg.QueryParameters.Limit = limit;
            }
            if (offset is not null)
            {
                cfg.QueryParameters.Offset = offset;
            }
            if (sort is not null)
            {
                cfg.QueryParameters.Sort = sort;
            }
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> Due to EU &amp; UK Payments regulatory requirements, an additional security verification via Digital Signatures is required for certain API calls that are made on behalf of EU/UK sellers, including all <b>Finances API</b> methods. Please refer to <see href="/develop/guides/digital-signatures-for-apis">Digital Signatures for APIs</see> to learn more on the impacted APIs and the process to create signatures to be included in the HTTP payload.
    /// <b>Note:</b> The <b>Finances API</b> does not support <see href="https://www.ebay.com/sellercenter/ebay-for-business/multi-user-account-access">Team Access</see>. Financial information, such as payouts or transactions, is only returned for the user that makes the call. You cannot use any of the methods in this API to return financial information for another user.
    /// The <b>getTransactionSummary</b> method retrieves cumulative information for monetary transactions. If applicable, the number of payments with a <c>transactionStatus</c> equal to <c>FUNDS_ON_HOLD</c> and the total monetary amount of these on-hold payments are also returned.
    /// <b>Note:</b> For a complete list of transaction types, refer to <see href="/develop/api/sell/finances_api#sell-finances_api-transaction-gettransactions.transactiontypeenum">TransactionTypeEnum</see>.
    /// Refer to the <b>filter</b> field for additional information about each filter and its use.
    /// <b>Note:</b> Unless a <c>transactionType</c> filter is used to retrieve a specific type of transaction (e.g., <c>SALE</c>, <c>REFUND</c>, etc.,) the <see href="/develop/api/sell/finances_api#sell-finances_api-transaction-gettransactionsummary.transactionsummaryresponse.creditcount">creditCount</see> and <see href="/develop/api/sell/finances_api#sell-finances_api-transaction-gettransactionsummary.transactionsummaryresponse.creditamount">creditAmount</see> response fields both include <i>order sales</i> and <i>seller credits</i> information. That is, the <b>count</b> and <b>value</b> fields do not distinguish between these two types monetary transactions.
    /// <b>Note:</b> <b>getTransactionSummary</b> will only return data on transactions that occurred less than five years in the past.
    /// </summary>
    public Task<TransactionSummaryResponse?> GetTransactionSummaryAsync(
        string xEBAYCMARKETPLACEID,
        string[] filter,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Transaction_summary.GetAsync(cfg =>
        {
            cfg.QueryParameters.Filter = filter;
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> Due to EU &amp; UK Payments regulatory requirements, an additional security verification via Digital Signatures is required for certain API calls that are made on behalf of EU/UK sellers, including all <b>Finances API</b> methods. Please refer to <see href="/develop/guides/digital-signatures-for-apis">Digital Signatures for APIs</see> to learn more on the impacted APIs and the process to create signatures to be included in the HTTP payload.
    /// <b>Note:</b> The <b>Finances API</b> does not support <see href="https://www.ebay.com/sellercenter/ebay-for-business/multi-user-account-access">Team Access</see>. Financial information, such as payouts or transactions, is only returned for the user that makes the call. You cannot use any of the methods in this API to return financial information for another user.
    /// This method retrieves detailed information regarding a <c>TRANSFER</c> transaction type. A <c>TRANSFER</c> is a monetary transaction type that involves a seller transferring money to eBay for reimbursement of one or more charges. For example, when a seller reimburses eBay for a buyer refund.
    /// If an ID is passed into the URI that is an identifier for another transaction type, this call will return an http status code of <c>404 Not found</c>.
    /// </summary>
    public Task<Transfer?> GetTransferAsync(
        string xEBAYCMARKETPLACEID,
        string transferId,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Transfer[transferId].GetAsync(cfg =>
        {
            cfg.Headers.TryAdd("X-EBAY-C-MARKETPLACE-ID", xEBAYCMARKETPLACEID);
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));

    /// <summary>
    /// Important! </b>"&gt;<b>Important!</b> Due to EU &amp; UK Payments regulatory requirements, an additional security verification via Digital Signatures is required for certain API calls that are made on behalf of EU/UK sellers, including all <b>Finances API</b> methods. Please refer to <see href="/develop/guides/digital-signatures-for-apis">Digital Signatures for APIs</see> to learn more on the impacted APIs and the process to create signatures to be included in the HTTP payload.
    /// <b>Note:</b> The <b>Finances API</b> does not support <see href="https://www.ebay.com/sellercenter/ebay-for-business/multi-user-account-access">Team Access</see>. Financial information, such as payouts or transactions, is only returned for the user that makes the call. You cannot use any of the methods in this API to return financial information for another user.
    /// This method retrieves filtered billing activities of the seller. Returned results are filtered through query parameters such as date range, activity ID, listing ID, or order ID. Sorting and pagination features help organize and navigate returned activities efficiently.
    /// </summary>
    public Task<BillingActivityResponse?> GetBillingActivitiesAsync(
        string? acceptLanguage = null,
        string? filter = null,
        string? limit = null,
        string? offset = null,
        string? sort = null,
        string? accessToken = null,
        RequestOptions? requestOptions = null,
        CancellationToken ct = default)
        => RestInvocation.CallAsync(
            _tokenContext,
            accessToken,
            () => _kiota.Billing_activity.GetAsync(cfg =>
        {
            if (filter is not null)
            {
                cfg.QueryParameters.Filter = filter;
            }
            if (limit is not null)
            {
                cfg.QueryParameters.Limit = limit;
            }
            if (offset is not null)
            {
                cfg.QueryParameters.Offset = offset;
            }
            if (sort is not null)
            {
                cfg.QueryParameters.Sort = sort;
            }
            if (acceptLanguage is not null)
            {
                cfg.Headers.TryAdd("Accept-Language", acceptLanguage);
            }
            RestInvocation.ApplyHeaders(cfg.Headers, _config, requestOptions);
        }, ct));
}
