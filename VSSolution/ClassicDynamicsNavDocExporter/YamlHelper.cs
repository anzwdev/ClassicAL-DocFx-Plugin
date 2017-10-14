using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicDynamicsNavDocExporter
{
    public static class YamlHelper
    {

        public static void ExportDocument(string fileName, string mimeType, object data)
        {
            StreamWriter writer = new StreamWriter(fileName);
            writer.WriteLine("### YamlMime:" + mimeType);
            YamlDotNet.Serialization.SerializerBuilder builder = new YamlDotNet.Serialization.SerializerBuilder();
            YamlDotNet.Serialization.Serializer s = builder.Build();
            s.Serialize(writer, data);
            writer.Close();
            writer.Dispose();
        }

    }
}
