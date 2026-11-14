---
category: Components
type: Data Display
title: Comment
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/ILhxpGzBO/Comment.svg
---

A comment displays user feedback and discussion to website content.

## When To Use

Comments can be used to enable discussions on an entity such as a page, blog post, issue or other.

## API

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Actions | List of action items rendered below the comment content | IEnumerable<RenderFragment> | - |  |
| Author | The element to display as the comment author | string | - |  |
| AuthorTemplate | The element to display as the comment author | RenderFragment | - |  |
| Avatar | The element to display as the comment avatar - generally ant src | string | - |  |
| AvatarTemplate | The element to display as the comment avatar - generally an antd `Avatar` or img | RenderFragment | - |  |
| ChildContent | Nested comments should be provided as children of the Comment | ReactNode | - |  |
| Content | The main content of the comment | string | - |  |
| ContentTemplate | The main content of the comment | string | - | RenderFragment |
| Datetime | A datetime element containing the time to be displayed | string | - |  |
| DatetimeTemplate | A datetime element containing the time to be displayed | RenderFragment | - |  |
| Placement | The placement of the avatar.  | `left` \| `right` |  `left` | 0.18.0  |