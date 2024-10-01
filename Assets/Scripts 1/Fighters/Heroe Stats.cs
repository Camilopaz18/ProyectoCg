using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HeroeStats 
{
 
        private static Dictionary<string, Stats> predefinedStats = new Dictionary<string, Stats>
    {
        { "Heroe 1", new Stats(20, 100, 30, 20, 25, 15) },
        { "Heroe 2", new Stats(25, 120, 35, 22, 30, 10) },
        { "Heroe 3", new Stats(24, 90, 25, 18, 20, 20) },
        { "Heroe 4", new Stats(23, 140, 40, 25, 35, 12) }
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



