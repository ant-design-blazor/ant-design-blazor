/**
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://github.com/NG-ZORRO/ant-design-blazor/blob/master/LICENSE
 */

import { dest, parallel, series, src, task } from 'gulp';

import * as fs from 'fs-extra';
import { join } from 'path';

import { buildConfig } from '../../build-config';
import { compile as compileLess } from '../../build/compile-styles';
import { generateLessVars } from '../../build/generate-less-vars';
import { copyStylesToSrc } from '../../build/migration-styles';
import { compileScripts }from '../../build/compile-scripts';

// import { execNodeTask } from '../util/task-helpers';

task('library:ensureDir', done => {
  fs.ensureDirSync(buildConfig.publishDir);
  done();
})

// Compile less to the public directory.
task('library:compile-less', done => {
  compileLess().then(() => {
    copyStylesToSrc();
    done();
  });
});

// Compile ts to the public directory.
task('library:compile-scripts', function () {
  return compileScripts().pipe(dest(join(buildConfig.publishDir, 'js')));
});

// Compile less to the public directory.
task('library:generate-less-vars', done => {
  generateLessVars();
  done();
});

// Copies README.md file to the public directory.
task('library:copy-resources', () => {
  return src([join(buildConfig.projectDir, 'README.md'), join(buildConfig.componentsDir)]).pipe(
    dest(join(buildConfig.publishDir))
  );
});

// Copies files without ngcc to lib folder.
task('library:copy-libs', () => {
  return src([join(buildConfig.publishDir, '**/*.css'),join(buildConfig.publishDir, '**/*.js*')]).pipe(dest(buildConfig.libDir));
});

task(
  'build:library',
  series(
    'clean',
    'library:ensureDir',
    parallel('library:compile-less', 'library:compile-scripts', 'library:copy-resources', 'library:generate-less-vars'),
    'library:copy-libs'
  )
);
