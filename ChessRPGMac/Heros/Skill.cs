using System;

using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public abstract class Skill : Action
    {
        public Texture2D icon { get; protected set; }
        public Hero user { get; protected set; }
        public int manaUsage { get; protected set; }
        public bool isActive { get; protected set; }

        public Skill(Hero user) { this.user = user; }

        public virtual bool IsAvailable(BattleStage stage)
        {
            return user.SP >= manaUsage;
        }
    }
    /// <summary>
    /// Melee attack. Only attacks front enemy. 
    /// Only Usable when user is in front.
    /// </summary>
    public class MeleeAttack : Skill
    {
        public MeleeAttack(Hero user) : base(user)
        {
            icon = Global.content.Load<Texture2D>("MeleeAttack");
            name = "Melee Attack";
            manaUsage = 0;
            isActive = true;
            targetType = TargetType.OneEnemyFront;
        }

        public override bool IsAvailable(BattleStage stage)
        {
            return user.state == FighterState.Front;
        }

        public override void PreAction(BattleStage stage, FighterObject user, FighterObject target)
        {
            SlashEffect effect = new SlashEffect(target.x, target.y, 0);
            bottomEffectLayer.elements.Add(effect);
            user.MeleeAttack();
            effect.EffectFinishEvent += (e) => DoAction(stage, user, target);
        }

        public override void DoAction(BattleStage stage, FighterObject user, FighterObject target)
        {
            target.Attacked();
            target.animationFinishHandler = () => { PostAction(stage, user, target); };
            target.DealDamage(this.user.strength);
        }

        public override string GetDescription()
        {
            return "Attack front enemy.";
        }
    }
    /// <summary>
    /// Range attack. Can attack behind enemy.
    /// </summary>
    public class RangeAttack : Skill
    {
        public RangeAttack(Hero user) : base(user)
        {
            icon = Global.content.Load<Texture2D>("RangeAttack");
            name = "Range Attack";
            manaUsage = 0;
            isActive = true;
            targetType = TargetType.OneEnemyFront;
        }

        public override void DoAction(BattleStage stage, FighterObject user, FighterObject target)
        {
            target.DealDamage(this.user.strength);

            base.DoAction(stage, user, target);
        }

        public override string GetDescription()
        {
            return "Attack front enemy. Can attack when user is in behind.";
        }
    }
}
