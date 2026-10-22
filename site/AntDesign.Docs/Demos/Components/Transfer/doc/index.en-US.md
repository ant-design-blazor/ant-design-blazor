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
| Status | Set the validation status (`error` or `warning`) of the transfer lists. | string | - |  |
| Footer | A function used for rendering the footer. | OneOf<string, RenderFragment> | null |  |
| Style | Custom the css for the whole componet. | string |  |  |
| ClassNames | Customize classes for semantic elements. Use shared keys such as `header`, or direction-specific dotted keys such as `source.header` and `target.header`. | IReadOnlyDictionary<string, string> | null |  |
| Styles | Customize inline CSS for semantic elements. Shared and direction-specific styles are merged, with direction-specific declarations taking precedence. | IReadOnlyDictionary<string, string> | null |  |
| operations | A set of operations that are sorted from top to bottom. | string\[] | \['right', 'left'] |  |
| Render | The function to generate the item shown on a column. Based on an record (element of the DataSource array), this function should return a OneOf<string, RenderFragment> which is generated from that record. | Func<TransferItem, OneOf<string, RenderFragment>> |  |  |
| SelectedKeys | A set of keys of selected items. Supports `@bind-SelectedKeys` for two-way binding; passing it without updating it from a change callback keeps selection externally controlled. | string\[] | \[] |  |
| SelectedKeysChanged | Callback invoked when selected keys change; also used by `@bind-SelectedKeys`. | EventCallback\<IEnumerable\<string\>> |  |  |
| ShowSearch | If included, a search box is shown on each column. | bool | false |  |
| FilterOption | Custom predicate used to filter items from the search input. | Func<string, TransferItem, bool> | Search title |  |
| ShowSelectAll | Show select all checkbox on the header | bool | true |  |
| TargetKeys | A set of keys of elements that are listed on the right column. | string\[] | \[] |  |
| Titles | A set of titles that are sorted from left to right. | string\[] | - |  |
| SelectAllLabels | A set of customized labels for select all checkboxs on the header |  |  |  |
| Locale | The i18n text including filter, empty text, item unit, etc. | TransferLocale |  |  |
| ListStyle | A custom CSS style used for rendering the transfer columns. | Func<TransferDirection,string> | | |
| OnChange | A callback function that is executed when the transfer between columns is complete. | TransferSelectChangeArgs |  |  |
| OnScroll | A callback function which is executed when scroll options list | TransferScrollArgs |  |  |
| OnSearch | A callback function which is executed when search field are changed | TransferSearchArgs                                | - |  |
| OnSelectChange | A callback function which is executed when selected items are changed. | TransferSelectChangeArgs |  |  |

## Semantic DOM

`ClassNames` and `Styles` support the shared slots `root`, `section`, `header`, `title`, `body`, `list`, `item`, `itemIcon`, `itemContent`, `footer`, and `actions`. The `source` and `target` keys customize the left and right section respectively; use dotted keys such as `source.item` or `target.header` to customize a slot on only one side.

<code src="./demo/SemanticStyle.razor">Semantic DOM styling</code>

### Render Props

`ChildContent` is a template receiving `TransferSidePanelContext<TransferItem>`. Transfer applies the same template to both panels. The Table demo renders the current panel's rows in the table; the Tree demo renders a tree on the left and uses `DefaultContent` for the default list on the right.

| Property        | Description                              | Type                          |
| --------------- | ---------------------------------------- | ----------------------------- |
| Direction       | Current panel direction                  | `TransferDirection`           |
| DataSource      | Filtered items for this panel            | `IReadOnlyList<TransferItem>` |
| SelectedKeys    | Selected keys in this panel              | `IReadOnlySet<string>`        |
| TargetKeys      | Keys currently in the target panel       | `IReadOnlySet<string>`        |
| Disabled        | Whether Transfer is disabled             | `bool`                        |
| KeySelector     | Gets an item's key                        | `Func<TransferItem, string>`  |
| SelectItemAsync | Changes an item's selected state         | `Task SelectItemAsync(string key, bool selected)` |
| DefaultContent  | The default item-list template            | `RenderFragment`              |

## Warning

According the [standard](http://facebook.github.io/react/docs/lists-and-keys.html#keys) of Blazor, the key should always be supplied directly to the elements in the array. In Transfer, the keys should be set on the elements included in `dataSource` array. By default, `key` property is used as an unique identifier.

If there's no `key` in your data, you should use `rowKey` to specify the key that will be used for uniquely identify each element.
