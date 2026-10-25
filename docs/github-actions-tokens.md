# GitHub Actions token requirements

## Use the automatically issued `GITHUB_TOKEN`

GitHub creates a short-lived `github.token` for each workflow run. Use it for
operations confined to this repository, and grant only the permissions needed
by the job:

| Workflow operation | Credential | Permission |
| --- | --- | --- |
| Check out source in the isolated PR build job | `github.token` | `contents: read` |
| Add or update a pull request preview comment | `github.token` | `issues: write`, `pull-requests: write` |
| Publish the documentation output to this repository's `gh-pages` branch | `github.token` | `contents: write` |

The single `Preview` workflow is triggered by `pull_request_target`. Its build
job checks out the PR merge commit with `persist-credentials: false`, has only
`contents: read`, and receives no Cloudflare secret. A separate deployment job
downloads the same-run artifact and publishes the static files to the Cloudflare
Pages branch `preview-pr-<PR>`. Artifact transfer uses the Actions runtime
credential; it does not need a GitHub PAT. The stable preview URL is
`https://preview-pr-<PR>.ant-design-blazor.pages.dev`.

## Keep a PAT only for style synchronization

`style-sync.yml` uses `GH_PUSH_TOKEN` to push a generated branch and open a pull
request. This must remain a PAT (preferably a fine-grained PAT limited to this
repository with **Contents: read and write** and **Pull requests: read and
write**): events created by `GITHUB_TOKEN` do not start the normal follow-up
workflows, so the generated pull request would not receive its expected CI.

## Cloudflare credentials

`CF_TOKEN` is a Cloudflare API token, not a GitHub PAT. It is used only by the
Cloudflare Pages deployment jobs. `CF_ID` is the Cloudflare account ID.
Other credentials used by workflows (for example NuGet keys, Azure publish
profiles, and Algolia keys) belong to those services and are not GitHub PATs.
