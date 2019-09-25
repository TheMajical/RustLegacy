using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fougerite;

namespace FPS
{
    public class FPS : Fougerite.Module
    {
        public override string Name { get { return "FPS"; } }
        public override string Author { get { return "TheMajical"; } }
        public override string Description { get { return "Improve your FPS"; } }
        public override Version Version { get { return new Version("1.0"); } }

        public override void Initialize()
        {
            Hooks.OnCommand += OnCommand;
            Hooks.OnPlayerConnected += OnPlayerConnected;
        }

        public override void DeInitialize()
        {
            Hooks.OnCommand -= OnCommand;
            Hooks.OnPlayerConnected -= OnPlayerConnected;
        }

        public void OnPlayerConnected(Fougerite.Player pl)
        {
            pl.MessageFrom(Name, "Use [color #e0b0b0]/fps[color white] to increase your FPS!");
        }

        public void OnCommand(Fougerite.Player player, string cmd, string[] args)
        {
            if (cmd == "fps")
            {
                player.SendCommand("grass.on false");
                player.SendCommand("grass.forceredraw False");
                player.SendCommand("grass.displacement True");
                player.SendCommand("grass.disp_trail_seconds 0");
                player.SendCommand("grass.shadowcast False");
                player.SendCommand("grass.shadowreceive False");
                player.SendCommand("render.level 0");
                player.SendCommand("render.vsync False");
                player.SendCommand("footsteps.quality 2");
                player.SendCommand("gfx.grain False");
                player.SendCommand("gfx.ssao False");
                player.SendCommand("gfx.shafts false");
                player.SendCommand("gfx.damage false");
                player.SendCommand("gfx.ssaa False");
                player.SendCommand("gfx.bloom False");
                player.SendCommand("gfx.tonemap False");
                player.MessageFrom(Name, "[color #c8102e]You Switched to FPS Mode!");
            }
            else if (cmd == "graph")
            {
                player.SendCommand("grass.on true");
                player.SendCommand("grass.forceredraw true");
                player.SendCommand("grass.displacement false");
                player.SendCommand("grass.disp_trail_seconds 10");
                player.SendCommand("grass.shadowcast true");
                player.SendCommand("grass.shadowreceive true");
                player.SendCommand("render.level 1");
                player.SendCommand("render.vsync true");
                player.SendCommand("footsteps.quality 2");
                player.SendCommand("gfx.grain true");
                player.SendCommand("gfx.ssao true");
                player.SendCommand("gfx.shafts true");
                player.SendCommand("gfx.damage true");
                player.SendCommand("gfx.tonemap true");
                player.SendCommand("gfx.ssaa true");
                player.SendCommand("gfx.bloom true");
                player.MessageFrom(Name, "[color #88b04b]You Switched to Graph Mode!");
            }
        }
    }
}
