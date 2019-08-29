using System;
using System.Collections.Generic;
using System.Text;

namespace Polkadot.Source
{
    public interface IApplication : IDisposable
    {
        int Connect(string node_url = "");
        void Disconnect();
        SystemInfo GetSystemInfo();
    }
}
