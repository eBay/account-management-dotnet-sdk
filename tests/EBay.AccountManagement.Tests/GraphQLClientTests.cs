using System.Net;
using System.Text.Json;
using EBay.AccountManagement.Configuration;
using EBay.AccountManagement.Exceptions;
using EBay.AccountManagement.Generated.GraphQL;
using Xunit;

namespace EBay.AccountManagement.Tests;

public sealed class GraphQLClientTests
{
    private const string RemovedResponse = """
        {"data":{"removeFromWatchList":{"__typename":"RemoveFromWatchListSuccess","numberOfItemsUnwatched":1,
          "watchListCount":4,"removedItems":[{"__typename":"Item","itemId":"110000000001"}],"watchListMaximum":300}}}
        """;

    [Fact]
    public async Task Typed_operation_sends_document_and_parses_union_result()
    {
        var sdk = new TestSdk { Handler = { ResponseBody = RemovedResponse } };

        var result = await sdk.Client.GraphQL.RemoveFromWatchListAsync(Remove());

        var success = Assert.IsAssignableFrom<IRemoveFromWatchList_RemoveFromWatchList_RemoveFromWatchListSuccess>(result);
        Assert.Equal(1, success.NumberOfItemsUnwatched);
        Assert.Equal("110000000001", Assert.Single(success.RemovedItems).ItemId);

        var request = sdk.Handler.Request!.RootElement;
        Assert.Equal("RemoveFromWatchList", request.GetProperty("operationName").GetString());
        Assert.StartsWith("mutation RemoveFromWatchList(", request.GetProperty("query").GetString());
        Assert.Equal("110000000001", Input(sdk).GetProperty("itemsToRemove")[0].GetProperty("itemId").GetString());
        Assert.Equal("Bearer config-token", sdk.Handler.Authorization);
    }

    [Fact]
    public async Task Per_call_access_token_overrides_configuration()
    {
        var sdk = new TestSdk { Handler = { ResponseBody = RemovedResponse } };

        await sdk.Client.GraphQL.RemoveFromWatchListAsync(Remove(), accessToken: "call-token");

        Assert.Equal("Bearer call-token", sdk.Handler.Authorization);
    }

    [Fact]
    public async Task Unset_input_fields_are_omitted_and_explicit_null_is_sent()
    {
        var sdk = new TestSdk { Handler = { ResponseBody = """{"data":{"createOrReplaceUserNote":null}}""" } };

        await Record.ExceptionAsync(() => sdk.Client.GraphQL.CreateOrReplaceUserNoteAsync(Note()));
        Assert.False(Input(sdk).TryGetProperty("orderLineItemId", out _));

        var withNull = Note();
        withNull.OrderLineItemId = null;
        await Record.ExceptionAsync(() => sdk.Client.GraphQL.CreateOrReplaceUserNoteAsync(withNull));
        Assert.Equal(JsonValueKind.Null, Input(sdk).GetProperty("orderLineItemId").ValueKind);
    }

    [Fact]
    public async Task GraphQL_errors_throw_GraphQLException_with_every_message()
    {
        var sdk = new TestSdk
        {
            Handler = { ResponseBody = """{"errors":[{"message":"boom"},{"message":"bang"}],"data":null}""" }
        };

        var error = await Assert.ThrowsAsync<GraphQLException>(() => sdk.Client.GraphQL.RemoveFromWatchListAsync(Remove()));

        Assert.Equal(["boom", "bang"], error.Errors);
    }

    [Fact]
    public async Task Http_401_throws_UnauthorizedException()
    {
        var sdk = new TestSdk { Handler = { Status = HttpStatusCode.Unauthorized } };

        await Assert.ThrowsAsync<UnauthorizedException>(() => sdk.Client.GraphQL.RemoveFromWatchListAsync(Remove()));
    }

    [Fact]
    public async Task Http_500_throws_SdkException_with_status()
    {
        var sdk = new TestSdk { Handler = { Status = HttpStatusCode.InternalServerError } };

        var error = await Assert.ThrowsAsync<SdkException>(() => sdk.Client.GraphQL.RemoveFromWatchListAsync(Remove()));

        Assert.Contains("HTTP 500", error.Message);
    }

    [Fact]
    public async Task Raw_queries_map_401_to_UnauthorizedException()
    {
        var sdk = new TestSdk { Handler = { Status = HttpStatusCode.Unauthorized } };

        await Assert.ThrowsAsync<UnauthorizedException>(() =>
            sdk.Client.GraphQL.ExecuteRawUnsafeAsync("query { __typename }"));
    }

    [Fact]
    public void Standalone_client_exposes_typed_graphql_operations()
    {
        // Before the in-house codegen, typed operations required DI and threw NotSupportedException.
        using var client = new AccountManagementClient(new AccountManagementConfiguration { AccessToken = "t" });

        Assert.NotNull(client.GraphQL);
    }

    private static RemoveFromWatchListInput Remove() => new()
    {
        ItemsToRemove = [new WatchListEntryToRemove { ItemId = "110000000001" }]
    };

    private static CreateOrReplaceUserNoteInput Note() => new() { ItemId = "110000000001", UserNote = "ship in gift box" };

    private static JsonElement Input(TestSdk sdk) =>
        sdk.Handler.Request!.RootElement.GetProperty("variables").GetProperty("input");
}
