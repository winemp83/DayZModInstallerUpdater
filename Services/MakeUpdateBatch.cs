using Database;
using Models;
using XmlIni;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Logging;

namespace Services
{
    public class MakeBatch
    {
        private readonly ModDB _DB;
        private readonly Config _Config;

        public MakeBatch()
        {
            _Config = new Config();
            _DB = new ModDB(ref _Config.Configs);
        }
        #region Private Methods
        private string MakeRemoveDirString(Mod value)
        {
            return $@"rmdir /Q /S {_Config.GetValue("DayZPfad")}\@{value.ModName}";
        }
        private string MakeSteamUpdateString(Mod value)
        {
            return "%steamcmdpath%\\steamcmd +login %login% %pass% +\"workshop_download_item 221100 " + value.ModID + "\" +quit";
        }
        private string MakeCopyString(Mod value)
        {
            return "copy \"" + _Config.GetValue("DayZPfad") + $@"\@" + value.ModName + "\\Keys\\*.bikey\" \"" + _Config.GetValue("DayZPfad") + "\\keys\\\"";
        }
        private string MakeMoveString(Mod value)
        {
            return "move \"" + _Config.GetValue("SteamPfad") + "\\steamapps\\workshop\\content\\221100\\" + value.ModID + "\" \"" + $@"{ _Config.GetValue("DayZPfad")}\@{ value.ModName}" + "\"";
        }
        #endregion
        #region Public Methods
        public void CreateModUpdateBatch(string filename)
        {

            string fileName = filename;
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Create a new file     
                using StreamWriter sw = File.CreateText(fileName);
                sw.WriteLine("@echo off");
                sw.WriteLine("set \"steamcmdpath=" + _Config.GetValue("SteamPfad") + "\"");
                sw.WriteLine("set \"login=" + _Config.GetValue("SteamUser") + "\"");
                sw.WriteLine("set \"pass=" + _Config.GetValue("SteamPasswort") + "\"");
                sw.WriteLine("echo.");
                sw.WriteLine($@"rmdir /Q /S {_Config.GetValue("SteamPfad")}" + "\\steamapps\\workshop\\content\\221100");
                sw.WriteLine("echo.");
                foreach (Mod m in _DB.Get())
                {
                    if (m.IsUpdate == "true")
                    {
                        sw.WriteLine("echo.");
                        sw.WriteLine(MakeRemoveDirString(m));
                        sw.WriteLine(MakeSteamUpdateString(m));
                        sw.WriteLine(MakeMoveString(m));
                        sw.WriteLine(MakeCopyString(m));
                        sw.WriteLine("echo.");
                    }
                }
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            catch (Exception Ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, Ex.Message);
            }
        }
        public void CreateServerUpdateBatch(string filename)
        {
            string fileName = filename;
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Create a new file     
                using StreamWriter sw = File.CreateText(fileName);
                sw.WriteLine("@echo off");
                sw.WriteLine("@rem http://media.steampowered.com/installer/steamcmd.zip");
                sw.WriteLine("SETLOCAL ENABLEDELAYEDEXPANSION");
                sw.WriteLine("set \"steamcmdpath=" + _Config.GetValue("SteamPfad") + "\"");
                sw.WriteLine("set \"login=" + _Config.GetValue("SteamUser") + "\"");
                sw.WriteLine("set \"pass=" + _Config.GetValue("SteamPasswort") + "\"");
                sw.WriteLine("set \"serverBRANCH=223350\"");
                sw.WriteLine("set \"serverPath=" + _Config.GetValue("DayZPfad") + "\"");
                sw.WriteLine("echo.");
                sw.WriteLine("echo.");
                sw.WriteLine("%steamcmdpath%\\steamcmd.exe + login %login% %pass% +force_install_dir %serverPath% +\"app_update %serverBRANCH% \" validate +quit");
                sw.WriteLine("echo \"Update Finish\"");
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            catch (Exception Ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, Ex.Message);
            }
        }
        public void CreateStartBatch(string filename)
        {
            string servermod = null;
            string mod = null;
            string fileName = filename;
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                foreach (Mod m in _DB.Get())
                {
                    if (m.IsActive.Equals("true") && m.IsServerMod.Equals("true"))
                    {
                        if (servermod == null)
                            servermod = "set \"servermod=@" + m.ModName;
                        else
                            servermod += ";@" + m.ModName;
                    }
                    else if (m.IsActive.Equals("true") && m.IsServerMod.Equals("false"))
                    {
                        if (mod == null)
                            mod = "set \"mods=@" + m.ModName;
                        else
                            mod += ";@" + m.ModName;
                    }
                }
                servermod += "\"";
                mod += "\"";
                // Create a new file     
                using StreamWriter sw = File.CreateText(fileName);
                sw.WriteLine("@echo off");
                sw.WriteLine("SETLOCAL ENABLEDELAYEDEXPANSION");
                sw.WriteLine(servermod);
                sw.WriteLine(mod);
                sw.WriteLine("set \"serverName=" + _Config.GetValue("ServerConfigConsoleName") + "\"");
                sw.WriteLine("set \"serverPort=" + _Config.GetValue("ServerConfigPort") + "\"");
                sw.WriteLine("set \"serverConfig=" + _Config.GetValue("ServerConfigName") + "\"");
                sw.WriteLine("set \"serverCPU=" + _Config.GetValue("ServerConfigCPU") + "\"");
                sw.WriteLine("title %serverName% batch");
                sw.WriteLine("cd \"" + _Config.GetValue("DayZPfad") + "\\\"");
                sw.WriteLine("echo(%time%) %serverName% started.");
                sw.WriteLine("echo \"Update Finish\"");
                sw.WriteLine("start \"DayZ Server\" /min \"DayZServer_x64.exe\" -config=%serverConfig% -port=%serverPort% -profiles=" + _Config.GetValue("ServerConfigProfilName") + " -cpuCount=%serverCPU% -doLogs -adminLog -netLog -freezecheck -mod=%mods% -servermod=%servermods%");
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            catch (Exception Ex)
            {
                EventLog.WriteEventLog(EventTyp.Error, Ex.Message);
            }
        }
        #endregion
    }
}