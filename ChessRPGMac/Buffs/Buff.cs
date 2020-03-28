using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ChessRPGMac
{
    public abstract class Buff
    {
        public int duration { get; protected set; }

        int duration_timespan;

        int timespan;
        int interval;

        protected FighterObject target;
        protected EffectLayer bottomEffectLayer;

        public bool isNegative { get; protected set; }

        public bool Finished { get; protected set; }

        // Implementation check variables.
        public bool start_implemented { get; protected set; }
        public bool attack_implemented { get; protected set; }
        public bool hit_implemented { get; protected set; }
        public bool ready_implemented { get; protected set; }
        public bool tick_implemented { get; protected set; }

        // Trigger check variables.
        bool start_triggered;
        bool attack_triggered;
        bool hit_triggered;
        bool ready_triggered;
        bool finish_triggered;

        // Extra datas.
        List<FighterObject> targetList;
        int damage;

        public Buff()
        {
            bottomEffectLayer = (EffectLayer)Global.world.GetLayer("BottomEffectLayer");
            Finished = false;
            start_triggered = true;
        }

        public void SetTarget(FighterObject target)
        {
            this.target = target;
        }

        public void SetDuration(int duration)
        {
            this.duration = duration;
        }

        public void SetInterval(int interval)
        {
            this.interval = interval;
        }

        public virtual void Update(GameTime gameTime) { }

        public string TriggerCheck()
        {
            if (start_triggered && start_implemented)
            {
                start_triggered = false;
                return "Start";
            }

            if (ready_triggered && ready_implemented)
            {
                ready_triggered = false;
                return "Ready";
            }

            if (hit_triggered && hit_implemented)
            {
                hit_triggered = false;
                return "Hit";
            }

            if (attack_triggered && attack_implemented)
            {
                attack_triggered = false;
                return "Attack";
            }

            if (interval != 0 && tick_implemented)
            {
                timespan++;
                if (timespan >= interval)
                {
                    timespan = 0;
                    return "Tick";
                }
            }

            duration_timespan++;
            if (duration_timespan >= duration && finish_triggered)
            {
                return "Finish";
            }
            return "";
        }

        public void TriggerAttack(List<FighterObject>targetList)
        {
            this.targetList = targetList;
            attack_triggered = true;
        }

        public void TriggerHit(int damage)
        {
            this.damage = damage;
            hit_triggered = true;
        }

        public void TriggerReady()
        {
            ready_triggered = true;
        }

        public void TriggerFinish()
        {
            finish_triggered = true;
        }

        public virtual void Start(ActionFinishHandler handler) { }
        public virtual void Finish(ActionFinishHandler handler) { Finished = true; }
        public virtual void Attack(ActionFinishHandler handler) { }
        public virtual void Hit(ActionFinishHandler handler) { }
        public virtual void Ready(ActionFinishHandler handler) { }
        public virtual void Tick(ActionFinishHandler handler) { }
    }

    public struct BuffAction
    {
        public Buff buff;
        public string method;

        public BuffAction(Buff buff, string method)
        {
            this.buff = buff;
            this.method = method;
        }
    }
}
