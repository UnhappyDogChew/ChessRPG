using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class SoulPurification : GUIGroup
    {
        Texture2D background;
        public Button purifyButton { get; private set; }
        public Button exitButton { get; private set; }
        public List<CheckBox> soulCheckBoxes { get; private set; }
        SpriteFont font;

        Player player;
        Hero hero;

        int selectedSoulAmount;

        const int TITLE_X = 127;
        const int TITLE_Y = 12;
        const int SOUL_X = 40;
        const int SOUL_Y = 38;
        const int SOUL_GAP = 18;
        const int SOUL_WIDTH = 14;
        const int ENERGY_X = 29;
        const int ENERGY_Y = 71;
        const int LEVEL_X = 151;
        const int LEVEL_Y = 71;
        const int BUTTON_X = 96;
        const int BUTTON_Y = 96;
        const int BUTTON_WIDTH = 64;
        const int EXIT_X = 230;
        const int EXIT_Y = 10;
        const int EXIT_WIDTH = 16;

        public SoulPurification(string name, GUIComponent parent, int rx, int ry, Hero hero) : base(name, parent, rx, ry)
        {
            background = Global.content.Load<Texture2D>("SoulPurificationBackground");
            Texture2D soulTexture = Global.content.Load<Texture2D>("SoulPurificationSoul");
            Texture2D buttonTexture = Global.content.Load<Texture2D>("SoulPurificationButton");
            Texture2D exitButtonTexture = Global.content.Load<Texture2D>("SoulPurificationExitButton");
            purifyButton = new Button("PurifyButton", this, BUTTON_X, BUTTON_Y, buttonTexture, BUTTON_WIDTH);
            exitButton = new Button("ExitButton", this, EXIT_X, EXIT_Y, exitButtonTexture, EXIT_WIDTH);
            font = Global.content.Load<SpriteFont>("neodgm12");
            player = Global.world.GetPlayer();
            this.hero = hero;
            selectedSoulAmount = 0;

            soulCheckBoxes = new List<CheckBox>();

            for (int i = 0; i < 10; i++)
            {
                CheckBox soulCheckBox = new CheckBox("SoulCheckBox" + (i + 1), this, SOUL_X + SOUL_GAP * i, SOUL_Y, soulTexture, SOUL_WIDTH);
                for (int j = 0; j < i; j++)
                {
                    soulCheckBox.synchronized.Add(soulCheckBoxes[j]);
                }
                soulCheckBox.Clicked += (sender, e) => { CheckSoul((CheckBox)sender); };
                AddComponent(soulCheckBox);
                soulCheckBoxes.Add(soulCheckBox);
            }

            AddComponent(purifyButton);
            AddComponent(exitButton);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(x, y), Color.White);
            Toolbox.DrawAlignedString(spriteBatch, "Purify Soul", x + TITLE_X, y + TITLE_Y, font, Color.Black, 
                background.Width, horizontalAlign: AlignType.Center);
            spriteBatch.End();
            base.Draw(gameTime, spriteBatch);
        }

        public void SetHero(Hero hero) { this.hero = hero; }

        private void CheckSoul(CheckBox soulCheckBox)
        {
            int number = int.Parse(soulCheckBox.name.Substring(12));
            if (number < selectedSoulAmount)
            {
                for (int i = 0; i < number; i++)
                {
                    soulCheckBoxes[i].Check();
                }
                for (int i = number; i < 10; i++)
                {
                    soulCheckBoxes[i].Uncheck();
                }
                selectedSoulAmount = number;
            }
            else if (number == selectedSoulAmount)
            {
                selectedSoulAmount = 0;
            }
            else
            {
                selectedSoulAmount = number;
            }
        }
    }
}
