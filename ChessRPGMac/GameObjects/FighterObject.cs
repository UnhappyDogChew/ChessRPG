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

        public int HP { get; protected set; }
        public float AP { get; protected set; }

        public bool alive { get; protected set; }
        public FighterState state { get; protected set; }

        // Final Status
        public int strength { get { return fighter.strength + str_increase; } }
        public int defense { get { return fighter.defense + def_increase; } }
        public int intelligence { get { return fighter.intelligence + int_increase; } }
        public float speed { get { return fighter.speed + spd_increase; } }
        public int maxHp { get { return fighter.maxHp + mhp_increase; } }

        // Status Increase
        int str_increase;
        int def_increase;
        int int_increase;
        float spd_increase;
        int mhp_increase;

        // 상태이상
        public bool paused { get; set; }
        public bool immovable { get; set; }
        public bool incurable { get; set; }

        bool moving;
        bool summoning;
        Point movePoint;

        EffectLayer bottomEffectLayer;

        public FighterObject(int x, int y, Fighter fighter, FighterState state) : base(x, y)
        {
            this.fighter = fighter;
            this.state = state;
            this.collider = new NullCollider();
            buffList = new List<Buff>();
            bottomEffectLayer = (EffectLayer)Global.world.GetLayer("BottomEffectLayer");
            summoning = true;

            alive = true;
            paused = false;
            immovable = false;
            incurable = false;

            Reset();
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
            foreach (Buff buffInList in buffList)
            {
                if (buff.GetType() == buffInList.GetType())
                {
                    buffInList.SetDuration(buffInList.duration + duration);
                    return;
                }
            }
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
            int actualDamage = (int)(pureDamage * (100.0f / (100 + defense - pierce)));
            DamageEffect damageEffect = new DamageEffect(actualDamage, x, y, 0);
            HP -= actualDamage;
            if (HP <= 0)
                alive = false;

            return damageEffect;
        }

        public virtual bool RestoreHP(int heal)
        {
            if (incurable)
                return false;

            HP += heal;
            if (HP > maxHp)
                HP = maxHp;

            return true;
        }

        public virtual bool IncreaseGage(int framePerSecond)
        {
            if (paused)
                return false;

            AP += (float)speed / framePerSecond;
            if (AP >= 100)
            {
                AP = 100;
                return true;
            }
            return false;
        }

        public void DecreaseAP(int ap)
        {
            AP -= ap;
            if (AP < 0)
                AP = 0;
        }

        public void ResetAP() { AP = 0; }

        public virtual void Reset()
        {
            HP = maxHp;
            AP = 0;
        }

        public virtual bool ChangeState(FighterState state)
        {
            if (state != FighterState.Stored && immovable)
                return false;

            this.state = state;
            return true;
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

        public float SP { get; protected set; }

        // Final Status
        public float mana { get { return hero.mana + mana_increase; } }

        // Status Increase
        float mana_increase;

        public HeroObject(int x, int y, Hero hero) : base(x, y, hero, hero.defaultFighterState)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            fighter.sprite.Draw(gameTime, spriteBatch, x, y, Color.White, index: 1);
        }

        public override void Reset()
        {
            HP = maxHp;
            SP = 0;
            AP = 0;
        }

        public override bool IncreaseGage(int framePerSecond)
        {
            IncreaseSP((float)mana / framePerSecond);

            return base.IncreaseGage(framePerSecond);
        }

        public void IncreaseSP(float increase)
        {
            SP += increase;
            if (SP > 100)
                SP = 100;
        }

        public void DecreaseSP(float decrease)
        {
            SP -= decrease;
            if (SP < 0)
                SP = 0;
        }
    }

    public class EnemyObject : FighterObject
    {
        public Enemy enemy { get { return (Enemy)fighter; } }
        public Hero[] souls { get; private set; }
        public int phase { get; protected set; }

        protected Queue<EnemySkill> pattern;

        public EnemyObject(int x, int y, Enemy hero, FighterState state, Hero[] souls) : base(x, y, hero, state)
        {
            this.souls = souls;
            phase = 0;
            pattern = new Queue<EnemySkill>();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            fighter.sprite.Draw(gameTime, spriteBatch, x, y, Color.White);
        }

        public override bool ChangeState(FighterState state)
        {
            if (state == FighterState.Stored)
                return false;

            return base.ChangeState(state);
        }

        public EnemySkill GetAction()
        {
            if (pattern.Count == 0)
                enemy.CreatePattern(pattern);
            return pattern.Dequeue();
        }

        public void PhaseCheck(BattleStage stage)
        {
            int p = enemy.PhaseCheck(stage);
            if (p == -1)
                return;
            phase = p;
        }
    }

    public class NullFighterObject : FighterObject
    {
        const int WIDTH = 64;
        const int HEIGHT = 64;
        readonly Vector2 ORIGIN = new Vector2(32, 40);
        public FighterType type { get; private set; }

        public NullFighterObject(int x, int y, FighterType type, FighterState state) : base(x, y, new NullFighter(), state)
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
