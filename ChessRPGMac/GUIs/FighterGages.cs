using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class HeroGages : GUIItem
    {
        public HeroObject heroObject { get; private set; }
        Texture2D gageTexture;

        Hero hero { get { return heroObject.hero; } }

        const int FRAME_WIDTH = 54;
        const int FRAME_HEIGHT = 20;
        const int GAGEBAR_WIDTH = 50;
        const int GAGEBAR_HEIGHT = 4;
        const int GAGEBARSTART_X = 2;
        const int GAGEBARSTART_Y = 2;
        const int GAGEBAR_GAP = 2;
        const int FIGHTER_GAGE_GAP = -60;

        public HeroGages(string name, GUIComponent parent, HeroObject heroObject) : base(name, parent, 0, 0)
        {
            this.heroObject = heroObject;
            gageTexture = Global.content.Load<Texture2D>("HeroGages");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (hero.alive)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(gageTexture, new Vector2(x, y),
                    sourceRectangle: new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT), color: Color.White);
                // Draw Gagebars.
                spriteBatch.Draw(gageTexture, new Vector2(x + GAGEBARSTART_X, y + GAGEBARSTART_Y),
                    sourceRectangle: new Rectangle(0, FRAME_HEIGHT, (int)(GAGEBAR_WIDTH * ((float)hero.HP / hero.maxHp)), GAGEBAR_HEIGHT),
                    color: Color.White);
                spriteBatch.Draw(gageTexture, new Vector2(x + GAGEBARSTART_X, y + GAGEBARSTART_Y + (GAGEBAR_HEIGHT + GAGEBAR_GAP)),
                    sourceRectangle: new Rectangle(0, FRAME_HEIGHT + GAGEBAR_HEIGHT, (int)(GAGEBAR_WIDTH * (hero.SP / 100)), GAGEBAR_HEIGHT),
                    color: Color.White);
                spriteBatch.Draw(gageTexture, new Vector2(x + GAGEBARSTART_X, y + GAGEBARSTART_Y + (GAGEBAR_HEIGHT + GAGEBAR_GAP) * 2),
                    sourceRectangle: new Rectangle(0, FRAME_HEIGHT + GAGEBAR_HEIGHT * 2, (int)(GAGEBAR_WIDTH * (hero.AP / 100)), GAGEBAR_HEIGHT),
                    color: Color.White);
                spriteBatch.End();
            }
            else
                Finish();
        }

        public override void Update(GameTime gameTime)
        {
            relativeX = heroObject.x - FRAME_WIDTH / 2;
            relativeY = heroObject.y + FIGHTER_GAGE_GAP;
        }
    }

    public class EnemyGages : GUIItem
    {
        public EnemyObject enemyObject { get; private set; }
        Texture2D gageTexture;

        Enemy enemy { get { return enemyObject.enemy; } }

        const int FRAME_WIDTH = 54;
        const int FRAME_HEIGHT = 14;
        const int GAGEBAR_WIDTH = 50;
        const int GAGEBAR_HEIGHT = 4;
        const int GAGEBARSTART_X = 2;
        const int GAGEBARSTART_Y = 2;
        const int GAGEBAR_GAP = 2;

        public EnemyGages(string name, GUIComponent parent, EnemyObject enemyObject) : base(name, parent, 0, 0)
        {
            this.enemyObject = enemyObject;
            gageTexture = Global.content.Load<Texture2D>("EnemyGages");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (enemy.alive)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(gageTexture, new Vector2(x, y),
                    sourceRectangle: new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT), color: Color.White);
                // Draw Gagebars.
                spriteBatch.Draw(gageTexture, new Vector2(x + GAGEBARSTART_X, y + GAGEBARSTART_Y),
                    sourceRectangle: new Rectangle(0, FRAME_HEIGHT, (int)(GAGEBAR_WIDTH * ((float)enemy.HP / enemy.maxHp)), GAGEBAR_HEIGHT),
                    color: Color.White);
                spriteBatch.Draw(gageTexture, new Vector2(x + GAGEBARSTART_X, y + GAGEBARSTART_Y + (GAGEBAR_HEIGHT + GAGEBAR_GAP)),
                    sourceRectangle: new Rectangle(0, FRAME_HEIGHT + GAGEBAR_GAP * 2, (int)(GAGEBAR_WIDTH * (enemy.AP / 100)), GAGEBAR_HEIGHT),
                    color: Color.White);
                spriteBatch.End();
            }
            else
                Finish();
        }

        public override void Update(GameTime gameTime)
        {
            relativeX = enemyObject.x - FRAME_WIDTH / 2;
            relativeY = enemyObject.y - enemy.sprite.height - 20;
        }
    }
}
