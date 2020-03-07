using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ChessRPGMac
{
    public abstract class Item
    {
        public Texture2D icon { get; protected set; }
        public string name { get; protected set; }
    }

    public abstract class Equipment : Item
    {

    }

    public class Ring1 : Equipment
    {
        public Ring1()
        {
            icon = Global.content.Load<Texture2D>("Ring1");
        }
    }
}
