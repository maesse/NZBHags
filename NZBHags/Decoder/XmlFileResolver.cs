using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using System.IO;

namespace NZBHags
{

    class XmlFileResolver : XmlResolver
    {
        private System.Net.ICredentials creds = System.Net.CredentialCache.DefaultCredentials;
        private XmlUrlResolver resolver = new XmlUrlResolver();

        public override System.Net.ICredentials Credentials { set{creds = value;} }

        public override object GetEntity(Uri uri, string str, Type type) {

            //Logging.Log("Redirecting DTD query.. " + uri.OriginalString);
            if (uri.AbsolutePath.Contains("NZB"))
            {
                Logging.Instance.Log("Redirecting DTD query.. ");
                return File.Open("lib\\nzb-1.0.dtd", FileMode.Open);
                
            }
            return resolver.GetEntity(uri, str, type);

        }

        
    }
}
