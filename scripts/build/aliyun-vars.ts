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
const aliyunLess = fs.readFileSync(path.join(stylePath, 'themes', 'aliyun.less'), 'utf8');

export const aliyunPaletteLess = lessToJs(`${aliyunLess}`, {
  stripPrefix: true,
  resolveVariables: false
});
