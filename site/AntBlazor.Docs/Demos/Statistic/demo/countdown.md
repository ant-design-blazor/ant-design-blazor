---
order: 3
title:
  zh-CN: 倒计时
  en-US: Countdown
---

## zh-CN

倒计时组件。可以两种方式倒计时即传入两种数据类型做为参数：
1.以现在时刻为起点的倒计时时间长度，类型为TimSpan 
2.倒计时截止到未来某时刻,类型为DateTime

## en-US

Countdown component.

```jsx
import { Statistic, Row, Col } from 'antd';

const { Countdown } = Statistic;
const deadline = Date.now() + 1000 * 60 * 60 * 24 * 2 + 1000 * 30; // Moment is also OK

function onFinish() {
  console.log('finished!');
}

ReactDOM.render(
  <Row gutter={16}>
    <Col span={12}>
      <Countdown title="Countdown" value={deadline} onFinish={onFinish} />
    </Col>
    <Col span={12}>
      <Countdown title="Million Seconds" value={deadline} format="HH:mm:ss:SSS" />
    </Col>
    <Col span={24} style={{ marginTop: 32 }}>
      <Countdown title="Day Level" value={deadline} format="D 天 H 时 m 分 s 秒" />
    </Col>
  </Row>,
  mountNode,
);
```
