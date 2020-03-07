using System;

using Microsoft.Xna.Framework;

namespace ChessRPGMac
{
    public interface ISelectable
    {
        Point GetLocation();
        Point GetSize();
        void Focus();
        void Leave();
        void SelectAction();
    }
}
