/**
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://github.com/NG-ZORRO/ant-design-blazor/blob/master/LICENSE
 */

/* eslint-disable import/no-unassigned-import */
import { task, series } from 'gulp';
import './tasks/clean';
import './tasks/default';
// import './tasks/schematic';
import './tasks/unit-test';
// import './tasks/universal';

import './tasks/library';
// import './tasks/site';

task('build:release', series('clean', 'build:library'));

// task('build:preview', series('clean', 'init:site', 'build:site-doc-es5'));

// task('start:dev', series('clean', 'start:site'));
