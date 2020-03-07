using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class SlashEffect : PointEffect
    {
        Sprite sprite;

        public SlashEffect(int x, int y, int depth) : base(x, y, depth)
        {
            sprite = Global.spriteBox.Pick("SlashEffect");
            sprite.Start();
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
            if (!sprite.animating)
                Finish();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, x, y, Color.White);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
