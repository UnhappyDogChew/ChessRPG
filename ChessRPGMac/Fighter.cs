using System;
namespace ChessRPGMac
{
    public enum FighterType
    {
        Hero, 
        Enemy,
    }

    public abstract class Fighter
    {
        public string name { get; protected set; }

        public int strength { get; protected set; }
        public int defense { get; protected set; }
        public int intelligence { get; protected set; }
        public float speed { get; protected set; }
        public int maxHp { get; protected set; }

        public int HP { get; protected set; }
        public float AP { get; protected set; }

        public Sprite sprite { get; protected set; }

        public bool alive { get; protected set; }
        public FighterState state { get; protected set; }

        // 상태이상
        public bool paused { get; set; }
        public bool immovable { get; set; }
        public bool incurable { get; set; }

        public Fighter()
        {
            alive = true;
            paused = false;
            immovable = false;
            incurable = false;
        }
        /// <summary>
        /// Deals damage to this <see cref=" Fighter"/>.
        /// </summary>
        /// <returns><c>true</c>, if <see cref=" Fighter"/> is dead, <c>false</c> otherwise.</returns>
        /// <param name="pureDamage">Pure damage.</param>
        /// <param name="pierce">Pierce.</param>
        public bool DealDamage(int pureDamage, int pierce = 0)
        {
            HP -= (int)(pureDamage * (100.0f / (100 + defense - pierce)));
            if (HP <= 0)
            {
                alive = false;
                return true;
            }

            return false;
        }

        public bool RestoreHP(int heal)
        {
            if (incurable)
                return false;

            HP += heal;
            if (HP > maxHp)
                HP = maxHp;

            return true;
        }
        /// <summary>
        /// Increases AP.
        /// </summary>
        /// <returns><c>true</c>, if AP exceed 100, <c>false</c> otherwise.</returns>
        /// <param name="framePerSecond">Frame per second.</param>
        public virtual bool IncreaseGage(int framePerSecond)
        {
            if (paused)
                return false;

            AP += (float)speed / framePerSecond;
            if (AP >= 100)
            {
                AP = 100;
                return true;
            }
            return false;
        }

        public void DecreaseAP(int ap)
        {
            AP -= ap;
            if (AP < 0)
                AP = 0;
        }

        public void ResetAP() { AP = 0; }

        public virtual void Reset()
        {
            HP = maxHp;
            AP = 0;
        }

        public virtual bool ChangeState(FighterState state)
        {
            if (state != FighterState.Stored && immovable)
                return false;

            this.state = state;
            return true;
        }

        public Fighter Copy()
        {
            return (Fighter)MemberwiseClone();
        }
    }

    public class NullFighter : Fighter
    {
        public NullFighter(FighterState state)
        {
            this.state = state;
        }

    }
}
