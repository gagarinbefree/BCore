/// <binding BeforeBuild='build' />
/// <reference path="wwwroot/lib/simple-uuid/lib/uuid-node.js" />
/// <reference path="wwwroot/lib/simple-uuid/lib/uuid-node.js" />
/// <binding BeforeBuild='build' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    tsc = require("gulp-tsc"),
    concat = require('gulp-concat');

var runSequence = require('run-sequence').use(gulp);

var paths = {
    webroot: "./wwwroot/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/*.min.js";
paths.css = paths.webroot + "app/css/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";
paths.src = paths.webroot + "app/ts/**/*.ts";
paths.dest = paths.webroot + "js/";

gulp.task("clean:js", function (cb) {
    return rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    return rimraf(paths.concatCssDest, cb);
});

gulp.task("build:ts", function (cb) {
    return gulp.src(paths.src)
        .pipe(tsc())
        .pipe(gulp.dest(paths.dest));
});

gulp.task("min:js", function (cb) {
    return gulp.src([
            paths.js,
            paths.webroot + "lib/jquery/dist/jquery.js",
            paths.webroot + "lib/jquery-ui/jquery-ui.js",
            paths.webroot + "lib/bootstrap/dist/js/bootstrap.js",
            paths.webroot + "lib/autosize/dist/autosize.js",
            paths.webroot + "lib/blueimp-file-upload/js/jquery.fileupload.js",
            paths.webroot + "lib/nprogress/nprogress.js",
            paths.webroot + "lib/ev-emitter/ev-emitter.js",
            paths.webroot + "lib/imagesloaded/imagesloaded.js",
            paths.webroot + "lib/imagefill.js/js/jquery-imagefill.js",            
            "!" + paths.minJs            
        ], { base: "." })
        .pipe(concat(paths.concatJsDest))
        //.pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function (cb) {
    return gulp.src([
            paths.css,
            "!" + paths.minCss,
            paths.webroot + "lib/nprogress/nprogress.css",
            paths.webroot + "lib/bootstrap/dist/css/bootstrap-theme.css"])
        .pipe(concat(paths.concatCssDest))
        //.pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("build", function (cb) {
    return runSequence("clean:js", "clean:css",
        "build:ts",
        "min:js",
        "min:css", cb)  
});