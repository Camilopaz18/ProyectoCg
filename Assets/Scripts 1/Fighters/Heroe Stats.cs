using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HeroeStats 
{
 
        private static Dictionary<string, Stats> predefinedStats = new Dictionary<string, Stats>
    {
        { "Heroe 1", new Stats(5, 100f, 30f, 20f, 25f, 15f) },
        { "Heroe 2", new Stats(6, 120f, 35f, 22f, 30f, 10f) },
        { "Heroe 3", new Stats(4, 90f, 25f, 18f, 20f, 20f) },
        { "Heroe 4", new Stats(7, 140f, 40f, 25f, 35f, 12f) }
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



