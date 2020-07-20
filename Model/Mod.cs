namespace Models
{
    public class Mod : BaseModel
    {
        private string _ModID = null;
        private string _ModName = null;
        private bool _IsActive = true;
        private bool _IsUpdate = false;
        private bool _IsServerMod = false;
        private string _Order;

        public string ModID
        {
            get
            {
                return NullCheck(ref _ModID, "00000000");
            }
            set
            {
                SetProperty(ref _ModID, value);
            }
        }

        public string ModName
        {
            get
            {
                return NullCheck(ref _ModName,"New_Mod");
            }
            set
            {
                SetProperty(ref _ModName, value);
            }
        }

        public string Order
        {
            get { return NullCheck(ref _Order, "0"); }
            set { SetProperty(ref _Order, value); }
        }

        public string IsActive
        {
            get
            {
                return GetBoolValue(_IsActive);
            }
            set
            {
                SetProperty(ref _IsActive, GetBoolValue(value));
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
                SetProperty(ref _IsUpdate, GetBoolValue(value));
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
                SetProperty(ref _IsServerMod, GetBoolValue(value));
            }
        }

        public Mod(string id = null, string modID = null, string modName = null, string order = null)
        {
            base.ID = NullCheck(ref id, "0");
            ModID = NullCheck(ref modID, "00000000");
            ModName = NullCheck(ref modName, "Mod Name");
            Order = NullCheck(ref order, base.ID);
        }

        public static string GetBoolValue(bool value)
        {
            return value.ToString().ToLowerInvariant();
        }

        public static bool GetBoolValue(string value) {
            return value.ToLowerInvariant().Equals("true");
        }

    }
}
