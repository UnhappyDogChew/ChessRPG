using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class HeroGages : GUIItem
    {
        public HeroObject heroObject { get; private set; }
        Texture2D gageTexture;

        int HP;
        float SP;
        float AP;

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
            if (heroObject.alive)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(gageTexture, new Vector2(x, y),
                    sourceRectangle: new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT), color: Color.White);
                // Draw Gagebars.
                spriteBatch.Draw(gageTexture, new Vector2(x + GAGEBARSTART_X, y + GAGEBARSTART_Y),
                    sourceRectangle: new Rectangle(0, FRAME_HEIGHT, (int)(GAGEBAR_WIDTH * ((float)HP / heroObject.maxHp)), GAGEBAR_HEIGHT),
                    color: Color.White);
                spriteBatch.Draw(gageTexture, new Vector2(x + GAGEBARSTART_X, y + GAGEBARSTART_Y + (GAGEBAR_HEIGHT + GAGEBAR_GAP)),
                    sourceRectangle: new Rectangle(0, FRAME_HEIGHT + GAGEBAR_HEIGHT, (int)(GAGEBAR_WIDTH * (SP / 100)), GAGEBAR_HEIGHT),
                    color: Color.White);
                spriteBatch.Draw(gageTexture, new Vector2(x + GAGEBARSTART_X, y + GAGEBARSTART_Y + (GAGEBAR_HEIGHT + GAGEBAR_GAP) * 2),
                    sourceRectangle: new Rectangle(0, FRAME_HEIGHT + GAGEBAR_HEIGHT * 2, (int)(GAGEBAR_WIDTH * (AP / 100)), GAGEBAR_HEIGHT),
                    color: Color.White);
                spriteBatch.End();
            }
            else
                Finish();
        }

        public override void Update(GameTime gameTime)
        {
            HP = heroObject.HP;
            SP = heroObject.SP;
            AP = heroObject.AP;
            relativeX = heroObject.x - FRAME_WIDTH / 2 - Global.camera.x;
            relativeY = heroObject.y + FIGHTER_GAGE_GAP - Global.camera.y;
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
            if (enemyObject.alive)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(gageTexture, new Vector2(x, y),
                    sourceRectangle: new Rectangle(0, 0, FRAME_WIDTH, FRAME_HEIGHT), color: Color.White);
                // Draw Gagebars.
                spriteBatch.Draw(gageTexture, new Vector2(x + GAGEBARSTART_X, y + GAGEBARSTART_Y),
                    sourceRectangle: new Rectangle(0, FRAME_HEIGHT, (int)(GAGEBAR_WIDTH * ((float)enemyObject.HP / enemy.maxHp)), GAGEBAR_HEIGHT),
                    color: Color.White);
                spriteBatch.Draw(gageTexture, new Vector2(x + GAGEBARSTART_X, y + GAGEBARSTART_Y + (GAGEBAR_HEIGHT + GAGEBAR_GAP)),
                    sourceRectangle: new Rectangle(0, FRAME_HEIGHT + GAGEBAR_GAP * 2, (int)(GAGEBAR_WIDTH * (enemyObject.AP / 100)), GAGEBAR_HEIGHT),
                    color: Color.White);
                spriteBatch.End();
            }
            else
                Finish();
        }

        public override void Update(GameTime gameTime)
        {
            relativeX = enemyObject.x - FRAME_WIDTH / 2 - Global.camera.x;
            relativeY = enemyObject.y - enemy.sprite.height - 20 - Global.camera.y;
        }
    }
}
