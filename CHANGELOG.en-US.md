---
order: 6
title: Change Log
toc: false
timeline: true
---

`Ant Design Blazor` strictly follows [Semantic Versioning 2.0.0](http://semver.org/).

#### Release Schedule

- Weekly release: patch version at the end of every week for routine bugfix (anytime for urgent bugfix).
- Monthly release: minor version at the end of every month for new features.
- Major version release is not included in this schedule for breaking change and new features.

---

### 0.19.4

`2024-7-03`

- 🔥Ant Design Icons for Blazor have been released！[ant-design-icons-blazor](https://github.com/ant-design-blazor/ant-design-icons-blazor)
- 🔥Add Form GenerateFormItem component for automatic generation basic FormItem. [#3877](https://github.com/ant-design-blazor/ant-design-blazor/pull/3877) [@dessli](https://github.com/dessli)

- Tree
  - 🆕 Add support to check/uncheck all the child nodes recursively. [#3937](https://github.com/ant-design-blazor/ant-design-blazor/pull/3937) [@pankey888](https://github.com/pankey888)
  - 🐞 Fixed hover state display when CheckOnClickNode is true. [#3952](https://github.com/ant-design-blazor/ant-design-blazor/pull/3952) [@pankey888](https://github.com/pankey888)
  - 🐞 Fixed 'SelectAll' to select all the nodes in the tree. [#3938](https://github.com/ant-design-blazor/ant-design-blazor/pull/3938) [@pankey888](https://github.com/pankey888)

- TreeSelect
  - 🆕 Add TreeCheckStrictly and ShowCheckedStrategy to customize the checked values' outputting. [#3946](https://github.com/ant-design-blazor/ant-design-blazor/pull/3946) [@pankey888](https://github.com/pankey888)
  - 🆕 Add TreeDefaultExpandParent & TreeDefaultExpandedKeys. [#3953](https://github.com/ant-design-blazor/ant-design-blazor/pull/3953) [@pankey888](https://github.com/pankey888)
  - 🆕 Add support customize the dropdown menu via DropdownRender. [#3939](https://github.com/ant-design-blazor/ant-design-blazor/pull/3939) [@pankey888](https://github.com/pankey888)
  - 🐞 Fixed setting item itself as value. [#3954](https://github.com/ant-design-blazor/ant-design-blazor/pull/3954) [@ElderJames](https://github.com/ElderJames)

- 💄 Fixed Checkbox diabled style of wrapper. [#3948](https://github.com/ant-design-blazor/ant-design-blazor/pull/3948) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Select Fix a bug which may raise an exception 'Index was out of Range'. (#3942). [#3947](https://github.com/ant-design-blazor/ant-design-blazor/pull/3947) [@pankey888](https://github.com/pankey888)
- 🐞 Fixed Modal doesn't return Yes/No result when create confirm by service. [#3945](https://github.com/ant-design-blazor/ant-design-blazor/pull/3945) [@ElderJames](https://github.com/ElderJames)


### 0.19.3

`2024-6-26`

- 🆕 Add Tree/TreeSelect support to select/check and expand node when clicking the node's title. [#3902](https://github.com/ant-design-blazor/ant-design-blazor/pull/3902) [@pankey888](https://github.com/pankey888)
- 🛠 Refactor Icon import JS directly to set up iconfont. [#3931](https://github.com/ant-design-blazor/ant-design-blazor/pull/3931) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Modal set result to tcs after pressing ESC. [#3934](https://github.com/ant-design-blazor/ant-design-blazor/pull/3934) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Table expression activator missing return. [#3933](https://github.com/ant-design-blazor/ant-design-blazor/pull/3933) [@ElderJames](https://github.com/ElderJames)


### 0.19.2

`2024-6-24`

🔥 Template support Blazor WebApp with auto render mode now! Let's try!

```
dotnet new update
dotnet new antdesign -n webapp --host webapp --full
```

- Table
  - 🆕 Add support custom attributes for filter inupt. [#3897](https://github.com/ant-design-blazor/ant-design-blazor/pull/3897) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add parameter Filtered for marking filter is actived. [#3911](https://github.com/ant-design-blazor/ant-design-blazor/pull/3911) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed creating TItem instance with default constructor. [#3916](https://github.com/ant-design-blazor/ant-design-blazor/pull/3916) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed check-all can't check the children rows with tree data. [#3909](https://github.com/ant-design-blazor/ant-design-blazor/pull/3909) [@ElderJames](https://github.com/ElderJames)

- Tree
  - 🆕 Add 'Checkable' to TreeNode. [#3899](https://github.com/ant-design-blazor/ant-design-blazor/pull/3899) [@pankey888](https://github.com/pankey888)
  - 🛠 Refactor tree's Selected/Checked/Expanded parameters. [#3896](https://github.com/ant-design-blazor/ant-design-blazor/pull/3896) [@pankey888](https://github.com/pankey888)

- Select
  - 🐞 Fixed throwing exception cause by browser's  auto complete. [#3925](https://github.com/ant-design-blazor/ant-design-blazor/pull/3925) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed Setting parameters asynchronously. [#3912](https://github.com/ant-design-blazor/ant-design-blazor/pull/3912) [@WoogaAndrew](https://github.com/WoogaAndrew)
  - 🐞 Fixed the width of searching input string. [#3910](https://github.com/ant-design-blazor/ant-design-blazor/pull/3910) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed flags enum splitting incorrectly. [#3907](https://github.com/ant-design-blazor/ant-design-blazor/pull/3907) [@ElderJames](https://github.com/ElderJames)

- Checkbox
  - 🐞 fix(module: checkbox): avoid propagation for the label. [#3918](https://github.com/ant-design-blazor/ant-design-blazor/pull/3918) [@ElderJames](https://github.com/ElderJames)
  - 🐞 fix(module: checkbox): checked incorrectly with checkbox group. [#3903](https://github.com/ant-design-blazor/ant-design-blazor/pull/3903) [@ElderJames](https://github.com/ElderJames)

- 🐞 Fixed Image that ImagePreviewGroup throwing exception while there is no image. [#3917](https://github.com/ant-design-blazor/ant-design-blazor/pull/3917) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed AutoComplete open dropdown only when there are matched options. [#3926](https://github.com/ant-design-blazor/ant-design-blazor/pull/3926) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed DatePicker year unit was wrapping because of the format. [#3919](https://github.com/ant-design-blazor/ant-design-blazor/pull/3919) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Menu exec stateHasChanged on select/deselect [#3894](https://github.com/ant-design-blazor/ant-design-blazor/pull/3894) [@iits-timon-holzhaeuser](https://github.com/iits-timon-holzhaeuser)
- 🐞 Fixed TreeSelect with single selection raises an excecption when clicking clear icon. [#3906](https://github.com/ant-design-blazor/ant-design-blazor/pull/3906) [@pankey888](https://github.com/pankey888)


### 0.19.1

`2024-5-27`

- Table
  - 🆕 Add HideColumnsByName parameter to handle GenerateColumns hide some column. [#3863](https://github.com/ant-design-blazor/ant-design-blazor/pull/3863) [@dessli](https://github.com/dessli)
  - 🐞 Fixed enum field filter throwing null ref exception when the field type is nullable. [#3870](https://github.com/ant-design-blazor/ant-design-blazor/pull/3870) [@ElderJames](https://github.com/ElderJames)

- 🆕 Add TreeSelect CascadingTypeParameter. [#3864](https://github.com/ant-design-blazor/ant-design-blazor/pull/3864) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed InputNumber nullable floating types not round with "Precision" set. [#3868](https://github.com/ant-design-blazor/ant-design-blazor/pull/3868) [@Jtfk](https://github.com/Jtfk)
- 🐞 Fixed Select that EnumSelect throwing exception cause by nullable enum type. [#3859](https://github.com/ant-design-blazor/ant-design-blazor/pull/3859) [@ElderJames](https://github.com/ElderJames)

### 0.19.0

`2024-5-7` 

- TreeSelect
  - 🆕 Add ExpandedKeys parameter. [#3844](https://github.com/ant-design-blazor/ant-design-blazor/pull/3844) [@pankey888](https://github.com/pankey888)
  - 🆕 Add TitleIconTemplate. [#3834](https://github.com/ant-design-blazor/ant-design-blazor/pull/3834) [@pankey888](https://github.com/pankey888)
  - 🆕 Add supports generic value. [#3831](https://github.com/ant-design-blazor/ant-design-blazor/pull/3831) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed mismatch occurs between tags and checked nodes in the TreeSelect if TreeCheckable is set. [#3839](https://github.com/ant-design-blazor/ant-design-blazor/pull/3839) [@pankey888](https://github.com/pankey888)
  - 🐞 Fixed the tree will be collapsed after any node is selected. [#3827](https://github.com/ant-design-blazor/ant-design-blazor/pull/3827) [@pankey888](https://github.com/pankey888)

- Form
  - 🔥 Add support for static SSR. [#3580](https://github.com/ant-design-blazor/ant-design-blazor/pull/3580) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add support interactive localization for DataAnnotations. [#3823](https://github.com/ant-design-blazor/ant-design-blazor/pull/3823) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed null reference exception. [#3815](https://github.com/ant-design-blazor/ant-design-blazor/pull/3815) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed shouldn't validate until submit in non-validate-on-change. [#3812](https://github.com/ant-design-blazor/ant-design-blazor/pull/3812) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed the required data annotation doesn't work in dynamic model. [#3811](https://github.com/ant-design-blazor/ant-design-blazor/pull/3811) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed IsModified work incorrectly when ValidateOnChange is false. [#3795](https://github.com/ant-design-blazor/ant-design-blazor/pull/3795) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed rule validation for dynamic fields. [#3791](https://github.com/ant-design-blazor/ant-design-blazor/pull/3791) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed AntInputComponentBase Dictionary Type judgement. [#3787](https://github.com/ant-design-blazor/ant-design-blazor/pull/3787) [@tiansfather](https://github.com/tiansfather)


- ReuseTabs
  - 🆕 Acd make the Page of ReuseTabsService public. [#3800](https://github.com/ant-design-blazor/ant-design-blazor/pull/3800) [@ElderJames](https://github.com/ElderJames)
  - 📖 Docs add reusetabs documation and demos. [#3802](https://github.com/ant-design-blazor/ant-design-blazor/pull/3802) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed shouldn't auto navigate to the first pinned tab. [#3825](https://github.com/ant-design-blazor/ant-design-blazor/pull/3825) [@ElderJames](https://github.com/ElderJames)


- Select
  - 🆕 Add EnumSelect Support using 'bind-Value' to get or set multiple enumeration values with the Flags attribute. [#3843](https://github.com/ant-design-blazor/ant-design-blazor/pull/3843) [@pankey888](https://github.com/pankey888)
  - 🐞 Fixed List order after datasource change. [#3806](https://github.com/ant-design-blazor/ant-design-blazor/pull/3806) [@miguelkmarques](https://github.com/miguelkmarques)
  - 📖 Docs add search sample for table select demo. [#3797](https://github.com/ant-design-blazor/ant-design-blazor/pull/3797) [@ElderJames](https://github.com/ElderJames)

- Modal
  - 🆕 Add support customized header (#3579). [7be4807](https://github.com/ant-design-blazor/ant-design-blazor/commit/7be4807) [@Pat Hartl](https://github.com/Pat Hartl)
  - 🆕 Add support updaet loading state of the confirm button. [#3796](https://github.com/ant-design-blazor/ant-design-blazor/pull/3796) [@ElderJames](https://github.com/ElderJames)
  - 🛠 Refacotr modal that the creating methods in ModalService will return ModalRef synchronously. [#3794](https://github.com/ant-design-blazor/ant-design-blazor/pull/3794) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed  delete element by JS throwing exception. [#3847](https://github.com/ant-design-blazor/ant-design-blazor/pull/3847) [@ElderJames](https://github.com/ElderJames)

- DatePicker
  - 🐞 Fixed DatePicker Now button breaks when using ShowTime and ChangeOnClose. [#3830](https://github.com/ant-design-blazor/ant-design-blazor/pull/3830) [@agolub-s](https://github.com/agolub-s)
  - 🐞 Fixed RangePicker two-way binding failed and the preset range could not be updated. [#3850](https://github.com/ant-design-blazor/ant-design-blazor/pull/3850 ) [@ElderJames](https://github.com/ElderJames)

- 🔥 Add implement interactive localization service. [#3804](https://github.com/ant-design-blazor/ant-design-blazor/pull/3804) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add Checkbox support generic value. [#3715](https://github.com/ant-design-blazor/ant-design-blazor/pull/3715) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed make IsExternalInit internal for avoiding runtime conflicts with 3rd-party libs. [#3799](https://github.com/ant-design-blazor/ant-design-blazor/pull/3799) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed InputNumber cannot bind when pasting numbers with delimiters. [#3841](https://github.com/ant-design-blazor/ant-design-blazor/pull/3841) [@HuaFangYun](https://github.com/HuaFangYun)
- 🐞 Fixed overlay Prarent overlay hides irregularly when child is open or closed. (#3836, #3837). [#3838](https://github.com/ant-design-blazor/ant-design-blazor/pull/3838) [@pankey888](https://github.com/pankey888)

### 0.18.3

`2024-4-9` 

- 🐞 Fixed Table row expand incorrectly because the cache is cleared due to re-rendering. [#3785](https://github.com/ant-design-blazor/ant-design-blazor/pull/3785) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Progress single strokecolor does not works for circle type. [#3783](https://github.com/ant-design-blazor/ant-design-blazor/pull/3783) [@jeffersyuan1976](https://github.com/jeffersyuan1976)
- 🐞 Fixed DatePicker that disabled date is not judging correctly in the larger range panels. [#3781](https://github.com/ant-design-blazor/ant-design-blazor/pull/3781) [@ElderJames](https://github.com/ElderJames)
- 📖 Updated get started for charts. [#3774](https://github.com/ant-design-blazor/ant-design-blazor/pull/3774) [@CAPCHIK](https://github.com/CAPCHIK)

Table row status behavior changes:

After rerendering or calling `ITable.ReloadData()`, the row state of the same RowKey as the current page data (e.g., expanded, selected) will not be reset.

### 0.18.2

`2024-4-2` 

- Form
  - 🆕 Add Form autocomplete parameter. [#3763](https://github.com/ant-design-blazor/ant-design-blazor/pull/3763) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed form validation for custom controls. [#3761](https://github.com/ant-design-blazor/ant-design-blazor/pull/3761) [@ElderJames](https://github.com/ElderJames)
- Table
  - 🐞 Fixed Table throw exception while sorting rows outside. [#3766](https://github.com/ant-design-blazor/ant-design-blazor/pull/3766) [@ElderJames](https://github.com/ElderJames)
  - 🐞 fix table rows which have been remove from selectedRows would be selected again  （client side data）. [#3762](https://github.com/ant-design-blazor/ant-design-blazor/pull/3762) [@ElderJames](https://github.com/ElderJames)

### 0.18.1

`2024-3-21` 

The Spring Equinox

- 🆕 Add modal support customized header (#3579). [4cfeffd](https://github.com/ant-design-blazor/ant-design-blazor/commit/4cfeffd) [@Pat Hartl](https://github.com/Pat Hartl)

- Form
  - 🐞 Fixed binding issue for static SSR，**static rendering support is available**. [#3580](https://github.com/ant-design-blazor/ant-design-blazor/pull/3580) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed cause exception while has no FieldIdentifier. [#3717](https://github.com/ant-design-blazor/ant-design-blazor/pull/3717) [@ElderJames](https://github.com/ElderJames)

- Select
  - 🐞 Fixed group names are not hidden when searching. [#3722](https://github.com/ant-design-blazor/ant-design-blazor/pull/3722) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
  - 🐞 Fixed clearing input in Select when value selected via search. [#3726](https://github.com/ant-design-blazor/ant-design-blazor/pull/3726) [@agolub-s](https://github.com/agolub-s)

- 💄 Style Upload with no button. [#3734](https://github.com/ant-design-blazor/ant-design-blazor/pull/3734) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Table resizable column did'n work with `ScrollY`. [#3746](https://github.com/ant-design-blazor/ant-design-blazor/pull/3746) [@thirking](https://github.com/thirking)
- 🐞 Fixed JS  circular referencing during serialization. [#3739](https://github.com/ant-design-blazor/ant-design-blazor/pull/3739) [@jxcproject](https://github.com/jxcproject)


### 0.18.0

`2024-02-29`

🐉Good luck in the Year of the Loong！

- Table
  - 🆕 Add default ScrollBar style. [#3668](https://github.com/ant-design-blazor/ant-design-blazor/pull/3668) [@thirking](https://github.com/thirking)
  - 🐞 Fixed the DateField filter would throw exception when property type is nullable. [#3704](https://github.com/ant-design-blazor/ant-design-blazor/pull/3704) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed the shadow style of Header in Table with fixed columns. [#3691](https://github.com/ant-design-blazor/ant-design-blazor/pull/3691) [@thirking](https://github.com/thirking)
  - 🐞 Fixed built-in filter carriage jump. [#3683](https://github.com/ant-design-blazor/ant-design-blazor/pull/3683) [@m-khrapunov](https://github.com/m-khrapunov)
  - 🐞 Fixed that remove loading and show no data when datasource is abstract and empty. [#3688](https://github.com/ant-design-blazor/ant-design-blazor/pull/3688) [@ElderJames](https://github.com/ElderJames)

- Select
  - 🆕 Add support table select. [#3693](https://github.com/ant-design-blazor/ant-design-blazor/pull/3693) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add FilterExpression on select for customize how to filter when searching. [#3656](https://github.com/ant-design-blazor/ant-design-blazor/pull/3656) [@Magehernan](https://github.com/Magehernan)
  - 🐞 Fixed placeholder display in Select Content when the input (search) value is not null. [#3701](https://github.com/ant-design-blazor/ant-design-blazor/pull/3701) [@agolub-s](https://github.com/agolub-s)
  - 🐞 Fixed incorrect html title for selected item label. [#3695](https://github.com/ant-design-blazor/ant-design-blazor/pull/3695) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed the DropdownRender doesn't pass original content into renderfargment. [#3675](https://github.com/ant-design-blazor/ant-design-blazor/pull/3675) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed bug where selects contained in forms with ValidateOnChange = true don't appear to update when bound values change . [#3703](https://github.com/ant-design-blazor/ant-design-blazor/pull/3703) [@edwardbarford](https://github.com/edwardbarford)


- Form
  - 🆕 Add Method parameter for SSR. [#3608](https://github.com/ant-design-blazor/ant-design-blazor/pull/3608) [@CrosRoad95](https://github.com/CrosRoad95)
  - 🆕 Add FormItemName for dynamic model by DataIndex support. [#3612](https://github.com/ant-design-blazor/ant-design-blazor/pull/3612) [@Zonciu](https://github.com/Zonciu)

- 🆕 Add Tabs CreateTab method for ReuseTabsService to create tabs. [#3671](https://github.com/ant-design-blazor/ant-design-blazor/pull/3671) [@jxcproject](https://github.com/jxcproject)
- 🆕 Add Comment placement parameter. [#3670](https://github.com/ant-design-blazor/ant-design-blazor/pull/3670) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Modal  repeated cleaning dom. [#3673](https://github.com/ant-design-blazor/ant-design-blazor/pull/3673) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed Message non thread safe on Webview. [#3698](https://github.com/ant-design-blazor/ant-design-blazor/pull/3698) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fix Radio style issue cause by preent default. [#3694](https://github.com/ant-design-blazor/ant-design-blazor/pull/3694) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Card Tabs size. [#3661](https://github.com/ant-design-blazor/ant-design-blazor/pull/3661) [@thirking](https://github.com/thirking)
- 🐞 Fixed Segmented label in SegmentedOption. [#3659](https://github.com/ant-design-blazor/ant-design-blazor/pull/3659) [@CrosRoad95](https://github.com/CrosRoad95)
- 📖 Add Blazor Webapp site. [#3642](https://github.com/ant-design-blazor/ant-design-blazor/pull/3642) [@bxjg1987](https://github.com/bxjg1987)

### 0.17.4

`2024-02-01`

- Select
  - 🐞 Fixed search input box still editable when disabled. [#3655](https://github.com/ant-design-blazor/ant-design-blazor/pull/3655) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed scroll in select not working correctly when EnableVirtualization is true. [#3625](https://github.com/ant-design-blazor/ant-design-blazor/pull/3625) [@Magehernan](https://github.com/Magehernan)

- 🐞 Fixed Collapse `Accordion` doesn't work. [#3646](https://github.com/ant-design-blazor/ant-design-blazor/pull/3646) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Modal draggable and centered work incorrectly. [#3647](https://github.com/ant-design-blazor/ant-design-blazor/pull/3647) [@zxyao145](https://github.com/zxyao145)

### 0.17.3

`2024-01-14`

- Table
  - 🐞 Fixed should flush cache only pagging. [#3620](https://github.com/ant-design-blazor/ant-design-blazor/pull/3620) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed UnselectAll() was not work. [#3618](https://github.com/ant-design-blazor/ant-design-blazor/pull/3618) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed thrown exception when property type is `Char`. [#3617](https://github.com/ant-design-blazor/ant-design-blazor/pull/3617) [@ElderJames](https://github.com/ElderJames)

- 🐞 Fixed Modal dom instance clear when url changed. [#3630](https://github.com/ant-design-blazor/ant-design-blazor/pull/3630) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed Select placholder doesn't hide correctly. [#3628](https://github.com/ant-design-blazor/ant-design-blazor/pull/3628) [@ElderJames](https://github.com/ElderJames)

### 0.17.2

`2024-01-07`

- 🐞 Fixed Menu wrong judge about InlineCollapsed parameter. [#3614](https://github.com/ant-design-blazor/ant-design-blazor/pull/3614) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Table caches page data multiple times on remote DataSource mode. [#3611](https://github.com/ant-design-blazor/ant-design-blazor/pull/3611) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add more options for js initializers. [#3610](https://github.com/ant-design-blazor/ant-design-blazor/pull/3610) [@ElderJames](https://github.com/ElderJames)

### 0.17.1

`2023-12-27`

- 🐞 Fixed Table avoid duplicated row key. [#3594](https://github.com/ant-design-blazor/ant-design-blazor/pull/3594) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Select avoid selected item being set to 0 when the default value of TItem is 0. [#3595](https://github.com/ant-design-blazor/ant-design-blazor/pull/3595) [@ElderJames](https://github.com/ElderJames)
- 💄 Add support for custom script/style import locations. [#3596](https://github.com/ant-design-blazor/ant-design-blazor/pull/3596) [@ElderJames](https://github.com/ElderJames)

### 0.17.0

`2023-12-25`

- 🔥 Add new component WaterMark. [#3441](https://github.com/ant-design-blazor/ant-design-blazor/pull/3441) [@ElderJames](https://github.com/ElderJames)
- 🔥 Add new component Flex. [#3547](https://github.com/ant-design-blazor/ant-design-blazor/pull/3547) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add Mentions dynamic async loading. [#3503](https://github.com/ant-design-blazor/ant-design-blazor/pull/3503) [@kooliokey](https://github.com/kooliokey)
- 🆕 Add Radio button style support for RadioGroup with opt…. [#3589](https://github.com/ant-design-blazor/ant-design-blazor/pull/3589) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add Menu collapse montion. [#3395](https://github.com/ant-design-blazor/ant-design-blazor/pull/3395) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add Collapse collapse montion. [#3562](https://github.com/ant-design-blazor/ant-design-blazor/pull/3562) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add setup JS initializers. [#3557](https://github.com/ant-design-blazor/ant-design-blazor/pull/3557) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Upload exception when uploading a file without an extension. [#3554](https://github.com/ant-design-blazor/ant-design-blazor/pull/3554) [@SapientGuardian](https://github.com/SapientGuardian)
- 🐞 Fixed Tree that should show all nodes while search value is empty. [#3587](https://github.com/ant-design-blazor/ant-design-blazor/pull/3587) [@ElderJames](https://github.com/ElderJames)


- Select
  - 🆕 Add parameters that support use delegate to set option label and value. [#3569](https://github.com/ant-design-blazor/ant-design-blazor/pull/3569) [@MarvelTiter](https://github.com/MarvelTiter)
  - 🐞 Fixed that avoid search value binding while the IME is duri…. [#3583](https://github.com/ant-design-blazor/ant-design-blazor/pull/3583) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed that ensure that the click event is processed properly. [#3525](https://github.com/ant-design-blazor/ant-design-blazor/pull/3525) [@zxyao145](https://github.com/zxyao145)

- Table
  - 🆕 Add ExpandAll and CollapseAll methods. [#3491](https://github.com/ant-design-blazor/ant-design-blazor/pull/3491) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed rows can't rerender in some cases. [#3586](https://github.com/ant-design-blazor/ant-design-blazor/pull/3586) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed JSException on filter input autofocus. [#3543](https://github.com/ant-design-blazor/ant-design-blazor/pull/3543) [@m-khrapunov](https://github.com/m-khrapunov)
  - 🐞 Fixed clearing the selection state after pages was changed outside. [#3577](https://github.com/ant-design-blazor/ant-design-blazor/pull/3577) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed that can't clear the selected rows if they were not on the current page. [#3566](https://github.com/ant-design-blazor/ant-design-blazor/pull/3566) [@ElderJames](https://github.com/ElderJames)

- Input
  - 🐞 fix(module: input): hide the clear icon when the input was disabled. [#3585](https://github.com/ant-design-blazor/ant-design-blazor/pull/3585) [@ElderJames](https://github.com/ElderJames)
  - 🐞 fix(module: input): textarea missing rows attribute. [#3561](https://github.com/ant-design-blazor/ant-design-blazor/pull/3561) [@ElderJames](https://github.com/ElderJames)
  - 🐞 SetClass method of textarea component adds {PrefixCls}-affix-wrapper-disabled to _warpperClassMapper. [#3538](https://github.com/ant-design-blazor/ant-design-blazor/pull/3538) [@zuevus](https://github.com/zuevus)

- Tabs
   - 🆕 support separate the tab from the page,  also page reloading. [#3467](https://github.com/ant-design-blazor/ant-design-blazor/pull/3467) [@ElderJames](https://github.com/ElderJames)
  - 🐞 feat(module: tabs):  reusetabs supports the interactive auto mode. [#3564](https://github.com/ant-design-blazor/ant-design-blazor/pull/3564) [@ElderJames](https://github.com/ElderJames)
  - 🐞 fix(module: tabs): default to open first pinned page with reusetabs. [#3519](https://github.com/ant-design-blazor/ant-design-blazor/pull/3519) [@ElderJames](https://github.com/ElderJames)


### 0.16.3

`2023-12-04`

- Table
  - 🛠 Refactor some internal components to render fragments, reducing allocation and avoid side effects cause by life cycle. [#3545](https://github.com/ant-design-blazor/ant-design-blazor/pull/3545) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed the row clearing state after page index was changed in client resource mode. [#3546](https://github.com/ant-design-blazor/ant-design-blazor/pull/3546) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed the data of row did't update after it's data source was changed. [#3544](https://github.com/ant-design-blazor/ant-design-blazor/pull/3544) [@ElderJames](https://github.com/ElderJames)

- 🐞 Fixed Select clear selectd option when the default value isn't in the options. [#3529](https://github.com/ant-design-blazor/ant-design-blazor/pull/3529) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Tree two-way binding for check/select/expand. [#3520](https://github.com/ant-design-blazor/ant-design-blazor/pull/3520) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed core enum name supports localization. [#3536](https://github.com/ant-design-blazor/ant-design-blazor/pull/3536) [@ElderJames](https://github.com/ElderJames)
- 💄 Fixed Radio checked effect in ssr. [#3532](https://github.com/ant-design-blazor/ant-design-blazor/pull/3532) [@ElderJames](https://github.com/ElderJames)
- 💄 Fixed Checkbox checked effect in ssr. [#3535](https://github.com/ant-design-blazor/ant-design-blazor/pull/3535) [@ElderJames](https://github.com/ElderJames)


### 0.16.2

`2023-11-17`

- 🔥 update Blazor to .NET 8. [#3514](https://github.com/ant-design-blazor/ant-design-blazor/pull/3514) [@ElderJames](https://github.com/ElderJames)

- Table
  - 🆕 Add row grouping (experimental). [#3487](https://github.com/ant-design-blazor/ant-design-blazor/pull/3487) [@ElderJames](https://github.com/ElderJames)
  - 🛠 Refactor: rename RenderMode to RerenderStrategy. [#3515](https://github.com/ant-design-blazor/ant-design-blazor/pull/3515) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Refactor: reorganize the selection of rows. [#3502](https://github.com/ant-design-blazor/ant-design-blazor/pull/3502) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed setup RowKey EqualityComparer for caches. [#3483](https://github.com/ant-design-blazor/ant-design-blazor/pull/3483) [@ElderJames](https://github.com/ElderJames)
  - 📖 docs: fix router paging demo. [#3507](https://github.com/ant-design-blazor/ant-design-blazor/pull/3507) [@ElderJames](https://github.com/ElderJames)

- 🐞 Fixed Select that selected value will be reset when ValidateOnChange is on. [#3508](https://github.com/ant-design-blazor/ant-design-blazor/pull/3508) [@ldsenow](https://github.com/ldsenow)
- 🐞 Fixed TimePicker ArgumentOutOfRangeException with TiemOnly value. [#3501](https://github.com/ant-design-blazor/ant-design-blazor/pull/3501) [@Alexbits](https://github.com/Alexbits)
- 🐞 Fixed TreeSelect value binding on datasource was changed. [#3492](https://github.com/ant-design-blazor/ant-design-blazor/pull/3492) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Datepicker that RangePicker focus not cleared. [#3488](https://github.com/ant-design-blazor/ant-design-blazor/pull/3488) [@Alexbits](https://github.com/Alexbits)


#### Breaking Changes

Because the `RowSelectable` duplicated the function of `Selection.Disabled` and did not use the disabled style, so it was removed. Please feel free to give us feedback if you have any suggestions.

You can set the disabled parameter to achieve the same functionality.

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
  - 🆕 Add supports datasource of abstract classes. [#3475](https://github.com/ant-design-blazor/ant-design-blazor/pull/3475) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed row selection shouldn't need the selection column. [#3465](https://github.com/ant-design-blazor/ant-design-blazor/pull/3465) [@ElderJames](https://github.com/ElderJames)

- 🆕 Add Modal supports two-way binding for Visible parameter. [#3466](https://github.com/ant-design-blazor/ant-design-blazor/pull/3466) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Input incorrectly validation and required message. [#3474](https://github.com/ant-design-blazor/ant-design-blazor/pull/3474) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Layout NoTrigger not effect when CollapsedWidth is zero. [#3476](https://github.com/ant-design-blazor/ant-design-blazor/pull/3476) [@ElderJames](https://github.com/ElderJames)


### 0.16.0

`2023-10-24`

1024 LoL


- Table

  - 🆕 Add Custom and default Field filter support in Table. [#3279](https://github.com/ant-design-blazor/ant-design-blazor/pull/3279) [@rhodon-jargon](https://github.com/rhodon-jargon)
  - 🆕 Add resizable column. [#3340](https://github.com/ant-design-blazor/ant-design-blazor/pull/3340) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add `FilterTemplate` property on `Column`` and `PropertyColumn` to customize the filters dropdown. [#3285](https://github.com/ant-design-blazor/ant-design-blazor/pull/3285) [@manuelelucchi](https://github.com/manuelelucchi)
  - 🆕 Add filter input focus on dropdown visible. [#3450](https://github.com/ant-design-blazor/ant-design-blazor/pull/3450) [@m-khrapunov](https://github.com/m-khrapunov)
  - 🆕 Add RowKey parameter for row data compare. [#3439](https://github.com/ant-design-blazor/ant-design-blazor/pull/3439) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed the 'radio' Selection column will now correctly deselect all other items in a table with EnableVirtualization. [#3282](https://github.com/ant-design-blazor/ant-design-blazor/pull/3282) [@rhodon-jargon](https://github.com/rhodon-jargon)
  - 🐞 Fixed when the selection line is disabled, it can also be selected by "select All" and code. [#3436](https://github.com/ant-design-blazor/ant-design-blazor/pull/3436) [@ElderJames](https://github.com/ElderJames)


- Datepicker
  - 🆕 Add support multiple formats for input in DatePciker. [#3120](https://github.com/ant-design-blazor/ant-design-blazor/pull/3120) [@agolub-s](https://github.com/agolub-s)
  - 🆕 Add support specific popup placement. [#3345](https://github.com/ant-design-blazor/ant-design-blazor/pull/3345) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add DateTimeOffset, DateOnly, TimeOnly support. [#3443](https://github.com/ant-design-blazor/ant-design-blazor/pull/3443) [@Alexbits](https://github.com/Alexbits)
  - 🐞 Fixed override `ResetValue` so that `Reset` works in a form. [#3458](https://github.com/ant-design-blazor/ant-design-blazor/pull/3458) [@LeaFrock](https://github.com/LeaFrock)
  - 🐞 Add commit changes after blur in Calendar. Fix click on suffix icon in DatePickerInput. [#3087](https://github.com/ant-design-blazor/ant-design-blazor/pull/3087) [@agolub-s](https://github.com/agolub-s)

- ReuseTabs
  - 🆕 Add support specific the PinUrl for the routes which contains parameters. [#3363](https://github.com/ant-design-blazor/ant-design-blazor/pull/3363) [@James Yeung](https://github.com/James Yeung)
  - 🆕 Add order options for pinned tabs in reusetabs. [#3335](https://github.com/ant-design-blazor/ant-design-blazor/pull/3335) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add KeepAlive parameter to choose whether to keep the page state. [#3334](https://github.com/ant-design-blazor/ant-design-blazor/pull/3334) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add support reload page inside blazor. [#3396](https://github.com/ant-design-blazor/ant-design-blazor/pull/3396) [@ElderJames](https://github.com/ElderJames)
  - 🗑 Remove AuthorizeReuseTabsRouteView component and library. [#3437](https://github.com/ant-design-blazor/ant-design-blazor/pull/3437) [@ElderJames](https://github.com/ElderJames)


- Select
  - 🆕 Add 'Select' parameter ListboxStyle to handle selectlist display style. [#3288](https://github.com/ant-design-blazor/ant-design-blazor/pull/3288) [@dessli](https://github.com/dessli)
  - 🐞 Fix showing the arrow in SelectContent when mode is multiple. [#3430](https://github.com/ant-design-blazor/ant-design-blazor/pull/3430) [@agolub-s](https://github.com/agolub-s)


- 🆕 Add From that use 'DisplayAttribute.GetName()' to get the lable of FormItem. [#3426](https://github.com/ant-design-blazor/ant-design-blazor/pull/3426) [@huhangfei](https://github.com/huhangfei)
- 🆕 Add Image support drag and drop for preview images. [#3394](https://github.com/ant-design-blazor/ant-design-blazor/pull/3394) [@llp1520](https://github.com/llp1520)
- 🆕 Add InputNumber the `MaxLength` parameter. [#3455](https://github.com/ant-design-blazor/ant-design-blazor/pull/3455) [@chazikaifa](https://github.com/chazikaifa)
- 🆕 Add Drawer parameter VisibleChanged for two-way binding support. [#3333](https://github.com/ant-design-blazor/ant-design-blazor/pull/3333) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add Tabs Enter button to naviagte. [#3320](https://github.com/ant-design-blazor/ant-design-blazor/pull/3320) [@bweissronin](https://github.com/bweissronin)
- 🆕 Add Modal the resizable paramter that can be resized horizontally; Fixed the class and id parameters are not valid in modal component usage. [#3311](https://github.com/ant-design-blazor/ant-design-blazor/pull/3311) [@zxyao145](https://github.com/zxyao145)
- 🆕 Add Statistic the CultureInfo parameter to support localization number format. [#3299](https://github.com/ant-design-blazor/ant-design-blazor/pull/3299) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add Collapse the expand animation. [#3389](https://github.com/ant-design-blazor/ant-design-blazor/pull/3389) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add Tree the ExpandAll/CollapseAll TreeNode methods. [#3336](https://github.com/ant-design-blazor/ant-design-blazor/pull/3336) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed input can't change value onblur when composition inputting. [#3462](https://github.com/ant-design-blazor/ant-design-blazor/pull/3462) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Button that add multithreaded wasm compatibility on .NET 8. [#3451](https://github.com/ant-design-blazor/ant-design-blazor/pull/3451) [@petertorocsik](https://github.com/petertorocsik)

#### Breaking Changes:

- RangePicker the `OnChange` event was changed from `DateRangeChangedEventArgs` to `DateRangeChangedEventArgs<TValue>`, the type of `Dates` is changed to `TValue`.


### 0.15.5

`2023-09-10`

Happy Teachers' Day!

- Table
  - 🐞 Fixed avoid disabled selection would be selected when select-all checked. [#3419](https://github.com/ant-design-blazor/ant-design-blazor/pull/3419) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed reset `PageIndex` when filters and sorters change. [#3397](https://github.com/ant-design-blazor/ant-design-blazor/pull/3397) [@ElderJames](https://github.com/ElderJames)
  - 📖 Docs introduce how to use RowClassName with fixed column and hover rows. [#3409](https://github.com/ant-design-blazor/ant-design-blazor/pull/3409) [@ElderJames](https://github.com/ElderJames)

- 🐞 Fixed Input null check for the js of textarea resizable. [#3382](https://github.com/ant-design-blazor/ant-design-blazor/pull/3382) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Modal centered conflict maximizable style. [#3403](https://github.com/ant-design-blazor/ant-design-blazor/pull/3403) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed AutoComplete dropdown resize. [#3402](https://github.com/ant-design-blazor/ant-design-blazor/pull/3402) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Notification exception that change sync statehaschanged to async. [#3400](https://github.com/ant-design-blazor/ant-design-blazor/pull/3400) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed Progress text wrapping issue of the line type. [#3387](https://github.com/ant-design-blazor/ant-design-blazor/pull/3387) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Button avoid fire `OnClick` when it is Loading. [#3414](https://github.com/ant-design-blazor/ant-design-blazor/pull/3414) [@ElderJames](https://github.com/ElderJames)


- Accessibility:
  - ⌨️ Add Input the `required` attribute to input elements. [#3383](https://github.com/ant-design-blazor/ant-design-blazor/pull/3383) [@eizzn](https://github.com/eizzn)
  - ⌨️ Add Input the aria-invalid attribute when input fails validation. [#3378](https://github.com/ant-design-blazor/ant-design-blazor/pull/3378) [@eizzn](https://github.com/eizzn)
  - ⌨️ Add Select option the aria-label to select option. [#3385](https://github.com/ant-design-blazor/ant-design-blazor/pull/3385) [@eizzn](https://github.com/eizzn)

- 🌐 Fixed i18n: ko-KR locale Confim And Form. [#3415](https://github.com/ant-design-blazor/ant-design-blazor/pull/3415) [@Jeongyong-park](https://github.com/Jeongyong-park)


### 0.15.4

`2023-07-31`

- 🆕 Add AutoFocus for all select components. [#3375](https://github.com/ant-design-blazor/ant-design-blazor/pull/3375) [@LuukGlorie](https://github.com/LuukGlorie)
- 🐞 Fixed Tree would not invoke CheckedKeys changed while CheckStrictly was set. [#3379](https://github.com/ant-design-blazor/ant-design-blazor/pull/3379) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Checkbox passing disabled value to templated options from checkbox group. [#3365](https://github.com/ant-design-blazor/ant-design-blazor/pull/3365) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed DomEventListener check ContainsKey for shared event subscriptions store. [#3364](https://github.com/ant-design-blazor/ant-design-blazor/pull/3364) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Input incorrect html structure of TextArea icons. [#3367](https://github.com/ant-design-blazor/ant-design-blazor/pull/3367) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Menu missing null check. [#3368](https://github.com/ant-design-blazor/ant-design-blazor/pull/3368) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Form Help message change. [#3373](https://github.com/ant-design-blazor/ant-design-blazor/pull/3373) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed ReuseTabs throwing navigate exceptiion while base path was set. [#3362](https://github.com/ant-design-blazor/ant-design-blazor/pull/3362) [@ElderJames](https://github.com/ElderJames)
- ⌨️ i11y: Icon role update. [#3370](https://github.com/ant-design-blazor/ant-design-blazor/pull/3370) [@eizzn](https://github.com/eizzn)

### 0.15.3

`2023-07-13`

- 🐞 Fixed Tree multiple should not take effect when Ctrl is not pressed. [#3350](https://github.com/ant-design-blazor/ant-design-blazor/pull/3350) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- 🐞 Fixed Icons compatible with bootstrap. [#3348](https://github.com/ant-design-blazor/ant-design-blazor/pull/3348) [@ElderJames](https://github.com/ElderJames)
- 💄 Fixed Steps missing RTL style. [#3343](https://github.com/ant-design-blazor/ant-design-blazor/pull/3343) [@ElderJames](https://github.com/ElderJames)
- 🌐 i18n changed dateFormat and dateTimeFormat in ru-RU locale from d.M.yyyy to dd.MM.yyyy. [#3342](https://github.com/ant-design-blazor/ant-design-blazor/pull/3342) [@Life-is-Peachy](https://github.com/Life-is-Peachy)
- 📖 Docs optimize the performance of document navigation. [#3347](https://github.com/ant-design-blazor/ant-design-blazor/pull/3347) [@ElderJames](https://github.com/ElderJames)

### 0.15.2

`2023-07-03`

- Table
  - 🐞 Fixed Table avoid exception at DisposeAsync method. [#3337](https://github.com/ant-design-blazor/ant-design-blazor/pull/3337) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed Table render incorrectly with `ParametersHashCodeChanged` render mode in some case. [#3313](https://github.com/ant-design-blazor/ant-design-blazor/pull/3313) [@ElderJames](https://github.com/ElderJames)

- 🐞 Fixed Menu that MenuItem unselect incorrectly when menu was inline and collapsed. [#3338](https://github.com/ant-design-blazor/ant-design-blazor/pull/3338) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Drawer scroll not enable when page url changed. [#3316](https://github.com/ant-design-blazor/ant-design-blazor/pull/3316) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed Slider does not always fire OnAfterChange. [#3323](https://github.com/ant-design-blazor/ant-design-blazor/pull/3323) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Statistic that coundown pause while the navigate to other page. [#3329](https://github.com/ant-design-blazor/ant-design-blazor/pull/3329) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Card missing loading effect. [#3319](https://github.com/ant-design-blazor/ant-design-blazor/pull/3319) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed TreeSelect that removing last option can't change the binding values. [#3314](https://github.com/ant-design-blazor/ant-design-blazor/pull/3314) [@ElderJames](https://github.com/ElderJames)
- 🌐 i18n ru-RU changed dateFormat and dateTimeFormat to `d.m.yyyy` in ru-RU locale. [#3327](https://github.com/ant-design-blazor/ant-design-blazor/pull/3327) [@Life-is-Peachy](https://github.com/Life-is-Peachy)

### 0.15.1

`2023-06-18`

Happy Father's Day!

- Table
  - 🆕 Add the items of DataSource support interface types. [#3297](https://github.com/ant-design-blazor/ant-design-blazor/pull/3297) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed the text would overflow at the cell which is fixed and ellipsis. [#3291](https://github.com/ant-design-blazor/ant-design-blazor/pull/3291) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed empty status position incorrectly during pre-rendering stage, and avoid unnecessary use of ResizeObserver. [#3281](https://github.com/ant-design-blazor/ant-design-blazor/pull/3281) [@ElderJames](https://github.com/ElderJames)

- Tree
  - 🐞 Fixed search crash issue after Tree custom SearchExpression. [#3274](https://github.com/ant-design-blazor/ant-design-blazor/pull/3274) [@ruyisee](https://github.com/ruyisee)
  - 🐞 Fixed DataSource cannot be modify in place after drag and drop. [#3275](https://github.com/ant-design-blazor/ant-design-blazor/pull/3275) [@Jtfk](https://github.com/Jtfk)

- 🐞 Fixed DatePicker that OnOpenChange would be called twice on RangePicker close. [#3307](https://github.com/ant-design-blazor/ant-design-blazor/pull/3307) [@Alexbits](https://github.com/Alexbits)
- 🐞 Fixed Tabs duplicated pinned tabs in reusetab. [#3306](https://github.com/ant-design-blazor/ant-design-blazor/pull/3306) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Radio infinite loop render after changing the bind value out of the optons. [#3287](https://github.com/ant-design-blazor/ant-design-blazor/pull/3287) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed DomEventListener that remove the shared event subscriptions from it's store after there are no one are listening the event. [#3278](https://github.com/ant-design-blazor/ant-design-blazor/pull/3278) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed incorrect call of `EditContext.NotifyFieldChanged` when `SelectBase.Values` is set (unchanged) to null. [#3277](https://github.com/ant-design-blazor/ant-design-blazor/pull/3277) [@rhodon-jargon](https://github.com/rhodon-jargon)
- 📖 Update Statistic docs and add a demo about Separator usage. [#3166](https://github.com/ant-design-blazor/ant-design-blazor/pull/3166) [@Alerinos](https://github.com/Alerinos)

### 0.15.0

`2023-05-21`

- Table
  - 🆕 Add ItemsProvider support for Virtualization. [#3262](https://github.com/ant-design-blazor/ant-design-blazor/pull/3262) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add EF Core support for Virtualization ItemsProvider. [#3270](https://github.com/ant-design-blazor/ant-design-blazor/pull/3270) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed not logging exception and ignore JSDisconnectedException. [#3216](https://github.com/ant-design-blazor/ant-design-blazor/pull/3216) [@LuukGlorie](https://github.com/LuukGlorie)
  - 🐞 Fixed the filter of column with the flags enum type place incorrectly. [#3168](https://github.com/ant-design-blazor/ant-design-blazor/pull/3168) [@ElderJames](https://github.com/ElderJames)

- Layout
  - 🆕 Add `DefaultCollapsed` for sider. [#3260](https://github.com/ant-design-blazor/ant-design-blazor/pull/3260) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed menu can't collapsed when sider is default to collapsed. [#3268](https://github.com/ant-design-blazor/ant-design-blazor/pull/3268) [@ElderJames](https://github.com/ElderJames)

- Tree
  - 🆕 Add `HideUnmatched` parameter, which allows you to hide all TreeNodes that are not matched to the `SearchValue`. [#3242](https://github.com/ant-design-blazor/ant-design-blazor/pull/3242) [@rhodon-jargon](https://github.com/rhodon-jargon)
  - 🆕 Add public method `GetNode(string key)`. [#3243](https://github.com/ant-design-blazor/ant-design-blazor/pull/3243) [@AndrewKaninchen](https://github.com/AndrewKaninchen)

- TreeSelect
  - 🆕 Add TreeSelect `OnSearch` and `OnNodeLoadDelayAsync` to allow dynamic loading. [#3240](https://github.com/ant-design-blazor/ant-design-blazor/pull/3240) [@rhodon-jargon](https://github.com/rhodon-jargon)
  - 🆕 Add `TreeAttributes` parameter to give additional parameters to internal Tree component. [#3234](https://github.com/ant-design-blazor/ant-design-blazor/pull/3234) [@rhodon-jargon](https://github.com/rhodon-jargon)

- Select
  - 🆕 Add Select support for <code class="notranslate">accesskey</code> attribute. [#3228](https://github.com/ant-design-blazor/ant-design-blazor/pull/3228) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed empty incorrectly in virtualization mode. [#3171](https://github.com/ant-design-blazor/ant-design-blazor/pull/3171) [@ElderJames](https://github.com/ElderJames)

- Tabs
  - 🐞 Fixed Tabs animated cause display incorrectly. [#3177](https://github.com/ant-design-blazor/ant-design-blazor/pull/3177) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed the logic of ReuseTabs about tab key. [#3153](https://github.com/ant-design-blazor/ant-design-blazor/pull/3153) [@berkerdong](https://github.com/berkerdong)

- Datepicker
  - 🐞 Fixed Calculation for WeekPicker. [#3214](https://github.com/ant-design-blazor/ant-design-blazor/pull/3214) [@sebastian-wachsmuth](https://github.com/sebastian-wachsmuth)
  - 🐞 Fixed un-representable DateTime when day > daysInMonth. [#3193](https://github.com/ant-design-blazor/ant-design-blazor/pull/3193) [@Alexbits](https://github.com/Alexbits)


- 🆕 Add Typography editable text support. [#3173](https://github.com/ant-design-blazor/ant-design-blazor/pull/3173) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add Mentions ability to customize the rendering of the textarea. [#3178](https://github.com/ant-design-blazor/ant-design-blazor/pull/3178) [@wss-kroche](https://github.com/wss-kroche)
- 🆕 Add Menu `ShowCollapsedTooltip` parameter to handle Tooltip display. [#3226](https://github.com/ant-design-blazor/ant-design-blazor/pull/3226) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add Descriptions `LabelStyle` and `ContentStyle` for DescriptionItem custom styles. [#3186](https://github.com/ant-design-blazor/ant-design-blazor/pull/3186) [@ElderJames](https://github.com/ElderJames)
- 🛠 Add InputNumber the id attribute on internal input element. [#3198](https://github.com/ant-design-blazor/ant-design-blazor/pull/3198) [@varbedi](https://github.com/varbedi)
- 🛠 Refactor Form expose the feedback status of FormItem for the input component base class. [#3227](https://github.com/ant-design-blazor/ant-design-blazor/pull/3227) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Dropdown not hiding after clicking the selected menuitem. [#3231](https://github.com/ant-design-blazor/ant-design-blazor/pull/3231) [@huangjia2107](https://github.com/huangjia2107)
- 🐞 Fixed Input that read spaces or empty strings as null. [#3190](https://github.com/ant-design-blazor/ant-design-blazor/pull/3190) [@berkerdong](https://github.com/berkerdong)
- 🐞 Fixed Image preview operations would be covered by the preview image. [#3170](https://github.com/ant-design-blazor/ant-design-blazor/pull/3170) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Checkbox that CheckboxGroup will report an error when the internal Checkbox is null. [#3162](https://github.com/ant-design-blazor/ant-design-blazor/pull/3162) [@berkerdong](https://github.com/berkerdong)
- 🐞 Fixed Pagination mini class name was changed. [#3266](https://github.com/ant-design-blazor/ant-design-blazor/pull/3266) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed AutoComplete that can't be selected. (#3252). [7d24d09](https://github.com/ant-design-blazor/ant-design-blazor/commit/7d24d09) [@James Yeung](https://github.com/James Yeung)
- 🐞 Fixed Drawer that add `type="button"` to close button to avoid submitting form. [#3233](https://github.com/ant-design-blazor/ant-design-blazor/pull/3233) [@trafium](https://github.com/trafium)




### 0.14.4

`2023-03-01`

- 🐞 Fixed Radio avoid infinite loop when the binding vaule is not in options. [#3123](https://github.com/ant-design-blazor/ant-design-blazor/pull/3123) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed InputNumber allow passing Style and ID to InputNumber without a wrapper. [#3144](https://github.com/ant-design-blazor/ant-design-blazor/pull/3144) [@Epictek](https://github.com/Epictek)
- 🐞 Fixed Select that OnSelectedItemsChanged is not triggered in form. [#3129](https://github.com/ant-design-blazor/ant-design-blazor/pull/3129) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Tree that add preventdefault for oncontextmenu. [#3076](https://github.com/ant-design-blazor/ant-design-blazor/pull/3076) [@AndrewKaninchen](https://github.com/AndrewKaninchen)
- 🐞 Fixed Pagination avoid trigger ChangeSize while the size was not be changed. [#3133](https://github.com/ant-design-blazor/ant-design-blazor/pull/3133) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Transfer that add ListStyle to custom the css for columns. [#3139](https://github.com/ant-design-blazor/ant-design-blazor/pull/3139) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Tabs that ReuseTabs occur error when the base url is not the default. [#3142](https://github.com/ant-design-blazor/ant-design-blazor/pull/3142) [@berkerdong](https://github.com/berkerdong)
- 🐞 Fixed AutoComplete that fill the input component only if Backfill is true. [#3140](https://github.com/ant-design-blazor/ant-design-blazor/pull/3140) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed DatePicker that DisabledDate does not affect keyboard input. [#3134](https://github.com/ant-design-blazor/ant-design-blazor/pull/3134) [@Alexbits](https://github.com/Alexbits)
- 🐞 Fixed Input that avoid textarea call jsinterop before rendering. [#3128](https://github.com/ant-design-blazor/ant-design-blazor/pull/3128) [@ElderJames](https://github.com/ElderJames)


### 0.14.3

`2023-02-19`

- Popconfirm
  - 🐞 Fixed icon missing color. [#3093](https://github.com/ant-design-blazor/ant-design-blazor/pull/3093) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed missing built-in localization. [#3095](https://github.com/ant-design-blazor/ant-design-blazor/pull/3095) [@ElderJames](https://github.com/ElderJames)

- Pagination
  - 🐞 Fixed `DefaultCurrent` doesn't work. [#3085](https://github.com/ant-design-blazor/ant-design-blazor/pull/3085) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed jump button icon direction in RTL language. [#3084](https://github.com/ant-design-blazor/ant-design-blazor/pull/3084) [@ElderJames](https://github.com/ElderJames)

- Modal
  - 🐞 Fixed every keydown will trigger Modal re-render. [#3099](https://github.com/ant-design-blazor/ant-design-blazor/pull/3099) [@zxyao145](https://github.com/zxyao145)
  - 🛠 Refactor ConfirmService to use interface. [#3083](https://github.com/ant-design-blazor/ant-design-blazor/pull/3083) [@wss-awachowicz](https://github.com/wss-awachowicz)

- 🐞 Fixed Overlay popup cannot picked in drawer. [#3106](https://github.com/ant-design-blazor/ant-design-blazor/pull/3106) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed Textarea not rendering maxlength on textarea. [#3108](https://github.com/ant-design-blazor/ant-design-blazor/pull/3108) [@wss-kroche](https://github.com/wss-kroche)
- 🐞 Fixed Tabs supports Reusetabs title update. [#3088](https://github.com/ant-design-blazor/ant-design-blazor/pull/3088) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed radio can't be selected while radio list and value of RadioGroup was changed at the same time. [#3098](https://github.com/ant-design-blazor/ant-design-blazor/pull/3098) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Segmented exception cause by label index was changed. [#3096](https://github.com/ant-design-blazor/ant-design-blazor/pull/3096) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Table default pagination position in RTL language. [#3086](https://github.com/ant-design-blazor/ant-design-blazor/pull/3086) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Select don't `OnSelectedItemsChanged` triggers. [#3079](https://github.com/ant-design-blazor/ant-design-blazor/pull/3079) [@m-khrapunov](https://github.com/m-khrapunov)
- 🐞 Fixed Menu title padding direction in RTL language. [#3080](https://github.com/ant-design-blazor/ant-design-blazor/pull/3080) [@ElderJames](https://github.com/ElderJames)


### 0.14.2

`2023-02-06`

- 🐞 Fixed Menu incorrect submenu styles in RTL language. [#3065](https://github.com/ant-design-blazor/ant-design-blazor/pull/3065) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Tabs that reusetabs null reference exception. [#3060](https://github.com/ant-design-blazor/ant-design-blazor/pull/3060) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed drawer mask not disappear. [#3059](https://github.com/ant-design-blazor/ant-design-blazor/pull/3059) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed Calendar wrong selected date. [#3069](https://github.com/ant-design-blazor/ant-design-blazor/pull/3069) [@agolub-s](https://github.com/agolub-s)

### 0.14.1

`2023-02-01`

- 🐞 Fixed Notification RTL incorrect style, and add top and bottom placement support; [#3049](https://github.com/ant-design-blazor/ant-design-blazor/pull/3049) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed Table `PageSize` can't update once the datasource is changed while `HidePagination` is enabled. [#3052](https://github.com/ant-design-blazor/ant-design-blazor/pull/3052) [@wss-javeney](https://github.com/wss-javeney)
- 🐞 Fixed Tabs that ReuseTabs keep obsoleted usage. [#3051](https://github.com/ant-design-blazor/ant-design-blazor/pull/3051) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Tree OnContextMenu event not firing. [#3042](https://github.com/ant-design-blazor/ant-design-blazor/pull/3042) [@AndrewKaninchen](https://github.com/AndrewKaninchen)
- 🐞 Fixed Select order issues with Select two-way binding selected items. [#3037](https://github.com/ant-design-blazor/ant-design-blazor/pull/3037) [@CuteLeon](https://github.com/CuteLeon)
- 🐞Fixed Drawer mask not closing immediately. [#3047](https://github.com/ant-design-blazor/ant-design-blazor/pull/3047) [@zxyao145](https://github.com/zxyao145)
- 🛠 Marked multiple redundant parameters as obsolete for future removal:  `Calendar.OnSelect`, `Card.Body`, `Sider.OnCollapse`, `PageHeader.PageHeaderTitle`, `PageHeader.PageHeaderSubtitle`, `Radio.CheckedChange`. [#3035](https://github.com/ant-design-blazor/ant-design-blazor/pull/3035) [@kooliokey](https://github.com/kooliokey)

### 0.14.0

`2023-01-26`

Happy Chinese New Year of rabbit!

- Table
  - 🆕 supports automatic column generation based on the `TItem` type. [#2978](https://github.com/ant-design-blazor/ant-design-blazor/pull/2978) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add support for header and row grouping. [#2973](https://github.com/ant-design-blazor/ant-design-blazor/pull/2973) [@anranruye](https://github.com/anranruye)
  - 🆕 Add empty template parameter and make it fixed while column scrolling. [#3031](https://github.com/ant-design-blazor/ant-design-blazor/pull/3031) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed thrown null reference exception  in Selection column. [#3028](https://github.com/ant-design-blazor/ant-design-blazor/pull/3028) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed column title align incorrectly when filterable. [#3023](https://github.com/ant-design-blazor/ant-design-blazor/pull/3023) [@ElderJames](https://github.com/ElderJames)

- Input
  - 🆕 Add ShowCount. [#3033](https://github.com/ant-design-blazor/ant-design-blazor/pull/3033) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add EventCallback OnClear. [#3020](https://github.com/ant-design-blazor/ant-design-blazor/pull/3020) [@Abin-Liu](https://github.com/Abin-Liu)

- Menu
  - 🆕 Add `PopupClassName` parameter. [#3027](https://github.com/ant-design-blazor/ant-design-blazor/pull/3027) [@JustGentle](https://github.com/JustGentle)
  - 🐞 Fixed submenu montion & style. [#3024](https://github.com/ant-design-blazor/ant-design-blazor/pull/3024) [@ElderJames](https://github.com/ElderJames)

- Transfer
  - 🐞 Fixed throw exception when it's in Form. [#3015](https://github.com/ant-design-blazor/ant-design-blazor/pull/3015) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed can't select items. [#3011](https://github.com/ant-design-blazor/ant-design-blazor/pull/3011) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed DataSource refresh. [#2998](https://github.com/ant-design-blazor/ant-design-blazor/pull/2998) [@ElderJames](https://github.com/ElderJames)

- InputNumber
  - 🆕 Add borderless style. [#3019](https://github.com/ant-design-blazor/ant-design-blazor/pull/3019) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed step rollback and null reference exception. [#3018](https://github.com/ant-design-blazor/ant-design-blazor/pull/3018) [@ElderJames](https://github.com/ElderJames)

- Tabs
  - 🛠 refactor ReuseTabs remove `ReuseTabsRouteView` to reduce coupling to native components. [#3009](https://github.com/ant-design-blazor/ant-design-blazor/pull/3009) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed ReuseTabs close other tab rendering error. [#3002](https://github.com/ant-design-blazor/ant-design-blazor/pull/3002) [@berkerdong](https://github.com/berkerdong)
  - 🐞 Fixed Tabs exception at first load while activekey is specificed to a disabled tab. [#2997](https://github.com/ant-design-blazor/ant-design-blazor/pull/2997) [@ElderJames](https://github.com/ElderJames)


- 🆕 Add theme service. [#2883](https://github.com/ant-design-blazor/ant-design-blazor/pull/2883) [@melinyi](https://github.com/melinyi)
- 🆕 Add DatePicker selected week range visualization. [#2892](https://github.com/ant-design-blazor/ant-design-blazor/pull/2892) [@Alexbits](https://github.com/Alexbits)
- 🆕 Add Radio cascading type parameter for RadioGroup. [#3022](https://github.com/ant-design-blazor/ant-design-blazor/pull/3022) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Overlay would show after mouse leave the trigger. [#3025](https://github.com/ant-design-blazor/ant-design-blazor/pull/3025) [@JustGentle](https://github.com/JustGentle)
- 🐞 Fixed ResizeObserver work incorrectly cause by wrong key type. [#3030](https://github.com/ant-design-blazor/ant-design-blazor/pull/3030) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Select bug where selected options will display even with HideSelected set to true when searching or clearing search. [#3010](https://github.com/ant-design-blazor/ant-design-blazor/pull/3010) [@wss-kroche](https://github.com/wss-kroche)
- 🐞 Fixed Form validation status styles. [#3005](https://github.com/ant-design-blazor/ant-design-blazor/pull/3005) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Cascader boudary adjust mode default to InView. [#2999](https://github.com/ant-design-blazor/ant-design-blazor/pull/2999) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Descriptions invalid messages in Console. [#3012](https://github.com/ant-design-blazor/ant-design-blazor/pull/3012) [@berkerdong](https://github.com/berkerdong)
- 💄 sync ant-design v4.24.2. [#2877](https://github.com/ant-design-blazor/ant-design-blazor/pull/2877) [@ElderJames](https://github.com/ElderJames)


#### Breaking Changes

- Table : `RowTemplate` was Changed to `ColumnDefinitions`。`RowTemplate` was originally used for the `Column` definition, but this version was changed to define the row template.
- ReuseTabs: `ReuseTabsRouteView` and `AuthorizeReuseTabsRouteView` have been marked as obsolete. Please use `<CascadingValue Value="routeData">` to wrap `<RouteView>` or `<AuthorizeRouteView>`.

  See：

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
  - 🐞 Fixed tags mode will retain tag options when datasource is empty. [#2986](https://github.com/ant-design-blazor/ant-design-blazor/pull/2986) [@wss-javeney](https://github.com/wss-javeney)
  - 🐞 Fixed dropdown boundary adjust mode defult to InView. [#2995](https://github.com/ant-design-blazor/ant-design-blazor/pull/2995) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed search input visual width adjustment issue. [#2994](https://github.com/ant-design-blazor/ant-design-blazor/pull/2994) [@ElderJames](https://github.com/ElderJames)
  
- AutoComplete
  - 🐞 Fixed dropdown would open when page was render. [#2992](https://github.com/ant-design-blazor/ant-design-blazor/pull/2992) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed the composition session and debounce  for input. [#2988](https://github.com/ant-design-blazor/ant-design-blazor/pull/2988) [@ElderJames](https://github.com/ElderJames)

- Tabs
  - 🐞 Fixed exception at first rendering when the first TabPane is set Disabled. [#2982](https://github.com/ant-design-blazor/ant-design-blazor/pull/2982) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed active pane after tabs is dispsed. [#2981](https://github.com/ant-design-blazor/ant-design-blazor/pull/2981) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed close exception, remove dispose call after event listener is removed. [#2980](https://github.com/ant-design-blazor/ant-design-blazor/pull/2980) [@ElderJames](https://github.com/ElderJames)

- 🐞 Fixed Transfer refresh data on change SelectedKeys or TargetKeys parameters. [#2977](https://github.com/ant-design-blazor/ant-design-blazor/pull/2977) [@Magehernan](https://github.com/Magehernan)
- 🐞 Fixed TreeSelect value bind incorrectly when default value was set. [#2990](https://github.com/ant-design-blazor/ant-design-blazor/pull/2990) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Input Search wrong style with clear button. [#2991](https://github.com/ant-design-blazor/ant-design-blazor/pull/2991) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed RangePicker disabled date logic to always apply default logic even when custom is provided. This keeps ranges in the proper order even with custom disabled logic. [#2947](https://github.com/ant-design-blazor/ant-design-blazor/pull/2947) [@wss-kroche](https://github.com/wss-kroche)

### 0.13.2

`2022-12-31`

- Table
  - 📖 Docs add a search box in the filter & sorter demo to enable custom filtering. [#2955](https://github.com/ant-design-blazor/ant-design-blazor/pull/2955) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed Selection Component Hidden Property not working. [#2945](https://github.com/ant-design-blazor/ant-design-blazor/pull/2945) [@berkerdong](https://github.com/berkerdong)
  - 🐞 Fixed `Hidden` parameter for ActionColumn doesn't work. [#2946](https://github.com/ant-design-blazor/ant-design-blazor/pull/2946) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed IQueryable or IEnumerable using AsNoTracking will cause select data duplication bug. [#2944](https://github.com/ant-design-blazor/ant-design-blazor/pull/2944) [@berkerdong](https://github.com/berkerdong)
  - 🐞 Fxied can't restore the query state of filters which value is enum type. [#2941](https://github.com/ant-design-blazor/ant-design-blazor/pull/2941) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed Infinite loop when HidePagination and PageSize were set at the same time and datasource is empty. [#2919](https://github.com/ant-design-blazor/ant-design-blazor/pull/2919) [@ElderJames](https://github.com/ElderJames)

- DatePicker
  - 🆕 Use SuffixIcon passed to RangePicker to allow for a custom suffix icon. [#2935](https://github.com/ant-design-blazor/ant-design-blazor/pull/2935) [@wss-javeney](https://github.com/wss-javeney)
  - 🐞 Fixed Exception on input with time when Value is null. [#2920](https://github.com/ant-design-blazor/ant-design-blazor/pull/2920) [@Alexbits](https://github.com/Alexbits)

- Input
  - 🐞 Fixed the `OnChange` event would be triggered three times and the clear button would not be displayed with the `Suffix` template. [#2970](https://github.com/ant-design-blazor/ant-design-blazor/pull/2970) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed null reference exception on dispose. [#2966](https://github.com/ant-design-blazor/ant-design-blazor/pull/2966) [@dracan](https://github.com/dracan)

- 🆕 Add TreeSelect TitleTemplate for tree nodes. [#2940](https://github.com/ant-design-blazor/ant-design-blazor/pull/2940) [@rhodon-jargon](https://github.com/rhodon-jargon)
- 🆕 Add RequiredMark to Form to allow displaying indicators next to required, optional or no fields. [#2930](https://github.com/ant-design-blazor/ant-design-blazor/pull/2930) [@wss-kroche](https://github.com/wss-kroche)
- 🐞 Fixed Tabs some issues with dynamic rendering. [#2967](https://github.com/ant-design-blazor/ant-design-blazor/pull/2967) [@ElderJames](https://github.com/ElderJames)
- 🛠 Refactor Notification that add an interface INotificationService. It is backwards compatible, but new code should inject INotificationService. [#2948](https://github.com/ant-design-blazor/ant-design-blazor/pull/2948) [@wss-javeney](https://github.com/wss-javeney)
- 🐞 Fixed InputNumber triggering a constant Increase/Decrease in certain scenarios. [#2953](https://github.com/ant-design-blazor/ant-design-blazor/pull/2953) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Statistic CountDown format incorrectly when publish with trimming. [#2943](https://github.com/ant-design-blazor/ant-design-blazor/pull/2943) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed `ClassMapper` would get twice css classes. [#2934](https://github.com/ant-design-blazor/ant-design-blazor/pull/2934) [@berkerdong](https://github.com/berkerdong)
- 🐞 Fixed built-in System.Text.Json for netstandard2.1 target to avoid compatibility exceptions. [#2922](https://github.com/ant-design-blazor/ant-design-blazor/pull/2922) [@ElderJames](https://github.com/ElderJames)

### 0.13.1

`2022-11-29`

- 🐞 Fixed Input that when its value is changed in code, it would rollback on click. [#2906](https://github.com/ant-design-blazor/ant-design-blazor/pull/2906) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Table infinite loop when HidePagination ant PageSize was set at same time. [#2905](https://github.com/ant-design-blazor/ant-design-blazor/pull/2905) [@ElderJames](https://github.com/ElderJames)

### 0.13.0

`2022-11-22`

- 🔥 Add .NET 7 as target framework. [#2810](https://github.com/ant-design-blazor/ant-design-blazor/pull/2810) [@ElderJames](https://github.com/ElderJames)
- 🔥 Refactor mentions, fixed positioning and hiding issues. [#2874](https://github.com/ant-design-blazor/ant-design-blazor/pull/2874) [@dingyanwu](https://github.com/dingyanwu)

- Datepicker
  - 🆕 Add OnOk event. [#2840](https://github.com/ant-design-blazor/ant-design-blazor/pull/2840) [@Alexbits](https://github.com/Alexbits)
  - 🐞 Fixed RangePicker's Week selection mode where the week range was disabled across the year. [#2889](https://github.com/ant-design-blazor/ant-design-blazor/pull/2889) [@Alexbits](https://github.com/Alexbits)

- Table
  - 📖 Add OData query demo . [#2861](https://github.com/ant-design-blazor/ant-design-blazor/pull/2861) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add if HidePagination is true,set PagSize value is DataSource Count. [#2476](https://github.com/ant-design-blazor/ant-design-blazor/pull/2476) [@CareyYang](https://github.com/CareyYang)

- Modal 
  - 🆕 Add only one OK footer and onlt one Cancel footer. [#2812](https://github.com/ant-design-blazor/ant-design-blazor/pull/2812) [@zxyao145](https://github.com/zxyao145)
  - 🆕 Add default maximization of Modal initialization. [#2834](https://github.com/ant-design-blazor/ant-design-blazor/pull/2834) [@zxyao145](https://github.com/zxyao145)

- Input
  - 🆕 Add input binding parameter BindOnInput, default binding event changed to onchange. [#2838](https://github.com/ant-design-blazor/ant-design-blazor/pull/2838) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed convert error status work incorrectly. [#2846](https://github.com/ant-design-blazor/ant-design-blazor/pull/2846) [@ElderJames](https://github.com/ElderJames)

- 🆕 Add Tree the `DropBelow` for Drag&drop event to flag whether to drop dragged node as a sibling (below) or as a child of target node. [#2864](https://github.com/ant-design-blazor/ant-design-blazor/pull/2864) [@AndrewKaninchen](https://github.com/AndrewKaninchen)
- 🆕 Add Cascader `Disabled` parameter. [#2835](https://github.com/ant-design-blazor/ant-design-blazor/pull/2835) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add DownloadButton. [#2819](https://github.com/ant-design-blazor/ant-design-blazor/pull/2819) [@LeaFrock](https://github.com/LeaFrock)
- 🆕 Add Drawer `HeaderStyle` parameter. [#2809](https://github.com/ant-design-blazor/ant-design-blazor/pull/2809) [@danielbotn](https://github.com/danielbotn)
- 💄 Add Dropdown `Arrow` parameter. [#2795](https://github.com/ant-design-blazor/ant-design-blazor/pull/2795) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add InputNumber `Precision` parameter. [#2774](https://github.com/ant-design-blazor/ant-design-blazor/pull/2774) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- 🆕 Add Tooltip component uses RenderFragment TitleTeplate instead OneOf Title. [#2711](https://github.com/ant-design-blazor/ant-design-blazor/pull/2711) [@CAPCHIK](https://github.com/CAPCHIK)
- 🆕 Add Select virtualization support. [#2654](https://github.com/ant-design-blazor/ant-design-blazor/pull/2654) [@m-khrapunov](https://github.com/m-khrapunov)
- 🌐 Fix cs-CZ locale wrong shortWeekDays for cs-CZ. [#2866](https://github.com/ant-design-blazor/ant-design-blazor/pull/2866) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Segmented binding value type causes an incorrect initialization selection. [#2869](https://github.com/ant-design-blazor/ant-design-blazor/pull/2869) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed docs demo anchor case. [#2826](https://github.com/ant-design-blazor/ant-design-blazor/pull/2826) [@kooliokey](https://github.com/kooliokey)
- 🐞 Fixed Menu that the arrow of submenu has no animation effect when it is expanded and collapsed. [#2876](https://github.com/ant-design-blazor/ant-design-blazor/pull/2876) [@wangj90](https://github.com/wangj90)


### 0.12.7

`2022-11-6`

- DatePicker
  - 🐞 Fixed wrong day order in some locales and fallback to use Globalization libaray when there is no day locale. [#2855](https://github.com/ant-design-blazor/ant-design-blazor/pull/2855) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed `smoothScrollTo` causes an infinite loop. [#2854](https://github.com/ant-design-blazor/ant-design-blazor/pull/2854) [@Alexbits](https://github.com/Alexbits)
  - 🐞 fix day order in calendar header for russian locale. [#2845](https://github.com/ant-design-blazor/ant-design-blazor/pull/2845) [@ocoka](https://github.com/ocoka)
  - 🐞 Fixed tab key does not confirm the value. [#2847](https://github.com/ant-design-blazor/ant-design-blazor/pull/2847) [@Alexbits](https://github.com/Alexbits)

- Core
  - ✅ Improve unit tests cover for Core module. [#2821](https://github.com/ant-design-blazor/ant-design-blazor/pull/2821) [@LeaFrock](https://github.com/LeaFrock)
  - ⚡️ Optimize CssSizeLength and CssStyleBuilder. [#2803](https://github.com/ant-design-blazor/ant-design-blazor/pull/2803) [@LeaFrock](https://github.com/LeaFrock)

- 🐞 Fixed Tabs support of tab bar css style and class. [#2844](https://github.com/ant-design-blazor/ant-design-blazor/pull/2844) [@ldsenow](https://github.com/ldsenow)
- 🐞 Fixed BackTop doesn't remove the dom when visible is false. [#2831](https://github.com/ant-design-blazor/ant-design-blazor/pull/2831) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed bug where Content wouldn't render in Drawer if it was a string and not RenderFragment. [#2833](https://github.com/ant-design-blazor/ant-design-blazor/pull/2833) [@kooliokey](https://github.com/kooliokey)
- 🐞 Fixed bug where Title parameter was not being rendered. [#2830](https://github.com/ant-design-blazor/ant-design-blazor/pull/2830) [@kooliokey](https://github.com/kooliokey)
- 🐞 Fixed Slider accessibility updates with aria labels. [#2818](https://github.com/ant-design-blazor/ant-design-blazor/pull/2818) [@kooliokey](https://github.com/kooliokey)
- 🐞 Fixed Table exception during page navigation [#2797](https://github.com/ant-design-blazor/ant-design-blazor/pull/2797) [@Kyojuro27](https://github.com/Kyojuro27)
- 🐞 Fixed bug with tag color change after render not always styling properly. [#2816](https://github.com/ant-design-blazor/ant-design-blazor/pull/2816) [@kooliokey](https://github.com/kooliokey)
- 🐞 Fixed Cascader AllowClear was not working when false. [#2792](https://github.com/ant-design-blazor/ant-design-blazor/pull/2792) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- 🐞 Fixed AutoComplete search panel show. [#2793](https://github.com/ant-design-blazor/ant-design-blazor/pull/2793) [@lyj0309](https://github.com/lyj0309)
- 💄 Fixed Menu that class name of the expand icon for submenu. [#2796](https://github.com/ant-design-blazor/ant-design-blazor/pull/2796) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix descriptions component miss div element. [#2798](https://github.com/ant-design-blazor/ant-design-blazor/pull/2798) [@Weilence](https://github.com/Weilence)
- 🐞 Fixed Upload should get error raw response. [#2858](https://github.com/ant-design-blazor/ant-design-blazor/pull/2858) [@yosheng](https://github.com/yosheng)

### 0.12.6

`2022-10-11`

- 🐞 Fixed JS event listener registration. [#2783](https://github.com/ant-design-blazor/ant-design-blazor/pull/2783) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Segmented that the Disabled parameter does not work on items and cannot be dynamically toggled. [#2778](https://github.com/ant-design-blazor/ant-design-blazor/pull/2778) [@ElderJames](https://github.com/ElderJames)
- 🐞 Removing the gulp task to exclude empty files. [#2779](https://github.com/ant-design-blazor/ant-design-blazor/pull/2779) [@paulsuart](https://github.com/paulsuart)


### 0.12.5

`2022-10-09`

- Datepicker
  - 🐞 Fixed correct culture not applied when manual input. [#2715](https://github.com/ant-design-blazor/ant-design-blazor/pull/2715) [@Alexbits](https://github.com/Alexbits)
 - 🐞 Fixed a series of issues to make Datepicker and RangePicker behave more like antd. [#2741](https://github.com/ant-design-blazor/ant-design-blazor/pull/2741) [@Alexbits](https://github.com/Alexbits)
    - Fixed an issue with the OnChange event passing in an old value.
    - Fixed RangePicker head not switching year.
    - Fixed RangePicker selection panel display problem when both start and end in the same period.
    - Fixed an issue where the end date was not highlighted when the RangePicker was selected in week mode.
    - The start date is not highlighted during the end date input in the date picker with the time.
    - Other minor fixes and refactorings

- Modal
  - 🐞 Fixed maximizable not work when using Title. [#2750](https://github.com/ant-design-blazor/ant-design-blazor/pull/2750) [@zxyao145](https://github.com/zxyao145)
  - 🐞 Fixed Confirm dialog closeable. [#2776](https://github.com/ant-design-blazor/ant-design-blazor/pull/2776) [@zxyao145](https://github.com/zxyao145)

- 🐞 Fixed Core that remove the event listener when the component is disposed. [#2738](https://github.com/ant-design-blazor/ant-design-blazor/pull/2738) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Radio that the disabled parameter for RadioGroup with <code class="notranslate">RadioOption&lt;TValue&gt;</code> options doesn't work. [#2744](https://github.com/ant-design-blazor/ant-design-blazor/pull/2744) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Table that set value for table header checkbox Disabled attribute. [#2737](https://github.com/ant-design-blazor/ant-design-blazor/pull/2737) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- ⚡️ Fixed Select that remove redundant CreateDeleteSelectOptions() calls in render cycles. [#2657](https://github.com/ant-design-blazor/ant-design-blazor/pull/2657) [@m-khrapunov](https://github.com/m-khrapunov)
- 🛠  Fixed gulp pipeline to include less files so they end up in /staticwebassets/less in the nuget package. [#2730](https://github.com/ant-design-blazor/ant-design-blazor/pull/2730) [@paulsuart](https://github.com/paulsuart)

### 0.12.4

`2022-09-14`

- 🐞 Fixed Table exceptions caused by  sort. [#2710](https://github.com/ant-design-blazor/ant-design-blazor/pull/2710) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- 🐞 Fixed AutoComplete that void browser's autocomplete popup.[#2708](https://github.com/ant-design-blazor/ant-design-blazor/pull/2708) [@lyj0309](https://github.com/lyj0309)
- 🐞 Fixed DatePicker that several issues with RangePicker [#2707](https://github.com/ant-design-blazor/ant-design-blazor/pull/2707) [@Alexbits](https://github.com/Alexbits):
- RangePicker end panel not shown in RTL mode
- RangePicker range preset value resets when time input is enabled
- RangePicker keeps focus when input canceled
- RangePicker cannot clear value when one of the inputs has a focus

### 0.12.3

`2022-09-13`

🥮Happy Mid-Autumn Festival!

- 🐞 Fixed TreeSelect that support Searching [#2686](https://github.com/ant-design-blazor/ant-design-blazor/pull/2686) [@Magehernan](https://github.com/Magehernan)
- 🆕 Add Grid alias GridRow for Row to be consistent with GridCol. [#2690](https://github.com/ant-design-blazor/ant-design-blazor/pull/2690) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- 🐞 Fixed Message that exception casue by Invariant Globalization setting. [#2697](https://github.com/ant-design-blazor/ant-design-blazor/pull/2697) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Segmentd default value binding incorrectly with options. [#2699](https://github.com/ant-design-blazor/ant-design-blazor/pull/2699) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Table multiple sort doesn't work with EFCore [#2701](https://github.com/ant-design-blazor/ant-design-blazor/pull/2701) [@YongQuan-dotnet](https://github.com/YongQuan-dotnet)
- 📖 Added demos for exception handling, respectively in Alert, Result and Notification. [#2706](https://github.com/ant-design-blazor/ant-design-blazor/pull/2706) [#2703](https://github.com/ant-design-blazor/ant-design-blazor/pull/2703) [@ElderJames](https://github.com/ElderJames)

### 0.12.2

`2022-09-08`

- Table
  - 🐞 Fixed Converting IQueryable to IOrderedQueryable returned null. [#2687](https://github.com/ant-design-blazor/ant-design-blazor/pull/2687) [@JamesGit-hash](https://github.com/JamesGit-hash)
  - 🐞 Fixed the exception cause by reload data with state and the table has an ActionColumn. [#2683](https://github.com/ant-design-blazor/ant-design-blazor/pull/2683) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Tabs panel  display incorrectly without animated [#2677](https://github.com/ant-design-blazor/ant-design-blazor/pull/2677) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed DatePicker that an exception is caused when passes the value as null to RangePicker [#2688](https://github.com/ant-design-blazor/ant-design-blazor/pull/2688) [@ElderJames](https://github.com/ElderJames)


### 0.12.1

`2022-09-04`

- Tabs
  - 🐞 Fix tabs content overflow at animated mode. [#2671](https://github.com/ant-design-blazor/ant-design-blazor/pull/2671) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fix reusetabs invaild uri exception. [#2663](https://github.com/ant-design-blazor/ant-design-blazor/pull/2663) [@ElderJames](https://github.com/ElderJames)

- Icon
  - 📖 Fix Icon that add missing ZoomOut outline icon. [#2667](https://github.com/ant-design-blazor/ant-design-blazor/pull/2667) [@kooliokey](https://github.com/kooliokey)
  - 🐞 Fix Icon that state can't update & optimize the  first rendering for two-tone icon. [#2666](https://github.com/ant-design-blazor/ant-design-blazor/pull/2666) [@ElderJames](https://github.com/ElderJames)

- 🐞 Fix collapse outdated html structures. [#2668](https://github.com/ant-design-blazor/ant-design-blazor/pull/2668) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fix Breadcrumb that add OnClick parameter to BreadcrumbItem. Markup of Breadcrumb updated to match Ant.Design React. This could break custom CSS targeting this component's resulting markup. [#2655](https://github.com/ant-design-blazor/ant-design-blazor/pull/2655) [@kooliokey](https://github.com/kooliokey)
- 🐞 Fix Datepicker that timepicker value would resets to default. [#2660](https://github.com/ant-design-blazor/ant-design-blazor/pull/2660) [@Alexbits](https://github.com/Alexbits)
- 📖 Fix the presetted ranges demo of datepicker which would casuse …. [#2659](https://github.com/ant-design-blazor/ant-design-blazor/pull/2659) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fix Avatar that make size parameter support more length unit. [#2653](https://github.com/ant-design-blazor/ant-design-blazor/pull/2653) [@ElderJames](https://github.com/ElderJames)
- 📖 Fix demo & document edit url. [#2661](https://github.com/ant-design-blazor/ant-design-blazor/pull/2661) [@ElderJames](https://github.com/ElderJames)

### 0.12.0

`2022-08-29`

- 🔥 Sync ant-design v4.20.7 styles. [#2497](https://github.com/ant-design-blazor/ant-design-blazor/pull/2497) [@ElderJames](https://github.com/ElderJames)
- 🔥 Add segmented component. [#2503](https://github.com/ant-design-blazor/ant-design-blazor/pull/2503) [@ElderJames](https://github.com/ElderJames)
- 🔥 Add Table PropertyColumn. [#2624](https://github.com/ant-design-blazor/ant-design-blazor/pull/2624) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add ReuseTabs pinned tabs. [#2545](https://github.com/ant-design-blazor/ant-design-blazor/pull/2545) [@HaoZhiYing](https://github.com/HaoZhiYing)
- 🆕 Add PageHeader responsive compact style. [#2606](https://github.com/ant-design-blazor/ant-design-blazor/pull/2606) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add ResizeObserver component. [#2605](https://github.com/ant-design-blazor/ant-design-blazor/pull/2605) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add List draggable item demo. [#2563](https://github.com/ant-design-blazor/ant-design-blazor/pull/2563) [@charset](https://github.com/charset)
- 🆕 Add Tooltip TabIndex parameter. [#2567](https://github.com/ant-design-blazor/ant-design-blazor/pull/2567) [@lukblazewicz](https://github.com/lukblazewicz)
- 🆕 Add Drawer OnOpen Event. [#2553](https://github.com/ant-design-blazor/ant-design-blazor/pull/2553) [@zxyao145](https://github.com/zxyao145)

- Icon
  - 🔥 Add Two-tone color icon implements. [#2513](https://github.com/ant-design-blazor/ant-design-blazor/pull/2513) [@rqx110](https://github.com/rqx110)
  - 🐞 Fix icon demo exception when prerendering. [#2527](https://github.com/ant-design-blazor/ant-design-blazor/pull/2527) [@ElderJames](https://github.com/ElderJames)

- Modal
  - 🆕 Add Modal maximize within the browser. [#2573](https://github.com/ant-design-blazor/ant-design-blazor/pull/2573) [@zxyao145](https://github.com/zxyao145)
  - 🐞 Fix reset enter animation className. [#2561](https://github.com/ant-design-blazor/ant-design-blazor/pull/2561) [@zxyao145](https://github.com/zxyao145)

- Datepicker
  - 🆕 Add Scroll to selected time in DatePicker/TimePicker. [#2512](https://github.com/ant-design-blazor/ant-design-blazor/pull/2512) [@Alexbits](https://github.com/Alexbits)
  - 🆕 Add 12-hour time support. [#2501](https://github.com/ant-design-blazor/ant-design-blazor/pull/2501) [@Alexbits](https://github.com/Alexbits)
  - 🆕 Add preset range for RangePicker. [#2487](https://github.com/ant-design-blazor/ant-design-blazor/pull/2487) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
  - 🐞 Fix culture is not applied correctly to the year. [#2589](https://github.com/ant-design-blazor/ant-design-blazor/pull/2589) [@Alexbits](https://github.com/Alexbits)
  - 🐞 Fix wrong day headers when FirstDayOfWeek!=Sunday. [#2571](https://github.com/ant-design-blazor/ant-design-blazor/pull/2571) [@Alexbits](https://github.com/Alexbits)
  - 🐞 Fix date selection. [#2570](https://github.com/ant-design-blazor/ant-design-blazor/pull/2570) [@Alexbits](https://github.com/Alexbits)
  - 🐞 Fix value not updated when changed programmatically. [#2551](https://github.com/ant-design-blazor/ant-design-blazor/pull/2551) [@Alexbits](https://github.com/Alexbits)
  - 🐞 Fix ok button issues for pickers. [#2531](https://github.com/ant-design-blazor/ant-design-blazor/pull/2531) [@Alexbits](https://github.com/Alexbits)    

- Image
  - 🆕 Add controlled preview support. [#2600](https://github.com/ant-design-blazor/ant-design-blazor/pull/2600) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fix an image which was setted after a fallback image can't display in preview. [#2599](https://github.com/ant-design-blazor/ant-design-blazor/pull/2599) [@ElderJames](https://github.com/ElderJames)

- Form
  - 🐞 Fix error status for entry components. [#2647](https://github.com/ant-design-blazor/ant-design-blazor/pull/2647) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fix Select was not highlighted when validation failed. [#2642](https://github.com/ant-design-blazor/ant-design-blazor/pull/2642) [@JamesGit-hash](https://github.com/JamesGit-hash)
  - 🐞 Fix Input wrong sytle of error status. [#2639](https://github.com/ant-design-blazor/ant-design-blazor/pull/2639) [@JamesGit-hash](https://github.com/JamesGit-hash)

- Cascader
  - 💄 Fix style confusion caused by the latest antd style. [#2636](https://github.com/ant-design-blazor/ant-design-blazor/pull/2636) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fix Cascader that cannot use `Allowclear` to clear the content when searching (#2607). [#2610](https://github.com/ant-design-blazor/ant-design-blazor/pull/2610) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
  - 🐞 Fix Cascader display text does not refresh. [#2575](https://github.com/ant-design-blazor/ant-design-blazor/pull/2575) [@noctis0430](https://github.com/noctis0430)

- Select
  - 🐞 Fix when ignoreitemchanges is false, deleting multiple selections will cause an exception (#2617). [#2620](https://github.com/ant-design-blazor/ant-design-blazor/pull/2620) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
  - 🛠 Refactor some events from Action to EventCallback. [#2601](https://github.com/ant-design-blazor/ant-design-blazor/pull/2601) [@ElderJames](https://github.com/ElderJames)

- Badge
  - 🐞 Fix show/hide montion optimization. [#2609](https://github.com/ant-design-blazor/ant-design-blazor/pull/2609) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fix offset didn't support negative numbers. [#2608](https://github.com/ant-design-blazor/ant-design-blazor/pull/2608) [@ElderJames](https://github.com/ElderJames)

- Statistic
  - 🐞 Fix CountDown that can't refresh in the background. [#2598](https://github.com/ant-design-blazor/ant-design-blazor/pull/2598) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
  - 🐞 Fix CountDown can't refresh when set value. [#2587](https://github.com/ant-design-blazor/ant-design-blazor/pull/2587) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)

- InputNumber
  - 🆕 Add PlaceHolder parameter. [#2528](https://github.com/ant-design-blazor/ant-design-blazor/pull/2528) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fix and create test to value display when step value notation is scientific. [#2547](https://github.com/ant-design-blazor/ant-design-blazor/pull/2547) [@petertorocsik](https://github.com/petertorocsik)

- 🛠 Refactor Message Service, add `IMessageService` for more abstract. [#2633](https://github.com/ant-design-blazor/ant-design-blazor/pull/2633) [@kooliokey](https://github.com/kooliokey)
- 🐞 Fix Pagination won't show certain current page block when window width is small after a specific sequence of operation. [#2616](https://github.com/ant-design-blazor/ant-design-blazor/pull/2616) [@fcxxzux](https://github.com/fcxxzux)
- 🐞 Fix Upload image recognition supports user-defined modification of image file extension, adding webp format, and repairing that the filename does not exist. A subscript out of range exception is generated. [#2626](https://github.com/ant-design-blazor/ant-design-blazor/pull/2626) [@AigioL](https://github.com/AigioL)
- 🐞 Fix Input that OnChange will invoke twice when paste data (#2591). [#2592](https://github.com/ant-design-blazor/ant-design-blazor/pull/2592) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
- 🐞 Fix Overlay trigger can't open browser native menu after it was right-click. [#2602](https://github.com/ant-design-blazor/ant-design-blazor/pull/2602) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fix TimeLine that the `Pending` could not be closed.(#2271). [#2588](https://github.com/ant-design-blazor/ant-design-blazor/pull/2588) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
- 🐞 Fix Tree that the coexistence of Disable and Checked attributes of TreeNode does not take effect. [#2583](https://github.com/ant-design-blazor/ant-design-blazor/pull/2583) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
- 🐞 Fix Layout that add two-way binding for Sider Collapsed parameter。 [#2536](https://github.com/ant-design-blazor/ant-design-blazor/pull/2536) [@ElderJames](https://github.com/ElderJames)

Note that since the last synchronization with V4.16.9 of antd, the original script could not be compiled due to the modification of the antd style file. Until this update span is large, there may be problems with the style. If found, please submit an issue.

### 0.11.0

`2022-06-16`

🌈Every cloud has a silver lining.

- Table
  - 🔥 support for Table virtualization [#2143](https://github.com/ant-design-blazor/ant-design-blazor/pull/2143) [@anranruye](https://github.com/anranruye)
  - 🔥 Support to control/restore table filter/sorter state using existing QueryModel [#2129](https://github.com/ant-design-blazor/ant-design-blazor/pull/2129) [@AnaNikolasevic](https://github.com/AnaNikolasevic)
  - 🆕 support setting table scrollbar width using `ScrollBarWidth` parameter. [#2451](https://github.com/ant-design-blazor/ant-design-blazor/pull/2451) [@ElderJames](https://github.com/ElderJames)
  - 🆕 support for using built-in logic when defining the PaginationTemplate. [#2220](https://github.com/ant-design-blazor/ant-design-blazor/pull/2220) [@anranruye](https://github.com/anranruye)
  - 🛠 make Responsive default to false   (with a breaking change). [#2419](https://github.com/ant-design-blazor/ant-design-blazor/pull/2419) [@ElderJames](https://github.com/ElderJames)
  - 🛠 Use Small size Pagination to fit compact Table [#2246](https://github.com/ant-design-blazor/ant-design-blazor/pull/2246) [@anranruye](https://github.com/anranruye)

- TreeSelect
  - 🐞 Fixed TreeSelect expressions and selection [#2507](https://github.com/ant-design-blazor/ant-design-blazor/pull/2507) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed TreeSelect binding default where it did not show selected items [#2134](https://github.com/ant-design-blazor/ant-design-blazor/pull/2134) [@gmij](https://github.com/gmij)

- 🆕 Add Upload support for incorporating build-in InputFile [#2443](https://github.com/ant-design-blazor/ant-design-blazor/pull/2443) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add Select search debounce. [#2275](https://github.com/ant-design-blazor/ant-design-blazor/pull/2275) [@tompru](https://github.com/tompru)
- 🆕 Component library added .net 6 target framework [#2484](https://github.com/ant-design-blazor/ant-design-blazor/pull/2484) [@ElderJames](https://github.com/ElderJames)
- ⌨️ Add Form Feedback Icon when Invalid [#2418](https://github.com/ant-design-blazor/ant-design-blazor/pull/2418) [@bweissronin](https://github.com/bweissronin)
- ⌨️ Add Checkbox supports trigger check when clicking label [#2296](https://github.com/ant-design-blazor/ant-design-blazor/pull/2296) [@bweissronin](https://github.com/bweissronin)
- ⌨️ Add Icon `Alt` Parameter to set the alt attribute that pairs with role="img" [#2302](https://github.com/ant-design-blazor/ant-design-blazor/pull/2302) [@bweissronin](https://github.com/bweissronin)
- ⌨️ Add Button `AriaLabel` Parameter [#2278](https://github.com/ant-design-blazor/ant-design-blazor/pull/2278) [@bweissronin](https://github.com/bweissronin)
- 🐞 Fixed Tree incorrect checking during initialization. [#2506](https://github.com/ant-design-blazor/ant-design-blazor/pull/2506) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed DatePicker that week selection issue when unable to click date selection. [#2463](https://github.com/ant-design-blazor/ant-design-blazor/pull/2463) [@WhyILoveSpringRoll](https://github.com/WhyILoveSpringRoll)
- 📖 docs(faq): add CSS isolation explanation. [#2158](https://github.com/ant-design-blazor/ant-design-blazor/pull/2158) [@dennisrahmen](https://github.com/dennisrahmen)

### 0.10.7

`2022-05-22`

- 🐞 Fixed select replacing a datasource with some of the same items was not in the right order. [#2462](https://github.com/ant-design-blazor/ant-design-blazor/pull/2462) [@ElderJames](https://github.com/ElderJames)

- Table
  - 🐞 Fixed exception caused by js interop with addEventListener. [#2460](https://github.com/ant-design-blazor/ant-design-blazor/pull/2460) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed null exception cause by event listener. [#2448](https://github.com/ant-design-blazor/ant-design-blazor/pull/2448) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed that too length content in responsive mode will brace up the table.  [#2470](https://github.com/ant-design-blazor/ant-design-blazor/pull/2470) [@ElderJames](https://github.com/ElderJames)

- 🐞 Fixed cascader to avoid adding items in search list in a loop. [#2457](https://github.com/ant-design-blazor/ant-design-blazor/pull/2457) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed menu IconTemplate does not work in MenuItem of SubMenu. [#2449](https://github.com/ant-design-blazor/ant-design-blazor/pull/2449) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Tree that when the node contains `|` characters, and `SearchVaule` search `|` showed abnormal problem. [#2437](https://github.com/ant-design-blazor/ant-design-blazor/pull/2437) [@ElderJames](https://github.com/ElderJames)

### 0.10.6

`2022-05-10`

- 🐞 Fixed Tooltip Tabindex. [#2404](https://github.com/ant-design-blazor/ant-design-blazor/pull/2404) [@bweissronin](https://github.com/bweissronin)
- 🐞 Fixed From FieldIdentifier equality check in Rules Mode OnFieldChanged. [#2400](https://github.com/ant-design-blazor/ant-design-blazor/pull/2400) [@GHMonad](https://github.com/GHMonad)
- 🐞 Fixed localization of decimal point when converting InputNumber string to numeric type. [#2398](https://github.com/ant-design-blazor/ant-design-blazor/pull/2398) [@jp-rl](https://github.com/jp-rl)
- Select
  - 🐞 Fixed a bug when using LabelTemplate in Select. [#2399](https://github.com/ant-design-blazor/ant-design-blazor/pull/2399) [@charset](https://github.com/charset)
  - 🐞 Fixed can't clear selected option when set value null. [#2371](https://github.com/ant-design-blazor/ant-design-blazor/pull/2371) [@ElderJames](https://github.com/ElderJames)

- ⚡️ Optimize the speed of expanding lots of nodes [#2385](https://github.com/ant-design-blazor/ant-design-blazor/pull/2385) [@densen2014](https://github.com/densen2014)
- 🐞 Fixed Cascader dropdown can't open correctly on click. [#2363](https://github.com/ant-design-blazor/ant-design-blazor/pull/2363) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Upload wrong drag area. [#2360](https://github.com/ant-design-blazor/ant-design-blazor/pull/2360) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Input throw exception when binding a List or Dictionary item. [#2359](https://github.com/ant-design-blazor/ant-design-blazor/pull/2359) [@ElderJames](https://github.com/ElderJames)

### 0.10.5

2022-03-15

- 🐞 Fixed Radio default name value for radio in group. [#2330](https://github.com/ant-design-blazor/ant-design-blazor/pull/2330) [@bweissronin](https://github.com/bweissronin)
- 🛠 Fixed Upload that add more image file type [#2321](https://github.com/ant-design-blazor/ant-design-blazor/pull/2321) [@scugzbc](https://github.com/scugzbc)
- 🐞 Fixed Tabs that TabTemplate can't display in the overflow dropdown. [#2320](https://github.com/ant-design-blazor/ant-design-blazor/pull/2320) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Tree that dead-loop rendering and checkbox check incorrectly [#2319](https://github.com/ant-design-blazor/ant-design-blazor/pull/2319) [@gmij](https://github.com/gmij)
- 🐞 Fixed InputNumber that possible nonstoppable increase/decrease. [#2317](https://github.com/ant-design-blazor/ant-design-blazor/pull/2317) [@jeffraska](https://github.com/jeffraska)
- 🐞 Fix Select that item scroll into view when using DataSource. [#2316](https://github.com/ant-design-blazor/ant-design-blazor/pull/2316) [@jeffraska](https://github.com/jeffraska)
- 🐞 Fixed Badge the gap of numbers. [#2315](https://github.com/ant-design-blazor/ant-design-blazor/pull/2315) [@ElderJames](https://github.com/ElderJames)

### 0.10.4

2022-02-25

- Table

  - 🆕 Add allow access field value from CellRender context. [#2257](https://github.com/ant-design-blazor/ant-design-blazor/pull/2257) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed Use same Locale for Pagination and PaginationOptions. [#2244](https://github.com/ant-design-blazor/ant-design-blazor/pull/2244) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed when pageIndex and pageSize change together, trigger PageSizeChanged event before PageIndexChanged event, and trigger OnChange event only one time. [#2239](https://github.com/ant-design-blazor/ant-design-blazor/pull/2239) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed support for using DataTable as the data source. [#2234](https://github.com/ant-design-blazor/ant-design-blazor/pull/2234) [@ElderJames](https://github.com/ElderJames)
  - 📖 Improve the API part about Table in the component document. [#2219](https://github.com/ant-design-blazor/ant-design-blazor/pull/2219) [@SmRiley](https://github.com/SmRiley)

- Upload

  - 📖 Add a reference implementation of the Upload interfacing API. [#2274](https://github.com/ant-design-blazor/ant-design-blazor/pull/2274) [@SmRiley](https://github.com/SmRiley)
  - 🐞 Fixed center layout of upload. [#2267](https://github.com/ant-design-blazor/ant-design-blazor/pull/2267) [@oemil](https://github.com/oemil)

- Modal

  - 🆕 Add max content body set supported. [#2264](https://github.com/ant-design-blazor/ant-design-blazor/pull/2264) [@zxyao145](https://github.com/zxyao145)
  - 🐞 Fixed wrong width when without scroll bar. [#2212](https://github.com/ant-design-blazor/ant-design-blazor/pull/2212) [@zxyao145](https://github.com/zxyao145)
  - 🐞 Fixed RTL issue. [#2295](https://github.com/ant-design-blazor/ant-design-blazor/pull/2295) [@zxyao145](https://github.com/zxyao145)

- Datepicker

  - 🐞 Fixed up suffix icon show issue. [#2226](https://github.com/ant-design-blazor/ant-design-blazor/pull/2226) [@KarimFereidooni](https://github.com/KarimFereidooni)
  - 🌐 Fixed incorrect order of czech week days in datepicker. [#2247](https://github.com/ ant-design-blazor/ant-design-blazor/pull/2247) [@jeffraska](https://github.com/jeffraska)

- 🐞 Image: Fixed cannot be centered vertically. [#2287](https://github.com/ant-design-blazor/ant-design-blazor/pull/2287) [@zxyao145](https://github.com/zxyao145)
- 💄 Result: Add missed style to Results. [#2256](https://github.com/ant-design-blazor/ant-design-blazor/pull/2256) [@CAPCHIK](https://github.com/CAPCHIK)
- 🐞 Radio: Improve sync logic about `Disabled` between `RadioGroup` and `Radio`s. [#2197](https://github.com/ant-design-blazor/ant-design-blazor/pull/2197) [@LeaFrock](https://github.com/LeaFrock)
- 📖 Input: Improve the API part about Input and Select in the component document. [#2251](https://github.com/ant-design-blazor/ant-design-blazor/pull/2251) [@SmRiley](https://github.com/SmRiley)

### 0.10.3

2021-12-19

- Typography

  - 🐞 Fixed an issue with copying HTML content. [#2118](https://github.com/ant-design-blazor/ant-design-blazor/pull/2118) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed `OnCopy` not invoked when `Text` is null or empty. [#2098](https://github.com/ant-design-blazor/ant-design-blazor/pull/2098) [@LeaFrock](https://github.com/LeaFrock)

- Cascader

  - 🆕 Add display indicator when options is null or empty. [#2108](https://github.com/ant-design-blazor/ant-design-blazor/pull/2108) [@noctis0430](https://github.com/noctis0430)
  - 🐞 Fixed crashes when options is null. [#2105](https://github.com/ant-design-blazor/ant-design-blazor/pull/2105) [@noctis0430](https://github.com/noctis0430)

- Tree

  - 🐞 Fixed an issue where the check status was not modified when CheckedKeys were modified. [#2133](https://github.com/ant-design-blazor/ant-design-blazor/pull/2133) [@Guyiming](https://github.com/Guyiming)
  - 🐞 Fixed an issue where MatchedClass did not work when Draggable was set. [#2171](https://github.com/ant-design-blazor/ant-design-blazor/pull/2171) [@jp-rl](https://github.com/jp-rl)
  - 🐞 Fixed an issue where all nodes are collapsed when the SearchValue is cleared. [#2177](https://github.com/ant-design-blazor/ant-design-blazor/pull/2177) [@ElderJames](https://github.com/ElderJames)

- 🆕 Add Form support for native `EditForm`. [#2138](https://github.com/ant-design-blazor/ant-design-blazor/pull/2138) [@knight1219](https://github.com/knight1219)
- 🐞 fix LocaleProvider type initializer throws CultureNotFoundException. [#2094](https://github.com/ant-design-blazor/ant-design-blazor/pull/2094) [@anranruye](https://github.com/anranruye)
- 🐞 Fixed Modal an error width for disable body scroll. [#2163](https://github.com/ant-design-blazor/ant-design-blazor/pull/2163) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed Transfer the style of the button. [#2156](https://github.com/ant-design-blazor/ant-design-blazor/pull/2156) [@dennisrahmen](https://github.com/dennisrahmen)
- 🐞 Fixed Select incorrect to detect type this way, it throws when class inheritance is used. [#2121](https://github.com/ant-design-blazor/ant-design-blazor/pull/2121) [@ocoka](https://github.com/ocoka)
- 🐞 Fixed Checkbox two-way binding issue with Checkbox Groups. [#2173](https://github.com/ant-design-blazor/ant-design-blazor/pull/2173) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Radio that the `Disabled` property should always be consistent with `RadioGroup.Disabled` when Radio is a member of a group. [#2142](https://github.com/ant-design-blazor/ant-design-blazor/pull/2142) [@LeaFrock](https://github.com/LeaFrock)

### 0.10.2

2021-11-5

- Descriptions

  - 💄 fixed descriptions: header styles. [#2078](https://github.com/ant-design-blazor/ant-design-blazor/pull/2078) [@ElderJames](https://github.com/ElderJames)
  - 💄 The list item describing the horizontal mode of the component is supplemented with the 'ant descriptions item container' style. [#2024](https://github.com/ant-design-blazor/ant-design-blazor/pull/2024) [@weidyg](https://github.com/weidyg)

- Tabs

  - 🆕 Add context menu and page config for reusetabs. [#2075](https://github.com/ant-design-blazor/ant-design-blazor/pull/2075) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed tabs broken in a card. [#2053](https://github.com/ant-design-blazor/ant-design-blazor/pull/2053) [@anddrzejb](https://github.com/anddrzejb)

- Table

  - 🆕 Support reload data with specific page index and size. [#2072](https://github.com/ant-design-blazor/ant-design-blazor/pull/2072) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add Align property for column. [#2045](https://github.com/ant-design-blazor/ant-design-blazor/pull/2045) [@Qyperion](https://github.com/Qyperion)
  - 🐞 Fixed `ReloadData()` can't invoke `OnChange`. [#2071](https://github.com/ant-design-blazor/ant-design-blazor/pull/2071) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed handle null value of propertyInfo. [#2025](https://github.com/ant-design-blazor/ant-design-blazor/pull/2025) [@Guyiming](https://github.com/Guyiming)

- Select

  - 🐞 Fixed Select with group refresh on datasource change. [#2048](https://github.com/ant-design-blazor/ant-design-blazor/pull/2048) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Add `ValueOnClear` to stored `Value` that will be used when clear button is pressed. [#2023](https://github.com/ant-design-blazor/ant-design-blazor/pull/2023) [@anddrzejb](https://github.com/anddrzejb)

- 💄 Fixed Area style issue for auto-size. [#2001](https://github.com/ant-design-blazor/ant-design-blazor/pull/2001) [@anranruye](https://github.com/anranruye)
- 🐞 Fixed Calendar `DateFullCellRender` throwing an exception. [#2068](https://github.com/ant-design-blazor/ant-design-blazor/pull/2068) [@szymski](https://github.com/szymski)
- 🐞 Fixed Upload IsPicture issue. [#2049](https://github.com/ant-design-blazor/ant-design-blazor/pull/2049) [@berkerdong](https://github.com/berkerdong)
- 🐞 Fixed Overlay premature reset of \_mouseInTrigger. [#2036](https://github.com/ant-design-blazor/ant-design-blazor/pull/2036) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Fixed Menu make router match defaualt to all. [d8352b8](https://github.com/ant-design-blazor/ant-design-blazor/commit/d8352b8) [@James Yeung](https://github.com/James Yeung)
- 🌐 i18n: czech localization update. [#2030](https://github.com/ant-design-blazor/ant-design-blazor/pull/2030) [@Martin-Pucalka](https://github.com/Martin-Pucalka)

### 0.10.1

2021-10-13

- Tabs

  - 🆕 Add `AuthorizeReuseTabsRouteView` component for enable authorize of `ReuseTabs` . [#1910](https://github.com/ant-design-blazor/ant-design-blazor/pull/1910) [@Guyiming](https://github.com/Guyiming)
  - 🛠 refactor & improve rendering. [#1970](https://github.com/ant-design-blazor/ant-design-blazor/pull/1970) [@ElderJames](https://github.com/ElderJames)

- Select

  - 🐞 Fixed arrow down click does not auto close. [#1977](https://github.com/ant-design-blazor/ant-design-blazor/pull/1977) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed null value option. [#1996](https://github.com/ant-design-blazor/ant-design-blazor/pull/1996) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed keyboard on mobile devices would brings up at non-searchable mode. [#1992](https://github.com/ant-design-blazor/ant-design-blazor/pull/1992) [@anranruye](https://github.com/anranruye)

- Table

  - 🐞 Fixed when change page index, backgroud of the selection box is updated before the table row; Fix when change page index, the selected rows still exist. [#1973](https://github.com/ant-design-blazor/ant-design-blazor/pull/1973) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed tree data sort & filter. [#1966](https://github.com/ant-design-blazor/ant-design-blazor/pull/1966) [@ElderJames](https://github.com/ElderJames)
  - 🐞 fFixed incorrectly render on first loading. [#1957](https://github.com/ant-design-blazor/ant-design-blazor/pull/1957) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed label display incorrectly in responsive mode. [#1952](https://github.com/ant-design-blazor/ant-design-blazor/pull/1952) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed nullable built-in DateTime filter [#1964](https://github.com/ant-design-blazor/ant-design-blazor/pull/1964) [@anranruye](https://github.com/anranruye)
  - 🐞Fixed the implement of RemoveMilliseconds. [#1895](https://github.com/ant-design-blazor/ant-design-blazor/pull/1895) [@iamSmallY](https://github.com/iamSmallY)

- Menu

  - 🐞 Fixed IconTemplate when InlineCollapse is used. [#2006](https://github.com/ant-design-blazor/ant-design-blazor/pull/2006) [@knight1219](https://github.com/knight1219)
  - 🐞 Fixed Overlay bug fix & menu renering optimization. [#1949](https://github.com/ant-design-blazor/ant-design-blazor/pull/1949) [@anddrzejb](https://github.com/anddrzejb)

- 🐞 Fixed Slider Positioning Using Min/Max. [#1940](https://github.com/ant-design-blazor/ant-design-blazor/pull/1940) [@rabberbock](https://github.com/rabberbock)
- 🐞 Fixed grid issue with gutter match due to breakpoint enum name case. [#1963](https://github.com/ant-design-blazor/ant-design-blazor/pull/1963) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed positioning overlay items when locale has ',' as decimal separator. [#1956](https://github.com/ant-design-blazor/ant-design-blazor/pull/1956) [@bezysoftware](https://github.com/bezysoftware)
- 🐞 Fixed cannot drag when Modal excessive height. [#1951](https://github.com/ant-design-blazor/ant-design-blazor/pull/1951) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed LocaleProvider: custom language resource; improve fallback strategy. [#1988](https://github.com/ant-design-blazor/ant-design-blazor/pull/1988) [@anranruye](https://github.com/anranruye)
- 🐞 Fixed List: dynamic response to changes in Grid parameter [#2014](https://github.com/ant-design-blazor/ant-design-blazor/pull/2014) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Fixed Progress: use invariant culture for style strings [#2017](https://github.com/ant-design-blazor/ant-design-blazor/pull/2017) [@CAPCHIK](https://github.com/CAPCHIK)
- 🌐 i18n: czech localization update [#2019](https://github.com/ant-design-blazor/ant-design-blazor/pull/2019) [@Martin Pučálka](https://github.com/Martin-Pucalka)

### 0.10.0

2021-09-15

- 🔥 Add TreeSelect component. [#1773](https://github.com/ant-design-blazor/ant-design-blazor/pull/1773) [@gmij](https://github.com/gmij)

- Tree

  - 🆕 Add Tree ChildContent template. [#1887](https://github.com/ant-design-blazor/ant-design-blazor/pull/1887) [@ElderJames](https://github.com/ElderJames)
  - 🛠 Refactor Tree API name : `CheckedAll`-> `CheckAll`, `DecheckedAll`-> `UncheckAll`. [#1792](https://github.com/ant-design-blazor/ant-design-blazor/pull/1792) [@lukblazewicz](https://github.com/lukblazewicz)

- Radio

  - 🆕 Add Radio support enum type for `RadioGroup` options, use `EnumRadioGroup`. [#1840](https://github.com/ant-design-blazor/ant-design-blazor/pull/1840) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add Radio options property for `RadioGroup`. [#1839](https://github.com/ant-design-blazor/ant-design-blazor/pull/1839) [@ElderJames](https://github.com/ElderJames)

- 🆕 Add Image preview mode. [#1842](https://github.com/ant-design-blazor/ant-design-blazor/pull/1842) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add Form FormItem parameter `Help`, `ValidateStatus` and `HasFeedback`. [#1807](https://github.com/ant-design-blazor/ant-design-blazor/pull/1807) [@JamesGit-hash](https://github.com/JamesGit-hash)
- 🆕 Add Table responsive support. It will become a card list under the mobile screen. [#1802](https://github.com/ant-design-blazor/ant-design-blazor/pull/1802) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add Timeline Label parameter. [#1941](https://github.com/ant-design-blazor/ant-design-blazor/pull/1941) [@ElderJames](https://github.com/ElderJames)
- 🆕 Add `Component` component for generating dynamically typed components. [#1703](https://github.com/ant-design-blazor/ant-design-blazor/pull/1703) [@anranruye](https://github.com/anranruye)

### 0.9.4

2021-09-12

- Table

  - 🐞 Fixed an issue that initialization is refreshed twice when PageSize is not equal to 10. [#1933](https://github.com/ant-design-blazor/ant-design-blazor/pull/1933) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Addd CellData for CellRender. [#1907](https://github.com/ant-design-blazor/ant-design-blazor/pull/1907) [@ElderJames](https://github.com/ElderJames)
  - ⚡️ Put fixed column style into js. [#1897](https://github.com/ant-design-blazor/ant-design-blazor/pull/1897) [@ElderJames](https://github.com/ElderJames)
  - 📖 add dynamic table demo. [#1908](https://github.com/ant-design-blazor/ant-design-blazor/pull/1908) [@ElderJames](https://github.com/ElderJames)

- InputNumber

  - 🆕 Add OnFocus event [#1931](https://github.com/ant-design-blazor/ant-design-blazor/pull/1931) [@Hona](https://github.com/Hona)
  - 🐞 Fixed inputmode to support mobile numeric keypad. [#1923](https://github.com/ant-design-blazor/ant-design-blazor/pull/1923) [@CAPCHIK](https://github.com/CAPCHIK)

- Select

  - 🐞 Fixed the data source which has members of different types. [#1932](https://github.com/ant-design-blazor/ant-design-blazor/pull/1932) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed the problem that the selected item will be reset when setting DataSource [#1906](https://github.com/ant-design-blazor/ant-design-blazor/pull/1906) [@anddrzejb](https://github.com/anddrzejb)

- 🐞 Fixed Menu that the Title of MenuItem with RouterLink is not hidden when it is collapsed. [#1934](https://github.com/ant-design-blazor/ant-design-blazor/pull/1934) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Overlay with a series of issues related to dropdown & popup。 [#1848](https://github.com/ant-design-blazor/ant-design-blazor/pull/1848) [@anddrzejb](https://github.com/anddrzejb)
- 💄 Fixed loading icon styles. [#1902](https://github.com/ant-design-blazor/ant-design-blazor/pull/1902) [@CAPCHIK](https://github.com/CAPCHIK)
- 🐞 Added parameter `Rows`. [#1920](https://github.com/ant-design-blazor/ant-design-blazor/pull/1920) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Add stop propogation. [#1917](https://github.com/ant-design-blazor/ant-design-blazor/pull/1917) [@Hona](https://github.com/Hona)
- 🐞 Fixed Form modifies the bound model to throw an exception in Rule validation mode. [#1901](https://github.com/ant-design-blazor/ant-design-blazor/pull/1901) [@lxyruanjian](https://github.com/lxyruanjian)
- 🐞 Fixed List resposive style doesn't work. [#1937](https://github.com/ant-design-blazor/ant-design-blazor/pull/1937) [@ElderJames](https://github.com/ElderJames)
- ⚡️ Fixed EventListener avoid memory leak issue. [#1857](https://github.com/ant-design-blazor/ant-design-blazor/pull/1857) [@tonyyip1969](https://github.com/tonyyip1969)

### 0.9.3

2021-08-29

- Table

  - 🆕 Add `TheSameDateWith` condition for the built-in filter of DateTime Column, compare only date. [#1856](https://github.com/ant-design-blazor/ant-design-blazor/pull/1856) [@iamSmallY](https://github.com/iamSmallY)
    [#1889](https://github.com/ant-design-blazor/ant-design-blazor/pull/1889) [@anranruye](https://github.com/anranruye)
  - 📖 Add an example of nested table. [#1884](https://github.com/ant-design-blazor/ant-design-blazor/pull/1884) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed Time column built-in filter will ignore milliseconds when filtering.[#1864](https://github.com/ant-design-blazor/ant-design-blazor/pull/1864) [@iamSmallY](https://github.com/iamSmallY)
  - 🐞 Fixed the issue that operations such as page turning, sorting and filtering are not refreshed by using client mode. [#1858](https://github.com/ant-design-blazor/ant-design-blazor/pull/1858) [@ElderJames](https://github.com/ElderJames)
    [#1875](https://github.com/ant-design-blazor/ant-design-blazor/pull/1875) [@nikolaykrondev](https://github.com/nikolaykrondev)
  - 🐞 Fixed the issue that OnChange is called multiple times after initialization. [#1855](https://github.com/ant-design-blazor/ant-design-blazor/pull/1855) [@ElderJames](https://github.com/ElderJames)

- 🆕 Breadcrumb add Href and Overlay dropdown. [#1859](https://github.com/ant-design-blazor/ant-design-blazor/pull/1859) [@CAPCHIK](https://github.com/CAPCHIK)
- 🆕 MenuItem add IconTemplate. [#1879](https://github.com/ant-design-blazor/ant-design-blazor/pull/1879) [@Guyiming](https://github.com/Guyiming)
- 🆕 Upload add Support for custom HttpMethod. [#1853](https://github.com/ant-design-blazor/ant-design-blazor/pull/1853) [@SapientGuardian](https://github.com/SapientGuardian)
- 🐞 Fixed Tag two-way binding of Checked parameter. [#1876](https://github.com/ant-design-blazor/ant-design-blazor/pull/1876) [@stefanodriussi](https://github.com/stefanodriussi)
- 🐞 Fixed AutoComplete Dropdown menu positioning issue. [#1860](https://github.com/ant-design-blazor/ant-design-blazor/pull/1860) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Fixed InputNumber DefaultValue binding issue. [#1871](https://github.com/ant-design-blazor/ant-design-blazor/pull/1871) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Checkbox an issue that caused an exception when CheckboxGroup option was modified. [#1863](https://github.com/ant-design-blazor/ant-design-blazor/pull/1863) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Fixed Modal and Confirm cannot focus the button automatically. [#1838](https://github.com/ant-design-blazor/ant-design-blazor/pull/1838) [@zxyao145](https://github.com/zxyao145)

### 0.9.2

2021-08-18

- Table

  - 🐞 Fixed prevent propagation of expand button click events. [#1850](https://github.com/ant-design-blazor/ant-design-blazor/pull/1850) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed initial load and render. [#1835](https://github.com/ant-design-blazor/ant-design-blazor/pull/1835) [@ElderJames](https://github.com/ElderJames)

- 🐞 Fixed Tree: `SelectedNodeChanged` would be fired twice twice. [#1849](https://github.com/ant-design-blazor/ant-design-blazor/pull/1849) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Tag: Style parameters are not rendered. [#1847](https://github.com/ant-design-blazor/ant-design-blazor/pull/1847) [@JohnHao421](https://github.com/JohnHao421)
- 🐞 Fixed Menu: `OnMenuItemClicked` should be triggered when menu `Selectable` is false. [#1843](https://github.com/ant-design-blazor/ant-design-blazor/pull/1843) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Checkbox: The checked state of `CheckboxGroup` didn't follow the value change. [#1841](https://github.com/ant-design-blazor/ant-design-blazor/pull/1841) [@ElderJames](https://github.com/ElderJames)

### 0.9.1

2021-08-11

- Table

  - 🐞 Allow to set time in the filter. [#1827](https://github.com/ant-design-blazor/ant-design-blazor/pull/1827) [@anranruye](https://github.com/anranruye)
  - 🐞 Use "or" filter condition for List type built-in filter. [#1804](https://github.com/ant-design-blazor/ant-design-blazor/pull/1804) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed missing sorter model on first change. [#1823](https://github.com/ant-design-blazor/ant-design-blazor/pull/1823) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed the issue that tables inside a table column throw an exception. [#1732](https://github.com/ant-design-blazor/ant-design-blazor/pull/1732) [@anranruye](https://github.com/anranruye)

- DatePicker

  - 🐞 Remain millisecond value when change the picker value. [#1829](https://github.com/ant-design-blazor/ant-design-blazor/pull/1829) [@anranruye](https://github.com/anranruye)
  - 🐞 DatePicker: fix ShowToday behaviour when ShowTime set to true. [#1819](https://github.com/ant-design-blazor/ant-design-blazor/pull/1819) [@lukblazewicz](https://github.com/lukblazewicz)
  - 🐞 Fixed: ShowTime issues addressed. [#1788](https://github.com/ant-design-blazor/ant-design-blazor/pull/1788) [@anddrzejb](https://github.com/anddrzejb)

- Overlay

  - 🐞 Use right positioning for bottom-right and top-right placement. [#1799](https://github.com/ant-design-blazor/ant-design-blazor/pull/1799) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed overlay location when container element has border. [#1797](https://github.com/ant-design-blazor/ant-design-blazor/pull/1797) [@anranruye](https://github.com/anranruye)

- Select

  - 🐞 Fixed MaxTagCount behaves properly for non-responsive scenarios. [#1776](https://github.com/ant-design-blazor/ant-design-blazor/pull/1776) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Accept `null` as select option value. [#1786](https://github.com/ant-design-blazor/ant-design-blazor/pull/1786) [@anranruye](https://github.com/anranruye)
  - 🆕 Make EnumSelect support null option value [#1777](https://github.com/ant-design-blazor/ant-design-blazor/pull/1777) [@anranruye](https://github.com/anranruye)
  - 🐞 Remove ValueName null check for non-datasource approach. [#1785](https://github.com/ant-design-blazor/ant-design-blazor/pull/1785) [@anranruye](https://github.com/anranruye)

- Tree

  - 🐞 Bring `SearchExpression` back. [#1796](https://github.com/ant-design-blazor/ant-design-blazor/pull/1796) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fix parent checkbox state calculation in a Tree. [#1781](https://github.com/ant-design-blazor/ant-design-blazor/pull/1781) [@lukblazewicz](https://github.com/lukblazewicz)

- 🐞 Fixed AutoComplete: turn off the input autocomplete attribute. [#1825](https://github.com/ant-design-blazor/ant-design-blazor/pull/1825) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Menu: OnBreakpoint and OnCollapse only when they was changed. [#1815](https://github.com/ant-design-blazor/ant-design-blazor/pull/1815) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Calendar: the width of select component in the header. [#1801](https://github.com/ant-design-blazor/ant-design-blazor/pull/1801) [@anranruye](https://github.com/anranruye)
- 🐞 Fixed InputNumber: OnChange cannot be triggered during keyboard input. [#1830](https://github.com/ant-design-blazor/ant-design-blazor/pull/1830) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Modal: error when confirm TaskCompletionSource SetResult repeat. [#1795](https://github.com/ant-design-blazor/ant-design-blazor/pull/1795) [@zxyao145](https://github.com/zxyao145)

### 0.9.0

2021-07-27

🎉 As of this release, the project has welcomed a total of 101 contributors who have made this project possible! We'd like to thank them for their generous contributions!

- Tabs

  - 🔥 Add reuse tabs routeview. ([demo](https://github.com/ant-design-blazor/demo-reuse-tabs)) [#1704](https://github.com/ant-design-blazor/ant-design-blazor/pull/1704) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add `OnClose` event, TabTemplate. [#1698](https://github.com/ant-design-blazor/ant-design-blazor/pull/1698) [@ElderJames](https://github.com/ElderJames)

- Table

  - 🆕 Add built-in filter for `Guid` type. [#1756](https://github.com/ant-design-blazor/ant-design-blazor/pull/1756) [@anranruye](https://github.com/anranruye)
  - ⚡️ Optimize render fragments. [#1597](https://github.com/ant-design-blazor/ant-design-blazor/pull/1597) [@anranruye](https://github.com/anranruye)
  - 🛠 Refactor filter model classes, allow access filters through ITableFilterModel, allow access to filter compare operator and condition. [#1563](https://github.com/ant-design-blazor/ant-design-blazor/pull/1563) [@anranruye](https://github.com/anranruye)
  - 🆕 Add built-in filter for enum types, support null value for List filter type. [#1439](https://github.com/ant-design-blazor/ant-design-blazor/pull/1439) [@anranruye](https://github.com/anranruye)
  - 🆕 Add Columns Show/Hide functionality. [#1410](https://github.com/ant-design-blazor/ant-design-blazor/pull/1410) [@ldsenow](https://github.com/ldsenow)
  - 🆕 Add Allow custom pagination template. [#1409](https://github.com/ant-design-blazor/ant-design-blazor/pull/1409) [@ldsenow](https://github.com/ldsenow)
  - 🛠 Refactor PropertyAccessHelper to PathHelper, replace double quotes with single quotes to identify string index keys. [#1386](https://github.com/ant-design-blazor/ant-design-blazor/pull/1386) [@Zonciu](https://github.com/Zonciu)
  - 🐞 Add implement TotalChanged callback; add demo for loading data from remote data source. [#1558](https://github.com/ant-design-blazor/ant-design-blazor/pull/1558) [@anranruye](https://github.com/anranruye)
  - 📖 Fixed edit row demo can't recovery the editing on cancel. [#1745](https://github.com/ant-design-blazor/ant-design-blazor/pull/1745) [@ElderJames](https://github.com/ElderJames)

- Select

  - 🆕 Add `EnumSelect` for select componet with enum. [#1759](https://github.com/ant-design-blazor/ant-design-blazor/pull/1759) [@wangj90](https://github.com/wangj90) - 🆕 Add `Simple` data source: When the item in the data source and the value property of select use the same type, it is not necessary to specify `ValueName`; When `LabelName` is not specified, the return value of the `ToString()` method of the item in the data source is used as the label. [#1541](https://github.com/ant-design-blazor/ant-design-blazor/pull/1541) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed duplicated tags. [#1766](https://github.com/ant-design-blazor/ant-design-blazor/pull/1766) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed the issue that can not set select component initial value for non-datasource approach. [#1743](https://github.com/ant-design-blazor/ant-design-blazor/pull/1743) [@anranruye](https://github.com/anranruye)

- Form

  - 🆕 Add Support for setting validation rules on FormItem. [#1516](https://github.com/ant-design-blazor/ant-design-blazor/pull/1516) [@mutouzdl](https://github.com/mutouzdl)
  - 🆕 Add Support for `EditContext` dynamic change. Added `OnFieldChanged`, `OnValidationRequested` & `OnValidationStateChanged` events. [#1504](https://github.com/ant-design-blazor/ant-design-blazor/pull/1504) [@anddrzejb](https://github.com/anddrzejb)
  - 🆕 Added `LabelStyle` to `FormItem` for custom element styling. [#1503](https://github.com/ant-design-blazor/ant-design-blazor/pull/1503) [@anddrzejb](https://github.com/anddrzejb)
  - 🆕 Expose `EditContext` in `Form` component, allow access to validation messages. [#1464](https://github.com/ant-design-blazor/ant-design-blazor/pull/1464) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed `FormItem` would be default to property name. [#1738](https://github.com/ant-design-blazor/ant-design-blazor/pull/1738) [@ElderJames](https://github.com/ElderJames)

- Modal

  - 🆕 Add NotificationRef support. [#1498](https://github.com/ant-design-blazor/ant-design-blazor/pull/1498) [@zxyao145](https://github.com/zxyao145)
  - 🐞 Fixed wrong seter in ModalOptions.ConfirmLoading (always is true). [#1742](https://github.com/ant-design-blazor/ant-design-blazor/pull/1742) [@zxyao145](https://github.com/zxyao145)
  - 🐞 Fixed click dialog cause the mask click event triggered. [#1727](https://github.com/ant-design-blazor/ant-design-blazor/pull/1727) [@zxyao145](https://github.com/zxyao145)

- Tree

  - 🛠 Refactor and close to official features, fixes initial value issues, and supports drag and drop. [#1517](https://github.com/ant-design-blazor/ant-design-blazor/pull/1517) [@lovachen](https://github.com/lovachen)
  - 🆕 Added support for checking all items in a Tree from code. [#1722](https://github.com/ant-design-blazor/ant-design-blazor/pull/1722) [@lukblazewicz](https://github.com/lukblazewicz)

- 🆕 Upload: Add support for drag upload. [#1765](https://github.com/ant-design-blazor/ant-design-blazor/pull/1765) [@ElderJames](https://github.com/ElderJames)
- 🆕 Button: Allows you to set a color for the button according to the official palette. [#1774](https://github.com/ant-design-blazor/ant-design-blazor/pull/1774) [@boukenka](https://github.com/boukenka)
- 🆕 Dropdown: Add `ButtonsStyle` &amp; `ButtonsClass` parameters that allow to style each button separately. `Type` accepts single value that will be applied to both buttons. [#1659](https://github.com/ant-design-blazor/ant-design-blazor/pull/1659) [@anddrzejb](https://github.com/anddrzejb)
- 🆕 DatePicker: Support disable one of `RangePicker` inputs. [#1648](https://github.com/ant-design-blazor/ant-design-blazor/pull/1648) [@mutouzdl](https://github.com/mutouzdl)
- 🆕 Tag: Color parameter now supports custom hex values and an Enum type. [#1514](https://github.com/ant-design-blazor/ant-design-blazor/pull/1514) [@MutatePat](https://github.com/MutatePat)
- 🐞 Drawer: Fixed the bug that the page scroll bar is restored when one of them is closed when there are multiple Drawers at the same time. [#1771](https://github.com/ant-design-blazor/ant-design-blazor/pull/1771) [@zxyao145](https://github.com/zxyao145)
- 🌐 i18n: Add missing French short week days. [#1521](https://github.com/ant-design-blazor/ant-design-blazor/pull/1521) [@dust63](https://github.com/dust63)

### 0.8.3

`2021-07-13`

- Table

  - 🆕 Add a parameter to expand all rows on load. [#1695](https://github.com/ant-design-blazor/ant-design-blazor/pull/1695) [@henrikwidlund](https://github.com/henrikwidlund)
  - 🐞 fix the bug that changing filter and/or operator closes the filter dropdown. [#1687](https://github.com/ant-design-blazor/ant-design-blazor/pull/1687) [@anranruye](https://github.com/anranruye)
  - 🐞 allow set filters after table initialization. [#1667](https://github.com/ant-design-blazor/ant-design-blazor/pull/1667) [@anranruye](https://github.com/anranruye)

- Upload

  - 🐞 Fix GetResponse() deserialization to ignore case [#1717](https://github.com/ant-design-blazor/ant-design-blazor/pull/1717) [@BeiYinZhiNian](https://github.com/BeiYinZhiNian)
  - 🐞 Treat all 2xx status codes in responses in the upload module as successful. [#1705](https://github.com/ant-design-blazor/ant-design-blazor/pull/1705) [@henrikwidlund](https://github.com/henrikwidlund)

- DatePicker

  - 🐞 Fixed width for custom and culture-based format. [#1685](https://github.com/ant-design-blazor/ant-design-blazor/pull/1685) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed datetime parse error. [#1663](https://github.com/ant-design-blazor/ant-design-blazor/pull/1663) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed blur/focus & regression tests. [#1681](https://github.com/ant-design-blazor/ant-design-blazor/pull/1681) [@anddrzejb](https://github.com/anddrzejb)

- Form

  - 🆕 feat(module: form): use DisplayName attribute as the default FormItem Label. [#1682](https://github.com/ant-design-blazor/ant-design-blazor/pull/1682) [@gmij](https://github.com/gmij)
  - 🐞 allow to use input components without bind-Value attribute inside customized form control. [#1662](https://github.com/ant-design-blazor/ant-design-blazor/pull/1662) [@anranruye](https://github.com/anranruye)
  - 📖 add advanced search demo. [#1654](https://github.com/ant-design-blazor/ant-design-blazor/pull/1654) [@ElderJames](https://github.com/ElderJames)

- i18n

  - 🌐 Russian locale resources additions. [#1709](https://github.com/ant-design-blazor/ant-design-blazor/pull/1709) [@kuznecovIT](https://github.com/kuznecovIT)
  - 🐞 When a node is missing from a resource file, the default value is used and no runtime exception is thrown. [#1710](https://github.com/ant-design-blazor/ant-design-blazor/pull/1710) [@anranruye](https://github.com/anranruye)

- 🆕 Tag shows pointer cursor when `OnClick` is set. [#1660](https://github.com/ant-design-blazor/ant-design-blazor/pull/1660) [@anddrzejb](https://github.com/anddrzejb)
- ⚡️ Modal and Drawer render reducing, update document and demo. [#1701](https://github.com/ant-design-blazor/ant-design-blazor/pull/1701) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed Select allow change both data source and value in one render period. [#1720](https://github.com/ant-design-blazor/ant-design-blazor/pull/1720) [@anranruye](https://github.com/anranruye)
- 🐞 Fixed Tabs horizontal scrolling of tabs [#1581](https://github.com/ant-design-blazor/ant-design-blazor/pull/1581) [@Brian-Ding](https://github.com/Brian-Ding)
- 🐞 Fix Statistic `CountDown` OnFinish callback exception(#1712). [#1714](https://github.com/ant-design-blazor/ant-design-blazor/pull/1714) [@HexJacaranda](https://github.com/HexJacaranda)
- 🐞 Fixed Overlay OnMaskClick event will fire correctly when the overlay size changes. [#1692](https://github.com/ant-design-blazor/ant-design-blazor/pull/1692) [@anranruye](https://github.com/anranruye)
- 🐞 Fixed Space items behavior when they are inside `if` block. [#1684](https://github.com/ant-design-blazor/ant-design-blazor/pull/1684) [@anranruye](https://github.com/anranruye)

- 🐞 Fix Grid gutter adjustment on col initialize. [#1653](https://github.com/ant-design-blazor/ant-design-blazor/pull/1653) [@ElderJames](https://github.com/ElderJames)

### 0.8.2

`2021-06-17`

- Table

  - 🐞 Fixed selection issues. [#1632](https://github.com/ant-design-blazor/ant-design-blazor/pull/1632) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed filter wrong compare operator after remove one filter condition; remove input components for 'Is Null' and 'Is Not Null' filter operators. [#1596](https://github.com/ant-design-blazor/ant-design-blazor/pull/1596) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed the issue that filters are not applied when close the filter panel by clicking filter icon area. [#1594](https://github.com/ant-design-blazor/ant-design-blazor/pull/1594) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed filter icon wrong highlight after clear the filter value; fix filter behavior when there is no input value. [#1592](https://github.com/ant-design-blazor/ant-design-blazor/pull/1592) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed can not close filter by confirm button. [#1602](https://github.com/ant-design-blazor/ant-design-blazor/pull/1602) [@anranruye](https://github.com/anranruye)
  - 📖 update ‘fork official sample’ demo to enable the sorters. [#1544](https://github.com/ant-design-blazor/ant-design-blazor/pull/1544) [@anranruye](https://github.com/anranruye)

- Dropdown

  - 🐞 Add typical `Button` propertied to `DropdownButton`. Include demo &amp; API docs for `Dropdown` API and `Button` API. [#1609](https://github.com/ant-design-blazor/ant-design-blazor/pull/1609) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Add `Loading` parameter to `DropdownButton`. [#1588](https://github.com/ant-design-blazor/ant-design-blazor/pull/1588) [@anddrzejb](https://github.com/anddrzejb)

- DatePicker

  - 🐞 Add OnClearClick eventcallback. [#1586](https://github.com/ant-design-blazor/ant-design-blazor/pull/1586) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 fix(module:datepicker): in form behavior. [#1617](https://github.com/ant-design-blazor/ant-design-blazor/pull/1617) [@anddrzejb](https://github.com/anddrzejb)

- InputNumber

  - 🐞 fix the exception which is throwed when an InputNumber component for nullable type loses focus. [#1612](https://github.com/ant-design-blazor/ant-design-blazor/pull/1612) [@anranruye](https://github.com/anranruye)
  - 🐞 fix(module:inputnumber): include parser in value evaluation. [#1567](https://github.com/ant-design-blazor/ant-design-blazor/pull/1567) [@anddrzejb](https://github.com/anddrzejb)

- 🐞 New parameters for `Input`: `Bordered`, `ReadOnly`, `InputElementSuffixClass` &amp; methods: `Focus()`, `Blur()`, fixed clear icon. <br> New parameter for `TextArea` `ShowCount`, fixed clear icon. <br> `Search` gets new look and paramter `ClassicSearchIcon` for fallback to old look. <br> `InputGroup` whitespace removed. <br> New parameters for `InputPassword`: `ShowPassword` &amp; `IconRender`. [#1530](https://github.com/ant-design-blazor/ant-design-blazor/pull/1530) [@anddrzejb](https://github.com/anddrzejb)

- 🐞 Fixed Affix: remove wrong event listeners. [#1616](https://github.com/ant-design-blazor/ant-design-blazor/pull/1616) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Drawer: OffsetX and offsetY do not work of Drawer, and update the documents how to use DrawerService by the way. [#1448](https://github.com/ant-design-blazor/ant-design-blazor/pull/1448) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed Radio: add defaultChecked and defaultValue. [#1494](https://github.com/ant-design-blazor/ant-design-blazor/pull/1494) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Tag: Add support for Status and custom colors, add animation demo [#1631](https://github.com/ant-design-blazor/ant-design-blazor/pull/1631) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Fixed Image: fix the style property position. [#1642](https://github.com/ant-design-blazor/ant-design-blazor/pull/1642) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Checkbox: in-form behavior of CheckboxGroup component. [#1637](https://github.com/ant-design-blazor/ant-design-blazor/pull/1637) [@anranruye](https://github.com/anranruye)
- 🌐 Fixed nl-BE and nl-NL locales. [#1624](https://github.com/ant-design-blazor/ant-design-blazor/pull/1624) [@gregloones](https://github.com/gregloones)
- 🛠 add missing 'filterOptions' node to german locale file. [#1562](https://github.com/ant-design-blazor/ant-design-blazor/pull/1562) [@anranruye](https://github.com/anranruye)
- 🌐 Added values missing from locale es-ES. [#1534](https://github.com/ant-design-blazor/ant-design-blazor/pull/1534) [@Magehernan](https://github.com/Magehernan)

### 0.8.1

`2021-05-13`

- Overlay

  - 🐞 Fixed positioning should take scroll into account. [#1511](https://github.com/ant-design-blazor/ant-design-blazor/pull/1511) [@ocoka](https://github.com/ocoka)
  - 🐞 Fixed issues in boundaryAdjustMode. [#1420](https://github.com/ant-design-blazor/ant-design-blazor/pull/1420) [@mutouzdl](https://github.com/mutouzdl)

- Input

  - 🐞 Fixed for Guid type. [#1510](https://github.com/ant-design-blazor/ant-design-blazor/pull/1510) [@anranruye](https://github.com/anranruye)
  - 🐞 Added `CultureInfo` attribute to `Input` type components. [#1480](https://github.com/ant-design-blazor/ant-design-blazor/pull/1480) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed binding data when the Enter key is pressed. [#1375](https://github.com/ant-design-blazor/ant-design-blazor/pull/1375) [@ElderJames](https://github.com/ElderJames)

- Table

  - 🐞 Fixed built-in filter select option width. [#1500](https://github.com/ant-design-blazor/ant-design-blazor/pull/1500) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed error for EndsWith filter operator. [#1434](https://github.com/ant-design-blazor/ant-design-blazor/pull/1434) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed column header sorter not refresh after ClearSorter is called [#1385](https://github.com/ant-design-blazor/ant-design-blazor/pull/1385) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed can not use DataIndex nullable mode with not nullable property [#1382](https://github.com/ant-design-blazor/ant-design-blazor/pull/1382) [@anranruye](https://github.com/anranruye)
  - 🐞 Fixed Filter for DataIndex. Unify FieldName, add DisplayAttribute for DiplayName. [#1372](https://github.com/ant-design-blazor/ant-design-blazor/pull/1372) [@Zonciu](https://github.com/Zonciu)
  - 🐞 Fixed ellipsis can't work. [#1376](https://github.com/ant-design-blazor/ant-design-blazor/pull/1376) [@ElderJames](https://github.com/ElderJames)

- Cascader

  - 🐞 Fixed showSearch. [#1484](https://github.com/ant-design-blazor/ant-design-blazor/pull/1484) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed invoking SelectedNodesChanged after clear selected. [#1437](https://github.com/ant-design-blazor/ant-design-blazor/pull/1437) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed incorrect size. [#1432](https://github.com/ant-design-blazor/ant-design-blazor/pull/1432) [@ElderJames](https://github.com/ElderJames)

- DatePicker

  - 🐞 Fixed panel click closing + some issues from #1431. [#1452](https://github.com/ant-design-blazor/ant-design-blazor/pull/1452) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed validate manually entered date against format. [#1389](https://github.com/ant-design-blazor/ant-design-blazor/pull/1389) [@anddrzejb](https://github.com/anddrzejb)

- Modal

  - 🐞 Fixed Delay time to DOM* MIN* TIMEOUT\_ VALUE (4ms). [#1445](https://github.com/ant-design-blazor/ant-design-blazor/pull/1445) [@zxyao145](https://github.com/zxyao145)
  - 🐞 Fixed add Dispose lifecycle function to Dialog. [#1379](https://github.com/ant-design-blazor/ant-design-blazor/pull/1379) [@zxyao145](https://github.com/zxyao145)
  - 🆕 support define modal's style in ModalOptions [#1400](https://github.com/ant-design-blazor/ant-design-blazor/pull/1400) [@zxyao145](https://github.com/zxyao145)

- Form

  - 🆕 Select mutliple/tags can be used in forms. [#1460](https://github.com/ant-design-blazor/ant-design-blazor/pull/1460) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed validation message unique [#1391](https://github.com/ant-design-blazor/ant-design-blazor/pull/1391) [@ElderJames](https://github.com/ElderJames)

- Select

  - 🐞 Fixed error for nullable TItem of SelectOption. [#1451](https://github.com/ant-design-blazor/ant-design-blazor/pull/1451) [@anranruye](https://github.com/anranruye)
  - 🛠 Refactor: use ResizeObserver Api instead of window.resize. [#1392](https://github.com/ant-design-blazor/ant-design-blazor/pull/1392) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed OnDataSourceChange called when expected. [#1419](https://github.com/ant-design-blazor/ant-design-blazor/pull/1419) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed enum default value protection. [#1368](https://github.com/ant-design-blazor/ant-design-blazor/pull/1368) [@anddrzejb](https://github.com/anddrzejb)

- 🐞 Fixed checkbox remove Value initialization blocking. [#1459](https://github.com/ant-design-blazor/ant-design-blazor/pull/1459) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Fixed input number self changing. [#1490](https://github.com/ant-design-blazor/ant-design-blazor/pull/1490) [@CAPCHIK](https://github.com/CAPCHIK)
- 🐞 `Checkbox` and `Switch` allow now binding to `Changed` property. `Value` and `Changed` properties can be used interchangeably. [#1394](https://github.com/ant-design-blazor/ant-design-blazor/pull/1394) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Fixed Tag closable typo and delete mode [#1393](https://github.com/ant-design-blazor/ant-design-blazor/pull/1393) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed PasswordInput for retrieving and setting the `selectionStart`; Override `onClick`. [#1377](https://github.com/ant-design-blazor/ant-design-blazor/pull/1377) [@MihailsKuzmins](https://github.com/MihailsKuzmins)
- 🆕 feat: add element component. [#1378](https://github.com/ant-design-blazor/ant-design-blazor/pull/1378) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Affix can't affix while OffsetTop is zero. [#1373](https://github.com/ant-design-blazor/ant-design-blazor/pull/1373) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed js function getDom return null bug. [#1417](https://github.com/ant-design-blazor/ant-design-blazor/pull/1417) [@zxyao145](https://github.com/zxyao145)
- 🐞 Fixed dropdown width for IE. [#1469](https://github.com/ant-design-blazor/ant-design-blazor/pull/1469) [@anranruye](https://github.com/anranruye)

### 0.8.0

`2021-04-15`

- Theme and i18n

  - 🔥 add built-in themes. [#1286](https://github.com/ant-design-blazor/ant-design-blazor/pull/1286) [@ElderJames](https://github.com/ElderJames)
  - 🔥 docs: dynamic primary color changing. [#1332](https://github.com/ant-design-blazor/ant-design-blazor/pull/1332) [@ElderJames](https://github.com/ElderJames)
  - 🔥 add RTL support. [#1238](https://github.com/ant-design-blazor/ant-design-blazor/pull/1238) [@ElderJames](https://github.com/ElderJames)

- Form

  - 📖 docs(module:form): IsModified sample fix. [#1344](https://github.com/ant-design-blazor/ant-design-blazor/pull/1344) [@anddrzejb](https://github.com/anddrzejb)
  - 🆕 feat: Form lable supports left alignment. [#1292](https://github.com/ant-design-blazor/ant-design-blazor/pull/1292) [@unsung189](https://github.com/unsung189)

- Select

  - 🆕 Added missing `MaxCountTag`, `MaxTagPlaceholder` and `MaxTagTextLenght`. [#1338](https://github.com/ant-design-blazor/ant-design-blazor/pull/1338) [@anddrzejb](https://github.com/anddrzejb)
  - 🆕 added `PopupContainerGrowToMatchWidestItem` &amp; `PopupContainerMaxWidth`. [#1309](https://github.com/ant-design-blazor/ant-design-blazor/pull/1309) [@anddrzejb](https://github.com/anddrzejb)

- Table

  - 🔥 add build-in filters [#1267](https://github.com/ant-design-blazor/ant-design-blazor/pull/1267) [@YMohd](https://github.com/YMohd)
  - 🆕 add support for column names from Display attribute. [#1310](https://github.com/ant-design-blazor/ant-design-blazor/pull/1310) [@anranruye](https://github.com/anranruye)
  - 🆕 add summary row. [#1218](https://github.com/ant-design-blazor/ant-design-blazor/pull/1218) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Added event handler `OnExpand`. [#1208](https://github.com/ant-design-blazor/ant-design-blazor/pull/1208) [@anddrzejb](https://github.com/anddrzejb)
  - 🆕 add `GetQueryModel` method. [#1202](https://github.com/ant-design-blazor/ant-design-blazor/pull/1202) [@ElderJames](https://github.com/ElderJames)

- Date Picker

  - 🐞 fix(module:datepicker): OnChange invoke after typing. [#1347](https://github.com/ant-design-blazor/ant-design-blazor/pull/1347) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Overlay: fix resize event handler. [#1322](https://github.com/ant-design-blazor/ant-design-blazor/pull/1322) [@anddrzejb](https://github.com/anddrzejb)

- 🆕 Space: add wrap,split and size array. [#1314](https://github.com/ant-design-blazor/ant-design-blazor/pull/1314) [@ElderJames](https://github.com/ElderJames)
- 🆕 Alert: add message template and loop banner demo [#1250](https://github.com/ant-design-blazor/ant-design-blazor/pull/1250) [@MutatePat](https://github.com/MutatePat)
- 🆕 Upload: Added events: `OnDownload`, `BeforeAllUpload` &amp; `BeforeAllUploadAsync`. [#1302](https://github.com/ant-design-blazor/ant-design-blazor/pull/1302) [@anddrzejb](https://github.com/anddrzejb)
- 🆕 Tag: add closing event [#1268](https://github.com/ant-design-blazor/ant-design-blazor/pull/1268) [@TimChen44](https://github.com/TimChen44)
- 🛠 Pagination Port from react version. [#1220](https://github.com/ant-design-blazor/ant-design-blazor/pull/1220) [@Zonciu](https://github.com/Zonciu)
- 🆕 InputNumber: add long-click and keyboard operation. [#1235](https://github.com/ant-design-blazor/ant-design-blazor/pull/1235) [@lingrepo](https://github.com/lingrepo)
- 🆕 add TestKit for public tests [#1248](https://github.com/ant-design-blazor/ant-design-blazor/pull/1248) [@MutatePat](https://github.com/MutatePat)
- 🆕 Input add parameter `WrapperStyle` [#1351](https://github.com/ant-design-blazor/ant-design-blazor/pull/1351) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Result: fix the issue for modify status unsuccessfully after initialization(#1328). [#1336](https://github.com/ant-design-blazor/ant-design-blazor/pull/1336) [@JiaChengLuo](https://github.com/JiaChengLuo)
- 🛠 1. Unified use of FeedbackComponent template components for modal comfirm and drawer; 2. Add "pure event handlers" helper class, avoid triggering statehaschanged in an event to cause repeated rendering. [#1263](https://github.com/ant-design-blazor/ant-design-blazor/pull/1263) [@zxyao145](https://github.com/zxyao145)
- 🐞 fix: multiple bugs originating from js. [#1342](https://github.com/ant-design-blazor/ant-design-blazor/pull/1342) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Affix: fix the issue for affix to the viewport. [#1335](https://github.com/ant-design-blazor/ant-design-blazor/pull/1335) [@skystardust](https://github.com/skystardust)
- 🐞 Drawer: fix ZIndex has no effect. [#1362](https://github.com/ant-design-blazor/ant-design-blazor/pull/1362) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Avatar: fix text size calc regional independent [#1352] (https://github.com/ant-design-blazor/ant-design-blazor/pull/1352) [@anddrzejb](https://github.com/anddrzejb)

### 0.7.4

`2021-04-08`

- Table

  - 🐞 Fixed issue with table not being re-rendered when setting ScrollX. [#1311](https://github.com/ant-design-blazor/ant-design-blazor/pull/1311) [@Zonciu](https://github.com/Zonciu)
  - 🐞 Fixed an issue where modifying a DataSource would throw an exception. [5b0dbfb](https://github.com/ant-design-blazor/ant-design-blazor/commit/5b0dbfb) [@Andrzej Bakun](https://github.com/Andrzej Bakun)
  - 🐞 Fix DataIndex Column Filter, make DataIndex column refresh correctly. [#1295](https://github.com/ant-design-blazor/ant-design-blazor/pull/1295) [@Zonciu](https://github.com/Zonciu)
  - 🐞 ExpandIconColumnIndex invalid when specified to the ActionColumn. [#1285](https://github.com/ant-design-blazor/ant-design-blazor/pull/1285) [@Magehernan](https://github.com/Magehernan)
  - 🐞 perf optimization & data source change issue [#1304](https://github.com/ant-design-blazor/ant-design-blazor/pull/1304) [@anddrzejb](https://github.com/anddrzejb)

- Select
  - 🐞 Fixed an issue where clicking the Close option on multiple selections would trigger a drop-down menu. [#1308](https://github.com/ant-design-blazor/ant-design-blazor/pull/1308) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed Loading icon in Tag mode. [12ca2f7](https://github.com/ant-design-blazor/ant-design-blazor/commit/12ca2f7) [@Andrzej Bakun](https://github.com/Andrzej Bakun)
- 💄 fix missing flex and wrap style. [#1296](https://github.com/ant-design-blazor/ant-design-blazor/pull/1296) [@ElderJames](https://github.com/ElderJames)
- 🐞 default to empty string. [6944c13](https://github.com/ant-design-blazor/ant-design-blazor/commit/6944c13) [@Andrzej Bakun](https://github.com/Andrzej Bakun)
- 🐞 Fix Upload list [53c1285](https://github.com/ant-design-blazor/ant-design-blazor/commit/53c1285) [@Andrzej Bakun](https://github.com/Andrzej Bakun)
- 🐞 Fix DatePicker DisabledDate works incorect. [#1298](https://github.com/ant-design-blazor/ant-design-blazor/pull/1298) [@mutouzdl](https://github.com/mutouzdl)
- 🆕 Added LabelTemplate in FormItem. [#1293](https://github.com/ant-design-blazor/ant-design-blazor/pull/1293) [@ldsenow](https://github.com/ldsenow)
- 🐞 Value has priority over DefaultValue. [5f14377](https://github.com/ant-design-blazor/ant-design-blazor/commit/5f14377) [@Andrzej Bakun](https://github.com/Andrzej Bakun)
- 📖 fix Table RouterPagging demo. [#1313](https://github.com/ant-design-blazor/ant-design-blazor/pull/1313) [@Zonciu](https://github.com/Zonciu)

## 0.7.3

`2021-03-29`

- 🐞 Fixed Dropdown: Animations for down and up are inverse. [#1274](https://github.com/ant-design-blazor/ant-design-blazor/pull/1274) [@mutouzdl](https://github.com/mutouzdl)
- 🐞 Fixed Tree nodes expand incorrectly. [#1275](https://github.com/ant-design-blazor/ant-design-blazor/pull/1275) [@TimChen44](https://github.com/TimChen44)
- 💄 Fixed Cascader an issue where the style attribute could not affect the style. [#1269](https://github.com/ant-design-blazor/ant-design-blazor/pull/1269) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Datepicker [selected date on end picker]、[quarter panel's ranges] are incorrect. [#1260](https://github.com/ant-design-blazor/ant-design-blazor/pull/1260) [@mutouzdl](https://github.com/mutouzdl)
- 📖 chore: add the copyright of .NET Foundation. [#1272](https://github.com/ant-design-blazor/ant-design-blazor/pull/1272) [@ElderJames](https://github.com/ElderJames)
- 📖 chore: fix cmd for preview site and style sync. [68c7539](https://github.com/ant-design-blazor/ant-design-blazor/commit/68c7539) [@ElderJames](https://github.com/ElderJames)

## 0.7.2

`2021-03-14`

- Table

  - 🐞 Fixed invoke `OnChange` twice on pagination was changed [#1211](https://github.com/ant-design-blazor/ant-design-blazor/pull/1211) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed clicking outside couldn't close filter panel [#1232](https://github.com/ant-design-blazor/ant-design-blazor/pull/1232) [@mutouzdl](https://github.com/mutouzdl)

- Select

  - 🐞 Fixed reset when changed to a not existing value [#1209](https://github.com/ant-design-blazor/ant-design-blazor/pull/1209) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed Compiled method `.ToString()` was throwing AmbigiousMethod [#1214](https://github.com/ant-design-blazor/ant-design-blazor/pull/1214) [@anddrzejb](https://github.com/anddrzejb)

- 🐞 Fixed divider style was default to plain [#1215](https://github.com/ant-design-blazor/ant-design-blazor/pull/1215) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed upload Disable and delete button [#1219](https://github.com/ant-design-blazor/ant-design-blazor/pull/1219) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Fixed animation missing prefix [#1243](https://github.com/ant-design-blazor/ant-design-blazor/pull/1243) [@Zonciu](https://github.com/Zonciu)
- 🐞 Fixed progress add missing trailing color [#1241](https://github.com/ant-design-blazor/ant-design-blazor/pull/1241) [@NPadrutt](https://github.com/NPadrutt)
- 🐞 Fixed badge color behavior [#1216](https://github.com/ant-design-blazor/ant-design-blazor/pull/1216) [@ElderJames](https://github.com/ElderJames)

## 0.7.1

`2021-03-05`

- 🐞 Fixed `Input Search` loading animation toggling. [#1195](https://github.com/ant-design-blazor/ant-design-blazor/pull/1195) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Fixed table doesn't refresh when expand row. [#1199](https://github.com/ant-design-blazor/ant-design-blazor/pull/1199) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed bring OnRowClick back [#1200](https://github.com/ant-design-blazor/ant-design-blazor/pull/1200) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed select press enter on form cause validation to fail [#1196](https://github.com/ant-design-blazor/ant-design-blazor/pull/1196) [@anddrzejb](https://github.com/anddrzejb)

## 0.7.0

`2021-03-02`

- 🔥 Add overlay boundary detection and orientation adjustment. [#1109](https://github.com/ant-design-blazor/ant-design-blazor/pull/1109) [@mutouzdl](https://github.com/mutouzdl)
- 🐞 Tree: fixed selected highlight confusion. [#1161](https://github.com/ant-design-blazor/ant-design-blazor/pull/1161) [@qinhuaihe](https://github.com/qinhuaihe)
- 🐞 Grid: fixed gutter. [#1158](https://github.com/ant-design-blazor/ant-design-blazor/pull/1158) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 List: Removed unintended console output from SetGutterStyle. [#1159](https://github.com/ant-design-blazor/ant-design-blazor/pull/1159) [@superjerry88](https://github.com/superjerry88)
- 🐞 Docs: fixed anchor and improvement. [#1107](https://github.com/ant-design-blazor/ant-design-blazor/pull/1107) [@ElderJames](https://github.com/ElderJames)

- Select:

  - 🔥 Use Func to get/set value instead of reflection. [#1168](https://github.com/ant-design-blazor/ant-design-blazor/pull/1168) [@Zonciu](https://github.com/Zonciu)
  - 🐞 Fixed two-way binding [#1191](https://github.com/ant-design-blazor/ant-design-blazor/pull/1191) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed tag duplication at multiple selection mode. [#1162](https://github.com/ant-design-blazor/ant-design-blazor/pull/1162) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed new tag item label and value. [#1121](https://github.com/ant-design-blazor/ant-design-blazor/pull/1121) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed DefaultActiveFirstOption. [#1115](https://github.com/ant-design-blazor/ant-design-blazor/pull/1115) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Removed `AllowCustomTags` and `OnCreateCustomTag`, added `PrefixIcon`. [#1087](https://github.com/ant-design-blazor/ant-design-blazor/pull/1087) [@anddrzejb](https://github.com/anddrzejb)

- Table:

  - 🔥 Add table filters. [#1178](https://github.com/ant-design-blazor/ant-design-blazor/pull/1178) [@ElderJames](https://github.com/ElderJames)
  - 🔥 Add editable cell/row demo. [#1152](https://github.com/ant-design-blazor/ant-design-blazor/pull/1152) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add OnRow and OnRow method. [#1175](https://github.com/ant-design-blazor/ant-design-blazor/pull/1175) [@qinhuaihe](https://github.com/qinhuaihe)
  - 🐞 Fixed selectedRows exception. [#1148](https://github.com/ant-design-blazor/ant-design-blazor/pull/1148) [@qinhuaihe](https://github.com/qinhuaihe)
  - 🐞 Support more generic units for scroll x/y. [#1137](https://github.com/ant-design-blazor/ant-design-blazor/pull/1137) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed missing value for the sort property of SortModel. [#1105](https://github.com/ant-design-blazor/ant-design-blazor/pull/1105) [@ElderJames](https://github.com/ElderJames)
  - Fixed showing empty status when there are no data. [#1180](https://github.com/ant-design-blazor/ant-design-blazor/pull/1180) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed tree data exception when `GetChildren` return Null。[#1188](https://github.com/ant-design-blazor/ant-design-blazor/pull/1188) [@ElderJames](https://github.com/ElderJames)

- DatePicker

  - 🆕 Fixed for not nullable - on clear set to defaults. [#1100](https://github.com/ant-design-blazor/ant-design-blazor/pull/1100) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed date typing, enter behavior, overlay toggle. [#1145](https://github.com/ant-design-blazor/ant-design-blazor/pull/1145) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed incorrect date format strings fix. [#1097](https://github.com/ant-design-blazor/ant-design-blazor/pull/1097) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed FirstDayOfWeek configuration. [#1054](https://github.com/ant-design-blazor/ant-design-blazor/pull/1054) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed placeholder and value equals null. [#1088](https://github.com/ant-design-blazor/ant-design-blazor/pull/1088) [@anddrzejb](https://github.com/anddrzejb)

- Steps

  - 🐞 Fixed wrong progress style. [#1072](https://github.com/ant-design-blazor/ant-design-blazor/pull/1072) [@ElderJames](https://github.com/ElderJames)
  - 🐞 fixed blocked navigation. [#1071](https://github.com/ant-design-blazor/ant-design-blazor/pull/1071) [@Tfurrer](https://github.com/Tfurrer)

- Menu

  - 🆕 Add MenuItem tooltip and SubMenu trigger type. [#1082](https://github.com/ant-design-blazor/ant-design-blazor/pull/1082) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add inline indent parameter. [#1076](https://github.com/ant-design-blazor/ant-design-blazor/pull/1076) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed Nav Menu Collapse Unresponsive. [#1144](https://github.com/ant-design-blazor/ant-design-blazor/pull/1144) [@mutouzdl](https://github.com/mutouzdl)
  - 🐞 Fixed active parent menu for routed links. [#1134](https://github.com/ant-design-blazor/ant-design-blazor/pull/1134) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 Fixed can't collapse follow the sider. [#1069](https://github.com/ant-design-blazor/ant-design-blazor/pull/1069) [@ElderJames](https://github.com/ElderJames)

- Cascader

  - 🔥 Add a pull-down effect to Cascader (integrated Overlay components). [#1112](https://github.com/ant-design-blazor/ant-design-blazor/pull/1112) [@mutouzdl](https://github.com/mutouzdl)
  - 🐞 Fixed `OnChange` called twice. [#1151](https://github.com/ant-design-blazor/ant-design-blazor/pull/1151) [@anddrzejb](https://github.com/anddrzejb)

- Input
- 🚫 Fixed pressing enter not updating the value. [#1094](https://github.com/ant-design-blazor/ant-design-blazor/pull/1094) [@Hona](https://github.com/Hona)
- 🐞 fixed the focus bug for InputPassword. [#1146](https://github.com/ant-design-blazor/ant-design-blazor/pull/1146) [@anddrzejb](https://github.com/anddrzejb)

## 0.6.0

`2021-02-01`

- Table
  - 🆕 support DataIndex, access object's property by path-based string. [#1056](https://github.com/ant-design-blazor/ant-design-blazor/pull/1056) [@Zonciu](https://github.com/Zonciu)
  - 🆕 add RowClassName attribute to table component. [#1031](https://github.com/ant-design-blazor/ant-design-blazor/pull/1031) [@mostrowski123](https://github.com/mostrowski123)
  - 🆕 add sort directions and default sort order. [#778](https://github.com/ant-design-blazor/ant-design-blazor/pull/778) [@cqgis](https://github.com/cqgis)
  - 🆕 support multiple sorter. [#1019](https://github.com/ant-design-blazor/ant-design-blazor/pull/1019) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add `expandiconColumnIndex` property to specify the column in which the expansion button resides. [#1002](https://github.com/ant-design-blazor/ant-design-blazor/pull/1002) [@fan0217](https://github.com/fan0217)
  - 🐞 fix selection was broken with scroll y. [#1020](https://github.com/ant-design-blazor/ant-design-blazor/pull/1020) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed style errors when expandTemplate is NULL and when data is empty. [#985](https://github.com/ant-design-blazor/ant-design-blazor/pull/985) [@Magehernan](https://github.com/Magehernan)
  - 🐞 Table component add custom comparer, fix table's blazor demo. [#969](https://github.com/ant-design-blazor/ant-design-blazor/pull/969) [@Zonciu](https://github.com/Zonciu)
- Menu
  - 🆕 Add Menu divider. [#1017](https://github.com/ant-design-blazor/ant-design-blazor/pull/1017) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 fix loop on same route & performance & duplicate highlight. [#1027](https://github.com/ant-design-blazor/ant-design-blazor/pull/1027) [@anddrzejb](https://github.com/anddrzejb)
- Overlay
  - 🆕 support overlay trigger without bound to a div. [#937](https://github.com/ant-design-blazor/ant-design-blazor/pull/937) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 fix the interop delElementFrom() exception on page refresh. [#1008](https://github.com/ant-design-blazor/ant-design-blazor/pull/1008) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 fix(module: overlay): getFirstChildDomInfo when firstElementChild is null (#989). [#989](https://github.com/ant-design-blazor/ant-design-blazor/pull/989) [@Andrzej Bakun](https://github.com/Andrzej Bakun)
- DatePicker

  - 🐞 prevent the time out of range. [#973](https://github.com/ant-design-blazor/ant-design-blazor/pull/973) [@ElderJames](https://github.com/ElderJames)
  - 🐞 DatePicker: fix throw exception when has default value. [#972](https://github.com/ant-design-blazor/ant-design-blazor/pull/972) [@ElderJames](https://github.com/ElderJames)

- 🆕 add image component. [#1038](https://github.com/ant-design-blazor/ant-design-blazor/pull/1038) [@ElderJames](https://github.com/ElderJames)
- 🆕 add a separate action component. [#1030](https://github.com/ant-design-blazor/ant-design-blazor/pull/1030) [@ElderJames](https://github.com/ElderJames)
- 🆕 Added Static class IconType [#987](https://github.com/ant-design-blazor/ant-design-blazor/pull/987) [@porkopek](https://github.com/porkopek)

- 🐞 layout: fix missing trigger when sider open from zero-width mode. [#1007](https://github.com/ant-design-blazor/ant-design-blazor/pull/1007) [@ElderJames](https://github.com/ElderJames)
- 💄 fix back-top visible styles. [#1005](https://github.com/ant-design-blazor/ant-design-blazor/pull/1005) [@ElderJames](https://github.com/ElderJames)
- 💄 fix upload file list style. [#1001](https://github.com/ant-design-blazor/ant-design-blazor/pull/1001) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix calendar critical exception for ChangePickerValue. [#993](https://github.com/ant-design-blazor/ant-design-blazor/pull/993) [@anddrzejb](https://github.com/anddrzejb)
- 💄 Fix the missing HTML div [#990](https://github.com/ant-design-blazor/ant-design-blazor/pull/990) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 cascader: re-initialize options after options is changed. [#980](https://github.com/ant-design-blazor/ant-design-blazor/pull/980) [@imhmao](https://github.com/imhmao)
- 🐞 fix Input/inputNumber/TextArea disabled attribute. [#1048](https://github.com/ant-design-blazor/ant-design-blazor/pull/1048) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix throwing exception on page reload. [#1040](https://github.com/ant-design-blazor/ant-design-blazor/pull/1040) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Fixed issue where rebinding model or calling 'reset' method could not clear validation error message [#1035](https://github.com/ant-design-blazor/ant-design-blazor/pull/1035) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix the ink delay change. [#1037](https://github.com/ant-design-blazor/ant-design-blazor/pull/1037) [@ElderJames](https://github.com/ElderJames)
- 📖 update docs cache with version tag. [cf2d4ed](https://github.com/ant-design-blazor/ant-design-blazor/commit/cf2d4ed) [@ElderJames](https://github.com/ElderJames)
- 💄 sync the style of ant-design v4.11.1. [#1039](https://github.com/ant-design-blazor/ant-design-blazor/pull/1039) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix select issue with multiple modals. [#1012](https://github.com/ant-design-blazor/ant-design-blazor/pull/1012) [@mutouzdl](https://github.com/mutouzdl)
- 🛠 update bUnit to 1.0.0-preview-01. [#1009](https://github.com/ant-design-blazor/ant-design-blazor/pull/1009) [@anddrzejb](https://github.com/anddrzejb)
- 📖 docs: scroll to hash anchor after pages are rendered. [#1006](https://github.com/ant-design-blazor/ant-design-blazor/pull/1006) [@ElderJames](https://github.com/ElderJames)
