using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using Microsoft.DocAsCode.Common.EntityMergers;

namespace ClassicDynamicsNavReference
{

    [Serializable]
    public class ApiParameter
    {
        [YamlMember(Alias = "name")]
        [JsonProperty("name")]
        [MergeOption(MergeOption.MergeKey)]
        public string Name { get; set; }

        [YamlMember(Alias = "type")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [YamlMember(Alias = "description")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [YamlMember(Alias = "byvar")]
        [JsonProperty("byvar")]
        public bool ByVar { get; set; }

        [YamlMember(Alias = "length")]
        [JsonProperty("length")]
        public string Length { get; set; }

        [YamlMember(Alias = "subtype")]
        [JsonProperty("subtype")]
        public string SubType { get; set; }

        [YamlMember(Alias = "typeuid")]
        [JsonProperty("typeuid")]
        public string TypeUid { get; set; }

        [YamlMember(Alias = "typefullname")]
        [JsonProperty("typefullname")]
        public string TypeFullName { get; set; }

    }

}
