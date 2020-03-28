using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class Hero2 : Hero
    {
        public Hero2(FighterState state) : base(state)
        {
            name = "Warrior";
            level = 3;
            soul = 1;
            sprite = Global.spriteBox.Pick("Hero2");
            heroClass = Class.Warrior;
            heroElement = Element.Earth;
            maxHp = 700;
            mana = 50;
            speed = 50;
            strength = 40;
            intelligence = 15;
            defense = 25;
            rarity = 1;

            attacks = new Skill[] { new MeleeAttack() };
            skills = new Skill[] { new BloodySlash(), new FrozenSlash(), null, null, null };
        }

        public override string GetDescription()
        {
            return "This is super strong hero.";
        }
    }

    public class BloodySlash : Skill
    {
        public BloodySlash()
        {
            name = "Bloody Slash";
            hpUsage = 50;
            targetType = TargetType.OneEnemyFront;
            isActive = true;
            icon = Global.content.Load<Texture2D>("MeleeAttack");
        }

        public override bool IsAvailable(BattleStage stage, FighterObject user)
        {
            return (user.HP > hpUsage);
        }

        public override void Execute(BattleStage stage, FighterObject user, List<FighterObject> targetList, ActionFinishHandler handler)
        {
            PresentationGroup p = new PresentationGroup(3, stage, user, targetList, handler);
            p[0].AddEffect(user.DealDamage(hpUsage));

            p[1].AddEffect(new SlashEffect(targetList[0].x, targetList[0].y, 0));
            p[1].SetUserAnimation(SpriteAnimation.GetSpriteAnimation("MeleeAttackUp"));

            p[2].SetTargetAnimation(SpriteAnimation.GetSpriteAnimation("Shake"));
            p[2].AddEffect(targetList[0].DealDamage(((Hero)user.fighter).strength * 2));

            p.Start();

            base.Execute(stage, user, targetList, handler);
        }

        public override string GetDescription()
        {
            return "Reduce user's HP, and deals extreme damage to single front enemy.";
        }
    }

    public class FrozenSlash : Skill
    {
        public FrozenSlash()
        {
            name = "Frozen Slash";
            manaUsage = 50;
            targetType = TargetType.OneEnemyFront;
            isActive = true;
            icon = Global.content.Load<Texture2D>("MeleeAttack");
        }

        public override void Execute(BattleStage stage, FighterObject user, List<FighterObject> targetList, ActionFinishHandler handler)
        {
            PresentationGroup p = new PresentationGroup(3, stage, user, targetList, handler);
            p[0].AddEffect(new SlashEffect(targetList[0].x, targetList[0].y, 0));
            p[0].SetUserAnimation(SpriteAnimation.GetSpriteAnimation("MeleeAttackUp"));

            p[1].SetTargetAnimation(SpriteAnimation.GetSpriteAnimation("Shake"));
            p[1].AddEffect(targetList[0].DealDamage(((Hero)user.fighter).strength));

            p[2].SetExtraMethod(() => targetList[0].AddBuff(new Frozen(), Global.Properties.FRAME_PER_SECOND * 3));

            p.Start();

            base.Execute(stage, user, targetList, handler);
        }

        public override string GetDescription()
        {
            return "Attack a front enemy and freeze it.";
        }
    }
}
