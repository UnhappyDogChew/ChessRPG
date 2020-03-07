using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    /// <summary>
    /// This class provide many usefull static methods for developing game.
    /// </summary>
    public static class Toolbox
    {
        /// <summary>
        /// Splits the sprite sheet and returns List of each sprite Texture.
        /// </summary>
        /// <returns>List of each sprite texture.</returns>
        /// <param name="sheet">Source spritesheet texture.</param>
        /// <param name="tileWidth">Width of each tile.</param>
        /// <param name="tileHeight">Height of each tile.</param>
        public static List<Texture2D> SplitSpriteSheet(Texture2D sheet, int tileWidth, int tileHeight)
        {
            if (sheet.Width % tileWidth != 0 || sheet.Height % tileHeight != 0)
                throw new Exception("Sheet size and tile size is not compatible.");

            int width = (int)(sheet.Width / tileWidth);
            int height = (int)(sheet.Height / tileHeight);

            Color[] linearData = new Color[sheet.Width * sheet.Height];
            sheet.GetData<Color>(linearData);

            Color[,] mapData = new Color[sheet.Width, sheet.Height];

            // Copy linearData to mapData
            for (int x = 0; x < sheet.Width; x++)
            {
                for (int y = 0; y < sheet.Height; y++)
                {
                    mapData[x, y] = linearData[x + y * sheet.Width];
                }
            }
            // Create each tile texture and store it in resultList.
            List<Texture2D> result = new List<Texture2D>();

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    Texture2D tile = new Texture2D(Global.graphics, tileWidth, tileHeight);
                    Color[] tileData = new Color[tileWidth * tileHeight];
                    int i = 0;

                    for (int y = h * tileHeight; y < (h+1) * tileHeight; y++)
                    {
                        for (int x = w * tileWidth; x < (w+1) * tileWidth; x++)
                        {
                            tileData[i++] = mapData[x, y];
                        }
                    }
                    tile.SetData<Color>(tileData);
                    result.Add(tile);
                }
            }
            return result;
        }

        /// <summary>
        /// Flips the texture vertical.
        /// </summary>
        /// <param name="source">Source Texture.</param>
        public static void FlipTextureVertical(Texture2D source)
        {
            Color[] data = new Color[source.Width * source.Height];
            Color[] flipData = new Color[source.Width * source.Height];
            source.GetData<Color>(data);

            for (int h = 0; h < source.Height; h++)
            {
                for (int w = 0; w < source.Width; w++)
                {
                    flipData[(source.Height - 1 - h) * source.Width + w] = data[h * source.Width + w];
                }
            }

            source.SetData<Color>(flipData);
        }

        /// <summary>
        /// Flips the texture horizontal.
        /// </summary>
        /// <param name="source">Source texture.</param>
        public static void FlipTextureHorizontal(Texture2D source)
        {
            Color[] data = new Color[source.Width * source.Height];
            Color[] flipData = new Color[source.Width * source.Height];
            source.GetData<Color>(data);

            for (int h = 0; h < source.Height; h++)
            {
                for (int w = 0; w < source.Width; w++)
                {
                    flipData[h * source.Width + (source.Width - 1 - w)] = data[h * source.Width + w];
                }
            }

            source.SetData<Color>(flipData);
        }

        /// <summary>
        /// Sets the texture alpha. Alpha value is between 0 to 255.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="alpha">Alpha.</param>
        public static void SetTextureAlpha(Texture2D source, int alpha)
        {
            Color[] data = new Color[source.Width * source.Height];
            source.GetData<Color>(data);
            for (int i = 0; i < source.Width * source.Height; i++)
            {
                if (data[i].A == 0)
                    continue;
                data[i].A = (byte)alpha;
            }
            source.SetData<Color>(data);
        }

        public static Texture2D CopyTexture(Texture2D source)
        {
            Texture2D result = new Texture2D(Global.graphics, source.Width, source.Height);
            Color[] c = new Color[source.Width * source.Height];
            source.GetData(c);
            result.SetData(c);
            return result;
        }

        public static void DrawSquare(SpriteBatch spriteBatch, int x, int y, 
            int width, int height, Vector2 origin, Color color, float depth = 0)
        {
            Texture2D colliderTexture = new Texture2D(Global.graphics, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < width * height; i++)
            {
                data[i] = color;
            }
            colliderTexture.SetData<Color>(data);
            spriteBatch.Draw(colliderTexture, new Vector2(x, y), color: Color.White, origin: origin, layerDepth:depth);
        }

        public static bool IsPointInsideSquare(int x1, int y1, int x2, int y2, int x, int y)
        {
            if ((x >= x1) && (x <= x2) && (y >= y1) && (y <= y2))
            {
                return true;
            }
            return false;
        }

        public static Color ParseColor(string data)
        {
            if (data[0] == '#')
                data = data.Substring(1);
            int a = 255;
            if (data.Length == 8)
                a = Convert.ToInt32(data.Substring(6, 2), 16);

            int r = Convert.ToInt32(data.Substring(0, 2), 16);
            int g = Convert.ToInt32(data.Substring(2, 2), 16);
            int b = Convert.ToInt32(data.Substring(4, 2), 16);
            return new Color(r, g, b, a);
        }

        public static Texture2D CutTexture(Texture2D source, int x, int y, int width, int height)
        {
            Texture2D result = new Texture2D(Global.graphics, width, height);

            Color[] data = new Color[source.Width * source.Height];
            source.GetData<Color>(data);

            Color[] resultData = new Color[width * height];

            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    resultData[c + r * width] = data[(x + c) + (y + r) * source.Width];
                }
            }
            result.SetData<Color>(resultData);
            return result;
        }

        public static void DrawAlignedString(SpriteBatch spriteBatch, string text, int x, int y, 
            SpriteFont font, Color color, int lineWidth, 
            AlignType verticalAlign = AlignType.Top, AlignType horizontalAlign = AlignType.Left)
        {
            string word = "";
            string line = "";
            List<string> lines = new List<string>();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                {
                    line += word + ' ';
                    word = "";
                }
                else
                {
                    word += text[i];
                    if (i == text.Length - 1)
                    {
                        line += word;
                        lines.Add(line);
                        break;
                    }
                    if (font.MeasureString(line + word).X > lineWidth)
                    {
                        line = line.Trim();
                        lines.Add(line);
                        line = "";
                    }
                }
            }

            int textHeight = font.LineSpacing * lines.Count;
            int startY = y;
            switch (verticalAlign)
            {
                case AlignType.Top: startY = y; break;
                case AlignType.Center: startY = y - textHeight / 2; break;
                case AlignType.Bottom: startY = y - textHeight; break;
            }

            for (int i = 0; i < lines.Count; i++)
            {
                int startX = x;
                int textWidth = (int)font.MeasureString(lines[i]).X;
                switch (horizontalAlign)
                {
                    case AlignType.Left: startX = x; break;
                    case AlignType.Center: startX = x - textWidth / 2; break;
                    case AlignType.Right: startX = x - textWidth; break; 
                }
                spriteBatch.DrawString(font, lines[i], new Vector2(startX, startY + font.LineSpacing * i), color);
            }
        }
    }
}
