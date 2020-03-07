using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class Label : GUIItem
    {
        public string text;
        public SpriteFont font;
        public int lineWidth;
        public Color color;
        public AlignType verticalAlign;
        public AlignType horizontalAlign;

        public Label(string name, GUIComponent parent, int rx, int ry, string text, Color color, int lineWidth, SpriteFont font = null, 
            AlignType verticalAlign = AlignType.Top, AlignType horizontalAlign = AlignType.Left) : base(name, parent, rx, ry)
        {
            this.text = text;
            if (font == null)
                this.font = Global.content.Load<SpriteFont>("neodgm");
            else
                this.font = font;

            this.color = color;
            this.lineWidth = lineWidth;
            this.verticalAlign = verticalAlign;
            this.horizontalAlign = horizontalAlign;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Toolbox.DrawAlignedString(spriteBatch, text, x, y, font, color, lineWidth, verticalAlign, horizontalAlign);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            // Do nothing.
        }
    }

    public class Image : GUIItem
    {
        public Texture2D texture;

        public Image(string name, GUIComponent parent, int rx, int ry, Texture2D texture) : base(name, parent, rx, ry)
        {
            this.texture = texture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Vector2(x, y), Color.White);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            // Do nothing.
        }
    }
}
