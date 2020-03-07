using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChessRPGMac
{
    public delegate void CombatMethod(Combat combat);

    public class EnemySoul : GameObject
    {
        public Sprite sprite { get; set; }

        public CombatMethod combatMethod; //For ExploreState to make transition to BattleState.

        protected Collider interactionCollider;
        protected Player player;
        protected Combat combat;

        const int COLLIDER_WIDTH = 40;
        const int COLLIDER_HEIGHT = 24;

        public EnemySoul(int x, int y, Combat combat) : base(x, y)
        {
            player = Global.world.GetPlayer();
            this.combat = combat;
            collider = new SquareCollider(x, y, COLLIDER_WIDTH, COLLIDER_HEIGHT, COLLIDER_WIDTH / 2, COLLIDER_HEIGHT / 2);
            interactionCollider = new SquareCollider(x, y, COLLIDER_WIDTH + 10, COLLIDER_HEIGHT + 6, 
                COLLIDER_WIDTH / 2 + 5, COLLIDER_HEIGHT / 2 + 3, false);
            sprite = Global.spriteBox.Pick("PlunckWithFlower");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, x, y, Color.White, depth: depth);
        }

        public override void Update(GameTime gameTime) { }

        public void CombatStart()
        {
            combatMethod(combat);
            Finished = true;
        }

        /// <summary>
        /// Checks if player is inside of <see cref="interactionCollider"/> and looking at this obejct.
        /// </summary>
        /// <returns><c>true</c>, if player is near, <c>false</c> otherwise.</returns>
        public bool IsPlayerNear()
        {
            if (interactionCollider.Detect(player.collider))
            {
                if ((player.y > y) || (player.y < y) ||
                    (player.x > x) || (player.x < x))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks if mouse position is inside of objects sprite boundary.
        /// </summary>
        /// <returns><c>true</c>, if mouse is near, <c>false</c> otherwise.</returns>
        public bool IsMouseNear()
        {
            if ((x - sprite.origin.X) < Mouse.GetState().X &&
                Mouse.GetState().X < (x + (sprite.width - sprite.origin.X)) &&
                (y - sprite.origin.Y) < Mouse.GetState().Y &&
                Mouse.GetState().Y < (y + (sprite.height - sprite.origin.Y)))
                return true;
            return false;
        }
    }

    public class Combat
    {
        public List<Enemy> enemyList { get; private set; }
        public int frontCount { get; private set; }
        public int behindCount { get; private set; }

        public Combat(List<Enemy> enemyList)
        {
            this.enemyList = enemyList;
            frontCount = 0;
            behindCount = 0;
            foreach (Enemy enemy in enemyList)
            {
                if (enemy.state == FighterState.Front)
                    frontCount++;
                else if (enemy.state == FighterState.Behind)
                    behindCount++;
            }
        }

        public int GetEnemyFrontCount()
        {
            int result = 0;
            foreach (Enemy enemy in enemyList)
            {
                if (enemy.state == FighterState.Front && enemy.alive)
                    result++;
            }
            return result;
        }

        public int GetEnemyBehindCount()
        {
            int result = 0;
            foreach (Enemy enemy in enemyList)
            {
                if (enemy.state == FighterState.Behind && enemy.alive)
                    result++;
            }
            return result;
        }
    }
}
