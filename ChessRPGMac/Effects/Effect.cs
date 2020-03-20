using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public abstract class Effect : ITask
    {
        public bool Finished { get; private set; }
        public int Depth { get; protected set; }

        protected bool started;

        public event TaskFinishedHandler TaskFinished;

        public Effect(int depth) { Finished = false; started = false; this.Depth = depth; }

        public virtual void Update(GameTime gameTime)
        {
            if (!started)
                return;
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!started)
                return;
        }

        /// <summary>
        /// Use this method to finish effect, rather than turning <see cref="Finished"/> variable to true.
        /// </summary>
        public virtual void Finish() { Finished = true; TaskFinished?.Invoke(this); }

        public void StartTask()
        {
            started = true;
        }
    }

    public abstract class PointEffect : Effect
    {
        public int x { get; set; }
        public int y { get; set; }
        public PointEffect (int x, int y, int depth) : base(depth)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class EffectSprite : PointEffect
    {
        public Sprite sprite { get; private set; }

        public EffectSprite(Sprite sprite, int x, int y, int depth = 0) : base(x, y, depth)
        {
            this.sprite = sprite;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            sprite.Draw(gameTime, spriteBatch, x, y, Color.White);
        }
    }

    public class EffectTexture : PointEffect
    {
        public Texture2D texture { get; private set; }

        public EffectTexture(Texture2D texture, int x, int y, int depth = 0) : base(x, y, depth)
        {
            this.texture = texture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(texture, new Vector2(x, y), Color.White);
        }
    }

    public class SelectingEffect : PointEffect
    {
        Texture2D texture;
        public int width { get; set; }
        public int height { get; set; }

        const int interval = 10;
        int timespan;
        int index;
        int[] move = { 0, -1 };

        public SelectingEffect(int x, int y, int width, int height, int depth = 0) : base(x, y, depth)
        {
            texture = Global.content.Load<Texture2D>("Selecting");
            this.width = width;
            this.height = height;
            timespan = 0;
            index = 0;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(texture, new Vector2(x - 8 + move[index], y - 8 + move[index]), 
                sourceRectangle: new Rectangle(0, 0, 16, 16), color: Color.White);
            spriteBatch.Draw(texture, new Vector2(x + width - 8 - move[index], y - 8 + move[index]), 
                sourceRectangle: new Rectangle(16, 0, 16, 16), color: Color.White);
            spriteBatch.Draw(texture, new Vector2(x - 8 + move[index], y + height - 8 - move[index]), 
                sourceRectangle: new Rectangle(32, 0, 16, 16), color: Color.White);
            spriteBatch.Draw(texture, new Vector2(x + width - 8 - move[index], y + height - 8 - move[index]), 
                sourceRectangle: new Rectangle(48, 0, 16, 16), color: Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timespan++;
            if (timespan >= interval)
            {
                timespan = 0;
                index = (index == 1) ? 0 : 1;
            }
        }
    }

    public class SelectedEffect : PointEffect
    {
        Sprite sprite;

        public SelectedEffect(int x, int y, int depth) : base(x, y, depth) 
        {
            sprite = new Sprite(Global.content.Load<Texture2D>("Selected"), new Vector2(12, 14));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            sprite.Draw(gameTime, spriteBatch, x, y, Color.White);
        }
    }

    public class Timer : Effect
    {
        int time;
        int timespan;

        public Timer(int frame) : base(0)
        {
            time = frame;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timespan++;
            if (timespan >= time)
                Finish();
        }
    }
}
