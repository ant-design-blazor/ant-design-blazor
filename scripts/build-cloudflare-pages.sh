#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/.." && pwd)"
OUTPUT_DIR="${1:-dist}"
ENABLE_AOT="${ENABLE_AOT:-true}"
KEEP_ORIGINALS="${KEEP_ORIGINALS:-false}"

if [[ "${OUTPUT_DIR}" != /* ]]; then
  OUTPUT_DIR="${REPO_ROOT}/${OUTPUT_DIR}"
fi

DOTNET_INSTALL_DIR="${REPO_ROOT}/.cloudflare-dotnet"
PUBLISH_DIR="${REPO_ROOT}/.cloudflare-publish"

cleanup() {
  rm -rf "${PUBLISH_DIR}"
}
trap cleanup EXIT

cd "${REPO_ROOT}"
rm -rf "${OUTPUT_DIR}" "${PUBLISH_DIR}"
mkdir -p "${OUTPUT_DIR}"

if ! command -v dotnet >/dev/null 2>&1 || ! dotnet --list-sdks | grep -q '^10\.'; then
  curl -fsSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin \
    --channel 10.0 \
    --install-dir "${DOTNET_INSTALL_DIR}"
  export DOTNET_ROOT="${DOTNET_INSTALL_DIR}"
  export PATH="${DOTNET_INSTALL_DIR}:${PATH}"
fi

if [[ "${ENABLE_AOT}" == "true" ]]; then
  dotnet workload install wasm-tools
fi
npm install

VERSION="${PACKAGE_VERSION:-}"
if [[ -z "${VERSION}" ]]; then
  VERSION="$(curl -fsSL https://api.github.com/repos/ant-design-blazor/ant-design-blazor/tags \
    | sed -n 's/.*"name": "\([^"]*\)".*/\1/p' | head -1 || true)"
fi
VERSION="${VERSION:-${CF_PAGES_COMMIT_SHA:-dev}}"

echo "Building docs version: ${VERSION}"
echo "/* updated $(date '+%Y-%m-%d %H:%M:%S') */" >> site/AntDesign.Docs.Wasm/wwwroot/service-worker.published.js
cp -rf scripts/gh-pages/* scripts/gh-pages/.nojekyll scripts/gh-pages/.spa site/AntDesign.Docs.Wasm/wwwroot
sed -i "s/{version}/${VERSION}/g" site/AntDesign.Docs/Shared/HeaderMenu.razor
sed -i "s/{version}/${VERSION}/g" site/AntDesign.Docs.Wasm/wwwroot/version.json
sed -i "s/{version}/${VERSION}/g" site/AntDesign.Docs.Wasm/wwwroot/index.html
sed -i "s/{version}/${VERSION}/g" site/AntDesign.Docs.Wasm/wwwroot/service-worker.published.js

dotnet build
PUBLISH_ARGS=(./site/AntDesign.Docs.Wasm -c Release -o "${PUBLISH_DIR}")
if [[ "${ENABLE_AOT}" == "true" ]]; then
  PUBLISH_ARGS+=("-p:EnableAOT=true")
fi
dotnet publish "${PUBLISH_ARGS[@]}"
cp -rf "${PUBLISH_DIR}/wwwroot/." "${OUTPUT_DIR}/"
cp -f "${PUBLISH_DIR}/staticwebapp.config.json" "${OUTPUT_DIR}/" 2>/dev/null || true
echo "/* /index.html 200" > "${OUTPUT_DIR}/_redirects"

find "${OUTPUT_DIR}" -type f -name '*.html' -exec bash -c \
  'if [ "$(grep -o "<title>" "$1" | wc -l)" -gt 1 ]; then perl -0777 -i -pe "s/<title>.*?<\/title>//s" "$1"; fi' _ {} \;

if [[ "${KEEP_ORIGINALS}" != "true" ]]; then
  # Cloudflare Pages has a 25 MiB per-file limit. Blazor publish already creates
  # Brotli copies, so remove only originals that have a matching .br file.
  while IFS= read -r -d '' file; do
    if [[ -f "${file}.br" ]]; then
      rm "${file}"
    else
      echo "Missing Brotli resource: ${file}.br" >&2
      exit 1
    fi
  done < <(find "${OUTPUT_DIR}/_framework" -type f \( \
    -name '*.wasm' -o -name '*.dll' -o -name '*.pdb' -o -name '*.dat' \
    \) -print0)
fi

echo "Cloudflare Pages files generated in ${OUTPUT_DIR}"
