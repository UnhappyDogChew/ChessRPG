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

        public Class heroClass { get; protected set; }
        public Element heroElement { get; protected set; }
        /// <summary>
        /// Rarity is between 0 ~ 4.
        /// </summary>
        /// <value>The rarity.</value>
        public int rarity { get; protected set; }

        public static int[] SoulPerLevel = { 1, 1, 2, 2, 4 };

        public int[] maxHpPerLevel { get; protected set; }
        public int[] manaPerLevel { get; protected set; }
        public int[] speedPerLevel { get; protected set; }
        public int[] strengthPerLevel { get; protected set; }
        public int[] intelligencePerLevel { get; protected set; }
        public int[] defensePerLevel { get; protected set; }

        public Skill[] attacks { get; protected set; }
        public Skill[] skills { get; protected set; }

        public FighterState defaultFighterState { get; private set; }

        public Hero(FighterState defaultFighterState)
        {
            this.defaultFighterState = defaultFighterState;
        }

        public void ChangeDefaultState(FighterState state)
        {
            this.defaultFighterState = state;
        }

        public abstract string GetDescription();
    }
}
