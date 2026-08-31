#!/usr/bin/env bash

# Copyright 2026 eBay Inc.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

set -euo pipefail

echo "Restoring tools..."
dotnet tool restore

echo "Regenerating REST clients via Kiota..."
dotnet kiota generate \
  -l csharp \
  -d specs/openapi/account-v1.json \
  -c AccountV1ApiClient \
  -n EBay.AccountManagement.Generated.AccountV1 \
  -o EBay.AccountManagement/Generated/AccountV1 \
  --ebc --co

dotnet kiota generate \
  -l csharp \
  -d specs/openapi/account-v2.json \
  -c AccountV2ApiClient \
  -n EBay.AccountManagement.Generated.AccountV2 \
  -o EBay.AccountManagement/Generated/AccountV2 \
  --ebc --co

dotnet kiota generate \
  -l csharp \
  -d specs/openapi/finances.json \
  -c FinancesApiClient \
  -n EBay.AccountManagement.Generated.Finances \
  -o EBay.AccountManagement/Generated/Finances \
  --ebc --co

echo "Building (triggers StrawberryShake GraphQL codegen)..."
DOTNET_ROLL_FORWARD=LatestMajor dotnet build EBay.AccountManagement/EBay.AccountManagement.csproj

echo "Done."
