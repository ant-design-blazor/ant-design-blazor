---
order: 11
title: How to Contribute
---

The following is a set of guidelines for contributing to Ant Design Blazor. Please spend several minutes in reading these guidelines before you create an issue or pull request.

## Code of Conduct

We have adopted a [Code of Conduct](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/CODE_OF_CONDUCT.md) that we expect project participants to adhere to. Please read the full text so that you can understand what actions will and will not be tolerated.

## Open Development

All work on ant-design-blazor happens directly on [GitHub](https://github.com/ant-design-blazor/ant-design-blazor). Both core team members and external contributors send pull requests which go through the same review process.

## Bugs

We are using [GitHub Issues](https://github.com/ant-design-blazor/ant-design-blazor/issues) for bug tracing. The best way to get your bug fixed is using our [issue helper](https://ng.ant.design/issue-helper/#/en).

Before you reporting a bug, please make sure you've searched exists issues, and read our [FAQ](docs/faq).

## Proposing a Change

If you intend to change the public API or introduce new feature, we also recommend use our [issue helper](https://ng.ant.design/issue-helper/#/en) to create a feature request issue.

## Your First Pull Request

Working on your first Pull Request? You can learn how from these resources:

* [How to Contribute to Open Source](https://opensource.guide/how-to-contribute/)
* [First Contributions](https://github.com/firstcontributions/first-contributions)

To help you get your feet wet and get you familiar with our contribution process, we have a list of [Good First Issue](https://github.com/ant-design-blazor/ant-design-blazor/labels/good%20first%20issue) that contain bugs or small features that have a relatively limited scope. This is a great place to get started.

If you decide to fix an issue, please be sure to check the comment thread in case somebody is already working on a fix. If nobody is working on it at the moment, please leave a comment stating that you intend to work on it so other people don’t accidentally duplicate your effort.

If somebody claims an issue but doesn’t follow up for more than two weeks, it’s fine to take over it but you should still leave a comment.

## Contribute

The core team is monitoring for pull requests. We will review your pull request and either merge it, request changes to it, or close it with an explanation.

**Before submitting a pull request**, please make sure the followings are done:

1. Run `dotnet build` in the repository root.
2. If you’ve fixed a bug or added code that should be tested, add tests!
3. Make sure the test suite passes (`dotnet test`).
4. Make sure your code lints (`dotnet build`).
5. Make sure rebase your code to keep the history clean.
6. Make sure your commit message meet the [guidelines](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/CONTRIBUTING.md#-commit-message-guidelines)

## How to send a Pull Request

1. fork the repo of `ant-design-blazor`
2. On `master`: `git remote add upstream https://github.com/ant-design-blazor/ant-design-blazor.git`
3. On `master`: `git pull upstream master`
4. On `master`: `git push origin master`
5. Checkout the feature branch (for example if the branch is called `docs-fix`): `git checkout docs-fix`
6. On `docs-fix` I rebase: `git rebase origin/master`
7. On `docs-fix` change the code and commit: `git commit -a`, you need to follow the [commit message guidelines](https://github.com/ant-design-blazor/ant-design-blazor/blob/master/CONTRIBUTING.md#-commit-message-guidelines)
8. Then, push up: `git push` (might need `-f`, just be sure you understand force pushing before you do it)
9. Send Pull Request in the Github

## Development Workflow

After cloning `ant-design-blazor` and running `npm install` to fetch its dependencies, you can run the following commands:

* `dotnet run` runs ant-design-blazor website locally.
* `dotnet build` checks the code style.
* `dotnet test` runs the complete test suite.
* `dotnet publish -c release -o publish` creates build of `ant-design-blazor`.

If you running into problems while developing ant-design-blazor, you can refer to our [developement guide](https://github.com/ant-design-blazor/ant-design-blazor/wiki/Development-Guide) on GitHub.

