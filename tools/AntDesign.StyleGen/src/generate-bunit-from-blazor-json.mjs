#!/usr/bin/env node

import fs from 'node:fs';
import crypto from 'node:crypto';
import path from 'node:path';
import process from 'node:process';

function escape(value) {
  return String(value)
    .replace(/\\/g, '\\\\')
    .replace(/"/g, '\\"')
    .replace(/\r?\n/g, '\\n');
}
function verbatim(value) { return String(value).replace(/"/g, '""'); }
function pascalCase(value) { return value.split(/[-.]/).map((part) => part ? part[0].toUpperCase() + part.slice(1) : part).join(''); }
function methodName(value) { return value.replace(/[^a-zA-Z0-9_]/g, '_'); }

function csharpValue(value) {
  if (value === null) return 'null';
  if (typeof value === 'boolean' || typeof value === 'number') return String(value).toLowerCase();
  return `"${escape(value)}"`;
}

function renderChildTree(node, builder = 'builder', indent = 0) {
  const pad = ' '.repeat(indent);
  if (Array.isArray(node)) {
    return node.map((child) => renderChildTree(child, builder, indent)).join('\n');
  }
  if (node.text !== undefined) {
    return `${pad}${builder}.AddContent(0, "${escape(node.text)}");`;
  }
  const lines = [`${pad}${builder}.OpenComponent(${indent}, ReactSourceTestSupport.ResolveComponentType("${escape(node.component)}"));`];
  let sequence = indent + 1;
  for (const [name, value] of Object.entries(node.parameters ?? {})) {
    lines.push(`${pad}${builder}.AddAttribute(${sequence}, "${escape(parameterName(name))}", ${csharpValue(value)});`);
    sequence += 1;
  }
  if (node.childContent) lines.push(`${pad}${builder}.AddAttribute(${sequence}, "ChildContent", (RenderFragment)(childBuilder => childBuilder.AddContent(0, "${escape(node.childContent)}")));`);
  lines.push(`${pad}${builder}.CloseComponent();`);
  return lines.join('\n');
}

function parameterName(name) {
  return name.length ? name[0].toUpperCase() + name.slice(1) : name;
}

function sourceComponent(item) {
  const match = item.react?.sourceFile?.match(/(?:^|\/)components\/([^/]+)/);
  return match?.[1] ?? 'requires-adapter';
}

function selectorExpression(selector) {
  return `cut.Find("${escape(selector)}")`;
}

function parameterAssignments(scenario, variable = 'parameters') {
  return Object.entries(scenario.parameters ?? {})
    .filter(([, value]) => value?.kind !== 'callback')
    .map(([name, value]) => `${variable}["${escape(parameterName(name))}"] = ${csharpValue(value)};`)
    .join('\n        ');
}

function callbackAssignments(scenario, variable = 'parameters') {
  const callbackTypes = {
    'Button:onClick': { parameter: 'OnClick', generic: 'MouseEventArgs' },
    'Switch:onClick': { parameter: 'OnClick', generic: null },
    'BackTop:onClick': { parameter: 'OnClick', generic: null },
    'Pagination:onChange': { parameter: 'OnChange', generic: 'PaginationEventArgs' },
    'Input:onChange': { parameter: 'OnChange', generic: 'string' },
    'TextArea:onChange': { parameter: 'OnChange', generic: 'string' },
    'Checkbox:onChange': { parameter: 'CheckedChanged', generic: 'bool' },
    'Radio:onChange': { parameter: 'CheckedChanged', generic: 'bool' },
    'Tabs:onChange': { parameter: 'OnChange', generic: 'string' },
    'Calendar:onChange': { parameter: 'OnChange', generic: 'DateTime' },
    'Calendar:onSelect': { parameter: 'OnSelect', generic: 'DateTime' },
    'DatePicker:onChange': { parameter: 'OnChange', generic: 'DateTimeChangedEventArgs<DateTime>' },
    'AutoComplete:onSelect': { parameter: 'OnSelectionChange', generic: 'AutoCompleteOption' },
  };
  return Object.entries(scenario.parameters ?? {})
    .filter(([, value]) => value?.kind === 'callback')
    .map(([name, value]) => {
      const metadata = callbackTypes[`${scenario.component}:${name}`];
      const parameter = metadata?.parameter ?? parameterName(name);
      const generic = metadata?.generic ? `<${metadata.generic}>` : '';
      const counter = metadata?.parameter ?? parameter;
      const originalCounter = `${parameter}Calls` === `${parameterName(name)}Calls` ? '' : `${parameterName(name)}Calls++; `;
      const sourceCounter = value.source && /^[A-Za-z_$][\w$]*$/.test(value.source) ? `${parameterName(value.source)}Calls++; ` : '';
      const callback = metadata?.generic ? `_ => { ${counter}Calls++; ${originalCounter}${sourceCounter}}` : `() => { ${counter}Calls++; ${originalCounter}${sourceCounter}}`;
      return `${variable}["${escape(parameter)}"] = EventCallback.Factory.Create${generic}(this, ${callback});`;
    })
    .join('\n        ');
}

function callbackDeclarations(scenario, assertions = []) {
  const mapped = {
    'Checkbox:onChange': 'CheckedChanged',
    'Radio:onChange': 'CheckedChanged',
    'AutoComplete:onSelect': 'OnSelectionChange',
  };
  const names = Object.entries(scenario.parameters ?? {})
    .filter(([, value]) => value?.kind === 'callback')
    .map(([name, value]) => [mapped[`${scenario.component}:${name}`] ?? parameterName(name), parameterName(name), value.source])
    .flatMap(([parameter, original, source]) => [parameter, original, source])
    .concat(assertions.filter((item) => item.type === 'callback-called').map((item) => parameterName(item.callback)))
    .filter((name, index, all) => name && all.indexOf(name) === index)
    .map((name) => `var ${name}Calls = 0;`)
    .join('\n        ');
  return names;
}

function childAssignment(scenario, variable = 'parameters') {
  if (scenario.childContent) {
    return `${variable}["ChildContent"] = (RenderFragment)(builder => builder.AddContent(0, "${escape(scenario.childContent)}"));`;
  }
  if (scenario.childTree) {
    return `${variable}["ChildContent"] = (RenderFragment)(builder => {
${renderChildTree(scenario.childTree, 'builder', 4)}
        });`;
  }
  return '';
}

function generateAssertion(assertion) {
  const selector = selectorExpression(assertion.selector);
  switch (assertion.type) {
    case 'exists':
      return `Assert.NotNull(${selector});`;
    case 'not-exists':
      return `Assert.Empty(cut.FindAll("${escape(assertion.selector)}"));`;
    case 'class':
      return `Assert.Contains(${csharpValue(assertion.expected)}, ${selector}.ClassList);`;
    case 'not-class':
      return `Assert.DoesNotContain(${csharpValue(assertion.expected)}, ${selector}.ClassList);`;
    case 'attribute':
      return `Assert.Equal(${csharpValue(assertion.expected)}, ${selector}.GetAttribute("${escape(assertion.name)}"));`;
    case 'not-attribute':
      return `Assert.NotEqual(${csharpValue(assertion.expected)}, ${selector}.GetAttribute("${escape(assertion.name)}"));`;
    case 'text':
      return `Assert.Contains(${csharpValue(assertion.expected)}, ${selector}.TextContent);`;
    case 'not-text':
      return `Assert.DoesNotContain(${csharpValue(assertion.expected)}, ${selector}.TextContent);`;
    case 'count':
      return `Assert.Equal(${csharpValue(assertion.expected)}, cut.FindAll("${escape(assertion.selector)}").Count);`;
    case 'callback-called': {
      const counter = `${({
        onChange: 'OnChange',
        onSelect: 'OnSelectionChange',
      }[assertion.callback] ?? parameterName(assertion.callback))}Calls`;
      if (assertion.negative) return `Assert.Equal(0, ${counter});`;
      if (assertion.expected !== undefined) return `Assert.Equal(${assertion.expected}, ${counter});`;
      return `Assert.NotEqual(0, ${counter});`;
    }
    default:
      return null;
  }
}

function generatedFileName(component) {
  return `ReactSource_${component.replace(/[^a-z0-9]+/gi, '-').toLowerCase()}-tests.g.cs`;
}

const manifestName = '.generated-manifest.json';
const generatedHeader = '// <auto-generated />';

function sha256(value) {
  return crypto.createHash('sha256').update(value).digest('hex');
}

export function cleanGeneratedOutput(output) {
  if (!fs.existsSync(output)) return [];
  const manifestPath = path.join(output, manifestName);
  const manifest = fs.existsSync(manifestPath) ? JSON.parse(fs.readFileSync(manifestPath, 'utf8')) : { files: [] };
  const files = new Set(Array.isArray(manifest.files) ? manifest.files : Object.keys(manifest.files ?? {}));
  for (const file of fs.readdirSync(output)) {
    const fullPath = path.join(output, file);
    if (file.endsWith('.g.cs') && (files.has(file) || fs.readFileSync(fullPath, 'utf8').startsWith(generatedHeader))) files.add(file);
  }
  for (const file of files) {
    const fullPath = path.join(output, file);
    if (fs.existsSync(fullPath) && fs.statSync(fullPath).isFile()) fs.unlinkSync(fullPath);
  }
  if (fs.existsSync(manifestPath)) fs.unlinkSync(manifestPath);
  return [...files];
}

function generateCase(item) {
  const scenario = item.blazor;
  const skipped = scenario.status !== 'candidate';
  const component = scenario.component ?? sourceComponent(item);
  const skipReason = scenario.status === 'skipped-component-not-found'
    ? `Blazor component '${component}' does not exist yet`
    : scenario.reason === 'callback-prop-requires-typed-event-adapter'
      ? 'React callback prop requires a typed EventCallback adapter'
      : scenario.reason === 'dynamic-jsx-expression-requires-adapter'
        ? 'React JSX expression requires an adapter'
        : 'React scenario requires a component adapter';
  const parameters = [parameterAssignments(scenario), callbackAssignments(scenario)].filter(Boolean).join('\n        ');
  const child = scenario.childContent
    ? `parameters["ChildContent"] = (RenderFragment)(builder => builder.AddContent(0, "${escape(scenario.childContent)}"));`
    : scenario.childTree
      ? `parameters["ChildContent"] = (RenderFragment)(builder => {\n${renderChildTree(scenario.childTree, 'builder', 4)}\n        });`
      : '';
  const snapshots = item.react.snapshots ?? [];
  let snapshotAssertion = snapshots.length === 1
    ? `// React snapshot: ${escape(snapshots[0].key)}\n        cut.MarkupMatches(@"${verbatim(snapshots[0].markup)}");`
    : `Assert.NotEmpty(cut.Markup); // ${snapshots.length} React snapshots require explicit scenario mapping`;
  const interactions = (scenario.interactions ?? []).map((action) => {
    if (action.type === 'wait') {
      return `cut.WaitForState(() => true, System.TimeSpan.FromMilliseconds(${action.milliseconds ?? 0}));`;
    }
    const target = selectorExpression(action.selector);
    if (action.type === 'keyboard') return `${target}.TriggerEvent("keydown");`;
    if (action.type === 'type') return `${target}.Change(${csharpValue(action.value ?? '')});`;
    if (action.type === 'change') return `${target}.Change(${csharpValue(action.value ?? '')});`;
    const method = action.type[0].toUpperCase() + action.type.slice(1);
    return `${target}.${method}();`;
  }).join('\n        ');
  const assertions = (scenario.assertions ?? []).map(generateAssertion).filter(Boolean).join('\n        ');
  const renderSteps = (scenario.renderSteps ?? []).map((step, index) => {
    const variable = `rerenderParameters${index}`;
    const assignments = [parameterAssignments(step, variable), callbackAssignments(step, variable)].filter(Boolean).join('\n        ');
    const child = childAssignment(step, variable);
    return `var ${variable} = new Dictionary<string, object?>();
        ${assignments}
        ${child}
        cut.SetParametersAndRender(builder =>
        {
            builder.Add(x => x.Type, type);
            builder.Add(x => x.Parameters, ${variable});
        });`;
  }).join('\n        ');
  if (interactions || assertions) {
    snapshotAssertion = [interactions, assertions, snapshotAssertion].filter(Boolean).join('\n        ');
  }
  if (renderSteps) snapshotAssertion = [renderSteps, snapshotAssertion].filter(Boolean).join('\n        ');
  const source = item.react.source?.replace(/\*\//g, '* /').split('\n').slice(0, 100).map((line) => `    // ${line}`).join('\n') ?? '    // React source unavailable';
  const testNameComment = String(item.react.name ?? '').split('\n').map((line) => `    // React test: ${escape(line)}`).join('\n');
  if (skipped) {
    return `\n${source}\n${testNameComment}\n    [Fact]
    public void React_${methodName(item.id)}_requires_a_real_Blazor_adapter()\n    {\n        throw new Xunit.Sdk.XunitException("${escape(skipReason)}; React scenario has not been translated to executable Blazor steps yet.");\n    }\n`;
  }
  return `\n${source}\n${testNameComment}\n    [Fact]\n    public void React_${methodName(item.id)}_renders_the_translated_Blazor_scenario()\n    {\n        ${callbackDeclarations(scenario, scenario.assertions ?? [])}\n        var parameters = new Dictionary<string, object?>();\n        ${parameters}\n        ${child}\n        var type = ReactSourceTestSupport.ResolveComponentType("${escape(scenario.component)}");\n        var cut = Context.RenderComponent<DynamicComponent>(builder =>\n        {\n            builder.Add(x => x.Type, type);\n            builder.Add(x => x.Parameters, parameters);\n        });\n\n        Assert.NotNull(cut);\n        ${snapshotAssertion}\n    }\n`;
}

function generateComponentFile(component, cases) {
  return `// <auto-generated />\nusing System;\nusing System.Collections.Generic;\nusing AntDesign;\nusing Bunit;\nusing Microsoft.AspNetCore.Components;\nusing Microsoft.AspNetCore.Components.Web;\nusing Xunit;\n\nnamespace AntDesignV6.Tests;\n\npublic sealed class ReactSource_${pascalCase(component)}Tests : AntDesign.Tests.AntDesignTestBase\n{${cases.map(generateCase).join('')}\n}\n`;
}

function generateSupportFile() {
  const lines = [
    '// <auto-generated />',
    'using System;',
    'using System.Linq;',
    'using AntDesign;',
    '',
    'namespace AntDesignV6.Tests;',
    '',
    'internal static class ReactSourceTestSupport',
    '{',
    '    public static Type ResolveComponentType(string component)',
    '    {',
    '        var typeName = string.Concat(component.Split(\'-\').Select(part => char.ToUpperInvariant(part[0]) + part[1..]));',
    '        var assembly = typeof(Button).Assembly;',
    '        var type = assembly.GetType($"AntDesign.{typeName}");',
    '        if (type is not null) return type;',
    '',
    '        var genericType = assembly.GetTypes().FirstOrDefault(candidate => candidate.Name == $"{typeName}`1" || candidate.Name == $"{typeName}`2");',
    '        if (genericType is not null)',
    '        {',
    '            var arguments = typeName switch',
    '            {',
    '                "InputNumber" or "Statistic" => new[] { typeof(decimal) },',
    '                "DatePicker" => new[] { typeof(DateTime) },',
    '                "Select" or "TreeSelect" => new[] { typeof(string), typeof(object) },',
    '                _ => new[] { typeof(string) },',
    '            };',
    '            return genericType.MakeGenericType(arguments);',
    '        }',
    '',
    '        throw new Xunit.Sdk.XunitException($"No Blazor component type found for \'{component}\'.");',
    '    }',
    '}',
  ];
  return `${lines.join('\n')}\n`;
}

export function generateBunitFromBlazorJson(input) {
  const value = typeof input === 'string' ? JSON.parse(fs.readFileSync(input, 'utf8')) : input;
  const grouped = new Map();
  for (const item of value.cases) {
    if (item.blazor.status !== 'candidate' && item.blazor.status !== 'skipped-component-not-found' && item.blazor.status !== 'requires-component-adapter') continue;
    const component = (item.blazor.component ?? sourceComponent(item)).replace(/\./g, '-').toLowerCase();
    if (!grouped.has(component)) grouped.set(component, []);
    grouped.get(component).push(item);
  }
  const files = { 'ReactSourceTestSupport.g.cs': generateSupportFile() };
  for (const [component, cases] of [...grouped.entries()].sort(([a], [b]) => a.localeCompare(b))) {
    files[generatedFileName(component)] = generateComponentFile(component, cases);
  }
  return files;
}

export function writeGeneratedBunitTests(input, output) {
  const inputPath = typeof input === 'string' ? path.resolve(input) : null;
  const files = generateBunitFromBlazorJson(input);
  fs.mkdirSync(output, { recursive: true });
  const removed = cleanGeneratedOutput(output);
  for (const [file, source] of Object.entries(files)) fs.writeFileSync(path.join(output, file), source);
  const manifest = {
    schemaVersion: 1,
    generator: 'tools/AntDesign.StyleGen/src/generate-bunit-from-blazor-json.mjs',
    source: inputPath,
    files: Object.fromEntries(Object.entries(files).map(([file, source]) => [file, sha256(source)])),
  };
  fs.writeFileSync(path.join(output, manifestName), `${JSON.stringify(manifest, null, 2)}\n`);
  return { files, removed, manifest };
}

if (import.meta.url === `file://${process.argv[1]}`) {
  const input = process.argv[2];
  const output = process.argv[3];
  if (!input || !output) { console.error('Usage: node generate-bunit-from-blazor-json.mjs <blazor-test-cases.json> <output-directory>'); process.exit(2); }
  const result = writeGeneratedBunitTests(input, output);
  console.log(JSON.stringify({ output, files: Object.keys(result.files).length, removed: result.removed.length, manifest: manifestName }, null, 2));
}
