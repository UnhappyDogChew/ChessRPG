using System;

using Microsoft.Xna.Framework;

namespace ChessRPGMac
{
    public interface IClickable
    {
        event EventHandler Clicked;
        event EventHandler MouseEntered;
        event EventHandler MouseLeaved;
        event EventHandler Released;

        void Click();
        void Release();
        void MouseEnter();
        void MouseLeave();
        Rectangle GetBoundary();
    }

    public enum MouseClickState
    {
        Clicked, 
        Released,
    }

    public enum MouseLocationState
    {
        Entered,
        Leaved,
    }
}
