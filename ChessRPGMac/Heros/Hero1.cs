using System;

using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class Hero1 : Hero
    {
        public Hero1()
        {
            name = "Healer";
            level = 4;
            soul = 2;
            sprite = Global.spriteBox.Pick("Hero1");
            HeroClass = Class.Priest;
            HeroElement = Element.Light;
            maxHp = 300;
            mana = 50;
            speed = 50;
            strength = 5;
            intelligence = 35;
            defense = 5;
            attacks = new Skill[] { new MeleeAttack(this) };
            skills = new Skill[] { new Heal(this), new SuperHeal(this), null, null, null };

            Reset();
        }
    }

    public class Heal : Skill
    {
        public Heal(Hero user) : base(user)
        {
            name = "Heal";
            icon = Global.content.Load<Texture2D>("Heal");
            manaUsage = 30;
            targetType = TargetType.OneAlley;
            isActive = true;

        }
        public override void DoAction(BattleStage stage, FighterObject user, FighterObject target)
        {
            target.fighter.RestoreHP(this.user.intelligence * 2);
        }

        public override string GetDescription()
        {
            return "Restore an Alley's HP.";
        }
    }

    public class SuperHeal : Skill
    {
        public SuperHeal(Hero user) : base(user)
        {
            name = "Super Heal";
            icon = Global.content.Load<Texture2D>("Heal");
            manaUsage = 100;
            targetType = TargetType.Row;
            isActive = true;

        }
        public override void DoAction(BattleStage stage, FighterObject user, FighterObject target)
        {
            target.fighter.RestoreHP(this.user.intelligence * 2);
        }

        public override string GetDescription()
        {
            return "This is super strong heal. You might change your underwear after seeing this skill.";
        }
    }
}
