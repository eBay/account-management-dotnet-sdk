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

echo "========================================="
echo "Account Management SDK - Regeneration"
echo "========================================="

# Restore dotnet tools
echo "Restoring .NET tools..."
dotnet tool restore

# Regenerate Kiota clients
echo ""
echo "Regenerating REST clients with Kiota..."

echo "  - AccountV1 client..."
dotnet kiota generate \
  -l csharp \
  -d specs/sell_account_v1_oas3.yaml \
  -c AccountV1ApiClient \
  -n EBay.AccountManagement.Generated.AccountV1 \
  -o EBay.AccountManagement/Generated/AccountV1 \
  --ebc --co

echo "  - AccountV2 client..."
dotnet kiota generate \
  -l csharp \
  -d specs/sell_account_v2_oas3.yaml \
  -c AccountV2ApiClient \
  -n EBay.AccountManagement.Generated.AccountV2 \
  -o EBay.AccountManagement/Generated/AccountV2 \
  --ebc --co

echo "  - Finances client..."
dotnet kiota generate \
  -l csharp \
  -d specs/sell_finances_v1_oas3.yaml \
  -c FinancesApiClient \
  -n EBay.AccountManagement.Generated.Finances \
  -o EBay.AccountManagement/Generated/Finances \
  --ebc --co

echo "Adding license headers to generated files..."
./add-license-headers.sh ./EBay.AccountManagement/Generated

# Build the project (also triggers StrawberryShake Roslyn source generator for GraphQL)
echo ""
echo "Building the SDK..."
DOTNET_ROLL_FORWARD=LatestMajor dotnet build EBay.AccountManagement/EBay.AccountManagement.csproj

echo ""
echo "========================================="
echo "Done! SDK regenerated successfully."
echo "========================================="
