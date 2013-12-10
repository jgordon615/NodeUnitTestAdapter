"use strict";

var fs = require("fs");

function disco(filename) {
    var n;
    var module = require(filename);

    var fileStr = fs.readFileSync(filename).toString();

    for (n in module) {
        switch (n) {
            case "setUp":
            case "tearDown":
                break;
            default:
                // Tests must be in the format:
                // exports.testName = function (test) { ... }
                var funStr = module[n].toString();
                var position = fileStr.indexOf(funStr);
                var line = fileStr.substring(0, position).match(/\n/g).length + 1;
                var params = funStr.slice(funStr.indexOf('(') + 1, funStr.indexOf(')')).match(/([^\s,]+)/g);
                var isATest = (params && params.length === 1 && params[0] === "test" && funStr.indexOf(".done()") > -1);

                if (isATest) {
                    var testDescriptor = null;

                    var hasMeta = !!module[n].meta;

                    if (hasMeta) {
                        testDescriptor = module[n].meta;
                        testDescriptor.name = n;
                        testDescriptor.line = line;
                    } else {
                        testDescriptor = {
                            name: null,
                            line: line,
                            description: null,
                            traits: []
                        };
                        testDescriptor.name = n;
                    }

                    console.log(JSON.stringify(testDescriptor));
                }

                break;
        }
    }
}

if (process.argv.length > 2) {
    disco(process.argv[2]);
}