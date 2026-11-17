using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EBay.AccountManagement;
using EBay.AccountManagement.Configuration;
using EBay.AccountManagement.Exceptions;

namespace EBay.AccountManagement.CSharp73
{
    /// <summary>Typical consumer code written in C# 7.3; it only has to compile.</summary>
    public static class ConsumerUsage
    {
        public static async Task<string> RunAsync()
        {
            var config = new AccountManagementConfiguration
            {
                AccessToken = "token",
                HttpTimeout = TimeSpan.FromSeconds(60),
                DiagnosticLogging = true,
                AccountV1BaseUrl = "https://ebay-proxy.example.com/sell/account/v1",
                AllowedHosts = { "ebay-proxy.example.com" },
                DefaultRequestOptions = new RequestOptions
                {
                    Headers = new Dictionary<string, string> { { "X-EBAY-C-MARKETPLACE-ID", "EBAY_US" } }
                }
            };

            using (var client = new AccountManagementClient(config))
            {
                try
                {
                    var result = await client.AccountV1.GetPrivilegesAsync();
                    return result != null ? "ok" : "empty";
                }
                catch (UnauthorizedException)
                {
                    return "unauthorized";
                }
            }
        }
    }
}
