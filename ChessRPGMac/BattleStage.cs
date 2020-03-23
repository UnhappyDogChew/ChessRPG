using System;
using System.Collections;
using System.Collections.Generic;

namespace ChessRPGMac
{
    public enum StageRow
    {
        EnemyBehind, 
        EnemyFront, 
        HeroFront, 
        HeroBehind, 
    }

    public class BattleStage : IEnumerable
    {  
        public List<FighterObject>[] fighterLists { get; private set; }

        GameObjectLayer gameObjectLayer;

        public BattleStage()
        {
            fighterLists = new List<FighterObject>[4];
            for (int i = 0; i < 4; i++)
            {
                fighterLists[i] = new List<FighterObject>();
            }
            gameObjectLayer = (GameObjectLayer)Global.world.GetLayer("GameObjectLayer");
        }
        /// <summary>
        /// Add <see cref="FighterObject"/> to right list and <see cref="GameObjectLayer"/>
        /// </summary>
        /// <returns><c>true</c>, if fighter object was added, <c>false</c> otherwise.</returns>
        /// <param name="fighterObject">Fighter object.</param>
        public bool AddFighterObject(FighterObject fighterObject, int i = -1)
        {
            int index = 0;
            if (fighterObject is EnemyObject ||
                (fighterObject is NullFighterObject && ((NullFighterObject)fighterObject).type == FighterType.Enemy))
            {
                if (fighterObject.state == FighterState.Behind)
                    index = 0;
                else if (fighterObject.state == FighterState.Front)
                    index = 1;
            }
            else if (fighterObject is HeroObject ||
                (fighterObject is NullFighterObject && ((NullFighterObject)fighterObject).type == FighterType.Hero))
            {
                if (fighterObject.state == FighterState.Front)
                    index = 2;
                else if (fighterObject.state == FighterState.Behind)
                    index = 3;
            }
            else
                throw new InvalidFighterObjectException();

            if (fighterLists[index].Count >= Global.Properties.FIGHTER_IN_ROW)
                return false;

            if (i == -1)
                fighterLists[index].Add(fighterObject);
            else
                fighterLists[index].Insert(i, fighterObject);
            gameObjectLayer.elements.Add(fighterObject);
            return true;
        }
        /// <summary>
        /// Search given <see cref="FighterObject"/> in lists and remove it.
        /// also that <paramref name="fighterObject"/> is finished.
        /// </summary>
        /// <returns><c>true</c>, if fighter object was removed, <c>false</c> otherwise.</returns>
        /// <param name="fighterObject">Fighter object.</param>
        public bool RemoveFighterObject(FighterObject fighterObject)
        {
            int index = 0;
            if (fighterObject is EnemyObject ||
                (fighterObject is NullFighterObject && ((NullFighterObject)fighterObject).type == FighterType.Enemy))
            {
                if (fighterObject.state == FighterState.Behind)
                    index = 0;
                else if (fighterObject.state == FighterState.Front)
                    index = 1;
            }
            else if (fighterObject is HeroObject ||
                (fighterObject is NullFighterObject && ((NullFighterObject)fighterObject).type == FighterType.Hero))
            {
                if (fighterObject.state == FighterState.Front)
                    index = 2;
                else if (fighterObject.state == FighterState.Behind)
                    index = 3;
            }
            else
                throw new InvalidFighterObjectException();

            return fighterLists[index].Remove(fighterObject);
        }

        public void SwitchLocation(FighterObject fighterObject1, FighterObject fighterObject2)
        {
            if (fighterObject1.GetType() != fighterObject2.GetType() && !(fighterObject1 is NullFighterObject) && 
                !(fighterObject2 is NullFighterObject))
                throw new InvalidFighterObjectException();

            string fl1 = GetFighterObjectLocation(fighterObject1);
            string fl2 = GetFighterObjectLocation(fighterObject2);

            // Sort fighterObject if both have same state.
            if (fl1[0] == fl2[0] && fl1[1] > fl2[1])
            {
                string tempString = fl1;
                fl1 = fl2;
                fl2 = tempString;

                FighterObject tempObject = fighterObject1;
                fighterObject1 = fighterObject2;
                fighterObject2 = tempObject;
            }

            // Remove fighterObjects
            fighterLists[int.Parse(fl1[0].ToString())].Remove(fighterObject1);
            fighterLists[int.Parse(fl2[0].ToString())].Remove(fighterObject2);

            // Add fighterObjects in proper location
            fighterLists[int.Parse(fl1[0].ToString())].Insert(int.Parse(fl1[1].ToString()), fighterObject2);
            fighterLists[int.Parse(fl2[0].ToString())].Insert(int.Parse(fl2[1].ToString()), fighterObject1);

            // Change state
            FighterState tempState = fighterObject1.state;

            fighterObject1.ChangeState(fighterObject2.state);
            fighterObject2.ChangeState(tempState);
        }

        public bool MoveLocation(FighterObject fighterObject, FighterState state)
        {
            RemoveFighterObject(fighterObject);
            fighterObject.ChangeState(state);
            return AddFighterObject(fighterObject);
        }

        /// <summary>
        /// Gets the fighter object location. Each character indicates row and column.
        /// </summary>
        /// <returns>The fighter object location.</returns>
        /// <param name="fighterObject">Fighter object.</param>
        public string GetFighterObjectLocation(FighterObject fighterObject)
        {
            int index = 0;
            if (fighterObject is EnemyObject ||
                (fighterObject is NullFighterObject && ((NullFighterObject)fighterObject).type == FighterType.Enemy))
            {
                if (fighterObject.state == FighterState.Behind)
                    index = 0;
                else if (fighterObject.state == FighterState.Front)
                    index = 1;
            }
            else if (fighterObject is HeroObject ||
                (fighterObject is NullFighterObject && ((NullFighterObject)fighterObject).type == FighterType.Hero))
            {
                if (fighterObject.state == FighterState.Front)
                    index = 2;
                else if (fighterObject.state == FighterState.Behind)
                    index = 3;
            }
            else
                throw new InvalidFighterObjectException();

            if (fighterLists[index].Contains(fighterObject))
                return index.ToString() + fighterLists[index].IndexOf(fighterObject);

            throw new FighterObjectNotFoundException();
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < 4; i++)
            {
                foreach (FighterObject fighter in fighterLists[i])
                    yield return fighter;
            }
        }
    }

    public class InvalidFighterObjectException : Exception
    {

    }

    public class FighterObjectNotFoundException : Exception
    {

    }
}
