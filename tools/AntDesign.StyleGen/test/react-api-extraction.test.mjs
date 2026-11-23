import test from 'node:test';
import assert from 'node:assert/strict';
import fs from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import { extractReactApi } from '../src/extract-react-api-to-json.mjs';

test('extracts React props as the canonical API source', () => {
  const result = extractReactApi('/Volumes/Data/GitHub/ant-design');
  assert.equal(result.schemaVersion, 1);
  assert.ok(result.summary.components >= 70);
  assert.ok(result.summary.callbacks >= 200);

  const button = result.props.find((item) => item.component === 'button' && item.declaration === 'ButtonProps');
  assert.ok(button);
  assert.ok(button.properties.some((property) => property.name === 'loading'));
  assert.ok(result.summary.callbacks > 0);
});

test('expands local union and intersection type aliases used by component props', () => {
  const root = fs.mkdtempSync(path.join(os.tmpdir(), 'antd-react-api-types-'));
  const componentDir = path.join(root, 'components', 'sample');
  fs.mkdirSync(componentDir, { recursive: true });
  fs.writeFileSync(path.join(componentDir, 'index.tsx'), `
    type SingleProps<T> = { multiple?: false; value?: T | null; defaultValue?: T | null };
    type MultipleProps<T> = { multiple: true; value?: T[]; defaultValue?: T[] };
    type BaseProps<T> = { prefixCls?: string; options?: T[]; disabled?: boolean }
      & (SingleProps<T> | MultipleProps<T>)
      & { classNames?: Record<string, string>; styles?: Record<string, string> };
    export type SampleProps<T> = BaseProps<T> & { rootClassName?: string };
  `);

  try {
    const result = extractReactApi(root);
    const props = result.props.find((item) => item.component === 'sample' && item.declaration === 'SampleProps');
    assert.ok(props);
    for (const name of ['prefixCls', 'options', 'disabled', 'multiple', 'value', 'defaultValue', 'classNames', 'styles', 'rootClassName']) {
      assert.ok(props.properties.some((property) => property.name === name), `expected ${name} to be expanded`);
    }
  } finally {
    fs.rmSync(root, { recursive: true, force: true });
  }
});

test('resolves component props inherited from an imported package declaration', () => {
  const root = fs.mkdtempSync(path.join(os.tmpdir(), 'antd-react-api-imported-types-'));
  const componentDir = path.join(root, 'components', 'sample');
  const packageDir = path.join(root, 'node_modules', '@rc-component', 'sample');
  const reactTypesDir = path.join(root, 'node_modules', '@types', 'react');
  fs.mkdirSync(componentDir, { recursive: true });
  fs.mkdirSync(packageDir, { recursive: true });
  fs.mkdirSync(reactTypesDir, { recursive: true });
  fs.writeFileSync(path.join(root, 'tsconfig.json'), JSON.stringify({
    compilerOptions: { target: 'es2020', module: 'esnext', moduleResolution: 'bundler', strict: true },
  }));
  fs.writeFileSync(path.join(packageDir, 'package.json'), JSON.stringify({ types: 'index.d.ts' }));
  fs.writeFileSync(path.join(reactTypesDir, 'index.d.ts'), `
    export interface HTMLAttributes<T> {
      title?: string;
      role?: string;
      'aria-label'?: string;
    }
  `);
  fs.writeFileSync(path.join(packageDir, 'index.d.ts'), `
    import type { HTMLAttributes } from 'react';
    export interface RcSelectProps<T> extends HTMLAttributes<HTMLElement> {
      value?: T;
      onChange?: (value: T) => void;
      popupMatchSelectWidth?: boolean | number;
    }
  `);
  fs.writeFileSync(path.join(componentDir, 'index.tsx'), `
    import type { RcSelectProps } from '@rc-component/sample';
    export interface InternalSelectProps<T> extends Omit<RcSelectProps<T>, 'popupMatchSelectWidth'> {
      prefix?: string;
    }
    export interface SelectProps<T> extends InternalSelectProps<T> {
      popupMatchSelectWidth?: boolean | number;
    }
  `);

  try {
    const result = extractReactApi(root);
    const props = result.props.find((item) => item.component === 'sample' && item.declaration === 'SelectProps');
    assert.ok(props);
    assert.deepEqual(new Set(props.properties.map((property) => property.name)), new Set(['value', 'onChange', 'prefix', 'popupMatchSelectWidth']));
    assert.equal(props.properties.find((property) => property.name === 'onChange').kind, 'callback');
    assert.ok(!props.properties.some((property) => property.name === 'aria-label'));
  } finally {
    fs.rmSync(root, { recursive: true, force: true });
  }
});
