using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChessRPGMac
{
    public class SoulPurifyState : State
    {
        List<EnemyObject> deadEnemyObjects;
        List<Soul> souls;
        List<IClickable> clickables;

        GameObjectLayer gameObjectLayer;
        GUILayer guiLayer;

        HeroDetail heroDetail;
        SoulPurification soulPurification;

        const int HERO_DETAIL_X = 100;
        const int HERO_DETAIL_Y = 100;
        const int SOUL_PURIFICATION_X = 120;
        const int SOUL_PURIFICATION_Y = 200;

        public SoulPurifyState(List<EnemyObject> deadEnemyObjects)
        {
            this.deadEnemyObjects = deadEnemyObjects;
            CreateSouls();
        }

        public SoulPurifyState(List<EnemyObject> deadEnemyObjects, List<Keys> pressedKeys) : base(pressedKeys)
        {
            this.deadEnemyObjects = deadEnemyObjects;
            CreateSouls();
        }

        public override void Update(GameTime gameTime)
        {
            bool atBlankSpace = true;
            MouseState mouse = Mouse.GetState();
            for (int i = 0; i < clickables.Count; i++)
            {
                IClickable clickable = clickables[i];
                if (Toolbox.IsPointInsideSquare(new Point(mouse.X, mouse.Y), clickable.GetBoundary()))
                {
                    atBlankSpace = false;
                    clickable.MouseEnter();
                    if (mouse.LeftButton == ButtonState.Pressed)
                        clickable.Click();
                    else
                        clickable.Release();
                }
                else
                    clickable.MouseLeave();
            }
            if (atBlankSpace && mouse.LeftButton == ButtonState.Pressed)
                heroDetail?.Deactivate();

            base.Update(gameTime);
        }

        protected override void Prepare()
        {
            gameObjectLayer = (GameObjectLayer)Global.world.GetLayer("GameObjectLayer");
            guiLayer = (GUILayer)Global.world.GetLayer("GUILayer");
            souls = new List<Soul>();
            clickables = new List<IClickable>();

            base.Prepare();
        }

        private void CreateSouls()
        {
            foreach (EnemyObject enemyObject in deadEnemyObjects)
            {
                for (int i = 0; i < enemyObject.souls.Length; i++)
                {
                    int r = 64;
                    double theta = i * (2 * Math.PI / enemyObject.souls.Length);
                    Soul soul = new Soul(enemyObject.souls[i], enemyObject.x, enemyObject.y);
                    souls.Add(soul);
                    soul.MoveLocationSmooth(new Point(enemyObject.x + (int)(r * Math.Cos(theta)), enemyObject.y + (int)(r * Math.Sin(theta))), 4.0f);
                }
            }

            foreach (Soul soul in souls)
            {
                soul.Clicked += (sender, e) => { OpenHeroDetail(((Soul)sender).hero); };
                clickables.Add(soul);
                gameObjectLayer.elements.Add(soul);
            }
        }

        private void OpenHeroDetail(Hero hero)
        {
            if (heroDetail == null)
            {
                heroDetail = new HeroDetail("HeroDetail", null, HERO_DETAIL_X, HERO_DETAIL_Y, hero);
                guiLayer.MainGroup.AddComponent(heroDetail);
                clickables.Add(heroDetail.purifyButton);
                heroDetail.purifyButton.Released += (sender, e) => { OpenSoulPurification(hero); };
            }
            else
                heroDetail.ChangeHero(hero);

            heroDetail.Activate();
        }

        private void OpenSoulPurification(Hero hero)
        {
            if (soulPurification == null)
            {
                soulPurification = new SoulPurification("SoulPurification", null, SOUL_PURIFICATION_X, SOUL_PURIFICATION_Y, hero);
                guiLayer.MainGroup.AddComponent(soulPurification);
                foreach (CheckBox checkBox in soulPurification.soulCheckBoxes)
                {
                    clickables.Add(checkBox);
                }
                clickables.Add(soulPurification.purifyButton);
                clickables.Add(soulPurification.exitButton);
            }
            else
                soulPurification.SetHero(hero);

            soulPurification.Activate();
        }

        private void CloseHeroDetail()
        {
            heroDetail.Deactivate();
        }
    }
}
