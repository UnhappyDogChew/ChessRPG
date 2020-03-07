using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public class DragSocket : GUIGroup
    {
        public string[] keys { get; private set; }

        public GUIComponent content { get { return (components.Count == 0) ? null : components[0]; } }
        public int width { get; protected set; }
        public int height { get; protected set; }
        public Texture2D emphasis { get; protected set; }

        public DragSocket(string name, GUIComponent parent, int rx, int ry, string[] keys, 
            int width, int height, Texture2D emphasis = null) : base(name, parent, rx, ry)
        {
            this.keys = keys;
            this.width = width;
            this.height = height;
            this.emphasis = emphasis;
        }

        /// <summary>
        /// Tries to set new content to this socket. 
        /// If <paramref name="content"/>'s key matches one of keys in this socket,
        /// returns true and set content. If not, returns false.
        /// If <paramref name="force"/> is true, set this content no matter what key it has.
        /// </summary>
        /// <returns><c>true</c>, if content was set, <c>false</c> otherwise.</returns>
        /// <param name="content">Content.</param>
        /// <param name="force">If set to <c>true</c> force.</param>
        public bool SetContent(IDragable content, bool force = false)
        {
            bool passed = MatchKey(content);

            if (passed || force)
            {
                DragSocket otherSocket = content.GetSocket();
                if (otherSocket != null)
                {
                    otherSocket.ResetContent();
                    // If this socket already have content, exchange content with given content's socket.
                    if (this.content != null)
                    {
                        if (!otherSocket.SetContent((IDragable)this.content))
                        {
                            otherSocket.SetContent(content, true);
                            return false; // If setting socket is failed, return false.
                        }
                        components.Clear();
                    }
                }
                if (components.Count > 0)
                    return false;
                components.Add((GUIComponent)content);
                content.ChangeSocket(this);
                return true;
            }

            return false;
        }

        public bool MatchKey(IDragable content)
        {
            foreach (string key in keys)
            {
                if (content.GetKey() == key)
                    return true;
            }
            return false;
        }

        public void ResetContent()
        {
            components.Clear();
        }

        public override void AddComponent(GUIComponent component)
        {
            throw new NotImplementedException();
        }

        public override void RemoveComponent(GUIComponent component)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if given point value is inside of this socket's boundary.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public bool IsInside(int x, int y)
        {
            if (Toolbox.IsPointInsideSquare(this.x, this.y, this.x + width, this.y + height, x, y))
                return true;
            return false;
        }
    }

    public class HeroSummarySocket : DragSocket, ISelectable
    {
        const int WIDTH = 100;
        const int HEIGHT = 50;

        public HeroSummarySocket(string name, GUIComponent parent, int rx, int ry, string[] keys) 
            : base(name, parent, rx, ry, keys, WIDTH, HEIGHT)
        {
            emphasis = Global.content.Load<Texture2D>("HeroSocketEmphasis");
        }

        public Point GetLocation()
        {
            return new Point(x, y);
        }

        public Point GetSize()
        {
            return new Point(WIDTH, HEIGHT);
        }

        public void SelectAction()
        {
            if (Global.state is TeamManageState)
            {
                if (content != null)
                    ((TeamManageState)Global.state).SelectHero(this);
                else
                    ((TeamManageState)Global.state).DeselectHero();
            }
        }

        public void Focus()
        {
        }

        public void Leave()
        {
        }
    }

    public class ItemSocket : DragSocket, ISelectable
    {
        public int column { get; private set; }
        public int row { get; private set; }

        const int WIDTH = 32;
        const int HEIGHT = 32;

        public ItemSocket(string name, GUIComponent parent, int rx, int ry, int col, int row)
            : base(name, parent, rx, ry, new string[] { "Item" }, WIDTH, HEIGHT)
        {
            this.column = col;
            this.row = row;
            emphasis = Global.content.Load<Texture2D>("ItemSocketEmphasis");
        }

        public Point GetLocation()
        {
            return new Point(x, y);
        }

        public Point GetSize()
        {
            return new Point(WIDTH, HEIGHT);
        }

        public void SelectAction()
        {

        }

        public void Focus()
        {
        }

        public void Leave()
        {
        }
    }

    public class DragSocketGroup : GUIGroup
    {
        public int size { get; private set; }

        public DragSocketGroup(string name, GUIComponent parent, int rx, int ry, int size) : base(name, parent, rx, ry)
        {
            this.size = size;
            for (int i = 0; i < size; i++)
                components.Add(new NullGUIComponent());
        }

        public DragSocket GetSocket(int index)
        {
            if (components[index] is DragSocket)
                return (DragSocket)components[index];
            return null;
        }

        public bool AddSocket(int index, DragSocket socket)
        {
            if (index >= size || index < 0)
                return false;
            if (!(components[index] is NullGUIComponent))
                return false;

            components[index] = socket;
            socket.ChangeParent(this);
            return true;
        }

        public bool RemoveSocket(int index)
        {
            if (index >= size || index < 0)
                return false;

            components[index] = new NullGUIComponent();
            return true;
        }

        public bool SetContent(int index, IDragable content, bool force = false)
        {
            if (index >= size || index < 0)
                return false;
            if (components[index] is NullGUIComponent)
                return false;

            return ((DragSocket)components[index]).SetContent(content, force);
        }
    }
}
