using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fougerite;
using UnityEngine;

namespace Material
{
    public class Material : Fougerite.Module
    {
        public override string Name { get { return "Material"; } }
        public override string Author { get { return "TheMajical"; } }
        public override string Description { get { return "Material"; } }
        public override Version Version { get { return new Version("1.0"); } }

        public System.IO.StreamWriter file;

        public override void Initialize()
        {
            if (!File.Exists(Path.Combine(ModuleFolder, "Use.log")))
            {
                File.Create(Path.Combine(ModuleFolder, "Use.log")).Dispose();
            }
            Hooks.OnCommand += OnCommand;
        }

        public override void DeInitialize()
        {
            Hooks.OnCommand -= OnCommand;
        }

        public void Log(string pname, string ucmd)
        {
            string line = DateTime.Now + " Player " + pname + " Used " + ucmd + "" ;

            file = new System.IO.StreamWriter(Path.Combine(ModuleFolder, "Use.log"), true);
            file.WriteLine(line);
            file.Close();
        }

        public void OnCommand(Fougerite.Player player, string cmd, string[] args)
        {
            if (cmd == "mhelp")
            {
                player.MessageFrom(Name, "[color #009900]/supply - [color white]You need to have all 7 Weapon Parts in your Inv!");
                player.MessageFrom(Name, "[color #009900]/armor - [color white]You need to have all 7 Armor Parts in your Inv!");
            }
            if (cmd == "supply")
            {
                if (player.Inventory.HasItem("Weapon Part 1", 1) && player.Inventory.HasItem("Weapon Part 2", 1) && player.Inventory.HasItem("Weapon Part 3", 1) && player.Inventory.HasItem("Weapon Part 4", 1) && player.Inventory.HasItem("Weapon Part 5", 1) && player.Inventory.HasItem("Weapon Part 6", 1) && player.Inventory.HasItem("Weapon Part 7", 1))
                {
                    string pname = player.Name;
                    string ucmd = "/supply";
                    Log(pname, ucmd);
                    player.Inventory.AddItem("Supply Signal");
                    player.Inventory.RemoveItem("Weapon Part 1", 1);
                    player.Inventory.RemoveItem("Weapon Part 2", 1);
                    player.Inventory.RemoveItem("Weapon Part 3", 1);
                    player.Inventory.RemoveItem("Weapon Part 4", 1);
                    player.Inventory.RemoveItem("Weapon Part 5", 1);
                    player.Inventory.RemoveItem("Weapon Part 6", 1);
                    player.Inventory.RemoveItem("Weapon Part 7", 1);
                    player.MessageFrom(Name, "You Crafted a [color orange]Supply Signal[color white] with your Materials!");               
                }
                else
                {
                    player.MessageFrom(Name, "[color #42a1f5]You don't have [color red]required [color #42a1f5]Items in You Inventory!Check /mhelp for more Information");
                }
            }
            if (cmd == "armor")
            {
                if (player.Inventory.HasItem("Armor Part 1", 1) && player.Inventory.HasItem("Armor Part 2", 1) && player.Inventory.HasItem("Armor Part 3", 1) && player.Inventory.HasItem("Armor Part 4", 1) && player.Inventory.HasItem("Armor Part 5", 1) && player.Inventory.HasItem("Armor Part 6", 1) && player.Inventory.HasItem("Armor Part 7", 1))
                {
                    string pname = player.Name;
                    string ucmd = "/armor";
                    Log(pname, ucmd);
                    player.Inventory.AddItem("Explosive Charge");
                    player.Inventory.AddItem("Low Quality Metal",25);
                    player.Inventory.RemoveItem("Armor Part 1",1);
                    player.Inventory.RemoveItem("Armor Part 2",1);
                    player.Inventory.RemoveItem("Armor Part 3",1);
                    player.Inventory.RemoveItem("Armor Part 4",1);
                    player.Inventory.RemoveItem("Armor Part 5",1);
                    player.Inventory.RemoveItem("Armor Part 6",1);
                    player.Inventory.RemoveItem("Armor Part 7",1);
                    player.MessageFrom(Name, "You Crafted an [color orange]Attack Kit[color white] with your Materials!"); 
                   
                }
                else
                {
                    player.MessageFrom(Name, "[color #42a1f5]You don't have [color red]required [color #42a1f5]Items in You Inventory!Check /mhelp for more Information");
                }
            }
        }
    }
}
