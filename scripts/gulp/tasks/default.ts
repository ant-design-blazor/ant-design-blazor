/**
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://github.com/ant-design-blazor/ant-design-blazor/blob/master/LICENSE
 */

import { yellow } from 'chalk';
import { parallel, task } from 'gulp';

task('help', done => {
  console.log();
  console.log('Please specify a gulp task you want to run.');
  console.log(yellow('start:dev    '), 'Start development.');
  console.log(yellow('build:library'), 'Build ant-design-blazor-lib to publish/ directory.');
  console.log(yellow('build:preview'), 'Build preview site to dist/ directory.');
  console.log(
    yellow('build:release'),
    'Build releaseable library to publish/ directory and deployable site to dist/ directory.'
  );
  console.log();
  done();
});

task('default', parallel('help'));
