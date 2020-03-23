using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public abstract class Skill : Action
    {
        public Texture2D icon { get; protected set; }
        public int manaUsage { get; protected set; }
        public int hpUsage { get; protected set; }
        public bool isActive { get; protected set; }

        public override bool IsAvailable(BattleStage stage, FighterObject user)
        {
            return ((HeroObject)user).SP >= manaUsage;
        }

        public override void Execute(BattleStage stage, FighterObject user, List<FighterObject> targetList, ActionFinishHandler handler)
        {
            ((HeroObject)user).DecreaseSP(manaUsage);
        }

        public abstract string GetDescription();
    }
    /// <summary>
    /// Melee attack. Only attacks front enemy. 
    /// Only Usable when user is in front.
    /// </summary>
    public class MeleeAttack : Skill
    {
        public MeleeAttack()
        {
            icon = Global.content.Load<Texture2D>("MeleeAttack");
            name = "Melee Attack";
            manaUsage = 0;
            isActive = true;
            targetType = TargetType.OneEnemyFront;
        }

        public override bool IsAvailable(BattleStage stage, FighterObject user)
        {
            return user.state == FighterState.Front;
        }

        public override void Execute(BattleStage stage, FighterObject user, List<FighterObject> targetList, ActionFinishHandler handler)
        {
            PresentationGroup p = new PresentationGroup(2, stage, user, targetList, handler);
            p[0].AddEffect(new SlashEffect(targetList[0].x, targetList[0].y, 0));
            p[0].SetUserAnimation(SpriteAnimation.GetSpriteAnimation("MeleeAttackUp"));

            p[1].SetTargetAnimation(SpriteAnimation.GetSpriteAnimation("Shake"));
            p[1].AddEffect(targetList[0].DealDamage(((Hero)user.fighter).strength));

            p.Start();

            base.Execute(stage, user, targetList, handler);
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
        public RangeAttack()
        {
            icon = Global.content.Load<Texture2D>("RangeAttack");
            name = "Range Attack";
            manaUsage = 0;
            isActive = true;
            targetType = TargetType.OneEnemyFront;
        }

        public override void Execute(BattleStage stage, FighterObject user, List<FighterObject> targetList, ActionFinishHandler handler)
        {
            PresentationGroup p = new PresentationGroup(2, stage, user, targetList, handler);
            p[0].AddEffect(new SlashEffect(targetList[0].x, targetList[0].y, 0));
            p[0].SetUserAnimation(SpriteAnimation.GetSpriteAnimation("MeleeAttackUp"));

            p[1].AddEffect(targetList[0].DealDamage(user.fighter.strength));
            p[1].SetTargetAnimation(SpriteAnimation.GetSpriteAnimation("Shake"));

            p.Start();

            base.Execute(stage, user, targetList, handler);
        }

        public override string GetDescription()
        {
            return "Attack front enemy. Can attack when user is in behind.";
        }
    }

    public class MoveAction : Skill
    {
        public MoveAction()
        {
            icon = Global.content.Load<Texture2D>("HealIcon");
            name = "Move";
            manaUsage = 0;
            targetType = TargetType.OneAlleyOtherLine;
            isActive = true;
        }

        public override bool IsAvailable(BattleStage stage, FighterObject user)
        {
            if (user.state == FighterState.Front)
                return (stage.fighterLists[3].Count < 5 && (stage.fighterLists[2].Count > 1 || 
                    (stage.fighterLists[2].Count == 1 && stage.fighterLists[3].Count > 0)));
            else
                return stage.fighterLists[2].Count < 5;
        }

        public override string GetDescription()
        {
            return "Move to other line or switch line with other alley.";
        }

        public override void Execute(BattleStage stage, FighterObject user, List<FighterObject> targetList, ActionFinishHandler handler)
        {
            stage.SwitchLocation(user, targetList[0]);
            handler(null);
            base.Execute(stage, user, targetList, handler);
        }
    }
}
