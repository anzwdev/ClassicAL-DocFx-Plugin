using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace ClassicDynamicsNavReference
{

    [Serializable]
    public class SyntaxDetailViewModel
    {
        [YamlMember(Alias = "content")]
        [JsonProperty("content")]
        public string Content { get; set; }

        [YamlMember(Alias = "parameters")]
        [JsonProperty("parameters")]
        public List<ApiParameter> Parameters { get; set; }

        [YamlMember(Alias = "return")]
        [JsonProperty("return")]
        public ApiParameter Return { get; set; }

        [YamlMember(Alias = "visibility")]
        [JsonProperty("visibility")]
        public Visibility? Visibility { get; set; }

        [YamlMember(Alias = "proceduretype")]
        [JsonProperty("proceduretype")]
        public ProcedureType? ProcedureType { get; set; }

        [YamlMember(Alias = "eventpublishertype")]
        [JsonProperty("eventpublishertype")]
        public EventPublisherType? EventPublisherType { get; set; }

    }

}
