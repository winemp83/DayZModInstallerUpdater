using System.ComponentModel;

namespace XmlIni
{
    public class Config : BindableBase.BindableBase
    {
        private readonly IniDB _DB;
        public BindingList<IniModel> Configs
        {
            get;
            set;
        }

        public Config()
        {
            _DB = new IniDB($@"C:\steam\ini.sak");
            Configs = new BindingList<IniModel>();
            LoadConfig();
        }
        public void Add(string key, string value = "NotSet") {
            IniModel n = new IniModel() { Key = key, Value = value };
            _DB.Insert(n);
            Reload();
        }
        public void Edit(string key, string newValue = "NotSet")
        {
            IniModel _tmp = null;
            foreach(IniModel i in Configs) {
                if (i.Key.Equals(key))
                {
                    _tmp = new IniModel();
                    i.Value = newValue;
                    _tmp = i;
                    break;
                }
            }
            if(_tmp != null)
                _DB.Update(_tmp);
            Reload();
        }
        public void Del(string key)
        {
            _DB.Delete(new IniModel() { Key = key });
            Reload();
        }
        public string GetValue(string key)
        {
            string result = "";
            foreach(IniModel i in Configs) {
                if (i.Key.Equals(key))
                {
                    result = i.Value;
                }
            }
            return result;
        }

        private void LoadConfig()
        {
            if (_DB.Get().Count <= 0)
            {
                Add("SteamUser");
                Add("SteamPass");
                Add("SteamPfad");

                Add("DayZPfad");

                Add("DBPfad");
                Add("DBName");

                Add("ServerConfigLaufzeit");
                Add("ServerConfigStartBatchName");
                Add("ServerConfigConsoleTitel");
                Add("ServerConfigProfilName");
                Add("ServerConfigPort");
                Add("ServerConfigName");
                Add("ServerConfigCpu");
                Add("ServerConfigdoLogs");
                Add("ServerConfigadmLog");
                Add("ServerConfignetLog");
                Add("ServerConfigfreezecheck");

                Add("ServerBackupPfad");
                Add("ServerBackupLogName");
                Add("ServerBackupLogPfad");
                Add("ServerBackupProfilPfad");

                Add("ServerClean");
            }
            Reload();
            Configs.ListChanged += new ListChangedEventHandler(Config_Changed);
        }
        private void Config_Changed(object sender, ListChangedEventArgs e)
        {
            if (e.OldIndex >= 0 && e.NewIndex >= 0)
                _DB.Update(Configs[e.NewIndex]);
            Reload();
        }
        public void Reload()
        {
            Configs.Clear();
            foreach (IniModel i in _DB.Get())
                Configs.Add(i);
        }
    }
}
