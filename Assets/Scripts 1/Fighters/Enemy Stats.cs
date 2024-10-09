using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private static Dictionary<string, Stats> predefinedEnemyStats = new Dictionary<string, Stats>
    {
        { "Enemy 1", new Stats(3, 13, 18, 15f, 10f, 12f,10) },
        { "Enemy 2", new Stats(4, 21,15, 18f, 15f, 8f,10) },
        { "Lupus", new Stats(2, 52, 20, 10f, 12f, 14f,30) },
        { "Helena", new Stats(5, 35, 18, 22f, 20f, 10f,50) }
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

