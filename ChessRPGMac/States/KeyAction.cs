using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChessRPGMac
{
    public struct KeyAction
    {
        public Keys key { get; private set; }
        public delegate void KeyActionHandler();
        public KeyActionHandler pressedAction { get; private set; }
        public KeyActionHandler releasedAction { get; private set; }
        public KeyActionHandler keyDownAction { get; private set; }
        public KeyActionHandler keyUpAction { get; private set; }

        public KeyAction(Keys key, KeyActionHandler pressed = null, KeyActionHandler released = null,
            KeyActionHandler keyDown = null, KeyActionHandler keyUp = null)
        {
            this.key = key;
            pressedAction = pressed;
            releasedAction = released;
            keyDownAction = keyDown;
            keyUpAction = keyUp;
        }

    }
}
