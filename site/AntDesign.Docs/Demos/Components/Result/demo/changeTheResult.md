---
order: 9
title:
  zh-CN: 修改结果
  en-US: Change the Result
---

## zh-CN

通过代码动态修改结果展示页。

## en-US

Dynamically modify the result display page through code.

```jsx
import { Result } from 'antd';
import { SmileOutlined } from '@ant-design/icons';

ReactDOM.render(
  <Result
    icon={<SmileOutlined />}
    title="Great, we have done all the operations!"
  />,
  mountNode,
);
```
