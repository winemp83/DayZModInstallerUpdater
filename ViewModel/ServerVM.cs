using System;
using Services;
using System.Collections.Generic;
using System.Text;

namespace ViewModel
{
    public class ServerVM :BindableBase.BindableBase
    {
        private readonly ServerVerwaltung _SV;
        private string _Text;

        public MyICommand.MyICommand StartCommand { get; set; }
        public MyICommand.MyICommand StopCommand { get; set; }
        public MyICommand.MyICommand UpdateCommand { get; set; }
        public string Text { get { return _Text; } set { SetProperty(ref _Text, value); } } 
        public ServerVM()
        {
            //_SV = new ServerVerwaltung();
            
        }


        public void OnStart()
        {
            //_SV.StartServer();
        }
        public bool CanStart()
        {
            //return (!_SV.GetServerRunning());
            return true;
        }
        public void OnStop()
        {
            //_SV.KillServer();
        }
        public bool CanStop()
        {
            if (DateTime.Now.Second % 45 == 0)
                return true;
            return false;
            //return _SV.GetServerRunning();
        }
        public void OnUpdate()
        {
            //_SV.UpdateServer();
        }
        public bool CanUpdate()
        {
            if (DateTime.Now.Second % 30 == 0)
                return true;
            return false;
            //return (!_SV.GetServerRunning());
        }
    }
}