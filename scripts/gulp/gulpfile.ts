/**
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE
 */

/* eslint-disable import/no-unassigned-import */
import { series, task, parallel } from 'gulp';
import './tasks/clean';
import './tasks/default';

import './tasks/interop';
import './tasks/library';
import './tasks/site';

task('build:site', series('clean', 'init:site'));

task('build:lib', series('clean', 'build:library'));

task('build:all', series('clean', parallel('build:library', 'init:site')));