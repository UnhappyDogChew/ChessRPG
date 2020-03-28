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
        public delegate void AnimationFinishHandler();
        public AnimationFinishHandler animationFinishHandler;

        bool moving;
        Point movePoint;
        float step;

        public GameObject(int x, int y)
        {
            this.x = x;
            this.y = y;
            Finished = false;
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public virtual void Update(GameTime gameTime)
        {
            if (moving)
            {
                float xMovement = (movePoint.x - x) / step;
                float yMovement = (movePoint.y - y) / step;
                if (xMovement > 0 && xMovement < 1)
                    xMovement = 1;
                else if (xMovement < 0 && xMovement > -1)
                    xMovement = -1;
                if (yMovement > 0 && yMovement < 1)
                    yMovement = 1;
                else if (yMovement < 0 && yMovement > -1)
                    yMovement = -1;

                x += (int)xMovement;
                y += (int)yMovement;

                if (x == movePoint.x && y == movePoint.y)
                {
                    moving = false;
                    animationFinishHandler?.Invoke();
                }
            }
        }

        public virtual bool MoveLocationSmooth(Point location, float step)
        {
            if (location.x == x && location.y == y)
                return false;
            moving = true;
            movePoint = location;
            this.step = step;
            return true;
        }

        public void Finish()
        {
            Finished = true;
        }
    }
}
