
using DayZModTool.Database.XmlDatabase;
using DayZModTool.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace DayZModTool.ViewModel
{
    public class ModViewModel
    {
        private XmlData _DB;
        private ModModel _SelectedMod;

        public MyICommand DeleteCommand { get; set; }

        public ModModel SelectedMod
        {
            get { return _SelectedMod; }
            set
            {
                _SelectedMod = value;
                DeleteCommand.RaiseCanExecuteChange();
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
            foreach(ModModel m in _DB.Get())
            {
                Mods.Add(m);
            }
            Mods.ListChanged += new ListChangedEventHandler(Mods_ListChanged);
        }

        private void OnDelete()
        {
            _DB.Remove(_SelectedMod);
            Mods.Remove(_SelectedMod);
            Mods.Clear();
            foreach (ModModel m in _DB.Get())
            {
                Mods.Add(m);
            }
            _SelectedMod = null;
        }
        private bool CanDelete()
        {
            return _SelectedMod != null;
        }
        private void Mods_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.OldIndex >= 0 && e.NewIndex > 0)
            {
                MessageBox.Show(e.NewIndex.ToString());
                _DB.Edit(Mods[e.NewIndex]);
            }
        }
    }
}
