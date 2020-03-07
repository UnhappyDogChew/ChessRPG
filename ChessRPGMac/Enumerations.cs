using System;

namespace ChessRPGMac
{
    /// <summary>
    /// Represents directions. The order is Down, Up, Right, Left
    /// </summary>
    public enum Direction
    {
        Down,
        Up,
        Right,
        Left
    }
    /// <summary>
    /// Hero Classes.
    /// </summary>
    public enum Class
    {
        Warrior,
        Priest,
    }
    /// <summary>
    /// Hero Elements.
    /// </summary>
    public enum Element
    {
        Flame, 
        Frost,
        Earth,
        Void,
        Light,
    }
    /// <summary>
    /// Represents <see cref="Fighter"/>'s state.
    /// </summary>
    public enum FighterState
    {
        Stored,
        Front,
        Behind,
    }

    public enum TargetType
    {
        All, 
        AllOther, 
        AllEnemy, 
        AllEnemyFront,
        AllEnemyBehind,
        AllAlley, 
        AllAlleyFront,
        AllAlleyBehind,
        AllAlleyOther, 
        AllAlleyOtherLine, 
        Row, 
        RowEnemy, 
        RowAlley, 
        One,
        OneOther, 
        OneEnemy, 
        OneEnemyFront,
        OneEnemyBehind,
        OneAlley, 
        OneAlleyFront,
        OneAlleyBehind,
        OneAlleyOther, 
        OneAlleyOtherLine, 
        Self,
    }

    public enum AlignType
    {
        Left, 
        Right, 
        Top, 
        Bottom, 
        Center,
    }

    public enum EffectTargetType
    {
        User, 
        Target, 
        AllEnemy, 
        AllHero, 
        All,
    }
}
