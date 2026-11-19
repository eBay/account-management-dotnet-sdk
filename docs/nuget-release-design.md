# Publishing the eBay .NET SDKs to NuGet

**Design proposal · November 19, 2026 · Applies independently to each of the nine SDK repositories**

On every push to `main`, build and test the pushed commit, pack its SDK, and ask nuget.org whether that exact package ID and version already exists. An existing version skips NuGet login and upload, then checks its GitHub Release. A missing version proceeds to publication. The maintainer's release decision is the version committed in the SDK project. The same workflow handles a handwritten change, a generated change, and a retry.

## Tooling decision

| Project | How it publishes | What applies here |
| --- | --- | --- |
| Changesets | Contributors record release intent, the version command updates package manifests and changelogs, and the publish command checks npm for unpublished versions. | The registry check is a good fit. The standard publisher targets npm. Adopting the CLI here would add Node tooling and still require a NuGet publisher and a mapping to MSBuild versions. [Changesets](https://github.com/changesets/changesets) |
| Nager.Date | Its release workflow derives the version from a tag, packs the project with the .NET CLI, logs in to NuGet, and pushes with duplicate handling. | Adopt the native pack, login, and push steps. Our SDK projects already carry the release version, so a tag would add a second release decision. [Nager.Date release workflow](https://github.com/nager/Nager.Date/blob/main/.github/workflows/dotnet-release.yml) |
| Intility's Serilog.Enrichers.AzureClaims | GitHub Actions packs with an explicit version, requests a NuGet credential through OIDC, and uses the .NET CLI to push the result. | A small repository can own the whole publishing path using GitHub Actions and the SDK it already builds with. [Intility publish workflow](https://github.com/intility/serilog-enrichers-azureclaims/blob/main/.github/workflows/publish.yaml) |

**Recommendation.** Use GitHub Actions, MSBuild, the .NET CLI and NuGet Trusted Publishing. Keep each family's workflow and any helper scripts in that family's SDK repo. Its inputs are the checked-out sources, project metadata and nuget.org. Release automation needs no generator run, cross-repository checkout, central workflow, tag, or release PR. Contributors who want to ship a change under a new version update the library project's already-established `<Version>` and changelog in their SDK PR.

## Workflow contract

The inspected SDK checkouts each contain one top-level library project, a changelog, and a PR validation workflow. All nine libraries target `net472`, `net8.0`, `net9.0`, and `net10.0`. Some set `PackageId` explicitly and others inherit it. Each repo should commit its own `.github/workflows/publish.yml` with the following jobs.

1. **Validate the pushed SHA.** Trigger on `push` to `main` with no path filter. Enable `workflow_call` on the repo's existing `validate.yml` and call it locally from the publish workflow. Keep its PR trigger. This preserves the SDK's own framework matrix, dependency checks, analysis and Windows .NET Framework coverage. Keep validation permissions at `contents: read`.
2. **Pack once.** After validation passes, use a clean runner with the .NET 10 SDK. Select the SDK library project explicitly. Pack the library into an empty output directory. For Account Management the command is shown below. Packing performs a full multi-target Release build. Read the ID and version from the embedded nuspec in the single resulting primary package. Check its ID against the repo's intended public package ID and its library assets against the project's target frameworks. Require a matching changelog section and embed the source SHA in the package. Upload that exact file as a run-scoped artifact when the version is missing.
3. **Check availability without credentials.** Discover the NuGet V3 package base address from the public service index. Request the published nuspec by lowercase package ID and normalized version. A matching manifest establishes that the version exists, including an unlisted version. Only a 404 at the version-specific endpoint makes the package eligible for upload. Other errors fail the run. [NuGet package content API](https://learn.microsoft.com/en-us/nuget/api/package-base-address-resource)
4. **Publish only if missing.** Gate a separate job on successful validation, packaging and a positive eligibility result. Download the artifact from this run. Use the GitHub environment `nuget` restricted to `main`. Grant `id-token: write` only to this job. Exchange the job's OIDC token using `NuGet/login` with the configured NuGet profile username, then pass its temporary key to the push command below. Pin actions to reviewed commit SHAs as the SDK validation workflows do. The job only needs the artifact and the .NET CLI. [NuGet Trusted Publishing](https://learn.microsoft.com/en-us/nuget/nuget-org/trusted-publishing)
5. **Create or recover the GitHub Release.** After a successful upload or skip, read the published nuspec, allowing up to 60 minutes for gallery validation. Take its repository commit as the release source. Require the SDK repository URL and a commit reachable from main. Create or verify `v<version>` at that commit and publish the matching changelog section from that commit as the GitHub Release body. An existing release completes the run. A conflicting tag or missing provenance fails for maintainer investigation. This also recovers when another push advances main between a successful upload and a failed release step. [NuGet publishing lifecycle](https://learn.microsoft.com/en-us/nuget/nuget-org/publish-a-package)

Representative commands from the SDK root, with the publish job binding the single downloaded file and short-lived login output to the two shell variables:

```bash
dotnet pack EBay.AccountManagement/EBay.AccountManagement.csproj \
  --configuration Release -p:ContinuousIntegrationBuild=true --output artifacts

dotnet nuget push "$PACKAGE_FILE" --source https://api.nuget.org/v3/index.json \
  --api-key "$NUGET_API_KEY" --skip-duplicate --no-symbols
```

This design publishes one primary package per family. The lookup avoids redundant uploads and credentials on ordinary pushes. The duplicate flag handles an HTTP 409 if another run wins the race or the gallery index lags an earlier upload. Other push errors fail the job. Allow each pushed commit to finish so a subsequent push cannot cancel a distinct pending version. If two commits carry the same previously unpublished version, the first accepted package wins. Merge the version bump together with all changes intended for that release. [CLI push semantics](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-nuget-push)

## Rollout and recovery

Start with Account Management. Confirm the intended public package ID, eBay's ownership or right to first publication, and the version maintainers intend consumers to receive. Enabling this workflow on an unpublished project version makes the enabling merge itself a release. Configure a NuGet policy under the appropriate owner for `eBay/account-management-dotnet-sdk`, workflow filename `publish.yml`, and environment `nuget`. Scope the policy to that SDK's package ID and allow first publication if needed. Set the GitHub environment variable `NUGET_USER` to the NuGet profile username. Restrict the environment to the main branch. Ensure repository tag rules permit the workflow token to create version tags and repository Actions settings permit the release job to write contents. Repeat using each family's own repository and package ID. [NuGet policy setup](https://learn.microsoft.com/en-us/nuget/nuget-org/trusted-publishing)

| Pilot check | Expected result |
| --- | --- |
| Merge the approved first version and restore it from nuget.org in a clean consumer project. | The exact version restores and the run identifies its source SHA. |
| Push a docs-only change with that version. | Validation and packaging run. The lookup reports the version already exists. Login and push are skipped. |
| Merge an SDK change and version bump. | Only the new package version is uploaded. |
| Exercise a failed upload in a rehearsal, then rerun the original workflow after resolving the failure. | If the earlier upload never landed, the same version can publish. If it landed despite a lost response, the lookup or duplicate handling completes the retry. |
| Simulate a feed outage or failed build in a rehearsal. | The run fails visibly. A failed lookup never authorizes an upload. |

Use GitHub Actions **Re-run all jobs** on the original push for recovery. That rechecks registry state at the original SHA. For an indexing timeout, first inspect the gallery's validation status. For incorrect contents already released, fix the SDK, bump the project version, and merge again. After the pilot passes, carry the workflow to the remaining eight repositories through independent PRs. Each repo's Actions history and NuGet package page then provide its release record.
