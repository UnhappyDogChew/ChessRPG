using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChessRPGMac
{
    public class EmptyState : State
    {
        Player player;

        public EmptyState() : base()
        {
            player = Global.world.GetPlayer();
        }

        public EmptyState(List<Keys> pressedKeys) : base(pressedKeys) 
        {
            player = Global.world.GetPlayer();
        }

        public override void Update(GameTime gameTime)
        {
            KeyCheck();

            Global.world.Update(gameTime);
        }

        protected override void Prepare()
        {
            keyActions.Add(new KeyAction(Keys.Enter, pressed: () => { ChangeState(new ExploreState(pressedKeys)); }));

            player.Stand();
            base.Prepare();
        }
    }
}
