/**
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE
 */

import { task } from 'gulp';

const themeGenerate = require('../../site/generate-theme');
const colorGenerate = require('../../site/generateColorLess');

/** Parse demos and docs to site directory. */
task('init:site', done => {
  colorGenerate().then(themeGenerate).then(done);
});
