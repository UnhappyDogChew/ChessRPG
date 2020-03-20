using System;
using System.Collections.Generic;

namespace ChessRPGMac
{
    public class Presentation
    {
        public delegate void ExtraMethod();
        ExtraMethod extra;

        EffectLayer bottomEffectLayer;
        FighterObject user;
        List<FighterObject> targetList;
        BattleStage stage;

        List<Effect> bottomEffects;
        SpriteAnimation userAnimation;
        SpriteAnimation targetAnimation;

        FighterObject summon;

        Presentation nextPresentation;

        ActionFinishHandler handler;

        public Presentation(BattleStage stage, FighterObject user, List<FighterObject> targetList, ActionFinishHandler handler = null)
        {
            bottomEffectLayer = (EffectLayer)Global.world.GetLayer("BottomEffectLayer");
            bottomEffects = new List<Effect>();
            this.stage = stage;
            this.user = user;
            this.targetList = targetList;
            this.handler = handler;
        }

        public void ConnectPresentation(Presentation next)
        {
            nextPresentation = next;
        }

        public void AddEffect(Effect effect)
        {
            bottomEffects.Add(effect);
        }

        public void SetSummon(FighterObject summon)
        {
            this.summon = summon;
        }

        public void SetUserAnimation(SpriteAnimation animation)
        {
            userAnimation = animation;
        }

        public void SetTargetAnimation(SpriteAnimation animation)
        {
            targetAnimation = animation;
        }

        public void SetExtraMethod(ExtraMethod extra)
        {
            this.extra = extra;
        }

        public void Start()
        {
            TaskManager taskManager = new TaskManager(userAnimation, targetAnimation);

            user.SetSpriteAnimation(userAnimation);

            foreach (FighterObject target in targetList)
            {
                target.SetSpriteAnimation(targetAnimation);
            }

            foreach (Effect effect in bottomEffects)
            {
                bottomEffectLayer.elements.Add(effect);
                taskManager.AddTask(effect);
            }

            taskManager.TaskFinished += (t) => { handler?.Invoke(summon); nextPresentation?.Start(); };
            taskManager.StartTask();
            extra?.Invoke();

            if (taskManager.IsEmpty())
            {
                handler?.Invoke(summon); 
                nextPresentation?.Start();
            }
        }
    }

    public class PresentationGroup
    {
        Presentation[] list;

        public Presentation this[int index]
        {
            get { return list[index]; }
            set { list[index] = value; }
        }

        public PresentationGroup(int length, BattleStage stage, FighterObject user, List<FighterObject> targetList, 
            ActionFinishHandler handler)
        {
            list = new Presentation[length];
            for (int i = 0; i < length - 1; i++)
            {
                list[i] = new Presentation(stage, user, targetList);
            }
            list[length - 1] = new Presentation(stage, user, targetList, handler);

            for (int i = 0; i < length - 1; i++)
            {
                list[i].ConnectPresentation(list[i + 1]);
            }
        }

        public void Start()
        {
            list[0].Start();
        }
    }
}
