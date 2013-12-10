using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeUnitTestAdapter.Helpers
{
    public interface ISolutionEventsListener
    {
        event EventHandler<SolutionEventsListenerEventArgs> SolutionProjectChanged;

        void StartListeningForChanges();
        void StopListeningForChanges();
        event EventHandler SolutionUnloaded;
    }
}
