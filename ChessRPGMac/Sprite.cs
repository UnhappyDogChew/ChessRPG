using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    /// <summary>
    /// Sprite for simply drawing single texture, or animation.
    /// 
    /// </summary>
    public class Sprite
    {
        protected readonly Texture2D texture;
        public readonly int width;
        public readonly int height;
        public readonly Vector2 origin;
        public SpriteAnimation animation;

        // Fields for animation
        public int index { get; set; }
        public int interval { get; set; }

        // Flags
        public bool reverse { get; set; }
        public bool repeate { get; set; }
        public bool animating { get; set; }

        public readonly int count; // This field determines whether sprite is animatable or not.

        int timespan;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Sprite() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ChessRPGMac.Sprite"/> class.
        /// Width and Height is textures width and height.
        /// This initialization is for sprite with no animation.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="origin">Origin.</param>
        public Sprite(Texture2D texture, Vector2 origin)
        {
            this.texture = texture;
            width = texture.Width;
            height = texture.Height;
            this.origin = origin;

            // Set default values for not animating sprite.
            count = 1; // count = 1 means this sprite is not for animation.
            index = 0;
            interval = 0;
            timespan = 0;
            // Set flags.
            reverse = false;
            animating = false;
            repeate = false;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ChessRPGMac.Sprite"/> class.
        /// This initialization is for animating sprite.
        /// Given texture is expected to have multiple frames in row.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="origin">Origin.</param>
        /// <param name="interval">Interval.</param>
        /// <param name="reverse">If set to <c>true</c> reverse.</param>
        /// <param name="repeate">If set to <c>true</c> repeate.</param>
        /// <param name="animating">If set to <c>true</c> animating.</param>
        public Sprite(Texture2D texture, int width, int height, Vector2 origin,
            int interval = 15, bool reverse = false, bool repeate = true, bool animating = true)
        {
            this.texture = texture;
            this.width = width;
            this.height = height;
            this.origin = origin;

            count = texture.Width / width;

            // Set flags.
            this.reverse = reverse;
            this.repeate = repeate;
            this.animating = animating;
            this.interval = interval;

            index = (reverse) ? count - 1 : 0;
        }
        /// <summary>
        /// Default setting. scales is 1, color is white, origin is zero.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (animation != null)
            {
                animation.Update(gameTime);
                if (animation.Finished)
                    animation = null;
            }
            if (animating)
                Tick();
        }
        /// <summary>
        /// Tick one frame. Animating flag doesn't matter.
        /// </summary>
        public void Tick()
        {
            if (count == 1)
                return;

            timespan++;
            if (timespan >= interval)
            {
                timespan = 0;
                index += (reverse) ? -1 : 1;
                if (repeate)
                {
                    if (index < 0)
                        index = (count - 1);
                    else if (index >= count)
                        index = 0;
                }
                else
                {
                    if ((!reverse && index - 1 >= count) ||
                        (reverse && index <= 0))
                        animating = false;
                }
            }
        }
        /// <summary>
        /// Start animation and set index to 0. If reverse flag is true, set index to last.
        /// </summary>
        public void Start()
        {
            if (count == 1)
                return;

            animating = true;
            index = (reverse) ? count - 1 : 0;
            timespan = 0;
        }
        /// <summary>
        /// Stop animation and set index to 0. If reverse flag is true, set index to last.
        /// </summary>
        public void Stop()
        {
            if (count == 1)
                return;

            animating = false;
            index = (reverse) ? count - 1 : 0;
            timespan = 0;
        }
        /// <summary>
        /// Draw sprite. This method must be covered by SpriteBatch.Begin() and SpriteBatch.End() methods.
        /// This overloaded method can specify index to draw.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        /// <param name="spriteBatch">Sprite batch.</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, int x, int y, Color color, int index = -1, 
            float xScale = 1.0f, float yScale = 1.0f, float depth = 0)
        {
            if (index == -1)
                index = this.index;

            if (animation == null)
                spriteBatch.Draw(texture,
                    destinationRectangle: new Rectangle(x, y, (int)(width * xScale), (int)(height * yScale)),
                    sourceRectangle: new Rectangle(index * width, 0, width, height),
                    color: color, origin: origin, layerDepth: depth);
            else
                spriteBatch.Draw(texture,
                    destinationRectangle: new Rectangle(x + animation.repr_x, y + animation.repr_y,
                        (int)(width * xScale * animation.repr_xScale), (int)(height * yScale * animation.repr_yScale)),
                    sourceRectangle: new Rectangle(index * width, 0, width, height),
                    color: animation.repr_color, rotation: animation.repr_rotation, origin: origin, layerDepth: depth);
        }

        public Sprite Copy()
        {
            return (Sprite)this.MemberwiseClone();
        }
    }
}
