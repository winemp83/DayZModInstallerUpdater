using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Windows.Threading;
using MyICommand;
using Services;
using ViewModel;

namespace DayZServerVerwaltung.ViewModel
{
    public class ServerViewModel : ServerVM
    {
        public DispatcherTimer _timer;

        public ServerViewModel() : base()
        {
            DateTime _now = DateTime.Now;
            //_SV = new ServerVerwaltung();
            StartCommand = new MyICommand.MyICommand(OnStart, CanStart);
            StopCommand = new MyICommand.MyICommand(OnStop, CanStop);
            UpdateCommand = new MyICommand.MyICommand(OnUpdate, CanUpdate);
            _timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (sender, args) =>
            {
                StartCommand.RaiseCanExecuteChange();
                StopCommand.RaiseCanExecuteChange();
                UpdateCommand.RaiseCanExecuteChange();
                Text = $@"Hier Könnte ihre Werbund stehen !!! und das ganze Seid : {_now.ToShortDateString()} {_now.ToShortTimeString()}";
            };
            _timer.Start();
        }
    }
}
