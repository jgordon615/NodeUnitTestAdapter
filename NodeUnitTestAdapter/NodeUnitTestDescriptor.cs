using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeUnitTestAdapter
{
    public class NodeUnitTestDescriptor
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Traits { get; set; }
        public int? Line { get; set; }
    }
}
