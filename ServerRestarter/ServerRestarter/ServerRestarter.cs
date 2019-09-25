using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Fougerite;
using DiscordWebHooks;
using UnityEngine;

namespace ServerRestarter
{
    public class ServerRestarter : Fougerite.Module
    {
        public Timer RestartTimer;
        public Timer AlertTimer;
        public Timer KickTimer;
        public Timer KDTimer;
        public int RestartTime = 7200000;
        public int AlertTime = 7080000;
        public int KickTime = 7190000;
        public int KDTime = 60000;
        public override string Name { get { return "ServerRestarter"; } }
        public override string Author { get { return "TheMajical"; } }
        public override string Description { get { return "AutoRestart "; } }
        public override Version Version { get { return new Version("1.1"); } }

        public System.IO.StreamWriter file;

        public override void Initialize()
        {
            if (!File.Exists(Path.Combine(ModuleFolder, "Restarts.log")))
            {
                File.Create(Path.Combine(ModuleFolder, "Restarts.log")).Dispose();
            }
            RestartTimer = new Timer(RestartTime);
            RestartTimer.Elapsed += RestartCall;
            RestartTimer.Start();
            AlertTimer = new Timer(AlertTime);
            AlertTimer.Elapsed += AlertCall;
            AlertTimer.Start();
            KickTimer = new Timer(KickTime);
            KickTimer.Elapsed += KickCall;
            KickTimer.Start();
            KDTimer = new Timer(KDTime);
            KDTimer.Elapsed += KDTimerCall;
            Hooks.OnCommand += OnCommand;
        }

        public override void DeInitialize()
        {
            if (RestartTimer != null)
            {
                RestartTimer.Dispose();
            }
            if (AlertTimer != null)
            {
                AlertTimer.Dispose();
            }
            if (KickTimer != null)
            {
                KickTimer.Dispose();
            }
            if (KDTimer != null)
            {
                KDTimer.Dispose();
            }

            Hooks.OnCommand -= OnCommand;
        }

        public void Log(string pname)
        {
            string line = DateTime.Now + " " + pname + " Server ro Restart Kard!";

            file = new System.IO.StreamWriter(Path.Combine(ModuleFolder, "Restarts.log"), true);
            file.WriteLine(line);
            file.Close();
        }

        private void RestartCall(object sender, ElapsedEventArgs e)
        {
            RestartTimer.Dispose();
            string pname = "Plugin";
            Log(pname);
            Server.GetServer().RunServerCommand("quit");
        }

        private void AlertCall(object sender, ElapsedEventArgs e)
        {
            AlertTimer.Dispose();
            Server.GetServer().BroadcastNotice("Server Restarting in 2 Minute!!!");
            Server.GetServer().Broadcast("Server [color #82009c]2 Daghighe [color white]Digar Be Surate Automatic Restart Mishavad!");
        }

        private void KickCall(object sender, ElapsedEventArgs e)
        {
            KickTimer.Dispose();
            Server.GetServer().BroadcastNotice("Server Restarting NOW!!!");
            Server.GetServer().Broadcast("Server Aknun [color #82009c]Restart [color white]Mishavad!");
            DiscordWebHooks.API.Send(false, "Server Restarted Automatically!Next AutoRestart in 2 Hours!", "Ray", "https://discordapp.com/api/webhooks/556172192417841154/RqWVzLlSh5tF4WHYtQ_zIpesc9Jwlm2wNphSueZq-bS-f9nGkDOOGO-gFLnJqf8MMxtQ");
            foreach (var play in Server.GetServer().Players)
            {
                play.Disconnect();
            }
        }

        private void KDTimerCall(object sender, ElapsedEventArgs e)
        {
            Server.GetServer().BroadcastNotice("Server Restarting NOW!!!");
            Server.GetServer().Broadcast("Server Aknun [color #82009c]Restart [color white]Mishavad!");
            foreach (var play in Server.GetServer().Players)
            {
                play.Disconnect();
            }
            Server.GetServer().RunServerCommand("quit");          
        }

        private void OnCommand(Fougerite.Player player, string cmd, string[] args)
        {
            if (cmd == "svres")
            {
                if (player.Admin || (player.Moderator))
                {
                    DiscordWebHooks.API.Send(false, "Admin " + player.Name + " Restarted Server!Next AutoRestart in 2 Hours!", "Ray", "https://discordapp.com/api/webhooks/556172192417841154/RqWVzLlSh5tF4WHYtQ_zIpesc9Jwlm2wNphSueZq-bS-f9nGkDOOGO-gFLnJqf8MMxtQ");
                    RestartTimer.Dispose();
                    AlertTimer.Dispose();
                    KickTimer.Dispose();
                    KDTimer.Start();
                    Server.GetServer().BroadcastNotice("Server Restarting in 60 Seconds!!!");
                    Server.GetServer().Broadcast("Server [color #82009c]60 Sanie [color white]Digar Be Daste Admin [color #82009c]" + player.Name + " [color white]Restart Mishavad!");
                    Logger.LogError("[AutoRestart] " + player.Name + " Server Ra Restart Kard!");
                    string pname = player.Name;
                    Log(pname);
                }
                else
                {
                    player.Message("[color red]Faghat Adminha Mitavanand Server Ra Restart Konanad!");
                }
                
            }
        }
    }
}
