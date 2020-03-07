using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class ActionMenu : GUIGroup
    {
        Texture2D background;
        Texture2D menuTexture;

        const int ITEMSTART_X = 11;
        const int ITEMSTART_Y = 43;
        const int ITEM_GAP = 36;

        public ActionMenu(string name, GUIComponent parent, int rx, int ry) : base(name, parent, rx, ry)
        {
            background = Global.content.Load<Texture2D>("ActionMenuBackground");
            menuTexture = Global.content.Load<Texture2D>("ActionMenus(English)");

            // Add ActionItems.
            components.Add(new ActionItem("Attack", this, ITEMSTART_X, ITEMSTART_Y,
                Toolbox.CutTexture(menuTexture, 0, 0, ActionItem.WIDTH, ActionItem.HEIGHT)));
            components.Add(new ActionItem("Skill", this, ITEMSTART_X, ITEMSTART_Y + ITEM_GAP,
                Toolbox.CutTexture(menuTexture, 0, ActionItem.HEIGHT, ActionItem.WIDTH, ActionItem.HEIGHT)));
            components.Add(new ActionItem("Defense", this, ITEMSTART_X, ITEMSTART_Y + ITEM_GAP * 2,
                Toolbox.CutTexture(menuTexture, 0, ActionItem.HEIGHT * 2, ActionItem.WIDTH, ActionItem.HEIGHT)));
            components.Add(new ActionItem("Meditate", this, ITEMSTART_X, ITEMSTART_Y + ITEM_GAP * 3,
                Toolbox.CutTexture(menuTexture, 0, ActionItem.HEIGHT * 3, ActionItem.WIDTH, ActionItem.HEIGHT)));
            components.Add(new ActionItem("Move", this, ITEMSTART_X, ITEMSTART_Y + ITEM_GAP * 4,
                Toolbox.CutTexture(menuTexture, 0, ActionItem.HEIGHT * 4, ActionItem.WIDTH, ActionItem.HEIGHT)));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(x, y), color: Color.White);

            base.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }

    public class ActionItem : GUIItem, ISelectable
    {
        Texture2D texture;

        public static readonly int WIDTH = 140;
        public static readonly int HEIGHT = 22;

        public ActionItem(string name, GUIComponent parent, int rx, int ry, Texture2D texture) : base(name, parent, rx, ry)
        {
            this.texture = texture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(x, y), color: Color.White);
        }

        public Point GetLocation()
        {
            return new Point(x, y);
        }

        public Point GetSize()
        {
            return new Point(WIDTH, HEIGHT);
        }

        public void SelectAction()
        {
            if (Global.state is BattleState)
            {
                ((BattleState)Global.state).ActionSelectEnable(name);
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
