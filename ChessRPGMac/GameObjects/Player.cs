using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ChessRPGMac
{
    /// <summary>
    /// Player. Singleton pattern.
    /// </summary>
    public class Player : GameObject
    {
        public static Player instance;
        /// <summary>
        /// AnimationSprite array of player walk animation. sequence is down, up, right, left
        /// </summary>
        public Sprite currentSprite { get; private set; }
        public List<Hero> heros { get; private set; }
        public Item[,] items { get; private set; }

        public int HeroFrontAmount { get; private set; }
        public int HeroBehindAmount { get; private set; }
        public int HeroStoredAmount { get; private set; }
        public int TeamSize { get; private set; }

        public static readonly int HEROFRONT_MAX = 5;
        public static readonly int HEROBEHIND_MAX = 5;
        public static readonly int HEROSTORED_MAX = 5;
        public static readonly int ITEM_WIDTH = 7;
        public static readonly int ITEM_HEIGHT = 3;
        public static readonly int SKILL_MAX = 5;
        public static int ITEM_MAX { get { return ITEM_WIDTH * ITEM_HEIGHT; } }

        private int maxHP = 100;

        public int HP { get; private set; }
        public int lanternEnergy { get; set; }
        public int gold { get; set; }

        public bool alive { get; private set; }

        private Sprite[] spritesNoLantern;
        private Sprite[] spritesWithLantern;
        private int direction;
        private int moveSpeed;
        private bool walking;
        private bool holdingLantern;

        private const int COLLIDER_WIDTH = 30;
        private const int COLLIDER_HEIGTH = 16;

        SoundEffect grassWalk_soundEffect;
        bool soundPlaying;

        int timespan_sound;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ChessRPGMac.Player"/> class.
        /// It must be called in <see cref="Game.LoadContent"/> method.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        private Player(int x, int y) : base(x, y)
        {
            grassWalk_soundEffect = Global.content.Load<SoundEffect>("SE_GrassWalk");

            // Get Sprites from SpriteBox.
            spritesNoLantern = new Sprite[4];
            spritesNoLantern[0] = Global.spriteBox.Pick("PlayerNoLantern_Down");
            spritesNoLantern[1] = Global.spriteBox.Pick("PlayerNoLantern_Up");
            spritesNoLantern[2] = Global.spriteBox.Pick("PlayerNoLantern_Right");
            spritesNoLantern[3] = Global.spriteBox.Pick("PlayerNoLantern_Left");

            spritesWithLantern = new Sprite[4];
            spritesWithLantern[0] = Global.spriteBox.Pick("PlayerWithLantern_Down");
            spritesWithLantern[1] = Global.spriteBox.Pick("PlayerWithLantern_Up");
            spritesWithLantern[2] = Global.spriteBox.Pick("PlayerWithLantern_Right");
            spritesWithLantern[3] = Global.spriteBox.Pick("PlayerWithLantern_Left");

            collider = new SquareCollider(x, y, COLLIDER_WIDTH, COLLIDER_HEIGTH, COLLIDER_WIDTH / 2, COLLIDER_HEIGTH / 2, true);

            heros = new List<Hero>();
            items = new Item[ITEM_HEIGHT, ITEM_WIDTH];
            HP = maxHP;
            lanternEnergy = 0;
            gold = 0;
            alive = true;
            direction = 0;
            walking = false;
            holdingLantern = false;
            moveSpeed = 3;

            TeamSize = 3;

            currentSprite = spritesNoLantern[direction];
        }
        /// <summary>
        /// Static constructor of singleton pattern.
        /// Initializes a new instance of the <see cref="T:ChessRPGMac.Player"/> class.
        /// It must be called in <see cref="Game.LoadContent"/> method.
        /// </summary>
        /// <returns>The player.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public static Player GetPlayer(int x, int y)
        {
            if (instance == null)
            {
                instance = new Player(x, y);
            }
            return instance;
        }
        /// <summary>
        /// Draw player.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        /// <param name="spriteBatch">Sprite batch.</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Toolbox.DrawSquare(spriteBatch, x, y, COLLIDER_WIDTH, COLLIDER_HEIGTH,
                    new Vector2(COLLIDER_WIDTH / 2, COLLIDER_HEIGTH / 2), Color.Bisque);
            currentSprite.Draw(gameTime, spriteBatch, x, y, Color.White, depth: depth);
        }
        /// <summary>
        /// Update player.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public override void Update(GameTime gameTime)
        {
            // Move Character.
            if (walking)
            {
                if (direction == 0 || direction == 1)
                    y -= (direction * 2 - 1) * moveSpeed;
                else if (direction == 2 || direction == 3)
                    x -= ((direction - 2) * 2 - 1) * moveSpeed;

                timespan_sound--;
                if (timespan_sound <= 0)
                {
                    grassWalk_soundEffect.Play(0.5f, 0, 0);
                    timespan_sound = currentSprite.interval * 2;
                }
            }
            else
                timespan_sound = 0;

            // Set Collider position.
            ((SquareCollider)collider).x = x;
            ((SquareCollider)collider).y = y;

            // Check collisions with other solid colliders.
            foreach (GameObjectLayer layer in Global.world.GameObjectLayers)
            {
                foreach (GameObject gameObject in layer.elements)
                {
                    // skip self detection.
                    if (gameObject == this)
                       continue;
                    Collider otherCollider = gameObject.collider;
                    if (gameObject.collider.solid && collider.Detect(otherCollider))
                    {
                        switch ((Direction)direction)
                        {
                            case Direction.Up: y -= collider.GetAdjacentDistance(otherCollider, Direction.Up); break;
                            case Direction.Down: y += collider.GetAdjacentDistance(otherCollider, Direction.Down); break;
                            case Direction.Left: x -= collider.GetAdjacentDistance(otherCollider, Direction.Left); break;
                            case Direction.Right: x += collider.GetAdjacentDistance(otherCollider, Direction.Right); break;
                        }
                        Stand();
                        break;
                    }
                }
            }

            // Set Collider position again.
            ((SquareCollider)collider).x = x;
            ((SquareCollider)collider).y = y;

            // Set Heros
            SetHeroAmounts();

            currentSprite.Update(gameTime);
        }
        /// <summary>
        /// Adds the hero. Front row first, Behind row second, Stored last.
        /// </summary>
        /// <returns><c>true</c>, if hero was added, <c>false</c> otherwise.</returns>
        /// <param name="hero">Hero.</param>
        public bool AddHero(Hero hero)
        {
            SetHeroAmounts();
            if (HeroFrontAmount + HeroBehindAmount < TeamSize)
            {
                heros.Add(hero);
                if (HeroFrontAmount < HEROFRONT_MAX)
                {
                    hero.ChangeDefaultState(FighterState.Front);
                    HeroFrontAmount++;
                }
                else
                {
                    hero.ChangeDefaultState(FighterState.Behind);
                    HeroBehindAmount++;
                }
                return true;
            }
            else if (HeroStoredAmount < HEROSTORED_MAX)
            {
                heros.Add(hero);
                hero.ChangeDefaultState(FighterState.Stored);
                HeroStoredAmount++;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <returns><c>true</c>, if item was added, <c>false</c> otherwise.</returns>
        /// <param name="item">Item.</param>
        public bool AddItem(Item item)
        {
            for (int row = 0; row < ITEM_HEIGHT; row++)
            {
                for (int col = 0; col < ITEM_WIDTH; col++)
                {
                    if (items[row, col] != null)
                        continue;
                    items[row, col] = item;
                    return true;
                }
            }
            return false;
        }
        public bool RemoveItem(int col, int row)
        {
            try
            {
                if (items[row, col] == null)
                    return false;
                items[row, col] = null;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Moves the item.
        /// </summary>
        /// <returns><c>true</c>, if item was moved, <c>false</c> otherwise.</returns>
        /// <param name="col1">Col1.</param>
        /// <param name="row1">Row1.</param>
        /// <param name="col2">Col2.</param>
        /// <param name="row2">Row2.</param>
        public bool MoveItem(int col1, int row1, int col2, int row2)
        {
            try
            {
                if (items[row2, col2] == null)
                {
                    items[row2, col2] = items[row1, col1];
                    items[row1, col1] = null;
                }
                else
                {
                    Item temp = items[row2, col2];
                    items[row2, col2] = items[row1, col1];
                    items[row1, col1] = temp;
                }
            }
            catch(IndexOutOfRangeException)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Makes player starts to walk in given direction.
        /// </summary>
        /// <param name="direction">Direction.</param>
        public void Walk(Direction direction)
        {
            this.direction = (int)direction;

            SetSprite();

            walking = true;
            currentSprite.Start();
            currentSprite.index = 1;
        }
        /// <summary>
        /// Makes player stop moving and stand still. 
        /// </summary>
        public void Stand()
        {
            walking = false;
            currentSprite.Stop();
        }
        public void Look(Direction direction)
        {
            this.direction = (int)direction;
            SetSprite();
        }
        /// <summary>
        /// Gives damage to player. if HP becomes lower than 0, <see cref="Player.Die"/> method are called.
        /// </summary>
        /// <param name="damage">Damage.</param>
        public void GiveDamage(int damage)
        {
            HP -= damage;
            if (HP < 0)
            {
                HP = 0;
                Die();
            }
        }
        /// <summary>
        /// Heals player health. cannot exceed maxHP.
        /// </summary>
        /// <param name="heal">Heal amount.</param>
        public void Heal(int heal)
        {
            HP += heal;
            if (HP > maxHP)
            {
                HP = maxHP;
            }
        }
        /// <summary>
        /// Holds the lantern.
        /// </summary>
        public void HoldLantern()
        {
            holdingLantern = true;
            SetSprite();
        }
        /// <summary>
        /// Sets player sprite.
        /// </summary>
        private void SetSprite()
        {
            if (holdingLantern)
                currentSprite = spritesWithLantern[direction];
            else
                currentSprite = spritesNoLantern[direction];
        }
        private void SetHeroAmounts()
        {
            HeroFrontAmount = 0;
            HeroBehindAmount = 0;
            HeroStoredAmount = 0;
            foreach (Hero hero in heros)
            {
                switch (hero.defaultFighterState)
                {
                    case FighterState.Front: HeroFrontAmount++; break;
                    case FighterState.Behind: HeroBehindAmount++; break;
                    case FighterState.Stored: HeroStoredAmount++; break;
                }
            }
        }
        /// <summary>
        /// Makes player die.
        /// </summary>
        private void Die()
        {
            alive = false;
        }
        /// <summary>
        /// Gets player direction.
        /// </summary>
        /// <returns>The direction.</returns>
        public int GetDirection() => direction;
    }
}
