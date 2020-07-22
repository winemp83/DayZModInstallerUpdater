using BindableBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Models
{
    public class BaseModel : ValidatableBindableBase
    {
        private string _ID;

        public string ID
        {
            get { return (_ID); }
            set { SetProperty(ref _ID, value); }
        }

        public string NullCheck(ref string Value, string msg) {
            return (Value != null && Value.Length >= 0) ? Value: msg;
        }
    }
}
