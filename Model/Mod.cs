namespace Model
{
    public class ModModel : BaseModel
    {
        private string _ModID = null;
        private string _ModName = null;
        private bool _IsActive = true;
        private bool _IsUpdate = false;
        private bool _IsServerMod = false;

        public string ModID
        {
            get
            {
                if (_ModID == null)
                    return "0000000000";
                return _ModID;
            }
            set
            {
                if (ModID != value)
                {
                    _ModID = value;
                    RaisePropertyChanged("ModID");
                }
            }
        }

        public string ModName
        {
            get
            {
                if (_ModName == null)
                    return "NewMod";
                return _ModName;
            }
            set
            {
                if (ModName != value)
                {
                    _ModName = value;
                    RaisePropertyChanged("ModName");
                }
            }
        }

        public string IsActive
        {
            get
            {
                return GetBoolValue(_IsActive);
            }
            set
            {
                if(IsActive != value)
                {
                    _IsActive = GetBoolValue(value);
                    RaisePropertyChanged("IsActive");
                }
            }
        }
        public string IsUpdate
        {
            get
            {
                return GetBoolValue(_IsUpdate);
            }
            set
            {
                if (IsUpdate != value)
                {
                    _IsUpdate = GetBoolValue(value);
                    RaisePropertyChanged("IsUpdate");
                }
            }
        }
        public string IsServerMod
        {
            get
            {
                return GetBoolValue(_IsServerMod);
            }
            set
            {
                if (IsServerMod != value)
                {
                    _IsServerMod = GetBoolValue(value);
                    RaisePropertyChanged("IsServerMod");
                }
            }
        }

        public ModModel(string id = null, string modID = null, string modName = null)
        {
            if (id != null)
                base.ID = id;
            if (modID != null)
                ModID = modID;
            if (modName != null)
                ModName = modName;
        }

        public static string GetBoolValue(bool value)
        {
            string result =  value.ToString().ToLowerInvariant();
            return result;
        }

        public static bool GetBoolValue(string value) {
            if (value.ToLowerInvariant().Equals("true"))
                return true;
            return false;
        }

    }
}
