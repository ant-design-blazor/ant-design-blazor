# GitHub Actions token requirements

## Use the automatically issued `GITHUB_TOKEN`

GitHub creates a short-lived `github.token` for each workflow run. Use it for
operations confined to this repository, and grant only the permissions needed
by the job:

| Workflow operation | Credential | Permission |
| --- | --- | --- |
| Check out source in the PR build | `github.token` | `contents: read` |
| Download the PR build artifact in the deployment workflow | `github.token` | `actions: read` |
| Add or update a pull request preview comment | `github.token` | `issues: write` |
| Publish the documentation output to this repository's `gh-pages` branch | `github.token` | `contents: write` |

The preview build does not need a GitHub secret: it builds untrusted pull
request code and uploads its output as an Actions artifact. The separate
`workflow_run` deployment downloads that artifact with its own `github.token`,
then publishes the files to the Cloudflare Pages branch `preview-pr-<PR>`.
Its stable URL is `https://preview-pr-<PR>.ant-design-blazor.pages.dev`.

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
