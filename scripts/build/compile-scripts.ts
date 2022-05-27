import { join } from 'path';
import { buildConfig } from '../build-config';

const browserify = require('browserify');
const tsify = require('tsify');
const source = require('vinyl-source-stream');
const buffer = require('vinyl-buffer');
const sourcemaps = require('gulp-sourcemaps');
const uglify = require('gulp-uglify');


export function compileScripts() {
  return browserify({
        basedir: '.',
        debug: true,
        entries: [join(buildConfig.componentsDir, './main.ts')],
        cache: {},
        packageCache: {},
      })
      .plugin(tsify)
      .transform('babelify', {
        presets: ['es2015'],
        extensions: ['.ts'],
      })
      .bundle()
      .pipe(source('ant-design-blazor.js'))
      .pipe(buffer())
      .pipe(sourcemaps.init({ loadMaps: true }))
      .pipe(uglify())
      .pipe(sourcemaps.write('./'));
}
  