Visual Studio 2012/2013 test adapter for NodeUnit.  This adapter integrates Visual Studio's test explorer with nodeunit unit tests.

This plugin looks at ALL .test.js files in your solution for nodeunit unit tests, and integrates them with the test explorer.

The following features are supported:
 * Run all tests
 * Run individual tests
 * Click test to go to source code
 * Click stack trace (in a failed test) to go to source code
 * Custom project type which includes 2 example tests

Requirements:
 * Must have node.js installed
 * Must have node.js tools for Visual Studio installed: https://nodejstools.codeplex.com
 * NodeUnit must be in an appropriate node_modules folder relative to the location of the nodeunit tests file.  
 * NodeUnit test files must have the file extension ".test.js"

Other:
 * Download the vsix plugin here: http://visualstudiogallery.msdn.microsoft.com/ff0608f4-be02-43e9-a588-abbc2a883f2b
 * OR search for "VsNodeTest" using Visual Studio's Extensions and Updates.
 * Contribute to the future of NodeUnit test adapter here: https://github.com/jgordon615/NodeUnitTestAdapter

If you like this, please leave a comment.