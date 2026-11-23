#!/usr/bin/env node

import fs from 'node:fs';
import path from 'node:path';

const [reactApiPath = 'docs/antd-v6/react-api.json', blazorApiPath = 'docs/antd-v6/blazor-api.json', outputPath = 'docs/antd-v6/react-to-blazor-api-map.json'] = process.argv.slice(2);

// These are deliberate API translations. They are not heuristics and must not
// be used to remove or rewrite a React scenario from the canonical React IR.
const aliases = new Map([
  ['children', 'ChildContent'],
  ['className', 'Class'],
  ['onClick', 'OnClick'],
  ['onChange', 'OnChange'],
  ['onBlur', 'OnBlur'],
  ['onFocus', 'OnFocus'],
  ['onMouseEnter', 'OnMouseEnter'],
  ['onMouseLeave', 'OnMouseLeave'],
]);

// Component-scoped aliases are used only where the React and Blazor APIs have
// the same semantic field but different established names. Keeping these
// scoped prevents a name such as `isActive` from being applied to unrelated
// components.
const scopedAliases = new Map([
  ['collapse:isActive', 'Active'],
  ['alert:title', 'Message'],
  ['alert:isClosable', 'Closable'],
  ['button:href', 'Url'],
  ['form:isRequired', 'Required'],
  ['progress:railColor', 'TrailColor'],
  ['drawer:open', 'Visible'],
  ['modal:open', 'Visible'],
  ['select:onOpenChange', 'OpenChanged'],
  ['select:onChange', 'ValueChanged'],
  ['select:popupStyle', 'DropdownStyle'],
  ['table:render', 'CellRender'],
  ['table:expandedRowRender', 'ExpandTemplate'],
  ['tag:onChange', 'CheckedChanged'],
]);

// React has a separate TimePicker type, while the Blazor TimePicker inherits
// the DatePicker parameter surface and is inventoried under that base type.
const componentAliases = new Map([
  ['timepicker', 'date-picker'],
]);

// Some component folders also declare private hook/overlay contracts that are
// discovered by the canonical React type extractor. They are useful in the
// source IR but are not props accepted by the public component. Restrict these
// components to the declarations exported by their public entry point.
const componentPublicDeclarations = new Map([
  ['table', new Set(['TableProps', 'ColumnProps', 'ColumnGroupProps'])],
]);

