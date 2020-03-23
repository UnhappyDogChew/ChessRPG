using System;
using System.Collections.Generic;

namespace ChessRPGMac
{
    public abstract class Enemy : Fighter
    {
        public int soulCount { get; protected set; }

        public abstract int PhaseCheck(BattleStage stage);
        public abstract void CreatePattern(Queue<EnemySkill> pattern);
    }
}
