using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DocAsCode.DataContracts.Common;
using Microsoft.DocAsCode.YamlSerialization;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace ClassicDynamicsNavReference
{

    [Serializable]
    public class PageViewModel
    {

        [YamlMember(Alias = "items")]
        [JsonProperty("items")]
        public List<ItemViewModel> Items { get; set; } = new List<ItemViewModel>();

        [YamlMember(Alias = "references")]
        [JsonProperty("references")]
        public List<ReferenceViewModel> References { get; set; } = new List<ReferenceViewModel>();

        [YamlMember(Alias = "shouldSkipMarkup")]
        [JsonProperty("shouldSkipMarkup")]
        public bool ShouldSkipMarkup { get; set; }

        [ExtensibleMember]
        [JsonExtensionData]
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

    }


}
