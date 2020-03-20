using System;
using System.Collections.Generic;

namespace ChessRPGMac
{
    public delegate void ActionFinishHandler(FighterObject summon);

    public abstract class Action
    {
        public string name { get; protected set; }
        public TargetType targetType { get; protected set; }
        public ActionFinishHandler actionFinishHandler;
        protected EffectLayer bottomEffectLayer { get { return (EffectLayer)Global.world.GetLayer("BottomEffectLayer"); } }

        public abstract void Execute(BattleStage stage, FighterObject user, List<FighterObject> targetList, ActionFinishHandler handler);

        public abstract bool IsAvailable(BattleStage stage, FighterObject user);
    }
}
