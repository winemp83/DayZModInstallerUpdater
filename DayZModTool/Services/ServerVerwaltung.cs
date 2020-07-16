using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;

namespace DayZModTool.Services
{
    public class ServerVerwaltung
    {
        private System.Timers.Timer _Timer;
        public void KillServer(int fromtimer  = 0)
        {
            // Store all running process in the system
            Process[] runingProcess = Process.GetProcesses();
            for (int i = 0; i < runingProcess.Length; i++)
            {
                // compare equivalent process by their name
                if (runingProcess[i].ProcessName == ConfigurationManager.AppSettings["ServerName"] || runingProcess[i].MainWindowTitle.Contains("DayZ Console") || runingProcess[i].MainWindowTitle.Contains(ConfigurationManager.AppSettings["ServerConsoleName"]))
                {
                    // kill  running process
                    runingProcess[i].Kill();
                }

            }
            if (fromtimer != 1)
                _Timer = null;
            else
                StartServer();
        }

        public void StartServer()
        {
            Process.Start(ConfigurationManager.AppSettings["ServerBatch"]);
            _Timer = new System.Timers.Timer
            {
                Interval = Convert.ToInt32(ConfigurationManager.AppSettings["ServerLaufzeit"])
            };
            //_Timer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["ServerLaufzeit"]);
            _Timer.Elapsed += OnTimedEvent;
            _Timer.AutoReset = true;
            _Timer.Enabled = true;
        }
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            KillServer(1);
        }

        public void BackupServer(int fromtimer = 0)
        {
            if(fromtimer > 0)
                KillServer();
            Process.Start("robocopy", $@"{ConfigurationManager.AppSettings["DayZPfad"]} {ConfigurationManager.AppSettings["ServerBackupPfad"]} /mir /r:5 /log+:{ConfigurationManager.AppSettings["ServerBackupLog"]}");
        }

        public void CleanServer(int fromtimer = 0)
        {
            if(fromtimer > 0)
                KillServer();
            Process.Start("del", "/q \"" + ConfigurationManager.AppSettings["ServerProfil"]+ "\\crash*.log\"");
            Process.Start("del", "/q \"" + ConfigurationManager.AppSettings["ServerProfil"]+ "\\DayZServer_x64_2*\"");
            Process.Start("del", "/q \"" + ConfigurationManager.AppSettings["ServerProfil"] + "\\script*.log\"");
        }

        public void UpdateServerMods()
        {
            KillServer();
            Process.Start(ConfigurationManager.AppSettings["XmlPfad"] + @"\MUpdate.bat");
        }

        public void UpdateServer()
        {
            KillServer();
            CleanServer();
            BackupServer();
            Process.Start("robocopy", $@"{ConfigurationManager.AppSettings["ServerProfil"]} {ConfigurationManager.AppSettings["ServerProfilBackup"]} /mir /r:5 /log+:{ConfigurationManager.AppSettings["ServerBackupLog"]}");
            Process.Start(ConfigurationManager.AppSettings["XmlPfad"] + @"\SUpdate.bat");
        }
    }
}
