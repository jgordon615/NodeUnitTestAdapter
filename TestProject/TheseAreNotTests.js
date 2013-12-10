"use strict";

var mockery = require("mockery");

mockery.scope = function (callback) {
    mockery.enable({ useCleanCache: true });
    callback();
    mockery.disable();
};

exports.setUp = function (callback) {
    mockery.registerAllowable("path");
    mockery.registerAllowable("os");
    callback();
};

exports.tearDown = function (callback) {
    callback();
};

exports.ShouldNotSeeThisTest = function (test) {
    test.ok(false, "This is not a test because the file extension is not .test.js");
    test.done();
};