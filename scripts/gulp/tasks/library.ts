/**
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://github.com/NG-ZORRO/ant-design-blazor/blob/master/LICENSE
 */

import { dest, series, src, task } from 'gulp';
import * as fs from 'fs-extra';
import { join } from 'path';

import { buildConfig } from '../../build-config';
import { compile as compileLess } from '../../build/compile-styles';
import { generateLessVars } from '../../build/generate-less-vars';
import { copyStylesToSrc } from '../../build/migration-styles';

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

task('library:copy-less', () => {
  fs.mkdirsSync(buildConfig.publishDir);
  return src([join(buildConfig.componentsDir, '**/*.less')]).pipe(dest(join(buildConfig.publishDir)));
});

task(
  'build:library',
  series(
    // 'library:build-zorro',
    'library:copy-less',
    'library:compile-less',
    'library:generate-less-vars',
    // 'library:copy-libs'
    // parallel('library:compile-less', 'library:copy-resources', 'library:generate-less-vars'),
  )
);
