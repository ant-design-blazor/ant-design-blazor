---
category: Components
type: 反馈
title: Spin
subtitle: 加载中
cover: https://gw.alipayobjects.com/zos/alicdn/LBcJqCPRv/Spin.svg
---

用于页面和区块的加载中状态。

## 何时使用

页面局部处于等待异步数据或正在渲染过程时，合适的加载动效会有效缓解用户的焦虑。

## API

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Delay            | 延迟显示加载效果的时间（防止闪烁）           | number (毫秒) | -         |
| Description      | 自定义加载描述内容                           | string        | -         |
| Indicator        | 加载指示符                                   | ReactNode     | -         |
| ClassNames       | 设置 Spin Semantic DOM 节点（`root`、`section`、`indicator`、`description`、`container`）的 CSS 类名 | `IReadOnlyDictionary<string, string>` | - | 6.0.0 |
| Styles           | 设置 Spin Semantic DOM 节点（`root`、`section`、`indicator`、`description`、`container`）的内联样式 | `IReadOnlyDictionary<string, string>` | - | 6.0.0 |
| Size             | 组件大小，可选值为 `small` `default` `large` | string        | 'default' |
| Spinning         | 是否为加载中状态                             | boolean       | true      |
| Tip              | 当作为包裹元素时自定义描述；请改用 `Description` | string     | - | Deprecated |
| WrapperClassName | 包装器的类属性                               | string        | -         |

### 静态方法

- `Spin.setDefaultIndicator(indicator: ReactNode)`

  你可以自定义全局默认 Spin 的元素。
