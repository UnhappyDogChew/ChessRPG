using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChessRPGMac
{
    /// <summary>
    /// This layer represents tilemap. tilemap data can be driven out from .tm file.
    /// </summary>
    public class Tilemap : Layer
    {
        // Library informations
        List<Texture2D> library;
        int tileWidth, tileHeight;

        // Map informations
        int mapWidth, mapHeight;
        int[,] map;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ChessRPGMac.Tilemap"/> class with no data.
        /// </summary>
        /// <param name="name">Name.</param>
        public Tilemap(string name) : base(name)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ChessRPGMac.Tilemap"/> class with .tm file.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="fs">Fs.</param>
        public Tilemap(string name, FileStream fs) : base(name)
        {
            SetFromFile(fs);
        }
        /// <summary>
        /// Sets data from .tm filestream.
        /// </summary>
        /// <param name="fs">FileStream.</param>
        public void SetFromFile(FileStream fs)
        {
            StreamReader sr = new StreamReader(fs);

            try
            {
                string libraryName = sr.ReadLine();
                string[] datas = sr.ReadLine().Split(',');
                tileWidth = int.Parse(datas[0]);
                tileHeight = int.Parse(datas[1]);
                library = Toolbox.SplitSpriteSheet(Global.content.Load<Texture2D>(libraryName), tileWidth, tileHeight);

                datas = sr.ReadLine().Split(',');
                mapWidth = int.Parse(datas[0]);
                mapHeight = int.Parse(datas[1]);

                map = new int[mapWidth, mapHeight];
                for (int h = 0; h < mapHeight; h++)
                {
                    datas = sr.ReadLine().Split(',');
                    for (int w = 0; w < mapWidth; w++)
                    {
                        map[w, h] = int.Parse(datas[w]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            sr.Close();
            sr.Dispose();
        }
        /// <summary>
        /// Draw tilemap.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix:Global.camera.FollowPlayer());
            for (int h = 0; h < mapHeight; h++)
            {
                for (int w = 0; w < mapWidth; w++)
                {
                    spriteBatch.Draw(library[map[w, h]], new Vector2(w * tileWidth, h * tileHeight), Color.White);
                }
            }
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            // Do nothing.
        }
    }
}
