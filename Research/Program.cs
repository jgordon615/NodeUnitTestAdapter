using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NodeUnitTestAdapter;

namespace Research
{
    class ConsoleMessageLogger : IMessageLogger
    {
        public void SendMessage(TestMessageLevel testMessageLevel, string message)
        {
            Console.WriteLine(testMessageLevel.ToString() + ": " + message);
        }
    }

    class Program
    {
        static void Main(string[] commandArgs)
        {
            string filename = @"C:\Projects\NodeUnitTestAdapter\TestProject\settings.test.js";

            if (!File.Exists(filename))
                filename = @"C:\Projects\EnterpriseAPI\Main\NodeUnitTestAdapter\TestProject\settings.test.js";

            if (!File.Exists(filename))
                throw new FileNotFoundException("Could not find settings.test.js; bailing.");

            var logger = new ConsoleMessageLogger();

            logger.SendMessage(TestMessageLevel.Informational, "Discovering tests...");
            var tests = NodeUnitTestDiscoverer.DiscoverTests(filename, logger);

            foreach (var td in tests)
            {
                var desc = string.IsNullOrEmpty(td.Description) ? "(no description)" : td.Description;
                logger.SendMessage(TestMessageLevel.Informational, "Found " + td.Name + ": " + desc);
            }

            logger.SendMessage(TestMessageLevel.Informational, "Done.");
            logger.SendMessage(TestMessageLevel.Informational, "Press ENTER to quit.");
            Console.ReadLine();
        }

       

    }
}
