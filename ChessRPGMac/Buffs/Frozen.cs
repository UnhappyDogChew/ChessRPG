using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class Frozen : Buff
    {
        FrozenEffect effect;

        public Frozen()
        {
            start_implemented = true;
            isNegative = true;
        }

        public override void Update(GameTime gameTime)
        {
            target.fighter.paused = true;
            base.Update(gameTime);
        }

        public override void Start(ActionFinishHandler handler)
        {
            effect = new FrozenEffect(target.x, target.y, 0);
            bottomEffectLayer.elements.Add(effect);
            target.fighter.paused = true;
            handler(null);
            base.Start(handler);
        }

        public override void Finish(ActionFinishHandler handler)
        {
            effect.Finish();
            target.fighter.paused = false;
            handler(null);
            base.Finish(handler);
        }

        public class FrozenEffect : PointEffect
        {
            Sprite sprite;

            public FrozenEffect(int x, int y, int depth) : base(x, y, depth)
            {
                sprite = Global.spriteBox.Pick("Frozen");
            }

            public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
            {
                sprite.Draw(gameTime, spriteBatch, x, y, Color.White);
                base.Draw(gameTime, spriteBatch);
            }
        }
    }
}
