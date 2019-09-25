using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fougerite;
using UnityEngine;

namespace AdminSys
{
    public class AdminSys : Fougerite.Module
    {
        public override string Name { get { return "AdminSys"; } }
        public override string Author { get { return "TheMajical"; } }
        public override string Description { get { return "Admin System "; } }
        public override Version Version { get { return new Version("1.2"); } }

        public System.IO.StreamWriter file;
        public bool ClansSupport = false;
        public static IniParser ClansIni;

        public override void Initialize()
        {
            Hooks.OnModulesLoaded += OnModulesLoaded;
            Hooks.OnCommand += OnCommand;
            Hooks.OnPlayerConnected += OnPlayerConnected;
            Hooks.OnPlayerHurt += OnPlayerHurt;
            Hooks.OnPlayerGathering += OnPlayerGathering;
            Hooks.OnChat += OnChat;
            Hooks.OnEntityDeployedWithPlacer += OnEntityDeployedWithPlacer;
        }

        public override void DeInitialize()
        {
            Hooks.OnModulesLoaded -= OnModulesLoaded;
            Hooks.OnCommand -= OnCommand;
            Hooks.OnPlayerConnected -= OnPlayerConnected;
            Hooks.OnPlayerHurt -= OnPlayerHurt;
            Hooks.OnPlayerGathering -= OnPlayerGathering;
            Hooks.OnChat -= OnChat;
            Hooks.OnEntityDeployedWithPlacer -= OnEntityDeployedWithPlacer;
        }

        private Fougerite.Player GetPlayerByName(string name)
        {
            name = name.ToLower();
            foreach (var x in Server.GetServer().Players)
            {
                if (x.Name.ToLower().Equals(name))
                {
                    return x;
                }
            }

            return null;
        }

        public Fougerite.Player FindSimilarPlayer(Fougerite.Player Sender, object args)
        {
            int count = 0;
            Fougerite.Player Similar = null;
            string printablename = "";
            if (args as string[] != null)
            {
                string[] array = args as string[];
                printablename = string.Join(" ", array);
                var player = GetPlayerByName(printablename);
                if (player != null)
                {
                    return player;
                }

                foreach (var x in Server.GetServer().Players)
                {
                    foreach (var namepart in array)
                    {
                        if (x.Name.ToLower().Contains(namepart.ToLower()))
                        {
                            Similar = x;
                            count++;
                        }
                    }
                }
            }
            else
            {
                string name = ((string)args).ToLower();
                printablename = name;
                var player = GetPlayerByName(name);
                if (player != null)
                {
                    return player;
                }

                foreach (var x in Server.GetServer().Players)
                {
                    if (x.Name.ToLower().Contains(name))
                    {
                        Similar = x;
                        count++;
                    }
                }
            }

            if (count == 0)
            {
                Sender.MessageFrom(Name, "Couldn't find [color#00FF00]" + printablename + "[/color]!");
            }
            else if (count > 1)
            {
                Sender.MessageFrom(Name, "Found [color#FF0000]" + count
                                                                   + "[/color] players with similar name. [color#FF0000] Use more correct name![/color]");
            }
            else
            {
                return Similar;
            }

            return null;
        }

        private static Vector3 StringToVector3(string sVector)
        {
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }
            string[] sArray = sVector.Split(',');
            Vector3 result = new Vector3(float.Parse(sArray[0]), float.Parse(sArray[1]), float.Parse(sArray[2]));
            return result;
        }

        public void Log(string logtext, string logname)
        {
            string line = "[" + DateTime.Now + "] " + logtext + " ";

            file = new System.IO.StreamWriter(Path.Combine(ModuleFolder, "" + logname + ".log"), true);
            file.WriteLine(line);
            file.Close();
        }
        //this function is from Salva "AntiBugOnSpawn"
        public string GetClanName(string TargetID)
        {
            string clanname = "XXXXXXXXXXXXX";
            if (ClansIni.ContainsSetting("ClanOwners", TargetID))
            {
                clanname = ClansIni.GetSetting("ClanOwners", TargetID);
            }
            else
            {
                if (ClansIni.ContainsSetting("ClanMembers", TargetID))
                {
                    clanname = ClansIni.GetSetting("ClanMembers", TargetID);
                }
            }
            return clanname;
        }

        //this function is from Salva "AntiBugOnSpawn"
        public void RecuperaListaDeClanes()
        {
            ClansIni = new IniParser(Directory.GetCurrentDirectory() + "\\save\\PyPlugins\\Clans\\Clans.ini");
            return;
        }

