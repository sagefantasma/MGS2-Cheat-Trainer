#!/usr/bin/env bash
set -e

PROJECT="MGS2_CheatTrainer_V2/MGS2_CheatTrainer_V2.csproj"
VERSION=$(dotnet build "$PROJECT" -getProperty:Version)
BASE="v2_releases/$VERSION"

for RID in win-x64 linux-x64; do
    dotnet publish "$PROJECT" \
        -c Debug \
        -r "$RID" \
        --self-contained true \
        -p:PublishSingleFile=true \
        -p:IncludeNativeLibrariesForSelfExtract=true \
        -p:DebugType=embedded \
        -o "$BASE/$RID"
done
