using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicDynamicsNavDocExporter
{
    public static class UidHelpers
    {

        public static string GetObjectTypeGroupUid(string projectId, string objectType)
        {
            return projectId + "." + objectType + "s";
        }

        public static string GetObjectUid(string projectId, string objectType, string objectId)
        {
            return GetObjectTypeGroupUid(projectId, objectType) + "." + objectId;
        }

    }
}
