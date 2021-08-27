# Contributing to ant-design-blazor

We would love to have your contribution to make ant-design-blazor better than it is today! You can contribute by reporting issues, participating in discussions on issues or submitting pull requests. When doing so, you are expected to follow the guidelines below:

 - [Code of Conduct](#coc)
 - [Question or Problem?](#question)
 - [Issues and Bugs](#issue)
 - [Feature Requests](#feature)
 - [Submission Guidelines](#submit)
 - [Coding Rules](#rules)
 - [Commit Message Guidelines](#commit)

## <a name="coc"></a> Code of Conduct
Help us keep ant-design-blazor open and inclusive, by reading and following our [Code of Conduct][coc].

## <a name="question"></a> Got a Question or Problem?

Do not open issues for general support purposes as we want to keep GitHub issues for bug reports and feature requests only. You probably have already got your questions answered on [Segmentfault](https://segmentfault.com/t/ant-design-blazor) or [Stack Overflow](https://stackoverflow.com/tags/ant-design-blazor) where the questions should be tagged with tag `ant-design-blazor`.

Segmentfault / Stack Overflow is a much better place to ask questions since:

- there are thousands of people willing to help on Segmentfault
- questions and answers stay available for public viewing so your question / answer might help someone else
- Segmentfault's voting system assures that the best answers are prominently visible.

To save time, we will systematically close all issues about general support and redirect people to Segmentfault / Stack Overflow.

If you would like to chat about the question in real-time, you can reach out via [our slack channel][slack].

## <a name="issue"></a> Found a Bug?
If you find a bug in the source code, you can help us by
[submitting an issue](#submit-issue) to our [GitHub Repository][github]. Even better, you can
[submit a Pull Request](#submit-pr) with a fix.

## <a name="feature"></a> Missing a Feature?
You can *request* a new feature by [submitting an issue](#submit-issue) to our GitHub
Repository. If you would like to *implement* a new feature, please submit an issue with
a usage scene, to make it easily used.
Please also consider what kind of change it is:

* For a **Major Feature**, first open an issue and outline your proposal so that it can be
discussed. This will also allow us to better coordinate our efforts, prevent duplication of work,
and help you to craft the change so that it is successfully accepted into the project.
* **Small Features** can be crafted and directly [submitted as a Pull Request](#submit-pr).

## <a name="submit"></a> Submission Guidelines

### <a name="submit-issue"></a> Submitting an Issue

Before you submit an issue, please search the issue tracker as there might be existing related issues for your problem and the discussion might inform you about workarounds readily available.

We want to fix all the issues as soon as possible, but before fixing a bug we need to reproduce and confirm it. In order to reproduce bugs we will systematically ask you to provide a minimal reproduction scenario using http://plnkr.co. Having a live, reproducible scenario gives us wealth of important information without going back & forth to you with additional questions like:

- version of ant-design-blazor being used
- 3rd-party libraries and their versions
- and most importantly - a use-case that fails

A minimal reproduce scenario using http://plnkr.co/ allows us to quickly confirm a bug (or point out a coding problem) as well as confirming that we are fixing the right problem. If plunker is not a suitable way to demonstrate the problem (for example, issues related to our npm packaging), please create a standalone git repository demonstrating the problem.

We will be insisting on a minimal reproduce scenario in order to save maintainers time and ultimately be able to fix more bugs. Interestingly, from our experience, users often find coding problems themselves while preparing a minimal plunk. We understand that sometimes it might be hard to extract essential bits of code from a larger code-base but we really need to isolate the problem before we fix it.

Unfortunately we are not able to investigate / fix bugs without a minimal reproduction, so if we don't hear back from you we will close your issue that doesn't have enough info to be reproduced.

You can file new issues by filling out our [new issue form](https://github.com/ant-design-blazor/ant-design-blazor/issues/new).


### <a name="submit-pr"></a> Submitting a Pull Request (PR)
Before you submit your Pull Request (PR), consider the following guidelines:

* Search [GitHub](https://github.com/ant-design-blazor/ant-design-blazor/pulls) for an open or closed PR
  that relates to your submission. You don't want to duplicate effort.
* Make your changes in a new git branch:

     ```shell
     git checkout -b my-fix-branch master
     ```

* Create your patch, **including appropriate test cases**.
* Follow our [Coding Rules](#rules).
* Run the full ant-design-blazor test suite <!-- , as described in the [developer documentation][dev-doc] -->, and ensure that all tests pass.
* Commit your changes using a descriptive commit message that follows our
  [commit message conventions](#commit). Adherence to these conventions
  is necessary because release notes are automatically generated from these messages.

     ```shell
     git commit -a
     ```
  Note: the optional commit `-a` command line option will automatically "add" and "rm" edited files.

* Push your branch to GitHub:

    ```shell
    git push origin my-fix-branch
    ```

* In GitHub, send a pull request to `ant-design-blazor:master`.
* If we suggest changes then:
  * Make the required updates.
  * Re-run the ant-design-blazor test suites to ensure tests are still passed.
  * Rebase your branch and force push to your GitHub repository (this will update your Pull Request):

    ```shell
    git rebase master -i
    git push -f
    ```

That's it! Thank you for your contribution!

#### After your pull request is merged

After your pull request is merged, you can safely delete your branch and pull the changes
from the main (upstream) repository:

* Delete the remote branch on GitHub either through the GitHub web UI or your local shell as follows:

    ```shell
    git push origin --delete my-fix-branch
    ```

* Check out the master branch:

    ```shell
    git checkout master -f
    ```

* Delete the local branch:

    ```shell
    git branch -D my-fix-branch
    ```

* Update your master with the latest upstream version:

    ```shell
    git pull --ff upstream master
    ```

## <a name="rules"></a> Coding Rules
To ensure consistency throughout the source code, keep these rules in mind as you are working:

* All features or bug fixes **must be tested** by one or more specs (unit-tests).
* All public API methods **must be documented**.

## <a name="commit"></a> Commit Message Guidelines

We have very precise rules over how our git commit messages can be formatted.  This leads to **more
readable messages** that are easy to follow when looking through the **project history**.  Meanwhile,
we use the git commit messages to **generate the ant-design-blazor change log**.

### Commit Message Format
Each commit message consists of a **header**, a **body** and a **footer**.  The header has a special
format that includes a **type**, a **scope** and a **subject**:

```
<type>(<scope>): <subject>
<BLANK LINE>
<body>
<BLANK LINE>
<footer>
```

The **header** is mandatory and the **scope** of the header is optional.

Any line of the commit message cannot be longer than 100 characters! This allows the message to be easier read on GitHub as well as in various git tools.

Footer should also contain a [closing reference to an issue](https://help.github.com/articles/closing-issues-via-commit-messages/) if any.

Samples: (even more [samples](https://github.com/ant-design-blazor/ant-design-blazor/commits/master))

```
docs(changelog): update change log to beta.5
```
```
fix(release): need to depend on latest rxjs and zone.js

The version in our package.json gets copied to the one we publish, and users need the latest of these.
```

### Revert
If the commit reverts a previous commit, it should begin with `revert: `, followed by the header of the reverted commit. In the body it should say: `This reverts commit <hash>.`, where the hash is the SHA of the commit being reverted.

### Type
Must be one of the following:

* **build**: Changes that affect the build system or external dependencies (example scopes: gulp, broccoli, npm)
* **ci**: Changes to our CI configuration files and scripts (example scopes: Travis, Circle, BrowserStack, SauceLabs)
* **docs**: Documentation only changes
* **feat**: A new feature
* **fix**: A bug fix
* **perf**: A code change that improves performance
* **refactor**: A code change that neither fixes a bug nor adds a feature
* **style**: Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc)
* **test**: Adding missing tests or correcting existing tests

### Scope
The scope should be the name of the module affected (folder name or other meaningful words), and should have prefix *module:* (as perceived by person reading changelog generated from commit messages.

The following are some examples:

* **module:alert**
* **module:badge**
* **module:breadcrumb**
* **module:OTHER_COMPONENT_NAME**

There are currently a few exceptions to the "use module name" rule:

* **packaging**: used for changes that change the npm package layout, e.g. public path changes, package.json changes, d.ts file/format changes, changes to bundles, etc.
* **changelog**: used for updating the release notes in CHANGELOG.md
* **showcase**: used for docs-app (ng.ant.design) related changes within the showcase / directory of the repo
* none/empty string: useful for `style`, `test` and `refactor` changes that are done across all packages (e.g. `style: add missing semicolons`)

### Subject
The subject contains succinct description of the change:

* use the imperative, present tense: "change" not "changed" nor "changes"
* don't capitalize first letter
* no period (.) at the end

### Body
Just as in the **subject**, use the imperative, present tense: "change" not "changed" nor "changes".
The body should include the motivation for the change and contrast this with previous behavior.

### Footer
The footer should contain any information about **Breaking Changes** and is also the place to
reference GitHub issues that this commit **Closes**.

**Breaking Changes** should start with the words `BREAKING CHANGE:` with a space or two newlines. The rest of the commit message is then used for this.

A detailed explanation can be found in this [document][commit-message-format].


[coc]: https://github.com/ant-design-blazor/ant-design-blazor/blob/master/CODE_OF_CONDUCT.md
[commit-message-format]: https://docs.google.com/document/d/1QrDFcIiPjSLDn3EL15IJygNPiHORgU1_OOAqWjiDU5Y/edit#
[dev-doc]: https://github.com/ant-design-blazor/ant-design-blazor/blob/master/docs/DEVELOPER.md
[github]: https://github.com/ant-design-blazor/ant-design-blazor
[slack]: https://join.slack.com/t/AntBlazor/shared_invite/zt-etfaf1ww-AEHRU41B5YYKij7SlHqajA
[plunker]: http://plnkr.co/edit
