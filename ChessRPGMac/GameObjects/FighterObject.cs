using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public abstract class FighterObject : GameObject, ISelectable
    {
        public Fighter fighter { get; protected set; }
        public delegate void AnimationFinishHandler();
        public AnimationFinishHandler animationFinishHandler;

        bool moving;
        Point movePoint;

        EffectLayer bottomEffectLayer;

        public FighterObject(int x, int y, Fighter fighter) : base(x, y)
        {
            this.fighter = fighter.Copy();
            this.collider = new NullCollider();
            bottomEffectLayer = (EffectLayer)Global.world.GetLayer("BottomEffectLayer");
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

        public virtual void Attacked()
        {
            ShakeAnimation animation = new ShakeAnimation();
            animation.animationFinishHandler = () => { animationFinishHandler(); };
            fighter.sprite.animation = animation;
        }

        public virtual void MeleeAttack()
        {
            MeleeAttackAnimation animation = new MeleeAttackAnimation(Direction.Up);
            animation.animationFinishHandler = () => { animationFinishHandler(); };
            fighter.sprite.animation = animation;
        }

        public virtual bool DealDamage(int pureDamage, int pierce = 0)
        {
            int actualDamage = fighter.HP;
            bool isDead = fighter.DealDamage(pureDamage, pierce);
            actualDamage = actualDamage - fighter.HP;
            DamageEffect damageEffect = new DamageEffect(actualDamage, x, y, 0);
            bottomEffectLayer.elements.Add(damageEffect);
            if (animationFinishHandler != null)
                damageEffect.EffectFinishEvent += (e) => { animationFinishHandler(); };

            return isDead;
        }

        public void MoveLocation(Point location)
        {
            if (location.x == x && location.y == y)
                return;
            moving = true;
            movePoint = location;
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

        public override void MeleeAttack()
        {
            fighter.sprite.animation = new MeleeAttackAnimation(Direction.Down);
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
        public override void Attacked()
        {
            throw new NotImplementedException();
        }

        public override bool DealDamage(int pureDamage, int pierce = 0)
        {
            throw new NotImplementedException();
        }

        public override void MeleeAttack()
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
