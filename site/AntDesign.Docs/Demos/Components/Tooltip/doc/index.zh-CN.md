---
category: Components
subtitle: 文字提示
type: 数据展示
title: Tooltip
cover: https://gw.alipayobjects.com/zos/alicdn/Vyyeu8jq2/Tooltp.svg
---

简单的文字提示气泡框。

## 何时使用

鼠标移入则显示提示，移出消失，气泡浮层不承载复杂文本和操作。

可用来代替系统默认的 `title` 提示，提供一个`按钮/文字/操作`的文案解释。

## API

| 参数  | 说明     | 类型                               | 默认值 |
| ----- | -------- | ---------------------------------- | ------ |
| Title | 提示文字 | string | string.Empty |
| TitleTemplate | 标题模板 | RenderFragment | - |

### 共同的 API

以下 API 为 Tooltip、Popconfirm、Popover 共享的 API。

| 参数                   | 说明                                                         | 类型                | 默认值            | 版本 |
| ---------------------- | ------------------------------------------------------------ | ------------------- | ----------------- | ---- |
| ArrowPointAtCenter     | 箭头是否指向目标元素中心                                     | bool                | `false`           |      |
| AutoAdjustOverflow     | 气泡被遮挡时自动调整位置                                     | bool                | `true`            |      |
| DefaultVisible         | 默认是否显隐                                                 | bool                | false             |      |
| PopupContainerSelector | 浮层渲染父节点，默认渲染到 body 上                           | string (css选择器)  | -                 |      |
| MouseEnterDelay        | 鼠标移入后延时多少才显示 Tooltip，单位：秒                   | double              | 0.1               |      |
| MouseLeaveDelay        | 鼠标移出后延时多少才隐藏 Tooltip，单位：秒                   | double              | 0.1               |      |
| OverlayClassName       | 卡片类名                                                     | string              | 无                |      |
| OverlayStyle           | 卡片样式                                                     | string              | 无                |      |
| Placement              | 气泡框位置，可选 `Top` `Left` `Right` `Bottom` `TopLeft` `TopRight` `BottomLeft` `BottomRight` `LeftTop` `LeftBottom` `RightTop` `RightBottom` | PlacementType       | PlacementType.Top |      |
| Trigger                | 触发行为，可选 Her/Focus/Click/ContextMenu`，可使用数组设置多个触发行为 | TriggerType[]       | TriggerType.Hover |      |
| Visible                | 用于手动控制浮层显隐                                         | bool                | false             |      |
| OnVisibleChange        | 显示隐藏的回调                                               | EventCallback<bool> | -                 |      |                            | bool                | false             |      |

## 注意

请确保 `Tooltip` 的子元素能接受 `onMouseEnter`、`onMouseLeave`、`onFocus`、`onClick` 事件。
