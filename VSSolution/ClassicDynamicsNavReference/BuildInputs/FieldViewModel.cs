using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DocAsCode.Common.EntityMergers;
using Microsoft.DocAsCode.DataContracts.Common;
using CommonConstants = Microsoft.DocAsCode.DataContracts.Common.Constants;
using Microsoft.DocAsCode.YamlSerialization;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace ClassicDynamicsNavReference
{
    public class FieldViewModel : ItemViewModel
    {
            
        [YamlMember(Alias = "fieldid")]
        [JsonProperty("fieldid")]
        public string FieldId { get; set; }

        [YamlMember(Alias = "datatype")]
        [JsonProperty("datatype")]
        public string DataType { get; set; }

        [YamlMember(Alias = "description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [YamlMember(Alias = "fieldclass")]
        [JsonProperty("fieldclass")]
        public string FieldClass { get; set; }

        [YamlMember(Alias = "tablerelation")]
        [JsonProperty("tablerelation")]
        public string TableRelation { get; set; }

        [YamlMember(Alias = "formula")]
        [JsonProperty("formula")]
        public string Formula { get; set; }

    }
}
