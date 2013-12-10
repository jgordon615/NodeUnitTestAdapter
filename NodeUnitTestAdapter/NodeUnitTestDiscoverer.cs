using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Newtonsoft.Json;
using NodeUnitTestAdapter.Helpers;

namespace NodeUnitTestAdapter
{
    [DefaultExecutorUri(NodeUnitTestExecutor.ExecutorUriString)]
    [FileExtension(".js")]
    public class NodeUnitTestDiscoverer : ITestDiscoverer
    {
        private IMessageLogger _logger;

        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            _logger = logger;

            _logger.SendMessage(TestMessageLevel.Informational, ">>> DiscoverTests");
            GetTests(sources, discoverySink);
        }

        public static IEnumerable<NodeUnitTestDescriptor> DiscoverTests(string filename, IMessageLogger logger)
        {
            List<NodeUnitTestDescriptor> foundTests = new List<NodeUnitTestDescriptor>();
            StringBuilder sbException = new StringBuilder();
            Process proc = new Process();

            string nodeFullPath = NodeJsHelper.LocateNodeJs();

            proc.StartInfo.FileName = nodeFullPath;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;

            proc.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    try
                    {
                        var test = JsonConvert.DeserializeObject<NodeUnitTestDescriptor>(args.Data);
                        foundTests.Add(test);
                    }
                    catch(JsonReaderException)
                    {
                        logger.SendMessage(TestMessageLevel.Informational, args.Data);
                    }
                }
            };

            proc.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    sbException.AppendLine(args.Data);
                }
            };

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            proc.StandardInput.Write(Resources.Disco);
            proc.StandardInput.Write("disco(\"" + filename.Replace("\\", "\\\\") + "\");");
            proc.StandardInput.Close();

            proc.WaitForExit();

            if (sbException.Length > 0)
            {
                throw new Exception(sbException.ToString());
            }

            return foundTests;
        }

        private void GetTests(IEnumerable<string> sources, ITestCaseDiscoverySink discoverySink)
        {
            foreach (string source in sources)
            {
                if (!source.EndsWith(".test.js"))
                    continue;

                _logger.SendMessage(TestMessageLevel.Informational, "Discovering tests in " + source);

                var tests = DiscoverTests(source, _logger);
                foreach (var td in tests)
                {
                    _logger.SendMessage(TestMessageLevel.Informational, "Found test: " + td.Name);

                    TestCase test = new TestCase(td.Name, NodeUnitTestExecutor.ExecutorUri, source)
                    {
                        CodeFilePath = source
                    };

                    test.Traits.Add(new Trait("", "NodeUnit"));
                    if (td.Line.HasValue)
                        test.LineNumber = td.Line.Value;

                    foreach (var trait in td.Traits)
                    {
                        test.Traits.Add(new Trait("", trait));
                    }


                    if (discoverySink != null)
                    {
                        discoverySink.SendTestCase(test);
                    }
                }

                _logger.SendMessage(TestMessageLevel.Informational, "Done discovering tests in " + source);
            }
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _logger.SendMessage(TestMessageLevel.Informational, e.Data);
        }
    }
}
