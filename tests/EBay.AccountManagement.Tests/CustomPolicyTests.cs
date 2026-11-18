using EBay.AccountManagement.Generated.AccountV1.Models;
using Xunit;

namespace EBay.AccountManagement.Tests;

public sealed class CustomPolicyTests
{
    [Fact]
    public async Task Custom_policies_response_is_parsed()
    {
        // Regression: the portal spec had CustomPolicyResponse's description inside its properties, which broke
        // generation (api-sdk-definitions #62). The response has no "description" property.
        var sdk = new TestSdk
        {
            Handler =
            {
                ResponseBody = """{"customPolicies":[{"customPolicyId":"1","name":"Takeback","policyType":"TAKE_BACK"}],"total":1}"""
            }
        };

        var response = await sdk.Client.AccountV1.GetCustomPoliciesAsync();

        Assert.Equal("1", Assert.Single(response!.CustomPolicies!).CustomPolicyId);
        Assert.Null(typeof(CustomPolicyResponse).GetProperty("Description"));
    }
}
