using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class Soul : GameObject, IClickable
    {
        public Hero hero { get; private set; }
        Sprite aura;
        Sprite heroSprite;

        int alpha;

        MouseClickState clickState;
        MouseLocationState locationState;

        public event EventHandler Clicked;
        public event EventHandler MouseEntered;
        public event EventHandler MouseLeaved;
        public event EventHandler Released;

        public Soul(Hero hero, int x, int y) : base(x, y)
        {
            this.hero = hero;
            aura = Global.spriteBox.Pick("Auras");
            heroSprite = hero.sprite;
            collider = new NullCollider();
            alpha = 100;

            clickState = MouseClickState.Released;
            locationState = MouseLocationState.Leaved;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            aura.Draw(gameTime, spriteBatch, x, y, Color.White, index: hero.rarity);
            heroSprite.Draw(gameTime, spriteBatch, x, y, new Color(256, 256, 256, alpha), index: 0);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Click()
        {
            if (clickState == MouseClickState.Released)
            {
                Clicked?.Invoke(this, null);
                clickState = MouseClickState.Clicked;
            }
        }

        public void Release()
        {
            if (clickState == MouseClickState.Clicked)
            {
                Released?.Invoke(this, null);
                clickState = MouseClickState.Released;
            }
        }

        public void MouseEnter()
        {
            if (locationState == MouseLocationState.Leaved)
            {
                MouseEntered?.Invoke(this, null);
                alpha = 150;
                locationState = MouseLocationState.Entered;
            }

        }

        public void MouseLeave()
        {
            if (locationState == MouseLocationState.Entered)
            {
                MouseLeaved?.Invoke(this, null);
                alpha = 100;
                locationState = MouseLocationState.Leaved;
            }
        }

        public Rectangle GetBoundary()
        {
            return new Rectangle(x - Global.camera.x - (int)aura.origin.X, y - Global.camera.y - (int)aura.origin.Y, aura.width, aura.height);
        }
    }
}
