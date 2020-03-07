using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChessRPGMac
{
    public delegate void InteractionMethod(Interaction interaction);

    /// <summary>
    /// Interactable objects need to inherit this abstract class.
    /// This abstract class provides basic methods and fields for interactable objects.
    /// </summary>
    public abstract class InteractableObject : GameObject
    {
        public Sprite sprite { get; set; }

        protected Collider interactionCollider;
        protected Player player;

        public InteractionMethod interactionMethod;

        public InteractableObject(int x, int y) : base(x, y)
        {
            player = Global.world.GetPlayer();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, x, y, Color.White, depth: depth);
        }

        public override abstract void Update(GameTime gameTime);

        /// <summary>
        /// This method determines what interaction instance should be sent to <see cref="ExploreState"/>
        /// when event is invoked. All Children class need to have there own interaction class 
        /// if there interaction rogic is unique.
        /// </summary>
        public abstract void Interact();

        /// <summary>
        /// Checks if player is inside of <see cref="interactionCollider"/> and looking at this obejct.
        /// </summary>
        /// <returns><c>true</c>, if player is near, <c>false</c> otherwise.</returns>
        public bool IsPlayerNear()
        {
            if (interactionCollider.Detect(player.collider))
            {
                if ((player.y > y && player.GetDirection() == (int)Direction.Up) ||
                    (player.y < y && player.GetDirection() == (int)Direction.Down) ||
                    (player.x > x && player.GetDirection() == (int)Direction.Left) ||
                    (player.x < x && player.GetDirection() == (int)Direction.Right))
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



    /// <summary>
    /// This class is executable component that <see cref="InteractionState"/> can use.
    /// All Interactions need to inherit this class.
    /// </summary>
    public abstract class Interaction
    {
        public bool IsEnd { get; protected set; }
        public Textbox InteractionTextbox { get; protected set; }

        protected int actionIndex;

        delegate void InteractionDelgate(int selection);
        List<InteractionDelgate> selectActions;

        public Interaction()
        {
            IsEnd = false;
            selectActions = new List<InteractionDelgate>();
            actionIndex = 0;
        }

        public void ChooseAnswer(int selection)
        {
            selectActions[actionIndex](selection);
        }

        public virtual void Finish() { IsEnd = true; }
    }
}
