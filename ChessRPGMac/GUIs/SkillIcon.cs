using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class SkillIcon : GUIItem, ITooltipInvoker
    {
        public Skill skill { get; set; }

        const int WIDTH = 32;
        const int HEIGHT = 32;

        public SkillIcon(string name, GUIComponent parent, int rx, int ry) : base(name, parent, rx, ry)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (skill != null)
                spriteBatch.Draw(skill.icon, new Vector2(x, y), Color.White);
        }

        public void InvokeTooltip()
        {
            throw new NotImplementedException();
        }
    }
}
