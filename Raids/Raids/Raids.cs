using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fougerite;
using UnityEngine;

namespace Raids
{
    public class Raids : Fougerite.Module
    {
        public override string Name { get { return "RaidSystem"; } }
        public override string Author { get { return "TheMajical"; } }
        public override string Description { get { return "Raid Broadcaster"; } }
        public override Version Version { get { return new Version("1.0"); } }

        public readonly Dictionary<string, Vector2> MapLocations = new Dictionary<string, Vector2>()
        {
            {"Hacker Valley South", new Vector2(5907,-1848)},
            {"Hacker Mountain South", new Vector2(5268,-1961)},
            {"Hacker Valley Middle", new Vector2(5268,-2700)},
            {"Hacker Mountain North", new Vector2(4529,-2274)},
            {"Hacker Valley North", new Vector2(4416,-2813)},
            {"Wasteland North", new Vector2(3208,-4191)},
            {"Wasteland South", new Vector2(6433,-2374)},
            {"Wasteland East", new Vector2(4942,-2061)},
            {"Wasteland West", new Vector2(3827,-5682)},
            {"Sweden", new Vector2(3677,-4617)},
            {"Everust Mountain", new Vector2(5005,-3226)},
            {"North Everust Mountain", new Vector2(4316,-3439)},
            {"South Everust Mountain", new Vector2(5907,-2700)},
            {"Metal Valley", new Vector2(6825,-3038)},
            {"Metal Mountain", new Vector2(7185,-3339)},
            {"Metal Hill", new Vector2(5055,-5256)},
            {"Resource Mountain", new Vector2(5268,-3665)},
            {"Resource Valley", new Vector2(5531,-3552)},
            {"Resource Hole", new Vector2(6942,-3502)},
            {"Resource Road", new Vector2(6659,-3527)},
            {"Beach", new Vector2(5494,-5770)},
            {"Beach Mountain", new Vector2(5108,-5875)},
            {"Coast Valley", new Vector2(5501,-5286)},
            {"Coast Mountain", new Vector2(5750,-4677)},
            {"Coast Resource", new Vector2(6120,-4930)},
            {"Secret Mountain", new Vector2(6709,-4730)},
            {"Secret Valley", new Vector2(7085,-4617)},
            {"Factory Radtown", new Vector2(6446,-4667)},
            {"Small Radtown", new Vector2(6120,-3452)},
            {"Big Radtown", new Vector2(5218,-4800)},
            {"Hangar", new Vector2(6809,-4304)},
            {"Tanks", new Vector2(6859,-3865)},
            {"Civilian Forest", new Vector2(6659,-4028)},
            {"Civilian Mountain", new Vector2(6346,-4028)},
            {"Civilian Road", new Vector2(6120,-4404)},
            {"Ballzack Mountain", new Vector2(4316,-5682)},
            {"Ballzack Valley", new Vector2(4720,-5660)},
            {"Spain Valley", new Vector2(4742,-5143)},
            {"Portugal Mountain", new Vector2(4203,-4570)},
            {"Portugal", new Vector2(4579,-4637)},
            {"Lone Tree Mountain", new Vector2(4842,-4354)},
            {"Forest", new Vector2(5368,-4434)},
            {"Rad-Town Valley", new Vector2(5907,-3400)},
            {"Next Valley", new Vector2(4955,-3900)},
            {"Silk Valley", new Vector2(5674,-4048)},
            {"French Valley", new Vector2(5995,-3978)},
            {"Ecko Valley", new Vector2(7085,-3815)},
            {"Ecko Mountain", new Vector2(7348,-4100)},
            {"Zombie Hill", new Vector2(6396,-3428)}
        };

        public override void Initialize()
        {
            Hooks.OnCommand += OnCommand;
            Hooks.OnEntityDestroyed += OnEntityDestroyed;
        }

        public override void DeInitialize()
        {
            Hooks.OnCommand -= OnCommand;
            Hooks.OnEntityDestroyed -= OnEntityDestroyed;
        }

        private string CalcPosition(Vector3 v)
        {
            float closest = float.MaxValue;
            Vector2 pos = new Vector2(v.x, v.z);
            string name = "Unknown";
            foreach (var x in MapLocations)
            {
                float cdist = Vector2.Distance(x.Value, pos);
                if (cdist < closest)
                {
                    closest = cdist;
                    name = x.Key;
                }
            }

            return name;
        }

        private void OnCommand(Player player, string cmd, string[] args)
        {
            if (cmd == "location")
            {
                Vector3 v = player.Location;
                string pos = CalcPosition(v);
                player.MessageFrom("GPS", "You are near[color #00ff7b]: " + pos);
            }
        }

        private void OnEntityDestroyed(Fougerite.Events.DestroyEvent de)
        {
            if (de.Attacker is Fougerite.Player && de.Entity is Fougerite.Entity && !de.IsDecay && de.DamageType != null)
            {
                Fougerite.Player Raider = (Fougerite.Player)de.Attacker;
                Vector3 v = Raider.Location;
                string pos = CalcPosition(v);
                Server.GetServer().BroadcastFrom("Raid NEWS", "[color red]" + Raider.Name + "[color white] Is Raiding [color green]" + de.Entity.OwnerName + "[color white] House with [color orange]" + de.WeaponName + "[color white]!Area: [color #00ff7b]" + pos);
            }
        }
    }
}
