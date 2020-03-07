using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class GUILayer : Layer
    {
        public GUIGroup MainGroup { get; private set; }

        public GUILayer(string name) : base(name)
        {
            MainGroup = new GUIGroup("MainGroup", new NullGUIComponent(), 0, 0);
        }

        public override void DrawBegin(GameTime gameTime, SpriteBatch spriteBatch)
        {
            MainGroup.DrawBegin(gameTime, spriteBatch);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            MainGroup.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            MainGroup.Update(gameTime);
        }

        public void AddGUI(GUIComponent gui)
        {
            MainGroup.AddComponent(gui);
        }

        public void RemoveGUI(string name)
        {
            foreach (GUIComponent component in MainGroup)
            {
                if (component.name == name)
                {
                    component.Finish();
                    break;
                }
            }
        }

        public GUIComponent GetGUI<GUIType>()
        {
            foreach (GUIComponent element in MainGroup)
            {
                if (element is GUIType)
                    return element;
            }
            return null;
        }

        /// <summary>
        /// Gets GUI with name. This method can get array of names to specify target GUI
        /// with duplicated name.
        /// </summary>
        /// <returns>The GUI.</returns>
        /// <param name="names">Names.</param>
        public GUIComponent GetGUI(params string[] names)
        {
            GUIComponent result = null;
            foreach (GUIComponent element in MainGroup)
            {
                if (element.name == names[0])
                {
                    result = element;
                }
            }
            if (result == null)
                return null;

            bool first = true;
            foreach (string name in names)
            {
                if (first)
                {
                    first = false;
                    continue;
                }
                bool found = false;
                foreach (GUIComponent element in result)
                {
                    if (element.name == name)
                    {
                        result = element;
                        found = true;
                        break;
                    }
                }
                if (!found)
                    return null;
            }

            return result;
        }
    }
}
