using System;
using System.Collections.Generic;

namespace ChessRPGMac
{
    public abstract class Enemy : Fighter
    {
        public int phase { get; protected set; }

        Queue<EnemySkill> pattern;

        public Enemy(FighterState state)
        {
            if (state == FighterState.Stored)
                state = FighterState.Front;
            this.state = state;
            pattern = new Queue<EnemySkill>();
        }

        public override bool ChangeState(FighterState state)
        {
            if (state == FighterState.Stored)
                return false;
            this.state = state;
            return true;
        }

        public abstract void PhaseCheck();
        protected abstract void CreatePattern();
    }
}
