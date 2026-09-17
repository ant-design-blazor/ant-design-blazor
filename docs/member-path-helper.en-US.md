---
order: 5 
title: Member path helper
---

Provides `PathHelper` to read and write object member value via member path string.
* Support for Property and Field, get operations and assignment operations.
* <span style="font-weight: bold;">⚠ Breaking change: Since double quotes need to be escaped when used in strings, now use single quotes for string index keys instead.</span>

## Supported operations

### 1. Accessing descendant properties

Example: `obj.PathGet("A.B.C")`.

### 2. Array pattern indexing, and `List-like` types that implement the `Count` property and the `get_Item(int) (i.e. this[int])` method

Example: `obj.PathGet<string>("A.B[1].C")`.

### 3. Dictionary pattern indexing, and `Dictionary-like` types that implement the `ContainsKey` method and the `get_Item (i.e. this[key])` method.

Example：`obj.PathGetDefault<int?>("A.B['test'].C")`, returns null if the object does not exist.
Example：`obj.PathGetDefault<int>("A.B['test'].C")`, Returns default(int) if the object does not exist.

### 4. Arrays, dictionary nesting

Example: 

`obj.PathGet("A.B['test'][3].C")`，

`obj.PathGet("A.B[1][5].C")`，

`obj.PathGet("A.B[1]['user id'].C")`.

### 5. Member assignment

<span style="font-weight: bold;">⚠ Notice: When using the assignment operation, the path only supports `class type property`, `class type field`, `value type fields`, if there are `value type property`, the assignment is not possible, and the execution results in unchanged member values.</span>

Example: `obj.PathSet("A.B['test']", "test value")`, then `obj.PathGet("A.B['test']") == "test value"` is `true`
Example: `obj.PathSet("A.C", "abcde")`, then `obj.PathGet<string>("A.C") == "abcde"` is `true`

### 6. Not null and Nullable mode

#### 6.1. Not null mode

<span style="font-weight: bold;">⚠ Notice: Non-null mode requires the developer to ensure that the properties on the property path are not null, and if array or dictionary pattern is included, to also ensure that the indexed object must exist, otherwise an exception will be thrown. </span>.

When the result data type is a value type and not Nullable (such as int), a direct access expression is generated, and if there is a Nullable type in the property path, it will be accessed directly without doing a null check.

For example, when accessing the property `A.B.C`, where `B` is `Nullable<MyStruct>`, an expression like `A.B!.Value.C`

For array pattern or dictionary pattern, it is accessed directly, e.g., when accessing the array object property `A.B[i].C`, `i < B.Count && i >= 0` is not checked. When accessing the dictionary object property `A.D["my data"].C`, it will not check if `D["my data"]` exists.

#### 6.2. Nullable mode

<span style="font-weight: bold;">Nullable mode does not require a guarantee that the data is not null, nor does it require a guarantee that array pattern or dictionary pattern must have a value, return null when accessing the property of a non-existent object.</span>.

When the result data type is a Nullable value type or class (such as int?, string), a conditional expression is generated, and if there is a Nullable or class type in the property path, it will check for non-null before accessing, and will return null when it encounters a null object. 

For example, accessing the property `A.B.C`, where `B` is `Nullable<MyStruct>`, will generate an expression like `A.B.HasValue ? A.B.Value.C : null`.

For array pattern or dictionary pattern, it will check before accessing, e.g. when accessing the array object property `A.B[i].C`, it will check `i < B.Count && i >= 0` , and return `default(T)` if the check result is `false`. When accessing the dictionary object property `A.D["my data"].C`, it checks `D.ContainsKey("my data")`, and returns `default(T)` if the check result is `false`.

## API

There are three types of generic method.
1. `<T,V>` : For scenarios where both item and return value (assignment)'s generic parameters are available.
2. `<object, V>` : For scenarios where item's generic parameter is available, but return value (assignment)'s generic parameter.
3. `<object, object>` : For scenarios where both item and return value (assignment)'s generic parameters are not available.

The above generic methods are extended from the non-generic methods.


| Method                          | Description                                                                                                                        |
| ------------------------------- | :--------------------------------------------------------------------------------------------------------------------------------- |
| PathExtensions.PathGet          | Accesses the member value of the object.                                                                                           |
| PathExtensions.PathGetOrDefault | Accesses a member of the object, checks if the member is valid before accessing it, and returns the default value if it is invalid |
| PathExtensions.PathSet          | Assign a value to a member of the object.                                                                                          |
| PathHelper.GetDelegate          | Get the delegate method of the object member.                                                                                      |
| GetDelegateDefault              | Get the nullable delegate method of an object member                                                                               |
| GetLambda                       | Get the object member's lambda expression                                                                                          |
| GetLambdaDefault                | Get the object member's nullable lambda expression                                                                                 |
| SetDelegate                     | Get the delegate method of an object member's assignment                                                                           |
| SetLambda                       | Get the object member's assignment lambda expression                                                                                 |

