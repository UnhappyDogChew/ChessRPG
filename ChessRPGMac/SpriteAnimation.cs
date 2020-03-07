using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public abstract class SpriteAnimation
    {
        public int repr_x { get; protected set; }
        public int repr_y { get; protected set; }
        public float repr_xScale { get; protected set; }
        public float repr_yScale { get; protected set; }
        public float repr_rotation { get; protected set; }
        public Color repr_color { get; protected set; }

        protected int timespan;
        protected int interval;

        public bool Stopped = false;
        public bool Finished { get; protected set; }

        public delegate void AnimationFinishHandler();
        public AnimationFinishHandler animationFinishHandler;

        public SpriteAnimation()
        {
            repr_xScale = 1.0f;
            repr_yScale = 1.0f;
            repr_color = Color.White;
            Finished = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Finished)
                animationFinishHandler();
        }
    }

    public class ShakeAnimation : SpriteAnimation
    {
        int index;
        int[] animation = { -4, 4, -2, 2, -1, 1 };

        public ShakeAnimation()
        {
            interval = 5;
        }

        public override void Update(GameTime gameTime)
        {
            if (Stopped)
                return;

            timespan++;
            if (timespan >= interval)
            {
                timespan = 0;
                index++;
                if (index >= 6)
                {
                    Finished = true;
                }
                else
                    repr_x = animation[index];
            }
            base.Update(gameTime);
        }
    }

    public class MeleeAttackAnimation : SpriteAnimation
    {
        int index;
        int[] animation = { 0, -3, -7, -13, -20, -13, -7, -3,};
        Direction direction;

        public MeleeAttackAnimation(Direction direction)
        {
            this.direction = direction;
            interval = 2;
        }
        public override void Update(GameTime gameTime)
        {
            if (Stopped)
                return;

            timespan++;
            if (timespan >= interval)
            {
                timespan = 0;
                index++;
                if (index >= animation.Length)
                {
                    Finished = true;
                }
                else
                    repr_y = (direction == Direction.Up) ? animation[index] : -animation[index];
            }
            base.Update(gameTime);
        }
    }
}
