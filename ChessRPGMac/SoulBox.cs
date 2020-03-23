using System;
using System.Collections.Generic;

namespace ChessRPGMac
{
    public class SoulBox
    {
        Dictionary<Hero, int>[] heroPool;

        public SoulBox()
        {
            heroPool = new Dictionary<Hero, int>[5];
            for (int i = 0; i < 5; i++)
            {
                heroPool[i] = new Dictionary<Hero, int>();
            }

            AddHero(new Hero1(FighterState.Front), 5);
            AddHero(new Hero2(FighterState.Front), 5);
        }

        public Hero[] GetRandomHeros(int level, int count)
        {
            int rarityRange = level + 2;
            int availableCount = 0;
            Dictionary<Hero, int> availableHeros = new Dictionary<Hero, int>();
            for (int i = 0; i < rarityRange; i++)
            {
                foreach (Hero hero in heroPool[i].Keys)
                {
                    availableHeros.Add(hero, heroPool[i][hero]);
                    availableCount += heroPool[i][hero];
                }
            }

            if (availableCount < count)
                return null;

            Random random = new Random();
            int[] picks = new int[count];
            for (int i = 0; i < count; i++)
            {
                int pick = random.Next(availableCount);
                for (int j = 0; j < i; j++)
                {
                    while (picks[j] == pick)
                    {
                        pick++;
                        if (pick >= availableCount)
                            pick = 0;
                    }
                }
                picks[i] = pick;
            }

            // Sort picks (Bubble sort)
            for (int i = 0; i < count - 1; i++)
            {
                for (int j = 0; j < count - i - 1; j++)
                {
                    if (picks[i + j] > picks[i + j + 1])
                    {
                        int temp = picks[i + j];
                        picks[i + j] = picks[i + j + 1];
                        picks[i + j + 1] = temp;
                    }
                }
            }

            int v = 0;
            int pickIndex = 0;
            Hero[] result = new Hero[count];
            foreach (Hero hero in availableHeros.Keys)
            {
                v += availableHeros[hero];
                for (int i = pickIndex; i < count; i++)
                {
                    if (v >= picks[i])
                    {
                        heroPool[hero.rarity][hero] -= 1;
                        if (heroPool[hero.rarity][hero] == 0)
                        {
                            heroPool[hero.rarity].Remove(hero);
                        }
                        result[pickIndex] = hero;
                        pickIndex++;
                        break;
                    }
                }
            }
            return result;
        }

        private void AddHero(Hero hero, int count)
        {
            heroPool[hero.rarity].Add(hero, count);
        }
    }
}
