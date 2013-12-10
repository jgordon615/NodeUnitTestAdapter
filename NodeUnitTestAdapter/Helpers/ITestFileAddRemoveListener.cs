using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeUnitTestAdapter.Helpers
{
    public interface ITestFileAddRemoveListener
    {
        event EventHandler<TestFileChangedEventArgs> TestFileChanged;
        void StartListeningForTestFileChanges();
        void StopListeningForTestFileChanges();
    }
}
