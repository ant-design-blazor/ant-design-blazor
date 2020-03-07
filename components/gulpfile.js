var gulp = require("gulp"),
  cleanCss = require("gulp-clean-css"),
  less = require("gulp-less");

gulp.task("default", function () {
  return gulp.src('ant-design-blazor.less')
    .pipe(less({ javascriptEnabled: true }))
    .pipe(cleanCss({ compatibility: 'ie8' }))
    .pipe(gulp.dest('wwwroot/css'));
});