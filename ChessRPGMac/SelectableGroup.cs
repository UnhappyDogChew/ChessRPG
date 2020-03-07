using System;
using System.Collections.Generic;

namespace ChessRPGMac
{
    /// <summary>
    /// Selectable group. This class merge several class together, so that selected together.
    /// </summary>
    public class SelectableGroup : ISelectable
    {
        public List<ISelectable> items { get; private set; }

        public SelectableGroup()
        {
            items = new List<ISelectable>();
        }

        public Point GetLocation()
        {
            Point result = new Point(0, 0);
            if (items[0] != null)
                result = items[0].GetLocation();
            if (items.Count == 1)
                return result;
            for (int i = 1; i < items.Count; i++)
            {
                if (items[i] == null)
                    continue;
                if (result > items[i].GetLocation())
                    result = items[i].GetLocation();
            }
            return result;
        }

        public Point GetSize()
        {
            Point result = new Point(0, 0);

            foreach (ISelectable item in items)
            {
                Point location = item.GetLocation();
                Point size = item.GetSize();

                if (result.x < (location.x + size.x) - GetLocation().x)
                    result.x = (location.x + size.x) - GetLocation().x;
                if (result.y < (location.y + size.y) - GetLocation().y)
                    result.y = (location.y + size.y) - GetLocation().y;
            }
            return result;
        }

        public void Focus()
        {
            foreach (ISelectable item in items)
            {
                item.Focus();
            }
        }

        public void Leave()
        {
            foreach (ISelectable item in items)
            {
                item.Leave();
            }
        }

        public void SelectAction()
        {
            foreach (ISelectable item in items)
            {
                item.SelectAction();
            }
        }
    }
}