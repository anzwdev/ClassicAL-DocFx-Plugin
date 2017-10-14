using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    [Serializable]
    public class ItemViewModel : IOverwriteDocumentViewModel
    {

        [YamlMember(Alias = CommonConstants.PropertyName.Uid)]
        [JsonProperty(CommonConstants.PropertyName.Uid)]
        [MergeOption(MergeOption.MergeKey)]
        public string Uid { get; set; }

        [YamlMember(Alias = CommonConstants.PropertyName.Id)]
        [JsonProperty(CommonConstants.PropertyName.Id)]
        public string Id { get; set; }

        [YamlMember(Alias = CommonConstants.PropertyName.Href)]
        [JsonProperty(CommonConstants.PropertyName.Href)]
        public string Href { get; set; }

        [YamlMember(Alias = "name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [YamlMember(Alias = CommonConstants.PropertyName.Type)]
        [JsonProperty(CommonConstants.PropertyName.Type)]
        public string Type { get; set; }

        [YamlMember(Alias = "itemtype")]
        [JsonProperty("itemtype")]
        public ItemType? ItemType { get; set; }

        [YamlMember(Alias = CommonConstants.PropertyName.Source)]
        [JsonProperty(CommonConstants.PropertyName.Source)]
        public SourceDetail Source { get; set; }

        [YamlMember(Alias = CommonConstants.PropertyName.Documentation)]
        [JsonProperty(CommonConstants.PropertyName.Documentation)]
        public SourceDetail Documentation { get; set; }

        [YamlMember(Alias = "summary")]
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [YamlMember(Alias = "remarks")]
        [JsonProperty("remarks")]
        public string Remarks { get; set; }

        [YamlMember(Alias = "example")]
        [JsonProperty("example")]
        [MergeOption(MergeOption.Replace)]
        public List<string> Examples { get; set; }

        [YamlMember(Alias = "syntax")]
        [JsonProperty("syntax")]
        public SyntaxDetailViewModel Syntax { get; set; }

        [YamlMember(Alias = "seealso")]
        [JsonProperty("seealso")]
        public List<LinkInfo> SeeAlsos { get; set; }

        [YamlMember(Alias = "see")]
        [JsonProperty("see")]
        public List<LinkInfo> Sees { get; set; }

        [YamlMember(Alias = CommonConstants.PropertyName.Conceptual)]
        [JsonProperty(CommonConstants.PropertyName.Conceptual)]
        public string Conceptual { get; set; }

        //Nav object properties
        [YamlMember(Alias = "objectid")]
        [JsonProperty("objectid")]
        public int ObjectId { get; set; }

        [YamlMember(Alias = "fields")]
        [JsonProperty("fields")]
        public List<FieldViewModel> Fields { get; set; }

        [YamlMember(Alias = "procedures")]
        [JsonProperty("procedures")]
        public List<ItemViewModel> Procedures { get; set; }

        [YamlMember(Alias = "eventsubscribers")]
        [JsonProperty("eventsubscribers")]
        public List<ItemViewModel> EventSubscribers { get; set; }

        [YamlMember(Alias = "eventpublishers")]
        [JsonProperty("eventpublishers")]
        public List<ItemViewModel> EventPublishers { get; set; }

        [YamlMember(Alias = "keys")]
        [JsonProperty("keys")]
        public List<KeyViewModel> Keys { get; set; }

        [YamlMember(Alias = "items")]
        [JsonProperty("items")]
        public List<ItemViewModel> Items { get; set; }

        [YamlMember(Alias = "sourcetableno")]
        [JsonProperty("sourcetableno")]
        public string SourceTableId { get; set; }

        [YamlMember(Alias = "sourcetablename")]
        [JsonProperty("sourcetablename")]
        public string SourceTableName { get; set; }

        [YamlMember(Alias = "sourcetableuid")]
        [JsonProperty("sourcetableuid")]
        public string SourceTableUid { get; set; }

        [ExtensibleMember]
        [JsonExtensionData]
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }

}
