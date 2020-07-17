using System.ComponentModel;

namespace XmlIni
{
    public class Config
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
            foreach(IniModel i in Configs) {
                if (i.Key.Equals(key))
                    i.Value = newValue;
            }
            _DB.Update(new IniModel() { Key = key, Value = newValue });
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
                    result = i.Value;
            }
            return result;
        }

        private void LoadConfig()
        {
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
