var gulp = require("gulp"),
    cleanCss = require("gulp-clean-css"),
    less = require("gulp-less");

var rename = require('gulp-rename');

gulp.task("default",
    function() {
        return gulp.src('style/index.less')
            .pipe(less({ javascriptEnabled: true }))
            .pipe(cleanCss({ compatibility: 'ie8' }))
            .pipe(rename('docs.css'))
            .pipe(gulp.dest('wwwroot/css'));
    });