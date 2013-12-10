using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeUnitTestAdapter
{
    public class NodeUnitTestResult
    {
        public string TestName { get; set; }
        public bool Passed { get; set; }
        public float Duration { get; set; }
        public NodeUnitTestAssertion[] Assertions { get; set; }
    }
}
