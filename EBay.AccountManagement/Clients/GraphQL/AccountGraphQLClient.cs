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
using EBay.AccountManagement.Generated.GraphQL;
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

    public Task<IAddToWatchList_AddToWatchList?> AddToWatchListAsync(
        AddToWatchListInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.AddToWatchList.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.AddToWatchList;
        });

    public Task<ICreateOrReplaceUserNote_CreateOrReplaceUserNote?> CreateOrReplaceUserNoteAsync(
        CreateOrReplaceUserNoteInput? input = null,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.CreateOrReplaceUserNote.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreateOrReplaceUserNote;
        });

    public Task<IDeleteUserNote_DeleteUserNote?> DeleteUserNoteAsync(
        DeleteUserNoteInput? input = null,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.DeleteUserNote.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.DeleteUserNote;
        });

    public Task<IItemBiddingActivity_ItemBiddingActivity?> ItemBiddingActivityAsync(
        ItemBiddingActivityInput input,
        PageInfoInput? activeBidHistoryRecordsPaginationInput = null,
        PageInfoInput? cancelledBidHistoryRecordsPaginationInput = null,
        PageInfoInput? retractedBidHistoryRecordsPaginationInput = null,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.ItemBiddingActivity.ExecuteAsync(input, activeBidHistoryRecordsPaginationInput, cancelledBidHistoryRecordsPaginationInput, retractedBidHistoryRecordsPaginationInput, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ItemBiddingActivity;
        });

    public Task<IRemoveAllFromWatchList_RemoveAllFromWatchList?> RemoveAllFromWatchListAsync(
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.RemoveAllFromWatchList.ExecuteAsync(ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.RemoveAllFromWatchList;
        });

    public Task<IRemoveFromWatchList_RemoveFromWatchList?> RemoveFromWatchListAsync(
        RemoveFromWatchListInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.RemoveFromWatchList.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.RemoveFromWatchList;
        });

    public Task<IUser_User?> UserAsync(
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.User.ExecuteAsync(ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.User;
        });

    public Task<IUserBiddingActivity_UserBiddingActivity?> UserBiddingActivityAsync(
        UserBiddingActivityInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.UserBiddingActivity.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UserBiddingActivity;
        });

    public Task<IUserCustomLists_UserCustomLists?> UserCustomListsAsync(
        UserCustomListsInput? input = null,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.UserCustomLists.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UserCustomLists;
        });

    public Task<IUserWatchList_UserWatchList?> UserWatchListAsync(
        UserWatchListInput? input = null,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            EnsureTypedClientAvailable();
            var result = await GraphQLInvocation.ExecuteAsync(() => _client!.UserWatchList.ExecuteAsync(input, ct));
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UserWatchList;
        });

    public Task<JsonDocument> ExecuteRawUnsafeAsync(
        string query, object? variables = null, string? accessToken = null, CancellationToken ct = default)
        => _dynamic.ExecuteRawUnsafeAsync(query, variables, accessToken, ct);
}
