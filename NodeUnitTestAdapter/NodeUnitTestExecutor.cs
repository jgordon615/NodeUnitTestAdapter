using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Newtonsoft.Json;
using NodeUnitTestAdapter.Helpers;

namespace NodeUnitTestAdapter
{
    [ExtensionUri(NodeUnitTestExecutor.ExecutorUriString)]
    public class NodeUnitTestExecutor : ITestExecutor
    {
        public const string ExecutorUriString = "executor://nodeunittestadapter/v1";
        public static readonly Uri ExecutorUri = new Uri(ExecutorUriString);
        private bool _cancelled = false;

        public void Cancel()
        {
            _cancelled = true;
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            _cancelled = false;

            foreach (string fileName in sources)
            {
                if (_cancelled)
                    break;

                try
                {
                    RunFileOrTest(frameworkHandle, runContext, fileName);

                    frameworkHandle.SendMessage(TestMessageLevel.Informational, "Process Done.");
                }
                catch (Exception ex)
                {
                    frameworkHandle.SendMessage(TestMessageLevel.Error, "Exception spawning nodeunit.cmd: " + ex.ToString());
                }
            }
        }

        private static void GenericFailTest(IFrameworkHandle frameworkHandle, string fileName, string testName, string message = null)
        {
            var testCase = new TestCase(testName, NodeUnitTestExecutor.ExecutorUri, fileName) { DisplayName = testName };
            var testResult = new TestResult(testCase) { DisplayName = testName };
            testResult.Outcome = TestOutcome.Failed;
            testResult.ErrorMessage = message;

            frameworkHandle.SendMessage(TestMessageLevel.Informational, "Recording Result for " + testCase.DisplayName + " (" + testResult.Outcome.ToString() + ")");
            frameworkHandle.RecordResult(testResult);
        }

        private static void RunFileOrTest(IFrameworkHandle frameworkHandle, IRunContext runContext, string fileName, string testName = null)
        {
            frameworkHandle.SendMessage(TestMessageLevel.Informational, "runContext.SolutionDirectory: " + runContext.SolutionDirectory);
            frameworkHandle.SendMessage(TestMessageLevel.Informational, "runContext.TestRunDirectory: " + runContext.TestRunDirectory);
            frameworkHandle.SendMessage(TestMessageLevel.Informational, "source: " + fileName);

            string nodeFullPath = NodeJsHelper.LocateNodeJs();

            Process proc = new Process();

            proc.StartInfo.FileName = nodeFullPath;
            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(fileName);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;

            proc.OutputDataReceived += (sender, args) =>
            {
                var data = args.Data;

                if (!string.IsNullOrEmpty(data))
                {
                    frameworkHandle.SendMessage(TestMessageLevel.Informational, "> " + data);

                    if (data.Contains("Error: Cannot find module 'nodeunit'"))
                    {
                        if (!string.IsNullOrEmpty(testName))
                        {
                            GenericFailTest(frameworkHandle, fileName, testName, data);
                        }
                    }
                    else
                    {
                        try
                        {
                            var result = JsonConvert.DeserializeObject<NodeUnitTestResult>(data);

                            if (result != null && !string.IsNullOrEmpty(result.TestName))
                            {
                                var testCase = new TestCase(result.TestName, NodeUnitTestExecutor.ExecutorUri, fileName) { DisplayName = result.TestName };
                                var testResult = new TestResult(testCase) { DisplayName = result.TestName };
                                testResult.Duration = TimeSpan.FromSeconds(Math.Max(.001, result.Duration));
                                testResult.Outcome = result.Passed ? TestOutcome.Passed : TestOutcome.Failed;

                                if (result.Assertions.Length > 0)
                                {
                                    var first = result.Assertions.First();
                                    testResult.ErrorStackTrace = FormatStackTrace(first.Stack);
                                    testResult.ErrorMessage = first.Message;
                                }

                                frameworkHandle.SendMessage(TestMessageLevel.Informational, "Recording Result for " + testCase.DisplayName + " (" + testResult.Outcome.ToString() + ")");
                                frameworkHandle.RecordResult(testResult);
                            }
                        }
                        catch (Newtonsoft.Json.JsonException)
                        {
                            //frameworkHandle.SendMessage(TestMessageLevel.Informational, data);
                        }
                    }
                }
            };

            proc.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    frameworkHandle.SendMessage(TestMessageLevel.Warning, "^ " + args.Data);

                    if (args.Data.Contains("Error: Cannot find module 'nodeunit'"))
                    {
                        if (!string.IsNullOrEmpty(testName))
                        {
                            GenericFailTest(frameworkHandle, fileName, testName, args.Data);
                        }
                    }
                }
            };

