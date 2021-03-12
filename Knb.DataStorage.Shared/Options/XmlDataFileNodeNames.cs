using System;
using System.Collections.Generic;
using System.Text;

namespace Knb.DataStorage.Options
{
    public class XmlDataFileNodeNames
    {
        public static readonly string Position = "XmlDataFileNodeNames";
        public string Root { get; set; }
        public string PlayData { get; set; }
        public string RoundData { get; set; }
        public string TurnData { get; set; }
        public string FacedUpCards { get; set; }
        public string Hand { get; set; }
        public string PrevTurns { get; set; }
        public string ActiveCards { get; set; }
        public string InactiveCards { get; set; }
        public string PlayerIdAttr { get; set; }
    }
}
