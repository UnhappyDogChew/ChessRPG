using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class Boundary : GameObject
    {
        Color c;
        Player player;
        public Boundary(int x, int y) : base(x, y)
        {
            collider = new SquareCollider(x, y, 100, 50, 50, 25, true);
            player = Global.world.GetPlayer();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Toolbox.DrawSquare(spriteBatch, x, y,
                    ((SquareCollider)collider).width, ((SquareCollider)collider).height,
                    new Vector2(50, 25), c, depth);
        }

        public override void Update(GameTime gameTime)
        {
            if (collider.Detect(player.collider))
                c = Color.RosyBrown;
            else
                c = Color.Aqua;
        }
    }
}
