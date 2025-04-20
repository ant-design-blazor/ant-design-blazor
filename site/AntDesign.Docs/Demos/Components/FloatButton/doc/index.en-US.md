---
category: Components
type: General
title: FloatButton
description: A button that floats at the top of the page.
cover: https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*tXAoQqyr-ioAAAAAAAAAAAAADrJ8AQ/original
coverDark: https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*hSAwR7cnabwAAAAAAAAAAAAADrJ8AQ/original
---

## When To Use

- For global functionality on the site.
- Buttons that can be seen wherever you browse.


## API

### common API

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| icon | Set the icon component of button | ReactNode | - |  |
| description | Text and other | ReactNode | - |  |
| tooltip | The text shown in the tooltip | ReactNode \| () => ReactNode |  |  |
| type | Setting button type | `default` \| `primary` | `default` |  |
| shape | Setting button shape | `circle` \| `square` | `circle` |  |
| onClick | Set the handler to handle `click` event | (event) => void | - |  |
| href | The target of hyperlink | string | - |  |
| target | Specifies where to display the linked URL | string | - |  |
| badge | Attach Badge to FloatButton. `status` and other props related are not supported. | [BadgeProps](/components/badge#api) | - | 5.4.0 |

### FloatButton.Group

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| shape | Setting button shape of children | `circle` \| `square` | `circle` |  |
| trigger | Which action can trigger menu open/close | `click` \| `hover` | - |  |
| open | Whether the menu is visible or not, use it with trigger | boolean | - |  |
| closeIcon | Customize close button icon | React.ReactNode | `<CloseOutlined />` |  |
| onOpenChange | Callback executed when active menu is changed, use it with trigger | (open: boolean) => void | - |  |

### FloatButton.BackTop

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| duration | Time to return to top（ms） | number | 450 |  |
| target | Specifies the scrollable area dom node | () => HTMLElement | () => window |  |
| visibilityHeight | The BackTop button will not show until the scroll height reaches this value | number | 400 |  |
| onClick | A callback function, which can be executed when you click the button | () => void | - |  |

## Design Token

<ComponentTokenTable component="FloatButton"></ComponentTokenTable>
