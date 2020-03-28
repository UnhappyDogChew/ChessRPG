using System;
using System.Collections.Generic;
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

    public class Button : GUIItem, IClickable
    {
        Texture2D texture;
        int width;
        int textureIndex;

        MouseClickState clickState;
        MouseLocationState locationState;

        public event EventHandler Clicked;
        public event EventHandler MouseEntered;
        public event EventHandler MouseLeaved;
        public event EventHandler Released;

        public Button(string name, GUIComponent parent, int rx, int ry, Texture2D texture, int width) : base(name, parent, rx, ry)
        {
            this.texture = texture;
            textureIndex = 0;
            this.width = width;
            clickState = MouseClickState.Released;
            locationState = MouseLocationState.Leaved;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle: new Rectangle(x, y, width, texture.Height), 
                sourceRectangle: new Rectangle(width * textureIndex, 0, width, texture.Height), color: Color.White);
            spriteBatch.End();
            base.Draw(gameTime, spriteBatch);
        }

        public void Click()
        {
            if (clickState == MouseClickState.Released)
            {
                Clicked?.Invoke(this, null);
                textureIndex = 0;
                clickState = MouseClickState.Clicked;
            }
        }

        public void MouseEnter()
        {
            if (locationState == MouseLocationState.Leaved)
            {
                MouseEntered?.Invoke(this, null);
                textureIndex = 1;
                locationState = MouseLocationState.Entered;
            }

        }

        public void MouseLeave()
        {
            if (locationState == MouseLocationState.Entered)
            {
                MouseLeaved?.Invoke(this, null);
                textureIndex = 0;
                locationState = MouseLocationState.Leaved;
            }

        }

        public void Release()
        {
            if (clickState == MouseClickState.Clicked)
            {
                Released?.Invoke(this, null);
                textureIndex = 1;
                clickState = MouseClickState.Released;
            }
        }

        public Rectangle GetBoundary()
        {
            if (!Activated)
                return Rectangle.Empty;
            return new Rectangle(x, y, width, texture.Height);
        }
    }

    public class CheckBox : GUIItem, IClickable
    {
        Texture2D texture;
        int width;
        int textureIndex;

        public bool isChecked { get; private set; }

        public List<CheckBox> synchronized { get; private set; }

        MouseClickState clickState;
        MouseLocationState locationState;

        public event EventHandler Clicked;
        public event EventHandler MouseEntered;
        public event EventHandler MouseLeaved;
        public event EventHandler Released;

        public CheckBox(string name, GUIComponent parent, int rx, int ry, Texture2D texture, int width) : base(name, parent, rx, ry)
        {
            this.texture = texture;
            textureIndex = 0;
            this.width = width;
            clickState = MouseClickState.Released;
            locationState = MouseLocationState.Leaved;
            isChecked = false;
            synchronized = new List<CheckBox>();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle: new Rectangle(x, y, width, texture.Height),
                sourceRectangle: new Rectangle(width * textureIndex, 0, width, texture.Height), color: Color.White);
            spriteBatch.End();
            base.Draw(gameTime, spriteBatch);
        }

        public void Click()
        {
            if (clickState == MouseClickState.Released)
            {
                if (isChecked)
                {
                    Uncheck();
                    foreach (CheckBox checkBox in synchronized)
                        checkBox.Uncheck();
                }
                else
                {
                    Check();
                    foreach (CheckBox checkBox in synchronized)
                        checkBox.Check();
                }


                Clicked?.Invoke(this, null);
                clickState = MouseClickState.Clicked;
            }
        }

        public void Release()
        {
            if (clickState == MouseClickState.Clicked)
            {
                Released?.Invoke(this, null);
                clickState = MouseClickState.Released;
            }
        }

        public void Check()
        {
            textureIndex = 2;
            isChecked = true;
        }

        public void Uncheck()
        {
            textureIndex = 0;
            isChecked = false;
        }

        public void MouseEnter()
        {
            if (locationState == MouseLocationState.Leaved)
            {
                MouseEntered?.Invoke(this, null);
                if (!isChecked)
                {
                    textureIndex = 1;
                    foreach (CheckBox checkBox in synchronized)
                    {
                        if (!checkBox.isChecked)
                            checkBox.SetTextureIndex(1);
                    }
                }

                locationState = MouseLocationState.Entered;
            }
        }

        public void MouseLeave()
        {
            if (locationState == MouseLocationState.Entered)
            {
                MouseLeaved?.Invoke(this, null);
                if (!isChecked)
                {
                    textureIndex = 0;
                    foreach (CheckBox checkBox in synchronized)
                    {
                        if (!checkBox.isChecked)
                            checkBox.SetTextureIndex(0);
                    }
                }

                locationState = MouseLocationState.Leaved;
            }
        }

        public void SetTextureIndex(int index) { textureIndex = index; }

        public Rectangle GetBoundary()
        {
            if (!Activated)
                return Rectangle.Empty;
            return new Rectangle(x, y, width, texture.Height);
        }
    }
}
