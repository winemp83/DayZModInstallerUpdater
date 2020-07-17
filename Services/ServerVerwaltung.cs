using System;
using System.Timers;
using System.Diagnostics;

using Logging;
using XmlIni;
using System.Collections.Generic;

namespace Services
{
    public class ServerVerwaltung
    {
        private Timer _Timer;
        private readonly Config _Config;
        
        public ServerVerwaltung()
        {
            _Timer = new Timer();
            _Config = new Config();
        }

        #region Public Methods
        public void KillServer(int fromtimer = 0)
        {
            try
            {
                // Store all running process in the system
                Process[] runingProcess = Process.GetProcesses();
                for (int i = 0; i < runingProcess.Length; i++)
                {
                    // compare equivalent process by their name
                    if (runingProcess[i].ProcessName.Equals(_Config.GetValue("ServerConfigExe")) || runingProcess[i].MainWindowTitle.Contains(_Config.GetValue("ServerConfigConsoleName")))
                    {
                        // kill  running process
                        runingProcess[i].Kill();
                    }

                }
                EventLog.WriteEventLog(EventTyp.Information,"!Server beendet!");
                if (fromtimer != 1)
                    _Timer = null;
                else
                    StartServer();
            }catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
        public void StartServer()
        {
            try
            {
                Process.Start($@"{_Config.GetValue("ServerBatchPfad")}\{_Config.GetValue("ServerBatchName")}");
                _Timer = new System.Timers.Timer
                {
                    Interval = Convert.ToInt32(_Config.GetValue("ServerLaufzeit"))
                };
#if DEBUG
                EventLog.WriteEventLog(EventTyp.Debug, "Server Laufzeit auf 120 Sekunden gesetzt!");
                _Timer.Interval = 12000;
#endif
                _Timer.Elapsed += OnTimedEvent;
                _Timer.AutoReset = true;
                _Timer.Enabled = true;
                EventLog.WriteEventLog(EventTyp.Information, "!Server gestartet!");
            }
            catch(Exception ex) {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
            
        }
        public void BackupServer(int fromtimer = 0)
        {
            try
            {
                if (fromtimer > 0)
                    KillServer();
                Process backupServer = new Process();
                backupServer.StartInfo.FileName = "robocopy";
                backupServer.StartInfo.Arguments = $@"{_Config.GetValue("DayZPfad")} {_Config.GetValue("ServerBackupPfad")} /mir /r:5 /log+:{_Config.GetValue("ServerBackupLogPfad")}\{_Config.GetValue("ServerBackupLogName")}";
                backupServer.Start();
                backupServer.WaitForExit();
                EventLog.WriteEventLog(EventTyp.Information, "!Server gesichert!");
            }catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
        public void CleanServer(int fromtimer = 0)
        {
            List<string> files = new List<string>()
            {
                "crash*.log",
                "DayZServer_64_2*",
                "script*.log"
            };
            try {
                if (fromtimer > 0)
                    KillServer();
                foreach(string s in files)
                {
                    Process clean = new Process();
                    clean.StartInfo.FileName = "del";
                    clean.StartInfo.Arguments = "/q \"" + _Config.GetValue("ServerConfigProfilPfad")+$@"\"+_Config.GetValue("ServerConfigProfilName") + "\\"+s+"\"";
                    clean.Start();
                    clean.WaitForExit();
                }
                EventLog.WriteEventLog(EventTyp.Information, "!Server aufgeräumt!");
            }catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
        public void UpdateServerMods()
        {
            try
            {
                KillServer();
                Process updateMods = new Process();
                updateMods.StartInfo.FileName = _Config.GetValue("DBPfad") + @"\MUpdate.bat";
                updateMods.Start();
                updateMods.WaitForExit();
                EventLog.WriteEventLog(EventTyp.Information, "!Mods geupdated!");
            }catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error,ex.Message);
            }
        }
        public void UpdateServer()
        {
            try
            {
                KillServer();
                CleanServer();
                BackupServer();
                Process backupUser = new Process();
                backupUser.StartInfo.FileName = "robocopy";
                backupUser.StartInfo.Arguments = $@"{_Config.GetValue("ServerBackupProfilPfad")}\{_Config.GetValue("ServerBackupProfilName")} {_Config.GetValue("ServerBackupProfilTarget")} /mir /r:5 /log+:{_Config.GetValue("ServerBackupLogPfad")}\{_Config.GetValue("ServerBackupLogName")}";
                backupUser.Start();
                backupUser.WaitForExit();
                Process Update = new Process();
                Update.StartInfo.FileName = _Config.GetValue("DBPfad") + @"\SUpdate.bat";
                Update.Start();
                Update.WaitForExit();
                EventLog.WriteEventLog(EventTyp.Information, "!Server geupdated!");
            }catch(Exception ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, ex.Message);
            }
        }
        #endregion
        #region Private Methods
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            KillServer(1);
            BackupServer(1);
        }
        #endregion
    }
}