// Same-name properties can still expose a different subset or shape. Keep
// these cases visible in the generated inventory instead of implying that a
// casing match means full API parity.
const scopedShapeNotes = new Map([
  ['tag:icon', 'React accepts any ReactNode; Blazor supports named Icon strings and IconTemplate RenderFragments.'],
  ['tag:classNames', 'React accepts an object or a props callback; Blazor currently accepts the semantic slot dictionary only.'],
  ['tag:styles', 'React accepts an object or a props callback; Blazor currently accepts the semantic slot dictionary only.'],
  ['tag:options', 'React options accept string or number values and ReactNode labels; Blazor CheckableTagOption currently uses string values and string labels.'],
  ['tag:defaultValue', 'React values support strings, numbers, and arrays of either; Blazor supports string values or IReadOnlyList<string>.'],
  ['tag:onChange', 'CheckableTag.onChange maps to CheckedChanged; CheckableTagGroup uses ValueChanged or ValuesChanged depending on Multiple.'],
  ['dropdown:menu', 'Blazor DropdownMenuOptions supports data-driven items, groups, submenus, selection, activeKey, icons, state, and click callbacks; verify remaining React MenuProps fields separately.'],
  ['dropdown:getPopupContainer', 'Blazor accepts Func<ElementReference, ElementReference> to select the popup parent; the callback intent matches React, with Blazor DOM-reference types.'],
  ['dropdown:align', 'React accepts rc-trigger AlignType (points, offset, targetOffset, and overflow); Blazor currently exposes Placement and scalar offsets, so arbitrary alignment objects are not yet represented.'],
  ['dropdown:autoFocus', 'React focuses the popup menu when opened; Blazor does not yet provide equivalent popup focus management.'],
  ['button:classNames', 'React classNames accepts an object or a callback; Blazor exposes the object as ClassNames and the callback as ClassNamesFunc receiving the current Button instance.'],
  ['button:styles', 'React styles accepts an object or a callback; Blazor exposes the object as Styles and the callback as StylesFunc receiving the current Button instance.'],
  ['select:classNames', 'React classNames accepts an object or a props callback; Blazor exposes the semantic dictionary as ClassNames and a ClassNamesFunc callback receiving the current Select instance.'],
  ['select:styles', 'React styles accepts an object or a props callback; Blazor exposes the semantic dictionary as Styles and a StylesFunc callback receiving the current Select instance.'],
  ['select:onChange', 'React onChange uses one callback whose value shape depends on mode; Blazor exposes ValueChanged for single mode and ValuesChanged for multiple/tags mode.'],
  ['select:prefixCls', 'React prefixCls is supplied through ConfigProvider in Blazor; Select does not expose a per-instance prefix override.'],
  ['select:direction', 'Blazor accepts a per-Select Direction override and inherits ConfigProvider direction when omitted.'],
  ['select:popupStyle', 'React popupStyle is represented by Blazor DropdownStyle.'],
  ['select:showScrollBar', 'rc-select virtual-list scrollbar rendering option; Blazor virtualization does not expose this presentation flag.'],
  ['select:choiceTransitionName', 'rc-select internal choice animation override; Blazor uses the shared component motion tokens.'],
  ['select:getInputElement', 'React custom input rendering callback; Blazor Select currently owns its input markup.'],
  ['select:getRawInputElement', 'React custom raw input rendering callback; Blazor Select currently owns its input markup.'],
  ['select:animation', 'Legacy rc-trigger animation API; Blazor uses the Overlay motion configuration.'],
  ['select:transitionName', 'Legacy rc-trigger transition API; Blazor uses the Overlay motion configuration.'],
  ['select:popupAlign', 'React rc-trigger alignment object is represented by Placement and overlay boundary adjustment settings in Blazor; arbitrary AlignType offsets are not exposed.'],
  ['select:builtinPlacements', 'React rc-trigger placement map is represented by Blazor Placement; custom placement maps are not exposed.'],
  ['select:showAction', 'React trigger activation list is represented by Blazor AutoFocus/Open and pointer events; a configurable activation list is not exposed.'],
  ['select:components', 'React virtual-list component injection API has no direct Blazor extension point.'],
  ['select:backfill', 'React combobox backfill behavior is not part of the Blazor Select modes.'],
  ['select:listItemHeight', 'React virtual-list row height tuning is fixed to the component option height in Blazor.'],
  ['table:sorter', 'React sorter accepts a boolean, comparator, or object with comparator and priority; Blazor splits these into Sortable, SorterCompare, and SorterMultiple.'],
  ['table:sortOrder', 'React sortOrder is controlled state; Blazor currently exposes DefaultSortOrder for initial state and does not expose a controlled per-column sortOrder parameter.'],
  ['table:filterDropdownProps', 'React filterDropdownProps configures rc-dropdown; Blazor exposes FilterDropdownOpen, FilterDropdownAutoClose, and GetPopupContainer separately, without the full DropdownProps object.'],
  ['table:filteredValue', 'React filteredValue is a controlled list; Blazor supports DefaultFilteredValue for initialization and Filtered for the indicator, but not controlled per-column selected values.'],
  ['table:onFilterDropdownOpenChange', 'React callback was folded into filterDropdownProps.onOpenChange; Blazor exposes FilterDropdownOpen state but no matching per-column open-change callback.'],
  ['table:shouldCellUpdate', 'React provides a per-cell render predicate; Blazor uses component-level RerenderStrategy and has no per-cell predicate.'],
  ['table:onCellClick', 'React click callback receives the record and mouse event; Blazor can attach cell attributes and handlers through OnCell, but does not expose this typed callback.'],
  ['table:dropdownPrefixCls', 'React prefix customization is supplied through ConfigProvider in Blazor.'],
  ['table:columns', 'React passes a data array of column configs; Blazor declares columns with Column child components or ColumnDefinitions templates.'],
  ['table:pagination', 'React pagination config is represented by Blazor page parameters and PaginationTemplate; see the Table API mapping notes.'],
  ['table:rowSelection', 'React rowSelection config is represented by Blazor Selection/SelectionColumn parameters and callbacks rather than one object parameter.'],
  ['table:scroll', 'React scroll config is split into Blazor ScrollX, ScrollY, and ScrollToFirstRowOnChange.'],
  ['table:prefixCls', 'React prefixCls is supplied through ConfigProvider in Blazor.'],
  ['table:direction', 'React direction is inherited from ConfigProvider direction in Blazor.'],
  ['table:components', 'React can replace internal table elements; Blazor Table has no equivalent component override API.'],
  ['table:expandIcon', 'React accepts a custom expand icon renderer; Blazor exposes expansion content and column placement but no icon renderer parameter.'],
  ['table:summary', 'React summary receives current page data; Blazor uses declarative SummaryRow components and does not pass the current data to a callback.'],
  ['table:sticky', 'Blazor exposes StickyOffsetHeader; React sticky offsetScroll and getContainer settings have no matching parameters.'],
  ['table:expandedRowKeys', 'React expandedRowKeys is controlled state; Blazor exposes DefaultExpandedRowKeys and OnExpandedRowsChange without a controlled key parameter.'],
  ['table:expandedRowRender', 'React expandedRowRender maps to Blazor ExpandTemplate with RowData context.'],
  ['table:expandRowByClick', 'Blazor currently expands rows through the expand control and does not expose expandRowByClick.'],
  ['table:childrenColumnName', 'React selects tree children by property name; Blazor accepts a TreeChildren accessor function.'],
  ['table:expandable', 'React expandable config is represented by Blazor expansion parameters and callbacks; see the Table API mapping notes.'],
  ['table:caption', 'React caption is not currently rendered by Blazor Table.'],
  ['table:tailor', 'Low-level rc-table implementation option; it is not part of the Ant Design Table documentation API.'],
  ['table:getContainerWidth', 'Low-level rc-table measurement callback; it is not part of the Ant Design Table documentation API.'],
  ['table:measureRowRender', 'Low-level rc-table measurement renderer; it is not part of the Ant Design Table documentation API.'],
  ['table:render', 'React render maps to Blazor CellRender using a CellData context; callback argument shapes differ.'],
  ['table:onCell', 'The callback intent matches, but React receives record/index and Blazor receives a CellData object.'],
  ['table:onHeaderCell', 'The callback intent matches, but React receives a column and Blazor supplies a parameterless attribute factory.'],
  ['table:footer', 'React footer is a callback receiving current page data; Blazor FooterTemplate has no data argument and Footer is string-valued.'],
  ['table:rowClassName', 'React rowClassName receives record/index; Blazor RowClassName receives a RowData object.'],
  ['table:loading', 'React accepts Spin props as well as a boolean; Blazor Loading currently accepts only a boolean.'],
]);

