---
order: 5 
title: 路径式属性访问
---

提供 `PropertyAccessHelper` 以实现通过属性路径字符串来访问对象的属性及其后代属性。

## 支持的操作

### 1. 访问后代属性

例：`"A.B.C"`。

### 2. 数组模式索引，以及实现了 `Count` 属性和 `get_Item(int) (即 this[int]` 方法的 `类似 List 的` 类型

例：`"A.B[1].C"`。

### 3. 字典模式索引，以及实现了 `ContainsKey` 方法和 `get_Item (即 this[key])` 方法的 `类似 Dictionary 的` 类型。

例：`"A.B[\"test\"].C"`。

### 4. 数组、字典嵌套

例：`"A.B[\"test\"][3].C"`，`"A.B[1][5].C"`，`"A.B[1][\"user id\"].C"`。

### 5. 非空模式和可空模式

#### 5.1. 非空模式

<span style="font-weight: bold;">⚠注意：非空模式下需要开发者保证属性路径上的属性不为null，如果包含数组模式或字典模式，还需要保证索引对象必须存在，否则访问不存在的对象的属性时会抛出异常。</span>

当结果数据类型是值类型且不是Nullable时(如int)，会生成直接访问表达式，如果属性路径中存在Nullable类型，会不做null检查直接访问。

如：访问属性`A.B.C` 时，其中 `B` 是 `Nullable<MyStruct>`，会生成类似 `A.B!.Value.C` 的表达式。

对于数组或字典，会直接访问，如：访问数组对象属性 `A.B[i].C` 时，不会检查 `B.Count > i && i > 0` 。访问字典对象属性 `A.D["my data"].C` 时，不会检查 `D.ContainsKey("my data")`。

#### 5.2. 可空模式

<span style="font-weight: bold;">可空模式下不需要保证数据不为null，也不用保证数组模式或字典模式必定有值，访问到不存在对象的属性时会返回null。</span>

当结果数据类型是Nullable值类型或class时(如int?, string)，会生成条件表达式，如果属性路径中存在Nullable或class类型，会先检查非null再访问，遇到null对象会返回null。

如：访问属性 `A.B.C` 时，其中 `B` 是 `Nullable<MyStruct>`，会生成类似 `A.B.HasValue ? A.B.Value.C : null` 的表达式。

对于数组或字典，会先检查再访问，如：访问数组对象属性 `A.B[i].C` 时，会检查 `B.Count > i && i > 0` , 结果是 `false` 时返回 `null` 。访问字典对象属性 `A.D["my data"].C` 时，会检查 `D.ContainsKey("my data")`, 结果是 `false` 时返回 `null` 。

## API

| 方法                                        | 说明                                                                                     |
| ------------------------------------------- | :-------------------------------------------------------------------------------------- |
| BuildAccessPropertyLambdaExpression         | 创建非空属性访问Lambda表达式, 调用Compile()后即可使用                                         |
| BuildAccessNullablePropertyLambdaExpression | 创建可空属性访问Lambda表达式, 调用Compile()后即可使用                                         |
| AccessProperty                              | 生成非空属性访问链表达式, 需要串联ToXXX方法创建最终表达式                                        |
| AccessNullableProperty                      | 生成可空属性访问链表达式，需要串联ToXXX方法创建最终表达式                                         |
| AccessPropertyDefaultIfNull                 | 生成DefaultIfNull属性表达式，当属性访问结果是null时，使用传入的默认值，需要串联ToXXX方法创建最终表达式 |
| ToDelegate                                  | 从AccessXXX方法创建委托方法                                                               |
| ToLambdaExpression                          | 从AccessXXX方法创建Lambda表达式                                                           |
| ToFuncExpression                            | 从AccessXXX方法创建Func<,>委托方法                                                         |
