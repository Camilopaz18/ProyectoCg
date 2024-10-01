using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRolls : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool IsAttackSuccessful()
    {
        int roll = Random.Range(0, 10);  // N�mero aleatorio entre 0 y 9
        if (roll >= 1 && roll <= 3)
        {
            LogPanel.Write("�Pifia! Ataque fallido.");
            return false;  // Fallo
        }
        else
        {
            LogPanel.Write($"Tirada de �xito: {roll}");
            return true;   // �xito
        }
    }

    // Tirada para calcular el da�o del ataque
    public static int CalculateDamage(Fighter attacker)
    {
        int roll1 = Random.Range(0, 10); // Primer dado de 10
        int roll2 = Random.Range(0, 10); // Segundo dado de 10

        int attackRoll = roll1 * 10 + roll2; // Combina los dos valores

        LogPanel.Write($"Tirada de da�o: {roll1}{roll2} = {attackRoll}");

        // Validar si es una pifia
        if (attackRoll > 70 && attackRoll <= 99)
        {
            LogPanel.Write("�Pifia en el da�o! Ataque fallido.");
            return 0; // No se realiza ning�n da�o
        }

        // Determinar si la tirada es exitosa en base a la fuerza del atacante
        if (attackRoll < 70 && attackRoll > attacker.GetCurrentStats().attack)
        {
            return attackRoll; // Tirada exitosa
        }
        else
        {
            LogPanel.Write("Da�o insuficiente para superar la fuerza del atacante.");
            return 0; // Da�o bloqueado
        }
    }
}

