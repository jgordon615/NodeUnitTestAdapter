﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NodeUnitTestAdapter {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NodeUnitTestAdapter.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;use strict&quot;;
        ///
        ///var fs = require(&quot;fs&quot;);
        ///
        ///function disco(filename) {
        ///    var n;
        ///    var module = require(filename);
        ///
        ///    var fileStr = fs.readFileSync(filename).toString();
        ///
        ///    for (n in module) {
        ///        switch (n) {
        ///            case &quot;setUp&quot;:
        ///            case &quot;tearDown&quot;:
        ///                break;
        ///            default:
        ///                // Tests must be in the format:
        ///                // exports.testName = function (test) { ... }
        ///                var funStr = module[n].toString();
        ///                 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Disco {
            get {
                return ResourceManager.GetString("Disco", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;use strict&quot;;
        ///
        ///function runTests(testFile, testName) {
        ///    var nodeunit = require(&quot;nodeunit&quot;),
        ///        path = require(&quot;path&quot;);
        ///
        ///    var paths = [path.join(process.cwd(), testFile)];
        ///    var results = {};
        ///
        ///    nodeunit.runFiles(paths, {
        ///        testFullSpec: testName,
        ///        testStart: function (name) {
        ///            results[name] = {
        ///                testName: name[0],
        ///                startTime: process.hrtime()
        ///            };
        ///        },
        ///        testDone: function (name, assertions) {
        ///        [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string RunTests {
            get {
                return ResourceManager.GetString("RunTests", resourceCulture);
            }
        }
    }
}