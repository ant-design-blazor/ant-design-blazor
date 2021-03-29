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

## 0.7.3

`2021-03-29`

- 🐞 Fixed Dropdown: Animations for down and up are inverse. [#1274](https://github.com/ant-design/ant-design/pull/1274) [@mutouzdl](https://github.com/mutouzdl)
- 🐞 Fixed Tree nodes expand incorrectly. [#1275](https://github.com/ant-design/ant-design/pull/1275) [@TimChen44](https://github.com/TimChen44)
- 💄 Fixed Cascader an issue where the style attribute could not affect the style. [#1269](https://github.com/ant-design/ant-design/pull/1269) [@ElderJames](https://github.com/ElderJames)
- 🐞 Fixed Datepicker [selected date on end picker]、[quarter panel's ranges] are incorrect. [#1260](https://github.com/ant-design/ant-design/pull/1260) [@mutouzdl](https://github.com/mutouzdl)
- 📖 chore: add the copyright of .NET Foundation. [#1272](https://github.com/ant-design/ant-design/pull/1272) [@ElderJames](https://github.com/ElderJames)
- 📖 chore: fix cmd for preview site and style sync. [68c7539](https://github.com/ant-design/ant-design/commit/68c7539) [@ElderJames](https://github.com/ElderJames)

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