            frameworkHandle.SendMessage(TestMessageLevel.Informational, "Process FileName: " + proc.StartInfo.FileName);
            frameworkHandle.SendMessage(TestMessageLevel.Informational, "Process Arguments: " + proc.StartInfo.Arguments);
            frameworkHandle.SendMessage(TestMessageLevel.Informational, "Process WorkingDirectory: " + proc.StartInfo.WorkingDirectory);

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            proc.StandardInput.Write(Resources.RunTests);

            string testFile = Path.GetFileName(fileName).Replace("\\", "\\\\");
            string jsCommand = "runTests(\"" + testFile + "\"";
            if (!string.IsNullOrEmpty(testName))
                jsCommand += ", \"" + testName + "\"";
            jsCommand += ");";
            frameworkHandle.SendMessage(TestMessageLevel.Informational, "Process Emitting Command: " + jsCommand);
            proc.StandardInput.Write(jsCommand);
            proc.StandardInput.Close();

            proc.WaitForExit();
        }

        private static string FormatStackTrace(string jsStack)
        {
            string dotnetStack = jsStack;

            var lines = jsStack
                .Split('\n')
                .Where(x => !x.Contains(@"\nodeunit\"))
                .Select(x =>
            {
                if (x.Contains("    at "))
                {
                    if (x.Contains("(") && x.Contains(")"))
                    {
                        // Filename will be between the parenthesis in this format:
                        // (filename:line number:column number)
                        var re = new Regex(@"\s{4}at\s{1}(\S+)\s{1}\((.+)\:([0-9]+)\:([0-9]+)\)");
                        var match = re.Match(x);

                        if (match.Success)
                        {
                            string methodName = match.Groups[1].Value;
                            string fileName = match.Groups[2].Value;
                            string lineNumber = match.Groups[3].Value;

                            return "   at " + methodName + " in " + fileName + ":line " + lineNumber;
                        }
                        else
                        {
                            return x;
                        }
                    }
                    else
                    {
                        // Filename will be after the "at" in this format.  (There will be no method name)
                        // at filename:line number:column number
                        var re = new Regex(@"\s{4}at\s{1}(.+)\:([0-9]+)\:([0-9]+)");
                        var match = re.Match(x);

                        if (match.Success)
                        {
                            string fileName = match.Groups[1].Value;
                            string lineNumber = match.Groups[2].Value;

                            return "   at anonymous function in " + fileName + ":line " + lineNumber;
                        }
                        else
                        {
                            return x;
                        }
                    }
                }
                else
                {
                    return x; // Keep Original Formatting
                }
            }).ToArray();

            return string.Join("\r\n", lines);
        }

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            _cancelled = false;

            foreach (TestCase test in tests)
            {
                if (_cancelled)
                    break;

                var testResult = new TestResult(test);

                string fileName = test.Source;
                string testName = test.DisplayName;

                RunFileOrTest(frameworkHandle, runContext, fileName, testName);

                testResult.Outcome = TestOutcome.Passed;

                frameworkHandle.RecordResult(testResult);
            }
        }
    }
}
