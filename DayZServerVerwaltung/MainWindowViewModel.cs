using BindableBase;
using DayZServerVerwaltung.ViewModel;
using DayZServerVerwaltung.Views;
using MyICommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace DayZServerVerwaltung
{
    class MainWindowViewModel : BindableBase.BindableBase
    {

        public MainWindowViewModel()
        {
            NavCommand = new MyICommand<string>(OnNav);
        }

        private readonly IniViewModel iniViewModel = new IniViewModel();

        private ServerViewModel serverViewModel = new ServerViewModel();

        private ModViewModel modViewModel = new ModViewModel();

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
                "ini" => iniViewModel,
                "mod" => modViewModel,
                _ => serverViewModel,
            };
        }
    }
}