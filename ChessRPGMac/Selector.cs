using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;

namespace ChessRPGMac
{
    public enum SelectMode
    {
        One,
        Row,
        All,
    }

    public class Selector
    {
        SoundEffect menuSelect_soundEffect;
        SoundEffect menuDecide_soundEffect;

        Dictionary<string, ISelectable[,]> matrices;
        Stack<string> nameStack;
        Stack<Point> cursorStack;
        SelectableGroup allGroup;
        SelectableGroup[] rowGroups;
        public string matrixName { get { return (nameStack.Count > 0) ? nameStack.Peek() : ""; } }
        int width { get { return matrices[matrixName].GetLength(1); } }
        int height { get { return matrices[matrixName].GetLength(0); } }

        public delegate bool MatrixConstraint(int row, int col);
        public MatrixConstraint constraint = (int row, int col) => { return true; };
        public SelectMode mode { get; set; }
        public bool mirrorMode { get; set; }

        Point cursor;
        ISelectable currentSelectable {
            get
            {
                try
                {
                    switch (mode)
                    {
                        case SelectMode.One: return matrices[matrixName][cursor.y, cursor.x];
                        case SelectMode.Row: return rowGroups[cursor.y];
                        case SelectMode.All: return allGroup;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    return null;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
                return null;
            }
        }

        // About Effects.
        EffectLayer topEffectLayer;
        SelectingEffect selectingEffect;

        public Selector(bool mirrorMode = false)
        {
            menuSelect_soundEffect = Global.content.Load<SoundEffect>("SE_Menu5");
            menuDecide_soundEffect = Global.content.Load<SoundEffect>("SE_Menu3");
            cursor = new Point(-1, -1);
            topEffectLayer = (EffectLayer)Global.world.GetLayer("TopEffectLayer");
            matrices = new Dictionary<string, ISelectable[,]>();
            nameStack = new Stack<string>();
            cursorStack = new Stack<Point>();
            mode = SelectMode.One;
            allGroup = new SelectableGroup();
            this.mirrorMode = mirrorMode;
        }

        public void CreateNewMatrix(string name, int width, int height)
        {
            matrices.Add(name, new ISelectable[height, width]);
        }

        public bool SetItemToMatrix(string name, ISelectable item, int row, int col)
        {
            try
            {
                matrices[name][row, col] = item;
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public bool SetItemToMatrixAtLast(string name, ISelectable item, int row)
        {
            try
            {
                for (int col = 0; col < width; col++)
                {
                    if (matrices[matrixName][row, col] == null)
                    {
                        matrices[matrixName][row, col] = item;
                        return true;
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
                return false;
            }
            return false;
        }

        public bool RemoveItemToMatrix(string name, ISelectable item)
        {
            for (int row = 0; row < matrices[name].GetLength(0); row++)
            {
                for (int col = 0; col < matrices[name].GetLength(1); col++)
                {
                    if (matrices[name][row, col].Equals(item))
                    {
                        matrices[name][row, col] = null;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool RemoveMatrix(string name)
        {
            return matrices.Remove(name);
        }

        public bool ChangeMatrix(string name)
        {
            foreach (string key in matrices.Keys)
            {
                if (key == name)
                {
                    nameStack.Push(name);
                    cursorStack.Push(cursor);
                    SelectFirstItem();
                    return true;
                }
            }
            return false;
        }

        public void ClearMatrix(string name)
        {
            for (int row = 0; row < matrices[name].GetLength(0); row++)
            {
                for (int col = 0; col < matrices[name].GetLength(1); col++)
                {
                    matrices[name][row, col] = null;
                }
            }
        }

        #region Select Methods
        public bool SelectFirstItem()
        {
            try
            {
                currentSelectable?.Leave();
                bool itemFound = false;
                if (mode == SelectMode.One)
                {
                    for (int row = 0; row < height; row++)
                    {
                        for (int col = 0; col < width; col++)
                        {
                            if (matrices[matrixName][row, col] != null && constraint(row, col))
                            {
                                cursor = new Point(col, row);
                                SetSelectingEffect();
                                itemFound = true;
                                break;
                            }
                        }
                        if (itemFound)
                            break;
                    }
                }
                else if (mode == SelectMode.Row)
                {
                    for (int row = 0; row < height; row++)
                    {
                        if (rowGroups[row] != null)
                        {
                            cursor.y = row;
                            SetSelectingEffect();
                            itemFound = true;
                            break;
                        }
                    }
                }
                else if (mode == SelectMode.All)
                    itemFound = true;

                currentSelectable?.Focus();
                return itemFound;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool SelectItem(int row, int col)
        {
            try
            {
                if (matrices[matrixName][row, col] == null || !constraint(row, col))
                    return false;
                currentSelectable?.Leave();
                cursor = new Point(col, row);
                currentSelectable?.Focus();
                SetSelectingEffect();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public void SelectNext(Direction direction)
        {
            if (mode == SelectMode.All)
                return;
            if (mode == SelectMode.Row)
            {
                if (direction == Direction.Right || direction == Direction.Left)
                    return;
            }
            if (cursor == new Point(-1, -1))
            {
                SelectFirstItem();
                return;
            }
            currentSelectable?.Leave();
            if (Next(direction))
                SetSelectingEffect();
            currentSelectable?.Focus();
        }
        #endregion

        public void SelectAction()
        {
            try
            {
                currentSelectable?.SelectAction();
                menuDecide_soundEffect.Play(0.5f, 0, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }
        }

        public ISelectable GetItem(string name, int row, int col)
        {
            return matrices[name][row, col];
        }

        public void Finish()
        {
            selectingEffect?.Finish();
        }

        public void Rewind()
        {
            // if nameStack has only one name, skip changing name and set cursor (-1, -1).
            if (nameStack.Count == 1)
            {
                cursor = new Point(-1, -1);
                cursorStack.Clear();
                SetSelectingEffect();
                return;
            }
            nameStack.Pop();
            if (cursorStack.Count == 0)
                return;
            cursor = cursorStack.Pop();
            SelectItem(cursor.y, cursor.x);
        }

        public void ResetConstraint() { constraint = (int row, int col) => true; }

        public void ChangeSelectMode(SelectMode mode)
        {
            if (this.mode == mode)
                return;
            this.mode = mode;
            if (mode == SelectMode.All)
                SetAllGroup();
            else if (mode == SelectMode.Row)
                SetRowGroups();

            SetSelectingEffect();
        }

        private void SetAllGroup()
        {
            allGroup.items.Clear();
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    if (!constraint(row, col) || matrices[matrixName][row, col] == null)
                        continue;
                    allGroup.items.Add(matrices[matrixName][row, col]);
                }
            }
        }

        private void SetRowGroups()
        {
            rowGroups = new SelectableGroup[height];
            for (int row = 0; row < height; row++)
            {
                rowGroups[row] = new SelectableGroup();
                for (int col = 0; col < width; col++)
                {
                    if (!constraint(row, col) || matrices[matrixName][row, col] == null)
                        continue;
                    rowGroups[row].items.Add(matrices[matrixName][row, col]);
                }
                if (rowGroups[row].items.Count == 0)
                    rowGroups[row] = null;
            }
        }

        public void Reset()
        {
            cursor = new Point(-1, -1);
            nameStack.Clear();
            cursorStack.Clear();
            selectingEffect?.Finish();
            selectingEffect = null;
        }

        public bool AlignItems(string name, int row)
        {
            try
            {
                Queue<int> nullIndexQueue = new Queue<int>();
                for (int col = 0; col < matrices[name].GetLength(1); col++)
                {
                    if (matrices[name][row, col] == null)
                    {
                        nullIndexQueue.Enqueue(col);
                    }
                    else if (nullIndexQueue.Count != 0)
                    {
                        ISelectable item = matrices[name][row, col];
                        matrices[name][row, col] = null;
                        matrices[name][row, nullIndexQueue.Dequeue()] = item;
                        nullIndexQueue.Enqueue(col);
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public bool AlignItems(string name)
        {
            try
            {
                for (int row = 0; row < matrices[name].GetLength(0); row++)
                {
                    AlignItems(name, row);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        private void SetSelectingEffect()
        {
            try
            {
                // Choose different ISelectable depends on mode.
                Point l;
                Point s;
                if (mode == SelectMode.One)
                {
                    if (matrices[matrixName][cursor.y, cursor.x] == null)
                    {
                        selectingEffect.Finish();
                        selectingEffect = null;
                        return;
                    }
                    l = matrices[matrixName][cursor.y, cursor.x].GetLocation();
                    s = matrices[matrixName][cursor.y, cursor.x].GetSize();
                }
                else if (mode == SelectMode.Row)
                {
                    if (rowGroups[cursor.y] == null)
                    {
                        selectingEffect.Finish();
                        selectingEffect = null;
                        return;
                    }
                    l = rowGroups[cursor.y].GetLocation();
                    s = rowGroups[cursor.y].GetSize();
                }
                else
                {
                    l = allGroup.GetLocation();
                    s = allGroup.GetSize();
                }

                if (selectingEffect == null)
                {
                    selectingEffect = new SelectingEffect(l.x, l.y, s.x, s.y, 2);
                    topEffectLayer.elements.Add(selectingEffect);
                }
                else
                {
                    selectingEffect.x = l.x;
                    selectingEffect.y = l.y;
                    selectingEffect.width = s.x;
                    selectingEffect.height = s.y;
                }
                menuSelect_soundEffect.Play(0.2f, 0, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                selectingEffect?.Finish();
                selectingEffect = null;
                return;
            }
        }

        private bool Next(Direction direction)
        {
            try
            {
                // If Cursor is out of range, Go to first item.
                if (cursor.x < 0 || cursor.x >= width || cursor.y < 0 || cursor.y >= height)
                {
                    return SelectFirstItem();
                }

                if (direction == Direction.Right)
                {
                    if (mirrorMode)
                    {
                        int start = cursor.x;
                        for (int i = 1; i < width; i++)
                        {
                            int x = (start + i > width - 1) ? start + i - width : start + i;
                            if (matrices[matrixName][cursor.y, x] != null && constraint(cursor.y, x))
                            {
                                cursor.x = x;
                                return true;
                            }
                        }
                        return false;
                    }
                    if (cursor.x >= width - 1)
                        return false;
                    for (int i = cursor.x + 1; i < width; i++)
                    {
                        if (matrices[matrixName][cursor.y, i] != null && constraint(cursor.y, i))
                        {
                            cursor.x = i;
                            return true;
                        }
                    }
                }
                else if (direction == Direction.Left)
                {
                    if (mirrorMode)
                    {
                        int start = cursor.x;
                        for (int i = 1; i < width; i++)
                        {
                            int x = (start - i < 0) ? start - i + width : start - i;
                            if (matrices[matrixName][cursor.y, x] != null && constraint(cursor.y, x))
                            {
                                cursor.x = x;
                                return true;
                            }
                        }
                        return false;
                    }
                    if (cursor.x <= 0)
                        return false;
                    for (int i = cursor.x - 1; i >= 0; i--)
                    {
                        if (matrices[matrixName][cursor.y, i] != null && constraint(cursor.y, i))
                        {
                            cursor.x = i;
                            return true;
                        }
                    }
                }
                else if (direction == Direction.Down)
                {
                    if (mirrorMode)
                    {
                        int start = cursor.y;
                        for (int i = 1; i < height; i++)
                        {
                            int y = (start + i > height - 1) ? start + i - height : start + i;

                            if (matrices[matrixName][y, cursor.x] != null && constraint(y, cursor.x))
                            {
                                cursor = new Point(cursor.x, y);
                                return true;
                            }
                            int maxDistance = Math.Max(cursor.x, width - cursor.x);
                            for (int j = 1; j <= maxDistance; j++)
                            {
                                if (cursor.x - j >= 0 && matrices[matrixName][y, cursor.x - j] != null 
                                    && constraint(y, cursor.x - j))
                                {
                                    cursor = new Point(cursor.x - j, y);
                                    return true;
                                }
                                else if (cursor.x + j < width && matrices[matrixName][y, cursor.x + j] != null
                                     && constraint(y, cursor.x + j))
                                {
                                    cursor = new Point(cursor.x + j, y);
                                    return true;
                                }
                            }

                        }
                        return false;
                    }
                    if (cursor.y >= height - 1)
                        return false;
                    for (int i = cursor.y + 1; i < height; i++)
                    {
                        if (matrices[matrixName][i, cursor.x] != null && constraint(i, cursor.x))
                        {
                            cursor = new Point(cursor.x, i);
                            return true;
                        }

                        int maxDistance = Math.Max(cursor.x, width - cursor.x);
                        for (int j = 1; j <= maxDistance; j++)
                        {
                            if (cursor.x - j >= 0 && matrices[matrixName][i, cursor.x - j] != null
                                && constraint(i, cursor.x - j))
                            {
                                cursor = new Point(cursor.x - j, i);
                                return true;
                            }
                            else if (cursor.x + j < width && matrices[matrixName][i, cursor.x + j] != null
                                 && constraint(i, cursor.x + j))
                            {
                                cursor = new Point(cursor.x + j, i);
                                return true;
                            }
                        }
                    }
                }
                else if (direction == Direction.Up)
                {
                    if (mirrorMode)
                    {
                        int start = cursor.y;
                        for (int i = 1; i < height; i++)
                        {
                            int y = (start - i < 0) ? start - i + height : start - i;

                            if (matrices[matrixName][y, cursor.x] != null && constraint(y, cursor.x))
                            {
                                cursor = new Point(cursor.x, y);
                                return true;
                            }

                            int maxDistance = Math.Max(cursor.x, width - cursor.x);
                            for (int j = 1; j <= maxDistance; j++)
                            {
                                if (cursor.x - j >= 0 && matrices[matrixName][y, cursor.x - j] != null
                                     && constraint(y, cursor.x - j))
                                {
                                    cursor = new Point(cursor.x - j, y);
                                    return true;
                                }
                                else if (cursor.x + j < width && matrices[matrixName][y, cursor.x + j] != null
                                     && constraint(y, cursor.x + j))
                                {
                                    cursor = new Point(cursor.x + j, y);
                                    return true;
                                }
                            }

                        }
                        return false;
                    }
                    if (cursor.y <= 0)
                        return false;
                    for (int i = cursor.y - 1; i >= 0; i--)
                    {
                        if (matrices[matrixName][i, cursor.x] != null && constraint(i, cursor.x))
                        {
                            cursor = new Point(cursor.x, i);
                            return true;
                        }

                        int maxDistance = Math.Max(cursor.x, width - cursor.x);
                        for (int j = 1; j <= maxDistance; j++)
                        {
                            if (cursor.x - j >= 0 && matrices[matrixName][i, cursor.x - j] != null
                                 && constraint(i, cursor.x - j))
                            {
                                cursor = new Point(cursor.x - j, i);
                                return true;
                            }
                            else if (cursor.x + j < width && matrices[matrixName][i, cursor.x + j] != null
                                 && constraint(i, cursor.x + j))
                            {
                                cursor = new Point(cursor.x + j, i);
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return false;
        }
    }
}
