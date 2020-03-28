using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class HeroDetail : GUIGroup
    {
        Texture2D background;
        Texture2D star;
        Hero hero;
        SpriteFont font;

        public Button purifyButton { get; private set; }
        public ClassIcon classIcon { get; private set; }
        public ElementIcon elementIcon { get; private set; }

        const int NAME_X = 9;
        const int NAME_Y = 7;
        const int CLASS_X = 5;
        const int CLASS_Y = 31;
        const int CLASS_ELEMENT_GAP = 16;
        const int HERO_X = 54;
        const int HERO_Y = 76;
        const int STAR_X = 90;
        const int STAR_Y = 31;
        const int STAR_GAP = 11;
        const int INFO_X = 116;
        const int INFO_Y = 14;
        const int INFO_WIDTH = 181;
        const int INFO_HEIGHT = 95;
        const int SKILL_X = 28;
        const int SKILL_Y = 142;
        const int SKILL_GAP = 59;
        const int BUTTON_X = 109;
        const int BUTTON_Y = 191;
        const int BUTTON_WIDTH = 105;   

        public HeroDetail(string name, GUIComponent parent, int rx, int ry, Hero hero) : base(name, parent, rx, ry)
        {
            this.hero = hero;
            star = Global.content.Load<Texture2D>("RarityStar");
            background = Global.content.Load<Texture2D>("HeroDetailBackground");
            font = Global.content.Load<SpriteFont>("neodgm12");

            Texture2D buttonTexture = Global.content.Load<Texture2D>("HeroDetailButton");
            purifyButton = new Button("PurifyButton", this, BUTTON_X, BUTTON_Y, buttonTexture, BUTTON_WIDTH);
            components.Add(purifyButton);

            classIcon = new ClassIcon("ClassIcon", this, CLASS_X, CLASS_Y, hero.heroClass);
            elementIcon = new ElementIcon("ElementIcon", this, CLASS_X, CLASS_Y + CLASS_ELEMENT_GAP, hero.heroElement);
            components.Add(classIcon);
            components.Add(elementIcon);
        }

        public override void DrawBegin(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw nothing.
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(x, y), Color.White);
            spriteBatch.DrawString(font, hero.name, new Vector2(x + NAME_X, y + NAME_Y), Color.White);
            hero.sprite.Draw(gameTime, spriteBatch, x + HERO_X, y + HERO_Y, Color.White, index: 0);
            for (int i= 0; i <= hero.rarity; i++)
            {
                spriteBatch.Draw(star, new Vector2(x + STAR_X, y + STAR_Y + STAR_GAP * i), Color.White);
            }
            for (int i = 0; i < 5; i++)
            {
                if (hero.skills[i] != null)
                    spriteBatch.Draw(hero.skills[i].icon, new Vector2(x + SKILL_X + SKILL_GAP * i, y + SKILL_Y), Color.White);
            }
            Toolbox.DrawAlignedString(spriteBatch, hero.GetDescription(), x + INFO_X, y + INFO_Y, font, Color.White, INFO_WIDTH);
            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }

        public void ChangeHero(Hero hero)
        {
            this.hero = hero;
            classIcon?.Finish();
            elementIcon?.Finish();
            classIcon = new ClassIcon("ClassIcon", this, CLASS_X, CLASS_Y, hero.heroClass);
            elementIcon = new ElementIcon("ElementIcon", this, CLASS_X, CLASS_Y + CLASS_ELEMENT_GAP, hero.heroElement);
            components.Add(classIcon);
            components.Add(elementIcon);
        }
    }
}
