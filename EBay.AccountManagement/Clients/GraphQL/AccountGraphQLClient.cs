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
    private readonly GraphQLExecutor _executor;
    private readonly DynamicGraphQLClient _dynamic;
    private readonly ITokenContext _tokenContext;

    internal AccountGraphQLClient(
        GraphQLExecutor executor,
        DynamicGraphQLClient dynamic,
        ITokenContext tokenContext)
        => (_executor, _dynamic, _tokenContext) = (executor, dynamic, tokenContext);

    public Task<IAddToWatchList_AddToWatchList?> AddToWatchListAsync(
        AddToWatchListInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                AddToWatchListOperation.OperationName,
                AddToWatchListOperation.Document,
                writer => AddToWatchListOperation.WriteVariables(writer, input),
                AddToWatchListOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.AddToWatchList;
        });

    public Task<ICreateOrReplaceUserNote_CreateOrReplaceUserNote?> CreateOrReplaceUserNoteAsync(
        CreateOrReplaceUserNoteInput? input = null,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                CreateOrReplaceUserNoteOperation.OperationName,
                CreateOrReplaceUserNoteOperation.Document,
                writer => CreateOrReplaceUserNoteOperation.WriteVariables(writer, input),
                CreateOrReplaceUserNoteOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.CreateOrReplaceUserNote;
        });

    public Task<IDeleteUserNote_DeleteUserNote?> DeleteUserNoteAsync(
        DeleteUserNoteInput? input = null,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                DeleteUserNoteOperation.OperationName,
                DeleteUserNoteOperation.Document,
                writer => DeleteUserNoteOperation.WriteVariables(writer, input),
                DeleteUserNoteOperation.Parse,
                ct)).ConfigureAwait(false);
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
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                ItemBiddingActivityOperation.OperationName,
                ItemBiddingActivityOperation.Document,
                writer => ItemBiddingActivityOperation.WriteVariables(writer, input, activeBidHistoryRecordsPaginationInput, cancelledBidHistoryRecordsPaginationInput, retractedBidHistoryRecordsPaginationInput),
                ItemBiddingActivityOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.ItemBiddingActivity;
        });

    public Task<IRemoveAllFromWatchList_RemoveAllFromWatchList?> RemoveAllFromWatchListAsync(
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                RemoveAllFromWatchListOperation.OperationName,
                RemoveAllFromWatchListOperation.Document,
                null,
                RemoveAllFromWatchListOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.RemoveAllFromWatchList;
        });

    public Task<IRemoveFromWatchList_RemoveFromWatchList?> RemoveFromWatchListAsync(
        RemoveFromWatchListInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                RemoveFromWatchListOperation.OperationName,
                RemoveFromWatchListOperation.Document,
                writer => RemoveFromWatchListOperation.WriteVariables(writer, input),
                RemoveFromWatchListOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.RemoveFromWatchList;
        });

    public Task<IUser_User?> UserAsync(
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                UserOperation.OperationName,
                UserOperation.Document,
                null,
                UserOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.User;
        });

    public Task<IUserBiddingActivity_UserBiddingActivity?> UserBiddingActivityAsync(
        UserBiddingActivityInput input,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                UserBiddingActivityOperation.OperationName,
                UserBiddingActivityOperation.Document,
                writer => UserBiddingActivityOperation.WriteVariables(writer, input),
                UserBiddingActivityOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UserBiddingActivity;
        });

    public Task<IUserCustomLists_UserCustomLists?> UserCustomListsAsync(
        UserCustomListsInput? input = null,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                UserCustomListsOperation.OperationName,
                UserCustomListsOperation.Document,
                writer => UserCustomListsOperation.WriteVariables(writer, input),
                UserCustomListsOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UserCustomLists;
        });

    public Task<IUserWatchList_UserWatchList?> UserWatchListAsync(
        UserWatchListInput? input = null,
        string? accessToken = null,
        CancellationToken ct = default)
        => _tokenContext.RunAsync(accessToken, async () =>
        {
            var result = await GraphQLInvocation.ExecuteAsync(() => _executor.ExecuteAsync(
                UserWatchListOperation.OperationName,
                UserWatchListOperation.Document,
                writer => UserWatchListOperation.WriteVariables(writer, input),
                UserWatchListOperation.Parse,
                ct)).ConfigureAwait(false);
            GraphQLInvocation.EnsureNoErrors(result);
            return result.Data?.UserWatchList;
        });

    public Task<JsonDocument> ExecuteRawUnsafeAsync(
        string query, object? variables = null, string? accessToken = null, CancellationToken ct = default)
        => _dynamic.ExecuteRawUnsafeAsync(query, variables, accessToken, ct);
}
