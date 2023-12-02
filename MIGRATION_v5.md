# Antd v5 样式迁移
Antd v5 样式渲染方式有了根本性变化，首先是移除了所有less文件，并将less文件中的样式使用ts重写。而TS要把样式代码转成css样式，使用了cssinjs的技术方案。作为blzor技术方案，使用CssInCsharp方案来替代。

## CssInCsharp
CssInCsharp技术包含三部分内容如下：
1. CSSObject 用于构建样式的基本对象，其实现思路来源于[csstype](https://github.com/frenic/csstype)。
2. CSSCompiler 一个C#版本的高效率样式转换器，用于在运行时期间将CSSObject转换成标准css样式表
3. Style组件，一组blazor控件可将样式内容输出到指定位置(包含StyleOutlet, StyleContent, Style等)

## 疑惑解答
### 1、C#写样式相比less有什么优点
* 能够使用C#语言所有特性，不需要额外学习less或sass语法，会C#就能写样式。
* 能够和现有组件代码互通，样式和控件可以共享变量方法等。可以彻底摆脱样式代码不可控的问题。保证了代码的一致性和可维护性。

### 2、运行时和编译哪个效率高
由于CssInCsharp本身是在运行时生成样式，所以相比之前less编译会存在一定的性能损耗。但是运行时样式是按需生成的，所以比起less需要加载整个样式表，CssInCsharp仅加载当前页需要的样式表，相对来说会小很多，加载效率会变高。

### 3、SSR是否会无效
由于样式在运行时按需生成，因此当页面发生切换时，可能存在新页面样式不完整的情况，目前还未验证。针对此也有一些解决方案就是采用预渲染模式，提前将所有样式表渲染好，页面初次加载时就将所有样式加载进去。其等同于直接使用样式文件。

## 迁移工具
为了提升效率，也准备了迁移工具，代码位于该路径下(./scripts/migrator)。  
```bash
# 迁移指令(具体使用请查看迁移步骤)
npm run migrate:v5
```

**注意: 该工具还未完全实现所有样式迁移，目前迁移后代码会存在一定问题，需要手动调整，并不能完全自动迁移，具体可以参考该工具的使用文档。**

### 简述下迁移工具技术方案
迁移工具使用的AST(可以理解为TS版的source generator)，利用AST读取所有样式的ts代码并解析代码结构提取出其中的样式内容转成C#代码。但是由于ts和C#代码结构上差异较大，并不是所有ts代码都能转成C#代码，因此需要人工进行审查结果。或者进一步完善此转换逻辑，在设计时只考虑了一些通用性转换逻辑，因此比较局限，可以在此基础上做一些定制性的调整。

## 迁移步骤
以迁移button组件为案例简述下迁移过程。
### 1、使用迁移工具生成初始代码
打开./scripts/migrator/Components.ts文件，填下如下代码：
```ts
{
    name: 'Button',                         // 组件名称
    src: [
        'components/button/style/index.ts', // 样式代码
        'components/button/style/group.ts'  // 可以包含多个
    ],
    dist: 'components/button/Style.cs',     // 目标cs代码文件(注意多个文件最终生成时会生成到一个文件中)
    csOptions: {
        ...defaultOptions,                  // 默认配置包含命名空间和引用
        defaultClass: 'Button',             // 生成的类的名称(默认找不到类名时会用此名称替代)
        typeMap: [                          // 生成代码时会有未能识别的类型，工具默认用Unknown占位，可以在此处，配置转换规则
            // 例如：可以将生成过程中Unknown1 转成 ButtonToken
            { from: 'Unknown1', to: 'ButtonToken', includes: [1] },
        ],
        transforms: [                       // 生成目标文件后需要对里面内容进行处理，可以用此配置
            // 例如：把cs代码中的class Button替换成partial class Button
            { source: 'class Button', target: 'partial class Button' }, 
        ]
    }
},
```

### 2、执行生成并完善生成代码
```bash
npm run migrate:v5
```
生成的代码会存在无法编译通过的内容，需要进一步完善。可以通过修改typeMap和transforms的配置，直到目标文件基本生成正确即可。

### 3、组件代码中添加样式Token
打开Button.razor.cs文件，增加成员变量如下：
```cs
// 当前控件的前缀
private const string PrefixCls = "ant-btn";
// 样式token
private static readonly TokenWithCommonCls _token = new() { PrefixCls = PrefixCls, ComponentCls = $".{PrefixCls}" };
```

在SetClassMap中添加样式的hashId
```cs
var prefixName = _token.PrefixCls;

ClassMapper.Clear()
    .Add(prefixName)
    .Add(_token.GetHashId()) // 添加hashId
```

### 4、增加样式渲染
打开Button.razor文件，新增代码如下：
```html
<StyleContent>
    @UseStyle(_token)
</StyleContent>
```
其中UseStyle方式是步骤2中生成的样式文件总调用方法。  


至此组件样式代码即迁移完成。

## 运行与测试
创建新工程引入AntDesign包，StyleContent里的样式是生成到head区域。因此需要对head区域进行设置。  

(1) 导入命名空间  
打开_Imports.razor
```cs
@using AntDesign
@using CssInCsharp
```

(2) 替换默认head根组件  
如果是wasm项目，则打开Program.cs
```cs
// 把这行代码替换成下面的代码
builder.RootComponents.Add<HeadOutlet>("head::after");
// 替换成
builder.RootComponents.Add<StyleOutlet>("head::after");
```
注意：**一定要替换，因为不能同时有两个head::after的根组件，但是blazor默认的HeadOutlet不支持StyleContent**    


说明: HeadOutlet是.net6版本才开始支持的，所以.net6以下版本不支持HeadOutlet，StyleOutlet是基于HeadOutlet扩展了StyleContent有关组件。.net6版本的HeadOutlet默认会把内容追加到head区域末尾，这导致生成样式会覆盖用户自己link的样式，.net8版本的HeadOutlet才能正常生成在head区域最开始。请先用.net8版本测试，后续CssInCsharp库会想办法解决这个问题。

## V5样式实现思路
### 误区
v5的样式虽然是运行时生成，但本质是把less内容先转成c#代码，运行时再按需加载到head里。其内容和less并无本质区别。因此样式不是依据组件实例来区分的。也就是一个Button组件在运行时可以创建多个实例，虽然它的样式生成方法会被调用多次，但生成的样式表只有一份，样式是依据组件类型来生成，而不是组件实例，这个是一个误区。

### 样式结构
以下是CssInCsharp生成的样式结果
```css
<style 
    data-css-hash="" 
    data-token-hash="1iw360o" 
    data-cache-path="1iw360o|Button-Button|ant-btn|anticon">
    /*
     * content
     */
</style>
```
参数说明：
| 参数       | 说明          |
| ---------- | ------------- |
| css-hash   | css样式hash值 |
| token-hash | token的hanh值 |
| cache-path | 缓存路径      |

* css-hash  
是由CssInCharp库最后生成样式时计算得到，用户不用关注。

* token-hash  
由用户传递，是基于token所有key和value计算的到的，目前放在GlobalToken类里，此类包含样式初始化所有的变量表。一般这个变量表需要在AddDesign()方法中进行初始化，需要使用主题来赋值，但是目前为了简化，还没有写成主题代码，所以先全部放在GlobalToken类里了。该hash值还用于为样式表创建:where()的伪类，来防止样式冲突。

* cache-path  
由用户传递，样式的唯一路径，Button组件在实例化时用户可能会创建非常多的实例，为了防止生成大量的重复样式，使用cache-path来避免。  
组件在调用创建样式对象方法时会先检查缓存，该类型的组件样式是否已经生成了，如果已经生成则忽略，否则会调用样式创建方法构建样式，并编译成样式表，加入全局缓存。

### 调用过程
```<StyleContent>@UseStyle(_token)</StyleContent>```被执行时，会调用UseStyle()方法，UseStyle调用GenComponentStyleHook()，返回值是RenderFragment里面包含了多个```<Style>```组件。GenComponentStyleHook只是用来创建Style组件。(这个目前可以优化，可以防止创建重复的Style组件，可以避免不必要的开销)。  

当StyleContent渲染每个Style组件时，Style组件会检查当前缓存路径，如果缓存中已经存在当前样式表，则跳过渲染，如果不存在则调用样式创建方法创建样式表并加入全局缓存，最后输出到head里。
