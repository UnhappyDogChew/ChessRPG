using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class Hero1 : Hero
    {
        public Hero1(FighterState state) : base(state)
        {
            name = "Healer";
            level = 4;
            soul = 2;
            sprite = Global.spriteBox.Pick("Hero1");
            heroClass = Class.Priest;
            heroElement = Element.Light;
            maxHp = 300;
            mana = 10;
            speed = 50;
            strength = 5;
            intelligence = 35;
            defense = 5;
            attacks = new Skill[] { new RangeAttack() };
            skills = new Skill[] { new Heal(), new SuperHeal(), null, null, null };
            rarity = 2;
        }

        public override string GetDescription()
        {
            return "This is Healer!";
        }
    }

    public class Heal : Skill
    {
        public Heal()
        {
            name = "Heal";
            icon = Global.content.Load<Texture2D>("Heal");
            manaUsage = 30;
            targetType = TargetType.OneAlley;
            isActive = true;

        }

        public override void Execute(BattleStage stage, FighterObject user, List<FighterObject> targetList, ActionFinishHandler handler)
        {
            targetList[0].RestoreHP(((Hero)user.fighter).intelligence * 2);
            handler(null);
            base.Execute(stage, user, targetList, handler);
        }

        public override string GetDescription()
        {
            return "Restore an Alley's HP.";
        }
    }

    public class SuperHeal : Skill
    {
        public SuperHeal()
        {
            name = "Super Heal";
            icon = Global.content.Load<Texture2D>("Heal");
            manaUsage = 70;
            isActive = true;
            targetType = TargetType.AllAlley;
        }

        public override void Execute(BattleStage stage, FighterObject user, List<FighterObject> targetList, ActionFinishHandler handler)
        {
            foreach (FighterObject target in targetList)
            {
                target.RestoreHP(((Hero)user.fighter).intelligence * 2);
            }
            handler(null);
            base.Execute(stage, user, targetList, handler);
        }

        public override string GetDescription()
        {
            return "This is super strong heal. You might change your underwear after seeing this skill.";
        }
    }
}