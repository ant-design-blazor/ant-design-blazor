---
order: 5 
title: Path-based property access
---

Provides `PropertyAccessHelper` to access to an object's properties and their descendants via a property path string.

## Supported operations

### 1. Accessing descendant properties

Example: `"A.B.C"`.

### 2. Array pattern indexing, and `List-like` types that implement the `Count` property and the `get_Item(int) (i.e. this[int])` method

Example: `"A.B[1].C"`, `A.B[1][5].C`.

### 3. Dictionary pattern indexing, and `Dictionary-like` types that implement the `ContainsKey` method and the `get_Item (i.e. this[key])` method.

Example: `"A.B[\"test\"].C"`, `"A.B[\"test\"][3].C"`.

### 4. Arrays, dictionary nesting

Example: `"A.B[\"test\"][3].C"`，`"A.B[1][5].C"`，`"A.B[1][\"user id\"].C"`.

### 5. Not null and Nullable mode

#### 5.1. Not null mode

<span style="font-weight: bold;">⚠Notice: Non-null mode requires the developer to ensure that the properties on the property path are not null, and if array or dictionary pattern is included, to also ensure that the indexed object must exist, otherwise an exception will be thrown. </span>.

When the result data type is a value type and not Nullable (such as int), a direct access expression is generated, and if there is a Nullable type in the property path, it will be accessed directly without doing a null check.

For example, when accessing the property `A.B.C`, where `B` is `Nullable<MyStruct>`, an expression like `A.B!.Value.C`

For array pattern or dictionary pattern, it is accessed directly, e.g., when accessing the array object property `A.B[i].C`, `B.Count > i && i > 0` is not checked. When accessing the dictionary object property `A.D["my data"].C`, `D.ContainsKey("my data")` is not checked.

#### 5.2. Nullable mode

<span style="font-weight: bold;">Nullable mode does not require a guarantee that the data is not null, nor does it require a guarantee that array pattern or dictionary pattern must have a value, return null when accessing the property of a non-existent object.</span>.

When the result data type is a Nullable value type or class (such as int?, string), a conditional expression is generated, and if there is a Nullable or class type in the property path, it will check for non-null before accessing, and will return null when it encounters a null object. 

For example, accessing the property `A.B.C`, where `B` is `Nullable<MyStruct>`, will generate an expression like `A.B.HasValue ? A.B.Value.C : null`.

For array pattern or dictionary pattern, it will check before accessing, e.g. when accessing the array object property `A.B[i].C`, it will check `B.Count > i && i > 0` , and return `null` if the check result is `false`. When accessing the dictionary object property `A.D["my data"].C`, it checks `D.ContainsKey("my data")`, and returns `null` if the check result is `false`.

## API

| Method                                      | Description                                                                                                                                                                                          |
| ------------------------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| BuildAccessPropertyLambdaExpression         | Create a non-nullable property access Lambda expression, call Compile() and use it                                                                                                                   |
| BuildAccessNullablePropertyLambdaExpression | Creates a nullable property access lambda expression, call Compile() and use it                                                                                                                      |
| AccessProperty                              | Build AccessNullablePropertyLambdaExpression, you need to concatenate the ToXXX method to create the final expression                                                                                |
| AccessNullableProperty                      | Generate a nullable property access chain expression, you need to concatenate the ToXXX method to create the final expression                                                                        |
| AccessPropertyDefaultIfNull                 | Generate a DefaultIfNull property expression that uses the default value passed in when the property access result is null, you need to concatenate the ToXXX methods to create the final expression |
| ToDelegate                                  | Create delegate method from AccessXXX method                                                                                                                                                         |
| ToLambdaExpression                          | Create a Lambda expression from the AccessXXX method                                                                                                                                                 |
| ToFuncExpression                            | Create the Func<,> delegate method from the AccessXXX method                                                                                                                                         |
