using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Knb.Shared
{
    public static class DefaultCancellationToken
    {
        public static CancellationToken Default { get; }
        static DefaultCancellationToken()
        {
            var cts = new CancellationTokenSource();
            Default = cts.Token;
        }
    }
}
