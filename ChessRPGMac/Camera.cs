using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class Camera
    {
        public int x { get { return (int)(Global.world.GetPlayer().x - Global.graphics.Viewport.Width / 2 - offsetX); } }
        public int y { get { return (int)(Global.world.GetPlayer().y - Global.graphics.Viewport.Height / 2 - offsetY); } }

        public float offsetX;
        public float offsetY;

        public int width { get { return Global.graphics.Viewport.Width; } }
        public int height { get { return Global.graphics.Viewport.Height; } }

        public Camera()
        {
            offsetX = 0;
            offsetY = 0;
        }

        /// <summary>
        /// Returns matrix for following player. 
        /// This method should be called after World is prepared.
        /// </summary>
        /// <returns>The player.</returns>
        public Matrix FollowPlayer()
        {
            Player player = Global.world.GetPlayer();
            Viewport viewport = Global.graphics.Viewport;
            
            return Matrix.CreateTranslation(-player.x + viewport.Width / 2 + offsetX, 
                -player.y + viewport.Height / 2 + offsetY, 0);
        }
    }
}
