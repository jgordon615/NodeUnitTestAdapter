Visual Studio 2012 test adapter for NodeUnit.  This adapter integrates Visual Studio 2012's test explorer with nodeunit unit tests.

This plugin looks at ALL .test.js files in your solution for nodeunit unit tests, and integrates them with the test explorer.

The following features are supported:
 * Run all tests
 * Run individual tests
 * Click test to go to source code
 * Click stack trace (in a failed test) to go to source code

Requirements:
 * MUST HAVE NODE.JS installed
 * NodeUnit must be in an appropriate node_modules folder relative to the location of the nodeunit tests file.  
 * NodeUnit test files must have the file extension ".test.js"

Installation:
 * Download the vsix plugin here: http://visualstudiogallery.msdn.microsoft.com/ff0608f4-be02-43e9-a588-abbc2a883f2b
 * OR search for "VsNodeTest" using Visual Studio's Extensions and Updates.

Todo:
 * Create a custom project type to house nodeunit unit tests. (Complete with custom templates for nodeunit test files)

If you like this, please leave a comment.