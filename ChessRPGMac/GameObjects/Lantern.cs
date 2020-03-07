using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChessRPGMac
{
    public class Lantern : InteractableObject
    {
        public Lantern(int x, int y) : base(x, y)
        {
            sprite = Global.spriteBox.Pick("Lantern");
            collider = new SquareCollider(x, y, 14, 7, 6, 3);
            interactionCollider = new SquareCollider(x, y, 18, 9, 8, 4, false);
        }

        public override void Update(GameTime gameTime) { }

        public override void Interact()
        {
            interactionMethod?.Invoke(new LanternInteraction(this));
        }

        public void Finish()
        {
            Finished = true;
        }
    }

    public class LanternInteraction : Interaction
    {
        Player player;
        Lantern lantern;

        public LanternInteraction(Lantern caller)
        {
            player = Global.world.GetPlayer();
            lantern = caller;
            List<string> textList = new List<string>();

            textList.Add("You found a lantern.<w>");
            textList.Add("This is another text for debug. <n><r:10>what is the problem?<w>");

            InteractionTextbox = new Textbox("Textbox", null, 0, 100, textList, Global.content.Load<SpriteFont>("neodgm12"));
        }

        public override void Finish()
        {
            player.HoldLantern();
            lantern.Finish();
            base.Finish();
        }
    }
}
