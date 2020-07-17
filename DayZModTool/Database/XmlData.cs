using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using DayZModTool.Model;

namespace DayZModTool.Database.XmlDatabase
{
    public class XmlData
    {
        private XmlDocument _Xml;
        private FileStream _File;
        private readonly string _FilePath;

        public XmlData()
        {
            _FilePath = ConfigurationManager.AppSettings["XmlPfad"] + @"\" + ConfigurationManager.AppSettings["XmlName"];
            if (!File.Exists(_FilePath))
                CreateFile();
        }

        public void Add(ModModel mod)
        {
            _Xml = new XmlDocument();
            _File = new FileStream(_FilePath, FileMode.Open);
            _Xml.Load(_File);
            XmlNodeList list = _Xml.GetElementsByTagName("ModModel");
            XmlElement cl = _Xml.CreateElement("ModModel");
            cl.SetAttribute("ID", (list.Count + 1).ToString());
            cl.SetAttribute("ModID", mod.ModID);
            cl.SetAttribute("ModName", mod.ModName);
            cl.SetAttribute("IsActive", mod.IsActive);
            cl.SetAttribute("IsUpdate", mod.IsUpdate);
            cl.SetAttribute("IsServerMod", mod.IsServerMod);
            _Xml.DocumentElement.AppendChild(cl);
            _File.Close();
            _Xml.Save(_FilePath);
        }

        public BindingList<ModModel> Get(string ID = null)
        {
            _Xml = new XmlDocument();
            _File = new FileStream(_FilePath, FileMode.Open);
            _Xml.Load(_File);
            BindingList<ModModel> result = new BindingList<ModModel> ();
            XmlNodeList list = _Xml.GetElementsByTagName("ModModel");
            for (int i = 0; i < list.Count; i++)
            {
                XmlElement cl = (XmlElement)_Xml.GetElementsByTagName("ModModel")[i];
                ModModel r = new ModModel
                {
                    ID = cl.GetAttribute("ID"),
                    ModID = cl.GetAttribute("ModID"),
                    ModName = cl.GetAttribute("ModName"),
                    IsActive = cl.GetAttribute("IsActive"),
                    IsUpdate = cl.GetAttribute("IsUpdate"),
                    IsServerMod = cl.GetAttribute("IsServerMod")};
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

        public void Edit(ModModel mod)
        {

            _Xml = new XmlDocument();
            _File = new FileStream(_FilePath, FileMode.Open);
            _Xml.Load(_File);
            XmlNodeList list = _Xml.GetElementsByTagName("ModModel");
            for (int i = 0; i < list.Count; i++)
            {
                XmlElement cu = (XmlElement)_Xml.GetElementsByTagName("ModModel")[i];
                if (cu.GetAttribute("ID") == mod.ID)
                {
                    cu.SetAttribute("ModID", mod.ModID);
                    cu.SetAttribute("ModName", mod.ModName);
                    cu.SetAttribute("IsActive", mod.IsActive);
                    cu.SetAttribute("IsUpdate", mod.IsUpdate);
                    cu.SetAttribute("IsServerMod", mod.IsServerMod);
                    break;
                }
            }
            _File.Close();
            _Xml.Save(_FilePath);
        }

        public void Remove(ModModel mod)
        {
            _File = new FileStream(_FilePath, FileMode.Open);
            XmlDocument tdoc = new XmlDocument();
            tdoc.Load(_File);
            XmlNodeList list = tdoc.GetElementsByTagName("ModModel");
            for (int i = 0; i < list.Count; i++)
            {
                XmlElement cl = (XmlElement)tdoc.GetElementsByTagName("ModModel")[i];
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
            XmlNodeList list = _Xml.GetElementsByTagName("ModModel");
            _File.Close();
            _Xml.Save(_FilePath);
            return list.Count;
        }

        private void ReCount()
        {
            _Xml = new XmlDocument();
            _File = new FileStream(_FilePath, FileMode.Open);
            _Xml.Load(_File);
            XmlNodeList list = _Xml.GetElementsByTagName("ModModel");
            for (int i = 0; i < list.Count; i++)
            {
                XmlElement cu = (XmlElement)_Xml.GetElementsByTagName("ModModel")[i];
                cu.SetAttribute("ID", (i+1).ToString());
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
