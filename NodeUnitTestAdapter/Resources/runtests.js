"use strict";

function runTests(testFile, testName) {
    var nodeunit = require("nodeunit"),
        path = require("path");

    var paths = [path.join(process.cwd(), testFile)];
    var results = {};

    nodeunit.runFiles(paths, {
        testFullSpec: testName,
        testStart: function (name) {
            results[name] = {
                testName: name[0],
                startTime: process.hrtime()
            };
        },
        testDone: function (name, assertions) {
            var r = results[name];
            var diff = process.hrtime(r.startTime);
            r.duration = (diff[0] * 1e9 + diff[1]) / 1000000000; // convert nanoseconds to seconds
            delete r.startTime;

            r.passed = assertions.failures() === 0;
            r.assertions = [];

            assertions.forEach(function (e) {
                var extra = {};
                if (e.error) {
                    var err = {
                        name: e.error.name,
                        message: e.error.message,
                        stack: e.error.stack
                    };
                    r.assertions.push(err);
                }
            });

            //console.log(JSON.stringify(r, null, 4));
            console.log(JSON.stringify(r));
        },
        done: function (assertions) {
        }
    });
}

if (process.argv.length > 2) {
    var fileName = process.argv[2],
        testName = process.argv.length > 3 ? process.argv[3] : null;
    runTests(fileName, testName);
}