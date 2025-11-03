---
order: 98
title: 更新日志
toc: false
timeline: true
---

`Ant Design Blazor` 严格遵循 [Semantic Versioning 2.0.0](http://semver.org/lang/zh-CN/) 语义化版本规范。

#### 发布周期

- 修订版本号：每周末会进行日常 bugfix 更新。（如果有紧急的 bugfix，则任何时候都可发布）
- 次版本号：每月发布一个带有新特性的向下兼容的版本。
- 主版本号：含有破坏性更新和新特性，不在发布周期内。

---

### 1.5.0

- Table
  - 🆕 新增 支持自定义筛选器面板时控制面板关闭。[#4645](https://github.com/ant-design-blazor/ant-design-blazor/pull/4645) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 支持设置 FilterMultiple=false 时隐藏字段类型筛选器的添加按钮。[#4684](https://github.com/ant-design-blazor/ant-design-blazor/pull/4684) [@alchiweb](https://github.com/alchiweb)
  - 🆕 新增 属性 ScrollItemIntoView 来滚动到指定的行数据。[#4664](https://github.com/ant-design-blazor/ant-design-blazor/pull/4664) [@pankey888](https://github.com/pankey888)
  - 🆕 新增 直接提交 QueryModel 到后端能正确模型绑定，支持直接对 IQueryable 进行筛选和排序。[#4658](https://github.com/ant-design-blazor/ant-design-blazor/pull/4658) [@ElderJames](https://github.com/ElderJames)

- Upload
  - 🆕 新增 Upload 的 Defer 延迟上传模式。[#4626](https://github.com/ant-design-blazor/ant-design-blazor/pull/4626) [@stfei](https://github.com/stfei)
  - 🆕 新增 Upload 在 Defer 延迟上传模式下，先利用 Object URL 在上传之前展示图片。[#4680](https://github.com/ant-design-blazor/ant-design-blazor/pull/4680) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 Upload 支持文本框粘贴上传。[#4650](https://github.com/ant-design-blazor/ant-design-blazor/pull/4650) [@ElderJames](https://github.com/ElderJames)

- Tabs
  - 🆕 新增 手势滑动翻页功能。[#4581](https://github.com/ant-design-blazor/ant-design-blazor/pull/4581) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 避免再次渲染已经关闭的 Tab 页。[#4681](https://github.com/ant-design-blazor/ant-design-blazor/pull/4681) [@ElderJames](https://github.com/ElderJames)

- ReuseTabs
  - 🐞 修复 ReuseTabs 的 Pin 属性在页面不在同一程序集时失效的问题。[#4702](https://github.com/ant-design-blazor/ant-design-blazor/pull/4702) [@shuangbaojun](https://github.com/shuangbaojun)
  - 🛠 重构 ReuseTabs 的页面缓存策略。[#4679](https://github.com/ant-design-blazor/ant-design-blazor/pull/4679) [@ElderJames](https://github.com/ElderJames)

- Select
  - 🆕 新增 Select 让 SelectOption 支持设置 ChildContent 模板。[#4662](https://github.com/ant-design-blazor/ant-design-blazor/pull/4662) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Select 在 FromItem 中不绑定 Value 时会报异常。 [#4700](https://github.com/ant-design-blazor/ant-design-blazor/pull/4700) [#4709](https://github.com/ant-design-blazor/ant-design-blazor/pull/4709) [@shuangbaojun](https://github.com/shuangbaojun)
  - 🐞 修复 Select 应该只在 DataSource 模式下要求 ValueName 非空。[#4683](https://github.com/ant-design-blazor/ant-design-blazor/pull/4683) [@ElderJames](https://github.com/ElderJames)

- Input
  - 🛠 重构 删除 ant-input-group-wrapper 的重复渲染。[#4689](https://github.com/ant-design-blazor/ant-design-blazor/pull/4689) [@zandiarash](https://github.com/zandiarash)
  - 🛠 重构 一些 API 的命名 OnkeyDown, OnkeyDownAsync and OnkeyUp。[#4697](https://github.com/ant-design-blazor/ant-design-blazor/pull/4697) [@zandiarash](https://github.com/zandiarash)
  - 🐞 修复 InputGroup 的 RTL 样式。[#4694](https://github.com/ant-design-blazor/ant-design-blazor/pull/4694) [@zandiarash](https://github.com/zandiarash)

- 🆕 新增 Mentions 支持自定义前缀与多前缀。[#4652](https://github.com/ant-design-blazor/ant-design-blazor/pull/4652) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Carousel 支持触摸滑动翻页功能。[#4580](https://github.com/ant-design-blazor/ant-design-blazor/pull/4580) [@ElderJames](https://github.com/ElderJames)

- 🛠 重构 Modal 使 ModalService.CreateComfirmAsync() 直接返回 ConfirmResult 并正常触发OnOk/OnCancel 等委托。[#4704](https://github.com/ant-design-blazor/ant-design-blazor/pull/4704) [@shuangbaojun](https://github.com/shuangbaojun)
- 🐞 修复 Datepicker 销毁日期选择器时出现的异常。[#4715](https://github.com/ant-design-blazor/ant-design-blazor/pull/4715) [@pankey888](https://github.com/pankey888)
- 🐞 修复 TreeSelect 在 FromItem 中如果未绑定 Value 会报异常。[#4714](https://github.com/ant-design-blazor/ant-design-blazor/pull/4714) [@shuangbaojun](https://github.com/shuangbaojun)
- 🐞 修复 Badge 的 BadgeSize 枚举缺少命名空间。[#4660](https://github.com/ant-design-blazor/ant-design-blazor/pull/4660) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 JsInvokeAsync 方法，不再向控制台打印异常堆栈信息。[#4669](https://github.com/ant-design-blazor/ant-design-blazor/pull/4669) [@Yusuftmle](https://github.com/Yusuftmle)
- 📖 修复 Icon 文档在复制时 IconThemeType 不正确。[#4706](https://github.com/ant-design-blazor/ant-design-blazor/pull/4706) [@zandiarash](https://github.com/zandiarash)

#### 破坏性更新

- Table: 默认情况下 `FilterMultiple=false`，内置 Filter 则不显示 “+” 按钮，如需开启只需设置 “FilterMultiple=true”。
- Input: 原来的 `Onkey*` API 重命名为 `OnKey*`。
- Modal

  - 方法 `CreateConfirmAsync<TComponent, TComponentOptions, TResult>(ConfirmOptions config, TComponentOptions componentOptions);` 返回值从 `Task<ConfirmRef<TResult>>` 改为 `Task<ConfirmResult>`。
  - ModalService.CreateComfirmAsync(...) 的 `OnOk`、`OnCancel` 等委托设置从原来的在 ConfirmRef 改为在 ConfirmOptions 设置：
  
    ```cs
      var options = new ConfirmOptions<string>()
      {
         Title = "Confirm",
         Width = 350,
         Content = content,
         OnOpen = async () =>
         {
             Console.WriteLine("Open Confirm");
         },
         OnClose = async () =>
         {
             Console.WriteLine("Close Confirm");
         },
         OnCancel = async (result) =>
         {
             Console.WriteLine($"OnCancel:{result}");
         },
         OnOk = async (result) =>
         {
             Console.WriteLine($"OnOk:{result}");
         }
      };

      await ModalService.CreateConfirmAsync(options);
    ```


### 1.4.3

`2025-07-13`

- 🆕 新增 InputNumber 值为null时上下键的默认值。[#4654](https://github.com/ant-design-blazor/ant-design-blazor/pull/4654) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Table 在客户端数据源前单选时，如果选中行被移除，自动删除SelectedRows中的选中行。[#4651](https://github.com/ant-design-blazor/ant-design-blazor/pull/4651) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Checkbox 当值不变时避免不必要的更新。[#4653](https://github.com/ant-design-blazor/ant-design-blazor/pull/4653) [@ElderJames](https://github.com/ElderJames)
- 📖 文档 Form 改进验证示例的说明。[#4643](https://github.com/ant-design-blazor/ant-design-blazor/pull/4643) [@ElderJames](https://github.com/ElderJames)

### 1.4.2

`2025-06-30`

- Tabs
  - 🐞 修复 Tabs 点击激活的Tab时应触发 OnTabClick 事件。[#4634](https://github.com/ant-design-blazor/ant-design-blazor/pull/4634) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Tabs 执行 ResetSizes 时wrapper id 相关错误。[#4629](https://github.com/ant-design-blazor/ant-design-blazor/pull/4629) [@ElderJames](https://github.com/ElderJames)

- Table
  - ⚡️ 性能提升 Table 利用缓存机制改进反射性能。[#4627](https://github.com/ant-design-blazor/ant-design-blazor/pull/4627) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Table 在固定头或列时斑马纹展示问题。[#4630](https://github.com/ant-design-blazor/ant-design-blazor/pull/4630) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Table 分组 GroupFooterTemplate 渲染。[#4623](https://github.com/ant-design-blazor/ant-design-blazor/pull/4623) [@GlodenBoy](https://github.com/GlodenBoy)

- Form
  - 🐞 重构 Form 内部数字相关的验证特性。[#4624](https://github.com/ant-design-blazor/ant-design-blazor/pull/4624) [@LeaFrock](https://github.com/LeaFrock)
  - 🐞 修复 Form `StringLengthAttribute` 验证信息，新增 `LengthAttribute` 特性验证。[#4616](https://github.com/ant-design-blazor/ant-design-blazor/pull/4616) [@LeaFrock](https://github.com/LeaFrock)

- 🐞 修复 Overlay 相关组件在鼠标快速略过时不会关闭 (#4636)。[#4637](https://github.com/ant-design-blazor/ant-design-blazor/pull/4637) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Menu 内联收起时状态未刷新。[#4636](https://github.com/ant-design-blazor/ant-design-blazor/pull/4636) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Input 按键编码兼容性。[#4633](https://github.com/ant-design-blazor/ant-design-blazor/pull/4633) [@Qingmei16](https://github.com/Qingmei16)
- 🐞 修复 manipulationHelper 安全解析CSS 值。[#4619](https://github.com/ant-design-blazor/ant-design-blazor/pull/4619) [@ElderJames](https://github.com/ElderJames)
- 🌐 新增 eu_ES 巴斯克语。[#4617](https://github.com/ant-design-blazor/ant-design-blazor/pull/4617) [@izurza](https://github.com/izurza)
- 📖 文档增加 AI 组件链接。[#4635](https://github.com/ant-design-blazor/ant-design-blazor/pull/4635) [@ElderJames](https://github.com/ElderJames)


### 1.4.1.1

`2025-06-15`

父亲节快乐！

- Input
  - 🆕 新增 DefaultToEmptyString 属性以支持默认值为空字符串。[#4586](https://github.com/ant-design-blazor/ant-design-blazor/pull/4586) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 TextArea 的 OnPressEnter 属性支持组合键处理。[#4585](https://github.com/ant-design-blazor/ant-design-blazor/pull/4585) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Textarea 的 Class 属性没有传递给 textarea 元素。[#4591](https://github.com/ant-design-blazor/ant-design-blazor/pull/4591) [@ElderJames](https://github.com/ElderJames)

- Splitter
  - 🛎 性能优化 只当鼠标拖动结束才触发刷新。 [#4614](https://github.com/ant-design-blazor/ant-design-blazor/pull/4614) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 SplitterPanel 遗漏了命名空间, 并修复 CS8785 和 RZ3008 编译错误。[#4602](https://github.com/ant-design-blazor/ant-design-blazor/pull/4602) [@ElderJames](https://github.com/ElderJames)

- Select
  - 🐞 修复 在用模版选项时，默认选中项不显示。[#4607](https://github.com/ant-design-blazor/ant-design-blazor/pull/4607) [@ElderJames](https://github.com/ElderJames)
  - 🚫 重构 增加 ChildContent 属性以方便替代 SelectOptions。[#4603](https://github.com/ant-design-blazor/ant-design-blazor/pull/4603) [@ElderJames](https://github.com/ElderJames)

- 🆕 新增 Tabs 属性 StandaloneInCard 来在 Card 组件中独立显示。[#4608](https://github.com/ant-design-blazor/ant-design-blazor/pull/4608) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Grid 的 RowAlign 和 SpaceAlign 补充遗漏属性。[#4604](https://github.com/ant-design-blazor/ant-design-blazor/pull/4604) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Progress 类型为 Dashboard 时 StrokeColor 属性无效。[#4610](https://github.com/ant-design-blazor/ant-design-blazor/pull/4610) [@pankey888](https://github.com/pankey888)
- 🐞 修复 Upload 当用户绑定 FileList 时，避免内部和外部重复增加 UploadFileItem。[#4592](https://github.com/ant-design-blazor/ant-design-blazor/pull/4592) [@ElderJames](https://github.com/ElderJames)
- 📖 文档 增加 Carbon 广告。[#4593](https://github.com/ant-design-blazor/ant-design-blazor/pull/4593) [@ElderJames](https://github.com/ElderJames)

#### Breaking Changes

Input/TextArea/Search 组件的 `OnPressEnter` 事件参数从 `KeyboardEventArgs` 改为 `PressEnterEventArgs`。

### 1.4.0

`2025-05-07`

- 🔥 新增 Splitter 组件。[#4555](https://github.com/ant-design-blazor/ant-design-blazor/pull/4555) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Notification 支持当鼠标移动到通知上时暂停关闭。[#4535](https://github.com/ant-design-blazor/ant-design-blazor/pull/4535) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Collapse 支持双向绑定控制激活面板。[#4564](https://github.com/ant-design-blazor/ant-design-blazor/pull/4564) [@ElderJames](https://github.com/ElderJames)

- Table
  - 🆕 新增 支持吸顶滚动。[#4566](https://github.com/ant-design-blazor/ant-design-blazor/pull/4566) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 EllipsisShowTitle 属性，可自定义省略提示。[#4565](https://github.com/ant-design-blazor/ant-design-blazor/pull/4565) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 设置 ExpandTemplate 时不显示展开按钮。[#4554](https://github.com/ant-design-blazor/ant-design-blazor/pull/4554) [@ElderJames](https://github.com/ElderJames)

- Select
  - 🐞 修复 防止在 IME 合成期间触发搜索。[#4572](https://github.com/ant-design-blazor/ant-design-blazor/pull/4572) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 筛选时选项对象创建。[#4571](https://github.com/ant-design-blazor/ant-design-blazor/pull/4571) [@ElderJames](https://github.com/ElderJames)
  - 🛠 重构 删除在绑定值初始化时触发SelectedItemChanged。[#4568](https://github.com/ant-design-blazor/ant-design-blazor/pull/4568) [@ElderJames](https://github.com/ElderJames)

- Message
  - 🛠 重构 MessageService 以支持异步与同步方法。[#4548](https://github.com/ant-design-blazor/ant-design-blazor/pull/4548) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 支持当鼠标移动到消息上时暂停关闭。[#4536](https://github.com/ant-design-blazor/ant-design-blazor/pull/4536) [@ElderJames](https://github.com/ElderJames)

- Upload
  - 🆕 新增 WithCredentials 选项，以支持上传时携带cookie。[#4547](https://github.com/ant-design-blazor/ant-design-blazor/pull/4547) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 支持多文件在一次请求上传。[#4544](https://github.com/ant-design-blazor/ant-design-blazor/pull/4544) [@ElderJames](https://github.com/ElderJames)
  - 📖 文档 使用原生 InputFile 的 demo 增加拖拽上传示例。[#4546](https://github.com/ant-design-blazor/ant-design-blazor/pull/4546) [@ElderJames](https://github.com/ElderJames)

- ⚡️ 性能提升 尽量使用 JsonSerializerOptions单例。[#4538](https://github.com/ant-design-blazor/ant-design-blazor/pull/4538) [@LeaFrock](https://github.com/LeaFrock)
- ⚡️ 性能提升 优化字典使用。[#4537](https://github.com/ant-design-blazor/ant-design-blazor/pull/4537) [@LeaFrock](https://github.com/LeaFrock)
- ⚡️ 性能提升 重构 Event Listener 支持异步。[#4573](https://github.com/ant-design-blazor/ant-design-blazor/pull/4573) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tree 更新数据源后行为异常。[#4575](https://github.com/ant-design-blazor/ant-design-blazor/pull/4575) [@DarkElfes](https://github.com/DarkElfes)
- 🐞 修复 Cascader 的 Placeholder 属性无效问题。[#4545](https://github.com/ant-design-blazor/ant-design-blazor/pull/4545) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Flex 的 Justify 属性。[#4539](https://github.com/ant-design-blazor/ant-design-blazor/pull/4539) [@thirking](https://github.com/thirking)
- 🐞 修复 Mentions 选项菜单展开逻辑。[#4574](https://github.com/ant-design-blazor/ant-design-blazor/pull/4574) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 DatePicker 的 RangePicker 选中值绑定。[#4570](https://github.com/ant-design-blazor/ant-design-blazor/pull/4570) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Form 动态修改必填规则。[#4543](https://github.com/ant-design-blazor/ant-design-blazor/pull/4543) [@ElderJames](https://github.com/ElderJames)
- 🗑 移除弃用方法，增强  Confirm 弹窗可编程性。[#4549](https://github.com/ant-design-blazor/ant-design-blazor/pull/4549) [@ElderJames](https://github.com/ElderJames)
- 📖 文档 更新 Simple Json 本地化方式的使用方法。[#4563](https://github.com/ant-design-blazor/ant-design-blazor/pull/4563) [@ElderJames](https://github.com/ElderJames)

#### 破坏性更新

此版本之后 IMesesageService 的方法签名区分同步与异步。异步的方法带有 Async 后缀，而原来没有后缀的异步方法变为同步方法，因此更新时需求删除前面的 await 或者 _=, 或者全局增加Async后缀。请参考以下代码： 

```cs
// 之前用法
// 同步
_ = _message.Success("Operation completed");
// 异步
await _message.Success("Operation completed");

// 此后语法
// 同步
_message.Success("Operation completed");
// 异步
await _message.SuccessAsync("Operation completed");
```
详情请参考这个Pull Request https://github.com/ant-design-blazor/ant-design-blazor/pull/4548



### 1.3.2

`2025-04-07`

- 🐞 修复 Upload 文件列表删除时渲染问题。[#4533](https://github.com/ant-design-blazor/ant-design-blazor/pull/4533) [@ElderJames](https://github.com/ElderJames)
- 🐞 All Flex components that have the default FlexGap now no longer give an KeyNotFoundException。[#4529](https://github.com/ant-design-blazor/ant-design-blazor/pull/4529) [@MauritsDodo](https://github.com/MauritsDodo)
- ⚡️ 新增 `params ReadOnlySpan<>` 重载。[#4531](https://github.com/ant-design-blazor/ant-design-blazor/pull/4531) [@LeaFrock](https://github.com/LeaFrock)
- 📖 修复 组件搜索。[#4530](https://github.com/ant-design-blazor/ant-design-blazor/pull/4530) [@CAPCHIK](https://github.com/CAPCHIK)


### 1.3.1

`2025-04-02`

- 🛠 重构 Form 补充遗漏的 Model 到 ValidationContext。[#4525](https://github.com/ant-design-blazor/ant-design-blazor/pull/4525) [@ElderJames](https://github.com/ElderJames)
- 🛠 重构 Form 自定义验证 Attribute 传入 ValidationContext。[#4523](https://github.com/ant-design-blazor/ant-design-blazor/pull/4523) [@ElderJames](https://github.com/ElderJames)
- 🐞 重构 Table 优化行展开并提高可读性。[#4519](https://github.com/ant-design-blazor/ant-design-blazor/pull/4519) [@ElderJames](https://github.com/ElderJames)
- ⚡️ 使用源生成器提升Regex性能。[#4524](https://github.com/ant-design-blazor/ant-design-blazor/pull/4524) [@LeaFrock](https://github.com/LeaFrock)
- ⚡️ 使用Dictionary<TKey, TValue>替换HashTable。[#4520](https://github.com/ant-design-blazor/ant-design-blazor/pull/4520) [@LeaFrock](https://github.com/LeaFrock)

### 1.3.0

`2025-03-29`

- Table
  - 🆕 新增 字符串筛选器的操作增加”不包含“。[#4494](https://github.com/ant-design-blazor/ant-design-blazor/pull/4494) [@rtrocmn](https://github.com/rtrocmn)
  - 🐞 修复 当同时设置了 RowExpandable 和 OnExpand 时避免重复的显示展开按钮 。[#4508](https://github.com/ant-design-blazor/ant-design-blazor/pull/4508) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 当外部修改 SelectedRows 时避免触发 SelectedRowsChanged。[#4486](https://github.com/ant-design-blazor/ant-design-blazor/pull/4486) [@ElderJames](https://github.com/ElderJames)

- 🆕 新增 DatePicker 支持显示周数。[#4507](https://github.com/ant-design-blazor/ant-design-blazor/pull/4507) [@duseo](https://github.com/duseo)
- 🆕 新增 Message 服务接口 IMessageService 实现 LoadingWhen 扩展方法，以方便回调操作。[#4493](https://github.com/ant-design-blazor/ant-design-blazor/pull/4493) [@XmmShp](https://github.com/XmmShp)
- 🆕 新增 Menu 菜单项的 Target 属性。[#4502](https://github.com/ant-design-blazor/ant-design-blazor/pull/4502) [@pathartl](https://github.com/pathartl)
- 🆕 新增 ReuseTabs 支持等到 Menu 加载后再收集标题。[#4487](https://github.com/ant-design-blazor/ant-design-blazor/pull/4487) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Form 绑定模型变更时未能及时触发状态。[#4514](https://github.com/ant-design-blazor/ant-design-blazor/pull/4514) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Drawer 关闭时，留下滚动条阴影。[#4512](https://github.com/ant-design-blazor/ant-design-blazor/pull/4512) [@thirking](https://github.com/thirking)
- 💄 优化 Spin 为form, list, 和 table 内置的增加一个classname。[#4500](https://github.com/ant-design-blazor/ant-design-blazor/pull/4500) [@pathartl](https://github.com/pathartl)
- 🐞 修复隐式转换导致的 System.ArgumentException。[#4498](https://github.com/ant-design-blazor/ant-design-blazor/pull/4498) [@XmmShp](https://github.com/XmmShp)
- 🌐 优化本地化服务的序列化支持JSON源生成器。[#4489](https://github.com/ant-design-blazor/ant-design-blazor/pull/4489) [@ElderJames](https://github.com/ElderJames)
- 🛠 更新 Node.js 版本以迎合依赖要求。[#4499](https://github.com/ant-design-blazor/ant-design-blazor/pull/4499) [@XmmShp](https://github.com/XmmShp)
- 🐞 修复 npm 启动脚本以适应 .Net9.0。[#4495](https://github.com/ant-design-blazor/ant-design-blazor/pull/4495) [@XmmShp](https://github.com/XmmShp)


### 1.2.1

`2025-02-25`

- 🛠 优化 Tag 组件 preset color 实现。[#4479](https://github.com/ant-design-blazor/ant-design-blazor/pull/4479) [@LeaFrock](https://github.com/LeaFrock)
- 🐞 修复 Progress 的 Status 属性不正确。 [#4475](https://github.com/ant-design-blazor/ant-design-blazor/pull/4475) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Overlay 打开时超出边界时的定位调整问题。 [#4483](https://github.com/ant-design-blazor/ant-design-blazor/pull/4483) [@ElderJames](https://github.com/ElderJames)
- 🛠 重构 Datepicker 将DatePicker 和 RangePicker 的 Disabled 属性分开。[#4474](https://github.com/ant-design-blazor/ant-design-blazor/pull/4474) [@ElderJames](https://github.com/ElderJames)
- 📖 文档 Table 增加 HidePagination 行为的说明。[#4473](https://github.com/ant-design-blazor/ant-design-blazor/pull/4473) [@ElderJames](https://github.com/ElderJames)
- 🌐 更新 fa-IR 语言包。[#4464](https://github.com/ant-design-blazor/ant-design-blazor/pull/4464) [@zandiarash](https://github.com/zandiarash)

### 1.2.0

`2025-02-02`

新春快乐，巳巳如意！

- Cascader
  - 🆕 新增 支持键盘导航。[#4414](https://github.com/ant-design-blazor/ant-design-blazor/pull/4414) [@ElderJames](https://github.com/ElderJames)
  - 🛠 重构 派生自 SelectBase。[#4408](https://github.com/ant-design-blazor/ant-design-blazor/pull/4408) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 支持自定义触发器。[#4404](https://github.com/ant-design-blazor/ant-design-blazor/pull/4404) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Value 双向绑定默认值选中。[#4415](https://github.com/ant-design-blazor/ant-design-blazor/pull/4415) [@ElderJames](https://github.com/ElderJames)

- 🆕 新增 Flex 属性 Direction。[#4410](https://github.com/ant-design-blazor/ant-design-blazor/pull/4410) [@pathartl](https://github.com/pathartl)
- 🆕 新增 Select 属性 Placement。[#4409](https://github.com/ant-design-blazor/ant-design-blazor/pull/4409) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Input 属性 WrapperClass。[#4403](https://github.com/ant-design-blazor/ant-design-blazor/pull/4403) [@zandiarash](https://github.com/zandiarash)
- 🌐 更新土耳其语本地化。[#4460](https://github.com/ant-design-blazor/ant-design-blazor/pull/4460) [@gunesoguzhan](https://github.com/gunesoguzhan)

### 1.1.4

`2025-01-24`

- Table
  - 🆕 新增 动态列支持识别内置筛选器类型。[#4439](https://github.com/ant-design-blazor/ant-design-blazor/pull/4439) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 按回车键时确认筛选器。[#4441](https://github.com/ant-design-blazor/ant-design-blazor/pull/4441) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 全选状态更新不正常。[#4449](https://github.com/ant-design-blazor/ant-design-blazor/pull/4449) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 选中行会被 Disabled 行翻页后清除。[#4450](https://github.com/ant-design-blazor/ant-design-blazor/pull/4450) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 筛选器条件从Between切换到Equals时异常。[#4447](https://github.com/ant-design-blazor/ant-design-blazor/pull/4447) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 设置 PageSize 并隐藏分页器时偶发的无限循环。[#4446](https://github.com/ant-design-blazor/ant-design-blazor/pull/4446) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 筛选器下拉组件的边界调整模式。[#4445](https://github.com/ant-design-blazor/ant-design-blazor/pull/4445) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Querymodel 反序列化。[#4443](https://github.com/ant-design-blazor/ant-design-blazor/pull/4443) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 计算自动高度时未将 Modal 和 Drawer 排除。[#4440](https://github.com/ant-design-blazor/ant-design-blazor/pull/4440) [@pankey888](https://github.com/pankey888)

- 🐞 修复 Tabs 当关闭了中间一个标签后，其右边标签的右键菜单失效。 [#4456](https://github.com/ant-design-blazor/ant-design-blazor/pull/4456) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Form 动态更新 Help 提示。[#4452](https://github.com/ant-design-blazor/ant-design-blazor/pull/4452) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Icon 设置 TwoToneColor 值时自动生成双色。[#4451](https://github.com/ant-design-blazor/ant-design-blazor/pull/4451) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 ReuseTabs 当 RouteValue 是 null 时 NRE 异常。[#4438](https://github.com/ant-design-blazor/ant-design-blazor/pull/4438) [@ElderJames](https://github.com/ElderJames)
- 🗑 移除 TestKit 中的 FluentAssertions 引用。[#4444](https://github.com/ant-design-blazor/ant-design-blazor/pull/4444) [@ElderJames](https://github.com/ElderJames)

破坏性更新：

- 表格： 当 HidePagination 设置为 true 时，不应设置 PageSize，此时展示的是所以数据。否则，如果设置了 PageSize，将根据 PageSize 显示每页的行数，这就需要用户自己处理分页的逻辑。

### 1.1.3

`2025-01-15`

- 🐞 修复 Modal 当没有滚动条时不应增加空白。[#4434](https://github.com/ant-design-blazor/ant-design-blazor/pull/4434) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Timeline 颜色Color属性不正常。[#4433](https://github.com/ant-design-blazor/ant-design-blazor/pull/4433) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 DatePicker 在手动输入后不能重新打开的问题。[#4431](https://github.com/ant-design-blazor/ant-design-blazor/pull/4431) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 ReuseTabs 在导航时 RouteData 未及时更新时显示问题。[#4429](https://github.com/ant-design-blazor/ant-design-blazor/pull/4429) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Breadcrumb 自动生成时路由匹配方式使用 MenuItem 中的。[#4428](https://github.com/ant-design-blazor/ant-design-blazor/pull/4428) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 AutoComplete 在异步增加选项后展开面板。[#4425](https://github.com/ant-design-blazor/ant-design-blazor/pull/4425) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Badge Ribbon 的预设颜色。[#4426](https://github.com/ant-design-blazor/ant-design-blazor/pull/4426) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Table 筛选组件输入框增加最小宽度。[#4424](https://github.com/ant-design-blazor/ant-design-blazor/pull/4424) [@ElderJames](https://github.com/ElderJames)
- 💄 修复 Space 的 Size 和 Align 样式。[#4421](https://github.com/ant-design-blazor/ant-design-blazor/pull/4421) [@ElderJames](https://github.com/ElderJames)
- 📖 文档 修复示例源码展开被自动关闭的问题[#4430](https://github.com/ant-design-blazor/ant-design-blazor/pull/4430) [@ElderJames](https://github.com/ElderJames)
- 📖 文档 增加 PeterCat AI 挂件。[#4420](https://github.com/ant-design-blazor/ant-design-blazor/pull/4420) [@ElderJames](https://github.com/ElderJames)

### 1.1.2

`2025-01-08`

- Overlay
  - 🆕 新增 Overlay 组件支持 Visible 属性控制打开关闭。[#4418](https://github.com/ant-design-blazor/ant-design-blazor/pull/4418) [@ElderJames](https://github.com/ElderJames)
  - 💄 修复 Overlay 相关组件有 div 包围的触发器的内联样式。[#4405](https://github.com/ant-design-blazor/ant-design-blazor/pull/4405) [@ElderJames](https://github.com/ElderJames)

- Table
  - 🐞 修复 默认筛选方向只有一个。[#4411](https://github.com/ant-design-blazor/ant-design-blazor/pull/4411) [@ElderJames](https://github.com/ElderJames)
  - 📖 更新 树形数据的文档并调整demo顺序。[#4398](https://github.com/ant-design-blazor/ant-design-blazor/pull/4398) [@ElderJames](https://github.com/ElderJames)

- 🆕 新增 Typography 一个 article 元素的组件。[#4400](https://github.com/ant-design-blazor/ant-design-blazor/pull/4400) [@ElderJames](https://github.com/ElderJames)
- 💄 新增 Cascader 缺少的 class 。[#4407](https://github.com/ant-design-blazor/ant-design-blazor/pull/4407) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 ReuseTabs 的 Singleton 页面的属性更新。[#4399](https://github.com/ant-design-blazor/ant-design-blazor/pull/4399) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Result 的 Http 状态图片失效。[#4396](https://github.com/ant-design-blazor/ant-design-blazor/pull/4396) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Menu 子菜单布局问题。[#4417](https://github.com/ant-design-blazor/ant-design-blazor/pull/4417) [@ElderJames](https://github.com/ElderJames)
- 🛠 重构 AutoComplete 脱离 ShowPanel 属性作用并标记废弃。[#4393](https://github.com/ant-design-blazor/ant-design-blazor/pull/4393) [@ElderJames](https://github.com/ElderJames)
- 📖 清理 Dropdown 文档 demo 多余的代码。[#4401](https://github.com/ant-design-blazor/ant-design-blazor/pull/4401) [@zandiarash](https://github.com/zandiarash)

### 1.1.1

`2025-01-02`

- 🛠 重构 Badge 的 Size 属性和 BadgeRibbon 的 Color 属性类型改为枚举。[#4389](https://github.com/ant-design-blazor/ant-design-blazor/pull/4389) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Backtop 滚动死循环。[#4391](https://github.com/ant-design-blazor/ant-design-blazor/pull/4391) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Upload 的 OneOf 类型转换报错。[#4390](https://github.com/ant-design-blazor/ant-design-blazor/pull/4390) [@ElderJames](https://github.com/ElderJames)

### 1.1.0

`2024-12-31`

元旦快乐！

- Table
  - 🆕 新增 条纹样式 Striped 属性。[#4372](https://github.com/ant-design-blazor/ant-design-blazor/pull/4372) [@zandiarash](https://github.com/zandiarash)
  - 🆕 新增 树行结构支持延迟加载（即在无子数据时也能显示展开按钮）。[#4228](https://github.com/ant-design-blazor/ant-design-blazor/pull/4228) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 支持手动设置列索引 ColIndex，以避免动态列时无法正确计算顺序。[#4343](https://github.com/ant-design-blazor/ant-design-blazor/pull/4343) [@GoldSucc](https://github.com/GoldSucc)
  - 🐞 修复 在行分组的设置发生改变时，无法正确删除分组行。[#4366](https://github.com/ant-design-blazor/ant-design-blazor/pull/4366) [@GlodenBoy](https://github.com/GlodenBoy)
  - 🐞 修复 分组后的数据行状态不能刷新。[#4368](https://github.com/ant-design-blazor/ant-design-blazor/pull/4368) [@GlodenBoy](https://github.com/GlodenBoy)
  - 🐞 修复 行分组由于未正确缓存导致无法通过程序方式展开。[#4358](https://github.com/ant-design-blazor/ant-design-blazor/pull/4358) [@ElderJames](https://github.com/ElderJames)
  - 🛠 重构 允许 RebuildColumns 支持重写。[#4242](https://github.com/ant-design-blazor/ant-design-blazor/pull/4242) [@agolub-s](https://github.com/agolub-s)
  - 🛠 新增 IColumn 的 Hidden 属性。[#4344](https://github.com/ant-design-blazor/ant-design-blazor/pull/4344) [@pathartl](https://github.com/pathartl)

- 🛠 参数标准化，将有限选项的参数类型从字符串改为枚举。[#4352](https://github.com/ant-design-blazor/ant-design-blazor/pull/4352) [@pathartl](https://github.com/pathartl)

- 🆕 新增 Icon 支持自定义设置 svg 字符串。[#4380](https://github.com/ant-design-blazor/ant-design-blazor/pull/4380) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Typography  为 Title 增加级别 5 。[#4377](https://github.com/ant-design-blazor/ant-design-blazor/pull/4377) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Modal 打开后触发事件 AfterOpen。[#4353](https://github.com/ant-design-blazor/ant-design-blazor/pull/4353) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tabs 新增标签时同时渲染ink。[#4387](https://github.com/ant-design-blazor/ant-design-blazor/pull/4387) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Statistic 当提取 double 的整数部分时发生溢出异常。[#4383](https://github.com/ant-design-blazor/ant-design-blazor/pull/4383) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Input 使 TextArea 支持 OnPressEnter 事件。[#4381](https://github.com/ant-design-blazor/ant-design-blazor/pull/4381) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Spin 展示文本时的样式。[#4351](https://github.com/ant-design-blazor/ant-design-blazor/pull/4351) [@ElderJames](https://github.com/ElderJames)
- 📖 修复 Layout 示例中 Menu 的 title。[#4367](https://github.com/ant-design-blazor/ant-design-blazor/pull/4367) [@ElderJames](https://github.com/ElderJames)
- 📖 使用 DelegatingHandler 来处理动态 Table 示例中的请求。[#4379](https://github.com/ant-design-blazor/ant-design-blazor/pull/4379) [@zandiarash](https://github.com/zandiarash)
- 📖 文档 修复切换菜单时同名 demo 未更新。[#4386](https://github.com/ant-design-blazor/ant-design-blazor/pull/4386) [@ElderJames](https://github.com/ElderJames)

#### 破坏性更新：

主要变更说明在 [#4352](https://github.com/ant-design-blazor/ant-design-blazor/pull/4352) 这个 PR 中，将一批魔法字符串改为了枚举，直接运行会导致报错，但这有利于提高日后的可维护性。

另外还有三个命名被修改：
Twotone 改为 TwoTone
Input.OnkeyDown 改为 Input.OnKeyDown
Input.OnkeyUp 改为 Input.OnKeyUp

### 1.0.1

`2024-11-18`

- 🔥 更新到 .NET 9 正式版。[#4330](https://github.com/ant-design-blazor/ant-design-blazor/pull/4330) [@ElderJames](https://github.com/ElderJames)
- 🛠 更新基础设施到 .NET 9。[#4335](https://github.com/ant-design-blazor/ant-design-blazor/pull/4335) [@ElderJames](https://github.com/ElderJames)

- TreeSelect
  - 🐞 修复 TreeSelect 可能的空引用异常。[#4316](https://github.com/ant-design-blazor/ant-design-blazor/pull/4316) [@pankey888](https://github.com/pankey888)
  - 🐞 修复 TreeSelect 在模版options方式时默认选中无效。[#4315](https://github.com/ant-design-blazor/ant-design-blazor/pull/4315) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Checkbox 在自定义CheckboxGroup 时用ItemValue 比对无效。[#4333](https://github.com/ant-design-blazor/ant-design-blazor/pull/4333) [@pankey888](https://github.com/pankey888)
- 📖 优化 Table 英文文档。[#4331](https://github.com/ant-design-blazor/ant-design-blazor/pull/4331) [@pathartl](https://github.com/pathartl)
- 📖 更新 本地化 英文文档。[#4319](https://github.com/ant-design-blazor/ant-design-blazor/pull/4319) [@JackLovel](https://github.com/JackLovel)
- 🌐 增加 ReuseTabs Reload 属性荷兰语翻译。[#4323](https://github.com/ant-design-blazor/ant-design-blazor/pull/4323) [@rtrocmn](https://github.com/rtrocmn)


### 1.0.0

`2024-11-01`

- Modal
  - 🐞 修复 Modal 确保在渲染后调用JS。[#4311](https://github.com/ant-design-blazor/ant-design-blazor/pull/4311) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Modal 用css实现打开遮罩时隐藏滚动条。[#4302](https://github.com/ant-design-blazor/ant-design-blazor/pull/4302) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Modal 的遮罩点击事件。[#4294](https://github.com/ant-design-blazor/ant-design-blazor/pull/4294) [@chazikaifa](https://github.com/chazikaifa)

- ⚡️ 改进 Table 当 PageIndex 小于1时不执行加载。[#4305](https://github.com/ant-design-blazor/ant-design-blazor/pull/4305) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Grid 中Col 级联样式的分号。[#4301](https://github.com/ant-design-blazor/ant-design-blazor/pull/4301) [@pathartl](https://github.com/pathartl)
- 🐞 修复 Drawer 利用 css 隐藏滚动条。[#4299](https://github.com/ant-design-blazor/ant-design-blazor/pull/4299) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Form 设置字典作为Model时会抛出NRE异常。[#4296](https://github.com/ant-design-blazor/ant-design-blazor/pull/4296) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Tabs 在不设置TabPane 的 Key 时仍然工作。[#4297](https://github.com/ant-design-blazor/ant-design-blazor/pull/4297) [@ElderJames](https://github.com/ElderJames)
- 🌐 补充荷兰语部分翻译。[#4313](https://github.com/ant-design-blazor/ant-design-blazor/pull/4313) [@rtrocmn](https://github.com/rtrocmn)
- 🌐 补充意大利语翻译。[#4303](https://github.com/ant-design-blazor/ant-design-blazor/pull/4303) [@ElderJames](https://github.com/ElderJames)

### 1.0.0 RC 3

`2024-10-22`

- 🆕 新增 Upload 自动在请求注入 AntiforgeryToken。[#4271](https://github.com/ant-design-blazor/ant-design-blazor/pull/4271) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Steps 常量值。[#4267](https://github.com/ant-design-blazor/ant-design-blazor/pull/4267) [@pathartl](https://github.com/pathartl)
- 🆕 新增 space 常量值。[#4263](https://github.com/ant-design-blazor/ant-design-blazor/pull/4263) [@pathartl](https://github.com/pathartl)

- TreeSelect
  - 🆕 新增 一些控制 Tree 的方法。[#4283](https://github.com/ant-design-blazor/ant-design-blazor/pull/4283) [@pankey888](https://github.com/pankey888)
  - 🐞 修复 当 TItemValue 为非可空类型时无法清楚选项的问题。[#4291](https://github.com/ant-design-blazor/ant-design-blazor/pull/4291) [@pankey888](https://github.com/pankey888)

- Select
  - 🐞 修复 搜索模式的输入框焦点。[#4286](https://github.com/ant-design-blazor/ant-design-blazor/pull/4286) [@pankey888](https://github.com/pankey888)
  - 🐞 修复 搜索关键字的清除和恢复。[#4281](https://github.com/ant-design-blazor/ant-design-blazor/pull/4281) [#4276](https://github.com/ant-design-blazor/ant-design-blazor/pull/4276) [@ElderJames](https://github.com/ElderJames)

- 💄 修复 Tree 连线的缩进样式。[#4290](https://github.com/ant-design-blazor/ant-design-blazor/pull/4290) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Overlay 跟随 Trigger 滚动。[#4285](https://github.com/ant-design-blazor/ant-design-blazor/pull/4285) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Empty 在 Select 中不显示。[#4282](https://github.com/ant-design-blazor/ant-design-blazor/pull/4282) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 AutoComplete 避免打开空的下拉面板。[#4284](https://github.com/ant-design-blazor/ant-design-blazor/pull/4284) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Modal 的按钮默认大小为 Default。[#4268](https://github.com/ant-design-blazor/ant-design-blazor/pull/4268) [@wangj90](https://github.com/wangj90)
- 🐞 修复 Tabs 中 TabPanes 无法渲染的问题。[#4269](https://github.com/ant-design-blazor/ant-design-blazor/pull/4269) [@ysj265](https://github.com/ysj265)
- 🐞 修复 cascader 错误的打开位置和选中值的顺序。[#4265](https://github.com/ant-design-blazor/ant-design-blazor/pull/4265) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 ReuseTabs 部分配置失效。[#4266](https://github.com/ant-design-blazor/ant-design-blazor/pull/4266) [@ElderJames](https://github.com/ElderJames)
- 🛠 项目配置更新 .NET 9 目标。[#4262](https://github.com/ant-design-blazor/ant-design-blazor/pull/4262) [@WeihanLi](https://github.com/WeihanLi)

### 1.0.0 RC 2

`2024-10-09`

> 人生自古谁无死？留取丹心照汗青。

- 🔥 支持 .NET 9.0，文档站点使用 RC2 发布。[#4196](https://github.com/ant-design-blazor/ant-design-blazor/pull/4196) [@ElderJames](https://github.com/ElderJames)

- Tabs
  - ⚡️ 优化 防止切换时重新渲染 TabPane。[#4255](https://github.com/ant-design-blazor/ant-design-blazor/pull/4255) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Tabs 超出宽度时滚动显示问题。[#4253](https://github.com/ant-design-blazor/ant-design-blazor/pull/4253) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Tab 宽度在添加新tab时重新获取。[#4239](https://github.com/ant-design-blazor/ant-design-blazor/pull/4239) [@agolub-s](https://github.com/agolub-s)
  - 🐞 修复 Tabs 在隐藏时会导致无限循环获取尺寸。[#4225](https://github.com/ant-design-blazor/ant-design-blazor/pull/4225) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 GoTo 方法会抛空引用异常。[#4217](https://github.com/ant-design-blazor/ant-design-blazor/pull/4217) [@ElderJames](https://github.com/ElderJames)

- Table
  - 🐞 设置 AutoHeight 时默认设置 TableLayout 为 fixed。[#4258](https://github.com/ant-design-blazor/ant-design-blazor/pull/4258) [@ElderJames](https://github.com/ElderJames)
  - 🐞 重构 AutoHeight 算法。[#4238](https://github.com/ant-design-blazor/ant-design-blazor/pull/4238) [@ysj265](https://github.com/ysj265)
  
- Form
  - 🆕 新增 Form 支持字段验证。[#4240](https://github.com/ant-design-blazor/ant-design-blazor/pull/4240) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Form 避免绑定空模型时循环渲染。[#4254](https://github.com/ant-design-blazor/ant-design-blazor/pull/4254) [@ElderJames](https://github.com/ElderJames)

- Input
  - 🆕 增加 ShowClear 属性控制是否显示清空按钮。[#4221](https://github.com/ant-design-blazor/ant-design-blazor/pull/4221) [@rhodon-jargon](https://github.com/rhodon-jargon)
  - 🐞 给 Search 组件的 Button 添加 Disabled 属性。[#4214](https://github.com/ant-design-blazor/ant-design-blazor/pull/4214) [@jeffersyuan1976](https://github.com/jeffersyuan1976)

- 🆕 新增 Popconfirm 支持隐藏按钮。[#3895](https://github.com/ant-design-blazor/ant-design-blazor/pull/3895) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Tree 的 Selectable 和 SelectableExpression 属性设置 TreeNode可选择。[#4229](https://github.com/ant-design-blazor/ant-design-blazor/pull/4229) [@pankey888](https://github.com/pankey888)
- 🐞 修复 TreeSelect 有时会触发 OnSelectedItemChanged 两次。[#4232](https://github.com/ant-design-blazor/ant-design-blazor/pull/4232) [@pankey888](https://github.com/pankey888)
- 🐞 修复 DatePicker 避免浏览器自动填充时异常。[#4251](https://github.com/ant-design-blazor/ant-design-blazor/pull/4251) [@ogix](https://github.com/ogix)
- 🐞 修复 Menu 的 tooltip 失效。[#4222](https://github.com/ant-design-blazor/ant-design-blazor/pull/4222) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Modal 当用服务创建时按钮没有本地化。[#4215](https://github.com/ant-design-blazor/ant-design-blazor/pull/4215) [@ElderJames](https://github.com/ElderJames)

### 1.0.0 RC 1

`2024-09-19`

� 中秋快乐！

- ReuseTabs 
  - 🔥 重构 无需级联 RouteData 也能实现标签页。[#4205](https://github.com/ant-design-blazor/ant-design-blazor/pull/4205) [@ElderJames](https://github.com/ElderJames)
  - 🆕 重构 继承 Tabs 的所有功能。[#4200](https://github.com/ant-design-blazor/ant-design-blazor/pull/4200) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 单例页面，实现不同参数重复利用一个页面实例。[#4151](https://github.com/ant-design-blazor/ant-design-blazor/pull/4151) [@pankey888](https://github.com/pankey888)

- Tabs
  - 🐞 修复 拖拽和右键菜单冲突。[#4199](https://github.com/ant-design-blazor/ant-design-blazor/pull/4199) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 删除激活页后应激活前一个页签。[#4197](https://github.com/ant-design-blazor/ant-design-blazor/pull/4197) [@ElderJames](https://github.com/ElderJames)

- Form
  - 🆕 新增 文字提示图标。[#4211](https://github.com/ant-design-blazor/ant-design-blazor/pull/4211) [@jeffersyuan1976](https://github.com/jeffersyuan1976)
  - 🆕 新增 GenerateFormItem 自动生成表单时，识别TModel属性中的ReadOnlyAttribute 标识，并使禁用组件生效。[#4191](https://github.com/ant-design-blazor/ant-design-blazor/pull/4191) [@lishewen](https://github.com/lishewen)

- 🐞 修复 Table 解决 AutoHeight 和 Resizable 冲突。[#4195](https://github.com/ant-design-blazor/ant-design-blazor/pull/4195) [@ysj265](https://github.com/ysj265)
- 🐞 修复 Menu 选中菜单项在页面刷新后变为未选中。[#4194](https://github.com/ant-design-blazor/ant-design-blazor/pull/4194) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Button 的 AutoLoading 属性，在 Task 执行时自动切换 Loading 状态。[#4193](https://github.com/ant-design-blazor/ant-design-blazor/pull/4193) [@ElderJames](https://github.com/ElderJames)
- 📖 文档 启用预渲染，优化 SEO。[#4207](https://github.com/ant-design-blazor/ant-design-blazor/pull/4207) [@jsakamoto](https://github.com/jsakamoto)

破坏性更新：

- ReuseTabs: 旧版本中的 Body 属性被重命名为 TabPaneTemplate 属性，此版本之后的 Body 属性只用于绑定Layout组件 Body 属性。

### 0.20.4

`2024-09-09`

- 🐞 修复 Tree 恢复 pointer events。[#4176](https://github.com/ant-design-blazor/ant-design-blazor/pull/4176) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tabs 的新增和删除。[#4173](https://github.com/ant-design-blazor/ant-design-blazor/pull/4173) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Select 元素引用和重复的div。[#4175](https://github.com/ant-design-blazor/ant-design-blazor/pull/4175) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 AutoComplete 应在 options 赋值后立即打开选择面板。[#4172](https://github.com/ant-design-blazor/ant-design-blazor/pull/4172) [@ElderJames](https://github.com/ElderJames)
- 🔥 新增 Table 支持自动高度 AutoHeight 属性。[#4168](https://github.com/ant-design-blazor/ant-design-blazor/pull/4168) [@ysj265](https://github.com/ysj265)
- 🐞 修复 Drawer 关闭效果。[#4166](https://github.com/ant-design-blazor/ant-design-blazor/pull/4166) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Card 下面的 Tabs不能记忆输入状态的问题。[#4164](https://github.com/ant-design-blazor/ant-design-blazor/pull/4164) [@ysj265](https://github.com/ysj265)
- 🐞 修复 DatePicker 的公开方法。[#4153](https://github.com/ant-design-blazor/ant-design-blazor/pull/4153) [@youcaiyouyoucai](https://github.com/youcaiyouyoucai)
- 🛠 重构 Form 的验证模式默认混合 。[#4163](https://github.com/ant-design-blazor/ant-design-blazor/pull/4163) [@ElderJames](https://github.com/ElderJames)

### 0.20.3

`2024-09-03`

- 🔥 文档 API 改为从公开方法注释生成，补齐所有属性的注释。[#3013](https://github.com/ant-design-blazor/ant-design-blazor/pull/3013) [@kooliokey](https://github.com/kooliokey)

- Table
  - 🆕 新增 Table SelectAll 事件回调。[#4142](https://github.com/ant-design-blazor/ant-design-blazor/pull/4142) [@ElderJames](https://github.com/ElderJames)
  - 🛠 重构 Table 在动态调整列时减少渲染。[#4138](https://github.com/ant-design-blazor/ant-design-blazor/pull/4138) [@ElderJames](https://github.com/ElderJames)

- Tabs
  - 🐞 修复 Tabs 拖动排序。[#4147](https://github.com/ant-design-blazor/ant-design-blazor/pull/4147) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Reusetabs 当 Menu 加载后刷新标签。[#4112](https://github.com/ant-design-blazor/ant-design-blazor/pull/4112) [@ElderJames](https://github.com/ElderJames)

- Form
  - 🐞 修复 Form 检查disposing时 _editContext 是否存在。[#4136](https://github.com/ant-design-blazor/ant-design-blazor/pull/4136) [@ogix](https://github.com/ogix)
  - 🐞 修复 Form 在验证可空类型的时候抛异常。[#4137](https://github.com/ant-design-blazor/ant-design-blazor/pull/4137) [@ElderJames](https://github.com/ElderJames)

- Drawer
  - 🛠 重构 Drawer 的 Height 和 Width 属性改为字符串，以支持百分比等单位。[#4120](https://github.com/ant-design-blazor/ant-design-blazor/pull/4120) [@kx500](https://github.com/kx500)
  - 🐞 修复 drawer 关闭动画效果。[#4122](https://github.com/ant-design-blazor/ant-design-blazor/pull/4122) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Select 当 values 某些场景下被修改时抛异常。[#4117](https://github.com/ant-design-blazor/ant-design-blazor/pull/4117) [@pankey888](https://github.com/pankey888)
- 🐞 修复 Menu 在处理 OnClick 事件前关闭overlay。[#4121](https://github.com/ant-design-blazor/ant-design-blazor/pull/4121) [@pankey888](https://github.com/pankey888)
- 🛠 优化 全局服务依赖注册的生命周期，WebAssembly 使用单例。[#4123](https://github.com/ant-design-blazor/ant-design-blazor/pull/4123) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 语言包文件反序列化异常。[#4116](https://github.com/ant-design-blazor/ant-design-blazor/pull/4116) [@ogix](https://github.com/ogix)

- Codebase
  - 🛠 启用 CA1852/IDE0040/IDE0005 代码分析规则。[#4126](https://github.com/ant-design-blazor/ant-design-blazor/pull/4126) [@WeihanLi](https://github.com/WeihanLi)
  - 🛠 优化 构建目标项目的时候才复制智能提醒 xml 文件，以减少打包体积。[#4129](https://github.com/ant-design-blazor/ant-design-blazor/pull/4129) [@stratosblue](https://github.com/stratosblue)
  - 🛠 移除 重复的 InternalsVisibleTo 设置。[#4124](https://github.com/ant-design-blazor/ant-design-blazor/pull/4124) [@WeihanLi](https://github.com/WeihanLi)
  - 🛠 移除 Microsoft.SourceLink.GitHub 包引用。[#4125](https://github.com/ant-design-blazor/ant-design-blazor/pull/4125) [@WeihanLi](https://github.com/WeihanLi)

**不兼容变更**

- Card 中的 CardTabs 属性已删除, 应直接将 Tabs 放到 Card 的 ChildContent 中。
- Darwer 中的 Height 和 Width 属性改为字符串，因此您原来绑定的 int 类型变量需要转换后赋值。

### 0.20.2

`2024-08-20`

- 🔥 增加 API 智能提醒语言支持 zh-CN、ja-JP、ko-KR。[#4107](https://github.com/ant-design-blazor/ant-design-blazor/pull/4107) [@ElderJames](https://github.com/ElderJames)

- Form
  - 🆕 增加 支持无绑定属性验证。[#4102](https://github.com/ant-design-blazor/ant-design-blazor/pull/4102) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 CompareAttribute 验证。[#4098](https://github.com/ant-design-blazor/ant-design-blazor/pull/4098) [@ElderJames](https://github.com/ElderJames)
  - 🛠 删除复杂对象验证器。[#4098](https://github.com/ant-design-blazor/ant-design-blazor/pull/4098) [@ElderJames](https://github.com/ElderJames)
  - 📖 文档 增加 Table 录入验证示例。[#4102](https://github.com/ant-design-blazor/ant-design-blazor/pull/4102) [@ElderJames](https://github.com/ElderJames)
  - 📖 文档 增加静态渲染表单验证示例。[#4105](https://github.com/ant-design-blazor/ant-design-blazor/pull/4105) [@ElderJames](https://github.com/ElderJames)

- Table
  - 🐞 修复 在跳过两边的列设置固定列，或全部没设置Width的时候固定列时，样式错乱的问题。[#4097](https://github.com/ant-design-blazor/ant-design-blazor/pull/4097) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 动态修改列显示隐藏时样式错乱的问题。[#4093](https://github.com/ant-design-blazor/ant-design-blazor/pull/4093) [@ElderJames](https://github.com/ElderJames)

- 💄 修复 Upload 的拖拽上传区域“未选择文件”的提示。[#4096](https://github.com/ant-design-blazor/ant-design-blazor/pull/4096) [@pankey888](https://github.com/pankey888)
- 🐞 修复 Tabs 的定位 ink 在 ActiveKey 变化时不更新。[#4094](https://github.com/ant-design-blazor/ant-design-blazor/pull/4094) [@pankey888](https://github.com/pankey888)
- 📖 文档 修改图表 GroupedColumn 示例。[#3524](https://github.com/ant-design-blazor/ant-design-blazor/pull/3524) [@SuperQuestions](https://github.com/SuperQuestions)


### 0.20.1

`2024-08-15`

- Table
  - 🆕 新增 多级行分组支持。[#4089](https://github.com/ant-design-blazor/ant-design-blazor/pull/4089) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 树形数据的 Children 无法更新。[#4086](https://github.com/ant-design-blazor/ant-design-blazor/pull/4086) [@ysj265](https://github.com/ysj265)

- Form  
  - 🆕 新增 利用 FormItem 的 Label 或者特性指定的名称作为错误信息的字段名。[#4074](https://github.com/ant-design-blazor/ant-design-blazor/pull/4074) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 验证异常。[#4080](https://github.com/ant-design-blazor/ant-design-blazor/pull/4080) [#4084](https://github.com/ant-design-blazor/ant-design-blazor/pull/4084) [@ElderJames](https://github.com/ElderJames)

- 🆕 增加 Select 支持 @bind-Visible 控制打开关闭。[#4079](https://github.com/ant-design-blazor/ant-design-blazor/pull/4079) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tree 节点的 CheckAllChildren 方法未更新 CheckedKeys。[#4088](https://github.com/ant-design-blazor/ant-design-blazor/pull/4088) [@pankey888](https://github.com/pankey888)
- 🌐 更新 Form 的语言包。[#4076](https://github.com/ant-design-blazor/ant-design-blazor/pull/4076) [@ElderJames](https://github.com/ElderJames)
- 🌐 增加 更多国际化语言包。[#4078](https://github.com/ant-design-blazor/ant-design-blazor/pull/4078) [@ElderJames](https://github.com/ElderJames)
- 📖 完善 国际化 文档。[#4085](https://github.com/ant-design-blazor/ant-design-blazor/pull/4085) [@ElderJames](https://github.com/ElderJames)

### 0.20.0

`2024-08-07`

- Form
  - 🔥 新增 验证信息内置本地化。[#4058](https://github.com/ant-design-blazor/ant-design-blazor/pull/4058) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加 直接设置模型字段的验证信息。[#4014](https://github.com/ant-design-blazor/ant-design-blazor/pull/4014) [@PengYuee](https://github.com/PengYuee)
  - 🆕 新增 支持数组索引值的验证。[#4053](https://github.com/ant-design-blazor/ant-design-blazor/pull/4053) [@ElderJames](https://github.com/ElderJames)

- Table
  - 🔥 新增 内置筛选器支持时间区间条件，筛选指定时间范围内的数据。[#4036](https://github.com/ant-design-blazor/ant-design-blazor/pull/4036) [@ElderJames](https://github.com/ElderJames)
  - 🔥 新增 内置筛选器支持 DateOnly 与 TimeOnly 类型的属性。[#4034](https://github.com/ant-design-blazor/ant-design-blazor/pull/4034) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 行分组标题模板。[#3962](https://github.com/ant-design-blazor/ant-design-blazor/pull/3962) [@moumousoup](https://github.com/moumousoup)
  - 🆕 新增 滚动条自适应。[#4064](https://github.com/ant-design-blazor/ant-design-blazor/pull/4064) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 GetFilterExpression 方法获取当前的筛选表达式。[#3991](https://github.com/ant-design-blazor/ant-design-blazor/pull/3991) [@Ashhhhhh520](https://github.com/Ashhhhhh520)
  - 🆕 增加 InvokeDataSourceHasChanged 方法给派生类在刷新 DataSource 后调用，以刷新 Table 状态。[#4067](https://github.com/ant-design-blazor/ant-design-blazor/pull/4067) [@ElderJames](https://github.com/ElderJames)

- Modal
  - 🆕 新增支持 ForceRender 属性，提前渲染 DOM。[#4041](https://github.com/ant-design-blazor/ant-design-blazor/pull/4041) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 支持在关闭 Mask 时选择弹框下层的文本。[#4040](https://github.com/ant-design-blazor/ant-design-blazor/pull/4040) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 支持最大化垂直调整。[#4040](https://github.com/ant-design-blazor/ant-design-blazor/pull/4040) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 调整大小后最大化不正常。[#4070](https://github.com/ant-design-blazor/ant-design-blazor/pull/4070) [@ElderJames](https://github.com/ElderJames)

- Overlay
  - 🆕 新增 支持 @bind-Visible 双向绑定。[#4057](https://github.com/ant-design-blazor/ant-design-blazor/pull/4057) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 预渲染时调用 JS。[#4068](https://github.com/ant-design-blazor/ant-design-blazor/pull/4068) [@ElderJames](https://github.com/ElderJames)

- 🔥 新增 ReuseTabs 页签的标题自动从页面路由匹配的菜单数据中生成。[#3960](https://github.com/ant-design-blazor/ant-design-blazor/pull/3960) [@JaneConan](https://github.com/JaneConan)
- 🔥 新增 Breadcrumb 从菜单获取当前路径。[#4065](https://github.com/ant-design-blazor/ant-design-blazor/pull/4065) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Tag 图标主题属性。[#4063](https://github.com/ant-design-blazor/ant-design-blazor/pull/4063) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Carousel 的 Dots 与 DotsClass 属性用于隐藏翻页或设置样式。[#4062](https://github.com/ant-design-blazor/ant-design-blazor/pull/4062) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Cascader 的 Placement 属性。[#4046](https://github.com/ant-design-blazor/ant-design-blazor/pull/4046) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Watermark 支持内容更新。[#4043](https://github.com/ant-design-blazor/ant-design-blazor/pull/4043) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tree 同时设置 DataItem 时 TreeNods 不生效 (#2759)。[#4054](https://github.com/ant-design-blazor/ant-design-blazor/pull/4054) [@pankey888](https://github.com/pankey888)

### 0.19.7

`2024-7-31`

- 📖 升级图表组件到0.5.5。[#4047](https://github.com/ant-design-blazor/ant-design-blazor/pull/4047) [@jeffersyuan1976](https://github.com/jeffersyuan1976)
- 🆕 优化 DatePicker 绑定值根据 ShowTime 的格式，日期则去掉时间。[#4029](https://github.com/ant-design-blazor/ant-design-blazor/pull/4029) [@ElderJames](https://github.com/ElderJames)

- TreeSelect
  - 🐞 修复 TreeSelect 值绑定问题 (#4000)。[#4012](https://github.com/ant-design-blazor/ant-design-blazor/pull/4012) [@pankey888](https://github.com/pankey888)
  - 🐞 修复 TreeSelect 当不设置title时的搜索异常。[#4024](https://github.com/ant-design-blazor/ant-design-blazor/pull/4024) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Radio 在 Modal 不刷新。[#4023](https://github.com/ant-design-blazor/ant-design-blazor/pull/4023) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Input 未绑定 Class 到 input 元素。[#4005](https://github.com/ant-design-blazor/ant-design-blazor/pull/4005) [@ElderJames](https://github.com/ElderJames)
- 🛠 重构 Select 友好提示设置 CustomTagLabelToValue。[#4049](https://github.com/ant-design-blazor/ant-design-blazor/pull/4049) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Modal 的导航移除异常。[#4039](https://github.com/ant-design-blazor/ant-design-blazor/pull/4039) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Segmented 绑定 Value 会被 AddItem 修改。[#4051](https://github.com/ant-design-blazor/ant-design-blazor/pull/4051) [@pankey888](https://github.com/pankey888)
- 🐞 修复 Table 缓存的循环引用问题。[#4048](https://github.com/ant-design-blazor/ant-design-blazor/pull/4048) [@ElderJames](https://github.com/ElderJames)


### 0.19.6

`2024-7-22`

- Table
  - 🐞 修复 Table 树形数据单选问题。[#4002](https://github.com/ant-design-blazor/ant-design-blazor/pull/4002) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 树形数据选择行导致循环渲染。[#3998](https://github.com/ant-design-blazor/ant-design-blazor/pull/3998) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 RangePicker 的预设范围按钮点击后不触发OnChange事件。[#3999](https://github.com/ant-design-blazor/ant-design-blazor/pull/3999) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tree 当只有第一个子节点在CheckedKeys/DefaultCheckedKeys中，父节点和其他兄弟节点也被勾选的问题。[#3985](https://github.com/ant-design-blazor/ant-design-blazor/pull/3985) [@pankey888](https://github.com/pankey888)
- 🐞 修复 Modal 当 DestroyOnClose 为true 时，关闭弹窗会出现异常。[#3982](https://github.com/ant-design-blazor/ant-design-blazor/pull/3982) [@ElderJames](https://github.com/ElderJames)


### 0.19.5

`2024-7-15`

*公告：我们把文档项目分离到单独的仓库，计划会升级为文档系统，敬请关注和参与贡献：https://github.com/ElderJames/BlazorSiteGenerator*

- 📖 新增 Table 基础的编辑和搜索示例，替换原来Blazor复刻示例。[#3970](https://github.com/ant-design-blazor/ant-design-blazor/pull/3970) [@ElderJames](https://github.com/ElderJames)

- Modal
  - 🐞 修复 关闭时会抛出JS异常，且无法再次打开。[#3973](https://github.com/ant-design-blazor/ant-design-blazor/pull/3973) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 在跳转到同一个页面时无法再次打开。[#3963](https://github.com/ant-design-blazor/ant-design-blazor/pull/3963) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Menu 在首次加载时会被外部重新渲染时取消选中。[#3976](https://github.com/ant-design-blazor/ant-design-blazor/pull/3976) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Textarea 无边框样式丢失。[#3975](https://github.com/ant-design-blazor/ant-design-blazor/pull/3975) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 AutoComplete 在有选项时点击输入框可以打开下拉框。[#3971](https://github.com/ant-design-blazor/ant-design-blazor/pull/3971) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 FormItem 修复空引用异常。[#3967](https://github.com/ant-design-blazor/ant-design-blazor/pull/3967) [@agolub-s](https://github.com/agolub-s)
- 🐞 修复 Tabs 当 Tab 标题更新时下标长度未更新。[#3978](https://github.com/ant-design-blazor/ant-design-blazor/pull/3978) [@ElderJames](https://github.com/ElderJames)

### 0.19.4

`2024-7-03`

- 🔥 Ant Design Icons Blazor 组件库发布！[ant-design-icons-blazor](https://github.com/ant-design-blazor/ant-design-icons-blazor)
- 🔥 新增 Form 自动生成组件GenerateFormItem。[#3877](https://github.com/ant-design-blazor/ant-design-blazor/pull/3877) [@dessli](https://github.com/dessli)

- Tree
  - 🆕 新增 全选/取消全部选择两个方法。[#3937](https://github.com/ant-design-blazor/ant-design-blazor/pull/3937) [@pankey888](https://github.com/pankey888)
  - 🐞 修复 当 CheckOnClickNode 为 true 时hover样式。[#3952](https://github.com/ant-design-blazor/ant-design-blazor/pull/3952) [@pankey888](https://github.com/pankey888)
  - 🐞 修复 全选包括子节点。[#3938](https://github.com/ant-design-blazor/ant-design-blazor/pull/3938) [@pankey888](https://github.com/pankey888)

- TreeSelect
  - 🆕 新增 TreeCheckStrictly和 ShowCheckedStrategy 来设置勾选的节点和绑定策略。[#3946](https://github.com/ant-design-blazor/ant-design-blazor/pull/3946) [@pankey888](https://github.com/pankey888)
  - 🆕 新增  的TreeDefaultExpandParent 与 TreeDefaultExpandedKeys 属性。[#3953](https://github.com/ant-design-blazor/ant-design-blazor/pull/3953) [@pankey888](https://github.com/pankey888)
  - 🆕 新增 利用 DropdownRender 支持自定义下来面板。[#3939](https://github.com/ant-design-blazor/ant-design-blazor/pull/3939) [@pankey888](https://github.com/pankey888)
  - 🐞 修复 指定 item本身作为绑定值。[#3954](https://github.com/ant-design-blazor/ant-design-blazor/pull/3954) [@ElderJames](https://github.com/ElderJames)

- 💄 修复 Checkbox 禁用样式。[#3948](https://github.com/ant-design-blazor/ant-design-blazor/pull/3948) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Select 数组范围溢出异常。[#3947](https://github.com/ant-design-blazor/ant-design-blazor/pull/3947) [@pankey888](https://github.com/pankey888)
- 🐞 修复 Modal 使用service打开确认框不返回Yes/No结果。[#3945](https://github.com/ant-design-blazor/ant-design-blazor/pull/3945) [@ElderJames](https://github.com/ElderJames)


### 0.19.3

`2024-6-26`

- 🆕 新增 Tree 和 TreeSelect 支持点击节点标题的 select、check 和 expand 效果。[#3902](https://github.com/ant-design-blazor/ant-design-blazor/pull/3902) [@pankey888](https://github.com/pankey888)
- 🛠 重构 Icon 直接用 JS 标签来引入 iconfont。[#3931](https://github.com/ant-design-blazor/ant-design-blazor/pull/3931) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Modal 用ConfirmService 打开的确认框，在点击ESC关闭时没设置tcs，导致await不继续执行。[#3934](https://github.com/ant-design-blazor/ant-design-blazor/pull/3934) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Table 空实体实例化表达式构建异常。[#3933](https://github.com/ant-design-blazor/ant-design-blazor/pull/3933) [@ElderJames](https://github.com/ElderJames)

### 0.19.2

`2024-6-24`

🔥 模板已经支持 Blazor WebApp 自动渲染模式，敬请尝试!

```
dotnet new update
dotnet new antdesign -n webapp --host webapp --full
```


- Table  
  - 🆕 新增 Filtered 属性设置某列的筛选是激活的。[#3911](https://github.com/ant-design-blazor/ant-design-blazor/pull/3911) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 支持自定义筛选器输入组件的属性。[#3897](https://github.com/ant-design-blazor/ant-design-blazor/pull/3897) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 创建 TItem 实例时未调用构造函数。[#3916](https://github.com/ant-design-blazor/ant-design-blazor/pull/3916) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 树形数据全选不会选中子行。[#3909](https://github.com/ant-design-blazor/ant-design-blazor/pull/3909) [@ElderJames](https://github.com/ElderJames)

- Tree
  - 🆕 新增 子节点 Checkable 属性，启用选中效果。[#3899](https://github.com/ant-design-blazor/ant-design-blazor/pull/3899) [@pankey888](https://github.com/pankey888)
  - 🛠 重构 Selected/Checked/Expanded 属性。[#3896](https://github.com/ant-design-blazor/ant-design-blazor/pull/3896) [@pankey888](https://github.com/pankey888)

- Select
  - 🐞 修复 在浏览器默认选中时抛异常。[#3925](https://github.com/ant-design-blazor/ant-design-blazor/pull/3925) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 异步方法调用。[#3912](https://github.com/ant-design-blazor/ant-design-blazor/pull/3912) [@WoogaAndrew](https://github.com/WoogaAndrew)
  - 🐞 修复 可搜索时输入文本框的宽度。[#3910](https://github.com/ant-design-blazor/ant-design-blazor/pull/3910) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Flags 枚举遇到0值不正确选中。[#3907](https://github.com/ant-design-blazor/ant-design-blazor/pull/3907) [@ElderJames](https://github.com/ElderJames)

- Checkbox
  - 🐞 修复 文本点击事件穿透。[#3918](https://github.com/ant-design-blazor/ant-design-blazor/pull/3918) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 CheckboxGroup 中勾选不正常。[#3903](https://github.com/ant-design-blazor/ant-design-blazor/pull/3903) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Image 无图片时 ImagePreviewGroup 展开抛异常。[#3917](https://github.com/ant-design-blazor/ant-design-blazor/pull/3917) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 AutoComplete 应当有匹配选项时才展开。[#3926](https://github.com/ant-design-blazor/ant-design-blazor/pull/3926) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 DatePicker 选择年份时单位换行。[#3919](https://github.com/ant-design-blazor/ant-design-blazor/pull/3919) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Menu 在选择/未选 时调用StateHasChanged[#3894](https://github.com/ant-design-blazor/ant-design-blazor/pull/3894) [@iits-timon-holzhaeuser](https://github.com/iits-timon-holzhaeuser)
- 🐞 修复 TreeSelect 单选时点击清空按钮会抛异常。[#3906](https://github.com/ant-design-blazor/ant-design-blazor/pull/3906) [@pankey888](https://github.com/pankey888)

### 0.19.1

`2024-5-27`

- Table
  - 🆕 新增 Table  GenerateColumns 增加HideColumnsByName 属性来隐藏某些列。[#3863](https://github.com/ant-design-blazor/ant-design-blazor/pull/3863) [@dessli](https://github.com/dessli)
  - 🐞 修复 Table 当绑定字段类型是可空时，枚举选择器抛出异常。[#3870](https://github.com/ant-design-blazor/ant-design-blazor/pull/3870) [@ElderJames](https://github.com/ElderJames)

- 🆕 新增 TreeSelect 级联类型参数特性，以支持省略TreeNode的类型参数。[#3864](https://github.com/ant-design-blazor/ant-design-blazor/pull/3864) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 InputNumber 可空的浮点类型没有依据 Precision 舍入。[#3868](https://github.com/ant-design-blazor/ant-design-blazor/pull/3868) [@Jtfk](https://github.com/Jtfk)
- 🐞 修复 Select 的 EnumSelect 绑定可控枚举类型时异常。[#3859](https://github.com/ant-design-blazor/ant-design-blazor/pull/3859) [@ElderJames](https://github.com/ElderJames)

### 0.19.0

`2024-5-7` 

- TreeSelect
  - 🆕 新增 ExpandedKeys 属性。[#3844](https://github.com/ant-design-blazor/ant-design-blazor/pull/3844) [@pankey888](https://github.com/pankey888)
  - 🆕 新增 TitleIconTemplate 属性。[#3834](https://github.com/ant-design-blazor/ant-design-blazor/pull/3834) [@pankey888](https://github.com/pankey888)
  - 🆕 新增 泛型值支持。[#3831](https://github.com/ant-design-blazor/ant-design-blazor/pull/3831) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 设置 TreeChecable 时勾选无效的问题。[#3839](https://github.com/ant-design-blazor/ant-design-blazor/pull/3839) [@pankey888](https://github.com/pankey888)
  - 🐞 修复 在重新渲染时树节点会收取。[#3827](https://github.com/ant-design-blazor/ant-design-blazor/pull/3827) [@pankey888](https://github.com/pankey888)

- Form
  - 🔥 支持静态 SSR 模型绑定和验证。[#3580](https://github.com/ant-design-blazor/ant-design-blazor/pull/3580) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 模型 DataAnnotations 的本地化支持。[#3823](https://github.com/ant-design-blazor/ant-design-blazor/pull/3823) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 null 引用异常。[#3815](https://github.com/ant-design-blazor/ant-design-blazor/pull/3815) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 当没有设置"变更时验证"，在提交前不应该验证。[#3812](https://github.com/ant-design-blazor/ant-design-blazor/pull/3812) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 动态模型的必填数据声明失效。[#3811](https://github.com/ant-design-blazor/ant-design-blazor/pull/3811) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 的 IsModified 属性在 ValidateOnChange 时失效。[#3795](https://github.com/ant-design-blazor/ant-design-blazor/pull/3795) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 动态字段的 Rules 验证方式。[#3791](https://github.com/ant-design-blazor/ant-design-blazor/pull/3791) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 FormItem 绑定字典类型判断。[#3787](https://github.com/ant-design-blazor/ant-design-blazor/pull/3787) [@tiansfather](https://github.com/tiansfather)
 

- ReuseTabs

  - 🆕 新增 公开 ReuseTabsService 的 Pages 属性，以供持久化等场景。[#3800](https://github.com/ant-design-blazor/ant-design-blazor/pull/3800) [@ElderJames](https://github.com/ElderJames)
  - 📖 增加 文档和示例。[#3802](https://github.com/ant-design-blazor/ant-design-blazor/pull/3802) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 不应该自动跳转到第一个固定的tab。[#3825](https://github.com/ant-design-blazor/ant-design-blazor/pull/3825) [@ElderJames](https://github.com/ElderJames)

- Select

  - 🆕 新增 EnumSelect 绑定有 Flags 特性的枚举类型时，支持用 Value 绑定组合值。[#3843](https://github.com/ant-design-blazor/ant-design-blazor/pull/3843) [@pankey888](https://github.com/pankey888)
  - 🐞 修复 选项列表随数据源变更重新排序。[#3806](https://github.com/ant-design-blazor/ant-design-blazor/pull/3806) [@miguelkmarques](https://github.com/miguelkmarques)
  - 📖 给 TableSelect 示例增加查询功能。[#3797](https://github.com/ant-design-blazor/ant-design-blazor/pull/3797) [@ElderJames](https://github.com/ElderJames)


- Modal
  - 🆕 新增 自定义 Header。[7be4807](https://github.com/ant-design-blazor/ant-design-blazor/commit/7be4807) [@Pat Hartl](https://github.com/Pat Hartl)
  - 🆕 新增 支持更新确认按钮的 Loading 状态。[#3796](https://github.com/ant-design-blazor/ant-design-blazor/pull/3796) [@ElderJames](https://github.com/ElderJames)
  - 🛠 重构 ModalService 中创建模态框的方法同步地返回 ModalRef。[#3794](https://github.com/ant-design-blazor/ant-design-blazor/pull/3794) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 JS 删除元素抛出异常。[#3847](https://github.com/ant-design-blazor/ant-design-blazor/pull/3796) [@ElderJames](https://github.com/ElderJames)

- DatePicker
  - 🐞 修复 DatePicker 在使用 ShowTime 时，Now 按钮无法使用，并在关闭时更改。[#3830](https://github.com/ant-design-blazor/ant-design-blazor/pull/3830) [@agolub-s](https://github.com/agolub-s)
  - 🐞 修复 RangePicker 双向绑定失效，预设范围无法更新。[#3850](https://github.com/ant-design-blazor/ant-design-blazor/pull/3850 ) [@ElderJames](https://github.com/ElderJames)

- 🔥 新增 交互式本地化服务。[#3804](https://github.com/ant-design-blazor/ant-design-blazor/pull/3804) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Checkbox 支持泛型值绑定。[#3715](https://github.com/ant-design-blazor/ant-design-blazor/pull/3715) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 IsExternalInit 的访问级别，避免与第三方库冲突。[#3799](https://github.com/ant-design-blazor/ant-design-blazor/pull/3799) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 InputNumber 粘贴带有分隔符的数字时无法绑定。[#3841](https://github.com/ant-design-blazor/ant-design-blazor/pull/3841) [@HuaFangYun](https://github.com/HuaFangYun)
- 🐞 修复 Overlay 多子级关闭时，父级联动冲突。[#3838](https://github.com/ant-design-blazor/ant-design-blazor/pull/3838) [@pankey888](https://github.com/pankey888)

### 0.18.3

`2024-4-9` 

- 🐞 修复 Table 重新渲染导致的行展开状态无法维持。[#3785](https://github.com/ant-design-blazor/ant-design-blazor/pull/3785) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Progress 环形进度条 strokecolor 单色不生效。[#3783](https://github.com/ant-design-blazor/ant-design-blazor/pull/3783) [@jeffersyuan1976](https://github.com/jeffersyuan1976)
- 🐞 修复  DatePicker  禁用日期逻辑在更大的范围选择中判断不正确。[#3781](https://github.com/ant-design-blazor/ant-design-blazor/pull/3781) [@ElderJames](https://github.com/ElderJames)
- 📖 修复 Charts 文档。[#3774](https://github.com/ant-design-blazor/ant-design-blazor/pull/3774) [@CAPCHIK](https://github.com/CAPCHIK)

Table 行状态行为变更：

在重新渲染或调用 `ITable.ReloadData()` 后， RowKey 与当前页数据的相同的行状态（如展开、选中）不会被重置。

### 0.18.2

`2024-4-2` 

- Form
  - 🆕 新增 自动填充属性 AutoComplete。[#3763](https://github.com/ant-design-blazor/ant-design-blazor/pull/3763) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 自定义控件的验证。[#3761](https://github.com/ant-design-blazor/ant-design-blazor/pull/3761) [@ElderJames](https://github.com/ElderJames)

- Table
  - 🐞 修复 Table 在外部设置排序时异常[#3766](https://github.com/ant-design-blazor/ant-design-blazor/pull/3766) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Table 选择行清理后，全选时还会被选中 （客户端数据）。[#3762](https://github.com/ant-design-blazor/ant-design-blazor/pull/3762) [@ElderJames](https://github.com/ElderJames)

### 0.18.1

`2024-3-21` 

是日春分

- 🆕 新增 Modal 自定义头部 (#3579)。[4cfeffd](https://github.com/ant-design-blazor/ant-design-blazor/commit/4cfeffd) [@Pat Hartl](https://github.com/Pat Hartl)

- Form
  - 🐞 修复 静态渲染时的绑定与验证，**正式支持静态渲染**。[#3580](https://github.com/ant-design-blazor/ant-design-blazor/pull/3580) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 当没有绑定时的异常。[#3717](https://github.com/ant-design-blazor/ant-design-blazor/pull/3717) [@ElderJames](https://github.com/ElderJames)

- Select
  - 🐞 修复 在筛选时隐藏空分组。[#3722](https://github.com/ant-design-blazor/ant-design-blazor/pull/3722) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
  - 🐞 修复 通过搜索选择值时清除输入。[#3726](https://github.com/ant-design-blazor/ant-design-blazor/pull/3726) [@agolub-s](https://github.com/agolub-s)

- 💄 优化 Upload 无按钮时的样式。[#3734](https://github.com/ant-design-blazor/ant-design-blazor/pull/3734) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Table 在设置 `ScrollY` 时伸缩列失效。[#3746](https://github.com/ant-design-blazor/ant-design-blazor/pull/3746) [@thirking](https://github.com/thirking)
- 🐞 修复 JS 序列化循环引用异常。[#3739](https://github.com/ant-design-blazor/ant-design-blazor/pull/3739) [@jxcproject](https://github.com/jxcproject)


### 0.18.0

`2024-02-29`

🐉龙年吉祥！

- Table
  - 🆕 新增 默认滚动条样式。[#3668](https://github.com/ant-design-blazor/ant-design-blazor/pull/3668) [@thirking](https://github.com/thirking)
  - 🐞 修复 内置的日期类型筛选器在绑定类型为可空时，修改值会引发异常。[#3704](https://github.com/ant-design-blazor/ant-design-blazor/pull/3704) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 固定列的Table中Header的阴影样式。[#3691](https://github.com/ant-design-blazor/ant-design-blazor/pull/3691) [@thirking](https://github.com/thirking)
  - 🐞 修复 筛选器跳动。[#3683](https://github.com/ant-design-blazor/ant-design-blazor/pull/3683) [@m-khrapunov](https://github.com/m-khrapunov)
  - 🐞 修复 DataSource 为空且类型时抽象类时，一直显示加载中不显示空状态。[#3688](https://github.com/ant-design-blazor/ant-design-blazor/pull/3688) [@ElderJames](https://github.com/ElderJames)

- Select
  - 🆕 新增 支持列表选择器（Table Select）。[#3693](https://github.com/ant-design-blazor/ant-design-blazor/pull/3693) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 Select  筛选表达式属性 FilterExpression 用于自定义筛选逻辑。[#3656](https://github.com/ant-design-blazor/ant-design-blazor/pull/3656) [@Magehernan](https://github.com/Magehernan)
  - 🐞 修复 固定输入（搜索）值不为null时选择内容中的占位符显示。[#3701](https://github.com/ant-design-blazor/ant-design-blazor/pull/3701) [@agolub-s](https://github.com/agolub-s)
  - 🐞 修复 错误的 HTML Title 显示。[#3695](https://github.com/ant-design-blazor/ant-design-blazor/pull/3695) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 的 DropdownRender 属性没有传入原内容。[#3675](https://github.com/ant-design-blazor/ant-design-blazor/pull/3675) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 在 From 中当 ValidateOnChange 为 true 时，修改绑定值不会更新选中项。[#3703](https://github.com/ant-design-blazor/ant-design-blazor/pull/3703) [@edwardbarford](https://github.com/edwardbarford)

- Form
  - 🆕 新增 Form 的 Method 属性用于适配 SSR 表单。[#3608](https://github.com/ant-design-blazor/ant-design-blazor/pull/3608) [@CrosRoad95](https://github.com/CrosRoad95)
  - 🆕 新增 FormItem Name 属性，基于 DataIndex 支持动态属性。[#3612](https://github.com/ant-design-blazor/ant-design-blazor/pull/3612) [@Zonciu](https://github.com/Zonciu)

- 🆕 新增 Tabs 为 ReuseTabsService 新增创建标签的方法`CreateTab`。[#3671](https://github.com/ant-design-blazor/ant-design-blazor/pull/3671) [@jxcproject](https://github.com/jxcproject)
- 🆕 新增 Comment 头像位置属性 Placement。[#3670](https://github.com/ant-design-blazor/ant-design-blazor/pull/3670) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Modal 重复移除DOM的问题。[#3673](https://github.com/ant-design-blazor/ant-design-blazor/pull/3673) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复 Message 在  Webview 上非线程安全的问题。[#3698](https://github.com/ant-design-blazor/ant-design-blazor/pull/3698) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复 Radio 切换选项后原选中样式未重置。[#3694](https://github.com/ant-design-blazor/ant-design-blazor/pull/3694) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Card 中的 Tabs 尺寸属性。[#3661](https://github.com/ant-design-blazor/ant-design-blazor/pull/3661) [@thirking](https://github.com/thirking)
- 🐞 修复 Segmented 的 SegmentedOption 标签 。[#3659](https://github.com/ant-design-blazor/ant-design-blazor/pull/3659) [@CrosRoad95](https://github.com/CrosRoad95)
- 📖 新增 Blazor WebApp 示例站点。[#3642](https://github.com/ant-design-blazor/ant-design-blazor/pull/3642) [@bxjg1987](https://github.com/bxjg1987)

### 0.17.4

`2024-02-01`

- Select
  - 🐞 修复 Select 禁用时 Input 还能输入的问题。[#3655](https://github.com/ant-design-blazor/ant-design-blazor/pull/3655) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Select 在启用虚拟化时滚动不正常。[#3625](https://github.com/ant-design-blazor/ant-design-blazor/pull/3625) [@Magehernan](https://github.com/Magehernan)
- 🐞 修复 Collapse 无动画时手风琴模式失效。[#3646](https://github.com/ant-design-blazor/ant-design-blazor/pull/3646) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Modal 居中时拖拽问题。[#3647](https://github.com/ant-design-blazor/ant-design-blazor/pull/3647) [@zxyao145](https://github.com/zxyao145)

### 0.17.3

`2024-01-14`

- Table
  - 🐞 修复 应只在翻页时清理行状态。[#3620](https://github.com/ant-design-blazor/ant-design-blazor/pull/3620) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 无法反选全部。[#3618](https://github.com/ant-design-blazor/ant-design-blazor/pull/3618) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 当列类型是 `Char` 时会抛异常。[#3617](https://github.com/ant-design-blazor/ant-design-blazor/pull/3617) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Modal url 变化的时候 modal 实例未完全清理的问题。[#3630](https://github.com/ant-design-blazor/ant-design-blazor/pull/3630) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复 Select 的 Placholder 在选中后仍然显示。[#3628](https://github.com/ant-design-blazor/ant-design-blazor/pull/3628) [@ElderJames](https://github.com/ElderJames)

### 0.17.2

`2024-01-07`

- 🐞 修复 Menu 对 InlineCollapsed 属性的判断。[#3614](https://github.com/ant-design-blazor/ant-design-blazor/pull/3614) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Table 在远程加载模式重复增加选中行。[#3611](https://github.com/ant-design-blazor/ant-design-blazor/pull/3611) [@ElderJames](https://github.com/ElderJames)
- 🆕 增加 JS 启动器选项。[#3610](https://github.com/ant-design-blazor/ant-design-blazor/pull/3610) [@ElderJames](https://github.com/ElderJames)

### 0.17.1

`2023-12-27`

- 🐞 修复 Table 避免 key 重复异常。[#3594](https://github.com/ant-design-blazor/ant-design-blazor/pull/3594) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Select 避免非可空的 ITem 默认值为 0 导致默认选中0。[#3595](https://github.com/ant-design-blazor/ant-design-blazor/pull/3595) [@ElderJames](https://github.com/ElderJames)
- 💄 优化 脚本/样式导入支持指定位置。[#3596](https://github.com/ant-design-blazor/ant-design-blazor/pull/3596) [@ElderJames](https://github.com/ElderJames)

### 0.17.0

`2023-12-25`

- 🔥 新增 Watermark 水印组件 。[#3441](https://github.com/ant-design-blazor/ant-design-blazor/pull/3441) [@ElderJames](https://github.com/ElderJames)
- 🔥 新增 Flex 组件。[#3547](https://github.com/ant-design-blazor/ant-design-blazor/pull/3547) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Mentions 的动态异步加载。[#3503](https://github.com/ant-design-blazor/ant-design-blazor/pull/3503) [@kooliokey](https://github.com/kooliokey)
- 🆕 新增 Radio 在使用options时 RadioGroup 支持 RadioButton 样式。[#3589](https://github.com/ant-design-blazor/ant-design-blazor/pull/3589) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Menu 展开动画。[#3395](https://github.com/ant-design-blazor/ant-design-blazor/pull/3395) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Collapse 展开动画。[#3562](https://github.com/ant-design-blazor/ant-design-blazor/pull/3562) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 JS initializers，自动加载组件库的 js 和 css。[#3557](https://github.com/ant-design-blazor/ant-design-blazor/pull/3557) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Upload 选择没有扩展名的文件时出现的异常。[#3554](https://github.com/ant-design-blazor/ant-design-blazor/pull/3554) [@SapientGuardian](https://github.com/SapientGuardian)
- 🐞 修复 Tree 应在查询值清空时展开所有节点。[#3587](https://github.com/ant-design-blazor/ant-design-blazor/pull/3587) [@ElderJames](https://github.com/ElderJames)

- Select
  - 🆕 新增 LabelProperty、ValueProperty、DisabledPredice 属性，以支持表达式。[#3569](https://github.com/ant-design-blazor/ant-design-blazor/pull/3569) [@MarvelTiter](https://github.com/MarvelTiter)
  - 🐞 修复 查询时避免在输入法选择状态绑定。[#3583](https://github.com/ant-design-blazor/ant-design-blazor/pull/3583) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 确保 click 事件处理完毕。[#3525](https://github.com/ant-design-blazor/ant-design-blazor/pull/3525) [@zxyao145](https://github.com/zxyao145)

- Table
  - 🆕 新增 ExpandAll 和 CollapseAll 方法.。[#3491](https://github.com/ant-design-blazor/ant-design-blazor/pull/3491) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Table 在某些情况下翻页数据不刷新。[#3586](https://github.com/ant-design-blazor/ant-design-blazor/pull/3586) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 筛选器输入框的 autofocus 引发 JS 异常。[#3543](https://github.com/ant-design-blazor/ant-design-blazor/pull/3543) [@m-khrapunov](https://github.com/m-khrapunov)
  - 🐞 修复 从外部翻页未清除选中项。[#3577](https://github.com/ant-design-blazor/ant-design-blazor/pull/3577) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 当选中行不在当前页时无法清除。[#3566](https://github.com/ant-design-blazor/ant-design-blazor/pull/3566) [@ElderJames](https://github.com/ElderJames)

- Input
  - 🐞 修复 在禁用状态时应隐藏清除按钮。[#3585](https://github.com/ant-design-blazor/ant-design-blazor/pull/3585) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 TextArea rows 属性失效。[#3561](https://github.com/ant-design-blazor/ant-design-blazor/pull/3561) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Textarea component 的禁用样式。[#3538](https://github.com/ant-design-blazor/ant-design-blazor/pull/3538) [@zuevus](https://github.com/zuevus)

- Tabs
  - 🆕 新增 ReuseTabs 支持拆分标签和页面，并支持页面刷新。[#3467](https://github.com/ant-design-blazor/ant-design-blazor/pull/3467) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Reusetabs 支持 WebApp 自动交互模式。[#3564](https://github.com/ant-design-blazor/ant-design-blazor/pull/3564) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 ReuseTabs 应默认打开第一个固定的页面。[#3519](https://github.com/ant-design-blazor/ant-design-blazor/pull/3519) [@ElderJames](https://github.com/ElderJames)


### 0.16.3

`2023-12-04`

- Table
  - 🛠 重构 Table 将一些内部组件改为渲染片段。[#3545](https://github.com/ant-design-blazor/ant-design-blazor/pull/3545) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Table 在客户端模式翻页不清除已选行的问题。[#3546](https://github.com/ant-design-blazor/ant-design-blazor/pull/3546) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Table 在使用RowKey后不会立即刷新单元格的问题。[#3544](https://github.com/ant-design-blazor/ant-design-blazor/pull/3544) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Select 当默认绑定值不在DataSource中时，应清空选项。[#3529](https://github.com/ant-design-blazor/ant-design-blazor/pull/3529) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tree 勾选、选中、展开状态的双向绑定。[#3520](https://github.com/ant-design-blazor/ant-design-blazor/pull/3520) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 核心 枚举从 DisplayAttribute 获取名称支持本地化。 [#3536](https://github.com/ant-design-blazor/ant-design-blazor/pull/3536) [@ElderJames](https://github.com/ElderJames)
- 💄 修复 Radio 静态渲染时的选中样式。[#3532](https://github.com/ant-design-blazor/ant-design-blazor/pull/3532) [@ElderJames](https://github.com/ElderJames)
- 💄 修复 Checkbox 静态渲染时的选中样式。[#3535](https://github.com/ant-design-blazor/ant-design-blazor/pull/3535) [@ElderJames](https://github.com/ElderJames)


### 0.16.2

`2023-11-17`

- 🔥 更新 .NET 8 依赖库。[#3514](https://github.com/ant-design-blazor/ant-design-blazor/pull/3514) [@ElderJames](https://github.com/ElderJames)

- Table
  - 🆕 新增 Table 行分组（实验性）。[#3487](https://github.com/ant-design-blazor/ant-design-blazor/pull/3487) [@ElderJames](https://github.com/ElderJames)
  - 🛠 重构 Table 把 RenderMode t重命名为 RerenderStrategy。[#3515](https://github.com/ant-design-blazor/ant-design-blazor/pull/3515) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 行选择的多个问题。[#3502](https://github.com/ant-design-blazor/ant-design-blazor/pull/3502) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Table 设置 RowKey 用于行缓存的数据比较器。[#3483](https://github.com/ant-design-blazor/ant-design-blazor/pull/3483) [@ElderJames](https://github.com/ElderJames)
  - 📖 修复 文档中路由翻页的demo。[#3507](https://github.com/ant-design-blazor/ant-design-blazor/pull/3507) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Select 在被封装并在设置了 ValidateOnChange 的Form中时，已选项会被重置的问题。[#3508](https://github.com/ant-design-blazor/ant-design-blazor/pull/3508) [@ldsenow](https://github.com/ldsenow)
- 🐞 修复 TimePicker 设置了 TiemOnly 类型值时，修改时间时的 ArgumentOutOfRangeException 异常。[#3501](https://github.com/ant-design-blazor/ant-design-blazor/pull/3501) [@Alexbits](https://github.com/Alexbits)
- 🐞 修复 TreeSelect 当数据源变更时恢复绑定项。[#3492](https://github.com/ant-design-blazor/ant-design-blazor/pull/3492) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 RangePicker 焦点样式不清除和Tab/Enter按键会清除选中值的问题 。[#3488](https://github.com/ant-design-blazor/ant-design-blazor/pull/3488) [@Alexbits](https://github.com/Alexbits)

#### 破环性更新

由于 Table 的 `RowSelectable` 属性跟 `Selection.Disabled`功能重复，并且又没有禁用样式，所以在这个版本中去除。请使用 Disabled 来实现行选择禁用。

```diff
    <Table @ref="table" DataSource="@data" @bind-SelectedRows="selectedRows" RowKey="x=>x.Name">
+        <Selection Key="@context.Name" Type="@selectionType" Disabled="@(context.Name == "Disabled User")" />
        <PropertyColumn Property="c=>c.Name">
            <a>@context.Name</a>
        </PropertyColumn>
        <PropertyColumn Property="c=>c.Age" />
        <PropertyColumn Property="c=>c.Address" />
    </Table>
```

### 0.16.1

`2023-10-30`

- Table
  - 🆕 新增 支持抽象类数据源。[#3475](https://github.com/ant-design-blazor/ant-design-blazor/pull/3475) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 绑定 SelecetdRows 强制要求设置 Selection 列的问题。[#3465](https://github.com/ant-design-blazor/ant-design-blazor/pull/3465) [@ElderJames](https://github.com/ElderJames)

- 🆕 新增 modal Visible 属性支持双向绑定。[#3466](https://github.com/ant-design-blazor/ant-design-blazor/pull/3466) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Input 验证信息显示问题。[#3474](https://github.com/ant-design-blazor/ant-design-blazor/pull/3474) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Layout 当 CollapsedWidth  等于0时，Sider 的 NoTrigger 不生效。[#3476](https://github.com/ant-design-blazor/ant-design-blazor/pull/3476) [@ElderJames](https://github.com/ElderJames)

### 0.16.0

`2023-10-24`

1024 LoL

- Table
  - 🆕 新增 自定义字段类型筛选器，支持内置筛选器的操作类型配置。[#3279](https://github.com/ant-design-blazor/ant-design-blazor/pull/3279) [@rhodon-jargon](https://github.com/rhodon-jargon)
  - 🆕 新增 列宽拖动。[#3340](https://github.com/ant-design-blazor/ant-design-blazor/pull/3340) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 FilterTemplate 列属性来自定义筛选面板。[#3285](https://github.com/ant-design-blazor/ant-design-blazor/pull/3285) [@manuelelucchi](https://github.com/manuelelucchi)
  - 🆕 新增 使列筛选面板打开后输入框获得焦点。[#3450](https://github.com/ant-design-blazor/ant-design-blazor/pull/3450) [@m-khrapunov](https://github.com/m-khrapunov)
  - 🆕 新增 RowKey 属性用于指定行数据的对比依据值。[#3439](https://github.com/ant-design-blazor/ant-design-blazor/pull/3439) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复当启用虚拟化时，'radio' 类型的选择列选中后不能反选其他行的问题。[#3282](https://github.com/ant-design-blazor/ant-design-blazor/pull/3282) [@rhodon-jargon](https://github.com/rhodon-jargon)
  - 🐞 修复 选择列禁用时，还能被"全选"和代码选中。[#3436](https://github.com/ant-design-blazor/ant-design-blazor/pull/3436) [@ElderJames](https://github.com/ElderJames)

- Datepicker
  - 🆕 增加 输入格式掩码设置。[#3120](https://github.com/ant-design-blazor/ant-design-blazor/pull/3120) [@agolub-s](https://github.com/agolub-s)
  - 🆕 新增 支持指定打开方向。[#3345](https://github.com/ant-design-blazor/ant-design-blazor/pull/3345) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加 泛型参数 DateTimeOffset, DateOnly, TimeOnly 的支持。[#3443](https://github.com/ant-design-blazor/ant-design-blazor/pull/3443) [@Alexbits](https://github.com/Alexbits)
  - 🐞 修复 失去焦点后 提交更改。修复单击清理图标无效问题。[#3087](https://github.com/ant-design-blazor/ant-design-blazor/pull/3087) [@agolub-s](https://github.com/agolub-s)
  - 🐞 修复 RangePicker 在 Form 中重置默认值无效的问题。[#3458](https://github.com/ant-design-blazor/ant-design-blazor/pull/3458) [@LeaFrock](https://github.com/LeaFrock)

- ReuseTabs
  - 🆕 新增 支持指定 PinUrl 来固定打开带参数的路由。[#3363](https://github.com/ant-design-blazor/ant-design-blazor/pull/3363) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加 Order 指定特殊标签的顺序。[#3335](https://github.com/ant-design-blazor/ant-design-blazor/pull/3335) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加 ReuseTabs KeepAlive属性，表示是否保持 Tab 页面的状态。[#3334](https://github.com/ant-design-blazor/ant-design-blazor/pull/3334) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 重新加载 页面支持。[#3396](https://github.com/ant-design-blazor/ant-design-blazor/pull/3396) [@ElderJames](https://github.com/ElderJames)
  - 🗑 移除 AuthorizeReuseTabsRouteView 组件和 Nuget 包。[#3437](https://github.com/ant-design-blazor/ant-design-blazor/pull/3437) [@ElderJames](https://github.com/ElderJames)

- Select
  - 🆕 增加 ListboxStyle 属性来控制列表的样式。[#3288](https://github.com/ant-design-blazor/ant-design-blazor/pull/3288) [@dessli](https://github.com/dessli)
  - 🐞 修复了当模式为多个时在“选择内容”中显示箭头的问题。[#3430](https://github.com/ant-design-blazor/ant-design-blazor/pull/3430) [@agolub-s](https://github.com/agolub-s)

- 🆕 新增 Form 中 FormItem 的 Label 读取 'DisplayAttribute.GetName()' 以支持 Resx。[#3426](https://github.com/ant-design-blazor/ant-design-blazor/pull/3426) [@huhangfei](https://github.com/huhangfei)
- 🆕 新增 预览图片支持拖拽移动和滚动缩放。[#3394](https://github.com/ant-design-blazor/ant-design-blazor/pull/3394) [@llp1520](https://github.com/llp1520)
- 🆕 新增 InuptNumber 框添加MaxLength属性。[#3455](https://github.com/ant-design-blazor/ant-design-blazor/pull/3455) [@chazikaifa](https://github.com/chazikaifa)
- 🆕 新增 Drawer 属性 VisibleChanged 以支持双向绑定。[#3333](https://github.com/ant-design-blazor/ant-design-blazor/pull/3333) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Tabs 支持方向键导航和回车键切换。[#3320](https://github.com/ant-design-blazor/ant-design-blazor/pull/3320) [@bweissronin](https://github.com/bweissronin)
- 🆕 新增 Modal 组件新增 resizable 参数，允许水平方向对modal进行宽度调整。修复 modal 在组件用法中 id 和 class 参数不生效的问题。[#3311](https://github.com/ant-design-blazor/ant-design-blazor/pull/3311) [@zxyao145](https://github.com/zxyao145)
- 🆕 新增 Statistic 的 CultureInfo 属性来支持不同地区的数字格式。[#3299](https://github.com/ant-design-blazor/ant-design-blazor/pull/3299) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Collapse 展开动画。[#3389](https://github.com/ant-design-blazor/ant-design-blazor/pull/3389) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Tree 展开全部 和 收起全部 树节点的方法。[#3336](https://github.com/ant-design-blazor/ant-design-blazor/pull/3336) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Input 在输入法候选时失去焦点不能马上绑定值。[#3462](https://github.com/ant-design-blazor/ant-design-blazor/pull/3462) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Button 点击动画在 wasm 多线程的出现的异常。[#3451](https://github.com/ant-design-blazor/ant-design-blazor/pull/3451) [@petertorocsik](https://github.com/petertorocsik)

#### 破环性更新：

- RangePicker 的 `OnChange` 事件参数从 `DateRangeChangedEventArgs` 改为 `DateRangeChangedEventArgs<TValue>`, 其中的 `Dates` 类型改为 `TValue`。

### 0.15.5

`2023-09-10`

教师节快乐！

- Table
  - 🐞 修复 避免禁用了的行仍然可以被"全选"选中。[#3419](https://github.com/ant-design-blazor/ant-design-blazor/pull/3419) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 在排序和筛选被修改时，重置页码。[#3397](https://github.com/ant-design-blazor/ant-design-blazor/pull/3397) [@ElderJames](https://github.com/ElderJames)
  - 📖 更新 文档介绍列固定时的自定义行样式。[#3409](https://github.com/ant-design-blazor/ant-design-blazor/pull/3409) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Input Resizable TextArea空引用错误。[#3382](https://github.com/ant-design-blazor/ant-design-blazor/pull/3382) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Modal 居中和最大化的样式冲突。[#3403](https://github.com/ant-design-blazor/ant-design-blazor/pull/3403) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复 AutoComplete 下拉框的自动宽度调整。[#3402](https://github.com/ant-design-blazor/ant-design-blazor/pull/3402) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Notification 通知组件使用异步时异常。[#3400](https://github.com/ant-design-blazor/ant-design-blazor/pull/3400) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复 Progress 的 Line 类型进度条文本换行问题。[#3387](https://github.com/ant-design-blazor/ant-design-blazor/pull/3387) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Button 在 Loading 时禁止触发 `OnClick` 事件。[#3414](https://github.com/ant-design-blazor/ant-design-blazor/pull/3414) [@ElderJames](https://github.com/ElderJames)

- 可访问性增强
  - ⌨️ 增加 input 元素的 required 属性。[#3383](https://github.com/ant-design-blazor/ant-design-blazor/pull/3383) [@eizzn](https://github.com/eizzn)
  - ⌨️ 可访问性增强，在验证失败时候增加 input 元素的 aria-invalid 属性。[#3378](https://github.com/ant-design-blazor/ant-design-blazor/pull/3378) [@eizzn](https://github.com/eizzn)
  - ⌨️ 增加 Select 选项 aria-label 属性。[#3385](https://github.com/ant-design-blazor/ant-design-blazor/pull/3385) [@eizzn](https://github.com/eizzn)

- 🌐 修复 Confim 和 Form 的韩文。[#3415](https://github.com/ant-design-blazor/ant-design-blazor/pull/3415) [@Jeongyong-park](https://github.com/Jeongyong-park)


### 0.15.4

`2023-07-31`

- 🆕 新增 Select 的 AutoFocus 属性，自动获取焦点。[#3375](https://github.com/ant-design-blazor/ant-design-blazor/pull/3375) [@LuukGlorie](https://github.com/LuukGlorie)
- 🐞 修复 Tree 设置了 CheckStrictly 后不触发 CheckedKeys 更新 。[#3379](https://github.com/ant-design-blazor/ant-design-blazor/pull/3379) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Checkbox 不能从 CheckboxGroup 传递 Disabled 值给模板中的子项。[#3365](https://github.com/ant-design-blazor/ant-design-blazor/pull/3365) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 DomEventListener 回调共享订阅时未检查 key 不存在。[#3364](https://github.com/ant-design-blazor/ant-design-blazor/pull/3364) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 TextArea 显示图标的 HTML 结构。[#3367](https://github.com/ant-design-blazor/ant-design-blazor/pull/3367) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Menu 缺失的 NULL 检查。[#3368](https://github.com/ant-design-blazor/ant-design-blazor/pull/3368) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Form 的 Help 消息文本更新。[#3373](https://github.com/ant-design-blazor/ant-design-blazor/pull/3373) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 ReuseTabs 当设置了Base Url 时，访问加载不了页面。[#3362](https://github.com/ant-design-blazor/ant-design-blazor/pull/3362) [@ElderJames](https://github.com/ElderJames)
- ⌨️ 增强 Icon role 属性的可访问性。[#3370](https://github.com/ant-design-blazor/ant-design-blazor/pull/3370) [@eizzn](https://github.com/eizzn)

### 0.15.3

`2023-07-13`

- 🐞 修复 Tree 多选模式未使用 Ctrl 键。[#3350](https://github.com/ant-design-blazor/ant-design-blazor/pull/3350) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- 🐞 修复 Icons 与Bootstrap 样式冲突。[#3348](https://github.com/ant-design-blazor/ant-design-blazor/pull/3348) [@ElderJames](https://github.com/ElderJames)
- 💄 修复 Steps 的 RTL 样式。[#3343](https://github.com/ant-design-blazor/ant-design-blazor/pull/3343) [@ElderJames](https://github.com/ElderJames)
- 🌐 修复 俄语 dateFormat 和 dateTimeFormat 格式。[#3342](https://github.com/ant-design-blazor/ant-design-blazor/pull/3342) [@Life-is-Peachy](https://github.com/Life-is-Peachy)
- 📖 文档 优化demo渲染策略[#3347](https://github.com/ant-design-blazor/ant-design-blazor/pull/3347) [@ElderJames](https://github.com/ElderJames)

### 0.15.2

`2023-07-03`

- Table
  - 🐞 避免 DisposeAsync 中的异常。[#3337](https://github.com/ant-design-blazor/ant-design-blazor/pull/3337) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Table 在 ParametersHashCodeChanged 模式下某些情况不渲染的问题。[#3313](https://github.com/ant-design-blazor/ant-design-blazor/pull/3313) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Menu 在收起时 MenuItem 切换又是无法取消选中的问题。[#3338](https://github.com/ant-design-blazor/ant-design-blazor/pull/3338) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Drawer 滚动条在页面路由发生变化时没有重置。[#3316](https://github.com/ant-design-blazor/ant-design-blazor/pull/3316) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复 Slider 在拖动释放时不触发OnAfterChange。[#3323](https://github.com/ant-design-blazor/ant-design-blazor/pull/3323) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复  Statistic  的 CountDown 在切换其他页面时会暂停的问题。[#3329](https://github.com/ant-design-blazor/ant-design-blazor/pull/3329) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Card 加载效果失效。[#3319](https://github.com/ant-design-blazor/ant-design-blazor/pull/3319) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 TreeSelect 在移除最后一个选项时没有更新绑定的属性。[#3314](https://github.com/ant-design-blazor/ant-design-blazor/pull/3314) [@ElderJames](https://github.com/ElderJames)
- 🌐 i18n: 俄语本地化文件 dateFormat 和 dateTimeFormat 改为 d.m.yyyy。[#3327](https://github.com/ant-design-blazor/ant-design-blazor/pull/3327) [@Life-is-Peachy](https://github.com/Life-is-Peachy)

### 0.15.1

`2023-06-18`

父亲节快乐！

- Table
  - 🆕 增加 支持接口类型的 DataSource 子项。[#3297](https://github.com/ant-design-blazor/ant-design-blazor/pull/3297) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 在列定义设置了 Fixed 和 Ellipsis 后，文本超长会超出单元格的问题。[#3291](https://github.com/ant-design-blazor/ant-design-blazor/pull/3291) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 空状态团在预渲染时定位不正确的问题，并进少了不必要的 ResizeObserver 订阅。[#3281](https://github.com/ant-design-blazor/ant-design-blazor/pull/3281) [@ElderJames](https://github.com/ElderJames)

- Tree
  - 🐞 修复 在使用 SearchExpression 匹配关键字后会抛出异常的问题。[#3274](https://github.com/ant-design-blazor/ant-design-blazor/pull/3274) [@ruyisee](https://github.com/ruyisee)
  - 🐞 修复 拖拽节点不能修改 DataSource 的问题。[#3275](https://github.com/ant-design-blazor/ant-design-blazor/pull/3275) [@Jtfk](https://github.com/Jtfk)

- 🐞 修复 DatePicker 的 RangePicker 在关闭时会触发两次 OnOpenChange。[#3307](https://github.com/ant-design-blazor/ant-design-blazor/pull/3307) [@Alexbits](https://github.com/Alexbits)
- 🐞 修复 Tabs 的 Reusetabs 当有固定标签时，再次导航改标签会重复的问题。[#3306](https://github.com/ant-design-blazor/ant-design-blazor/pull/3306) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Radio 当绑定值变成不在选项中的值后会无限循环的问题。[#3287](https://github.com/ant-design-blazor/ant-design-blazor/pull/3287) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 DomEventListener 移除不再订阅共享事件的事件列表，避免再次订阅时没创新事件监听器。[#3278](https://github.com/ant-design-blazor/ant-design-blazor/pull/3278) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Select 在 `SelectBase.Values` 被设置为null时， `EditContext.NotifyFieldChanged`  调用不正常的问题。[#3277](https://github.com/ant-design-blazor/ant-design-blazor/pull/3277) [@rhodon-jargon](https://github.com/rhodon-jargon)
- 📖 更新 Statistic 文档，增加自定义数字分组和小数点符号的示例。[#3166](https://github.com/ant-design-blazor/ant-design-blazor/pull/3166) [@Alerinos](https://github.com/Alerinos)


### 0.15.0

`2023-05-21`

小满

- Table
  - 🆕 增加虚拟化的 EF Core 支持。[#3270](https://github.com/ant-design-blazor/ant-design-blazor/pull/3270) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加虚拟化的 ItemsProvider 支持按需请求数据。[#3262](https://github.com/ant-design-blazor/ant-design-blazor/pull/3262) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Table 不打印异常并忽略 JSDisconnectedException。[#3216](https://github.com/ant-design-blazor/ant-design-blazor/pull/3216) [@LuukGlorie](https://github.com/LuukGlorie)
  - 🐞 修复 Table 的 flags enum 类型列的 filter 展示位置错误。[#3168](https://github.com/ant-design-blazor/ant-design-blazor/pull/3168) [@ElderJames](https://github.com/ElderJames)

- Layout
  - 🆕 增加 Sider 新属性 DefaultCollapsed，默认收缩。[#3260](https://github.com/ant-design-blazor/ant-design-blazor/pull/3260) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 在 Sider 初始是收缩时，菜单没有跟着收缩的问题。[#3268](https://github.com/ant-design-blazor/ant-design-blazor/pull/3268) [@ElderJames](https://github.com/ElderJames)

- Tree
  - 🆕 增加 Tree 的 HideUnmatched 属性,，用于隐藏没匹配 SearchValue 的节点。[#3242](https://github.com/ant-design-blazor/ant-design-blazor/pull/3242) [@rhodon-jargon](https://github.com/rhodon-jargon)
  - 🆕 增加 Tree 的公开方法 GetNode(string key)。[#3243](https://github.com/ant-design-blazor/ant-design-blazor/pull/3243) [@AndrewKaninchen](https://github.com/AndrewKaninchen)

- TreeSelect
  - 🆕 增加 新属性 OnSearch 和 OnNodeLoadDelayAsync 来支持动态加载。[#3240](https://github.com/ant-design-blazor/ant-design-blazor/pull/3240) [@rhodon-jargon](https://github.com/rhodon-jargon)
  - 🆕 增加 新属性 TreeAttributes 来传递属性给内部的 Tree 组件。[#3234](https://github.com/ant-design-blazor/ant-design-blazor/pull/3234) [@rhodon-jargon](https://github.com/rhodon-jargon)

- Select
  - 🆕 增加 accesskey 属性支持快捷键。[#3228](https://github.com/ant-design-blazor/ant-design-blazor/pull/3228) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 在开启虚拟化时空状态显示不正常。[#3171](https://github.com/ant-design-blazor/ant-design-blazor/pull/3171) [@ElderJames](https://github.com/ElderJames)

- Tabs
  - 🐞 修复 Animated 属性导致显示不正常。[#3177](https://github.com/ant-design-blazor/ant-design-blazor/pull/3177) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 ReuseTabs 计算 Tab Key 的逻辑。[#3153](https://github.com/ant-design-blazor/ant-design-blazor/pull/3153) [@berkerdong](https://github.com/berkerdong)

- Datepicker
  - 🐞 修复 某些语言下 WeekPicker 的计算问题。[#3214](https://github.com/ant-design-blazor/ant-design-blazor/pull/3214) [@sebastian-wachsmuth](https://github.com/sebastian-wachsmuth)
  - 🐞 修复 计算日期时，有时日期超出月天数的问题。[#3193](https://github.com/ant-design-blazor/ant-design-blazor/pull/3193) [@Alexbits](https://github.com/Alexbits)

- 🆕 增加  Typography 可编辑文本支持。[#3173](https://github.com/ant-design-blazor/ant-design-blazor/pull/3173) [@ElderJames](https://github.com/ElderJames)
- 🆕 增加 Mentions 自定义输入组件，添加 TextArea 支持。[#3178](https://github.com/ant-design-blazor/ant-design-blazor/pull/3178) [@wss-kroche](https://github.com/wss-kroche)
- 🆕 新增 Menu 的 `ShowCollapsedTooltip` 属性来控制 Tooltip 的显示。[#3226](https://github.com/ant-design-blazor/ant-design-blazor/pull/3226) [@ElderJames](https://github.com/ElderJames)
- 🆕 增加 Descriptions 的 `LabelStyle` 和 `ContentStyle` 来自定义 DescriptionItem 的样式。[#3186](https://github.com/ant-design-blazor/ant-design-blazor/pull/3186) [@ElderJames](https://github.com/ElderJames)
- 🛠 增加 InputNumber 中 input 元素的 `id` 属性。[#3198](https://github.com/ant-design-blazor/ant-design-blazor/pull/3198) [@varbedi](https://github.com/varbedi)
- 🛠 重构 Form 为 input component base 基类中公开FormItem 的一些属性。[#3227](https://github.com/ant-design-blazor/ant-design-blazor/pull/3227) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Dropdown 点击已选择的 MenuItem 后，不自动关闭的问题。[#3231](https://github.com/ant-design-blazor/ant-design-blazor/pull/3231) [@huangjia2107](https://github.com/huangjia2107)
- 🐞 修复 Input 正确读取空字符串或空格。[#3190](https://github.com/ant-design-blazor/ant-design-blazor/pull/3190) [@berkerdong](https://github.com/berkerdong)
- 🐞 修复 Image 预览操作按钮被预览图片覆盖。[#3170](https://github.com/ant-design-blazor/ant-design-blazor/pull/3170) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Checkbox 的 CheckboxGroup 在内部有 Checkbox 是 null 时引发异常的问题。[#3162](https://github.com/ant-design-blazor/ant-design-blazor/pull/3162) [@berkerdong](https://github.com/berkerdong)
- 🐞 修复 Pagination 的 mini 样式。[#3266](https://github.com/ant-design-blazor/ant-design-blazor/pull/3266) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 AutoComplete 无法选中的问题 (#3252)。[7d24d09](https://github.com/ant-design-blazor/ant-design-blazor/commit/7d24d09) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Drawer 按钮会提交表单的问题，增加了 type="button" 属性避免。[#3233](https://github.com/ant-design-blazor/ant-design-blazor/pull/3233) [@trafium](https://github.com/trafium)

### 0.14.4

`2023-03-01`

- 🐞 修复 Radio 避免当绑定值不在选项中时造成的无限循环。[#3123](https://github.com/ant-design-blazor/ant-design-blazor/pull/3123) [@ElderJames](https://github.com/ElderJames)
- 🐞 允许传递 Style 和 Id。[#3144](https://github.com/ant-design-blazor/ant-design-blazor/pull/3144) [@Epictek](https://github.com/Epictek)
- 🐞 修复 Select 的 OnSelectedItemsChanged 在Form 中不触发。[#3129](https://github.com/ant-design-blazor/ant-design-blazor/pull/3129) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tree 给 oncontextmenu 事件增加 preventdefault。[#3076](https://github.com/ant-design-blazor/ant-design-blazor/pull/3076) [@AndrewKaninchen](https://github.com/AndrewKaninchen)
- 🐞 修复 Pagination 在大小没有变化时仍触发 ChangeSize，导致触发了 OnChange。[#3133](https://github.com/ant-design-blazor/ant-design-blazor/pull/3133) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Transfer 列表样式属性 ListStyle 来自定义每列的样式。[#3139](https://github.com/ant-design-blazor/ant-design-blazor/pull/3139) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tabs 当 base url 不是默认时， ReuseTabs 会引发异常。[#3142](https://github.com/ant-design-blazor/ant-design-blazor/pull/3142) [@berkerdong](https://github.com/berkerdong)
- 🐞 修复 AutoComplete 只当 Backfill 为 true 时才回填值给输入框。[#3140](https://github.com/ant-design-blazor/ant-design-blazor/pull/3140) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Datepicker 的 DisabledDate 属性没有影响到键盘输入。[#3134](https://github.com/ant-design-blazor/ant-design-blazor/pull/3134) [@Alexbits](https://github.com/Alexbits)
- 🐞 修复 Input 避免 Textarea 在渲染前调用 JS 。[#3128](https://github.com/ant-design-blazor/ant-design-blazor/pull/3128) [@ElderJames](https://github.com/ElderJames)


### 0.14.3

`2023-02-19`

- Popconfirm
  - 🐞 修复 Popconfirm 图标颜色缺失。[#3093](https://github.com/ant-design-blazor/ant-design-blazor/pull/3093) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Popconfirm 补全内置的本地化。[#3095](https://github.com/ant-design-blazor/ant-design-blazor/pull/3095) [@ElderJames](https://github.com/ElderJames)

- Pagination
  - 🐞 修复  Pagination 的 `DefaultCurrent` 参数无作用。[#3085](https://github.com/ant-design-blazor/ant-design-blazor/pull/3085) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Pagination 在RTL模式跳转按钮图标的方向。[#3084](https://github.com/ant-design-blazor/ant-design-blazor/pull/3084) [@ElderJames](https://github.com/ElderJames)

- Modal
  - 🐞 修复 每次在按键的时候重渲染content的问题。[#3099](https://github.com/ant-design-blazor/ant-design-blazor/pull/3099) [@zxyao145](https://github.com/zxyao145)
  - 🛠 重构 ConfirmService 改为注入 IConfirmService。[#3083](https://github.com/ant-design-blazor/ant-design-blazor/pull/3083) [@wss-awachowicz](https://github.com/wss-awachowicz)

- 🐞 修复 Drawer 中 popup 无法选择的问题。[#3106](https://github.com/ant-design-blazor/ant-design-blazor/pull/3106) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复 Textarea 不在 textarea 上渲染 maxlength。[#3108](https://github.com/ant-design-blazor/ant-design-blazor/pull/3108) [@wss-kroche](https://github.com/wss-kroche)
- 🐞 修复 Tabs 支持修改 ReuseTabs 标签名。[#3088](https://github.com/ant-design-blazor/ant-design-blazor/pull/3088) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Radio 在 RadioGroup 中的选项和绑定值同时被修改时，不能选中最新值的问题。[#3098](https://github.com/ant-design-blazor/ant-design-blazor/pull/3098) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Segmented 在 Labels 索引发送改变时抛出异常的问题。[#3096](https://github.com/ant-design-blazor/ant-design-blazor/pull/3096) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Table 分页器在 RTL 语言下的默认位置。[#3086](https://github.com/ant-design-blazor/ant-design-blazor/pull/3086) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Select 的 OnSelectedItemsChanged 事件不正常触发。[#3079](https://github.com/ant-design-blazor/ant-design-blazor/pull/3079) [@m-khrapunov](https://github.com/m-khrapunov)
- 🐞 修复 Menu 标题在 RTL 语言时内边距的方向。[#3080](https://github.com/ant-design-blazor/ant-design-blazor/pull/3080) [@ElderJames](https://github.com/ElderJames)

### 0.14.2

`2023-02-06`

开工大吉！

- 🐞 修复 Menu 在RTL语言中错误的子菜单样式。[#3065](https://github.com/ant-design-blazor/ant-design-blazor/pull/3065) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tabs 的 Reusetabs 在未打开过页面时会出现 null 引用异常。[#3060](https://github.com/ant-design-blazor/ant-design-blazor/pull/3060) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 drawe mask 不会消失的问题。[#3059](https://github.com/ant-design-blazor/ant-design-blazor/pull/3059) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复 Calendar 选中日期错误。[#3069](https://github.com/ant-design-blazor/ant-design-blazor/pull/3069) [@agolub-s](https://github.com/agolub-s)

### 0.14.1

`2023-02-01`

- 🐞 修复 Notification 在 RTL 下显示异常问题，新增 top 和 bottom 位置支持[#3049](https://github.com/ant-design-blazor/ant-design-blazor/pull/3049) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复 Table 隐藏分页器时，更新数据源时行数不立刻刷新[#3052](https://github.com/ant-design-blazor/ant-design-blazor/pull/3052) [@wss-javeney](https://github.com/wss-javeney)
- 🐞 修复 Tabs 保留 ReuseTabs 之前的用法。[#3051](https://github.com/ant-design-blazor/ant-design-blazor/pull/3051) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tree OnContextMenu 事件不起作用。[#3042](https://github.com/ant-design-blazor/ant-design-blazor/pull/3042) [@AndrewKaninchen](https://github.com/AndrewKaninchen)
- 🐞 修复 Select 双向绑定选项的顺序问题。[#3037](https://github.com/ant-design-blazor/ant-design-blazor/pull/3037) [@CuteLeon](https://github.com/CuteLeon)
- 🐞 修复 drawer 遮罩层不能立即关闭的问题。[#3047](https://github.com/ant-design-blazor/ant-design-blazor/pull/3047) [@zxyao145](https://github.com/zxyao145)
- 🛠 将多个冗余参数标记为已过时以供将来删除: `Calendar.OnSelect`, `Card.Body`, `Sider.OnCollapse`, `PageHeader.PageHeaderTitle`, `PageHeader.PageHeaderSubtitle`, `Radio.CheckedChange`。[#3035](https://github.com/ant-design-blazor/ant-design-blazor/pull/3035) [@kooliokey](https://github.com/kooliokey)

### 0.14.0

`2023-01-26`

新春快乐，兔年吉祥！

- Table
  - 🆕 支持根据表格的 `TItem` 类型自动生成列。[#2978](https://github.com/ant-design-blazor/ant-design-blazor/pull/2978) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加 Table 头部和行的分组。[#2973](https://github.com/ant-design-blazor/ant-design-blazor/pull/2973) [@anranruye](https://github.com/anranruye)
  - 🆕 增加 空状态模板，并使它在列滚动时固定。[#3031](https://github.com/ant-design-blazor/ant-design-blazor/pull/3031) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 选择列中的空引用异常。[#3028](https://github.com/ant-design-blazor/ant-design-blazor/pull/3028) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 列标题在可筛选时的对齐样式问题。[#3023](https://github.com/ant-design-blazor/ant-design-blazor/pull/3023) [@ElderJames](https://github.com/ElderJames)

- Input
  - 🆕 增加 ShowCount 属性显示字数。[#3033](https://github.com/ant-design-blazor/ant-design-blazor/pull/3033) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加 OnClear 事件回调。[#3020](https://github.com/ant-design-blazor/ant-design-blazor/pull/3020) [@Abin-Liu](https://github.com/Abin-Liu)

- Menu
  - 🆕 增加 `PopupClassName` 属性。[#3027](https://github.com/ant-design-blazor/ant-design-blazor/pull/3027) [@JustGentle](https://github.com/JustGentle)
  - 🐞 修复 Menu 子菜单的动画和样式。[#3024](https://github.com/ant-design-blazor/ant-design-blazor/pull/3024) [@ElderJames](https://github.com/ElderJames)

- Transfer
  - 🐞 修复 在 Form 内抛异常的问题。[#3015](https://github.com/ant-design-blazor/ant-design-blazor/pull/3015) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 无法选中的问题。[#3011](https://github.com/ant-design-blazor/ant-design-blazor/pull/3011) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 DataSource 的刷新。[#2998](https://github.com/ant-design-blazor/ant-design-blazor/pull/2998) [@ElderJames](https://github.com/ElderJames)

- InputNumber
  - 🆕 增加 无边框样式。[#3019](https://github.com/ant-design-blazor/ant-design-blazor/pull/3019) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Step 回退和空引用异常。[#3018](https://github.com/ant-design-blazor/ant-design-blazor/pull/3018) [@ElderJames](https://github.com/ElderJames)

- Tabs
  - 🛠 重构 ReuseTabs 移除 `ReuseTabsRouteView` 来降低跟原生组件的耦合，请使用 CsaCadingValue 传递 RouteData，[参考示例](https://github.com/ant-design-blazor/ant-design-blazor/blob/0.14.0/tests/AntDesign.TestApp/Client/App.razor#L4)。[#3009](https://github.com/ant-design-blazor/ant-design-blazor/pull/3009) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 ReuseTabs 关闭其他标签时的渲染错误。[#3002](https://github.com/ant-design-blazor/ant-design-blazor/pull/3002) [@berkerdong](https://github.com/berkerdong)
  - 🐞 修复 当 Activekey  指定了一个禁用的 TabPane 时，或第一个 TabPane 就是禁用时，初次加载会抛异常的问题。[#2997](https://github.com/ant-design-blazor/ant-design-blazor/pull/2997) [@ElderJames](https://github.com/ElderJames)

- 🆕 增加 ThemeService 用于主题切换。[#2883](https://github.com/ant-design-blazor/ant-design-blazor/pull/2883) [@melinyi](https://github.com/melinyi)
- 🆕 增加 Datepicker 已选星期范围的展示。[#2892](https://github.com/ant-design-blazor/ant-design-blazor/pull/2892) [@Alexbits](https://github.com/Alexbits)
- 🆕 新增 Radio 的 RadioGroup 级联类型参数。[#3022](https://github.com/ant-design-blazor/ant-design-blazor/pull/3022) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Overlay 会在鼠标离开触发元素之后还打开。[#3025](https://github.com/ant-design-blazor/ant-design-blazor/pull/3025) [@JustGentle](https://github.com/JustGentle)
- 🐞 修复 ResizeObserver 由于key类型的改动导致无效的问题。[#3030](https://github.com/ant-design-blazor/ant-design-blazor/pull/3030) [@ElderJames](https://github.com/ElderJames)
- 🐞修复 Select 在搜索或清除搜索时即使将 HideSelected 设置为 true 也会显示所选选项的错误。[#3010](https://github.com/ant-design-blazor/ant-design-blazor/pull/3010) [@wss-kroche](https://github.com/wss-kroche)
- 🐞 修复 Form 自定义校验的样式。[#3005](https://github.com/ant-design-blazor/ant-design-blazor/pull/3005) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Cascader 边界调整模式默认改为 InView。[#2999](https://github.com/ant-design-blazor/ant-design-blazor/pull/2999) [@ElderJames](https://github.com/ElderJames)
- 🐞 重构 Descriptions 删除控制台输出。[#3012](https://github.com/ant-design-blazor/ant-design-blazor/pull/3012) [@berkerdong](https://github.com/berkerdong)
- 💄 同步 ant-design v4.24.2 样式。[#2877](https://github.com/ant-design-blazor/ant-design-blazor/pull/2877) [@ElderJames](https://github.com/ElderJames)

#### 破环性更新

- Table : `RowTemplate` 改为 `ColumnDefinitions`。`RowTemplate` 原来用于 `Column` 定义，这个版本之后改为用于定义行模板。
- ReuseTabs: `ReuseTabsRouteView` 和 `AuthorizeReuseTabsRouteView` 已被标记为弃用。 请用`<CascadingValue Value="routeData">` 包裹 `<RouteView>` 或 `<AuthorizeRouteView>`。
  
  即：

  ```diff
  <Router AppAssembly="@typeof(Program).Assembly" PreferExactMatches="@true">
    <Found Context="routeData">
  +   <CascadingValue Value="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
  +   </CascadingValue>
    </Found>
    <NotFound>
        <LayoutView Layout="@typeof(MainLayout)">
            <p>Sorry, there's nothing at this address.</p>
        </LayoutView>
    </NotFound>
  </Router>
  ```

### 0.13.3

`2023-01-09`

- Select
  - 🐞 修复 标签模式在数据源为空时不保留选中项的问题。[#2986](https://github.com/ant-design-blazor/ant-design-blazor/pull/2986) [@wss-javeney](https://github.com/wss-javeney)
  - 🐞 修复 Select 下拉列表的边界调整模式为InView。[#2995](https://github.com/ant-design-blazor/ant-design-blazor/pull/2995) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Select 的搜索框字符被隐藏的问题。[#2994](https://github.com/ant-design-blazor/ant-design-blazor/pull/2994) [@ElderJames](https://github.com/ElderJames)

- AutoComplete
  - 🐞 修复 下拉列表会在页面刷新时自动打开。[#2992](https://github.com/ant-design-blazor/ant-design-blazor/pull/2992) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 输入框的拼写会话和防抖。[#2988](https://github.com/ant-design-blazor/ant-design-blazor/pull/2988) [@ElderJames](https://github.com/ElderJames)

- Tabs
  - 🐞 修复 当第一个 TabPane 设置了 Disabled 后，首次渲染异常的问题。[#2982](https://github.com/ant-design-blazor/ant-design-blazor/pull/2982) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 关闭异常，阻止在 disposed 后还触发渲染器。[#2981](https://github.com/ant-design-blazor/ant-design-blazor/pull/2981) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 关闭异常，移除了js 中对引用对象的 Dispose 调用。[#2980](https://github.com/ant-design-blazor/ant-design-blazor/pull/2980) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Transfer 在 SelectedKeys 或 TargetKeys 改变时刷新数据。[#2977](https://github.com/ant-design-blazor/ant-design-blazor/pull/2977) [@Magehernan](https://github.com/Magehernan)
- 🐞 修复  TreeSelect 当绑定了默认值后，不能正确修改值的问题。[#2990](https://github.com/ant-design-blazor/ant-design-blazor/pull/2990) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Input Search 组件设置了清除按钮的样式。[#2991](https://github.com/ant-design-blazor/ant-design-blazor/pull/2991) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 RangePicker 自定义禁用日期逻辑叠加默认禁用日期判断。[#2947](https://github.com/ant-design-blazor/ant-design-blazor/pull/2947) [@wss-kroche](https://github.com/wss-kroche)

### 0.13.2

`2022-12-31`

- Table
  - 📖 文档 查询和排序的 demo 中加入额外的查询框，并实现联合搜索。[#2955](https://github.com/ant-design-blazor/ant-design-blazor/pull/2955) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Selection 列 Hidden 属性不起作用的问题。[#2945](https://github.com/ant-design-blazor/ant-design-blazor/pull/2945) [@berkerdong](https://github.com/berkerdong)
  - 🐞 修复 Table 的 ActionColumn 的 Hidden 属性不起作用的问题。[#2946](https://github.com/ant-design-blazor/ant-design-blazor/pull/2946) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 当用了 AsNoTracking 的 IQueryable 作为 DataSource 时导致选中项重复的问题[#2944](https://github.com/ant-design-blazor/ant-design-blazor/pull/2944) [@berkerdong](https://github.com/berkerdong)
  - 🐞 修复 不能恢复枚举类型的 Filter 的查询状态。[#2941](https://github.com/ant-design-blazor/ant-design-blazor/pull/2941) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 当同时设置了 HidePagination 和 PageSize 且 DataSource 为空时导致无限循环。[#2919](https://github.com/ant-design-blazor/ant-design-blazor/pull/2919) [@ElderJames](https://github.com/ElderJames)

- DatePicker
  - 🆕 新增 RangePicker 的 SuffixIcon 属性以允许自定义后缀图标。[#2935](https://github.com/ant-design-blazor/ant-design-blazor/pull/2935) [@wss-javeney](https://github.com/wss-javeney)
  - 🐞 修复 DatePicker 在启用时间选择且 Value 为 null 时，点击 input 框出现异常的问题[#2920](https://github.com/ant-design-blazor/ant-design-blazor/pull/2920) [@Alexbits](https://github.com/Alexbits)

- Input
  - 🐞 修复 `OnChange` 事件会被触发三次，以及清除按钮不能跟 `Suffix` 同时显示的问题。[#2970](https://github.com/ant-design-blazor/ant-design-blazor/pull/2970) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复在 Dispose 时的偶尔的 null 引用异常.[#2966](https://github.com/ant-design-blazor/ant-design-blazor/pull/2966) [@dracan](https://github.com/dracan)


- 🆕 新增 TreeSelect 支持给节点设置 TitleTemplate[#2940](https://github.com/ant-design-blazor/ant-design-blazor/pull/2940) [@rhodon-jargon](https://github.com/rhodon-jargon)
- 🆕 新增 Form 的 RequiredMark 属性以允许在必填、可选或无的字段旁边显示指示符。[#2930](https://github.com/ant-design-blazor/ant-design-blazor/pull/2930) [@wss-kroche](https://github.com/wss-kroche)

- 🐞 修复 Tabs  的一些关于动态渲染的问题。[#2967](https://github.com/ant-design-blazor/ant-design-blazor/pull/2967) [@ElderJames](https://github.com/ElderJames)

- 🛠 重构 Notification 的 `NotificationService` 增加了 `INotificationService` 接口。[#2948](https://github.com/ant-design-blazor/ant-design-blazor/pull/2948) [@wss-javeney](https://github.com/wss-javeney)
- 🐞 修复 InputNumber 在某些场景下不停触发 递增/递减 的问题。[#2953](https://github.com/ant-design-blazor/ant-design-blazor/pull/2953) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复  Statistic 的 CountDown 再裁剪发布时格式化无效的问题。[#2943](https://github.com/ant-design-blazor/ant-design-blazor/pull/2943) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 ClassMapper 的 css classes 出现两次。[#2934](https://github.com/ant-design-blazor/ant-design-blazor/pull/2934) [@berkerdong](https://github.com/berkerdong)
- 🐞 修复 System.Text.Json 旧版本的bug，给 netstandard2.1 目标内置7.0版本[#2922](https://github.com/ant-design-blazor/ant-design-blazor/pull/2922) [@ElderJames](https://github.com/ElderJames)


### 0.13.1

`2022-11-29`

- 🐞 修复 Input 在被从代码修改值时，再点击输入框会回退的问题。[#2906](https://github.com/ant-design-blazor/ant-design-blazor/pull/2906) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Table 同时设置 HidePagination 和PageSize 时导致的无限循环。[#2905](https://github.com/ant-design-blazor/ant-design-blazor/pull/2905) [@ElderJames](https://github.com/ElderJames)

### 0.13.0

`2022-11-22`

- 🔥 新增 .NET 7 目标框架支持。[#2810](https://github.com/ant-design-blazor/ant-design-blazor/pull/2810) [@ElderJames](https://github.com/ElderJames)
- 🔥 重构 Mentions 组件，修复定位和隐藏问题。[#2874](https://github.com/ant-design-blazor/ant-design-blazor/pull/2874) [@dingyanwu](https://github.com/dingyanwu)

- DatePicker
  - 🆕 新增 OnOk 事件。[#2840](https://github.com/ant-design-blazor/ant-design-blazor/pull/2840) [@Alexbits](https://github.com/Alexbits)
  - 🐞 修复 星期选择模式中跨年选择星期范围时的禁用星期判断问题。[#2889](https://github.com/ant-design-blazor/ant-design-blazor/pull/2889) [@Alexbits](https://github.com/Alexbits)

- Table
  - 📖 新增 OData 查询示例。[#2861](https://github.com/ant-design-blazor/ant-design-blazor/pull/2861) [@ElderJames](https://github.com/ElderJames)
  - 🆕 新增 如果设置了 `HidePagination=true`，PageSize 自动等于 DataSource 的数量。[#2476](https://github.com/ant-design-blazor/ant-design-blazor/pull/2476) [@CareyYang](https://github.com/CareyYang)

- Modal
  - 🆕 新增 内置仅有一个 OK 按钮的 Footer 和仅有一个 Cancel 按钮的 Footer。[#2812](https://github.com/ant-design-blazor/ant-design-blazor/pull/2812) [@zxyao145](https://github.com/zxyao145)
  - 🆕 新增 初始化的时候最大化的支持。[#2834](https://github.com/ant-design-blazor/ant-design-blazor/pull/2834) [@zxyao145](https://github.com/zxyao145)

- Input
  - 🆕 新增 输入绑定属性 BindOnInput，默认绑定事件改为 onchange。[#2838](https://github.com/ant-design-blazor/ant-design-blazor/pull/2838) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 输入字符串类型转换错误的信息提示问题[#2846](https://github.com/ant-design-blazor/ant-design-blazor/pull/2846) [@ElderJames](https://github.com/ElderJames)

- 🆕 新增 Tree 拖拽事件的属性 DropBelow 来标记拖拽后变为目标节点的兄弟节点或是子节点。[#2864](https://github.com/ant-design-blazor/ant-design-blazor/pull/2864) [@AndrewKaninchen](https://github.com/AndrewKaninchen)
- 🌐 修复 cs-CZ 多语言 shortWeekDays 星期排序。[#2866](https://github.com/ant-design-blazor/ant-design-blazor/pull/2866) [@ElderJames](https://github.com/ElderJames)
- 🆕 增加 Cascader 禁用 Disabled 属性。[#2835](https://github.com/ant-design-blazor/ant-design-blazor/pull/2835) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 Button 文件下载按钮 DownloadButton 组件。[#2819](https://github.com/ant-design-blazor/ant-design-blazor/pull/2819) [@LeaFrock](https://github.com/LeaFrock)
- 🆕 新增 Drawer 的头部样式属性 HeaderStyle。[#2809](https://github.com/ant-design-blazor/ant-design-blazor/pull/2809) [@danielbotn](https://github.com/danielbotn)
- 💄 新增 Dropdown 箭头样式。[#2795](https://github.com/ant-design-blazor/ant-design-blazor/pull/2795) [@ElderJames](https://github.com/ElderJames)
- 🆕 新增 InputNumber 精度属性 Precision。[#2774](https://github.com/ant-design-blazor/ant-design-blazor/pull/2774) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- 🆕 新增 Tooltip TitleTemplate属性，原 Title 属性改为 string 类型。[#2711](https://github.com/ant-design-blazor/ant-design-blazor/pull/2711) [@CAPCHIK](https://github.com/CAPCHIK)
- 🆕 新增 Select 虚拟化支持。[#2654](https://github.com/ant-design-blazor/ant-design-blazor/pull/2654) [@m-khrapunov](https://github.com/m-khrapunov)
- 🐞 修复 Menu 二级菜单的箭头展开和折叠时没有动画效果的问题。[#2876](https://github.com/ant-design-blazor/ant-design-blazor/pull/2876) [@wangj90](https://github.com/wangj90)
- 🐞 修复 Segmented 绑定值类型会导致初始化选中错误。[#2869](https://github.com/ant-design-blazor/ant-design-blazor/pull/2869) [@ElderJames](https://github.com/ElderJames)
- 📖 修复 文档生成的demo 锚点。[#2826](https://github.com/ant-design-blazor/ant-design-blazor/pull/2826) [@kooliokey](https://github.com/kooliokey)

### 0.12.7

`2022-11-6`

- DatePicker
  - 🐞 修复 部分语言文件的星期排序，缺失的语言使用 Globalization 替代。[#2855](https://github.com/ant-design-blazor/ant-design-blazor/pull/2855) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 smoothScrollTo 方法导致无限循环的问题。[#2854](https://github.com/ant-design-blazor/ant-design-blazor/pull/2854) [@Alexbits](https://github.com/Alexbits)
  - 🐞 修复 俄语 的星期排序[#2845](https://github.com/ant-design-blazor/ant-design-blazor/pull/2845) [@ocoka](https://github.com/ocoka)
  - 🐞 修复 tab 键不确认值的问题。[#2847](https://github.com/ant-design-blazor/ant-design-blazor/pull/2847) [@Alexbits](https://github.com/Alexbits)

- Core
  - ✅ 增加核心模块的单元测试覆盖。[#2821](https://github.com/ant-design-blazor/ant-design-blazor/pull/2821) [@LeaFrock](https://github.com/LeaFrock)
  - ⚡️ 优化 CssSizeLength 和CssStyleBuilder。[#2803](https://github.com/ant-design-blazor/ant-design-blazor/pull/2803) [@LeaFrock](https://github.com/LeaFrock)

- 🐞 修复 `TabBarStyle` 和 `TabBarClass`。[#2844](https://github.com/ant-design-blazor/ant-design-blazor/pull/2844) [@ldsenow](https://github.com/ldsenow)
- 🐞 修复 BackTop 在隐藏时没有真正清除 dom。[#2831](https://github.com/ant-design-blazor/ant-design-blazor/pull/2831) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Drawer 如果 `Content` 是字符串而不是 RenderFragment，则内容不会渲染的错误。[#2833](https://github.com/ant-design-blazor/ant-design-blazor/pull/2833) [@kooliokey](https://github.com/kooliokey)
- 🐞 修复 Title 属性没有渲染。[#2830](https://github.com/ant-design-blazor/ant-design-blazor/pull/2830) [@kooliokey](https://github.com/kooliokey)
- 🐞 修复 Slider 可访问性 aria labels。[#2818](https://github.com/ant-design-blazor/ant-design-blazor/pull/2818) [@kooliokey](https://github.com/kooliokey)
- 🐞 修复 Table 在页面切换时偶尔的异常[#2797](https://github.com/ant-design-blazor/ant-design-blazor/pull/2797) [@Kyojuro27](https://github.com/Kyojuro27)
- 🐞 修复 Tag 渲染后标签颜色变化不总是正确更新样式。[#2816](https://github.com/ant-design-blazor/ant-design-blazor/pull/2816) [@kooliokey](https://github.com/kooliokey)
- 🐞 修复 Cascader 当 `AllowClear` 属性为false时清除无效的问题。[#2792](https://github.com/ant-design-blazor/ant-design-blazor/pull/2792) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- 🐞 修复 AutoComplete 搜索时下拉列表有时候不显示。[#2793](https://github.com/ant-design-blazor/ant-design-blazor/pull/2793) [@lyj0309](https://github.com/lyj0309)
- 🐞 修复 Menu 子菜单展开图标的样式。[#2796](https://github.com/ant-design-blazor/ant-design-blazor/pull/2796) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Descriptions  缺少的div元素。[#2798](https://github.com/ant-design-blazor/ant-design-blazor/pull/2798) [@Weilence](https://github.com/Weilence)
- 🐞 修复 Upload 返回错误时没有正确传出响应报文。[#2858](https://github.com/ant-design-blazor/ant-design-blazor/pull/2858) [@yosheng](https://github.com/yosheng)

### 0.12.6

`2022-10-11`

- 🐞 修复 JS事件监听器注册问题。[#2783](https://github.com/ant-design-blazor/ant-design-blazor/pull/2783) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Segmented 的 Disabled 参数对项不起作用，也不能动态切换的问题。[#2778](https://github.com/ant-design-blazor/ant-design-blazor/pull/2778) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复打包时 patch.less 空文件缺失。[#2779](https://github.com/ant-design-blazor/ant-design-blazor/pull/2779) [@paulsuart](https://github.com/paulsuart)

### 0.12.5

`2022-10-09`

- Datepicker
  - 🐞 修复 手动输入时间时，CultureInfo 格式不识别导致无法绑定的问题。[#2715](https://github.com/ant-design-blazor/ant-design-blazor/pull/2715) [@Alexbits](https://github.com/Alexbits)
  - 🐞 修复一系列问题，使 Datepicker 和 RangePicker 的行为更接近 antd。[#2741](https://github.com/ant-design-blazor/ant-design-blazor/pull/2741) [@Alexbits](https://github.com/Alexbits)
    - 修复 OnChange 事件传入旧值的问题。
    - 修复 RangePicker 的头部无法切换年份。
    - 修复 RangePicker 当开始和结束都在同一周期时，选择面板的显示问题。
    - 修复 RangePicker 周选择模式时，结束日期不高亮显示的问题。
    - 修复 RangePicker 在带有时间的日期选择器中输入结束日期时，开始日期不会高亮显示的问题。
    - 其他一些小修复和重构

- Modal
  - 🐞 修复 当使用 Title 时，Maximizable 设置不生效。[#2750](https://github.com/ant-design-blazor/ant-design-blazor/pull/2750) [@zxyao145](https://github.com/zxyao145)
  - 🐞 修复 Confirm 错误的响应 close icon 事件。[#2776](https://github.com/ant-design-blazor/ant-design-blazor/pull/2776) [@zxyao145](https://github.com/zxyao145)

- 🐞 修复 底层 当组件 Dispose 时移除JS事件监听器。[#2738](https://github.com/ant-design-blazor/ant-design-blazor/pull/2738) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Radio 的 Disabled 属性在使用了 RadioOption 作为 options 的 RadioGroup 中不起作用的问题。[#2744](https://github.com/ant-design-blazor/ant-design-blazor/pull/2744) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Table 当所有selection都为Disabled=true，则头部的全选 Selection 也变为 Disable 状态。[#2737](https://github.com/ant-design-blazor/ant-design-blazor/pull/2737) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- ⚡️ 修复 Select 内部 CreateDeleteSelectOptions 方法的循环调用。[#2657](https://github.com/ant-design-blazor/ant-design-blazor/pull/2657) [@m-khrapunov](https://github.com/m-khrapunov)
- 🛠 修复 Gulp 脚本，使LESS文件打包到NUGET包中。[#2730](https://github.com/ant-design-blazor/ant-design-blazor/pull/2730) [@paulsuart](https://github.com/paulsuart)

### 0.12.4

`2022-09-14`

- 🐞 修复 Table 排序引起的异常。[#2710](https://github.com/ant-design-blazor/ant-design-blazor/pull/2710) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- 🐞 修复 AutoComplete 自动弹出浏览器的输入记忆的问题。[#2708](https://github.com/ant-design-blazor/ant-design-blazor/pull/2708) [@lyj0309](https://github.com/lyj0309)
- 🐞 修复 RangePicker 几个问题[#2707](https://github.com/ant-design-blazor/ant-design-blazor/pull/2707) [@Alexbits](https://github.com/Alexbits)：
  - 修复 RTL 模式第二个面板不弹出的问题
  - 修复 预设范围在开启了时间输入时会被重置的问题
  - 修复 停止输入后没有保持焦点的问题
  - 修复 某个输入框有焦点时不能清除值的问题

### 0.12.3

`2022-09-13`

🥮中秋节快乐！

- 🐞 修复 TreeSelect 的查询支持。[#2686](https://github.com/ant-design-blazor/ant-design-blazor/pull/2686) [@Magehernan](https://github.com/Magehernan)
- 🆕 增加 Grid 的 `GridRow` 别名，文档更新使用 `GridRow` 和 `GridCol`。[#2690](https://github.com/ant-design-blazor/ant-design-blazor/pull/2690) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- 🐞 修复 Message 在 Invariant Globalization 模式下报错问题。[#2697](https://github.com/ant-design-blazor/ant-design-blazor/pull/2697) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Segmentd 的默认值回显。[#2699](https://github.com/ant-design-blazor/ant-design-blazor/pull/2699) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Table 的多重排序在EFCore上不生效的问题。[#2701](https://github.com/ant-design-blazor/ant-design-blazor/pull/2701) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- 📖 增加 异常捕捉提示的 demo，分别位于 Alert、Result 以及 Notification。[#2706](https://github.com/ant-design-blazor/ant-design-blazor/pull/2706) [#2703](https://github.com/ant-design-blazor/ant-design-blazor/pull/2703) [@ElderJames](https://github.com/ElderJames)


### 0.12.2

`2022-09-08`

- Table
  - 🐞 修复 使用 EF Core 作为数据源时，排序报错[#2687](https://github.com/ant-design-blazor/ant-design-blazor/pull/2687) [@JamesGit-hash](https://github.com/JamesGit-hash)
  - 🐞 修复 当 ActionColumn 放在数据列左边时的报错[#2683](https://github.com/ant-design-blazor/ant-design-blazor/pull/2683) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Tabs 没有 Animated 时，TabPane 样式问题[#2677](https://github.com/ant-design-blazor/ant-design-blazor/pull/2677) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Datepicker 给 RagePicker 的 Value 赋值 null 时的异常[#2688](https://github.com/ant-design-blazor/ant-design-blazor/pull/2688) [@ElderJames](https://github.com/ElderJames)

### 0.12.1

`2022-09-04`

- Tabs
  - 🐞 修复 Tabs 切换动效内容溢出问题[#2671](https://github.com/ant-design-blazor/ant-design-blazor/pull/2671) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 ReuseTabs 的 invaild uri 异常[#2663](https://github.com/ant-design-blazor/ant-design-blazor/pull/2663) [@ElderJames](https://github.com/ElderJames)

- Icon
  - 📖 修复 Icon文档遗漏的 ZoomOut Outline 图标[#2667](https://github.com/ant-design-blazor/ant-design-blazor/pull/2667) [@kooliokey](https://github.com/kooliokey)
  - 🐞 修复 Icon 的状态更新，以及双色图标预渲染优化[#2666](https://github.com/ant-design-blazor/ant-design-blazor/pull/2666) [@ElderJames](https://github.com/ElderJames)
  
- 🐞 修复 Collapse 失效的 HTML 结构[#2668](https://github.com/ant-design-blazor/ant-design-blazor/pull/2668) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Breadcrumb 未同步HTML导致的样式问题，BreadcrumbItem增加OnClick事件[#2655](https://github.com/ant-design-blazor/ant-design-blazor/pull/2655) [@kooliokey](https://github.com/kooliokey)
- 🐞 修复 TimePicker 会重置Value 的问题和错误的按钮[#2660](https://github.com/ant-design-blazor/ant-design-blazor/pull/2660) [@Alexbits](https://github.com/Alexbits)
- 📖 修复 DatePicker 文档异常[#2659](https://github.com/ant-design-blazor/ant-design-blazor/pull/2659) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Avatar 让 Size 属性支持各种长度单位。[#2653](https://github.com/ant-design-blazor/ant-design-blazor/pull/2653) [@ElderJames](https://github.com/ElderJames)
- 📖 修复文档 无效的编辑URL[#2661](https://github.com/ant-design-blazor/ant-design-blazor/pull/2661) [@ElderJames](https://github.com/ElderJames)

### 0.12.0

`2022-08-29`

- 🔥 同步 ant-design v4.20.7 样式，支持利用 CSS 变量修改主题。[#2497](https://github.com/ant-design-blazor/ant-design-blazor/pull/2497) [@ElderJames](https://github.com/ElderJames)
- 🔥 增加 Segmented 组件，同步 antd 4.20。[#2503](https://github.com/ant-design-blazor/ant-design-blazor/pull/2503) [@ElderJames](https://github.com/ElderJames)
- 🔥 增加 Table 的 PropertyColumn，支持多层级对象的绑定。[#2624](https://github.com/ant-design-blazor/ant-design-blazor/pull/2624) [@ElderJames](https://github.com/ElderJames)
- 🆕 增加 ReuseTabs 固定标签支持。[#2545](https://github.com/ant-design-blazor/ant-design-blazor/pull/2545) [@HaoZhiYing](https://github.com/HaoZhiYing)
- 🆕 增加 PageHeader 响应式紧凑样式支持。[#2606](https://github.com/ant-design-blazor/ant-design-blazor/pull/2606) [@ElderJames](https://github.com/ElderJames)
- 🆕 增加 ResizeObserver 组件。[#2605](https://github.com/ant-design-blazor/ant-design-blazor/pull/2605) [@ElderJames](https://github.com/ElderJames)
- 🆕 增加 List 的拖拽 Demo。[#2563](https://github.com/ant-design-blazor/ant-design-blazor/pull/2563) [@charset](https://github.com/charset)
- 🆕 增加 Tooltip 的 TabIndex 属性。[#2567](https://github.com/ant-design-blazor/ant-design-blazor/pull/2567) [@lukblazewicz](https://github.com/lukblazewicz)
- 🆕 增加 Drawer 的 OnOpen 事件的支持。[#2553](https://github.com/ant-design-blazor/ant-design-blazor/pull/2553) [@zxyao145](https://github.com/zxyao145)

- Icon
  - 🔥 增加 双色图标实现。[#2513](https://github.com/ant-design-blazor/ant-design-blazor/pull/2513) [@rqx110](https://github.com/rqx110)
  - 🐞 修复 预渲染时报错。[#2527](https://github.com/ant-design-blazor/ant-design-blazor/pull/2527) [@ElderJames](https://github.com/ElderJames)

- Modal
  - 🆕 增加 最大化支持。[#2573](https://github.com/ant-design-blazor/ant-design-blazor/pull/2573) [@zxyao145](https://github.com/zxyao145)
  - 🐞 修复 动画闪烁，修复无法复制的问题。[#2561](https://github.com/ant-design-blazor/ant-design-blazor/pull/2561) [@zxyao145](https://github.com/zxyao145)

  - Datepicker
  - 🆕 增加 12 小时制支持。[#2501](https://github.com/ant-design-blazor/ant-design-blazor/pull/2501) [@Alexbits](https://github.com/Alexbits)
  - 🆕 新增 RangePicker 预设范围。[#2487](https://github.com/ant-design-blazor/ant-design-blazor/pull/2487) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
  - 🆕 DatePicker/TimePicker 可以滚动到选中值。[#2512](https://github.com/ant-design-blazor/ant-design-blazor/pull/2512) [@Alexbits](https://github.com/Alexbits)
  - 🐞 修复 显示年份时本地化没效果。[#2589](https://github.com/ant-design-blazor/ant-design-blazor/pull/2589) [@Alexbits](https://github.com/Alexbits)
  - 🐞 修复 当 FirstDayOfWeek!=Sunday 时星期数错位问题。[#2571](https://github.com/ant-design-blazor/ant-design-blazor/pull/2571) [@Alexbits](https://github.com/Alexbits)
  - 🐞 修复一些 确认 按钮相关问题。[#2531](https://github.com/ant-design-blazor/ant-design-blazor/pull/2531) [@Alexbits](https://github.com/Alexbits)
  - 🐞 修复 Datepicker 使用代码修改绑定值时，面板没有改变的问题。[#2551](https://github.com/ant-design-blazor/ant-design-blazor/pull/2551) [@Alexbits](https://github.com/Alexbits)
  - 🐞 修复 日期选中的问题 。[#2570](https://github.com/ant-design-blazor/ant-design-blazor/pull/2570) [@Alexbits](https://github.com/Alexbits)

- Image
  - 🆕 支持可控预览。[#2600](https://github.com/ant-design-blazor/ant-design-blazor/pull/2600) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 在回退图像后设置的图像无法在预览中显示。[#2599](https://github.com/ant-design-blazor/ant-design-blazor/pull/2599) [@ElderJames](https://github.com/ElderJames)

- Form
  - 🐞 修复 AutoComplete、DatePicker、InputNumber 的验证错误样式。[#2647](https://github.com/ant-design-blazor/ant-design-blazor/pull/2647) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Select 验证错误样式[#2642](https://github.com/ant-design-blazor/ant-design-blazor/pull/2642) [@JamesGit-hash](https://github.com/JamesGit-hash)
  - 🐞 修复 Input 验证错误样式[#2639](https://github.com/ant-design-blazor/ant-design-blazor/pull/2639) [@JamesGit-hash](https://github.com/JamesGit-hash)

- Cascader
  - 💄 修复 最新样式导致的错乱。[#2636](https://github.com/ant-design-blazor/ant-design-blazor/pull/2636) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 在搜索时无法使用 AllowClear 清除内容的问题(#2607)。[#2610](https://github.com/ant-design-blazor/ant-design-blazor/pull/2610) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
  - 🐞 Cascader 显示文本不刷新。[#2575](https://github.com/ant-design-blazor/ant-design-blazor/pull/2575) [@noctis0430](https://github.com/noctis0430)
  
- Select
  - 🐞 修复当ignoreitemchanges=false时，删除select的已选择标签会报错的问题(#2617)。[#2620](https://github.com/ant-design-blazor/ant-design-blazor/pull/2620) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
  - 🛠 事件从 Action 改为 EventCallback。[#2601](https://github.com/ant-design-blazor/ant-design-blazor/pull/2601) [@ElderJames](https://github.com/ElderJames)

- Badge
  - 🐞 显示隐藏动画的优化[#2609](https://github.com/ant-design-blazor/ant-design-blazor/pull/2609) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Offset 属性不支持负数的问题[#2608](https://github.com/ant-design-blazor/ant-design-blazor/pull/2608) [@ElderJames](https://github.com/ElderJames)

- Statistic
  - 🐞 修复 CountDown 在后台不会刷新的问题。[#2598](https://github.com/ant-design-blazor/ant-design-blazor/pull/2598) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
  - 🐞 添加 CountDown 的 Reset 方法，并且重新设置绑定的 Value 后会自动刷新。[#2587](https://github.com/ant-design-blazor/ant-design-blazor/pull/2587) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)

- InputNumber
  - 🆕 增加 PlaceHolder 属性。[#2528](https://github.com/ant-design-blazor/ant-design-blazor/pull/2528) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Step 属性的值在某些文化里小数点不是 `.` 分割导致的显示异常。[#2547](https://github.com/ant-design-blazor/ant-design-blazor/pull/2547) [@petertorocsik](https://github.com/petertorocsik)
  
- 🛠 将 MessageService 更改为通过注入接口 IMessageService 使用。[#2633](https://github.com/ant-design-blazor/ant-design-blazor/pull/2633) [@kooliokey](https://github.com/kooliokey)
- 🐞 修复 Tree TreeNode 的 Disable 和 Checked 属性共存时不生效的问题。[#2583](https://github.com/ant-design-blazor/ant-design-blazor/pull/2583) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
- 🐞 修复 TimeLine 的 Pending 无法关闭的问题。[#2588](https://github.com/ant-design-blazor/ant-design-blazor/pull/2588) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
- 🐞 Sider 增加 Collapsed 属性双向绑定.[#2536](https://github.com/ant-design-blazor/ant-design-blazor/pull/2536) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Overlay 的 Trigger 右键不能打开浏览器菜单的问题。[#2602](https://github.com/ant-design-blazor/ant-design-blazor/pull/2602) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Pagination 在较小的屏幕宽度下，特定操作后，不会显示当前选中页的问题。[#2616](https://github.com/ant-design-blazor/ant-design-blazor/pull/2616) [@fcxxzux](https://github.com/fcxxzux)
- 🐞 修复 Upload 图片识别支持自定义修改图片文件扩展名，与添加 WebP 格式，修复 FileName 不存在 . 时产生下标越界异常。[#2626](https://github.com/ant-design-blazor/ant-design-blazor/pull/2626) [@AigioL](https://github.com/AigioL)
- 🐞 修复 Input 将数据粘贴到输入框时 OnChange 事件会调用两次的错误(#2591)。[#2592](https://github.com/ant-design-blazor/ant-design-blazor/pull/2592) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)

注意，由于上一次同步了v4.16.9 后，antd样式文件的调整导致原脚本无法编译，直到此次更新跨度较大，可能会出现样式的问题。如果发现，敬请提交issue告知。

### 0.11.0

`2022-06-16`

🌈守得云开见月明~

- Table
  - 🔥 支持虚拟化[#2143](https://github.com/ant-design-blazor/ant-design-blazor/pull/2143) [@anranruye](https://github.com/anranruye)
  - 🔥 支持使用已有的 QueryModel 控制/恢复表格筛选排序状态[#2129](https://github.com/ant-design-blazor/ant-design-blazor/pull/2129) [@AnaNikolasevic](https://github.com/AnaNikolasevic)
  - 🆕 支持用 `ScrollBarWidth` 属性来设置滚动条的宽度。[#2451](https://github.com/ant-design-blazor/ant-design-blazor/pull/2451) [@ElderJames](https://github.com/ElderJames)
  - 🆕 允许在定义 `PaginationTemplate` 时使用组件内置逻辑。[#2220](https://github.com/ant-design-blazor/ant-design-blazor/pull/2220) [@anranruye](https://github.com/anranruye)
  - 🛠 修改 Responsive 属性默认值为false，需要响应式样式时需要设为true。[#2419](https://github.com/ant-design-blazor/ant-design-blazor/pull/2419) [@ElderJames](https://github.com/ElderJames)
  - 🛠 使用 Small 大小的Pagination来适配紧凑型Table[#2246](https://github.com/ant-design-blazor/ant-design-blazor/pull/2246) [@anranruye](https://github.com/anranruye)

- 🆕 增加 Upload 支持结合原生 InputFile 组件。[#2443](https://github.com/ant-design-blazor/ant-design-blazor/pull/2443) [@ElderJames](https://github.com/ElderJames)

- 🆕 增加 Select 搜索框防抖延时绑定。[#2275](https://github.com/ant-design-blazor/ant-design-blazor/pull/2275) [@tompru](https://github.com/tompru)
- 🆕 组件库增加 .NET 6 目标框架。[#2484](https://github.com/ant-design-blazor/ant-design-blazor/pull/2484) [@ElderJames](https://github.com/ElderJames)

- TreeSelect
  - 🐞 修复表达式和选择功能。[#2507](https://github.com/ant-design-blazor/ant-design-blazor/pull/2507) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复绑定默认值时，没有显示选中项。[#2134](https://github.com/ant-design-blazor/ant-design-blazor/pull/2134) [@gmij](https://github.com/gmij)

- 🐞 修复 DatePicker 点击日期时，不能选中周的问题[#2463](https://github.com/ant-design-blazor/ant-design-blazor/pull/2463) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
- 🐞 修复 Tree 初始化时 checkbox 不能正常选中的问题。[#2506](https://github.com/ant-design-blazor/ant-design-blazor/pull/2506) [@ElderJames](https://github.com/ElderJames)

- ⌨️ 增加 Form 支持当验证失败时显示Feedback图标。[#2418](https://github.com/ant-design-blazor/ant-design-blazor/pull/2418) [@bweissronin](https://github.com/bweissronin)
- ⌨️ 增加 Checkbox 支持点击标签时触发勾选[#2296](https://github.com/ant-design-blazor/ant-design-blazor/pull/2296) [@bweissronin](https://github.com/bweissronin)
- ⌨️ 增加 Icon Alt 属性，与原来的`role="img"` 搭配[#2302](https://github.com/ant-design-blazor/ant-design-blazor/pull/2302) [@bweissronin](https://github.com/bweissronin)
- ⌨️ 增加 Button AriaLabel 属性[#2278](https://github.com/ant-design-blazor/ant-design-blazor/pull/2278) [@bweissronin](https://github.com/bweissronin)
- 📖 常用问答增加 CSS 隔离的组件样式修改方式[#2158](https://github.com/ant-design-blazor/ant-design-blazor/pull/2158) [@dennisrahmen](https://github.com/dennisrahmen)

### 0.10.7

`2022-05-22`

- 🐞 修复 Select 在更换有部分相同元素的 DataSource 时，不相同的元素会从最后开始排序的问题[#2462](https://github.com/ant-design-blazor/ant-design-blazor/pull/2462) [@ElderJames](https://github.com/ElderJames)

- Table
  - 🐞 修复 调用 addEventListener 时抛异常的问题[#2460](https://github.com/ant-design-blazor/ant-design-blazor/pull/2460) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 在 Dispose 时，DomEventListener 会抛出空引用异常的问题。[#2448](https://github.com/ant-design-blazor/ant-design-blazor/pull/2448) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 在小屏模式时，内容过长会导致页面被撑开的问题。[#2470](https://github.com/ant-design-blazor/ant-design-blazor/pull/2470) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Cascader 在搜索时，结果列表会循环增加的问题[#2457](https://github.com/ant-design-blazor/ant-design-blazor/pull/2457) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Menu 的 IconTemplate 在 SubMenu 中无效的问题。[#2449](https://github.com/ant-design-blazor/ant-design-blazor/pull/2449) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tree 节点含有 | 字符，并且 SearchVaule 搜索 | 时的显示异常问题。[#2437](https://github.com/ant-design-blazor/ant-design-blazor/pull/2437) [@ElderJames](https://github.com/ElderJames)


### 0.10.6

`2022-05-10`

上海加油 ❤️

- 🐞 修复 Tooltip 的 tabindex，优化 a11y。 [#2404](https://github.com/ant-design-blazor/ant-design-blazor/pull/2404) [@bweissronin](https://github.com/bweissronin)
- 🐞 修复 Form 绑定同名属性时的报异常问题。 [#2400](https://github.com/ant-design-blazor/ant-design-blazor/pull/2400) [@GHMonad](https://github.com/GHMonad)
- 🐞 修复 InputNumber 字符串转换为数字类型时的小数点本地化问题。 [#2398](https://github.com/ant-design-blazor/ant-design-blazor/pull/2398) [@jp-rl](https://github.com/jp-rl)

- Select
  - 🐞 修复当 Select 状态是禁用时, Label 还可以删除操作, 而且 Label 还有鲜艳的颜色。 [#2399](https://github.com/ant-design-blazor/ant-design-blazor/pull/2399) [@charset](https://github.com/charset)
  - 🐞 修复 Select 当设置 Value 为 null 后不能清除选项的问题。 [#2371](https://github.com/ant-design-blazor/ant-design-blazor/pull/2371) [@ElderJames](https://github.com/ElderJames)
  
- ⚡️ Tree 优化展开大量节点是的性能。 [#2385](https://github.com/ant-design-blazor/ant-design-blazor/pull/2385) [@densen2014](https://github.com/densen2014)
- 🐞 修复 Cascader 下拉列表在点击时不能打开的问题。 [#2363](https://github.com/ant-design-blazor/ant-design-blazor/pull/2363) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Upload 错误的拖动区域。 [#2360](https://github.com/ant-design-blazor/ant-design-blazor/pull/2360) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Input 绑定列表或字典元素时的报错。 [#2359](https://github.com/ant-design-blazor/ant-design-blazor/pull/2359) [@ElderJames](https://github.com/ElderJames)

### 0.10.5

2022-03-15

- 🐞 修复 Radio 在 RadioGroup 中时如果没有指定 Name 则设置默认的 Name。 [#2330](https://github.com/ant-design-blazor/ant-design-blazor/pull/2330) [@bweissronin](https://github.com/bweissronin)
- 🛠 修改 Upload 添加更多图片格式。[#2321](https://github.com/ant-design-blazor/ant-design-blazor/pull/2321) [@scugzbc](https://github.com/scugzbc)
- 🐞 修复 Tabs 数量超出范围时 TabTemplate 不能显示在 dropdown 中的问题。[#2320](https://github.com/ant-design-blazor/ant-design-blazor/pull/2320) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tree 渲染循环和选项框勾选不正常。[#2319](https://github.com/ant-design-blazor/ant-design-blazor/pull/2319) [@gmij](https://github.com/gmij)
- 🐞 修复 InputNumber 连续加减切换时偶尔出现的不停止问题。[#2317](https://github.com/ant-design-blazor/ant-design-blazor/pull/2317) [@jeffraska](https://github.com/jeffraska)
- 🐞 修复 Select 使用 DataSource 时下拉列表不能滚动到已选项的问题。[#2316](https://github.com/ant-design-blazor/ant-design-blazor/pull/2316) [@jeffraska](https://github.com/jeffraska)
- 🐞 修复 Badge 数字间的间隙。[#2315](https://github.com/ant-design-blazor/ant-design-blazor/pull/2315) [@ElderJames](https://github.com/ElderJames)

### 0.10.4

2022-02-25

- Table

  - 🆕 允许 从 CellRender 上下文中访问单元格数据。[#2257](https://github.com/ant-design-blazor/ant-design-blazor/pull/2257) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 PaginationOptions 的多语言设置。[#2244](https://github.com/ant-design-blazor/ant-design-blazor/pull/2244) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 当 PageIndex 和 PageSize 同时修改时，OnChagne 触发了两次。[#2239](https://github.com/ant-design-blazor/ant-design-blazor/pull/2239) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 使用 DataTable 作为数据源的支持。[#2234](https://github.com/ant-design-blazor/ant-design-blazor/pull/2234) [@ElderJames](https://github.com/ElderJames)
  - 📖 文档 完善组件文档中关于 Table 的 API 部分。[#2219](https://github.com/ant-design-blazor/ant-design-blazor/pull/2219) [@SmRiley](https://github.com/SmRiley)

- Upload

  - 🐞 修复 拖拽上传区域的居中样式。[#2267](https://github.com/ant-design-blazor/ant-design-blazor/pull/2267) [@oemil](https://github.com/oemil)
  - 📖 文档 添加 Upload 对接 API 的参考实现。[#2274](https://github.com/ant-design-blazor/ant-design-blazor/pull/2274) [@SmRiley](https://github.com/SmRiley)

- Modal

  - 🆕 增加 最大内容高度设置，提供内置 Form 表单的 demo。[#2264](https://github.com/ant-design-blazor/ant-design-blazor/pull/2264) [@zxyao145](https://github.com/zxyao145)
  - 🐞 修复 没有滚动条时错误的宽带设置。[#2212](https://github.com/ant-design-blazor/ant-design-blazor/pull/2212) [@zxyao145](https://github.com/zxyao145)
  - 🐞 修复 RTL 问题。[#2295](https://github.com/ant-design-blazor/ant-design-blazor/pull/2295) [@zxyao145](https://github.com/zxyao145)

- Datepicker

  - 🌐 修复 部分捷克语中的星期排序。[#2247](https://github.com/ant-design-blazor/ant-design-blazor/pull/2247) [@jeffraska](https://github.com/jeffraska)
  - 🐞 修复 缺失的前缀图标。 [#2226](https://github.com/ant-design-blazor/ant-design-blazor/pull/2226) [@KarimFereidooni](https://github.com/KarimFereidooni)

- 🐞 修复 图片无法居中。[#2287](https://github.com/ant-design-blazor/ant-design-blazor/pull/2287) [@zxyao145](https://github.com/zxyao145)
- 💄 修复 Result 缺少的 Style 属性渲染。[#2256](https://github.com/ant-design-blazor/ant-design-blazor/pull/2256) [@CAPCHIK](https://github.com/CAPCHIK)
- 🐞 修复 完善 RadioGroup 和 Radio 之间 Disabled 状态的同步逻辑。[#2197](https://github.com/ant-design-blazor/ant-design-blazor/pull/2197) [@LeaFrock](https://github.com/LeaFrock)
- 📖 文档 完善组件文档中关于 Input 和 Select 组件的 API 部分。[#2251](https://github.com/ant-design-blazor/ant-design-blazor/pull/2251) [@SmRiley](https://github.com/SmRiley)

### 0.10.3

2021-12-19

- Typography

  - 🐞 修复 复制 HTML 内容的功能。 [#2118](https://github.com/ant-design-blazor/ant-design-blazor/pull/2118) [@anranruye](https://github.com/anranruye)
  - 🐞 修复`Text`为 null 或空字符时`OnCopy`未被执行的问题。[#2098](https://github.com/ant-design-blazor/ant-design-blazor/pull/2098) [@LeaFrock](https://github.com/LeaFrock)

- Cascader

  - 🆕 增加 当可选项为空时显示空状态图片。[#2108](https://github.com/ant-design-blazor/ant-design-blazor/pull/2108) [@noctis0430](https://github.com/noctis0430)
  - 🐞 修复 当选项 Options 为 null 时引发异常的问题。[#2105](https://github.com/ant-design-blazor/ant-design-blazor/pull/2105) [@noctis0430](https://github.com/noctis0430)

- Tree

  - 🐞 修复 CheckedKeys 被修改时选中状态未修改的问题。[#2133](https://github.com/ant-design-blazor/ant-design-blazor/pull/2133) [@Guyiming](https://github.com/Guyiming)
  - 🐞 修复当设置了 Draggable 时，MatchedClass 不生效的问题。[#2171](https://github.com/ant-design-blazor/ant-design-blazor/pull/2171) [@jp-rl](https://github.com/jp-rl)
  - 🐞 修复 当 SearchValue 清空时，会收起全部节点的问题。[#2177](https://github.com/ant-design-blazor/ant-design-blazor/pull/2177) [@ElderJames](https://github.com/ElderJames)

- 🆕 增加 Form 组件对原生 `EditForm` 的支持。[#2138](https://github.com/ant-design-blazor/ant-design-blazor/pull/2138) [@knight1219](https://github.com/knight1219)
- 🐞 修复 LocaleProvider 初始化时会引发 `CultureNotFoundException` 异常的问题。[#2094](https://github.com/ant-design-blazor/ant-design-blazor/pull/2094) [@anranruye](https://github.com/anranruye)
- 🐞 修复 Modal 禁用 body 滚动条时宽度设置的问题。[#2163](https://github.com/ant-design-blazor/ant-design-blazor/pull/2163) [@zxyao145](https://github.com/zxyao145
- 🐞 修复 Transfer 按钮的样式。[#2156](https://github.com/ant-design-blazor/ant-design-blazor/pull/2156) [@dennisrahmen](https://github.com/dennisrahmen)
- 🐞 修复 Select 绑定数据源为数组类型时引发的异常。[#2121](https://github.com/ant-design-blazor/ant-design-blazor/pull/2121) [@ocoka](https://github.com/ocoka)
- 🐞 修复 Checkbox Group 的双向绑定问题。[#2173](https://github.com/ant-design-blazor/ant-design-blazor/pull/2173) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Radio 作为组成员时`Disabled`属性应当始终与 `RadioGroup.Disabled` 一致。[#2142](https://github.com/ant-design-blazor/ant-design-blazor/pull/2142) [@LeaFrock](https://github.com/LeaFrock)

### 0.10.2

2021-11-5

- Descriptions

  - 💄 修复 Descriptions 头部样式[#2078](https://github.com/ant-design-blazor/ant-design-blazor/pull/2078) [@ElderJames](https://github.com/ElderJames)
  - 💄 描述组件 水平模式 列表项 补充 ‘ant-descriptions-item-container’ 样式。[#2024](https://github.com/ant-design-blazor/ant-design-blazor/pull/2024) [@weidyg](https://github.com/weidyg)

- Tabs

  - 🆕 增加 ReuseTabs 的右键菜单和页面设置。[#2075](https://github.com/ant-design-blazor/ant-design-blazor/pull/2075) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 Card 中的 Tabs 显示问题。[#2053](https://github.com/ant-design-blazor/ant-design-blazor/pull/2053) [@anddrzejb](https://github.com/anddrzejb)

- Table

  - 🆕 支持调用 ReloadData 时指定页码和行数。[#2072](https://github.com/ant-design-blazor/ant-design-blazor/pull/2072) [@ElderJames](https://github.com/ElderJames)
  - 🆕 为 Column 增加 Align 属性。[#2045](https://github.com/ant-design-blazor/ant-design-blazor/pull/2045) [@Qyperion](https://github.com/Qyperion)
  - 🐞 修复 `ReloadData()` 不能触发 `OnChange` 的问题。[#2071](https://github.com/ant-design-blazor/ant-design-blazor/pull/2071) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 @bind-Field 为 null 时的异常[#2025](https://github.com/ant-design-blazor/ant-design-blazor/pull/2025) [@Guyiming](https://github.com/Guyiming)

- Select
  - 🐞 修复 Select 有分组时刷新数据源的异常。[#2048](https://github.com/ant-design-blazor/ant-design-blazor/pull/2048) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 增加 ValueOnClear 属性，指定点击清除按钮时绑定的值[#2023](https://github.com/ant-design-blazor/ant-design-blazor/pull/2023) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 修复 Calendar 在使用`DateFullCellRender` 时抛出异常[#2068](https://github.com/ant-design-blazor/ant-design-blazor/pull/2068) [@szymski](https://github.com/szymski)
- 💄 修复 Area 设置 AutoSize 时的问题。[#2001](https://github.com/ant-design-blazor/ant-design-blazor/pull/2001) [@anranruye](https://github.com/anranruye)
- 🐞 修复 Upload 因大小写导致的 IsPicture 判断异常[#2049](https://github.com/ant-design-blazor/ant-design-blazor/pull/2049) [@berkerdong](https://github.com/berkerdong)
- 🐞 修复 Overlay 组件的异常[#2036](https://github.com/ant-design-blazor/ant-design-blazor/pull/2036) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 RouterMatch 默认值改为 All[d8352b8](https://github.com/ant-design-blazor/ant-design-blazor/commit/d8352b8) [@ElderJames](https://github.com/ElderJames)
- 🌐 i18n: 捷克语言更新。[#2030](https://github.com/ant-design-blazor/ant-design-blazor/pull/2030) [@Martin-Pucalka](https://github.com/Martin-Pucalka)

### 0.10.1

2021-10-13

- Tabs

  - 🆕 增加 AuthorizeReuseTabsRouteView 组件，用于多标签页的验证[#1910](https://github.com/ant-design-blazor/ant-design-blazor/pull/1910) [@Guyiming](https://github.com/Guyiming)
  - 🛠 改进性能，修复了一些问题[#1970](https://github.com/ant-design-blazor/ant-design-blazor/pull/1970) [@ElderJames](https://github.com/ElderJames)

- Select

  - 🐞 修复 点击箭头自动关闭的问题[#1977](https://github.com/ant-design-blazor/ant-design-blazor/pull/1977) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 当存在值为 null 的选项时导致的问题[#1996](https://github.com/ant-design-blazor/ant-design-blazor/pull/1996) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 不可搜索的 Select 在移动设备激活键盘的问题。[#1992](https://github.com/ant-design-blazor/ant-design-blazor/pull/1992) [@anranruye](https://github.com/anranruye)

- Table

  - 🐞 修复 Selection 的性能问题和翻页时的状态问题[#1973](https://github.com/ant-design-blazor/ant-design-blazor/pull/1973) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 树形结构子级数据的排序和筛选[#1966](https://github.com/ant-design-blazor/ant-design-blazor/pull/1966) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复首次加载的问题[#1957](https://github.com/ant-design-blazor/ant-design-blazor/pull/1957) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复在小屏模式下标签不显示的问题[#1952](https://github.com/ant-design-blazor/ant-design-blazor/pull/1952) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 绑定了可空的 DateTime 属性时，内置 DateTime 类型筛选器的异常。 [#1964](https://github.com/ant-design-blazor/ant-design-blazor/pull/1964) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 `RemoveMilliseconds` 方法的实现在 EFCore Pomelo Povider 不被支持的问题。 [#1895](https://github.com/ant-design-blazor/ant-design-blazor/pull/1895) [@iamSmallY](https://github.com/iamSmallY)

- Menu

  - 🐞 修复 Menu 收起时 IconTemplate 的样式问题[#2006](https://github.com/ant-design-blazor/ant-design-blazor/pull/2006) [@knight1219](https://github.com/knight1219)
  - 🐞 修复 弹出层的一些问题，并优化性能。[#1949](https://github.com/ant-design-blazor/ant-design-blazor/pull/1949) [@anddrzejb](https://github.com/anddrzejb)

- 🐞 修复使用了 Min 或 Max 时的问题[#1940](https://github.com/ant-design-blazor/ant-design-blazor/pull/1940) [@rabberbock](https://github.com/rabberbock)
- 🐞 修复 Grid 由于 breakpoint 枚举命名大小写导致 breakpoint 匹配的问题。[#1963](https://github.com/ant-design-blazor/ant-design-blazor/pull/1963) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 ToString 的本地化问题[#1956](https://github.com/ant-design-blazor/ant-design-blazor/pull/1956) [@bezysoftware](https://github.com/bezysoftware)
- 📖 整理按钮组件的文档[#1953](https://github.com/ant-design-blazor/ant-design-blazor/pull/1953) [@Hona](https://github.com/Hona)
- 🐞 修复当 Modal 过高时无法拖动的问题。[#1951](https://github.com/ant-design-blazor/ant-design-blazor/pull/1951) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复自定义语言的问题，改进回退策略。[#1988](https://github.com/ant-design-blazor/ant-design-blazor/pull/1988) [@anranruye](https://github.com/anranruye)
- 🐞 修复 List 使其动态响应 `Grid` 属性的变化。 [#2014](https://github.com/ant-design-blazor/ant-design-blazor/pull/2014) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 修复 progress 忽略本地化差异对 Style 字符串的影响。 [#2017](https://github.com/ant-design-blazor/ant-design-blazor/pull/2017) [@CAPCHIK](https://github.com/CAPCHIK)
- 🌐 i18n: 更新捷克语。[#2019](https://github.com/ant-design-blazor/ant-design-blazor/pull/2019) [@Martin Pučálka](https://github.com/Martin-Pucalka)

### 0.10.0

2021-09-15

- 🔥 增加 TreeSelect 组件。[#1773](https://github.com/ant-design-blazor/ant-design-blazor/pull/1773) [@gmij](https://github.com/gmij)

- Tree

  - 🆕 增加 Tree 的 ChildContent 模板，不需要 Nodes。[#1887](https://github.com/ant-design-blazor/ant-design-blazor/pull/1887) [@ElderJames](https://github.com/ElderJames)
  - 🛠 修改 Tree 的 API 名称：`CheckedAll` 改为 `CheckAll`,`DecheckedAll` 改为 `UncheckAll`。[#1792](https://github.com/ant-design-blazor/ant-design-blazor/pull/1792) [@lukblazewicz](https://github.com/lukblazewicz)

- Radio

  - 🆕 增加 Radio 的 RadioGroup 枚举类型选项支持，可使用 `EnumRadioGroup`。[#1840](https://github.com/ant-design-blazor/ant-design-blazor/pull/1840) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加 Radio 的 RadioGroup Options 属性。[#1839](https://github.com/ant-design-blazor/ant-design-blazor/pull/1839) [@ElderJames](https://github.com/ElderJames)

- 🆕 增加 Timeline 的 Label 属性。[#1941](https://github.com/ant-design-blazor/ant-design-blazor/pull/1941) [@ElderJames](https://github.com/ElderJames)
- 🆕 增加 Component 组件，用于生成动态类型的组件。[#1703](https://github.com/ant-design-blazor/ant-design-blazor/pull/1703) [@anranruye](https://github.com/anranruye)
- 🆕 增加 Image 的相册模式。[#1842](https://github.com/ant-design-blazor/ant-design-blazor/pull/1842) [@ElderJames](https://github.com/ElderJames)
- 🆕 增加 Form FormItem 的 `Help`、`ValidateStatus` 和`HasFeedback` 属性，支持多种类型的表单信息。[#1807](https://github.com/ant-design-blazor/ant-design-blazor/pull/1807) [@JamesGit-hash](https://github.com/JamesGit-hash)
- 🆕 增加 Table 的响应式模式，移动端屏幕下将变成卡片式列表。[#1802](https://github.com/ant-design-blazor/ant-design-blazor/pull/1802) [@ElderJames](https://github.com/ElderJames)

### 0.9.4

2021-09-12

- Table

  - 🐞 修复 在 PageSize 不等于 10 时，初始化时会被刷新两次的问题。[#1933](https://github.com/ant-design-blazor/ant-design-blazor/pull/1933) [@ElderJames](https://github.com/ElderJames)
  - 🆕 传递 `CellData` 给 CellRender 模板，可访问当前单元格和行的一些信息。[#1907](https://github.com/ant-design-blazor/ant-design-blazor/pull/1907) [@ElderJames](https://github.com/ElderJames)
  - ⚡️ 将固定列的样式处理放到 JS，以提升性能。[#1897](https://github.com/ant-design-blazor/ant-design-blazor/pull/1897) [@ElderJames](https://github.com/ElderJames)
  - 📖 增加 动态表格 demo。[#1908](https://github.com/ant-design-blazor/ant-design-blazor/pull/1908) [@ElderJames](https://github.com/ElderJames)

- InputNumber

  - 🆕 增加 OnFocus 事件。[#1931](https://github.com/ant-design-blazor/ant-design-blazor/pull/1931) [@Hona](https://github.com/Hona)
  - 🐞 修复 inputmode，支持手机数字键盘。[#1923](https://github.com/ant-design-blazor/ant-design-blazor/pull/1923) [@CAPCHIK](https://github.com/CAPCHIK)

- Select

  - 🐞 修复 对异构选项类型的支持。[#1932](https://github.com/ant-design-blazor/ant-design-blazor/pull/1932) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 设置 DataSource 时，选中项会被的重置问题。[#1906](https://github.com/ant-design-blazor/ant-design-blazor/pull/1906) [@anddrzejb](https://github.com/anddrzejb)

- 🐞 修复 Overlay 与 dropdown、选项框、popup 有关的一系列问题。[#1848](https://github.com/ant-design-blazor/ant-design-blazor/pull/1848) [@anddrzejb](https://github.com/anddrzejb)
- 💄 修复 Button 的 loading 样式。[#1902](https://github.com/ant-design-blazor/ant-design-blazor/pull/1902) [@CAPCHIK](https://github.com/CAPCHIK)
- 🐞 增加 TextArea 的 `Rows` 属性，支持固定的行数。[#1920](https://github.com/ant-design-blazor/ant-design-blazor/pull/1920) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 增加 Input 的 StopPropogation 属性，以减少事件触发，提升性能。[#1917](https://github.com/ant-design-blazor/ant-design-blazor/pull/1917) [@Hona](https://github.com/Hona)
- 🐞 修复 Form 移除已释放的 FormItem 实例。[#1901](https://github.com/ant-design-blazor/ant-design-blazor/pull/1901) [@lxyruanjian](https://github.com/lxyruanjian)
- ⚡️ 事件订阅器的内存泄漏问题。[#1857](https://github.com/ant-design-blazor/ant-design-blazor/pull/1857) [@tonyyip1969](https://github.com/tonyyip1969)
- 🐞 修复 List 组件的响应式无效的问题。 [#1937](https://github.com/ant-design-blazor/ant-design-blazor/pull/1937) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Menu 有 RouterLink 的 MenuItem 在收起时 Title 不隐藏的问题。[#1934](https://github.com/ant-design-blazor/ant-design-blazor/pull/1934) [@ElderJames](https://github.com/ElderJames)

### 0.9.3

2021-08-29

- Table

  - 🆕 新增 时间列内置筛选器 `等于该日期` 条件，只匹配日期。[#1856](https://github.com/ant-design-blazor/ant-design-blazor/pull/1856) [@iamSmallY](https://github.com/iamSmallY)
    [#1889](https://github.com/ant-design-blazor/ant-design-blazor/pull/1889) [@anranruye](https://github.com/anranruye)
  - 📖 增加 表格嵌套的示例。[#1884](https://github.com/ant-design-blazor/ant-design-blazor/pull/1884) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 时间列内置筛选器在筛选时将忽略毫秒。[#1864](https://github.com/ant-design-blazor/ant-design-blazor/pull/1864) [@iamSmallY](https://github.com/iamSmallY)
  - 🐞 修复 使用客户端模式翻页、排序、筛选等操作不刷新的问题。[#1858](https://github.com/ant-design-blazor/ant-design-blazor/pull/1858) [@ElderJames](https://github.com/ElderJames)
    [#1875](https://github.com/ant-design-blazor/ant-design-blazor/pull/1875) [@nikolaykrondev](https://github.com/nikolaykrondev)
  - 🐞 修复 初始化后 OnChange 调用多次的问题。[#1855](https://github.com/ant-design-blazor/ant-design-blazor/pull/1855) [@ElderJames](https://github.com/ElderJames)

- 🆕 新增 Breadcrumb 的 Href 和 Overlay 下拉菜单。 [#1859](https://github.com/ant-design-blazor/ant-design-blazor/pull/1859) [@CAPCHIK](https://github.com/CAPCHIK)
- 🆕 新增 MenuItem 的 IconTemplate。 [#1879](https://github.com/ant-design-blazor/ant-design-blazor/pull/1879) [@Guyiming](https://github.com/Guyiming)
- 🆕 新增 Upload 指定 Http Method 的支持。[#1853](https://github.com/ant-design-blazor/ant-design-blazor/pull/1853) [@SapientGuardian](https://github.com/SapientGuardian)
- 🐞 修复 Tag 中 Checked 属性的双向绑定。[#1876](https://github.com/ant-design-blazor/ant-design-blazor/pull/1876) [@stefanodriussi](https://github.com/stefanodriussi)
- 🐞 修复 AutoComplete 下拉菜单定位问题。[#1860](https://github.com/ant-design-blazor/ant-design-blazor/pull/1860) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 修复 InputNumber 的 DefaultValue 默认绑定问题。[#1871](https://github.com/ant-design-blazor/ant-design-blazor/pull/1871) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Checkbox 的 CheckboxGroup 可选项被修改后，选择时引发异常的问题。 [#1863](https://github.com/ant-design-blazor/ant-design-blazor/pull/1863) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 修复 Modal 和 Confirm 无法自动 focus 的 bug。[#1838](https://github.com/ant-design-blazor/ant-design-blazor/pull/1838) [@zxyao145](https://github.com/zxyao145)

### 0.9.2

2021-08-18

- Table

  - 🐞 修复 阻止点击展开按钮时的事件穿透[#1850](https://github.com/ant-design-blazor/ant-design-blazor/pull/1850) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 服务端模式初次加载时不触发 OnChange 的问题。[#1835](https://github.com/ant-design-blazor/ant-design-blazor/pull/1835) [@ElderJames](https://github.com/ElderJames)

- 🐞 修复 Tree 切换时选中节点时 SelectedNodeChanged 事件触发两次[#1849](https://github.com/ant-design-blazor/ant-design-blazor/pull/1849) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tag 组件 Style 参数设置无效。[#1847](https://github.com/ant-design-blazor/ant-design-blazor/pull/1847) [@JohnHao421](https://github.com/JohnHao421)
- 🐞 修复 Menu `OnMenuItemClicked` 事件在 `Selectable=false` 时不触发的问题。[#1843](https://github.com/ant-design-blazor/ant-design-blazor/pull/1843) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Checkbox 当 CheckboxGroup 的 Value 被修改时，对应选项未选中的问题。[#1841](https://github.com/ant-design-blazor/ant-design-blazor/pull/1841) [@ElderJames](https://github.com/ElderJames)

### 0.9.1

2021-08-11

- Table

  - 🐞 内置日期筛选器可以设置时间了。[#1827](https://github.com/ant-design-blazor/ant-design-blazor/pull/1827) [@anranruye](https://github.com/anranruye)
  - 🐞 内置 列表类型筛选器改为 OR 操作[#1804](https://github.com/ant-design-blazor/ant-design-blazor/pull/1804) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 首次触发 OnChange 时没有传入默认排序的问题。[#1823](https://github.com/ant-design-blazor/ant-design-blazor/pull/1823) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 把另一个 Table 放到一个 Table 的 Column 中是引发的异常。[#1732](https://github.com/ant-design-blazor/ant-design-blazor/pull/1732) [@anranruye](https://github.com/anranruye)

- DatePicker

  - 🐞 修复 在选择改变时，毫秒丢失的问题[#1829](https://github.com/ant-design-blazor/ant-design-blazor/pull/1829) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 ShowTime 为 True 时，ShowToday 设置 false 无效的问题。[#1819](https://github.com/ant-design-blazor/ant-design-blazor/pull/1819) [@lukblazewicz](https://github.com/lukblazewicz)
  - 🐞 修复 RangePicker 的一些问题[#1788](https://github.com/ant-design-blazor/ant-design-blazor/pull/1788) [@anddrzejb](https://github.com/anddrzejb)

- Overlay

  - 🐞 修复 定位，使 BottomRight 和 TopRight 方向使总是向右对齐。[#1799](https://github.com/ant-design-blazor/ant-design-blazor/pull/1799) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 在容器元素中的定位[#1797](https://github.com/ant-design-blazor/ant-design-blazor/pull/1797) [@anranruye](https://github.com/anranruye)

- Select

  - 🆕 使 EnumSelect 支持 null 选项[#1777](https://github.com/ant-design-blazor/ant-design-blazor/pull/1777) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 MaxTagCount 与响应式的问题。[#1776](https://github.com/ant-design-blazor/ant-design-blazor/pull/1776) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 允许把 `null` 作为选项值。[#1786](https://github.com/ant-design-blazor/ant-design-blazor/pull/1786) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 非 DataSource 方式时对 ValueName 的 null 检查。[#1785](https://github.com/ant-design-blazor/ant-design-blazor/pull/1785) [@anranruye](https://github.com/anranruye)

- Tree

  - 🐞 加回 `SearchExpression` 属性。[#1796](https://github.com/ant-design-blazor/ant-design-blazor/pull/1796) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 父节点勾选状态的问题[#1781](https://github.com/ant-design-blazor/ant-design-blazor/pull/1781) [@lukblazewicz](https://github.com/lukblazewicz)

- 🐞 修复 InputNumber 使用键盘输入时不触发 `OnChange` 的问题。[#1830](https://github.com/ant-design-blazor/ant-design-blazor/pull/1830) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Confirm 在重复点击引起异常的问题。[#1795](https://github.com/ant-design-blazor/ant-design-blazor/pull/1795) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复 AutoComplete 浏览器自动填充的问题[#1825](https://github.com/ant-design-blazor/ant-design-blazor/pull/1825) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Menu 避免重复多次触发 OnBreakpoint 和 OnCollapse。[#1815](https://github.com/ant-design-blazor/ant-design-blazor/pull/1815) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Calendar 头部选择器的宽度[#1801](https://github.com/ant-design-blazor/ant-design-blazor/pull/1801) [@anranruye](https://github.com/anranruye)

### 0.9.0

2021-07-27

🎉 截至这个版本，本项目一共迎来 101 位贡献者，是他们成就了这个项目！在此感谢他们慷慨的贡献！

- Tabs

  - 🔥 增加路由服用多标签页组件 `ReuseTabs`。([demo](https://github.com/ant-design-blazor/demo-reuse-tabs)) [#1704](https://github.com/ant-design-blazor/ant-design-blazor/pull/1704) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加 `OnClose` 事件与 `TabTemplate`。[#1698](https://github.com/ant-design-blazor/ant-design-blazor/pull/1698) [@ElderJames](https://github.com/ElderJames)

- Table

  - 🆕 添加 Guid 类型的内置筛选器。[#1756](https://github.com/ant-design-blazor/ant-design-blazor/pull/1756) [@anranruye](https://github.com/anranruye)
  - ⚡️ 优化内部的渲染片段。[#1597](https://github.com/ant-design-blazor/ant-design-blazor/pull/1597) [@anranruye](https://github.com/anranruye)
  - 🛠 可通过 `ITableFilterModel` 访问 `TableFilter`，可访问 `TableFilter` 中的比较运算符和条件运算符。[#1563](https://github.com/ant-design-blazor/ant-design-blazor/pull/1563) [@anranruye](https://github.com/anranruye)
  - 🆕 为枚举类型添加内置筛选器, 列表类型的筛选器添加支持 null 值。[#1439](https://github.com/ant-design-blazor/ant-design-blazor/pull/1439) [@anranruye](https://github.com/anranruye)
  - 🆕 增加 可隐藏列[#1410](https://github.com/ant-design-blazor/ant-design-blazor/pull/1410) [@ldsenow](https://github.com/ldsenow)
  - 🆕 增加 自定义翻页器的支持[#1409](https://github.com/ant-design-blazor/ant-design-blazor/pull/1409) [@ldsenow](https://github.com/ldsenow)
  - 🛠 用 PathHelper 替换 PropertyAccessHelper, 用单引号替换双引号标识字符串索引键。[#1386](https://github.com/ant-design-blazor/ant-design-blazor/pull/1386) [@Zonciu](https://github.com/Zonciu)
  - 🐞 修复分页问题，实现 TotalChanged 回调；添加远程加载数据示例。[#1558](https://github.com/ant-design-blazor/ant-design-blazor/pull/1558) [@anranruye](https://github.com/anranruye)
  - 📖 修复 EditRow demo 在点击取消时不恢复原值的 bug。[#1745](https://github.com/ant-design-blazor/ant-design-blazor/pull/1745) [@ElderJames](https://github.com/ElderJames)

- Select

  - 🆕 增加 EnumSelect，可将枚举转化为选项[#1759](https://github.com/ant-design-blazor/ant-design-blazor/pull/1759) [@wangj90](https://github.com/wangj90)
  - 🐞 修复多选模式 Tag 重复的问题[#1766](https://github.com/ant-design-blazor/ant-design-blazor/pull/1766) [@anddrzejb](https://github.com/anddrzejb)
  - 🚫 当数据源中的项和 Select 的 Value 属性使用相同类型时，无需指定 ValueName；当不指定 LabelName 时，将使用数据源中的项的 `ToString()` 方法的返回值作为 Label。[#1541](https://github.com/ant-design-blazor/ant-design-blazor/pull/1541) [@anranruye](https://github.com/anranruye)
  - 🐞 修复当使用 `SelectOption` 时不能为 Select 组件设置初始值的问题。[#1743](https://github.com/ant-design-blazor/ant-design-blazor/pull/1743) [@anranruye](https://github.com/anranruye)

- Form

  - 🆕 支持在 FormItem 上直接添加验证规则（不只是通过 Model 上的特性）。[#1516](https://github.com/ant-design-blazor/ant-design-blazor/pull/1516) [@mutouzdl](https://github.com/mutouzdl)
  - 🆕 支持 `EditContext` 重新赋值，增加`OnFieldChanged`, `OnValidationRequested` 和 `OnValidationStateChanged` 事件[#1504](https://github.com/ant-design-blazor/ant-design-blazor/pull/1504) [@anddrzejb](https://github.com/anddrzejb)
  - 🆕 `FormItem` 增加 `LabelStyle` 属性，支持修改其样式。[#1503](https://github.com/ant-design-blazor/ant-design-blazor/pull/1503) [@anddrzejb](https://github.com/anddrzejb)
  - 🆕 暴露 `Form` 组件中的 `EditContext` ，使用户可以访问验证信息。[#1464](https://github.com/ant-design-blazor/ant-design-blazor/pull/1464) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 FormItem 默认会显示属性名的问题。[#1738](https://github.com/ant-design-blazor/ant-design-blazor/pull/1738) [@ElderJames](https://github.com/ElderJames)

- Modal

  - 🆕 添加 NotificationRef 的支持。[#1498](https://github.com/ant-design-blazor/ant-design-blazor/pull/1498) [@zxyao145](https://github.com/zxyao145)
  - 🐞 修复 ModalOptions.ConfirmLoading 中 setter 错误赋值（总是 true）。[#1742](https://github.com/ant-design-blazor/ant-design-blazor/pull/1742) [@zxyao145](https://github.com/zxyao145)
  - 🐞 修复 dialog 系列组件被点击时导致 mask 的点击事件被触发的 bug。[#1727](https://github.com/ant-design-blazor/ant-design-blazor/pull/1727) [@zxyao145](https://github.com/zxyao145)

- Tree

  - 🛠 接近官方功能，修复初始值问题，并支持拖拽。[#1517](https://github.com/ant-design-blazor/ant-design-blazor/pull/1517) [@lovachen](https://github.com/lovachen)
  - 🆕 支持通过组件 API 来全选[#1722](https://github.com/ant-design-blazor/ant-design-blazor/pull/1722) [@lukblazewicz](https://github.com/lukblazewicz)

- 🆕 Button: 支持设置官方色板中的颜色[#1774](https://github.com/ant-design-blazor/ant-design-blazor/pull/1774) [@boukenka](https://github.com/boukenka)
- 🆕 Dropdown: 增加 `ButtonsStyle` 和 `ButtonsClass` 属性来支持自定义各个按钮的样式，修改 `Type` 属性支持单个值来同时应用到两个按钮[#1659](https://github.com/ant-design-blazor/ant-design-blazor/pull/1659) [@anddrzejb](https://github.com/anddrzejb)
- 🆕 DatePicker: `RangePicker` 支持禁用单个输入框。[#1648](https://github.com/ant-design-blazor/ant-design-blazor/pull/1648) [@mutouzdl](https://github.com/mutouzdl)
- 🆕 Tag: 组件的 Color 属性支持十六进制色值或预设的枚举值。[#1514](https://github.com/ant-design-blazor/ant-design-blazor/pull/1514) [@MutatePat](https://github.com/MutatePat)
- 🐞 Drawer: 修复在同时有多个 Drawer 时关闭其中一个就恢复页面滚动条的 Bug。[#1771](https://github.com/ant-design-blazor/ant-design-blazor/pull/1771) [@zxyao145](https://github.com/zxyao145)
- 🆕 Upload: 支持拖拽上传。[#1765](https://github.com/ant-design-blazor/ant-design-blazor/pull/1765) [@ElderJames](https://github.com/ElderJames)
- 🌐 i18n: 修复法语的周数翻译。[#1521](https://github.com/ant-design-blazor/ant-design-blazor/pull/1521) [@dust63](https://github.com/dust63)

### 0.8.3

`2021-07-13`

- Table

  - 🆕 增加属性可使 Table 可展开行默认全部展开。[#1695](https://github.com/ant-design-blazor/ant-design-blazor/pull/1695) [@henrikwidlund](https://github.com/henrikwidlund)
  - 🐞 修复选择筛选器的 与/或 条件会关闭筛选器面板的错误。[#1687](https://github.com/ant-design-blazor/ant-design-blazor/pull/1687) [@anranruye](https://github.com/anranruye)
  - 🐞 允许在表格初始化之后设置筛选器。[#1667](https://github.com/ant-design-blazor/ant-design-blazor/pull/1667) [@anranruye](https://github.com/anranruye)

- Upload

  - 🐞 修复 GetResponse() 反序列化，忽略大小写。[#1717](https://github.com/ant-design-blazor/ant-design-blazor/pull/1717) [@BeiYinZhiNian](https://github.com/BeiYinZhiNian)
  - 🐞 将上传模块响应中的所有 2xx 状态代码视为成功。[#1705](https://github.com/ant-design-blazor/ant-design-blazor/pull/1705) [@henrikwidlund](https://github.com/henrikwidlund)

- DatePicker

  - 🐞 为自定义和基于文化的格式修复日期选择器宽度。[#1685](https://github.com/ant-design-blazor/ant-design-blazor/pull/1685) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 日期解析问题。[#1663](https://github.com/ant-design-blazor/ant-design-blazor/pull/1663) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 点击面板时导致 Input 框失去焦点的问题。[#1681](https://github.com/ant-design-blazor/ant-design-blazor/pull/1681) [@anddrzejb](https://github.com/anddrzejb)

- Form

  - 🆕 使用 DisplayName 特性作为 FormItem Label。[#1682](https://github.com/ant-design-blazor/ant-design-blazor/pull/1682) [@gmij](https://github.com/gmij)
  - 🐞 修复 多个输入组件组合时只需要最上层组件设置 bind-Value 属性。[#1662](https://github.com/ant-design-blazor/ant-design-blazor/pull/1662) [@anranruye](https://github.com/anranruye)
  - 📖 增加 form 的高级搜索 demo。[#1654](https://github.com/ant-design-blazor/ant-design-blazor/pull/1654) [@ElderJames](https://github.com/ElderJames)

- i18n

  - 🌐 修复俄语资源。[#1709](https://github.com/ant-design-blazor/ant-design-blazor/pull/1709) [@kuznecovIT](https://github.com/kuznecovIT)
  - 🐞 当资源文件中节点缺失时，使用默认值，不抛出运行时异常。[#1710](https://github.com/ant-design-blazor/ant-design-blazor/pull/1710) [@anranruye](https://github.com/anranruye)

- 🆕 Tag: 当 OnClick 事件绑定了方法时，指针变为手指。[#1660](https://github.com/ant-design-blazor/ant-design-blazor/pull/1660) [@anddrzejb](https://github.com/anddrzejb)
- ⚡️ Modal and Drawer 组件减少重复渲染，进行文档和 demo 的更新。[#1701](https://github.com/ant-design-blazor/ant-design-blazor/pull/1701) [@zxyao145](https://github.com/zxyao145)
- 🐞 允许在一个渲染周期内同时改变数据源和值。[#1720](https://github.com/ant-design-blazor/ant-design-blazor/pull/1720) [@anranruye](https://github.com/anranruye)
- 🐞 修复 标签的鼠标滚轮滚动。[#1581](https://github.com/ant-design-blazor/ant-design-blazor/pull/1581) [@Brian-Ding](https://github.com/Brian-Ding)
- 🐞 修复 CountDown 组件 OnFinish 回调异常。[#1714](https://github.com/ant-design-blazor/ant-design-blazor/pull/1714) [@HexJacaranda](https://github.com/HexJacaranda)
- 🐞 当弹出层大小改变时会触发 OnMaskClick 事件。[#1692](https://github.com/ant-design-blazor/ant-design-blazor/pull/1692) [@anranruye](https://github.com/anranruye)
- 🐞 修复 Space 子项在 "if "块中的渲染顺序问题。[#1684](https://github.com/ant-design-blazor/ant-design-blazor/pull/1684) [@anranruye](https://github.com/anranruye)
- 🐞 修复 Grid 的 Col 在初始化时的默认间距调整。[#1653](https://github.com/ant-design-blazor/ant-design-blazor/pull/1653) [@ElderJames](https://github.com/ElderJames)

### 0.8.2

`2021-06-17`

- Table

  - 🐞 修复 Selection 的选择和清空功能。 [#1632](https://github.com/ant-design-blazor/ant-design-blazor/pull/1632) [@anranruye](https://github.com/anranruye)
  - 🐞 修复删除一个筛选条件后筛选器比较运算符错误的问题；移除 Is Null 和 Is Not Null 筛选条件的输入组件。[#1596](https://github.com/ant-design-blazor/ant-design-blazor/pull/1596) [@anranruye](https://github.com/anranruye)
  - 🐞 修复点击筛选图标关闭筛选器面板时不应用筛选操作的问题。[#1594](https://github.com/ant-design-blazor/ant-design-blazor/pull/1594) [@anranruye](https://github.com/anranruye)
  - 🐞 修复筛选器图标错误地持续处于选中状态的错误；修复没有在筛选器的输入组件输入任何值时也会进行筛选的问题。[#1592](https://github.com/ant-design-blazor/ant-design-blazor/pull/1592) [@anranruye](https://github.com/anranruye)
  - 🐞 修复点击筛选器确定按钮筛选器面板不关闭的问题。[#1602](https://github.com/ant-design-blazor/ant-design-blazor/pull/1602) [@anranruye](https://github.com/anranruye)
  - 📖 更新“复刻官方示例”示例以使排序可用。[#1544](https://github.com/ant-design-blazor/ant-design-blazor/pull/1544) [@anranruye](https://github.com/anranruye)

- Dropdown

  - 🐞 修复 Dropdown 的触发按钮[#1609](https://github.com/ant-design-blazor/ant-design-blazor/pull/1609) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 给按钮增加 `Loading` 属性。[#1588](https://github.com/ant-design-blazor/ant-design-blazor/pull/1588) [@anddrzejb](https://github.com/anddrzejb)

- DatePicker

  - 🐞 增加 OnClearClick 事件回调[#1586](https://github.com/ant-design-blazor/ant-design-blazor/pull/1586) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 Datepicker 在 Form 中的行为[#1617](https://github.com/ant-design-blazor/ant-design-blazor/pull/1617) [@anddrzejb](https://github.com/anddrzejb)

- InputNumber

  - 🐞 修复可空类型的组件失去焦点时抛出的异常。[#1612](https://github.com/ant-design-blazor/ant-design-blazor/pull/1612) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 InputNumber 在值计算时没包括 Parser。[#1567](https://github.com/ant-design-blazor/ant-design-blazor/pull/1567) [@anddrzejb](https://github.com/anddrzejb)

- Input 系列组件 [#1530](https://github.com/ant-design-blazor/ant-design-blazor/pull/1530) [@anddrzejb](https://github.com/anddrzejb)

  - 🐞 修复 Input 缺失的 `Bordered`、`ReadOnly`、`InputElementSuffixClass` 属性，增加 `Focus()`, `Blur()`方法。
  - 🐞 修复 TextArea 缺失的 `TextArea` `ShowCount` 属性，修复清除按钮。
  - 🐞 修复 Search 的样式，使用 `ClassicSearchIcon` 来回滚到旧样式。
  - 🐞 修复 InputPassword 的 `ShowPassword` 和 `IconRender` 属性。

- 🐞 修复 Affix 监听器移除的问题[#1616](https://github.com/ant-design-blazor/ant-design-blazor/pull/1616) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Drawer OffsetX 和 offsetY 不起作用，并更新使用 DrawerService 的文档。[#1448](https://github.com/ant-design-blazor/ant-design-blazor/pull/1448) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复 Radio 的 defaultChecked 和 RadioGroup 的 DefaultValue。[#1494](https://github.com/ant-design-blazor/ant-design-blazor/pull/1494) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Tag 的 Status 和自定义颜色的支持，增加动画 demo。[#1631](https://github.com/ant-design-blazor/ant-design-blazor/pull/1631) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 修复 Image 组件 Style 属性的作用位置。[#1642](https://github.com/ant-design-blazor/ant-design-blazor/pull/1642) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 CheckboxGroup 组件不能在 Form 中使用的问题。[#1637](https://github.com/ant-design-blazor/ant-design-blazor/pull/1637) [@anranruye](https://github.com/anranruye)
- 🌐 修复 荷兰语资源。[#1624](https://github.com/ant-design-blazor/ant-design-blazor/pull/1624) [@gregloones](https://github.com/gregloones)
- 🌐 修复 德语资源。[#1562](https://github.com/ant-design-blazor/ant-design-blazor/pull/1562) [@anranruye](https://github.com/anranruye)
- 🌐 修复 西班牙语资源。[#1534](https://github.com/ant-design-blazor/ant-design-blazor/pull/1534) [@Magehernan](https://github.com/Magehernan)

### 0.8.1

`2021-05-13`

- Overlay

  - 🐞 修复 计算高度时加上滚动高度[#1511](https://github.com/ant-design-blazor/ant-design-blazor/pull/1511) [@ocoka](https://github.com/ocoka)
  - 🐞 修复 边界调整的问题[#1420](https://github.com/ant-design-blazor/ant-design-blazor/pull/1420) [@mutouzdl](https://github.com/mutouzdl)

- Input

  - 🐞 修复 不能使用 Guid 类型的问题。[#1510](https://github.com/ant-design-blazor/ant-design-blazor/pull/1510) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 字符串与特定类型的转换问题，增加了 `CultureInfo` 属性。[#1480](https://github.com/ant-design-blazor/ant-design-blazor/pull/1480) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 按回车键的数据绑定问题。[#1375](https://github.com/ant-design-blazor/ant-design-blazor/pull/1375) [@ElderJames](https://github.com/ElderJames)

- Table

  - 🐞 修复 内置筛选器选项菜单的宽度[#1500](https://github.com/ant-design-blazor/ant-design-blazor/pull/1500) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 使用“结尾是”过滤条件时的错误。[#1434](https://github.com/ant-design-blazor/ant-design-blazor/pull/1434) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 点击清除排序时不刷新的问题。[#1385](https://github.com/ant-design-blazor/ant-design-blazor/pull/1385) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 无法使用 DataIndex 绑定可空属性的问题[#1382](https://github.com/ant-design-blazor/ant-design-blazor/pull/1382) [@anranruye](https://github.com/anranruye)
  - 🐞 修复 筛选器对 DataIndex 的支持，统一 FieldName 定义，添加列名 DisplayAttribute 支持。[#1372](https://github.com/ant-design-blazor/ant-design-blazor/pull/1372) [@Zonciu](https://github.com/Zonciu)
  - 🐞 修复 ellipsis 无效的问题。[#1376](https://github.com/ant-design-blazor/ant-design-blazor/pull/1376) [@ElderJames](https://github.com/ElderJames)

- Cascader

  - 🐞 修复 搜索功能。[#1484](https://github.com/ant-design-blazor/ant-design-blazor/pull/1484) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 当点击清楚按钮时触发 SelectedNodesChanged。[#1437](https://github.com/ant-design-blazor/ant-design-blazor/pull/1437) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 无法设置 Size 的问题。[#1432](https://github.com/ant-design-blazor/ant-design-blazor/pull/1432) [@ElderJames](https://github.com/ElderJames)

- DatePicker

  - 🐞 修复 DatePicker 点击面板头部会关闭问题[#1452](https://github.com/ant-design-blazor/ant-design-blazor/pull/1452) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 验证手动输入格式的问题[#1389](https://github.com/ant-design-blazor/ant-design-blazor/pull/1389) [@anddrzejb](https://github.com/anddrzejb)

- Modal

  - 🆕 可通过 ModalOptions 设置 Style。 [#1400](https://github.com/ant-design-blazor/ant-design-blazor/pull/1400) [@zxyao145](https://github.com/zxyao145)
  - 🐞 修改 Dialog 组件中 Mask 点击判断 Task.Delay 的时间为 DOM* MIN* TIMEOUT\_ VALUE (4ms)。[#1445](https://github.com/ant-design-blazor/ant-design-blazor/pull/1445) [@zxyao145](https://github.com/zxyao145)
  - 🐞 修复 Dialog 关闭时不恢复显示滚动条的问题，为 Dialog 添加 Dispose。[#1379](https://github.com/ant-design-blazor/ant-design-blazor/pull/1379) [@zxyao145](https://github.com/zxyao145)

- Form

  - 🆕 使 Form 支持集合，Select 可绑定 Values[#1460](https://github.com/ant-design-blazor/ant-design-blazor/pull/1460) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复验证信息会重复的问题[#1391](https://github.com/ant-design-blazor/ant-design-blazor/pull/1391) [@ElderJames](https://github.com/ElderJames)

- Select

  - 🐞 修复在 SelectOption 中使用可空值类型时的错误。[#1451](https://github.com/ant-design-blazor/ant-design-blazor/pull/1451) [@anranruye](https://github.com/anranruye)
  - 🛠 使用 ResizeObserver 重构响应式时浏览器尺寸事件的订阅[#1392](https://github.com/ant-design-blazor/ant-design-blazor/pull/1392) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 当 DataSource 改变时触发 OnDataSourceChanged[#1419](https://github.com/ant-design-blazor/ant-design-blazor/pull/1419) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复值为枚举时的问题[#1368](https://github.com/ant-design-blazor/ant-design-blazor/pull/1368) [@anddrzejb](https://github.com/anddrzejb)

- 🆕 新增 Element 组件，用于动态渲染元素[#1378](https://github.com/ant-design-blazor/ant-design-blazor/pull/1378) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Checkbox 的 Value 在初始化时是阻塞[#1459](https://github.com/ant-design-blazor/ant-design-blazor/pull/1459) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 修复 InputNumber 按住时，离开组件还会自增的问题。[#1490](https://github.com/ant-design-blazor/ant-design-blazor/pull/1490) [@CAPCHIK](https://github.com/CAPCHIK)
- 🐞 修复 `Checkbox` and `Switch` 组件的 Value 和 Checked 绑定问题[#1394](https://github.com/ant-design-blazor/ant-design-blazor/pull/1394) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 修复 Tag 的 closable 拼写错误，和删除 Mode 属性[#1393](https://github.com/ant-design-blazor/ant-design-blazor/pull/1393) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 InputPassword 切换明文时，保持焦点和光标位置。[#1377](https://github.com/ant-design-blazor/ant-design-blazor/pull/1377) [@MihailsKuzmins](https://github.com/MihailsKuzmins)
- 🐞 修复 Affix 当 OffsetTop 为 0 时不能钉住的问题。[#1373](https://github.com/ant-design-blazor/ant-design-blazor/pull/1373) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 getDom 函数可能返回 null 的 bug。[#1417](https://github.com/ant-design-blazor/ant-design-blazor/pull/1417) [@zxyao145](https://github.com/zxyao145)
- 🐞 修复 IE 浏览器下拉选项宽度为 0 的问题。[#1469](https://github.com/ant-design-blazor/ant-design-blazor/pull/1469) [@anranruye](https://github.com/anranruye)

### 0.8.0

`2021-04-15`

- 主题和国际化

  - 🔥 文档支持主题色的动态切换[#1332](https://github.com/ant-design-blazor/ant-design-blazor/pull/1332) [@ElderJames](https://github.com/ElderJames)
  - 🔥 增加 RTL 切换。[#1238](https://github.com/ant-design-blazor/ant-design-blazor/pull/1238) [@ElderJames](https://github.com/ElderJames)
  - 🔥 加入内置主题样式。[#1286](https://github.com/ant-design-blazor/ant-design-blazor/pull/1286) [@ElderJames](https://github.com/ElderJames)

- Form

  - 📖 修改 IsModified 的实例。[#1344](https://github.com/ant-design-blazor/ant-design-blazor/pull/1344) [@anddrzejb](https://github.com/anddrzejb)
  - 🆕 增加 LabelAlign 属性，可使 Label 左对齐[#1292](https://github.com/ant-design-blazor/ant-design-blazor/pull/1292) [@unsung189](https://github.com/unsung189)

- Select

  - 🆕 增加 `MaxCountTag`, `MaxTagPlaceholder` 和 `MaxTagTextLenght`，以支持 Tag 模式的响应式处理。[#1338](https://github.com/ant-design-blazor/ant-design-blazor/pull/1338) [@anddrzejb](https://github.com/anddrzejb)
  - 🆕 增加 `PopupContainerGrowToMatchWidestItem` 和`PopupContainerMaxWidth` ，使下拉列表的宽度适应内容或 Input 的宽度[#1309](https://github.com/ant-design-blazor/ant-design-blazor/pull/1309) [@anddrzejb](https://github.com/anddrzejb)

- Table

  - 🔥 增加内置筛选器[#1267](https://github.com/ant-design-blazor/ant-design-blazor/pull/1267) [@YMohd](https://github.com/YMohd)
  - 🆕 支持 DisplayAttribute 特性指定列名[#1310](https://github.com/ant-design-blazor/ant-design-blazor/pull/1310) [@anranruye](https://github.com/anranruye)
  - 🆕 增加总结行。[#1218](https://github.com/ant-design-blazor/ant-design-blazor/pull/1218) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加 `OnExpand` 事件，可阻止展开或收起。[#1208](https://github.com/ant-design-blazor/ant-design-blazor/pull/1208) [@anddrzejb](https://github.com/anddrzejb)
  - 🆕 增加 `GetQueryModel` 方法。[#1202](https://github.com/ant-design-blazor/ant-design-blazor/pull/1202) [@ElderJames](https://github.com/ElderJames)

- Date Picker

  - 🐞 修复在输入后触发 OnChange 事件[#1347](https://github.com/ant-design-blazor/ant-design-blazor/pull/1347) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 resize 事件处理方法[#1322](https://github.com/ant-design-blazor/ant-design-blazor/pull/1322) [@anddrzejb](https://github.com/anddrzejb)

- 🆕 Space 增加 Wrap、Split 以及 Size 支持数组。[#1314](https://github.com/ant-design-blazor/ant-design-blazor/pull/1314) [@ElderJames](https://github.com/ElderJames)
- 🆕 Alert 增加 Message 模板，增加轮播公告示例[#1250](https://github.com/ant-design-blazor/ant-design-blazor/pull/1250) [@MutatePat](https://github.com/MutatePat)
- 🆕 Upload 增加 `OnDownload`, `BeforeAllUpload` `BeforeAllUploadAsync` 事件[#1302](https://github.com/ant-design-blazor/ant-design-blazor/pull/1302) [@anddrzejb](https://github.com/anddrzejb)
- 🆕 Tag 增加 `OnClosing` 事件，可阻止关闭。[#1268](https://github.com/ant-design-blazor/ant-design-blazor/pull/1268) [@TimChen44](https://github.com/TimChen44)
- 🆕 InputNumber 增加长按和键盘操作。[#1235](https://github.com/ant-design-blazor/ant-design-blazor/pull/1235) [@lingrepo](https://github.com/lingrepo)
- 🆕 增加单元测试辅助库 TestKit ，方便用户编写用例[#1248](https://github.com/ant-design-blazor/ant-design-blazor/pull/1248) [@MutatePat](https://github.com/MutatePat)
- 🆕 Input 增加 WrapperStyle 属性，可配置当有前缀、后缀时外部 span 的样式 [#1351](https://github.com/ant-design-blazor/ant-design-blazor/pull/1351) [@anddrzejb](https://github.com/anddrzejb)
- 🛠 统一 Modal、Comfirm 和 Drawer, 使用 FeedbackComponent 模板组件; 2. 增加纯事件处理 Helper，避免在事件中触发 StateHasChanged 导致重复渲染。[#1263](https://github.com/ant-design-blazor/ant-design-blazor/pull/1263) [@zxyao145](https://github.com/zxyao145)
- 🛠 Pagination 重构组件，完整实现功能 [#1220](https://github.com/ant-design-blazor/ant-design-blazor/pull/1220) [@Zonciu](https://github.com/Zonciu)
- 🐞 修复 JS 互操作的一些问题[#1342](https://github.com/ant-design-blazor/ant-design-blazor/pull/1342) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Affix 修复在目标容器中固定的问题[#1335](https://github.com/ant-design-blazor/ant-design-blazor/pull/1335) [@skystardust](https://github.com/skystardust)
- 🐞 Result 修复 动态切换图片的问题[#1336](https://github.com/ant-design-blazor/ant-design-blazor/pull/1336) [@JiaChengLuo](https://github.com/JiaChengLuo)
- 🐞 Drawer 修复 ZIndex 无效的问题 [#1362](https://github.com/ant-design-blazor/ant-design-blazor/pull/1362) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Avatar 修复 文本头像大小计算时的精度问题 [#1352] (https://github.com/ant-design-blazor/ant-design-blazor/pull/1352) [@anddrzejb](https://github.com/anddrzejb)

### 0.7.4

`2021-04-08`

- Table

  - 🐞 修复设置 ScrollX 时表格不重新渲染的问题。[#1311](https://github.com/ant-design-blazor/ant-design-blazor/pull/1311) [@Zonciu](https://github.com/Zonciu)
  - 🐞 修改修改 DataSource 会抛出异常的问题。[5b0dbfb](https://github.com/ant-design-blazor/ant-design-blazor/commit/5b0dbfb) [@Andrzej Bakun](https://github.com/Andrzej Bakun)
  - 🐞 修复 DataIndex 列过滤器无效的问题, 修复 DataIndex 列不刷新的问题。[#1295](https://github.com/ant-design-blazor/ant-design-blazor/pull/1295) [@Zonciu](https://github.com/Zonciu)
  - 🐞 ExpandIconColumnIndex 指定到 ActionColumn 时无效的问题。[#1285](https://github.com/ant-design-blazor/ant-design-blazor/pull/1285) [@Magehernan](https://github.com/Magehernan)
  - 🐞 优化性能并修复 DataSource 更新问题 [#1304](https://github.com/ant-design-blazor/ant-design-blazor/pull/1304) [@anddrzejb](https://github.com/anddrzejb)

- Select
  - 🐞 修复多选时点击关闭选项时，会触发下拉菜单的问题。[#1308](https://github.com/ant-design-blazor/ant-design-blazor/pull/1308) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 Tag 模式的 Loading 图标问题。[12ca2f7](https://github.com/ant-design-blazor/ant-design-blazor/commit/12ca2f7) [@Andrzej Bakun](https://github.com/Andrzej Bakun)
- 💄 修复 flex 和 wrap 的样式。[#1296](https://github.com/ant-design-blazor/ant-design-blazor/pull/1296) [@ElderJames](https://github.com/ElderJames)
- 🐞 使默认值为空字符串。[6944c13](https://github.com/ant-design-blazor/ant-design-blazor/commit/6944c13) [@Andrzej Bakun](https://github.com/Andrzej Bakun)
- 🐞 修复文件列表。[53c1285](https://github.com/ant-design-blazor/ant-design-blazor/commit/53c1285) [@Andrzej Bakun](https://github.com/Andrzej Bakun)
- 🐞 修复 DisabledDate 的问题。[#1298](https://github.com/ant-design-blazor/ant-design-blazor/pull/1298) [@mutouzdl](https://github.com/mutouzdl)
- 🆕 FormITem 增加 LabelTemplate 模板。[#1293](https://github.com/ant-design-blazor/ant-design-blazor/pull/1293) [@ldsenow](https://github.com/ldsenow)
- 🐞 修复当 Value 和 DefaultValue 同时设置时，Value 会被 DefaultValue 覆盖的问题。[5f14377](https://github.com/ant-design-blazor/ant-design-blazor/commit/5f14377) [@Andrzej Bakun](https://github.com/Andrzej Bakun)
- 📖 修复表格路由分页示例。[#1313](https://github.com/ant-design-blazor/ant-design-blazor/pull/1313) [@Zonciu](https://github.com/Zonciu)

## 0.7.3

`2021-03-29`

- 🐞 修复 Dropdown 下拉列表动画反向的问题。[#1274](https://github.com/ant-design-blazor/ant-design-blazor/pull/1274) [@mutouzdl](https://github.com/mutouzdl)
- 🐞 修正 Tree 组件节点无法展开的问题。[#1275](https://github.com/ant-design-blazor/ant-design-blazor/pull/1275) [@TimChen44](https://github.com/TimChen44)
- 💄 修复 Cascader 不能通过 Style 属性影响的样式的问题。[#1269](https://github.com/ant-design-blazor/ant-design-blazor/pull/1269) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 DatePicker [从结束日期面板选择的日期]、[季度面板的日期范围效果] 不正确。[#1260](https://github.com/ant-design-blazor/ant-design-blazor/pull/1260) [@mutouzdl](https://github.com/mutouzdl)
- 📖 增加 .NET Foundation 版权信息。[#1272](https://github.com/ant-design-blazor/ant-design-blazor/pull/1272) [@ElderJames](https://github.com/ElderJames)
- 📖 修复样式同步和 PR 预览的脚本。[68c7539](https://github.com/ant-design-blazor/ant-design-blazor/commit/68c7539) [@ElderJames](https://github.com/ElderJames)

## 0.7.2

`2021-03-14`

- Table

  - 🐞 修复 翻页时 `OnChange` 事件被触发两次。 [#1211](https://github.com/ant-design-blazor/ant-design-blazor/pull/1211) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 筛选展开后，点击外部时不能关闭的问题。[#1232](https://github.com/ant-design-blazor/ant-design-blazor/pull/1232) [@mutouzdl](https://github.com/mutouzdl)

- Select

  - 🐞 修复 使用绑定变量修改选中值时，当修改的值不在选项中时报异常的问题。 [#1209](https://github.com/ant-design-blazor/ant-design-blazor/pull/1209) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 表达式编译后调用 `ToString` 报 AmbigiousMethod 异常问题。 [#1214](https://github.com/ant-design-blazor/ant-design-blazor/pull/1214) [@anddrzejb](https://github.com/anddrzejb)

- 🐞 修复 Divider 样式总为 plain 的问题。 [#1215](https://github.com/ant-design-blazor/ant-design-blazor/pull/1215) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Upload 图片设置为 Disable 时删除按钮仍可点击的问题。 [#1219](https://github.com/ant-design-blazor/ant-design-blazor/pull/1219) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 修复 动画因样式同步后未及时修改，缺失前缀导致的失效问题。 [#1243](https://github.com/ant-design-blazor/ant-design-blazor/pull/1243) [@Zonciu](https://github.com/Zonciu)
- 🐞 修复 Progress 属性 TrailColor 不能设置未完整线段的背景色的问题 [#1241](https://github.com/ant-design-blazor/ant-design-blazor/pull/1241) [@NPadrutt](https://github.com/NPadrutt)
- 🐞 修复 Badge 属性 Color 的行为 [#1216](https://github.com/ant-design-blazor/ant-design-blazor/pull/1216) [@ElderJames](https://github.com/ElderJames)

## 0.7.1

`2021-03-05`

- 🐞 修复 Input Search 的加载动画。 [#1195](https://github.com/ant-design-blazor/ant-design-blazor/pull/1195) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 修复 Table 不能展开的问题 [#1199](https://github.com/ant-design-blazor/ant-design-blazor/pull/1199) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Table OnRowClick 事件 [#1200](https://github.com/ant-design-blazor/ant-design-blazor/pull/1200) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复 Select 在 Form 中按 Enter 键时会触发验证失败的问题。 [#1196](https://github.com/ant-design-blazor/ant-design-blazor/pull/1196) [@anddrzejb](https://github.com/anddrzejb)

## 0.7.0

`2021-03-02`

- 🔥 增加 overlay 的边界检测和方向调整。[#1109](https://github.com/ant-design-blazor/ant-design-blazor/pull/1109) [@mutouzdl](https://github.com/mutouzdl)
- 🐞 Tree: 修复 选中高亮问题。[#1161](https://github.com/ant-design-blazor/ant-design-blazor/pull/1161) [@qinhuaihe](https://github.com/qinhuaihe)
- 🐞 Grid: 修复 Gutter 问题。[#1158](https://github.com/ant-design-blazor/ant-design-blazor/pull/1158) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 List: 删除 SetGutterStyle 方法中的控制台输出。[#1159](https://github.com/ant-design-blazor/ant-design-blazor/pull/1159) [@superjerry88](https://github.com/superjerry88)
- 🐞 docs: 修复 锚点异常。[#1107](https://github.com/ant-design-blazor/ant-design-blazor/pull/1107) [@ElderJames](https://github.com/ElderJames)

- Select:

  - 🔥 使用 Func 代替反射读写数据。[#1168](https://github.com/ant-design-blazor/ant-design-blazor/pull/1168) [@Zonciu](https://github.com/Zonciu)
  - 🐞 修复 双向绑定问题 [#1191](https://github.com/ant-design-blazor/ant-design-blazor/pull/1191) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 多选模式选项重复问题。[#1162](https://github.com/ant-design-blazor/ant-design-blazor/pull/1162) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 标签异常。[#1121](https://github.com/ant-design-blazor/ant-design-blazor/pull/1121) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 DefaultActiveFirstOption 属性。[#1115](https://github.com/ant-design-blazor/ant-design-blazor/pull/1115) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 删除 `AllowCustomTags` 和 `OnCreateCustomTag`属性，增加 `PrefixIcon`。[#1087](https://github.com/ant-design-blazor/ant-design-blazor/pull/1087) [@anddrzejb](https://github.com/anddrzejb)

- Table

  - 🔥 增加 列表筛选支持。[#1178](https://github.com/ant-design-blazor/ant-design-blazor/pull/1178) [@ElderJames](https://github.com/ElderJames)
  - 🔥 增加 单元格编辑和行编辑的 demo。[#1152](https://github.com/ant-design-blazor/ant-design-blazor/pull/1152) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加 OnRow 和 OnRow 方法。 [#1175](https://github.com/ant-design-blazor/ant-design-blazor/pull/1175) [@qinhuaihe](https://github.com/qinhuaihe)
  - 🆕 ScrollX/ScrollY 增加更多长度单位的支持。[#1137](https://github.com/ant-design-blazor/ant-design-blazor/pull/1137) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 selectedRows 异常。[#1148](https://github.com/ant-design-blazor/ant-design-blazor/pull/1148) [@qinhuaihe](https://github.com/qinhuaihe)
  - 🐞 修复 SortModel 中丢失的 Sort 属性值。[#1105](https://github.com/ant-design-blazor/ant-design-blazor/pull/1105) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 空数据时的空状态显示问题。 [#1180](https://github.com/ant-design-blazor/ant-design-blazor/pull/1180) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 树形数据 GetChildren 返回 Null 时的异常问题。[#1188](https://github.com/ant-design-blazor/ant-design-blazor/pull/1188) [@ElderJames](https://github.com/ElderJames)

- DatePicker

  - 🆕 绑定 非可空类型时，点清除键会设置为原值。[#1100](https://github.com/ant-design-blazor/ant-design-blazor/pull/1100) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 date typing, enter behavior, overlay toggle。[#1145](https://github.com/ant-design-blazor/ant-design-blazor/pull/1145) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 错误的日期格式。[#1097](https://github.com/ant-design-blazor/ant-design-blazor/pull/1097) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 每周的第一天配置。[#1054](https://github.com/ant-design-blazor/ ant-design-blazor/pull/1054) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 placeholder 和 value 等于 null 时报错的问题。[#1088](https://github.com/ant-design-blazor/ant-design-blazor/pull/1088) [@anddrzejb](https://github.com/anddrzejb)

- Steps

  - 🐞 修复 进度条样式。[#1072](https://github.com/ant-design-blazor/ant-design-blazor/pull/1072) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 导航问题。[#1071](https://github.com/ant-design-blazor/ant-design-blazor/pull/1071) [@Tfurrer](https://github.com/Tfurrer)

- Menu

  - 🆕 增加 tooltip 和 submenu 浮层弹出触发类型。[#1082](https://github.com/ant-design-blazor/ant-design-blazor/pull/1082) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加 inline indent 属性。[#1076](https://github.com/ant-design-blazor/ant-design-blazor/pull/1076) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 导航菜单折叠无响应。[#1144](https://github.com/ant-design-blazor/ant-design-blazor/pull/1144) [@mutouzdl](https://github.com/mutouzdl)
  - 🐞 修复 匹配 routerlink 时激活父级菜单。[#1134](https://github.com/ant-design-blazor/ant-design-blazor/pull/1134) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 菜单不能跟随 Sider 侧边栏收起的问题。[#1069](https://github.com/ant-design-blazor/ant-design-blazor/pull/1069) [@ElderJames](https://github.com/ElderJames)

- Cascader

  - 🔥 增加 下拉效果（集成 Overlay 组件）。[#1112](https://github.com/ant-design-blazor/ant-design-blazor/pull/1112) [@mutouzdl](https://github.com/mutouzdl)
  - 🐞 修复 `OnChange` 调用了两遍的问题。[#1151](https://github.com/ant-design-blazor/ant-design-blazor/pull/1151) [@anddrzejb](https://github.com/anddrzejb)

- Input
  - 🐞 修复 InputPassword 的焦点。[#1146](https://github.com/ant-design-blazor/ant-design-blazor/pull/1146) [@anddrzejb](https://github.com/anddrzejb)
  - 🚫 修复 按 Enter 键时不立即更新绑定值的问题。[#1094](https://github.com/ant-design-blazor/ant-design-blazor/pull/1094) [@Hona](https://github.com/Hona)

## 0.6.0

`2021-02-01`

- Table

  - 🆕 增加 DataIndex 特性，基于路径字符串的对象属性访问。[#1056](https://github.com/ant-design-blazor/ant-design-blazor/pull/1056) [@Zonciu](https://github.com/Zonciu)
  - 🆕 增加 RowClassName 属性[#1031](https://github.com/ant-design-blazor/ant-design-blazor/pull/1031) [@mostrowski123](https://github.com/mostrowski123)
  - 🆕 支持设置排序方向以及默认排序。[#778](https://github.com/ant-design-blazor/ant-design-blazor/pull/778) [@cqgis](https://github.com/cqgis)
  - 🆕 支持多列排序。[#1019](https://github.com/ant-design-blazor/ant-design-blazor/pull/1019) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加属性 ExpandIconColumnIndex ，可指定展开按钮所在列。[#1002](https://github.com/ant-design-blazor/ant-design-blazor/pull/1002) [@fan0217](https://github.com/fan0217)
  - 🐞 设置 ScrollY 时行选择抛异常。[#1020](https://github.com/ant-design-blazor/ant-design-blazor/pull/1020) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 ExpandTemplate 为 null 时，空数据时的样式错误。[#985](https://github.com/ant-design-blazor/ant-design-blazor/pull/985) [@Magehernan](https://github.com/Magehernan)
  - 🐞 表格组件添加自定义比较器, 修复表格复刻例子。[#969](https://github.com/ant-design-blazor/ant-design-blazor/pull/969) [@Zonciu](https://github.com/Zonciu)
  - 🐞 修复在页面重载时抛出的异常。[#1040](https://github.com/ant-design-blazor/ant-design-blazor/pull/1040) [@anddrzejb](https://github.com/anddrzejb)

- Menu

  - 🐞 修复相同链接的死循环以及重复高亮[#1027](https://github.com/ant-design-blazor/ant-design-blazor/pull/1027) [@anddrzejb](https://github.com/anddrzejb)
  - 🆕 增加菜单分割线组件 MenuDivider。[#1017](https://github.com/ant-design-blazor/ant-design-blazor/pull/1017) [@anddrzejb](https://github.com/anddrzejb)

- Overlay

  - 🆕 弹出层支持无须 div 包裹触发元素的实现方式，但需要使用<Unbound> 模板和使用 RefBack 方法。[#937](https://github.com/ant-design-blazor/ant-design-blazor/pull/937) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 delElementFrom()在页面重载时的异常。[#1008](https://github.com/ant-design-blazor/ant-design-blazor/pull/1008) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 getFirstChildDomInfo 方法非空判断[#989](https://github.com/ant-design-blazor/ant-design-blazor/pull/989) [@Andrzej Bakun](https://github.com/Andrzej Bakun)

- DatePicker

  - 🐞 防止时间超出 DateTime 范围，导致异常[#973](https://github.com/ant-design-blazor/ant-design-blazor/pull/973) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 DatePicker 当有默认值时抛出异常[#972](https://github.com/ant-design-blazor/ant-design-blazor/pull/972) [@ElderJames](https://github.com/ElderJames)

- 🆕 增加 Image 组件[#1038](https://github.com/ant-design-blazor/ant-design-blazor/pull/1038) [@ElderJames](https://github.com/ElderJames)
- 🆕 Card 增加操作按钮组件`CardAction`，可分别设置点击事件。[#1030](https://github.com/ant-design-blazor/ant-design-blazor/pull/1030) [@ElderJames](https://github.com/ElderJames)
- 🆕 Icon 增加静态的图标类型 `IconType`。[#987](https://github.com/ant-design-blazor/ant-design-blazor/pull/987) [@porkopek](https://github.com/porkopek)
- 🐞 修复 Input/InputNumber/TextArea 丢失的 disabled 属性。[#1048](https://github.com/ant-design-blazor/ant-design-blazor/pull/1048) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Form 修复重新绑定 model 或在调用 `Reset()` 方法不能清空验证错误信息的问题[#1035](https://github.com/ant-design-blazor/ant-design-blazor/pull/1035) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Tabs 修复选中指示器的跳动问题。[#1037](https://github.com/ant-design-blazor/ant-design-blazor/pull/1037) [@ElderJames](https://github.com/ElderJames)
- 🐞 Layout 修复 Sider 在 zero-width 模式时按钮丢失的问题[#1007](https://github.com/ant-design-blazor/ant-design-blazor/pull/1007) [@ElderJames](https://github.com/ElderJames)
- 💄 BackTop 修复可见/隐藏的样式[#1005](https://github.com/ant-design-blazor/ant-design-blazor/pull/1005) [@ElderJames](https://github.com/ElderJames)
- 💄 Upload 修复文件列表的样式[#1001](https://github.com/ant-design-blazor/ant-design-blazor/pull/1001) [@ElderJames](https://github.com/ElderJames)
- 🐞 Calendar 修复关于 ChangePickerValue 的错误[#993](https://github.com/ant-design-blazor/ant-design-blazor/pull/993) [@anddrzejb](https://github.com/anddrzejb)
- 💄 Alert 修复丢失 html 结构导致的样式问题[#990](https://github.com/ant-design-blazor/ant-design-blazor/pull/990) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Cascader 当 options 更新时重新初始化[#980](https://github.com/ant-design-blazor/ant-design-blazor/pull/980) [@imhmao](https://github.com/imhmao)
- 📖 发布文档时按版本号获取静态资源，使缓存更新。[cf2d4ed](https://github.com/ant-design-blazor/ant-design-blazor/commit/cf2d4ed) [@ElderJames](https://github.com/ElderJames)
- 💄 同步 ant-design-blazor v4.11.1 样式。[#1039](https://github.com/ant-design-blazor/ant-design-blazor/pull/1039) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复在多个 Modal 同时存在时下拉选择等弹出组件无法弹出的问题。[#1012](https://github.com/ant-design-blazor/ant-design-blazor/pull/1012) [@mutouzdl](https://github.com/mutouzdl)
- 🛠 更新 bUnit 版本到 1.0.0-preview-01。[#1009](https://github.com/ant-design-blazor/ant-design-blazor/pull/1009) [@anddrzejb](https://github.com/anddrzejb)
- 📖 加载后自动滚动到 Url 锚点[#1006](https://github.com/ant-design-blazor/ant-design-blazor/pull/1006) [@ElderJames](https://github.com/ElderJames)
