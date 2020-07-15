using System;
using System.Collections.Generic;

namespace Model
{
    public class Mod : BaseModel, IEquatable<Mod>, IMod
    {
        private string _ModID = null;
        private string _ModName = null;

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

        public Mod(string id = null, string modID = null, string modName = null)
        {
            if (id != null)
                base.ID = id;
            if (modID != null)
                ModID = modID;
            if (modName != null)
                ModName = modName;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Mod);
        }

        public bool Equals(Mod other)
        {
            return other != null &&
                   base.Equals(other) &&
                   ID == other.ID &&
                   _ModID == other._ModID &&
                   _ModName == other._ModName &&
                   ModID == other.ModID &&
                   ModName == other.ModName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), ID, _ModID, _ModName, ModID, ModName);
        }

        public static bool operator ==(Mod left, Mod right)
        {
            return EqualityComparer<Mod>.Default.Equals(left, right);
        }

        public static bool operator !=(Mod left, Mod right)
        {
            return !(left == right);
        }
    }
}
