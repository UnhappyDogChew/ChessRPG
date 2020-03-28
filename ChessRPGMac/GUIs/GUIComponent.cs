using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessRPGMac
{
    public abstract class GUIComponent : IEnumerable
    {
        public bool Finished { get; protected set; }
        public bool Activated { get; protected set; }

        public string name { get; protected set; }
        public virtual int x { get { return parent.x + relativeX; } }
        public virtual int y { get { return parent.y + relativeY; } }

        public int relativeX { get; protected set; }
        public int relativeY { get; protected set; }

        public GUIComponent parent { get; protected set; }

        /// <summary>
        /// This Constructor should not be used except <see cref=" NullGUIComponent"/>.
        /// </summary>
        public GUIComponent() { Activated = true; }

        public GUIComponent(string name, GUIComponent parent, int rx, int ry)
        {
            relativeX = rx;
            relativeY = ry;
            Finished = false;
            this.parent = parent;
            this.name = name;
            Activated = true;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void DrawBegin(GameTime gameTime, SpriteBatch spriteBatch) { }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        public void ChangeParent(GUIComponent newParent)
        {
            this.parent = newParent;
        }

        public virtual void Finish() { Finished = true; }

        public void Activate() { Activated = true; }

        public void Deactivate() { Activated = false; }

        public virtual IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class GUIItem : GUIComponent
    {
        public GUIItem(string name, GUIComponent parent, int rx, int ry) : base(name, parent, rx, ry) { }
    }

    public class GUIGroup : GUIComponent
    {
        protected List<GUIComponent> components;
        List<GUIComponent> removeList;


        public GUIGroup(string name, GUIComponent parent, int rx, int ry) : base(name, parent, rx, ry)
        {
            components = new List<GUIComponent>();
            removeList = new List<GUIComponent>();
        }

        /// <summary>
        /// Adds the component to this group. Change component's parent to this group.
        /// </summary>
        /// <param name="component">Component.</param>
        public virtual void AddComponent(GUIComponent component)
        {
            components.Add(component);
            component.ChangeParent(this);
        }

        public virtual void RemoveComponent(GUIComponent component)
        {
            components.Remove(component);
        }

        public virtual GUIComponent FindComponent(string name)
        {
            foreach (GUIComponent component in components)
            {
                if (component.name == name)
                    return component;
            }
            return null;
        }

        public virtual List<GUIComponent> GetChild()
        {
            return components;
        }

        public override void DrawBegin(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GUIComponent component in components)
            {
                if (component.Activated)
                    component.DrawBegin(gameTime, spriteBatch);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (GUIComponent component in components)
            {
                if (component.Activated)
                    component.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GUIComponent component in components)
            {
                // Prevent multiple reference
                if (component.parent != this && !(component is NullGUIComponent))
                    removeList.Add(component);
                else
                {
                    if (component.Activated)
                        component.Update(gameTime);
                    if (component.Finished)
                        removeList.Add(component);
                }
            }
            for (int i = 0; i < removeList.Count; i++)
            {
                components.Remove(removeList[i]);
            }
            removeList.Clear();
        }

        public override IEnumerator GetEnumerator()
        {
            List<GUIComponent> copyList = new List<GUIComponent>();
            foreach (GUIComponent component in components)
            {
                copyList.Add(component);
            }

            int index = 0;
            while (index < copyList.Count)
            {
                yield return copyList[index];
                if (copyList[index] is GUIGroup)
                {
                    foreach (GUIComponent interComponent in copyList[index])
                    {
                        copyList.Add(interComponent);
                    }
                }

                index++;
            }
        }

        public override void Finish()
        {
            foreach (GUIComponent component in components)
            {
                component.Finish();
            }
            base.Finish();
        }
    }

    /// <summary>
    /// This class is used for parent component of gui that does not have parent from parameter.
    /// </summary>
    public class NullGUIComponent : GUIComponent
    {
        public override int x { get { return 0; } }
        public override int y { get { return 0; } }

    }
}
