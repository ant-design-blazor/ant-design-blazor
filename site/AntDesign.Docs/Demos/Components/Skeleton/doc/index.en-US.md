---
category: Components
type: Feedback
title: Skeleton
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/KpcciCJgv/Skeleton.svg
---

Provide a placeholder while you wait for content to load, or to visualise content that doesn't exist yet.

## When To Use

- When resource need long time loading, like low network speed.
- The component contains much information, such as List or Card.
- Only works when loading data for the first time.
- Could be replaced by Spin in any situation, but can provide a better user experience.

## API

### Skeleton

| Property         | Description                                                                | Type                                     | Default |
| ---------------- | -------------------------------------------------------------------------- | ---------------------------------------- | ------- |
| Active           | Show animation effect                                                      | `boolean`                                | `false` |
| Avatar           | Show avatar placeholder                                                    | `boolean`                                | `false` |
| AvatarSize       | Set the size of avatar                                                     | `OneOf<SkeletonElementSize, string>`     | -       |
| AvatarShape      | Set the shape of avatar                                                    | `SkeletonElementShape`                   | -       |
| Loading          | Display the skeleton when `true`                                           | `boolean`                                | -       |
| Paragraph        | Show paragraph placeholder                                                 | `boolean`                                | `true`  |
| ParagraphRows    | Set the row count of paragraph                                             | `int`                                    | -       |
| ParagraphWidth   | Set the width of paragraph. When width is an Array, it can set the width of each row. Otherwise only set the last row width | `int \| string \| Array<int \| string>`  | -       |
| Title            | Show title placeholder                                                     | `boolean`                                | `true`  |
| TitleWidth       | Set the width of title                                                     | `int \| string`                          | -       |


### SkeletonElement Type="SkeletonElementType.Button"

| Property | Description                 | Type                                 | Default                        |
| -------- | --------------------------- | ------------------------------------ | ------------------------------ |
| Active   | Show animation effect       | `boolean`                            | `false`                        |
| Size     | Set the size                | `OneOf<SkeletonElementSize, string>` | `SkeletonElementSize.Default`  |
| Shape    | Set the shape               | `SkeletonElementShape`               | `SkeletonElementShape.Default` |

### SkeletonElement Type="SkeletonElementType.Avatar"

| Property | Description                 | Type                                     | Default                        |
| -------- | --------------------------- | ---------------------------------------- | ------------------------------ |
| Active   | Show animation effect       | `boolean`                                | `false`                        |
| Size     | Set the size                | `OneOf<SkeletonElementSize, string>`     | `SkeletonElementSize.Default`  |
| Shape    | Set the shape               | `SkeletonElementShape`                   | `SkeletonElementShape.Default` |

### SkeletonElement Type="SkeletonElementType.Input"

| Property | Description                 | Type                                 | Default                       |
| -------- | --------------------------- | ------------------------------------ | ----------------------------- |
| Active   | Show animation effect       | `boolean`                            | `false`                       |
| Size     | Set the size                | `OneOf<SkeletonElementSize, string>` | `SkeletonElementSize.Default` |