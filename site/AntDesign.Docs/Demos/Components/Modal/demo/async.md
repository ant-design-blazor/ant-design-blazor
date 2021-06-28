---
order: 1
title:
  zh-CN: 异步关闭
  en-US: Asynchronously close
---

## zh-CN

点击确定后异步关闭对话框，例如提交表单。

## en-US

Asynchronously close a modal dialog when a the OK button is pressed. For example, you can use this pattern when you submit a form.

```jsx
import { Modal, Button } from 'antd';

class App extends React.Component {


  handleCancel = () => {
    console.log('Clicked cancel button');
    this.setState({
      visible: false,
    });
  };

}

ReactDOM.render(<App />, mountNode);
```
