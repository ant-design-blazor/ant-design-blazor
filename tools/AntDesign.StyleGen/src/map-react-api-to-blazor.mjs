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
]);

// Same-name properties can still expose a different subset or shape. Keep
// these cases visible in the generated inventory instead of implying that a
// casing match means full API parity.
const scopedShapeNotes = new Map([
  ['select:fetchOptions', 'React fetchOptions owns an asynchronous remote-search lifecycle; Blazor has caller-controlled DataSource and OnSearch but no asynchronous option-fetch contract yet.'],
  ['dropdown:menu', 'Blazor DropdownMenuOptions supports data-driven items, groups, submenus, selection, activeKey, icons, state, and click callbacks; verify remaining React MenuProps fields separately.'],
  ['dropdown:getPopupContainer', 'Blazor accepts Func<ElementReference, ElementReference> to select the popup parent; the callback intent matches React, with Blazor DOM-reference types.'],
  ['dropdown:align', 'React accepts rc-trigger AlignType (points, offset, targetOffset, and overflow); Blazor currently exposes Placement and scalar offsets, so arbitrary alignment objects are not yet represented.'],
  ['dropdown:autoFocus', 'React focuses the popup menu when opened; Blazor does not yet provide equivalent popup focus management.'],
  ['button:classNames', 'React classNames accepts an object or a callback; Blazor exposes the object as ClassNames and the callback as ClassNamesFunc receiving the current Button instance.'],
  ['button:styles', 'React styles accepts an object or a callback; Blazor exposes the object as Styles and the callback as StylesFunc receiving the current Button instance.'],
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
    const row = componentRows.get(declaration.component) ?? { component: declaration.component, declarations: [], props: [] };
    row.declarations.push(declaration.declaration);
    for (const property of declaration.properties) {
      if (!row.props.some(existing => existing.name === property.name)) row.props.push(property);
    }
    componentRows.set(declaration.component, row);
  }

  const components = [...componentRows.values()].map(row => {
    const blazorComponent = blazorByComponent.get(normalize(row.component));
    const props = row.props.map(property => {
      const mapping = findParameter(row.component, blazorComponent, property.name);
      return {
        reactProp: property.name,
        reactType: property.type,
        kind: property.kind,
        blazorComponent: blazorComponent?.component ?? null,
        blazorParameter: mapping.parameter?.name ?? null,
        blazorType: mapping.parameter?.type ?? null,
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
    scopedShapeNotes: Object.fromEntries(scopedShapeNotes),
    components: result.components,
  };

  fs.mkdirSync(path.dirname(outputPath), { recursive: true });
  fs.writeFileSync(outputPath, `${JSON.stringify(output, null, 2)}\n`);
  console.log(JSON.stringify({ output: outputPath, ...result.summary }, null, 2));
}
