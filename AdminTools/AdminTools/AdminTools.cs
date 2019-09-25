using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fougerite;
using UnityEngine;

namespace AdminTools
{
    public class AdminsTools : Fougerite.Module
    {
        public override string Name { get { return "AdminTools"; } }
        public override string Author { get { return "TheMajical"; } }
        public override string Description { get { return "ServerLogs"; } }
        public override Version Version { get { return new Version("1.0"); } }

        public System.IO.StreamWriter file;

        public override void Initialize()
        {
            Hooks.OnCommand += OnCommand;
            Hooks.OnEntityHurt += OnEntityHurt;
            Hooks.OnItemRemoved += OnItemRemoved;
            Hooks.OnEntityDestroyed += OnEntityDestroyed;
            Hooks.OnServerLoaded += OnServerLoaded;
        }

        public override void DeInitialize()
        {
            Hooks.OnCommand -= OnCommand;
            Hooks.OnEntityHurt -= OnEntityHurt;
            Hooks.OnItemRemoved -= OnItemRemoved;
            Hooks.OnEntityDestroyed -= OnEntityDestroyed;
            Hooks.OnServerLoaded -= OnServerLoaded;
        }

        public void Log(string logtext, string logname)
        {
            string line = "[" + DateTime.Now + "] " + logtext + " ";

            file = new System.IO.StreamWriter(Path.Combine(ModuleFolder, "" + logname + ".log"), true);
            file.WriteLine(line);
            file.Close();
        }

        private void OnServerLoaded()
        {
            DataStore.GetInstance().Flush("OwnerMode");
            DataStore.GetInstance().Flush("OwnerSave");
        }

        private void OnEntityDestroyed(Fougerite.Events.DestroyEvent de)
        {
            if (de.Attacker is Fougerite.Player && de.Entity is Fougerite.Entity && !de.IsDecay && de.DamageType != null)
            {
                Fougerite.Player Raider = (Fougerite.Player)de.Attacker;
                if (de.WeaponName == "Explosive Charge")
                {
                    string logtext = " Raider: " + Raider.Name + "| " + Raider.SteamID + " | " + de.Entity.Name + " of " + de.Entity.OwnerName + " | " + de.Entity.OwnerID + " || LOC: " + de.Entity.X + " " + de.Entity.Y + " " + de.Entity.Z + "";
                    Log(logtext, "C4");
                    return;
                }
                if (de.WeaponName == "F1 Grenade")
                {
                    string logtext = " Raider: " + Raider.Name + "| " + Raider.SteamID + " | " + de.Entity.Name + " of " + de.Entity.OwnerName + " | " + de.Entity.OwnerID + " || LOC: " + de.Entity.X + " " + de.Entity.Y + " " + de.Entity.Z + "";
                    Log(logtext, "Grenade");
                    return;
                }
            }
        }

        private void OnItemRemoved(Fougerite.Events.InventoryModEvent e)
        {
            string en = e.Inventory.name;
            if (en.ToLower().Contains("woodbox") || en.ToLower().Contains("stash"))
            {
                if (e.Inventory != null && e.Player != null)
                {
                    string n = e.Player.Name;
                    string d = e.Player.SteamID;
                    Vector3 loc = e.Player.Location;
                    Vector3 el = e.Inventory.transform.position;
                    string inn = e.ItemName;
                    int q = e.InventoryItem.uses;
                    string logtext = "[Box-Remove]: " + n + "|" + d + "|" + en + "| C: " + el + "| P: " + loc + " Item: " + q + "x " + inn + "";
                    Log(logtext, "BoxRemove");
                }
            }
        }

        private void OnEntityHurt(Fougerite.Events.HurtEvent he)
        {
            if (he.AttackerIsPlayer && !he.IsDecay)
            {
                Fougerite.Player attacker = (Fougerite.Player)he.Attacker;
                if (DataStore.GetInstance().ContainsKey("OwnerSave", attacker.SteamID))
                {
                    if (he.WeaponName == "Shotgun")
                    {
                        attacker.MessageFrom(Name, "You Can't use Shotgun in this mode!");
                        return;
                    }
                    attacker.Notice("Entity Loc + Owner Saved!");
                    string logtext = " " + he.Entity.Name + " Owner: " + he.Entity.OwnerName + " (" + he.Entity.OwnerID + ") | X: " + he.Entity.X + " Y: " + he.Entity.Y + " Z: " + he.Entity.Z + " Saved by " + attacker.Name + "";
                    Log(logtext, "SavedOwners");
                    return;
                }
                if (DataStore.GetInstance().ContainsKey("OwnerMode", attacker.SteamID))
                {
                    if (he.WeaponName == "Shotgun")
                    {
                        attacker.MessageFrom(Name, "You Can't use Shotgun in this mode!");
                        return;
                    }
                    attacker.Notice(he.Entity.Name + "Is Owned by " + he.Entity.OwnerName);
                    attacker.MessageFrom(Name, "[color #42f5b3]Owner: [color #f5e042]" + he.Entity.OwnerName + " [color #42f5b3]" + he.Entity.OwnerID);
                    return;
                }
            }
        }

        private void OnCommand(Fougerite.Player player, string cmd, string[] args)
        {
            if (cmd == "owner")
            {
                if (player.Admin)
                {
                    if (DataStore.GetInstance().ContainsKey("OwnerMode", player.SteamID))
                    {
                        player.MessageFrom(Name, "---[color #42f5b3]OwnerMode[color white]---");
                        player.MessageFrom(Name, "[color #42f5b3]You are no longer in OwnerMode!");
                        DataStore.GetInstance().Remove("OwnerMode", player.SteamID);
                    }
                    else
                    {
                        DataStore.GetInstance().Add("OwnerMode", player.SteamID, 0);
                        player.MessageFrom(Name, "---[color #42f5b3]OwnerMode[color white]---");
                        player.MessageFrom(Name, "[color #f5e042]OwnerMode Activated!");
                        player.MessageFrom(Name, "[color #f5e042]Make sure to use /owner again when you finished your job!");
                        player.MessageFrom(Name, "[color #f5e042]You Can't use Shotgun in this mode!");
                    }
                }
                else
                {
                    player.MessageFrom(Name, "You are not an Admin!");
                }
            }
            if (cmd == "osave")
            {
                if (player.Admin || player.Moderator)
                {
                    if (DataStore.GetInstance().ContainsKey("OwnerSave", player.SteamID))
                    {
                        player.MessageFrom(Name, "---[color #42f5b3]OwnerSave[color white]---");
                        player.MessageFrom(Name, "[color #42f5b3]You are no longer in OwnerSave!");
                        DataStore.GetInstance().Remove("OwnerSave", player.SteamID);
                    }
                    else
                    {
                        DataStore.GetInstance().Add("OwnerSave", player.SteamID, 0);
                        player.MessageFrom(Name, "---[color #42f5b3]OwnerSave[color white]---");
                        player.MessageFrom(Name, "[color #f5e042]OwnerSaveMode Activated!");
                        player.MessageFrom(Name, "[color #f5e042]Make sure to use /osave again when you finished your job!");
                        player.MessageFrom(Name, "[color #f5e042]You Can't use Shotgun in this mode!");
                    }
                }
            }
        }


    }
}
