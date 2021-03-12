using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.DataStorage.Options
{
    public class XmlDataFileSchemaInfo
    {
        public static readonly string Position = "XmlDataFileSchemaInfo";
        public string SchemaPrefix { get; set; }
        public string TargetNamespace { get; set; }
        public string XmlSchemaNamespace { get; set; }
        public string XmlSchemaInstance { get; set; }
        public string XmlSchemaFileUrl { get; set; }
    }
}
