using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeUnitTestAdapter.Helpers
{
    public enum TestFileChangedReason
    {
        None,
        Added,
        Removed,
        Changed,
    }

    public class TestFileChangedEventArgs : System.EventArgs
    {
        public string File { get; private set; }
        public TestFileChangedReason ChangedReason { get; private set; }

        public TestFileChangedEventArgs(string file, TestFileChangedReason reason)
        {
            File = file;
            ChangedReason = reason;
        }
    }
}
