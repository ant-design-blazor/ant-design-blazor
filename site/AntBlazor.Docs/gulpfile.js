var gulp = require("gulp"),
  cleanCss = require("gulp-clean-css"),
  less = require("gulp-less");

gulp.task("default", function () {
  return gulp.src('docs.less')
    .pipe(less({ javascriptEnabled: true }))
    .pipe(cleanCss({ compatibility: 'ie8' }))
    .pipe(gulp.dest('wwwroot/css'));
});