using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChessRPGMac
{
    /// <summary>
    /// SpriteBox creates all sprites needed in this game.
    /// </summary>
    public class SpriteBox
    {
        private ContentManager content;
        private GraphicsDevice graphicsDevice;
        private readonly Dictionary<string, Sprite> Sprites;

        public SpriteBox()
        {
            this.content = Global.game.Content;
            this.graphicsDevice = Global.game.GraphicsDevice;
            Sprites = new Dictionary<string, Sprite>();

            // Variables for constructions.
            int width;
            int height;
            string name;
            Vector2 origin;

            // !--- All Sprite Consturctions need to be declared here ---!

            #region Player Sprites
            width = 64; height = 64; origin = new Vector2(31, 58);
            List<Texture2D> playerTextures =
                Toolbox.SplitSpriteSheet(content.Load<Texture2D>("Player"), width * 4, height);

            name = "PlayerNoLantern_Down";
            Sprites[name] = new Sprite(playerTextures[0], width, height, origin, animating: false, interval: 10);
            name = "PlayerNoLantern_Up";
            Sprites[name] = new Sprite(playerTextures[1], width, height, origin, animating: false, interval: 10);
            name = "PlayerNoLantern_Right";
            Sprites[name] = new Sprite(playerTextures[2], width, height, origin, animating: false, interval: 10);
            name = "PlayerNoLantern_Left";
            Texture2D leftTexture = Toolbox.CopyTexture(playerTextures[2]);
            Toolbox.FlipTextureHorizontal(leftTexture);
            Sprites[name] = new Sprite(leftTexture, width, height, origin, animating: false, reverse: true, interval: 10);

            name = "PlayerWithLantern_Down";
            Sprites[name] = new Sprite(playerTextures[3], width, height, origin, animating: false, interval: 10);
            name = "PlayerWithLantern_Up";
            Sprites[name] = new Sprite(playerTextures[4], width, height, origin, animating: false, interval: 10);
            name = "PlayerWithLantern_Right";
            Sprites[name] = new Sprite(playerTextures[5], width, height, origin, animating: false, interval: 10);
            name = "PlayerWithLantern_Left";
            leftTexture = Toolbox.CopyTexture(playerTextures[5]);
            Toolbox.FlipTextureHorizontal(leftTexture);
            Sprites[name] = new Sprite(leftTexture, width, height, origin, animating: false, reverse: true, interval: 10);
            #endregion

            #region Environments
            origin = new Vector2(32, 40); name = "PlunckWithFlower";
            Sprites[name] = new Sprite(content.Load<Texture2D>("Plunck with Flower"), origin);
            origin = new Vector2(16, 27); name = "Lantern";
            Sprites[name] = new Sprite(content.Load<Texture2D>("Lantern"), origin);
            #endregion

            #region GUIs
            origin = new Vector2(0, 84); name = "Textbox";
            Sprites[name] = new Sprite(content.Load<Texture2D>(name), origin);
            origin = Vector2.Zero; name = "ActionSelectPage";
            Sprites[name] = new Sprite(content.Load<Texture2D>(name), ActionSelect.WIDTH, ActionSelect.HEIGHT, 
                origin, repeate: false, interval: 5);
            #endregion

            #region Heros
            List<Texture2D> heroSheet = Toolbox.SplitSpriteSheet(content.Load<Texture2D>("HeroSheet"), 64 * 2, 64);
            origin = new Vector2(32, 40); name = "Hero1";
            Sprites[name] = new Sprite(heroSheet[0], 64, 64, origin, animating: false);
            origin = new Vector2(32, 40); name = "Hero2";
            Sprites[name] = new Sprite(heroSheet[1], 64, 64, origin, animating: false);
            #endregion

            #region Enemies
            origin = new Vector2(33, 60); name = "Goul";
            Sprites[name] = new Sprite(content.Load<Texture2D>("goul"), origin);
            #endregion

            #region Effects
            origin = new Vector2(32, 32); name = "SlashEffect";
            Sprites[name] = new Sprite(content.Load<Texture2D>(name), 64, 64, origin, interval: 5, repeate: false);
            origin = new Vector2(32, 40); name = "Frozen";
            Sprites[name] = new Sprite(content.Load<Texture2D>(name), origin);
            #endregion
        }
        /// <summary>
        /// Pick a sprite with specific name. 
        /// </summary>
        /// <returns>The pick.</returns>
        /// <param name="name">Name.</param>
        public Sprite Pick(string name)
        {
            if (Sprites.ContainsKey(name))
                return Sprites[name];
            else
                throw new Exception("There's no sprite with that name.");
        }
        /// <summary>
        /// Clone a sprite with specific name.
        /// </summary>
        /// <returns>The clone.</returns>
        /// <param name="name">Name.</param>
        public Sprite Clone(string name)
        {
            if (Sprites.ContainsKey(name))
                return Sprites[name].Copy();
            else
                throw new Exception("There's no sprite with that name.");
        }
    }
}
