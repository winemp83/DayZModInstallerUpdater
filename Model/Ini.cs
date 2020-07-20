namespace Models
{
    public class Ini : BaseModel
    {
        private string _Key = null;
        private string _Value = null;

        public string Key
        {
            get {
                return NullCheck(ref _Key, "Not Set");
            }
            set
            {
                SetProperty(ref _Key, value);
            }
        }
        public string Value
        {
            get
            {
                return NullCheck(ref _Value, "Not Set");
            }
            set
            {
                SetProperty(ref _Value, value);
            }
        }
    }
}
