using System;


namespace ChessRPGMac
{
    public abstract class EnemySkill : Action
    {
        Enemy user;

        public EnemySkill(Enemy user)
        {
            this.user = user;
        }
    }
}
