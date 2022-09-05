import { task, src, dest, parallel } from 'gulp';

var browserify = require('browserify');
var source = require('vinyl-source-stream');
var tsify = require('tsify');
var uglify = require('gulp-uglify');
var sourcemaps = require('gulp-sourcemaps');
var buffer = require('vinyl-buffer');
var gts = require("gulp-typescript");
import { join } from 'path';

import { buildConfig } from '../../build-config';

var tsProject = gts.createProject(join(buildConfig.componentsDir, './core/JsInterop/tsconfig.json'));

task('ts', function () {
    return browserify({
        basedir: '.',
        debug: true,
        entries: [join(buildConfig.componentsDir, './core/JsInterop/main.ts')],
        cache: {},
        packageCache: {},
    })
        .plugin(tsify)
        .transform('babelify', {
            presets: ['es2015'],
            extensions: ['.ts']
        })
        .bundle()
        .pipe(source('ant-design-blazor.js'))
        .pipe(buffer())
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(uglify())
        .pipe(sourcemaps.write('./'))
        .pipe(dest(join(buildConfig.publishDir)));
});

task("tsSplit", function () {
    return src('core/JsInterop/**/*.ts').pipe(tsProject())
        .js.pipe(dest('wwwroot/js/split'));
});

task('library:scripts', parallel('ts', 'tsSplit', 'less-src'));