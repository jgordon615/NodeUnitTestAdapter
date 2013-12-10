using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeUnitTestAdapter.Helpers
{
    public interface ITestFilesUpdateWatcher
    {
        event EventHandler<TestFileChangedEventArgs> FileChangedEvent;
        void AddWatch(string path);
        void RemoveWatch(string path);
    }
}
