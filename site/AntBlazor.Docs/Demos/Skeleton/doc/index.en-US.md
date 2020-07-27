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

| 属性             | 说明                                                                       | 类型                                     | 默认值  |
| ---------------- | -------------------------------------------------------------------------- | ---------------------------------------- | ------- |
| `Active`         | Show animation effect                                                           | `boolean`                                | `false` |
| `Avatar`         | Show avatar placeholder                                                             | `boolean`                                | `false` |
| `AvatarSize`     | Set the size of avatar                                                    | `int \| 'large' \| 'small' \| 'default'` | -       |
| `AvatarShape`    | Set the shape of avatar                                                             | `'circle' \| 'square'`                   | -       |
| `Loading`        | Display the skeleton when `true`                           | `boolean`                                | -       |
| `Paragraph`      | Show paragraph placeholder                                                            | `boolean`                                | `true`  |
| `ParagraphRows`  | Set the row count of paragraph                                                        | `int`                                    | -       |
| `ParagraphWidth` | Set the width of paragraph. When width is an Array, it can set the width of each row. Otherwise only set the last row width | `int \| string \| Array<int \| string>`  | -       |
| `Title`          | Show title placeholder                                                       | `boolean`                                | `true`  |
| `TitleWidth`     | Set the width of title                                                   | `int \| string`                          | -       |


### SkeletonElement Type="button"

| 属性     | 说明             | 类型                               | 默认值      |
| -------- | ---------------- | ---------------------------------- | ----------- |
| `Active` | Show animation effect | `boolean`                          | `false`     |
| `Size`   | Set the size              | `'large' \| 'small' \| 'default'`  | `'default'` |
| `Shape`  | Set the shape               | `'circle' \| 'round' \| 'default'` | `'default'` |

### SkeletonElement Type="avatar"

| 属性     | 说明             | 类型                                     | 默认值      |
| -------- | ---------------- | ---------------------------------------- | ----------- |
| `Active` | Show animation effect | `boolean`                                | `false`     |
| `Size`   | Set the size             | `int \| 'large' \| 'small' \| 'default'` | `'default'` |
| `Shape`  | Set the shape              | `'circle' \| 'square'`                   | `'square'`  |

### SkeletonElement Type="input"

| 属性     | 说明             | 类型                              | 默认值      |
| -------- | ---------------- | --------------------------------- | ----------- |
| `Active` | Show animation effect | `boolean`                         | `false`     |
| `Size`   | Set the size                | `'large' \| 'small' \| 'default'` | `'default'` |