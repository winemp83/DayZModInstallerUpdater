using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using MyICommand;
namespace ViewModel
{
    public class ModVM : BindableBase.BindableBase
    {
        public ModVM()
        {

        }

        public MyICommand.MyICommand AddCommand { get; set; }
        public MyICommand.MyICommand EditCommand { get; set; }
        public MyICommand.MyICommand DeleteCommand { get; set; }

    }
}
