using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class DamageEffect : PointEffect
    {
        SpriteFont font;
        int damage;

        int timespan;
        int interval = 2;
        int[] animation = { -2, -6, -7, -6, -4, 0 };
        int index;
        int lastTimespan = 40;
        bool animationFinished = false;

        public DamageEffect(int damage, int x, int y, int depth) : base(x, y, depth)
        {
            this.damage = damage;
            font = Global.content.Load<SpriteFont>("neodgm22");
        }

        public override void Update(GameTime gameTime)
        {
            timespan++;
            if (animationFinished)
            {
                if (timespan >= lastTimespan)
                    Finish();
                return;
            }
            if (timespan >= interval)
            {
                timespan = 0;
                index++;
                if (index >= animation.Length)
                {
                    index--;
                    animationFinished = true;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Toolbox.DrawAlignedString(spriteBatch, damage.ToString(), x, y + animation[index], font, Color.White,
                    Global.Properties.GAME_WIDTH, AlignType.Center, AlignType.Center);
        }
    }
}
