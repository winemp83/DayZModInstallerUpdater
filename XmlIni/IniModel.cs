using Logging;

namespace XmlIni
{
    public class IniModel : BaseModel
    {
        private string _Key = null;
        private string _Value = null;

        public string Key
        {
            get {
                if (_Key == null || _Key.Length <= 0)
                {
                    EventLog.WriteEventLog(EventTyp.Warning, "Zugriff auf nicht gesetzten Wert in Ini");
                    return "NotSet";
                }
                return _Key;
            }
            set
            {
                if(Key != value) {
                    _Key = value;
                    RaisePropertyChanged("Key");
                }
            }
        }
        public string Value
        {
            get
            {
                if (_Value == null || _Value.Length <= 0)
                {
                    EventLog.WriteEventLog(EventTyp.Warning, "Zugriff auf nicht gesetzten Wert in Ini");
                    return "NotSet";
                }
                return _Value;
            }
            set
            {
                if (Value != value)
                {
                    _Value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }
    }
}
