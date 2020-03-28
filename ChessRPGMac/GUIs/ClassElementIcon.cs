using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class ClassIcon : GUIItem, ITooltipInvoker
    {
        Class heroClass;
        Texture2D iconTexture;

        public ClassIcon(string name, GUIComponent parent, int rx, int ry, Class heroClass) : base(name, parent, rx, ry)
        {
            this.heroClass = heroClass;
            iconTexture = Global.content.Load<Texture2D>(heroClass.ToString() + "Icon");
        }

        public override void DrawBegin(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(iconTexture, new Vector2(relativeX, relativeY), Color.White);
            spriteBatch.End();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(iconTexture, new Vector2(x, y), Color.White);
            spriteBatch.End();
        }

        public void InvokeTooltip()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
        }
    }

    public class ElementIcon : GUIItem, ITooltipInvoker
    {
        Element heroElement;
        Texture2D iconTexture;

        public ElementIcon(string name, GUIComponent parent, int rx, int ry, Element heroElement) : base(name, parent, rx, ry)
        {
            this.heroElement = heroElement;
            iconTexture = Global.content.Load<Texture2D>(heroElement.ToString() + "Icon");
        }

        public override void DrawBegin(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(iconTexture, new Vector2(relativeX, relativeY), Color.White);
            spriteBatch.End();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(iconTexture, new Vector2(x, y), Color.White);
            spriteBatch.End();
        }

        public void InvokeTooltip()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
