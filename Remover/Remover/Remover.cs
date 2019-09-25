using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Facepunch.ID;
using Fougerite;
using UnityEngine;
using RustPP.Social;

namespace Remover
{
    public class Remover : Fougerite.Module
    {
        public override string Name { get { return "Remover"; } }
        public override string Author { get { return "TheMajical"; } }
        public override string Description { get { return "RemoveTool"; } }
        public override Version Version { get { return new Version("1.5"); } }

        public bool RustPPSupport = false;
        public Dictionary<string, string> EntityList = new Dictionary<string, string>()
        {
            {"WoodFoundation", "Wood Foundation"},
            {"WoodDoorFrame", "Wood Doorway"},
            {"WoodDoor", "Wooden Door"},
            {"WoodPillar", "Wood Pillar"},
            {"WoodWall", "Wood Wall"},
            {"WoodCeiling", "Wood Ceiling"},
            {"WoodWindowFrame", "Wood Window"},
            {"WoodStairs", "Wood Stairs"},
            {"WoodRamp", "Wood Ramp"},
            {"WoodSpikeWall", "Spike Wall"},
            {"LargeWoodSpikeWall", "Large Spike Wall"},
            {"WoodBox", "Wood Storage Box"},
            {"WoodBoxLarge", "Large Wood Storage"},
            {"WoodGate", "Wood Gate"},
            {"WoodGateway", "Wood Gateway"},
            {"WoodenDoor", "Wooden Door"},
            {"Wood_Shelter", "Wood Shelter"},
            {"MetalWall", "Metal Wall"},
            {"MetalCeiling", "Metal Ceiling"},
            {"MetalDoorFrame", "Metal Doorway"},
            {"MetalPillar", "Metal Pillar"},
            {"MetalFoundation", "Metal Foundation"},
            {"MetalStairs", "Metal Stairs"},
            {"MetalRamp", "Metal Ramp"},
            {"MetalWindowFrame", "Metal Window"},
            {"MetalDoor", "Metal Door"},
            {"SmallStash", "Small Stash"},
            {"Campfire", "Camp fire"},
            {"Furnace", "Furnace"},
            {"Workbench", "Workbench"},
            {"Wood Barricade", "Wood Barricade"},
            {"RepairBench", "Repair Bench"},
            {"SleepingBagA", "Sleeping Bag"},
            {"SingleBed", "Bed"}
        };

        private RemoveTE CreateParallelTimer(int timeoutDelay, Dictionary<string, object> args)
        {
            RemoveTE timedEvent = new RemoveTE(timeoutDelay);
            timedEvent.Args = args;
            timedEvent.OnFire += Callback;
            return timedEvent;
        }

        private void Callback(RemoveTE e)
        {
            e.Kill();
            Dictionary<string, object> Data = e.Args;

            Fougerite.Player PlayerF = (Fougerite.Player) Data["Player"];
            if (PlayerF.IsOnline)
            {
                DataStore.GetInstance().Remove("RemoveTool", PlayerF.SteamID);
                PlayerF.MessageFrom(Name, "RemoveTool is now [color #fc0313] Disable!");
            }
        }

        public override void Initialize()
        {
            Hooks.OnCommand += OnCommand;
            Hooks.OnEntityHurt += OnEntityHurt;
            Hooks.OnModulesLoaded += OnModulesLoaded;
            Hooks.OnPlayerConnected += OnPlayerConnected;

            
        }

        public override void DeInitialize()
        {
            Hooks.OnCommand -= OnCommand;
            Hooks.OnEntityHurt -= OnEntityHurt;
            Hooks.OnModulesLoaded -= OnModulesLoaded;
            Hooks.OnPlayerConnected -= OnPlayerConnected;
        }

        public bool IsNotEligible(Fougerite.Entity en)
        {
            if (en.IsDeployableObject())
            {
                return false;
            }
            StructureComponent comp = (StructureComponent)en.GetObject<StructureComponent>();
            return comp._master.ComponentCarryingWeight((StructureComponent)en.Object);
        }

