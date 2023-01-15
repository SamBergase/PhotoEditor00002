using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoEditor00002
{
    public class ExtractXmp
    {
        public XmlDocument getXmpData(byte[] imageBytes)
        {
            var asString = Encoding.UTF8.GetString(imageBytes);
            var start = asString.IndexOf("<x:xmpmeta");
            var end = asString.IndexOf("/x:xmpmeta>") + 12;
            if (start == -1 || end == -1)
                return null;
            var justTheMeta = asString.Substring(start, end - start);
            var returnVal = new XmlDocument();
            returnVal.LoadXml(justTheMeta);
            return returnVal;
        }
    }
}
