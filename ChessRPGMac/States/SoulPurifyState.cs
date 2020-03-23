using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

namespace ChessRPGMac.States
{
    public class SoulPurifyState : State
    {
        List<EnemyObject> deadEnemyObjects;

        public SoulPurifyState(List<EnemyObject> deadEnemyObjects)
        {
            this.deadEnemyObjects = deadEnemyObjects;
        }

        public SoulPurifyState(List<EnemyObject> deadEnemyObjects, List<Keys> pressedKeys) : base(pressedKeys)
        {
            this.deadEnemyObjects = deadEnemyObjects;
        }
    }
}