        public bool SonDeElMismoClan(string IDjugador, string ClanName)
        {
            if (ClansIni.ContainsSetting(ClanName, IDjugador))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Game Functions
        public void OnModulesLoaded()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\save\\PyPlugins\\Clans\\Clans.ini"))
            {
                ClansSupport = true;
                ClansIni = new IniParser(Directory.GetCurrentDirectory() + "\\save\\PyPlugins\\Clans\\Clans.ini");
            }
            else
            {
                ConsoleSystem.PrintError("Plugin " + Name + " " + Version.ToString() + " Integration with Clans has Failed!!");
                ClansSupport = false;
            }
        }

        private void OnChat(Fougerite.Player player, ref ChatString text)
        {
            if (player.Moderator)
            {
                if (Fougerite.Server.GetServer().HasRustPP)
                {
                    if (Fougerite.Server.GetServer().GetRustPPAPI().IsMuted(Convert.ToUInt64(player.SteamID)))
                    {
                        return;
                    }
                }
                text = text.ToString().Trim('"');
                if (text.Contains("[color]"))
                {
                    return;
                }
                Fougerite.Server.GetServer().BroadcastFrom("[MOD] " + player.Name, "[color #cc7b0a]" + text);
                text.NewText = "*%";
            }
        }

        private void OnPlayerGathering(Fougerite.Player player, Fougerite.Events.GatherEvent ge)
        {
            if (player.Inventory.InternalInventory.activeItem != null)
            {
                string toolname = player.Inventory.InternalInventory._activeItem.datablock.name;
                if (toolname == "Uber Hatchet")
                {
                    Server.GetServer().Broadcast("[color #42b3f5]Admin/Mod [color #f5e042]" + player.Name + " [color #45f542]Get Kicked [color #42b3f5]cause of Farming with a UberHatchet!");
                    player.Disconnect();
                    string logtext = player.Name + " Kicked cause of Farming with a UberHatchet!";
                    Log(logtext, "UberFarm");
                }
            }
        }

        private void OnEntityDeployedWithPlacer(Fougerite.Player player, Entity e, Fougerite.Player actualplacer)
        {
            if (e != null && !e.IsDestroyed)
            {
                if (DataStore.GetInstance().ContainsKey("AdminDuty", player.SteamID))
                {
                    e.Destroy();
                    player.Inventory.AddItem(e.Name);
                    player.InventoryNotice("1 x " + e.Name + "");
                    player.MessageFrom("AntiAbuse", "[color #42b3f5]You [color #45f542]Can't [color #42b3f5]Build while you are in Admin-Duty!");
                }
            }     
        }

        public void OnPlayerHurt(Fougerite.Events.HurtEvent he)
        {
            if (he.Attacker != null && he.Victim != null)
            {
                if (he.Attacker is Fougerite.Player && he.Victim is Fougerite.Player)
                {
                    Fougerite.Player victim = (Fougerite.Player)he.Victim;
                    Fougerite.Player attacker = (Fougerite.Player)he.Attacker;
                    if (DataStore.GetInstance().ContainsKey("AdminDuty", attacker.SteamID) && attacker != victim)
                    {
                        he.DamageAmount = 0f;
                        attacker.MessageFrom(Name, "[color #42b3f5]You [color #45f542]Can't [color #42b3f5]Damage anyone while you are in Admin-Duty!");
                        attacker.Notice("You Can't Damage anyone in Admin-Duty!");
                        victim.MessageFrom("AntiAbuse", "[color #42b3f5]Admin [color #45f542]" + attacker.Name + " [color #42b3f5]tried to [color #45f542]Damage [color #42b3f5]You in Admin-Duty!");
                    }
                }   
            }

        }
        
