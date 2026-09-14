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
using EBay.AccountManagement.Exceptions;
using EBay.AccountManagement.Generated.GraphQL;
using StrawberryShake;
using System.Net;
using System.Text.Json;

namespace EBay.AccountManagement.Clients.GraphQL;

public sealed class AccountGraphQLClient
{
    private readonly IAccountManagementGraphQLClient? _client;
    private readonly DynamicGraphQLClient _dynamic;
    private readonly ITokenContext _tokenContext;

    internal AccountGraphQLClient(
        IAccountManagementGraphQLClient? client,
        DynamicGraphQLClient dynamic,
        ITokenContext tokenContext)
        => (_client, _dynamic, _tokenContext) = (client, dynamic, tokenContext);

    private void EnsureTypedClientAvailable()
    {
        if (_client is null)
            throw new NotSupportedException(
                "Typed GraphQL operations require DI. Use ExecuteRawUnsafeAsync for standalone mode.");
    }

    // ── User ──────────────────────────────────────────────────────────────────

    public Task<IUser_User?> UserAsync(
        string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.User.ExecuteAsync(ct));
            EnsureNoErrors(result);
            return result.Data?.User;
        });

    // ── Watch List ────────────────────────────────────────────────────────────

    public Task<IUserWatchList_UserWatchList?> UserWatchListAsync(
        UserWatchListInput? input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.UserWatchList.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.UserWatchList;
        });

    public Task<IAddToWatchList_AddToWatchList?> AddToWatchListAsync(
        AddToWatchListInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.AddToWatchList.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.AddToWatchList;
        });

    public Task<IRemoveFromWatchList_RemoveFromWatchList?> RemoveFromWatchListAsync(
        RemoveFromWatchListInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.RemoveFromWatchList.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.RemoveFromWatchList;
        });

    public Task<IRemoveAllFromWatchList_RemoveAllFromWatchList?> RemoveAllFromWatchListAsync(
        string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.RemoveAllFromWatchList.ExecuteAsync(ct));
            EnsureNoErrors(result);
            return result.Data?.RemoveAllFromWatchList;
        });

    // ── Custom Lists ──────────────────────────────────────────────────────────

    public Task<IUserCustomLists_UserCustomLists?> UserCustomListsAsync(
        UserCustomListsInput? input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.UserCustomLists.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.UserCustomLists;
        });

    // ── User Notes ────────────────────────────────────────────────────────────

    public Task<ICreateOrReplaceUserNote_CreateOrReplaceUserNote?> CreateOrReplaceUserNoteAsync(
        CreateOrReplaceUserNoteInput? input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.CreateOrReplaceUserNote.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.CreateOrReplaceUserNote;
        });

    public Task<IDeleteUserNote_DeleteUserNote?> DeleteUserNoteAsync(
        DeleteUserNoteInput? input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.DeleteUserNote.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.DeleteUserNote;
        });

    // ── Auction ───────────────────────────────────────────────────────────────

    public Task<IUserBiddingActivity_UserBiddingActivity?> UserBiddingActivityAsync(
        UserBiddingActivityInput input, string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.UserBiddingActivity.ExecuteAsync(input, ct));
            EnsureNoErrors(result);
            return result.Data?.UserBiddingActivity;
        });

    public Task<IItemBiddingActivity_ItemBiddingActivity?> ItemBiddingActivityAsync(
        ItemBiddingActivityInput input,
        PageInfoInput? activeBidHistoryRecordsPaginationInput = null,
        PageInfoInput? cancelledBidHistoryRecordsPaginationInput = null,
        PageInfoInput? retractedBidHistoryRecordsPaginationInput = null,
        string? accessToken = null, CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await ExecuteAsync(() => _client!.ItemBiddingActivity.ExecuteAsync(
                input,
                activeBidHistoryRecordsPaginationInput,
                cancelledBidHistoryRecordsPaginationInput,
                retractedBidHistoryRecordsPaginationInput,
                ct));
            EnsureNoErrors(result);
            return result.Data?.ItemBiddingActivity;
        });

    // ── Dynamic escape hatch ──────────────────────────────────────────────────

    /// <summary>
    /// Executes an arbitrary GraphQL query string against the API.
    /// <para>
    /// <b>WARNING — injection risk.</b> Never interpolate user-supplied input directly into
    /// <paramref name="query"/>. Always pass dynamic values via <paramref name="variables"/>
    /// so the server handles escaping. Treat this method like a raw SQL string: safe only
    /// with static, developer-controlled query text.
    /// </para>
    /// </summary>
    public Task<JsonDocument> ExecuteRawUnsafeAsync(
        string query, object? variables = null, string? accessToken = null, CancellationToken ct = default)
        => _dynamic.ExecuteRawUnsafeAsync(query, variables, accessToken, ct);

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static async Task<IOperationResult<T>> ExecuteAsync<T>(Func<Task<IOperationResult<T>>> call)
        where T : class
    {
        try { return await call(); }
        catch (Exception ex) when (ex is not SdkException) { throw WrapException(ex); }
    }

    private static SdkException WrapException(Exception ex) => ex switch
    {
        HttpRequestException { StatusCode: HttpStatusCode.Unauthorized } => new UnauthorizedException(ex),
        HttpRequestException { StatusCode: { } code } http =>
            new SdkException($"GraphQL request failed with HTTP {(int)code}: {http.Message}", ex),
        HttpRequestException http =>
            new SdkException($"GraphQL request failed (network error): {http.Message} — check that GraphQLBaseUrl is reachable.", ex),
        TaskCanceledException =>
            new SdkException("GraphQL request timed out — check that GraphQLBaseUrl is reachable and the network is accessible.", ex),
        InvalidOperationException { Message: var msg } when msg.Contains("execution attempt") =>
            new SdkException("GraphQL request returned no data; the endpoint may be unreachable or returned an unexpected response — check GraphQLBaseUrl.", ex),
        _ => new SdkException($"GraphQL request failed: {ex.Message}", ex)
    };

    private static void EnsureNoErrors<T>(IOperationResult<T> result)
        where T : class
    {
        if (result.Errors is { Count: > 0 })
        {
            var msgs = result.Errors
                .Select(e => e.Message ?? e.Code ?? "unknown error")
                .ToArray();
            throw new GraphQLException(msgs);
        }
    }
}
