using System;
namespace ChessRPGMac
{
    public class Hero2 : Hero
    {
        public Hero2()
        {
            name = "Warrior";
            level = 3;
            soul = 1;
            sprite = Global.spriteBox.Pick("Hero2");
            HeroClass = Class.Warrior;
            HeroElement = Element.Earth;
            maxHp = 700;
            mana = 10;
            speed = 20;
            strength = 40;
            intelligence = 15;
            defense = 25;

            attacks = new Skill[] { new MeleeAttack(this) };
            skills = new Skill[] { null, null, null, null, null };

            Reset();
        }
    }
}
