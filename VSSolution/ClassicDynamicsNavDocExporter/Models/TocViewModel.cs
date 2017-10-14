using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace ClassicDynamicsNavDocExporter.Models
{
    public class TocViewModel
    {

        [YamlMember(Alias = "uid")]
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [YamlMember(Alias = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [YamlMember(Alias = "items")]
        [JsonProperty("items")]
        public List<TocViewModel> Items { get; set; }

    }
}
