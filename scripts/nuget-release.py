#!/usr/bin/env python3
"""Inspect the SDK package, look up its published source, and extract release notes."""

import argparse
import json
import os
import re
import sys
import time
from pathlib import Path
from urllib.error import HTTPError
from urllib.request import urlopen
from xml.etree import ElementTree
from zipfile import ZipFile

PACKAGE_ID = "EBay.AccountManagement"
REPOSITORY = "https://github.com/eBay/account-management-dotnet-sdk"
SERVICE_INDEX = "https://api.nuget.org/v3/index.json"


def metadata(xml):
    root = ElementTree.fromstring(xml)
    return root.find("{*}metadata")


def identity(manifest):
    if manifest.findtext("{*}id", "").lower() != PACKAGE_ID.lower():
        raise ValueError("Unexpected NuGet package ID")
    repo = manifest.find("{*}repository")
    if repo is None or repo.get("url", "").removesuffix(".git").lower() != REPOSITORY.lower():
        raise ValueError("Package has no matching source repository")
    commit = repo.get("commit", "")
    if not re.fullmatch("[0-9a-f]{40}", commit):
        raise ValueError("Package has no source commit")
    return manifest.findtext("{*}version"), commit


def normalized(version):
    # The flat container lowercases versions and omits build metadata and a zero revision.
    base, sep, label = version.partition("+")[0].lower().partition("-")
    parts = [str(int(part)) for part in base.split(".")]
    parts += ["0"] * (3 - len(parts))
    if len(parts) == 4 and parts[-1] == "0":
        parts.pop()
    return ".".join(parts) + sep + label


def read_url(url):
    with urlopen(url, timeout=30) as response:
        return response.read()


def published_commit(version, wait=False):
    index = json.loads(read_url(SERVICE_INDEX))
    base = next(r["@id"] for r in index["resources"] if r["@type"] == "PackageBaseAddress/3.0.0")
    package = PACKAGE_ID.lower()
    url = f"{base.rstrip('/')}/{package}/{normalized(version)}/{package}.nuspec"
    deadline = time.monotonic() + (3600 if wait else 0)
    while True:
        try:
            manifest = metadata(read_url(url))
        except HTTPError as error:
            if error.code != 404:
                raise
            if not wait:
                return None
            if time.monotonic() >= deadline:
                raise TimeoutError(f"Check gallery validation for {PACKAGE_ID} {version}") from error
            time.sleep(30)
            continue
        actual_version, commit = identity(manifest)
        if normalized(actual_version) != normalized(version):
            raise ValueError("Published package version differs from requested version")
        return commit


def release_notes(changelog, version):
    headings = list(re.finditer(r"^## ([^\s]+)[^\n]*$", changelog, re.MULTILINE))
    matches = [i for i, h in enumerate(headings) if h[1] == version]
    if len(matches) != 1:
        raise ValueError(f"Expected one changelog entry for {version}")
    i = matches[0]
    end = headings[i + 1].start() if i + 1 < len(headings) else len(changelog)
    notes = changelog[headings[i].end():end].strip()
    if not notes:
        raise ValueError(f"Empty changelog entry for {version}")
    return notes


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    commands = parser.add_subparsers(dest="command", required=True)
    commands.add_parser("prepare").add_argument("directory", type=Path)
    commands.add_parser("confirm").add_argument("version")
    commands.add_parser("notes").add_argument("version")
    args = parser.parse_args()
    if args.command == "notes":
        print(release_notes(sys.stdin.read(), args.version))
    elif args.command == "confirm":
        print(published_commit(args.version, wait=True))
    else:
        path, = args.directory.glob("*.nupkg")
        with ZipFile(path) as archive:
            nuspec, = [n for n in archive.namelist() if n.endswith(".nuspec")]
            version, commit = identity(metadata(archive.read(nuspec)))
            frameworks = {n.split("/")[1] for n in archive.namelist()
                          if n.startswith("lib/") and n.endswith("/EBay.AccountManagement.dll")}
        if frameworks != {"net472", "net8.0", "net9.0", "net10.0"}:
            raise ValueError(f"Unexpected packaged frameworks: {frameworks}")
        if commit != os.environ["GITHUB_SHA"]:
            raise ValueError("Packed source commit differs from workflow commit")
        release_notes(Path("CHANGELOG.md").read_text(), version)
        missing = published_commit(version) is None
        print(f"{PACKAGE_ID} {version} at {commit}: {'ready to publish' if missing else 'already published'}")
        with open(os.environ["GITHUB_OUTPUT"], "a") as out:
            print(f"version={version}\nmissing={str(missing).lower()}", file=out)


if __name__ == "__main__":
    main()
