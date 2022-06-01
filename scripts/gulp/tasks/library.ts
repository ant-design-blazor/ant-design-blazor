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

// Copies README.md file to the public directory.
task('library:copy-resources', () => {
  return src([join(buildConfig.projectDir, 'README.md'), join(buildConfig.componentsDir)]).pipe(
    dest(join(buildConfig.publishDir))
  );
});

// Copies files without ngcc to lib folder.
task('library:copy-libs', () => {
  return src([join(buildConfig.publishDir, '**/*')]).pipe(dest(join(buildConfig.libDir)));
});

task(
  'build:library',
  series(
    'library:mkdir-dir',
    parallel('library:scripts', 'library:compile-less', 'library:copy-resources', 'library:generate-less-vars'),
    'library:copy-libs'
  )
);
