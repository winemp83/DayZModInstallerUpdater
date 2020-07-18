using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZToolIniFile.Model
{
    public class AddEditIniModel : BindableBase.ValidatableBindableBase
    {
        private string _Key;
        [Required]
        private string _Value;
        [Required]

        public string Key
        {
            get { return _Key; }
            set { SetProperty(ref _Key, value); }
        }

        public string Value
        {
            get { return _Value; }
            set { SetProperty(ref _Value, value); }
        }
    }
}
