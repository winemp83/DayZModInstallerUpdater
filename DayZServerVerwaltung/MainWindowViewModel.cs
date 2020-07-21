using BindableBase;
using DayZServerVerwaltung.ViewModel;
using DayZServerVerwaltung.Views;
using MyICommand;
using Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace DayZServerVerwaltung
{
    class MainWindowViewModel : BindableBase.BindableBase
    {

        public MainWindowViewModel()
        {
            //_SV = new ServerVerwaltung();
            NavCommand = new MyICommand<string>(OnNav);
            _timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (sender, args) =>
            {
                //StatusText = _SV.GetServerInfo();
                StatusText = $@"Test Run since : {DateTime.Now.ToShortTimeString()}";
            };
            _timer.Start();
            
        }

        private readonly ServerVerwaltung _SV;

        private string _StatusText;

        private readonly IniViewModel iniViewModel = new IniViewModel();

        private readonly ServerViewModel serverViewModel = new ServerViewModel();

        private readonly ModViewModel modViewModel = new ModViewModel();

        private BindableBase.BindableBase _CurrentViewModel;

        public string StatusText
        {
            get { return _StatusText; }
            set { SetProperty(ref _StatusText, value); }
        }
        public BindableBase.BindableBase CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { SetProperty(ref _CurrentViewModel, value); }
        }
        public DispatcherTimer _timer;

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