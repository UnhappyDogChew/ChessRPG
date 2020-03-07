using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public interface IDrawable
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
