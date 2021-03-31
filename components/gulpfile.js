var gulp = require('gulp'),
  cleanCss = require('gulp-clean-css'),
  less = require('gulp-less');

var browserify = require('browserify');
var source = require('vinyl-source-stream');
var tsify = require('tsify');
var uglify = require('gulp-uglify');
var sourcemaps = require('gulp-sourcemaps');
var buffer = require('vinyl-buffer');

gulp.task('less-default', function () {
  return gulp
    .src('ant-design-blazor.less')
    .pipe(less({ javascriptEnabled: true }))
    .pipe(cleanCss({ compatibility: 'ie8' }))
    .pipe(gulp.dest('wwwroot/css'));
});

gulp.task('less-aliyun', function () {
  return gulp
    .src('ant-design-blazor.aliyun.less')
    .pipe(less({ javascriptEnabled: true }))
    .pipe(cleanCss({ compatibility: 'ie8' }))
    .pipe(gulp.dest('wwwroot/css'));
});

gulp.task('less-compact', function () {
  return gulp
    .src('ant-design-blazor.compact.less')
    .pipe(less({ javascriptEnabled: true }))
    .pipe(cleanCss({ compatibility: 'ie8' }))
    .pipe(gulp.dest('wwwroot/css'));
});

gulp.task('less-dark', function () {
  return gulp
    .src('ant-design-blazor.dark.less')
    .pipe(less({ javascriptEnabled: true }))
    .pipe(cleanCss({ compatibility: 'ie8' }))
    .pipe(gulp.dest('wwwroot/css'));
});

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

gulp.task('src', function () {
  return gulp.src(['**/*.less', '!wwwroot/**']).pipe(gulp.dest('wwwroot/less'));
});

gulp.task('default', gulp.parallel('less-default', 'less-aliyun', 'less-compact', 'less-dark', 'ts', 'src'), function () { });