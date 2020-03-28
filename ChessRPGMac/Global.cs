using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChessRPGMac
{
    public static class Global
    {
        public static Game game;
        public static World world;
        public static SpriteBox spriteBox;
        public static State state;
        public static GraphicsDevice graphics { get { return game.GraphicsDevice; } }
        public static ContentManager content { get { return game.Content; } }
        public static Camera camera;
        public static SoulBox soulBox;

        public static class Properties
        {
            public static readonly int GAME_WIDTH = 512;
            public static readonly int GAME_HEIGHT = 704;
            public static readonly int FIGHTER_IN_ROW = 5;
            public static readonly int FRAME_PER_SECOND = 60;

            public static readonly int[] SOUL_MAGNIFICATION = { 1, 2, 3, 3, 4, 4, 5, 5, 5, 5 };
            public static readonly int[] SOUL_AMOUNT = { 10, 8, 5, 3, 1 };

            public static bool fastMode = false;
        }
    }
}
