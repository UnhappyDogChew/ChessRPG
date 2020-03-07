using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class EffectLayer : Layer
    {
        public List<Effect> elements { get; private set; }
        public bool cameraOn { get; set; }

        public EffectLayer(string name, bool cameraOn = false) : base(name)
        {
            elements = new List<Effect>();
            this.cameraOn = cameraOn;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (cameraOn)
                spriteBatch.Begin(transformMatrix: Global.camera.FollowPlayer());
            else
                spriteBatch.Begin();
            foreach (Effect effect in elements)
            {
                effect.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            elements.Sort((Effect e1, Effect e2) => { return e1.Depth - e2.Depth; });
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].Update(gameTime);
                if (elements[i].Finished)
                    elements.RemoveAt(i--);
            }
        }
    }
}