const normalize = value => value.replace(/[^a-z0-9]/gi, '').toLowerCase();

function findParameter(componentName, component, reactName) {
  const exact = component?.parameters.find(parameter => normalize(parameter.name) === normalize(reactName));
  if (exact) return { parameter: exact, status: 'exact', reason: 'case-insensitive-name-match' };

  const alias = scopedAliases.get(`${componentName}:${reactName}`) ?? aliases.get(reactName);
  if (alias) {
    const translated = component?.parameters.find(parameter => parameter.name === alias);
    if (translated) return { parameter: translated, status: 'alias', reason: `explicit-alias:${reactName}->${alias}` };
  }

  return { parameter: null, status: 'missing', reason: alias ? `explicit-alias-not-found:${reactName}->${alias}` : 'no-explicit-mapping' };
}

export function mapReactApiToBlazor(reactApi, blazorApi) {
  const blazorByComponent = new Map(blazorApi.components.map(component => [normalize(component.component), component]));
  const componentRows = new Map();
  for (const declaration of reactApi.props) {
    // Demo-local helpers may be declared inside a component's demo folder, but
    // they are not part of the public component API. Keep them in the canonical
    // React API IR; omit them only from this component-to-component mapping.
    if (/(?:^|\/)demo\//i.test(declaration.sourceFile ?? '')) continue;
    const publicDeclarations = componentPublicDeclarations.get(normalize(declaration.component));
    if (publicDeclarations && !publicDeclarations.has(declaration.declaration)) continue;
    const row = componentRows.get(declaration.component) ?? { component: declaration.component, declarations: [], props: [] };
    row.declarations.push(declaration.declaration);
    for (const property of declaration.properties) {
      if (!row.props.some(existing => existing.name === property.name)) row.props.push(property);
    }
    componentRows.set(declaration.component, row);
  }

  const components = [...componentRows.values()].map(row => {
    const targetComponent = componentAliases.get(normalize(row.component)) ?? row.component;
    const blazorComponent = blazorByComponent.get(normalize(targetComponent));
    const props = row.props.map(property => {
      const mapping = findParameter(row.component, blazorComponent, property.name);
      return {
        reactProp: property.name,
        reactType: property.type,
        kind: property.kind,
        blazorComponent: blazorComponent?.component ?? null,
        blazorParameter: mapping.parameter?.name ?? null,
        blazorType: mapping.parameter?.type ?? null,
        blazorReleaseVersion: mapping.parameter?.releaseVersion ?? null,
        status: blazorComponent ? mapping.status : 'missing-component',
        reason: blazorComponent ? mapping.reason : 'no-blazor-component',
        shapeNote: scopedShapeNotes.get(`${normalize(row.component)}:${property.name}`) ?? null,
      };
    });
    return {
      component: row.component,
      declarations: row.declarations,
      blazorComponent: blazorComponent?.component ?? null,
      status: blazorComponent ? 'mapped' : 'missing-component',
      props,
    };
  });

  const allProps = components.flatMap(component => component.props);
  const summary = {
    reactComponents: components.length,
    reactProps: allProps.length,
    mappedComponents: components.filter(component => component.status === 'mapped').length,
    missingComponents: components.filter(component => component.status === 'missing-component').length,
    exact: allProps.filter(property => property.status === 'exact').length,
    alias: allProps.filter(property => property.status === 'alias').length,
    shapeDifferences: allProps.filter(property => property.shapeNote).length,
    missing: allProps.filter(property => property.status === 'missing' || property.status === 'missing-component').length,
    callbacks: allProps.filter(property => property.kind === 'callback').length,
  };
  return { summary, components };
}

if (process.argv[1] && path.resolve(process.argv[1]) === path.resolve(new URL(import.meta.url).pathname)) {
  const reactApi = JSON.parse(fs.readFileSync(reactApiPath, 'utf8'));
  const blazorApi = JSON.parse(fs.readFileSync(blazorApiPath, 'utf8'));
  const result = mapReactApiToBlazor(reactApi, blazorApi);
  const output = {
    schemaVersion: 1,
    source: { reactApi: path.relative(process.cwd(), reactApiPath), blazorApi: path.relative(process.cwd(), blazorApiPath), mapping: 'react-primary-explicit-target-mapping' },
    summary: result.summary,
    aliases: Object.fromEntries(aliases),
    scopedAliases: Object.fromEntries(scopedAliases),
    componentAliases: Object.fromEntries(componentAliases),
    componentPublicDeclarations: Object.fromEntries([...componentPublicDeclarations].map(([component, declarations]) => [component, [...declarations]])),
    scopedShapeNotes: Object.fromEntries(scopedShapeNotes),
    components: result.components,
  };

  fs.mkdirSync(path.dirname(outputPath), { recursive: true });
  fs.writeFileSync(outputPath, `${JSON.stringify(output, null, 2)}\n`);
  console.log(JSON.stringify({ output: outputPath, ...result.summary }, null, 2));
}
