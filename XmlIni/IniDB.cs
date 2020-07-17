using Logging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;

namespace XmlIni
{
    public class IniDB
    {
        private XmlDocument _Xml;
        private FileStream _File;
        private readonly string _FilePath;

        public IniDB(string filename) {
            _FilePath = filename;
            if (!File.Exists(_FilePath))
                Create();
        }

        public void Insert(IniModel value) {
            try
            {
                _Xml = new XmlDocument();
                _File = new FileStream(_FilePath, FileMode.Open);
                _Xml.Load(_File);
                XmlElement cl = _Xml.CreateElement("Section");
                cl.SetAttribute("Key", value.Key);
                cl.SetAttribute("Value", value.Value);
                _File.Close();
                _Xml.Save(_FilePath);
            }
            catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
        public void Update(IniModel value)
        {
            try
            {
                _Xml = new XmlDocument();
                _File = new FileStream(_FilePath, FileMode.Open);
                _Xml.Load(_File);
                XmlNodeList list = _Xml.GetElementsByTagName("Section");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cu = (XmlElement)_Xml.GetElementsByTagName("Section")[i];
                    if (cu.GetAttribute("Key") == value.Key)
                    {
                        cu.SetAttribute("Value", value.Value);
                        break;
                    }
                }
                _File.Close();
            }
            catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
        public void Delete(IniModel value)
        {
            try
            {
                _File = new FileStream(_FilePath, FileMode.Open);
                _Xml = new XmlDocument();
                _Xml.Load(_File);
                XmlNodeList list = _Xml.GetElementsByTagName("Section");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cl = (XmlElement)_Xml.GetElementsByTagName("Section")[i];
                    if (cl.GetAttribute("Key") == value.Key)
                    {
                        _Xml.DocumentElement.RemoveChild(cl);
                    }
                }
                _File.Close();
                _Xml.Save(_FilePath);
            }
            catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error,ex.Message);
            }
        }
        public BindingList<IniModel>Get(string key = null)
        {
            BindingList<IniModel> result = new BindingList<IniModel>();
            try
            {
                _Xml = new XmlDocument();
                _File = new FileStream(_FilePath, FileMode.Open);
                _Xml.Load(_File);
                XmlNodeList list = _Xml.GetElementsByTagName("Section");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cl = (XmlElement)_Xml.GetElementsByTagName("Section")[i];
                    if (key != null || key.Length <= 0)
                    {
                        if (cl.GetAttribute("Key").Equals(key))
                        {
                            result.Add(new IniModel() { Key = cl.GetAttribute("Key"), Value = cl.GetAttribute("Value") });
                            break;
                        }
                    }
                    else {
                        result.Add(new IniModel() { Key = cl.GetAttribute("Key"), Value = cl.GetAttribute("Value") });
                    }
                }
                _File.Close();
            }
            catch (Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
            return result;
        }
        private void Create() {
            try
            {
                XmlTextWriter xtw;
                xtw = new XmlTextWriter(_FilePath, Encoding.UTF8);
                xtw.WriteStartDocument();
                xtw.WriteStartElement("Ini");
                xtw.WriteEndElement();
                xtw.Close();
            }
            catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
    }
}
