using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model
{
    public class BaseModel : INotifyPropertyChanged, IEquatable<BaseModel>, IBaseModel
    {
        private string _ID = null;
        public string ID
        {
            get
            {
                if (_ID == null)
                    return "-1";
                return _ID;
            }
            set
            {
                if (ID != value)
                {
                    _ID = value;
                    RaisePropertyChanged("ID");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseModel);
        }

        public bool Equals(BaseModel other)
        {
            return other != null &&
                   _ID == other._ID &&
                   ID == other.ID;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_ID, ID);
        }

        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public static bool operator ==(BaseModel left, BaseModel right)
        {
            return EqualityComparer<BaseModel>.Default.Equals(left, right);
        }

        public static bool operator !=(BaseModel left, BaseModel right)
        {
            return !(left == right);
        }
    }
}
