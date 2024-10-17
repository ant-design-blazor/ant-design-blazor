/**
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE
 */

import * as fs from 'fs-extra';

import * as path from 'path';

import { buildConfig } from '../build-config';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const lessToJs = require('less-vars-to-js') as any;

const stylePath = path.join(buildConfig.componentsDir, 'style');
const colorLess = fs.readFileSync(path.join(stylePath, 'color', 'colors.less'), 'utf8');
const compactLess = fs.readFileSync(path.join(stylePath, 'themes', 'compact.less'), 'utf8');
const darkLess = fs.readFileSync(path.join(stylePath, 'themes', 'dark.less'), 'utf8');

export const compactDarkPaletteLess = lessToJs(`${colorLess}${compactLess}${darkLess}`, {
  stripPrefix: true,
  resolveVariables: false
});
