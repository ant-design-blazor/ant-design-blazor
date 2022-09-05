/**
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE
 */

import { task } from 'gulp';
import { join } from 'path';
import { buildConfig } from '../../build-config';
import { cleanTask } from '../util/task-helpers';

/** Deletes the dist/ publish/ directory. */
task('clean', cleanTask([buildConfig.outputDir, buildConfig.publishDir, join(buildConfig.componentsDir, './core/JsInterop/main.ts')]));
