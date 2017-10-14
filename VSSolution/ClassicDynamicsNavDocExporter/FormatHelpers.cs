using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicDynamicsNavDocExporter
{
    public static class FormatHelpers
    {

        public static string FormatName(string name)
        {
            if ((String.IsNullOrWhiteSpace(name)) || (!NeedsFormat(name)))
                return name;
            return "\"" + name.Replace("\"", "\"\"") + "\"";
        }

        public static bool NeedsFormat(string name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                char nameChar = name[i];
                bool validChar = ((nameChar >= 'A') && (nameChar <= 'Z')) ||
                    ((nameChar >= 'a') && (nameChar <= 'z')) ||
                    ((nameChar >= '0') && (nameChar <= '9')) ||
                    (nameChar == '|');
                if (!validChar)
                    return true;
            }
            return false;
        }

    }
}
