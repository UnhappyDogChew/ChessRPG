using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    /// <summary>
    /// This layer contains GameObjects. This is <see cref="Layer"/>.
    /// </summary>
    public class GameObjectLayer : Layer
    {
        public List<GameObject> elements { get; set; }

        public GameObjectLayer(string name) : base(name)
        {
            elements = new List<GameObject>();
        }
        /// <summary>
        /// Draw all elements.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix:Global.camera.FollowPlayer());
            foreach (GameObject element in elements)
            {
                element.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }
        /// <summary>
        /// Update all elements.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].Update(gameTime);
                if (elements[i].Finished)
                    elements.RemoveAt(i--);
            }
        }
    }
}
