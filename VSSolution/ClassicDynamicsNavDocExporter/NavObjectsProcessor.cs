using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Dynamics.Nav.MetaModel;
using Microsoft.Dynamics.Nav.MetaMetaModel;
using Microsoft.Dynamics.Nav.Model.IO.Txt;

namespace ClassicDynamicsNavDocExporter
{
    public class NavObjectsProcessor
    {

        public List<ApplicationObject> AppObjects { get; private set; }

        public NavObjectsProcessor()
        {
        }

        public void LoadFromTextFile(string fileName)
        {
            Stream sourceStream = new FileStream(fileName, FileMode.Open);
            TxtImportOptions txtImportOptions = new TxtImportOptions();
            TxtImporter txtImporter = new TxtImporter(TxtFileModelInfo.Instance, txtImportOptions);
            this.AppObjects = txtImporter.ImportFromStream(sourceStream);
            sourceStream.Close();
            sourceStream.Dispose();
        }

        protected void GetXmlDocumentation(StringBuilder xmlDocBuilder, IElement procedureElement)
        {
            //analyze lines
            List<string> lines = new List<string>();
            lines.AddRange(procedureElement.GetCodeLines());

            for (int i = 0; i < lines.Count; i++)
            {
                string text = lines[i].Trim();
                if (text.StartsWith("///"))
                {
                    xmlDocBuilder.AppendLine(text.Substring(3).Trim());
                }
                else if (!String.IsNullOrWhiteSpace(text))
                    return;
            }
        }

    }
}
