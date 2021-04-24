---
order: 5 
title: 成员路径助手
---

提供 `PathHelper` 通过成员路径字符串来读写对象成员值。
* 支持Property和Field，支持取值操作和赋值操作。
* <span style="font-weight: bold;">⚠ 重要变化：因为字符串中使用双引号时需要转义，现在改为使用单引号来表示字符串索引键。</span>

## 支持的操作

### 1. 访问后代成员

例：`obj.PathGet("A.B.C")`。

### 2. 数组模式索引，以及实现了 `Count` 属性和 `get_Item(int) (即 this[int]` 方法的 `类似 List 的` 类型

例：`obj.PathGet<string>("A.B[1].C")`。

### 3. 字典模式索引，以及实现了 `ContainsKey` 方法和 `get_Item (即 this[key])` 方法的 `类似 Dictionary 的` 类型。

例：`obj.PathGetDefault<int?>("A.B['test'].C")`。 对象不存在时返回 null
例：`obj.PathGetDefault<int>("A.B['test'].C")`。 对象不存在时返回 default(int)

### 4. 数组、字典嵌套

例：`obj.PathGet("A.B['test'][3].C")`，`obj.PathGet("A.B[1][5].C")`，`obj.PathGet("A.B[1]['user id'].C")`。

### 5. 成员赋值

<span style="font-weight: bold;">⚠ 注意：使用赋值操作时，路径只支持`类属性`、`类字段`、`值类型字段`，如果存在`值类型属性`，则无法赋值，执行结果是原值保持不变。</span>

例: `obj.PathSet("A.B['test']", "test value")`, 则 `obj.PathGet("A.B['test']") == "test value"` 为 `true`
例: `obj.PathSet("A.C", "abcde")`, 则 `obj.PathGet<string>("A.C") == "abcde"` 为 `true`

### 6. 非空模式和可空模式

#### 6.1. 非空模式

<span style="font-weight: bold;">⚠ 注意：非空模式下需要开发者保证属性路径上的属性不为null，如果包含数组模式或字典模式，还需要保证索引对象必须存在，否则访问不存在的对象的属性时会抛出异常。</span>

当结果数据类型是值类型且不是Nullable时(如int)，会生成直接访问表达式，如果属性路径中存在Nullable类型，会不做null检查直接访问。

如：访问属性`A.B.C` 时，其中 `B` 是 `Nullable<MyStruct>`，会生成类似 `A.B!.Value.C` 的表达式。

对于数组或字典，会直接访问，如：访问数组对象属性 `A.B[i].C` 时，不会检查 `B[i]` 是否存在。访问字典对象属性 `A.D["my data"].C` 时，不会检查 `D["my data"]` 是否存在。

#### 6.2. 可空模式

<span style="font-weight: bold;">可空模式下不需要保证数据不为null，也不用保证数组模式或字典模式必定有值，访问到不存在对象的属性时会返回null。</span>

当结果数据类型是Nullable值类型或class时(如int?, string)，会生成条件表达式，如果属性路径中存在Nullable或class类型，会先检查非null再访问，遇到null对象会返回null。

如：访问属性 `A.B.C` 时，其中 `B` 是 `Nullable<MyStruct>`，会生成类似 `if(A != null && A.B.HasValue)` 的检查表达式。

对于数组或字典，会先检查再访问，如：访问数组对象属性 `A.B[i].C` 时，会检查 `i < B.Count && i >= 0` , 结果是 `false` 时返回 `default(T)`。访问字典对象属性 `A.D["my data"].C` 时，会检查 `D.ContainsKey("my data")`, 结果是 `false` 时返回 `default(T)` 。

## API

三种泛型用法：
1. `<T,V>` : 用于对象和返回值(赋值)都可以得到泛型参数的场景。
2. `<object, V>` : 用于可得到对象泛型参数，得不到返回值(赋值)泛型参数明确的场景。
3. `<object, object>` : 用于对象和返回值(赋值)泛型参数都无法获取的场景。

以上泛型方法都是从非泛型方法扩展而来。


| 方法                            | 说明                                                              |
| ------------------------------- | :---------------------------------------------------------------- |
| PathExtensions.PathGet          | 访问对象的成员值                                                  |
| PathExtensions.PathGetOrDefault | 访问对象的成员值, 访问前会检查成员值是否有效，无效时返回default值 |
| PathExtensions.PathSet          | 对对象的成员赋值                                                  |
| PathHelper.GetDelegate          | 获取对象成员的取值委托方法                                        |
| PathHelper.GetDelegateDefault   | 获取对象成员的可空取值委托方法                                    |
| PathHelper.GetLambda            | 获取对象成员的取值Lambda表达式                                      |
| PathHelper.GetLambdaDefault     | 获取对象成员的可空取值Lambda表达式                                  |
| PathHelper.SetDelegate          | 获取对象成员的赋值委托方法                                        |
| PathHelper.SetLambda            | 获取对象成员的赋值Lambda表达式                                      |

