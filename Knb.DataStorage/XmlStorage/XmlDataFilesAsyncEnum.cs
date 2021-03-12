using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;

namespace Knb.DataStorage.XmlStorage
{
    public class XmlDataFilesAsyncEnum : IAsyncEnumerable<XmlDocument>
    {
        protected XmlReader Reader { get; }
        protected Func<XmlReader> GetReaderFunc { get; }

        public XmlDataFilesAsyncEnum(Func<XmlReader> readerFunc)
        {
            var reader = readerFunc() ?? throw new ArgumentNullException();
            GetReaderFunc = readerFunc;
            Reader = reader;
        }

        public IAsyncEnumerator<XmlDocument> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new XmlDataFilesAsyncEnumerator(GetReaderFunc());
        }
    }
}
