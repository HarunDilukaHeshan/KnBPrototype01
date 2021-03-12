using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Knb.DataStorage.XmlStorage
{
    public class XmlDataFilesAsyncEnumerator : IAsyncEnumerator<XmlDocument>
    {
        protected string _xmlString = "";
        public XmlDocument Current
        {
            get
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(_xmlString);
                return xmlDoc;
            }
        }
        protected XmlReader Reader { get; }

        public XmlDataFilesAsyncEnumerator(XmlReader reader)
        {
            Reader = reader;
        }

        public async ValueTask DisposeAsync()
        {
            await Task.Run(() => { _xmlString = ""; });
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            var hasFound = false;
            while (await Reader.ReadAsync())
            {
                switch (Reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (Reader.LocalName == "play-data")
                        {
                            hasFound = true;
                            _xmlString = await Reader.ReadOuterXmlAsync();
                        }
                        break;
                    default:
                        break;
                }

                if (hasFound) break;
            }

            return !Reader.EOF;
        }
    }
}
