using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private static Dictionary<string, Stats> predefinedEnemyStats = new Dictionary<string, Stats>
    {
        { "Enemy 1", new Stats(3, 1, 20f, 15f, 10f, 12f) },
        { "Enemy 2", new Stats(4, 1,5f, 18f, 15f, 8f) },
        { "Enemy 3", new Stats(2, 70f, 15f, 10f, 12f, 14f) },
        { "Enemy 4", new Stats(5, 120f, 30f, 22f, 20f, 10f) }
    };

    public static Stats GetStatsForEnemy(string enemyName)
    {
        if (predefinedEnemyStats.ContainsKey(enemyName))
        {
            return predefinedEnemyStats[enemyName];
        }
        else
        {
            throw new System.ArgumentException("Enemigo no encontrado: " + enemyName);
        }
    }
}