        public void OnCommand(Fougerite.Player player, string cmd, string[] args)
        {
            if (player.Admin || player.Moderator)
            {
                if (cmd == "ahelp")
                {
                    player.MessageFrom(Name, "[color #42b3f5]/duty /unduty /warn /delwarn /hp ");
                    player.MessageFrom(Name, "[color #42b3f5]/mtp /mtpback /ann /wipebar /wipeshelt  ");
                    player.MessageFrom(Name, "[color #42b3f5]/kick /ban /unban /rbban /rbunban /unbanip /mute /unmute  ");
                    return;
                }
                if (cmd == "duty")
                {
                    if (!DataStore.GetInstance().ContainsKey("AdminDuty", player.SteamID))
                    {
                        DataStore.GetInstance().Add("AdminDuty", player.SteamID, 0);
                        player.Inventory.ClearArmor();
                        player.Inventory.AddItemTo("Invisible Helmet", 36, 1);
                        player.Inventory.AddItemTo("Invisible Vest", 37, 1);
                        player.Inventory.AddItemTo("Invisible Pants", 38, 1);
                        player.Inventory.AddItemTo("Invisible Boots", 39, 1);
                        player.Inventory.AddItem("Uber Hatchet", 1);
                        Server.GetServer().BroadcastFrom(Name, "[color #42b3f5]Admin [color #f5e042]" + player.Name + " [color #42b3f5]is now [color #45f542]On Duty!");
                    }
                    else
                    {
                        player.MessageFrom(Name, "[color #42b3f5]You are already OnDuty!!!");
                    }
                }
                if (cmd == "unduty")
                {
                    if (DataStore.GetInstance().ContainsKey("AdminDuty", player.SteamID))
                    {
                        DataStore.GetInstance().Remove("AdminDuty", player.SteamID);
                        player.Inventory.ClearArmor();
                        player.Inventory.RemoveItem("Invisible Helmet", 1);
                        player.Inventory.RemoveItem("Invisible Vest", 1);
                        player.Inventory.RemoveItem("Invisible Pants", 1);
                        player.Inventory.RemoveItem("Invisible Boots", 1);
                        player.Inventory.RemoveItem("Uber Hatchet", 1);
                        Server.GetServer().BroadcastFrom(Name, "[color #42b3f5]Admin [color #f5e042]" + player.Name + " [color #42b3f5]is now [color #f55142]Off Duty!");
                    }
                    else
                    {
                        player.MessageFrom(Name, "[color #42b3f5]You are [color #f55142]not [color #42b3f5]On Duty!!!");
                    }
                }
                if (cmd == "warn")
                {
                    if (args.Length == 0)
                    {
                        player.MessageFrom(Name, "[color #f55142]Syntax:[color #42b3f5] /warn PlayerName");
                        return;
                    }
                    else
                    {
                        Fougerite.Player playertor = FindSimilarPlayer(player, args);
                        if (playertor == null)
                        {
                            return;
                        }

                        if (!DataStore.GetInstance().ContainsKey("warn", playertor.SteamID))
                        {
                            DataStore.GetInstance().Add("warn", playertor.SteamID, 0);
                            Server.GetServer().Broadcast("[color #42b3f5]Admin/Mod [color #f5e042]" + player.Name + "[color #42b3f5] Warned [color #f5e042] " + playertor.Name + "![color #f5e042](1/2)");
                            string logtext = player.Name + " Warned " + playertor.Name + "!(1/2)";
                            Log(logtext, "Warn");
                            return;
                        }
                        else
                        {
                            DataStore.GetInstance().Remove("warn", playertor.SteamID);
                            Server.GetServer().Broadcast("[color #42b3f5]Admin/Mod [color #f5e042]" + player.Name + "[color #42b3f5] Warned[color #f5e042] " + playertor.Name + "![color red]Kicked (2/2)");
                            string logtext = player.Name + " Warned " + playertor.Name + "!Kicked (2/2)";
                            Log(logtext, "Warn");
                            playertor.Disconnect();
                        }
                    }
                }
                if (cmd == "delwarn")
                {
                    if (args.Length == 0)
                    {
                        player.MessageFrom(Name, "[color #f55142]Syntax:[color #42b3f5] /delwarn PlayerName");
                        return;
                    }
                    else
                    {
                        Fougerite.Player playertor = FindSimilarPlayer(player, args);
                        if (playertor == null)
                        {
                            return;
                        }
                        if (DataStore.GetInstance().ContainsKey("warn", playertor.SteamID))
                        {
                            DataStore.GetInstance().Remove("warn", playertor.SteamID);
                            Server.GetServer().Broadcast("[color #42b3f5]Admin/Mod [color #f5e042]" + player.Name + " [color #42b3f5]Deleted [color #f5e042] " + playertor.Name + " [color #42b3f5]Warns![color #f5e042](0/2)");
                            string logtext = player.Name + " Deleted " + playertor.Name + " Warns!";
                            Log(logtext, "DeleteWarn");
                            return;
                        }
                        else
                        {
                            player.MessageFrom(Name, "[color #42b3f5]This Player [color #f55142]Doesn't [color #42b3f5]have any Warn!");
                            return;
                        }
                    }
                }
                if (cmd == "hp")
                {
                    if (args.Length == 0)
                    {
                        player.MessageFrom(Name, "[color #f55142]Syntax:[color #42b3f5] /hp PlayerName");
                        return;
                    }
                    else
                    {
                        Fougerite.Player playertor = FindSimilarPlayer(player, args);
                        if (playertor == null)
                        {
                            return;
                        }
                        player.MessageFrom(Name, playertor.Name + " HP : " + playertor.Health);
                        string logtext = player.Name + " Used /HP On " + playertor.Name + "";
                        Log(logtext, "HP");
                    }
                }
                if (cmd == "mtp")
                {
                    if (args.Length == 0)
                    {
                        player.MessageFrom(Name, "[color #f55142]Syntax:[color #42b3f5] /mtp PlayerName");
                        return;
                    }
                    else
                    {
                        if (DataStore.GetInstance().ContainsKey("AdminDuty", player.SteamID))
                        {
                            Fougerite.Player playertor = FindSimilarPlayer(player, args);
                            if (playertor == null)
                            {
                                return;
                            }
                            if (playertor.Moderator)
                            {
                                player.MessageFrom("AntiAbuse", "[color #42b3f5]You [color #f55142]Can't [color #42b3f5]Teleport to another Mods!");
                                return;
                            }
                            RecuperaListaDeClanes();
                            if (SonDeElMismoClan(player.SteamID, GetClanName(playertor.SteamID)))
                            {
                                player.MessageFrom("AntiAbuse", "[color #42b3f5]You [color #f55142]Can't [color #42b3f5]Teleport to your Clanmates!");
                                return;
                            }
                            if (!DataStore.GetInstance().ContainsKey("ModeratorLastLoc", player.UID))
                            {
                                DataStore.GetInstance().Add("ModeratorLastLoc", player.UID, player.Location.ToString());
                            }
                            player.TeleportTo(playertor, 1.5f, false);
                            player.Notice("You have Teleported to " + playertor.Name + "");
                            player.InventoryNotice("Checkpoint Saved!");
                            playertor.Notice("Admin " + player.Name + " Teleported to You!");
                            string logtext = player.Name + " Teleported To " + playertor.Name + " !LOC: " + playertor.X + " " + playertor.Y + " " + playertor.Z + " ";
                            Log(logtext, "Teleports");
                        }
                        else
                        {
                            player.MessageFrom(Name, "[color #42b3f5]You are [color #f55142]not [color #42b3f5]OnDuty!!!");
                        }
                        
                    }
                }
                if (cmd == "mtpback")
                {
                    if (DataStore.GetInstance().ContainsKey("ModeratorLastLoc", player.UID) && !DataStore.GetInstance().ContainsKey("HGIG", player.UID))
                    {
                        var location = StringToVector3(DataStore.GetInstance().Get("ModeratorLastLoc", player.UID).ToString());
                        player.SafeTeleportTo(location, false);
                        DataStore.GetInstance().Remove("ModeratorLastLoc", player.UID);
                        player.Notice("You Teleported Back to your Checkpoint!");
                        string logtext = "Admin " + player.Name + " Teleported to his Checkpoint!";
                        Log(logtext, "Teleports");
                    }
                    else
                    {
                        player.Notice("You Don't have any save Checkpoint or You Are in HungerGame!");
                    }
                }
                if (cmd == "ann")
                {
                    if (args.Length == 0)
                    {
                        player.MessageFrom(Name, "[color #f55142]Syntax:[color #42b3f5] /ann TEXT");
                    }
                    else
                    {
                        Server.GetServer().BroadcastNotice(string.Join(" ", args));
                        string logtext = player.Name + " /ann ==> : "  + string.Join(" ", args);
                        Log(logtext, "Announce");
                    }
                }
                if (cmd == "wipeshelt")
                {
                    int c = 0;
                    foreach (var x in World.GetWorld().Entities)
                    {
                        if (x.Name.ToLower().Contains("shelter"))
                        {
                            x.Destroy();
                            c++;
                        }
                    }
                    Server.GetServer().BroadcastFrom("AntiLag", "[color #42b3f5]Admin/Mod [color #f5e042]" + player.Name + " [color #42b3f5]Deleted [color red] " + c + " [color #42b3f5]Shelters");
                    string logtext = player.Name + " Deleted " + c + " Shelters";
                    Log(logtext, "Shelter");
                }
                if (cmd == "wipebar")
                {
                    int c = 0;
                    foreach (var x in World.GetWorld().Entities)
                    {
                        if (x.Name.ToLower().Contains("barricade"))
                        {
                            x.Destroy();
                            c++;
                        }
                    }
                    Server.GetServer().BroadcastFrom("AntiLag", "[color #42b3f5]Admin/Mod [color #f5e042]" + player.Name + " [color #42b3f5]Deleted [color red] " + c + " [color #42b3f5]Barricades");
                    string logtext = player.Name + " Deleted " + c + " Barricades";
                    Log(logtext, "Barricade");
                }
            } 
        }

        public void OnPlayerConnected(Fougerite.Player player)
        {
            if (DataStore.GetInstance().ContainsKey("AdminDuty", player.SteamID))
            {
                DataStore.GetInstance().Remove("AdminDuty", player.SteamID);
            }
        }
    }


}
