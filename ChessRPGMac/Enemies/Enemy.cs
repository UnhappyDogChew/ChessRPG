using System;
using System.Collections.Generic;

namespace ChessRPGMac
{
    public abstract class Enemy : Fighter
    {
        public int phase { get; protected set; }

        protected Queue<EnemySkill> pattern;

        public Enemy(FighterState state)
        {
            if (state == FighterState.Stored)
                state = FighterState.Front;
            this.state = state;
            pattern = new Queue<EnemySkill>();
            phase = 0;
        }

        public override bool ChangeState(FighterState state)
        {
            if (state == FighterState.Stored)
                return false;
            this.state = state;
            return true;
        }

        public EnemySkill GetAction()
        {
            if (pattern.Count == 0)
                CreatePattern();
            return pattern.Dequeue();
        }

        public abstract void PhaseCheck(BattleStage stage);
        protected abstract void CreatePattern();
    }
}
