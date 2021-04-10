var gulp = require("gulp"),
  cleanCss = require("gulp-clean-css"),
  less = require("gulp-less");

var rename = require('gulp-rename');

gulp.task("less-default", function () {
  return gulp.src('docs.less')
    .pipe(less({ javascriptEnabled: true }))
    .pipe(cleanCss({ compatibility: 'ie8' }))
    .pipe(rename('docs.default.css'))
    .pipe(gulp.dest('wwwroot/css'));
});

gulp.task("less-aliyun", function () {
  return gulp.src('docs.aliyun.less')
    .pipe(less({ javascriptEnabled: true }))
    .pipe(cleanCss({ compatibility: 'ie8' }))
    .pipe(gulp.dest('wwwroot/css'));
});

gulp.task("less-compact", function () {
  return gulp.src('docs.compact.less')
    .pipe(less({ javascriptEnabled: true }))
    .pipe(cleanCss({ compatibility: 'ie8' }))
    .pipe(gulp.dest('wwwroot/css'));
});

gulp.task("less-dark", function () {
  return gulp.src('docs.dark.less')
    .pipe(less({ javascriptEnabled: true }))
    .pipe(cleanCss({ compatibility: 'ie8' }))
    .pipe(gulp.dest('wwwroot/css'));
});

gulp.task('default', gulp.parallel('less-default', 'less-aliyun', 'less-compact', 'less-dark'), function () { });