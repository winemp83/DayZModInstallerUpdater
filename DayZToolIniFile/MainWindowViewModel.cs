using MyICommand;
using DayZToolIniFile.ViewModel;
using BindableBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace DayZToolIniFile
{
    class MainWindowViewModel : BindableBase.BindableBase
    {

        public MainWindowViewModel()
        {
            NavCommand = new MyICommand<string>(OnNav);
        }

        private readonly AddEditIniViewModel addEditIniViewModel = new AddEditIniViewModel();

        private readonly IniViewModel iniViewModel = new IniViewModel();

        private BindableBase.BindableBase _CurrentViewModel;

        public BindableBase.BindableBase CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }
        }

        public MyICommand<string> NavCommand { get; private set; }

        private void OnNav(string destination)
        {

            CurrentViewModel = destination switch
            {
                "IniListView" => iniViewModel,
                _ => addEditIniViewModel,
            };
        }
    }
}