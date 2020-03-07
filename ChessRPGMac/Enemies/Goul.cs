using System;
namespace ChessRPGMac
{
    public class Goul : Enemy
    {
        public Goul(FighterState state) : base(state)
        {
            name = "Goul";
            strength = 10;
            defense = 5;
            intelligence = 1;
            maxHp = 100;
            speed = 50;
            sprite = Global.spriteBox.Pick("Goul");

            Reset();
        }

        public override void PhaseCheck()
        {
            throw new NotImplementedException();
        }

        protected override void CreatePattern()
        {
            throw new NotImplementedException();
        }
    }
}
