---
order: 6
title: Change Log
toc: false
timeline: true
---

`ant-design-blazor` strictly follows [Semantic Versioning 2.0.0](http://semver.org/).

#### Release Schedule

- Weekly release: patch version at the end of every week for routine bugfix (anytime for urgent bugfix).
- Monthly release: minor version at the end of every month for new features.
- Major version release is not included in this schedule for breaking change and new features.

---
## 0.7.0

`2021-03-02`

- 🚫 use Func to get/set value instead of reflection. [#1168](https://github.com/ant-design/ant-design/pull/1168) [@Zonciu](https://github.com/Zonciu)
- 🚫 add table filters. [#1178](https://github.com/ant-design/ant-design/pull/1178) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix(module:select): click to select on new tag. [#1162](https://github.com/ant-design/ant-design/pull/1162) [@anddrzejb](https://github.com/anddrzejb)
- 🚫 update docs. [536ba1a](https://github.com/ant-design/ant-design/commit/536ba1a) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix(module: tree) selected highlight confusion. [#1161](https://github.com/ant-design/ant-design/pull/1161) [@qinhuaihe](https://github.com/qinhuaihe)
- 🐞 fix(module:row): grid gutter fix. [#1158](https://github.com/ant-design/ant-design/pull/1158) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Removed unintended console output from SetGutterStyle. [#1159](https://github.com/ant-design/ant-design/pull/1159) [@superjerry88](https://github.com/superjerry88)
- 🐞 fix(module:inputpassword): focus fix. [#1146](https://github.com/ant-design/ant-design/pull/1146) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix(module:cascader): OnChange called twice. [#1151](https://github.com/ant-design/ant-design/pull/1151) [@anddrzejb](https://github.com/anddrzejb)
- 🚫 add editable cell/row demo. [#1152](https://github.com/ant-design/ant-design/pull/1152) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix(module:datepicker):date typing, enter behavior, overlay toggle. [#1145](https://github.com/ant-design/ant-design/pull/1145) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix(module:table): set selectedRows exception. [#1148](https://github.com/ant-design/ant-design/pull/1148) [@qinhuaihe](https://github.com/qinhuaihe)
- 🐞 Nav Menu Collapse Unresponsive. [#1144](https://github.com/ant-design/ant-design/pull/1144) [@mutouzdl](https://github.com/mutouzdl)
- 🐞 support more generic units for scroll x/y. [#1137](https://github.com/ant-design/ant-design/pull/1137) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix(module:select): new tag item label and value fix. [#1121](https://github.com/ant-design/ant-design/pull/1121) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix(module:menu): active parent menu for routed links. [#1134](https://github.com/ant-design/ant-design/pull/1134) [@anddrzejb](https://github.com/anddrzejb)
- 🚫 add overlay boundary detection and orientation adjustment. [#1109](https://github.com/ant-design/ant-design/pull/1109) [@mutouzdl](https://github.com/mutouzdl)
- 🐞 fix(module:select): property rename to follow docs. [#1115](https://github.com/ant-design/ant-design/pull/1115) [@anddrzejb](https://github.com/anddrzejb)
- 🚫 Add a pull-down effect to Cascader (integrated Overlay components). [#1112](https://github.com/ant-design/ant-design/pull/1112) [@mutouzdl](https://github.com/mutouzdl)
- 🚫 rename the docs project. [49a2d13](https://github.com/ant-design/ant-design/commit/49a2d13) [@ElderJames](https://github.com/ElderJames)
- 🐞 docs: fix anchor and improvement. [#1107](https://github.com/ant-design/ant-design/pull/1107) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix missing value for the sort property of SortModel. [#1105](https://github.com/ant-design/ant-design/pull/1105) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix(module:datepicker): for not nullable - on clear set to defaults. [#1100](https://github.com/ant-design/ant-design/pull/1100) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix(module:datepicker): incorrect date format strings fix. [#1097](https://github.com/ant-design/ant-design/pull/1097) [@anddrzejb](https://github.com/anddrzejb)
- 🚫 Fix pressing enter not updating the value. [#1094](https://github.com/ant-design/ant-design/pull/1094) [@Hona](https://github.com/Hona)
- 🐞 New `MondayIndex` property on `DatePickerLocale.cs` class that stores Monday index in `ShortWeekDays`. [#1054](https://github.com/ant-design/ant-design/pull/1054) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix(module:rangepicker): placeholder and value equals null. [#1088](https://github.com/ant-design/ant-design/pull/1088) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 removed `AllowCustomTags` and `OnCreateCustomTag` <br>    added `PrefixIcon`. [#1087](https://github.com/ant-design/ant-design/pull/1087) [@anddrzejb](https://github.com/anddrzejb)
- 🚫 add MenuItem tooltip and SubMenu trigger type. [#1082](https://github.com/ant-design/ant-design/pull/1082) [@ElderJames](https://github.com/ElderJames)
- 🚫 update the package version. [0e23bd0](https://github.com/ant-design/ant-design/commit/0e23bd0) [@ElderJames](https://github.com/ElderJames)
- 🚫 x. [#1077](https://github.com/ant-design/ant-design/pull/1077) [@MutatePat](https://github.com/MutatePat)
- 🚫 add inline indent parameter. [#1076](https://github.com/ant-design/ant-design/pull/1076) [@ElderJames](https://github.com/ElderJames)
- 💄 Fix steps wrong progress style. [#1072](https://github.com/ant-design/ant-design/pull/1072) [@ElderJames](https://github.com/ElderJames)
- 🚫 chore: sync ant-design v4.12.0. [#1067](https://github.com/ant-design/ant-design/pull/1067) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix: steps navigation was blocked. [#1071](https://github.com/ant-design/ant-design/pull/1071) [@Tfurrer](https://github.com/Tfurrer)
- 🐞 fix the menu can't collapse follow the sider. [#1069](https://github.com/ant-design/ant-design/pull/1069) [@ElderJames](https://github.com/ElderJames)

## 0.6.0

`2021-02-01`

- Table
  - 🆕 support DataIndex, access object's property by path-based string. [#1056](https://github.com/ant-design/ant-design/pull/1056) [@Zonciu](https://github.com/Zonciu)
  - 🆕 add RowClassName attribute to table component. [#1031](https://github.com/ant-design/ant-design/pull/1031) [@mostrowski123](https://github.com/mostrowski123)
  - 🆕 add sort directions and default sort order. [#778](https://github.com/ant-design/ant-design/pull/778) [@cqgis](https://github.com/cqgis)
  - 🆕 support multiple sorter. [#1019](https://github.com/ant-design/ant-design/pull/1019) [@ElderJames](https://github.com/ElderJames)
  - 🆕 Add `expandiconColumnIndex` property to specify the column in which the expansion button resides. [#1002](https://github.com/ant-design/ant-design/pull/1002) [@fan0217](https://github.com/fan0217)
  - 🐞 fix selection was broken with scroll y. [#1020](https://github.com/ant-design/ant-design/pull/1020) [@ElderJames](https://github.com/ElderJames)
  - 🐞 Fixed style errors when expandTemplate is NULL and when data is empty. [#985](https://github.com/ant-design/ant-design/pull/985) [@Magehernan](https://github.com/Magehernan)
  - 🐞 Table component add custom comparer, fix table's blazor demo. [#969](https://github.com/ant-design/ant-design/pull/969) [@Zonciu](https://github.com/Zonciu)
- Menu
  - 🆕 Add Menu divider. [#1017](https://github.com/ant-design/ant-design/pull/1017) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 fix loop on same route & performance & duplicate highlight. [#1027](https://github.com/ant-design/ant-design/pull/1027) [@anddrzejb](https://github.com/anddrzejb)
- Overlay
  - 🆕 support overlay trigger without bound to a div. [#937](https://github.com/ant-design/ant-design/pull/937) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 fix the interop delElementFrom() exception on page refresh. [#1008](https://github.com/ant-design/ant-design/pull/1008) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 fix(module: overlay): getFirstChildDomInfo when firstElementChild is null (#989). [#989](https://github.com/ant-design/ant-design/pull/989) [@Andrzej Bakun](https://github.com/Andrzej Bakun)
- DatePicker
  - 🐞 prevent the time out of range. [#973](https://github.com/ant-design/ant-design/pull/973) [@ElderJames](https://github.com/ElderJames)
  - 🐞 DatePicker: fix throw exception when has default value. [#972](https://github.com/ant-design/ant-design/pull/972) [@ElderJames](https://github.com/ElderJames)

- 🆕 add image component. [#1038](https://github.com/ant-design/ant-design/pull/1038) [@ElderJames](https://github.com/ElderJames)
- 🆕 add a separate action component. [#1030](https://github.com/ant-design/ant-design/pull/1030) [@ElderJames](https://github.com/ElderJames)
- 🆕 Added Static class IconType [#987](https://github.com/ant-design/ant-design/pull/987) [@porkopek](https://github.com/porkopek)

- 🐞 layout: fix missing trigger when sider open from zero-width mode. [#1007](https://github.com/ant-design/ant-design/pull/1007) [@ElderJames](https://github.com/ElderJames)
- 💄 fix back-top visible styles. [#1005](https://github.com/ant-design/ant-design/pull/1005) [@ElderJames](https://github.com/ElderJames)
- 💄 fix upload file list style. [#1001](https://github.com/ant-design/ant-design/pull/1001) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix calendar critical exception for ChangePickerValue. [#993](https://github.com/ant-design/ant-design/pull/993) [@anddrzejb](https://github.com/anddrzejb)
- 💄 Fix the missing HTML div [#990](https://github.com/ant-design/ant-design/pull/990) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 cascader: re-initialize options after options is changed. [#980](https://github.com/ant-design/ant-design/pull/980) [@imhmao](https://github.com/imhmao)
- 🐞 fix Input/inputNumber/TextArea disabled attribute. [#1048](https://github.com/ant-design/ant-design/pull/1048) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix throwing exception on page reload. [#1040](https://github.com/ant-design/ant-design/pull/1040) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Fixed issue where rebinding model or calling 'reset' method could not clear validation error message [#1035](https://github.com/ant-design/ant-design/pull/1035) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix the ink delay change. [#1037](https://github.com/ant-design/ant-design/pull/1037) [@ElderJames](https://github.com/ElderJames)
- 📖 update docs cache with version tag. [cf2d4ed](https://github.com/ant-design/ant-design/commit/cf2d4ed) [@ElderJames](https://github.com/ElderJames)
- 💄 sync the style of ant-design v4.11.1. [#1039](https://github.com/ant-design/ant-design/pull/1039) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix select issue with multiple modals. [#1012](https://github.com/ant-design/ant-design/pull/1012) [@mutouzdl](https://github.com/mutouzdl)
- 🛠 update bUnit  to 1.0.0-preview-01. [#1009](https://github.com/ant-design/ant-design/pull/1009) [@anddrzejb](https://github.com/anddrzejb)
- 📖 docs: scroll to hash anchor after pages are rendered. [#1006](https://github.com/ant-design/ant-design/pull/1006) [@ElderJames](https://github.com/ElderJames)