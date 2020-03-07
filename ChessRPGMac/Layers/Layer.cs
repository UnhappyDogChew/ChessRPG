 using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public abstract class Layer : IDrawable
    {
        protected string name;

        public Layer(string name)
        {
            this.name = name;
        }

        public string GetName() => name;

        public virtual void DrawBegin(GameTime gameTime, SpriteBatch spriteBatch) { }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);
    }
}
