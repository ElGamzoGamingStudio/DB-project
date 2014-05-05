using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PathToSuccess.BL
{
    public static class Settings
    {
        private static string path = "settings.xml";
        //add any properties you need, declarations and xelements to methods below
       

        public static void Save()
        {
            var document = new XmlDocument();
            document.Load(path);
            document.RemoveAll();
            
            //add to doc somehow?
            
            document.Save(path);
        }

        public static void Load()
        {
            var document = new XmlDocument();
            document.Load(path);
            
            //this.prop = document.GetElementById("id").Value
        }
    }
}
