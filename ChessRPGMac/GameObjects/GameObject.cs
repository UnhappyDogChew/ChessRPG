using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    /// <summary>
    /// Game object.
    /// </summary>
    public abstract class GameObject : IDrawable
    {
        public float depth { get { return y / 100000.0f; } }
        public Collider collider { get; protected set; }
        public int x { get; protected set; }
        public int y { get; protected set; }
        public bool Finished { get; protected set; }

        public GameObject(int x, int y)
        {
            this.x = x;
            this.y = y;
            Finished = false;
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

        public void Finish()
        {
            Finished = true;
        }
    }
}
