using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public static class HeroeStats 
    {
 
            private static Dictionary<string, Stats> predefinedStats = new Dictionary<string, Stats>
        {
            { "Heroe 1", new Stats(20, 38, 17,20, 25, 15,0) },
            { "Heroe 2", new Stats(25, 47, 23,22, 30, 10,0) },
            { "Heroe 3", new Stats(24, 50, 19,18, 20, 20, 0) },
            { "Heroe 4", new Stats(23, 38, 16,25, 35, 12,0) }
        };

            public static Stats GetStatsForHero(string heroName)
            {
                if (predefinedStats.ContainsKey(heroName))
                {
                    return predefinedStats[heroName];
                }
                else
                {
                    throw new System.ArgumentException("Heroe no encontrado: " + heroName);
                }
            }
     } 



