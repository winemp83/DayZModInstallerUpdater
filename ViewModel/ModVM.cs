using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Models;
using MyICommand;
using System.ComponentModel;

namespace ViewModel
{
    public class ModVM : BindableBase.BindableBase
    {
        private Mod _SelectedMod;


        public ModVM()
        {

        }

        public MyICommand.MyICommand AddCommand { get; set; }
        public MyICommand.MyICommand EditCommand { get; set; }
        public MyICommand.MyICommand DeleteCommand { get; set; }
        public MyICommand.MyICommand UpdateCommand { get; set; }

        public BindingList<Mod> Mods
        {
            get;
            set;
        }
        public Mod SelectedMod { 
            get { return _SelectedMod; }
            set { SetProperty(ref _SelectedMod, value); }
        }
        
        public void OnAdd()
        {

        }
        public void OnEdit()
        {

        }
        public void OnDelete()
        {

        }
        public void OnUpdate()
        {

        }
        public bool CanEdit()
        {
            return _SelectedMod != null;
        }
        public bool CanDelete()
        {
            return _SelectedMod != null;
        }
        public bool CanUpdate()
        {
            return true;
        }
    }
}
