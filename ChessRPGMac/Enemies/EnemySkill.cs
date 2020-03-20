using System;
using System.Collections.Generic;

namespace ChessRPGMac
{
    public abstract class EnemySkill : Action
    {
        public abstract List<FighterObject> SelectTarget(BattleStage stage);

        public override void Execute(BattleStage stage, FighterObject user, List<FighterObject> targetList, ActionFinishHandler handler)
        {
        }
    }
}
