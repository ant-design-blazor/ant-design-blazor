---
category: Components
type: Data Entry
cols: 1
title: Transfer
cover: https://gw.alipayobjects.com/zos/alicdn/QAXskNI4G/Transfer.svg
---

Double column transfer choice box.

## When To Use

- It is a select control essentially which can be use for selecting multiple items.
- Transfer can display more information for items and take up more space.

Transfer the elements between two columns in an intuitive and efficient way.

One or more elements can be selected from either column, one click on the proper `direction` button, and the transfer is done. The left column is considered the `source` and the right column is considered the `target`. As you can see in the API description, these names are reflected in.

## API

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| DataSource | Used for setting the source data. The elements that are part of this array will be present the left column. Except the elements whose keys are included in `targetKeys` prop. | List\<TransferItem> | null |  |
| Disabled | Whether disabled transfer | bool | false |  |
| Footer | A function used for rendering the footer. | OneOf<string, RenderFragment> | null |  |
| Style | Custom the css for the whole componet. | string |  |  |
| operations | A set of operations that are sorted from top to bottom. | string\[] | \['right', 'left'] |  |
| Render | The function to generate the item shown on a column. Based on an record (element of the DataSource array), this function should return a OneOf<string, RenderFragment> which is generated from that record. | Func<TransferItem, OneOf<string, RenderFragment>> |  |  |
| SelectedKeys | A set of keys of selected items. | string\[] | \[] |  |
| ShowSearch | If included, a search box is shown on each column. | bool | false |  |
| ShowSelectAll | Show select all checkbox on the header | bool | true |  |
| TargetKeys | A set of keys of elements that are listed on the right column. | string\[] | \[] |  |
| Titles | A set of titles that are sorted from left to right. | string\[] | - |  |
| Locale | The i18n text including filter, empty text, item unit, etc. | TransferLocale |  |  |
| ListStyle | A custom CSS style used for rendering the transfer columns. | Func<TransferDirection,string> | | |
| OnChange | A callback function that is executed when the transfer between columns is complete. | TransferSelectChangeArgs |  |  |
| OnScroll | A callback function which is executed when scroll options list | TransferScrollArgs |  |  |
| OnSearch | A callback function which is executed when search field are changed | TransferSearchArgs                                | - |  |
| OnSelectChange | A callback function which is executed when selected items are changed. | TransferSelectChangeArgs |  |  |

### Custom panel content

`ChildContent` is a template rendered in both panels. Its `TransferSidePanelContext<TransferItem>` provides the current panel's `DataSource`, `Direction`, `SelectedKeys`, `TargetKeys`, `Disabled`, and `DefaultContent`. Use `SelectItemAsync` to update selection from custom content. See the Table Transfer and Tree Transfer examples.

## Warning

According the [standard](http://facebook.github.io/react/docs/lists-and-keys.html#keys) of Blazor, the key should always be supplied directly to the elements in the array. In Transfer, the keys should be set on the elements included in `dataSource` array. By default, `key` property is used as an unique identifier.

If there's no `key` in your data, you should use `rowKey` to specify the key that will be used for uniquely identify each element.
