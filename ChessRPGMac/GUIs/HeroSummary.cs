using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class HeroSummary : GUIGroup, IDragable
    {
        public Hero hero { get; private set; }
        Texture2D background;
        Texture2D[] soul;
        Texture2D[] levels;
        RenderTarget2D target;
        SpriteFont font;

        public static readonly int WIDTH = 100;
        public static readonly int HEIGHT = 50;

        const int HEROLOCATION_X = 74;
        const int HEROLOCATION_Y = 49;
        const int SOULSTART_X = 88;
        const int SOULSTART_Y = 11;
        const int SOULTERM = 12;
        const int LEVELSTART_X = 71;
        const int LEVELSTART_Y = 0;
        const int CLASSSTART_X = 3;
        const int CLASSSTART_Y = 17;
        const int CLASSGAP = 16;

        public HeroSummary(string name, GUIComponent parent, int rx, int ry, Hero hero) : base(name, parent, rx, ry)
        {
            this.hero = hero;
            soul = new Texture2D[2];
            levels = new Texture2D[5];
            background = Global.content.Load<Texture2D>("HeroSummaryBackground");
            soul[0] = Global.content.Load<Texture2D>("HeroSummarySoulEmpty");
            soul[1] = Global.content.Load<Texture2D>("HeroSummarySoulFull");
            levels[0] = Global.content.Load<Texture2D>("HeroLevel1");
            levels[1] = Global.content.Load<Texture2D>("HeroLevel2");
            levels[2] = Global.content.Load<Texture2D>("HeroLevel3");
            levels[3] = Global.content.Load<Texture2D>("HeroLevel4");
            levels[4] = Global.content.Load<Texture2D>("HeroLevel5");
            font = Global.content.Load<SpriteFont>("neodgm12");
            components.Add(new ClassIcon(hero.heroClass.ToString(), this, CLASSSTART_X, CLASSSTART_Y, hero.heroClass));
            components.Add(new ElementIcon(hero.heroElement.ToString(), this, CLASSSTART_X, CLASSSTART_Y + CLASSGAP, hero.heroElement));

        }

        public override void DrawBegin(GameTime gameTime, SpriteBatch spriteBatch)
        {
            target = new RenderTarget2D(Global.graphics, WIDTH, HEIGHT);
            Global.graphics.SetRenderTarget(target);
            Global.graphics.Clear(new Color(0, 0, 0, 0));
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            hero.sprite.Draw(gameTime, spriteBatch, HEROLOCATION_X, HEROLOCATION_Y, Color.White, index: 0);
            for (int i = 0; i <= hero.level; i++)
            {
                for (int s = 0; s < Hero.SoulPerLevel[i]; s++)
                {
                    int si = 0;
                    if (hero.soul > s)
                        si = 1;
                    spriteBatch.Draw(soul[si], new Vector2(SOULSTART_X - (SOULTERM * s), SOULSTART_Y), Color.White);
                }
            }
            spriteBatch.Draw(levels[hero.level - 1], new Vector2(LEVELSTART_X, LEVELSTART_Y), Color.White);
            spriteBatch.DrawString(font, hero.name, new Vector2(2, 2), Toolbox.ParseColor("#5e3643ff"));
            foreach (GUIComponent component in components)
            {
                component.DrawBegin(gameTime, spriteBatch);
            }
            spriteBatch.End();

            Global.graphics.SetRenderTarget(null);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(target, new Vector2(x, y), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public Texture2D GetTexture()
        {
            Texture2D result = Toolbox.CopyTexture(target);
            Toolbox.SetTextureAlpha(result, 125);
            return result;
        }

        public Vector2 GetOrigin(int x, int y)
        {
            return new Vector2(x - this.x, y - this.y);
        }

        public bool IsInside(int x, int y)
        {
            return Toolbox.IsPointInsideSquare(this.x, this.y, this.x + WIDTH, this.y + HEIGHT, x, y);
        }

        public void ChangeSocket(DragSocket socket)
        {
            relativeX = 0;
            relativeY = 0;
            parent = socket;

            switch (((DragSocket)parent).name)
            {
                case "HeroFront": hero.ChangeDefaultState(FighterState.Front); break;
                case "HeroBehind": hero.ChangeDefaultState(FighterState.Behind); break;
                case "HeroStored": hero.ChangeDefaultState(FighterState.Stored); break;
            }
        }

        public string GetKey()
        {
            switch (hero.defaultFighterState)
            {
                case FighterState.Front: return "HeroFront";
                case FighterState.Behind: return "HeroBehind";
                case FighterState.Stored: return "HeroStored";
            }
            return null;
        }

        /// <summary>
        /// Rerturns reference of parent socket. If parent is not a <see cref="DragSocket"/>, returns null.
        /// </summary>
        /// <returns>The socket.</returns>
        public DragSocket GetSocket()
        {
            if (parent is DragSocket)
                return (DragSocket)parent;
            else
                return null;
        }
    }
}
