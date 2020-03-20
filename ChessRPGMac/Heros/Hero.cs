using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public abstract class Hero : Fighter
    {
        public Equipment equipment { get; protected set; }

        // Status
        public int level { get; protected set; }
        public int soul { get; protected set; }
        public float mana { get; protected set; }

        public float SP { get; protected set; }

        public Class HeroClass { get; protected set; }
        public Element HeroElement { get; protected set; }

        public static int[] SoulPerLevel = { 1, 1, 2, 2, 4 };

        public int[] maxHpPerLevel { get; protected set; }
        public int[] manaPerLevel { get; protected set; }
        public int[] speedPerLevel { get; protected set; }
        public int[] strengthPerLevel { get; protected set; }
        public int[] intelligencePerLevel { get; protected set; }
        public int[] defensePerLevel { get; protected set; }

        public Skill[] attacks { get; protected set; }
        public Skill[] skills { get; protected set; }

        public Hero()
        {
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
}
