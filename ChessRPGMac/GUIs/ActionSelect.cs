using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class ActionSelect : GUIGroup, ISelectable
    {
        Texture2D texture;
        Sprite page;
        Action[] actions;
        int actionIndex;
        SpriteFont font12;
        SpriteFont font24;

        public static readonly int WIDTH = 352;
        public static readonly int HEIGHT = 236;
        const int FRAME_WIDTH = 36;
        const int FRAME_HEIGHT = 36;
        const int ICON_X = 76;
        const int ICON_Y = 116;
        const int NAME_X = 92;
        const int NAME_Y = 82;
        const int CONSUMPTION_X = 92;
        const int CONSUMPTION_Y = 182;
        const int DESCRIPTION_X = 193;
        const int DESCRIPTION_Y = 130;
        const int PAGE_WIDTH = 140;

        public ActionSelect(string name, GUIComponent parent, int rx, int ry, Action[] actions)
            : base(name, parent, rx, ry)
        {
            texture = Global.content.Load<Texture2D>("ActionSelect");
            page = Global.spriteBox.Pick("ActionSelectPage");
            font12 = Global.content.Load<SpriteFont>("neodgm12");
            font24 = Global.content.Load<SpriteFont>("neodgm");
            page.Stop();
            this.actions = actions;
        }

        public void SetActions(Action[] actions)
        {
            this.actions = actions;
            actionIndex = 0;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Vector2(x, y), sourceRectangle: new Rectangle(0, 0, WIDTH, HEIGHT),
                color: Color.White);
            if (page.animating)
            {
                page.Draw(gameTime, spriteBatch, x, y, Color.White);
            }
            else
            {
                if (actions[actionIndex] is Skill)
                {
                    Skill skill = (Skill)actions[actionIndex];
                    spriteBatch.Draw(skill.icon, new Vector2(x + ICON_X, y + ICON_Y), Color.White);
                    spriteBatch.Draw(texture, new Vector2(x + ICON_X - 2, y + ICON_Y - 2),
                        sourceRectangle: new Rectangle(0, HEIGHT, FRAME_WIDTH, FRAME_HEIGHT), color: Color.White);
                    // Draw name
                    Toolbox.DrawAlignedString(spriteBatch, skill.name, x + NAME_X, y + NAME_Y, font24, Toolbox.ParseColor("#5e3643ff"), PAGE_WIDTH, 
                        AlignType.Center, AlignType.Center);
                    // Draw Consumption
                    Toolbox.DrawAlignedString(spriteBatch, "SP " + skill.manaUsage, x + CONSUMPTION_X, y + CONSUMPTION_Y, 
                        font12, Toolbox.ParseColor("#3978a8ff"), PAGE_WIDTH, AlignType.Center, AlignType.Center);
                    // Draw Description
                    Toolbox.DrawAlignedString(spriteBatch, skill.GetDescription(), x + DESCRIPTION_X, y + DESCRIPTION_Y,
                        font12, Toolbox.ParseColor("#5e3643ff"), PAGE_WIDTH, AlignType.Center, AlignType.Left);
                }
            }

            spriteBatch.End();
        }

        public bool Next(Direction direction)
        {
            if (direction == Direction.Up || direction == Direction.Down || page.animating)
                return false;
            int v = 1 - ((int)direction - 2) * 2;
            try
            {
                actionIndex += v;
                Action action = actions[actionIndex];
                if (action == null)
                    throw new IndexOutOfRangeException();
            }
            catch (IndexOutOfRangeException)
            {
                actionIndex -= v;
                return false;
            }
            if (direction == Direction.Right)
                page.reverse = false;
            else if (direction == Direction.Left)
                page.reverse = true;
            page.Start();

            return true;
        }

        public override void Update(GameTime gameTime)
        {
            page.Update(gameTime);
        }

        public Point GetLocation()
        {
            return new Point(x + 11, y + 43);
        }

        public Point GetSize()
        {
            return new Point(332, 182);
        }

        public void SelectAction()
        {
            if (Global.state is BattleState)
            {
                ((BattleState)Global.state).SelectTarget(actions[actionIndex]);
            }
        }

        public void Focus()
        {
        }

        public void Leave()
        {
        }
    }
}
