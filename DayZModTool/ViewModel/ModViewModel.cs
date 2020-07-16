
using DayZModTool.Database.XmlDatabase;
using DayZModTool.Model;
using DayZModTool.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Windows;

namespace DayZModTool.ViewModel
{
    public class ModViewModel
    {
        private XmlData _DB;
        private ModModel _SelectedMod;

        public MyICommand DeleteCommand { get; set; }
        public MyICommand AddCommand { get; set; }
        public MyICommand EditCommand { get; set; }
        public MyICommand MakeBatchCommand { get; set; }
        public MyICommand MakeStartBatchCommand { get; set; }
        public MyICommand UpdateModsCommand { get; set; }
        public MyICommand UpdateServerCommand { get; set; }
        public MyICommand StartCommand { get; set; }
        public MyICommand StopCommand { get; set; }
        public ModModel SelectedMod
        {
            get { return _SelectedMod; }
            set
            {
                _SelectedMod = value;
                DeleteCommand.RaiseCanExecuteChange();
                EditCommand.RaiseCanExecuteChange();
            }
        }
        public BindingList<ModModel> Mods
        {
            get;
            set;
        }

        public ModViewModel()
        {
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            AddCommand = new MyICommand(OnAdd);
            EditCommand = new MyICommand(OnEdit, CanEdit);
            MakeBatchCommand = new MyICommand(OnMakeBatch, CanMakeBatch);
            MakeStartBatchCommand = new MyICommand(OnMakeServerBatch);
            UpdateModsCommand = new MyICommand(OnUpdateMods);
            StartCommand = new MyICommand(OnStart);
            StopCommand = new MyICommand(OnStop);
            UpdateServerCommand = new MyICommand(OnUpdateServer);
            LoadData();
        }

        

        public void LoadData()
        {
            Mods = new BindingList<ModModel>();
            _DB = new XmlData();
            if(_DB.Count() == 0)
            {
                _DB.Add(new ModModel
                {
                    ID = "0",
                    ModID = "000000000001",
                    ModName = "TestName1"
                });
                _DB.Add(new ModModel
                {
                    ID = "1",
                    ModID = "000000000002",
                    ModName = "TestName2"
                });
                _DB.Add(new ModModel
                {
                    ID = "2",
                    ModID = "000000000003",
                    ModName = "TestName3"
                });
            }
            Reload();
            Mods.ListChanged += new ListChangedEventHandler(Mods_ListChanged);
        }

        private void OnDelete()
        {
            _DB.Remove(_SelectedMod);
            Reload();
            _SelectedMod = null;
        }
        private void OnAdd()
        {
            Views.Dialogs.AddEditMod ae = new Views.Dialogs.AddEditMod();
            ModModel n = new ModModel
            {
                ID = (_DB.Count() + 1).ToString(),
                ModID = "000000000",
                ModName = "new Mod",
                IsActive = "false"
            };
            ae.Mod = n;
            if(ae.ShowDialog() == true) {
                
                n.ID = ae.Mod.ID;
                n.ModID = ae.Mod.ModID;
                n.ModName = ae.Mod.ModName;
                n.IsActive = ae.Mod.IsActive;
                n.IsUpdate = ae.Mod.IsUpdate;
                n.IsServerMod = ae.Mod.IsServerMod;
                _DB.Add(n);
                Reload();
            }
        }
        private void OnEdit()
        {
            Views.Dialogs.AddEditMod ae = new Views.Dialogs.AddEditMod(SelectedMod);
            if(ae.ShowDialog() == true)
            {
                _DB.Edit(ae.Mod);
                Reload();
                _SelectedMod = null;
            }
            
        }
        private void OnMakeBatch()
        {
            MakeUpdateBat mub = new MakeUpdateBat();
            mub.CreateBatch(ConfigurationManager.AppSettings["XmlPfad"] + @"\MUpdate.bat");
            mub.CreateServerUpdate(ConfigurationManager.AppSettings["XmlPfad"] + @"\SUpdate.bat");
        }
        private void OnMakeServerBatch()
        {
            MakeUpdateBat mub = new MakeUpdateBat();
            mub.MakeStartScript(ConfigurationManager.AppSettings["DayZPfad"] + $@"\ServerStart.bat");
        }
        private void OnUpdateMods()
        {
            ServerVerwaltung sv = new ServerVerwaltung();
            sv.UpdateServerMods();
        }
        private void OnUpdateServer()
        {
            ServerVerwaltung sv = new ServerVerwaltung();
            sv.UpdateServer();
        }
        private void OnStart()
        {
            ServerVerwaltung sv = new ServerVerwaltung();
            sv.StartServer();
        }
        private void OnStop()
        {
            ServerVerwaltung sv = new ServerVerwaltung();
            sv.KillServer();
        }
        private bool CanDelete()
        {
            return _SelectedMod != null;
        }
        private bool CanEdit()
        {
            return _SelectedMod != null;
        }
        private bool CanMakeBatch()
        {
            foreach(ModModel m in _DB.Get()) {
                if (m.IsActive.Equals("true"))
                    return true;
            }
            return false;
        }
        private void Mods_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.OldIndex >= 0 && e.NewIndex >= 0)
            {
                _DB.Edit(Mods[e.NewIndex]);
            }
            MakeBatchCommand.RaiseCanExecuteChange();
        }
        private void Reload()
        {
            Mods.Clear();
            foreach (ModModel m in _DB.Get())
            {
                Mods.Add(m);
            }
        }
    }
}
