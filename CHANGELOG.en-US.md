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