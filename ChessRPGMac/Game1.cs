using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace ChessRPGMac.MacOS
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        State state;
        Texture2D cursor;
        Song song;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Global.Properties.GAME_WIDTH;
            graphics.PreferredBackBufferHeight = Global.Properties.GAME_HEIGHT;
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Global.game = this;
            Global.world = new World("Stage1");
            Global.camera = new Camera();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Create a new SpriteBox.
            Global.spriteBox = new SpriteBox();

            cursor = Content.Load<Texture2D>("MouseCursor");
            SoundEffect se = Content.Load<SoundEffect>("Town-of-Forgotten-Souls");

            // Create layers.
            FileStream fs = (FileStream)TitleContainer.OpenStream("Content/Tilemaps/tilemap1.tm");
            GameObjectLayer GOLayer = new GameObjectLayer("GameObjectLayer");
            GUILayer guiLayer = new GUILayer("GUILayer");
            Tilemap tm = new Tilemap("tilemap1", fs);
            EffectLayer bottomEffectLayer = new EffectLayer("BottomEffectLayer", true);
            EffectLayer topEffectLayer = new EffectLayer("TopEffectLayer");

            // Add layers to world. The order is important.
            Global.world.AddLayers(tm, GOLayer, bottomEffectLayer, guiLayer, topEffectLayer);

            GOLayer.elements.Add(Player.GetPlayer(100, 100));
            GOLayer.elements.Add(new Boundary(300, 300));
            GOLayer.elements.Add(new Lantern(200, 200));
            List<Enemy> enemyFrontList = new List<Enemy>();
            List<Enemy> enemyBehindList = new List<Enemy>();
            enemyFrontList.Add(new Goul());
            GOLayer.elements.Add(new EnemySoul(400, 400, new Combat(enemyFrontList, enemyBehindList)));

            Global.soulBox = new SoulBox();

            Hero1 hero = new Hero1(FighterState.Front);
            Hero2 hero2 = new Hero2(FighterState.Front);
            Ring1 ring1 = new Ring1();
            Global.world.GetPlayer().AddHero(hero);
            Global.world.GetPlayer().AddHero(hero2);
            Global.world.GetPlayer().AddItem(ring1);

            ChangeState(new ExploreState());
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            state.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            Global.world.Draw(gameTime, spriteBatch);

            // Draw mouse cursor.
            spriteBatch.Begin();
            spriteBatch.Draw(cursor, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ChangeState(State state)
        {
            this.state = state;
            Global.state = state;
            state.stateChangeMethod = ChangeState;
        }
    }
}
