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

        public Sprite sprite { get; protected set; }

    }

    public class NullFighter : Fighter
    {
    }
}
