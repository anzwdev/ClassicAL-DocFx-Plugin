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
    public class KeyViewModel
    {

        [YamlMember(Alias = "primarykey")]
        [JsonProperty("primarykey")]
        public bool PrimaryKey { get; set; }

        [YamlMember(Alias = "fields")]
        [JsonProperty("fields")]
        public string Fields { get; set; }

        [YamlMember(Alias = "description")]
        [JsonProperty("description")]
        public string Description { get; set; }


    }
}
