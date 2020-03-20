using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public abstract class FighterObject : GameObject, ISelectable
    {
        public Fighter fighter { get; protected set; }
        public List<Buff> buffList;
        public delegate void AnimationFinishHandler();
        public AnimationFinishHandler animationFinishHandler;

        bool moving;
        bool summoning;
        Point movePoint;

        EffectLayer bottomEffectLayer;

        public FighterObject(int x, int y, Fighter fighter) : base(x, y)
        {
            this.fighter = fighter.Copy();
            this.collider = new NullCollider();
            buffList = new List<Buff>();
            bottomEffectLayer = (EffectLayer)Global.world.GetLayer("BottomEffectLayer");
            summoning = true;
        }

        public override void Update(GameTime gameTime)
        {
            fighter.sprite.Update(gameTime);

            if (moving)
            {
                float xMovement = (movePoint.x - x) / 2.0f;
                float yMovement = (movePoint.y - y) / 2.0f;
                if (xMovement > 0 && xMovement < 1)
                    xMovement = 1;
                else if (xMovement < 0 && xMovement > -1)
                    xMovement = -1;
                if (yMovement > 0 && yMovement < 1)
                    yMovement = 1;
                else if (yMovement < 0 && yMovement > -1)
                    yMovement = -1;

                x += (int)xMovement;
                y += (int)yMovement;

                if (x == movePoint.x && y == movePoint.y)
                {
                    moving = false;
                    animationFinishHandler?.Invoke();
                }
            }
            if (summoning)
            {
                summoning = false;
            }

            // Remove finished buffs.
            for (int i = 0; i < buffList.Count; i++)
            {
                if (buffList[i].Finished)
                {
                    buffList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void AddBuff(Buff buff, int duration, int interval = 0)
        {
            buff.SetTarget(this);
            buff.SetDuration(duration);
            buff.SetInterval(interval);
            buffList.Add(buff);
        }

        public virtual Point GetLocation()
        {
            return new Point((int)(x - fighter.sprite.origin.X - Global.camera.x), 
                (int)(y - fighter.sprite.origin.Y - Global.camera.y));
        }

        public virtual Point GetSize()
        {
            return new Point(fighter.sprite.width, fighter.sprite.height);
        }

        public virtual void SelectAction()
        {
            if (Global.state is BattleState)
            {
                ((BattleState)Global.state).AddTargetBuffer(this);
            }
        }

        public virtual DamageEffect DealDamage(int pureDamage, int pierce = 0)
        {
            int actualDamage = fighter.HP;
            bool isDead = fighter.DealDamage(pureDamage, pierce);
            actualDamage = actualDamage - fighter.HP;
            DamageEffect damageEffect = new DamageEffect(actualDamage, x, y, 0);

            return damageEffect;
        }

        /// <summary>
        /// Moves the location.
        /// </summary>
        /// <returns><c>true</c>, if location was moved, <c>false</c> otherwise.</returns>
        /// <param name="location">Location.</param>
        public bool MoveLocation(Point location)
        {
            if (summoning)
            {
                x = location.x;
                y = location.y;
                return false;
            }
            if (location.x == x && location.y == y)
                return false;
            moving = true;
            movePoint = location;
            return true;
        }

        public void SetSpriteAnimation(SpriteAnimation animation)
        {
            fighter.sprite.animation = animation;
        }

        public void Kill()
        {
            // Dying animation should be implemented here.
            Finish();
        }

        public void Focus()
        {
        }

        public void Leave()
        {
        }
    }

    public class HeroObject : FighterObject
    {
        public Hero hero { get { return (Hero)fighter; } }

        public HeroObject(int x, int y, Hero hero) : base(x, y, hero)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            fighter.sprite.Draw(gameTime, spriteBatch, x, y, Color.White, index: 1);
        }
    }

    public class EnemyObject : FighterObject
    {
        public Enemy enemy { get { return (Enemy)fighter; } }

        public EnemyObject(int x, int y, Enemy hero) : base(x, y, hero)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            fighter.sprite.Draw(gameTime, spriteBatch, x, y, Color.White);
        }
    }

    public class NullFighterObject : FighterObject
    {
        const int WIDTH = 64;
        const int HEIGHT = 64;
        readonly Vector2 ORIGIN = new Vector2(32, 40);
        public FighterType type { get; private set; }

        public NullFighterObject(int x, int y, FighterType type, FighterState state) : base(x, y, new NullFighter(state))
        {
            this.type = type;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw nothing.
        }

        public override void Update(GameTime gameTime)
        {
            // Do nothing.
        }

        public override Point GetLocation()
        {
            return new Point((int)(x - ORIGIN.X - Global.camera.x),
                (int)(y - ORIGIN.Y - Global.camera.y));
        }

        public override Point GetSize()
        {
            return new Point(WIDTH, HEIGHT);
        }

        #region Not implemented methods
        public override DamageEffect DealDamage(int pureDamage, int pierce = 0)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