        public bool IsShared(ulong id, ulong id2)
        {
            System.Collections.ArrayList list = (System.Collections.ArrayList)Fougerite.Server.GetServer().GetRustPPAPI().GetShareCommand.GetSharedDoors()[id];
            if (list == null)
            {
                return false;
            }
            else
            {
                if (list.Contains(id2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void OnModulesLoaded()
        {
            Logger.Log("");
            if (Fougerite.Server.GetServer().HasRustPP)
            {
                RustPPSupport = true;
                Logger.Log("Plugin " + Name + " " + Version.ToString() + " Ba Rust++ OKaye!!");
            }
            else
            {
                Logger.LogError("WARNING ,Plugin " + Name + " " + Version.ToString() + " DISABLED, Rust++ Peyda Nashod!Ye Jaye Kar Ride Dadach!");
                RustPPSupport = false;
            }
            Logger.Log("");
        }

        private void OnPlayerConnected(Fougerite.Player player)
        {
            if (DataStore.GetInstance().ContainsKey("RemoveTool", player.SteamID))
            {
                DataStore.GetInstance().Remove("RemoveTool", player.SteamID);
            }
        }

        private void OnEntityHurt(Fougerite.Events.HurtEvent he)
        {
            if (he.AttackerIsPlayer && !he.IsDecay)
            {
                Fougerite.Player attacker = (Fougerite.Player)he.Attacker;
                
                ulong id = he.Entity.UOwnerID;
                ulong id2 = attacker.UID;

                if (DataStore.GetInstance().ContainsKey("RemoveTool", attacker.SteamID))
                {
                    if (he.WeaponName == "Shotgun")
                    {
                        attacker.MessageFrom(Name, "[color #fc0313]You Can't Use Shotgun!");
                        return;
                    }
                    if (he.Entity.Name == "MetalBarsWindow")
                    {
                        attacker.MessageFrom(Name, "You [color #fc0313]Can't[color white] Remove Metal Window Bars!");
                        return;
                    }
                    if (IsShared(id, id2) || id == id2)
                    {
                        if (!IsNotEligible(he.Entity))
                        {
                            he.Entity.Destroy();
                            attacker.Inventory.AddItem(EntityList[he.Entity.Name]);
                            attacker.InventoryNotice("+1 " + EntityList[he.Entity.Name]);
                        }
                        else
                        {
                            attacker.MessageFrom(Name, "[color #fc0313]Can't Remove this cause there is some Pillars/Ceilings on it!");
                        }
                        
                    }
                    else
                    {
                        attacker.MessageFrom(Name, "This " + he.Entity.Name + " Owner [color #fc0313]has not[color white] Shared you!");
                    }

                }
            }
        }

        private void OnCommand(Fougerite.Player player, string cmd, string[] args)
        {
            if (cmd == "remove")
            {
                if (DataStore.GetInstance().ContainsKey("RemoveTool", player.SteamID))
                {
                    player.MessageFrom(Name, "---[color #42f5b3]RemoveTool[color white]---");
                    player.MessageFrom(Name, "[color #42f5b3]Shoma Az RemoveTool Kharej Shodid!");
                    DataStore.GetInstance().Remove("RemoveTool", player.SteamID);
                }
                else
                {
                    DataStore.GetInstance().Add("RemoveTool", player.SteamID, 0);
                    player.MessageFrom(Name, "---[color #42f5b3]RemoveTool[color white]---");
                    player.MessageFrom(Name, "[color #f5e042]Shoma Aknun Dar RemoveTool Hastid!");
                    player.MessageFrom(Name, "[color #f5e042]Har Moghe Karetun Tamum Shod Hatman Ba / Az OwnerMode Kharej Shid!");
                    player.MessageFrom(Name, "[color #f5e042]Estefade Az Shotgun Baraye Remove Mojaz Nist!");
                    Dictionary<string, object> Data = new Dictionary<string, object>();
                    Data["Player"] = player;
                    CreateParallelTimer(30000, Data).Start();
                }
            }
            if (cmd == "sharelist")
            {
                System.Collections.ArrayList list = (System.Collections.ArrayList)Fougerite.Server.GetServer().GetRustPPAPI().GetShareCommand.GetSharedDoors()[player.UID];
                if (list == null)
                {
                    player.MessageFrom("ShareList", "Shoma Kesi Ro Share [color #fc0313]Nakardid!");
                    return;
                }
                else
                {
                    player.MessageFrom("ShareList", "---[color purple]ShareList[color white]---");
                    foreach (ulong id in list)
                    {
                        Fougerite.Player client = Fougerite.Server.GetServer().FindPlayer(id.ToString());
                        if (client != null)
                        {
                            player.MessageFrom("ShareList", "" + client.Name + "  | " + id + "");
                        }
                        else
                        {
                            player.MessageFrom("ShareList", "" + id + "");
                        }
                    }
                    player.MessageFrom("ShareList", "---[color purple]ShareList[color white]---"); 
                }
            }
        }
    }    
}
