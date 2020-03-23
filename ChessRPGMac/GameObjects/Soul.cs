using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class Soul : GameObject
    {
        public Hero hero { get; private set; }
        Sprite aura;
        Sprite heroSprite;

        public Soul(Hero hero, int x, int y) : base(x, y)
        {
            this.hero = hero;
            aura = Global.spriteBox.Pick("Auras");
            heroSprite = hero.sprite;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            aura.Draw(gameTime, spriteBatch, x, y, Color.White, index: hero.rarity);
            heroSprite.Draw(gameTime, spriteBatch, x, y, new Color(256, 256, 256, 100), index: 0);
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
