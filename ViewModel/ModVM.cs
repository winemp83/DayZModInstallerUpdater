using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Models;
using MyICommand;
using System.ComponentModel;
using Database;
using System.IO;

namespace ViewModel
{
    public class ModVM : BindableBase.BindableBase
    {
        private Mod _SelectedMod;
        private ModDB _DB;
        private IniDB _Configs;

        public ModVM()
        {
            /*
            _Configs = new IniDB($@"C:\steam\DayZApp\db_config.sak");
            _ = new BindingList<Ini>();
            BindingList<Ini> configs = _Configs.Get();
            _DB = new ModDB(ref configs);
            */
            AddCommand = new MyICommand.MyICommand(OnAdd);
            EditCommand = new MyICommand.MyICommand(OnEdit, CanEdit);
            DeleteCommand = new MyICommand.MyICommand(OnDelete, CanDelete);
            UpdateCommand = new MyICommand.MyICommand(OnUpdate, CanUpdate);
            SaveCommand = new MyICommand.MyICommand(OnSave, CanSave);
            CancelCommand = new MyICommand.MyICommand(OnCancel);
            Mods = new BindingList<Mod>();
            SelectedMod = new Mod();
            Load();
        }

        public MyICommand.MyICommand AddCommand { get; set; }
        public MyICommand.MyICommand EditCommand { get; set; }
        public MyICommand.MyICommand DeleteCommand { get; set; }
        public MyICommand.MyICommand UpdateCommand { get; set; }
        public MyICommand.MyICommand SaveCommand { get; set; }
        public MyICommand.MyICommand CancelCommand { get; set; }

        public BindingList<Mod> Mods
        {
            get;
            set;
        }
        public Mod SelectedMod { 
            get { return _SelectedMod; }
            set { SetProperty(ref _SelectedMod, value); DeleteCommand.RaiseCanExecuteChange();SaveCommand.RaiseCanExecuteChange(); }
        }
        
        public void OnCancel() {
            OnAdd();
        }
        public void OnSave()
        {
            Mods.Add(SelectedMod);
        }
        public void OnAdd()
        {
            SelectedMod = new Mod();
        }
        public void OnEdit()
        {
        }
        public void OnDelete()
        {
            Mods.Remove(SelectedMod);
            SelectedMod = new Mod();
            
        }
        public void OnUpdate()
        {
            /*
            Services.MakeBatch mb = new Services.MakeBatch();
            mb.CreateModUpdateBatch($@"C:\steam\DayZTools\batch\MUpdate.bat");
            Services.ServerVerwaltung sv = new Services.ServerVerwaltung();
            sv.UpdateServerMods();
            if (!File.Exists($@"C:\steam\DayZTools\batch\MUpdate.bat"))
                File.Delete($@"C:\steam\DayZTools\batch\MUpdate.bat");
        */
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
        public bool CanSave()
        {
            return !Mods.Contains(SelectedMod);
        }

        private void Reload()
        {
            Mods.Clear();
            Load();
            //foreach (Mod m in _DB.Get())
                //Mods.Add(m);
        }
        private void Load()
        {
            Mods.Add(new Mod()
            {
                ID = "0",
                ModID = "1010101010",
                ModName = "TestMod",
                IsActive = "false",
                IsServerMod = "false",
                IsUpdate = "false",
                Order = "1"
            }) ;
            Mods.Add(new Mod()
            {
                ID = "1",
                ModID = "1010101010",
                ModName = "TestMod",
                IsActive = "false",
                IsServerMod = "false",
                IsUpdate = "false",
                Order = "2"
            });
            Mods.Add(new Mod()
            {
                ID = "2",
                ModID = "1010101010",
                ModName = "TestMod",
                IsActive = "false",
                IsServerMod = "false",
                IsUpdate = "false",
                Order = "3"
            });
            Mods.Add(new Mod()
            {
                ID = "3",
                ModID = "1010101010",
                ModName = "TestMod",
                IsActive = "false",
                IsServerMod = "false",
                IsUpdate = "false",
                Order = "4"
            });
            Mods.Add(new Mod()
            {
                ID = "4",
                ModID = "1010101010",
                ModName = "TestMod",
                IsActive = "false",
                IsServerMod = "false",
                IsUpdate = "false",
                Order = "5"
            });
            Mods.Add(new Mod()
            {
                ID = "5",
                ModID = "1010101010",
                ModName = "TestMod",
                IsActive = "false",
                IsServerMod = "false",
                IsUpdate = "false",
                Order = "6"
            });
            Mods.Add(new Mod()
            {
                ID = "6",
                ModID = "1010101010",
                ModName = "TestMod",
                IsActive = "false",
                IsServerMod = "false",
                IsUpdate = "false",
                Order = "7"
            });
            Mods.Add(new Mod()
            {
                ID = "7",
                ModID = "1010101010",
                ModName = "TestMod",
                IsActive = "false",
                IsServerMod = "false",
                IsUpdate = "false",
                Order = "8"
            });
            Mods.Add(new Mod()
            {
                ID = "8",
                ModID = "1010101010",
                ModName = "TestMod",
                IsActive = "false",
                IsServerMod = "false",
                IsUpdate = "false",
                Order = "9"
            });
            Mods.Add(new Mod()
            {
                ID = "9",
                ModID = "1010101010",
                ModName = "TestMod",
                IsActive = "false",
                IsServerMod = "false",
                IsUpdate = "false",
                Order = "10"
            });
        }
    }
}
