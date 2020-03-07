using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public interface IDragable
    {
        Texture2D GetTexture();
        Vector2 GetOrigin(int x, int y);
        bool IsInside(int x, int y);
        void ChangeSocket(DragSocket socket);
        void DrawBegin(GameTime gameTime, SpriteBatch spriteBatch);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        string GetKey();
        DragSocket GetSocket();
    }
}
