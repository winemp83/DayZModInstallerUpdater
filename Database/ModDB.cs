using Logging;
using System;
using Models;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Xml;
using System.ComponentModel;
using System.Linq;

namespace Database
{
    public class ModDB
    {
        private readonly BindingList<Ini> _Configs = new BindingList<Ini>();
        private FileStream _File;
        private XmlDocument _Xml;
        private readonly string _FilePath = null;

        public ModDB(ref BindingList<Ini> config)
        {
            _Configs = new BindingList<Ini>();
            _Configs = config;
            _FilePath = $@"{GetConfigValue("DBPfad")}\{GetConfigValue("DBName")}";
            if (!File.Exists(_FilePath))
                Create();
        }

        #region Public Methods
        public void Add(Mod mod)
        {
            try
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
            }catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
        public BindingList<Mod> Get(string ID = null)
        {
            BindingList<Mod> result = new BindingList<Mod>();
            try
            {
                _Xml = new XmlDocument();
                _File = new FileStream(_FilePath, FileMode.Open);
                _Xml.Load(_File);

                XmlNodeList list = _Xml.GetElementsByTagName("ModModel");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cl = (XmlElement)_Xml.GetElementsByTagName("ModModel")[i];
                    Mod r = new Mod
                    {
                        ID = cl.GetAttribute("ID"),
                        ModID = cl.GetAttribute("ModID"),
                        ModName = cl.GetAttribute("ModName"),
                        IsActive = cl.GetAttribute("IsActive"),
                        IsUpdate = cl.GetAttribute("IsUpdate"),
                        IsServerMod = cl.GetAttribute("IsServerMod")
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
            }catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
            return result;
        }
        public void Edit(Mod mod)
        {
            try
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
            }catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
        public void Remove(Mod mod)
        {
            try
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
            }
            catch (Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
            finally
            {
                ReCount();
            }
        }
        public int Count()
        {
            int result = 0;
            try
            {
                _Xml = new XmlDocument();
                _File = new FileStream(_FilePath, FileMode.Open);
                _Xml.Load(_File);
                XmlNodeList list = _Xml.GetElementsByTagName("ModModel");
                _File.Close();
                _Xml.Save(_FilePath);
                result = list.Count;
            }catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
            return result;
        }
        #endregion
        #region Private Methods
        private void ReCount()
        {
            try
            {
                _Xml = new XmlDocument();
                _File = new FileStream(_FilePath, FileMode.Open);
                _Xml.Load(_File);
                XmlNodeList list = _Xml.GetElementsByTagName("ModModel");
                for (int i = 0; i < list.Count; i++)
                {
                    XmlElement cu = (XmlElement)_Xml.GetElementsByTagName("ModModel")[i];
                    cu.SetAttribute("ID", (i + 1).ToString());
                }
                _File.Close();
                _Xml.Save(_FilePath);
            }catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
        private void Create()
        {
            try
            {
                XmlTextWriter xtw;
                xtw = new XmlTextWriter(_FilePath, Encoding.UTF8);
                xtw.WriteStartDocument();
                xtw.WriteStartElement("ModDetails");
                xtw.WriteEndElement();
                xtw.Close();
            }
            catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
        #endregion
        private string GetConfigValue(string keyName)
        {
            return _Configs.FirstOrDefault(x=> x.Key.Equals(keyName)).Value;
        }
    }
}
