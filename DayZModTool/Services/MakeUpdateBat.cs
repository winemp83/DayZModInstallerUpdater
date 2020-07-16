using DayZModTool.Database.XmlDatabase;
using DayZModTool.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace DayZModTool.Services
{
    public class MakeUpdateBat
    {
        private readonly XmlData _DB = new XmlData();
        private ModModel _Mod;


        private string MakeRemoveDir() {
            return $@"rmdir /Q /S {ConfigurationManager.AppSettings["DayZPfad"]}\@{_Mod.ModName}";
        }
        private string MakeSteamUpdate()
        {
            return "%steamcmdpath%\\steamcmd +login %login% %pass% +\"workshop_download_item 221100 "+_Mod.ModID+"\" +quit";
        }
        private string MakeCopy() {
            return "copy \"" + ConfigurationManager.AppSettings["DayZPfad"] + $@"\@" + _Mod.ModName + "\\Keys\\*.bikey\" \""+ ConfigurationManager.AppSettings["DayZPfad"] + "\\keys\\\"";
        }
        private string MakeMove()
        {
            return "move \"" + ConfigurationManager.AppSettings["SteamPfad"] + "\\steamapps\\workshop\\content\\221100\\" + _Mod.ModID + "\" \""+$@"{ ConfigurationManager.AppSettings["DayZPfad"]}\@{ _Mod.ModName}"+"\"";
        }

        public void CreateBatch(string filename)
        {

            string fileName = filename;
            _Mod = new ModModel();
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
                sw.WriteLine("set \"steamcmdpath=" + ConfigurationManager.AppSettings["SteamPfad"] + "\"");
                sw.WriteLine("set \"login=" + ConfigurationManager.AppSettings["User"] + "\"");
                sw.WriteLine("set \"pass=" + ConfigurationManager.AppSettings["Password"] + "\"");
                sw.WriteLine("echo.");
                sw.WriteLine($@"rmdir /Q /S {ConfigurationManager.AppSettings["SteamPfad"]}" + "\\steamapps\\workshop\\content\\221100");
                sw.WriteLine("echo.");
                foreach (ModModel m in _DB.Get())
                {
                    if (m.IsUpdate == "true")
                    {
                        _Mod = m;
                        sw.WriteLine("echo.");
                        sw.WriteLine(MakeRemoveDir());
                        sw.WriteLine(MakeSteamUpdate());
                        sw.WriteLine(MakeMove());
                        sw.WriteLine(MakeCopy());
                        sw.WriteLine("echo.");
                    }
                }
                sw.WriteLine("echo \"Update Finish\"");
                sw.WriteLine("pause");
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        public void CreateServerUpdate(string filename)
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
                sw.WriteLine("set \"steamcmdpath=" + ConfigurationManager.AppSettings["SteamPfad"] + "\"");
                sw.WriteLine("set \"login=" + ConfigurationManager.AppSettings["User"] + "\"");
                sw.WriteLine("set \"pass=" + ConfigurationManager.AppSettings["Password"] + "\"");
                sw.WriteLine("set \"serverBRANCH=223350\"");
                sw.WriteLine("set \"serverPath="+ConfigurationManager.AppSettings["DayZPfad"]+"\"");
                sw.WriteLine("echo."); 
                sw.WriteLine("echo.");
                sw.WriteLine("%steamcmdpath%\\steamcmd.exe + login %login% %pass% +force_install_dir %serverPath% +\"app_update %serverBRANCH% \" validate +quit");
                sw.WriteLine("echo \"Update Finish\"");
                sw.WriteLine("pause");
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        public void MakeStartScript(string filename)
        {
            string servermod = null;
            string mod = null;
            string fileName = filename;
            _Mod = new ModModel();
            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                foreach(ModModel m in _DB.Get())
                {
                    if(m.IsActive.Equals("true") && m.IsServerMod.Equals("true")) {
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
                sw.WriteLine("set \"serverName=" + ConfigurationManager.AppSettings["ServerConsoleName"] + "\"");
                sw.WriteLine("set \"serverPort=" + ConfigurationManager.AppSettings["ServerPort"] + "\"");
                sw.WriteLine("set \"serverConfig=" + ConfigurationManager.AppSettings["ServerConfig"] + "\"");
                sw.WriteLine("set \"serverCPU=" + ConfigurationManager.AppSettings["ServerCPU"] + "\"");
                sw.WriteLine("title %serverName% batch");
                sw.WriteLine("cd \""+ConfigurationManager.AppSettings["DayZPfad"] +"\\\"");
                sw.WriteLine("echo(%time%) %serverName% started.");
                sw.WriteLine("echo \"Update Finish\"");
                sw.WriteLine("start \"DayZ Server\" /min \"DayZServer_x64.exe\" -config=%serverConfig% -port=%serverPort% -profiles=" + ConfigurationManager.AppSettings["ServerProfil"] + " -cpuCount=%serverCPU% -doLogs -adminLog -netLog -freezecheck -mod=%mods% -servermod=%servermods%");
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }
}