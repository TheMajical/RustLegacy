using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fougerite;
using UnityEngine;
using DiscordWebHooks;

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
            string line = DateTime.Now + " Player " + pname + " Az " + ucmd + " Estefade Kard!" ;

            file = new System.IO.StreamWriter(Path.Combine(ModuleFolder, "Use.log"), true);
            file.WriteLine(line);
            file.Close();
        }

        public void OnCommand(Fougerite.Player player, string cmd, string[] args)
        {
            if (cmd == "mhelp")
            {
                player.MessageFrom(Name, "[color #009900]/supply - [color white]Agar Shoma Har 7 Teke Weapon Part Ra Dara Hastid!");
                player.MessageFrom(Name, "[color #009900]/armor - [color white]Agar Shoma Har 7 Teke Armor Part Ra Dara Hastid!");
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
                    player.MessageFrom(Name, "Shoma Ba Material Hayetan Yek [color orange]Supply Signal[color white] Sakhtid!");
                    DiscordWebHooks.API.Send(false, "Player `" + player.Name + "` Ba Estefade Az /supply, `Supply Signal` Bedast Avard!", "Ray", "https://discordapp.com/api/webhooks/556172192417841154/RqWVzLlSh5tF4WHYtQ_zIpesc9Jwlm2wNphSueZq-bS-f9nGkDOOGO-gFLnJqf8MMxtQ");
                }
                else
                {
                    player.MessageFrom(Name, "[color #42a1f5]Materiale Jam Avari Shode Baraye Sakhte Kite Armor [color red]Kafi Nist!");
                    player.MessageFrom(Name, "[color #42a1f5]Shoma Niaz Darid Har [color #4257f5]7ta Weapon Part [color #42a1f5]Ro Dashte Bashid!");
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
                    //Logger.Log(player.Name + " Az /armor Ba Moafaghiat Estefade Kard!");
                    player.MessageFrom(Name, "Shoma Ba Material Hayetan Yek [color orange]Kite Armor[color white] Sakhtid!");
                    DiscordWebHooks.API.Send(false, "Player `" + player.Name + "` Ba Estefade Az /armor, `1x C4 + 25x Low Quality Metal` Bedast Avard!", "Ray", "https://discordapp.com/api/webhooks/556172192417841154/RqWVzLlSh5tF4WHYtQ_zIpesc9Jwlm2wNphSueZq-bS-f9nGkDOOGO-gFLnJqf8MMxtQ");
                }
                else
                {
                    player.MessageFrom(Name, "[color #42a1f5]Materiale Jam Avari Shode Baraye Sakhte Kite Armor [color red]Kafi Nist!");
                    player.MessageFrom(Name, "[color #42a1f5]Shoma Niaz Darid Har [color #4257f5]7ta Armor Part [color #42a1f5]Ro Dashte Bashid!");
                }
            }
            if (cmd == "matkits")
            {
                if (player.Admin)
                {
                    player.Inventory.AddItem("Weapon Part 1", 1);
                    player.Inventory.AddItem("Weapon Part 2", 1);
                    player.Inventory.AddItem("Weapon Part 3", 1);
                    player.Inventory.AddItem("Weapon Part 4", 1);
                    player.Inventory.AddItem("Weapon Part 5", 1);
                    player.Inventory.AddItem("Weapon Part 6", 1);
                    player.Inventory.AddItem("Weapon Part 7", 1);
                    player.Inventory.AddItem("Weapon Part 8", 1);
                    player.Inventory.AddItem("Armor Part 1", 1);
                    player.Inventory.AddItem("Armor Part 2", 1);
                    player.Inventory.AddItem("Armor Part 3", 1);
                    player.Inventory.AddItem("Armor Part 4", 1);
                    player.Inventory.AddItem("Armor Part 5", 1);
                    player.Inventory.AddItem("Armor Part 6", 1);
                    player.Inventory.AddItem("Armor Part 7", 1);
                }  
            }
        }
    }
}
