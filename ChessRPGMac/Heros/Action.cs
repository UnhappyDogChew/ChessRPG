using System;

namespace ChessRPGMac
{
    public abstract class Action
    {
        public string name { get; protected set; }
        public TargetType targetType { get; protected set; }
        public delegate void ActionFinishHandler();
        public ActionFinishHandler actionFinishHandler;
        protected EffectLayer bottomEffectLayer { get { return (EffectLayer)Global.world.GetLayer("BottomEffectLayer"); } }

        public void Execute(BattleStage stage, FighterObject user, FighterObject target, ActionFinishHandler handler)
        {
            actionFinishHandler = handler;
            PreAction(stage, user, target);
        }

        public virtual void PreAction(BattleStage stage, FighterObject user, FighterObject target) 
            { DoAction(stage, user, target); }

        public virtual void DoAction(BattleStage stage, FighterObject user, FighterObject target) 
            { PostAction(stage, user, target); }

        public virtual void PostAction(BattleStage stage, FighterObject user, FighterObject target) 
            { actionFinishHandler(); }

        public abstract string GetDescription();
    }

    public class MoveAction : Action
    {
        public MoveAction()
        {
            targetType = TargetType.OneAlleyOtherLine;
        }

        public override string GetDescription()
        {
            return "Move to other line.";
        }

        public override void DoAction(BattleStage stage, FighterObject user, FighterObject target)
        {
            stage.SwitchLocation(user, target);

            base.DoAction(stage, user, target);
        }
    }
}
