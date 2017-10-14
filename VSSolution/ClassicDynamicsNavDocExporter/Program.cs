using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicDynamicsNavDocExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Nav Xml Documentation exporter");
            if (args.Length != 3)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("NavXmlDocExporter NavSourceCodeTextFileName DocFXApiFolder ProjectName");
            }
            else
            {
                DocFxNavObjectsProcessor docFxProcessor = new DocFxNavObjectsProcessor();
                docFxProcessor.ExportDocumentation(args[0], args[1], args[2]);
            }

        }
    }
}
