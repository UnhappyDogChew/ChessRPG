using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChessRPGMac
{
    public delegate void StateChangeMethod(State state);

    /// <summary>
    /// State abstract class. The rogic of controlling world should be implemented in child class.
    /// </summary>
    public abstract class State
    {
        protected delegate void KeyHandler();
        protected List<Keys> pressedKeys;
        protected List<KeyAction> keyActions;

        public StateChangeMethod stateChangeMethod;

        public State()
        {
            pressedKeys = new List<Keys>();
            keyActions = new List<KeyAction>();
            Prepare();
        }

        public State(List<Keys> pressedKeys)
        {
            this.pressedKeys = pressedKeys;
            keyActions = new List<KeyAction>();
            Prepare();
        }

        protected virtual void Prepare()
        {
        }

        protected virtual void Finish() { }

        public virtual void Update(GameTime gameTime)
        {
            Global.world.Update(gameTime);
        }

        protected void ChangeState(State state)
        {
            Finish();
            stateChangeMethod?.Invoke(state);
        }

        protected bool CheckKeyPress(Keys key)
        {
            if (Keyboard.GetState().IsKeyDown(key) && !pressedKeys.Contains(key))
            {
                pressedKeys.Add(key);
                return true;
            }
            return false;
        }

        protected bool CheckKeyRelease(Keys key)
        {
            if (Keyboard.GetState().IsKeyUp(key) && pressedKeys.Contains(key))
            {
                pressedKeys.Remove(key);
                return true;
            }
            return false;
        }
        /// <summary>
        /// For each key in detectableKeys, check if key is pressed or released, 
        /// and do actions related to that key. This method should be called in Update method.
        /// </summary>
        protected void KeyCheck()
        {
            foreach (KeyAction action in keyActions)
            {
                if (Keyboard.GetState().IsKeyDown(action.key) && !pressedKeys.Contains(action.key))
                {
                    pressedKeys.Add(action.key);
                    action.pressedAction?.Invoke();
                }
                else if (Keyboard.GetState().IsKeyUp(action.key) && pressedKeys.Contains(action.key))
                {
                    pressedKeys.Remove(action.key);
                    action.releasedAction?.Invoke();
                }

                if (Keyboard.GetState().IsKeyDown(action.key))
                    action.keyDownAction?.Invoke();
                else if (Keyboard.GetState().IsKeyUp(action.key))
                    action.keyUpAction?.Invoke();
            }
        }
    }
}
