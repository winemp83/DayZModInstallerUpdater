using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Crypt : BaseModel
    {
        private readonly int _KeySize = 256;
        private string _Secret;
        private string _Password;
        private string _Value;
        private string _Result;

        public string Password
        {
            get { return NullCheck(ref _Password, "Helena2014!"); }
            set { SetProperty(ref _Password, value); }
        }
        public string Value
        {
            get { return NullCheck(ref _Value, "Kein Text angegeben!"); }
            set { SetProperty(ref _Value, value); }
        }
        public string Result
        {
            get { return NullCheck(ref _Result, "Kein Ergebenis"); }
            set { SetProperty(ref _Result, value); }
        }
        public string Secret { 
            get { return NullCheck(ref _Secret, "!HashP2020MKFidb"); }
            protected set { SetProperty(ref _Secret, value); }
        }

        public int Keysize
        {
            get { return _KeySize; }
        }

        public Crypt(){
            Secret = "!HashP2020MKFidb";
        }
    }
}
