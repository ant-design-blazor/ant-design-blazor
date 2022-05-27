var gulp = require('gulp'),
  cleanCss = require('gulp-clean-css'),
  less = require('gulp-less');

var browserify = require('browserify');
var source = require('vinyl-source-stream');
var tsify = require('tsify');
var uglify = require('gulp-uglify');
var sourcemaps = require('gulp-sourcemaps');
var buffer = require('vinyl-buffer');
var gts = require("gulp-typescript");

var tsProject = gts.createProject('tsconfig.json');

gulp.task('ts', function () {
  return browserify({
    basedir: '.',
    debug: true,
    entries: ['./main.ts'],
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
    .pipe(gulp.dest('wwwroot/js'));
});

gulp.task("tsSplit", function () {
  var tsResult = gulp.src('core/JsInterop/**/*.ts')
    .pipe(tsProject());
return tsResult.js.pipe(gulp.dest('wwwroot/js/split'))
});

gulp.task('src', function () {
  return gulp.src(['**/*.less', '!wwwroot/**']).pipe(gulp.dest('wwwroot/less'));
});

gulp.task('default', gulp.parallel('ts', 'tsSplit'), function () { });