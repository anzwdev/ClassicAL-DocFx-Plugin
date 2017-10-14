using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DocAsCode.Common.EntityMergers;
using YamlDotNet.Serialization;
using Newtonsoft.Json;

namespace ClassicDynamicsNavReference
{

    [Serializable]
    public class LinkInfo
    {
        [YamlMember(Alias = "linkType")]
        [JsonProperty("linkType")]
        public LinkType LinkType { get; set; }

        [YamlMember(Alias = "linkId")]
        [MergeOption(MergeOption.MergeKey)]
        [JsonProperty("linkId")]
        public string LinkId { get; set; }

        [YamlMember(Alias = "altText")]
        [JsonProperty("altText")]
        public string AltText { get; set; }
    }

    [Serializable]
    public enum LinkType
    {
        CRef,
        HRef,
    }
}
