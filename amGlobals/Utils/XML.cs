using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace am.Utils.Xml
{
	public class Doc : XmlDocument
	{
		public Doc(string FileName) : base()
		{ 
			Load(FileName);
		}

		public XmlNode Node(string name)
		{
			return GetElementsByTagName(name)[0];
		}
	}
}
