using System;
using System.Collections.Generic;

namespace ChessRPGMac
{
    public class Goul : Enemy
    {
        public Goul()
        {
            name = "Goul";
            strength = 120;
            defense = 5;
            intelligence = 1;
            maxHp = 500;
            speed = 49;
            sprite = Global.spriteBox.Pick("Goul");
        }

        public override int PhaseCheck(BattleStage stage)
        {
            // No phase transition.
            return -1;
        }

        public override void CreatePattern(Queue<EnemySkill> pattern)
        {
            pattern.Enqueue(new Scratch());
        }


        public class Scratch : EnemySkill
        {
            public Scratch()
            {
                name = "Scratch";
            }

            public override bool IsAvailable(BattleStage stage, FighterObject user)
            {
                return stage.fighterLists[(int)StageRow.HeroFront].Count > 0;
            }

            public override void Execute(BattleStage stage, FighterObject user, List<FighterObject> targetList, ActionFinishHandler handler)
            {
                PresentationGroup p = new PresentationGroup(2, stage, user, targetList, handler);
                p[0].AddEffect(new SlashEffect(targetList[0].x, targetList[0].y, 0));
                p[0].SetUserAnimation(SpriteAnimation.GetSpriteAnimation("MeleeAttackDown"));

                p[1].AddEffect(targetList[0].DealDamage(user.fighter.strength));
                p[1].SetTargetAnimation(SpriteAnimation.GetSpriteAnimation("Shake"));

                p.Start();

                base.Execute(stage, user, targetList, handler);
            }

            public override List<FighterObject> SelectTarget(BattleStage stage)
            {
                int count = stage.fighterLists[(int)StageRow.HeroFront].Count;
                Random random = new Random();
                List<FighterObject> result = new List<FighterObject>();
                result.Add(stage.fighterLists[2][random.Next(count)]);
                return result;
            }
        }
    }
}
