using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Fougerite;
using UnityEngine;
using Random = System.Random;

namespace Payday
{
    public class Payday : Fougerite.Module
    {
        public Timer PaydayTimer;
        public int PDTime = 35 * 60000;
        public override string Name { get { return "Payday"; } }
        public override string Author { get { return "TheMajical"; } }
        public override string Description { get { return "Give Some Rewards to Online Players "; } }
        public override Version Version { get { return new Version("1.0"); } }

        public readonly Random Randomizer = new Random();
        public readonly Dictionary<int, string> RewardList = new Dictionary<int, string>()
        {
            {1, "Revolver"},
            {2, "Hatchet"},
            {3, "Armor Part 1"},
            {4, "Pick Axe"},
            {5, "Granola Bar"},
            {6, "Flare"},
            {7, "Weapon Part 4"},
            {8, "Leather Helmet"},
            {9, "F1 Grenade"},
            {10, "Wood Shelter"},
            {11, "Leather Pants"},
            {12, "Small Water Bottle"},
            {13, "Large Medkit"},
            {14, "Weapon Part 7"},
            {15, "Small Medkit"},
            {16, "Armor Part 5"},
            {17, "Wood Shelter"},
            {18, "Handmade Lockpick"},
            {19, "Weapon Part 1"},
            {20, "F1 Grenade"}
        };

        public override void Initialize()
        {
            PaydayTimer = new Timer(PDTime);
            PaydayTimer.Elapsed += PaydayCall;
            PaydayTimer.Start();
        }

        public override void DeInitialize()
        {
            if (PaydayTimer != null)
            {
                PaydayTimer.Dispose();
            }
        }

        private void PaydayCall(object sender, ElapsedEventArgs e)
        {
            int random = Randomizer.Next(1, 21);
            foreach (var play in Server.GetServer().Players)
            {
                play.Inventory.AddItem(RewardList[random]);
                play.MessageFrom(Name, "[color #FF66FF]━━━━━━━━━━━━━ Payday ━━━━━━━━━━━━━━");
                play.MessageFrom(Name, "You have a new [color #42f5b3]" + RewardList[random] + "[color white] in your Inventory![color #42f5b3]Enjoy Playing ^__^");
                play.MessageFrom(Name, "[color #FF66FF]━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");   
            }
        }

    }
}
