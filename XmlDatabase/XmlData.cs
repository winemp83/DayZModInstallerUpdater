using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using Model;

namespace XmlDatabase
{
    public class XmlData : IXmlData
    {
        private XmlDocument _Xml;
        private FileStream _File;
        private string _FilePath;

        public XmlData()
        {
            _FilePath = ConfigurationManager.AppSettings["XmlPfad"] + @"\" + ConfigurationManager.AppSettings["XmlName"];
            if (!File.Exists(_FilePath))
                CreateFile();
        }

        public void Add(Mod mod)
        {
            _Xml = new XmlDocument();
            _File = new FileStream(_FilePath, FileMode.Open);
            _Xml.Load(_File);
            XmlElement cl = _Xml.CreateElement("Mod");
            cl.SetAttribute("ID", (Count() + 1).ToString());
            cl.SetAttribute("ModID", mod.ModID);
            cl.SetAttribute("ModName", mod.ModName); ;
            _Xml.DocumentElement.AppendChild(cl);
            _File.Close();
            _Xml.Save(_FilePath);
        }

        public BindingList<Mod> Get(string ID = null)
        {
            _Xml = new XmlDocument();
            _File = new FileStream(_FilePath, FileMode.Open);
            _Xml.Load(_File);
            BindingList<Mod> result = new BindingList<Mod>();
            XmlNodeList list = _Xml.GetElementsByTagName("Mod");
            for (int i = 0; i < list.Count; i++)
            {
                XmlElement cl = (XmlElement)_Xml.GetElementsByTagName("Mod")[i];
                Mod r = new Mod
                {
                    ID = cl.GetAttribute("ID"),
                    ModID = cl.GetAttribute("ModID"),
                    ModName = cl.GetAttribute("ModName")
                };
                if (ID != null)
                    if (r.ID == ID)
                    {
                        result.Clear();
                        result.Add(r);
                        break;
                    }
                result.Add(r);
            }
            _File.Close();
            return result;
        }

        public void Edit(Mod mod)
        {

            _Xml = new XmlDocument();
            _File = new FileStream(_FilePath, FileMode.Open);
            _Xml.Load(_File);
            XmlNodeList list = _Xml.GetElementsByTagName("Mod");
            for (int i = 0; i < list.Count; i++)
            {
                XmlElement cu = (XmlElement)_Xml.GetElementsByTagName("Mod")[i];
                if (cu.GetAttribute("ID") == mod.ID)
                {
                    cu.SetAttribute("ModID", mod.ModID);
                    cu.SetAttribute("ModName", mod.ModName);
                    break;
                }
            }
            _File.Close();
            _Xml.Save(_FilePath);
        }

        public void Remove(Mod mod)
        {
            _File = new FileStream(_FilePath, FileMode.Open);
            XmlDocument tdoc = new XmlDocument();
            tdoc.Load(_File);
            XmlNodeList list = tdoc.GetElementsByTagName("Mod");
            for (int i = 0; i < list.Count; i++)
            {
                XmlElement cl = (XmlElement)tdoc.GetElementsByTagName("Mod")[i];
                if (cl.GetAttribute("ID") == mod.ID)
                {
                    tdoc.DocumentElement.RemoveChild(cl);
                }
            }
            _File.Close();
            tdoc.Save(_FilePath);
            ReCount();
        }

        public int Count()
        {
            _Xml = new XmlDocument();
            _File = new FileStream(_FilePath, FileMode.Open);
            _Xml.Load(_File);
            XmlNodeList list = _Xml.GetElementsByTagName("Mod");
            _File.Close();
            _Xml.Save(_FilePath);
            return list.Count;
        }

        private void ReCount()
        {
            _Xml = new XmlDocument();
            _File = new FileStream(_FilePath, FileMode.Open);
            _Xml.Load(_File);
            XmlNodeList list = _Xml.GetElementsByTagName("Mod");
            for (int i = 0; i < list.Count; i++)
            {
                XmlElement cu = (XmlElement)_Xml.GetElementsByTagName("Mod")[i];
                cu.SetAttribute("ID", i.ToString());
            }
            _File.Close();
            _Xml.Save(_FilePath);
        }

        private void CreateFile()
        {
            XmlTextWriter xtw;
            xtw = new XmlTextWriter(_FilePath, Encoding.UTF8);
            xtw.WriteStartDocument();
            xtw.WriteStartElement("ModDetails");
            xtw.WriteEndElement();
            xtw.Close();
        }
    }
}
