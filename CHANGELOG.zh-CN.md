---
order: 6
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
- 🐞 RouterMatch 默认值改为 All[d8352b8](https://github.com/ant-design-blazor/ant-design-blazor/commit/d8352b8) [@James Yeung](https://github.com/James Yeung)
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
