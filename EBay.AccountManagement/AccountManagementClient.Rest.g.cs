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


using EBay.AccountManagement.Clients.Rest;
using EBay.AccountManagement.Configuration;
using EBay.AccountManagement.Generated.AccountV1;
using EBay.AccountManagement.Generated.AccountV2;
using EBay.AccountManagement.Generated.Finances;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace EBay.AccountManagement;

public partial class AccountManagementClient
{
    public AccountV1Client AccountV1 { get; private set; } = null!;
    public AccountV2Client AccountV2 { get; private set; } = null!;
    public FinancesClient Finances { get; private set; } = null!;

    private void InitializeRestClients(
        IAuthenticationProvider auth,
        HttpClient accountV1Client,
        HttpClient accountV2Client,
        HttpClient financesClient,
        AccountManagementConfiguration config,
        ITokenContext tokenContext)
    {
        AccountV1 = new AccountV1Client(new AccountV1ApiClient(
            new HttpClientRequestAdapter(auth, httpClient: accountV1Client)),
            config, tokenContext);

        AccountV2 = new AccountV2Client(new AccountV2ApiClient(
            new HttpClientRequestAdapter(auth, httpClient: accountV2Client)),
            config, tokenContext);

        Finances = new FinancesClient(new FinancesApiClient(
            new HttpClientRequestAdapter(auth, httpClient: financesClient)),
            config, tokenContext);
    }
}
