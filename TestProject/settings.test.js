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

exports.alwaysPass = function (test) {
    test.ok(true, "This should always pass.");
    test.done();
};
exports.alwaysPass.meta = { traits: ["Sample Test", "Sample Trait", "Randolpho"], description: "This test should always pass." };

exports.alwaysFail = function AlwaysFail(test) {
    (function () {
        test.ok(false, "This should always fail.");
        test.done();
    }());
};
exports.alwaysFail.meta = { traits: ["Sample Test", "Sample Trait"], description: "This test should always fail." };


exports.testDefaultSetting = function (test) {
    var mockFs = {
        existsSync: function (file) {
            return !!file.match(/default\.json/);
        },
        readFileSync: function (file) {
            if (file.match(/default\.json/)) {
                return JSON.stringify({
                    apiPort: 80
                });
            } else {
                return null;
            }
        }
    };
    mockery.registerAllowable("C:\\Projects\\EnterpriseAPI\\Main\\EnterpriseAPI\\settings");
    mockery.registerMock("fs", mockFs);

    mockery.scope(function () {
        var setting = require("C:\\Projects\\EnterpriseAPI\\Main\\EnterpriseAPI\\settings");
        test.ok(setting.apiPort === 80, "Port was 80");
    });

    mockery.deregisterMock("fs");
    test.done();
};

exports.testOverridenSetting = function (test) {
    var mockFs = {
        existsSync: function (file) {
            return true;
        },
        readFileSync: function (file) {
            if (file.match(/default\.json/)) {
                return JSON.stringify({
                    apiPort: 80
                });
            } else {
                return JSON.stringify({
                    apiPort: 1234
                });
            }
        }
    };
    mockery.registerAllowable("C:\\Projects\\EnterpriseAPI\\Main\\EnterpriseAPI\\settings");
    mockery.registerMock("fs", mockFs);

    mockery.scope(function () {
        var setting = require("C:\\Projects\\EnterpriseAPI\\Main\\EnterpriseAPI\\settings");
        test.ok(setting.apiPort === 1234, "Port was 1234");
    });

    mockery.deregisterMock("fs");
    test.done();
};

exports.michael123 = function (test) {
    test.ok(false, "This does not qualify as a test because it does not call the 'done' method on the 'test' object.");
};