using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChessRPGMac
{
    public class ExploreState : State
    {
        Player player;
        GUILayer guiLayer;

        Dictionary<Keys, Direction> moveKeys = new Dictionary<Keys, Direction>()
        {
            { Keys.S, Direction.Down },
            { Keys.W, Direction.Up },
            { Keys.D, Direction.Right },
            { Keys.A, Direction.Left }
        };
        Stack<Keys> moveKeyStack = new Stack<Keys>();
        List<InteractableObject> interactables;

        public ExploreState() : base()
        {
            GetPlayer();
            player.Stand();
        }

        public ExploreState(List<Keys> pressedKeys) : base(pressedKeys)
        {
            GetPlayer();
            player.Stand();
        }

        public override void Update(GameTime gameTime)
        {
            // !--- Do not add codes above. ---!
            KeyCheck();

            bool moveKeyChanged = false;
            if (moveKeyStack.Count != 0)
            {
                while (!pressedKeys.Contains(moveKeyStack.Peek()))
                {
                    moveKeyStack.Pop();
                    moveKeyChanged = true;
                    if (moveKeyStack.Count == 0)
                        break;
                }
            }

            if (moveKeyChanged)
            {
                if (moveKeyStack.Count == 0)
                    player.Stand();
                else
                    player.Walk(moveKeys[moveKeyStack.Peek()]);
            }

            // Enemy contect check
            foreach (GameObjectLayer layer in Global.world.GameObjectLayers)
            {
                foreach (GameObject element in layer.elements)
                {
                    if (element is EnemySoul)
                    {
                        if (((EnemySoul)element).IsPlayerNear())
                        {
                            ((EnemySoul)element).combatMethod = (combat) => { ChangeState(new BattleState(pressedKeys, combat)); };
                            ((EnemySoul)element).CombatStart();
                        }
                    }
                }
            }


            base.Update(gameTime);
        }

        private void InteractionButtonPressed()
        {
            interactables = Global.world.GetInteractableObjects();
            foreach (InteractableObject element in interactables)
            {
                if (element.IsPlayerNear())
                {
                    element.interactionMethod = (interaction) => { ChangeState(new InteractionState(pressedKeys, interaction)); };
                    element.Interact();
                }
            }
        }

        private void GetPlayer()
        {
            player = Global.world.GetPlayer();
            if (player == null)
                throw new NullReferenceException("Player reference are not found.");
        }
        /// <summary>
        /// Prepare this instance.
        /// Actions for key pressed and released event need to be declared here.
        /// </summary>
        protected override void Prepare()
        {
            // Set Key Actions.
            keyActions.Add(new KeyAction(Keys.Enter, pressed: () => { ChangeState(new TeamManageState(pressedKeys)); }));

            keyActions.Add(new KeyAction(Keys.E, pressed: InteractionButtonPressed));

            // Set movement actions.
            foreach (Keys moveKey in moveKeys.Keys)
            {
                keyActions.Add(new KeyAction(moveKey, pressed: () => { player.Walk(moveKeys[moveKey]); moveKeyStack.Push(moveKey); }));
            }

            // Set GUIs
            guiLayer = (GUILayer)Global.world.GetLayer("GUILayer");
            if (guiLayer.GetGUI<TeamManagement>() == null)
            {
                guiLayer.AddGUI(new TeamManagement("TeamManagement", null, 0, 288));
            }

            base.Prepare();
        }
    }
}
