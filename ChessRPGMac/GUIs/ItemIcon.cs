using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class ItemIcon : GUIItem, ISelectable, IDragable
    {
        Item item;
        ItemSocket socket { get { return (parent is ItemSocket) ? (ItemSocket)parent : null; } }
        Player player;

        const int WIDTH = 32;
        const int HEIGHT = 32;

        public ItemIcon(string name, GUIComponent parent, int rx, int ry, Item item) : base(name, parent, rx, ry)
        {
            this.item = item;
            player = Global.world.GetPlayer();
        }

        public void ChangeSocket(DragSocket socket)
        {
            relativeX = 0;
            relativeY = 0;

            if (this.socket != null && socket is ItemSocket)
            {
                player.MoveItem(this.socket.column, this.socket.row, ((ItemSocket)socket).column, ((ItemSocket)socket).row);
            }

            parent = socket;
        }

        public string GetKey()
        {
            return "Item";
        }

        public Point GetLocation()
        {
            return new Point(x, y);
        }

        public Vector2 GetOrigin(int x, int y)
        {
            return new Vector2(x - this.x, y - this.y);
        }

        public Point GetSize()
        {
            return new Point(WIDTH, HEIGHT);
        }

        public DragSocket GetSocket()
        {
            if (parent is DragSocket)
                return (DragSocket)parent;
            return null;
        }

        public Texture2D GetTexture()
        {
            Texture2D result = Toolbox.CopyTexture(item.icon);
            Toolbox.SetTextureAlpha(result, 125);
            return result;
        }

        public bool IsInside(int x, int y)
        {
            return Toolbox.IsPointInsideSquare(this.x, this.y, this.x + WIDTH, this.y + HEIGHT, x, y);
        }

        public void SelectAction()
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(item.icon, new Vector2(x, y), Color.White);
        }

        public void Focus()
        {
        }

        public void Leave()
        {
        }
    }
}
