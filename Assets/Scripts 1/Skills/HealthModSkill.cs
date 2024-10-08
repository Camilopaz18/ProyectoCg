using System.Collections.Generic;
using UnityEngine;

public enum HealthModType
{
    STAT_BASED, FIXED, PERCENTAGE
}

public class HealthModSkill : Skill
{
    [Header("Health Mod")]
    public List<string> diceCombinations; // Ej: "1D10", "1D4", "2D6"

    public HealthModType modType;

    [Range(0f, 1f)]
    public float critChance = 0;

    protected override void OnRun(Fighter receiver)
    {
        // Calcular el daño basado en las tiradas de dados
        int totalDamage = 0;
        foreach (var Combi in diceCombinations)
        {
            totalDamage += RollDiceFromString(Combi); // Realizar tirada por cada combinación de dados
        }

        // Probabilidad de golpe crítico
        float dice = Random.Range(0f, 1f);
        if (dice <= this.critChance)
        {
            totalDamage *= 2; // Duplicar daño por crítico
            this.messages.Enqueue("¡Golpe crítico!");
        }

        // Aplicar el daño al enemigo
        receiver.ModifyHealth(-totalDamage);
    }

    // Método para convertir la cadena de dados "1D10", "2D6", etc., en una tirada real
    private int RollDiceFromString(string dice)
    {
        string[] parts = dice.Split('D'); // Separar el número de dados y las caras
        int numberOfDice = int.Parse(parts[0]); // Ej: "1D10" -> número de dados = 1
        int sides = int.Parse(parts[1]); // Ej: "1D10" -> caras = 10

        return DiceRoller.RollDice(numberOfDice, sides); // Usar DiceRoller para calcular el resultado
    }
}
    public class DiceRoller
    {
        // Realiza una tirada de varios dados del tipo especificado
        public static int RollDice(int numberOfDice, int sides)
        {
            int total = 0;
            for (int i = 0; i < numberOfDice; i++)
            {
                total += Random.Range(1, sides + 1); // Tirada de 1 a "sides"
            }
            return total;
        }
    }
