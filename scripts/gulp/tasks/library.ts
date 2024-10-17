/**
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE
 */

import { dest, parallel, series, src, task } from 'gulp';

import { join } from 'path';
import { mkdirsSync } from 'fs-extra';
import { buildConfig } from '../../build-config';
import { compile as compileLess } from '../../build/compile-styles';
import { generateLessVars } from '../../build/generate-less-vars';
import { copyStylesToSrc } from '../../build/migration-styles';

task('library:mkdir-dir', done => {
  mkdirsSync(buildConfig.publishDir);
  done();
})

// Compile less to the public directory.
task('library:compile-less', done => {
  compileLess().then(() => {
    copyStylesToSrc();
    done();
  });
});

// Compile less to the public directory.
task('library:generate-less-vars', done => {
  generateLessVars();
  done();
});

// Copies files without ngcc to lib folder.
task('library:copy-libs-css', () => {
  return src([join(buildConfig.publishDir, '*.css')]).pipe(dest(join(buildConfig.componentsDir, 'wwwroot/css')));
});

task('library:copy-libs-js', () => {
  return src([join(buildConfig.publishDir, '*.js*')]).pipe(dest(join(buildConfig.componentsDir, 'wwwroot/js')));
});

task('library:copy-libs-less', () => {
  // Copy all the less files, excluding the src folder which would duplicate some files.  
  return src([
      join(buildConfig.publishDir, '**/*.less'),
      '!' + join(buildConfig.publishDir, 'src', '**/*.less')
    ])
    .pipe(dest(join(buildConfig.componentsDir, 'wwwroot/less')));
});

task(
  'build:library',
  series(
    'library:mkdir-dir',
    parallel('library:scripts', 'library:compile-less'),
    parallel('library:copy-libs-css', 'library:copy-libs-js', 'library:copy-libs-less'),
  )
);