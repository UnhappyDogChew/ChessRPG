using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class DynamicWord
    {
        SpriteFont font;
        string word;
        public int Index { get; private set; }

        int x;
        int y;
        Color color;

        int timespan;
        const int interval = 6;
        int shakeIndex;

        int[,] shakePosition = { { -1, -1 }, { 1, 1 }, { 1, -1 }, { -1, 1 } };
        bool shake;

        public DynamicWord(SpriteFont font, string word, int x, int y, Color color, bool shake = false)
        {
            Index = 0;
            this.font = font;
            this.word = word;
            this.x = x;
            this.y = y;
            this.color = color;
            Random r = new Random();
            shakeIndex = r.Next(0, 4);
            timespan = r.Next(0, interval);
            this.shake = shake;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (shake)
                spriteBatch.DrawString(font, word.Substring(0, Index), 
                    new Vector2(x + shakePosition[shakeIndex, 0], y + shakePosition[shakeIndex, 1]), color);
            else
                spriteBatch.DrawString(font, word.Substring(0, Index), new Vector2(x, y), color);
        }

        public void Update(GameTime gameTime)
        {
            timespan++;
            if (timespan >= interval)
            {
                timespan = 1;
                shakeIndex++;
            }
            if (shakeIndex >= 4)
                shakeIndex = 0;
        }

        public bool Process()
        {
            Index++;
            if (Index >= word.Length)
                return true;
            else
                return false;
        }

        public float GetWidth()
        {
            return font.MeasureString(word).X;
        }
    }
}
