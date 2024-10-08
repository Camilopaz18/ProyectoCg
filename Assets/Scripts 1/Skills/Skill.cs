using UnityEngine;
using System.Collections.Generic;
using System.Text;

public abstract class Skill : MonoBehaviour
{
    [Header("Base Skill")]
    public string skillName;
    public float animationDuration;

    public SkillTargeting targeting;

    public GameObject effectPrfb;

    protected Fighter emitter;
    protected List<Fighter> receivers;

    protected Queue<string> messages;

    protected int damage;

    public bool needsManualTargeting
    {
        get
        {
            switch (this.targeting)
            {
                case SkillTargeting.SINGLE_ALLY:
                case SkillTargeting.SINGLE_OPPONENT:
                    return true;

                default:
                    return false;
            }
        }
    }

    void Awake()
    {
        this.messages = new Queue<string>();
        this.receivers = new List<Fighter>();
    }

    private void Animate(Fighter receiver)
    {
        var go = Instantiate(this.effectPrfb, receiver.transform.position, Quaternion.identity);
        Destroy(go, this.animationDuration);
    }



    public void Run()
    {
        foreach(var receiver in this.receivers)
    {
            // Usar StringBuilder para acumular los mensajes
            StringBuilder logMessages = new StringBuilder();

            // Realizar la tirada de ataque inicial
            int initialRoll = Random.Range(0, 10); // Genera un n�mero entre 0 y 9
            logMessages.AppendLine($"{emitter.idName} realiza una tirada de ataque: {initialRoll}");

            if (initialRoll <= 3) // Pifia
            {
                logMessages.AppendLine($"{emitter.idName} ha fallado su ataque (pifia) con un n�mero de {initialRoll} y no realiza ninguna acci�n.");
                LogPanel.Write(logMessages.ToString());
                continue; // No ejecuta la habilidad si hay pifia
            }

            logMessages.AppendLine($"{emitter.idName} ha tenido �xito en su ataque con un n�mero de {initialRoll}.");

            // Si el ataque inicial es exitoso, se realizan dos tiradas para calcular el da�o
            int roll1 = Random.Range(0, 10); // Primera tirada
            int roll2 = Random.Range(0, 10); // Segunda tirada
            int totalAttackValue = roll1 * 10 + roll2; // Valor total del ataque
            logMessages.AppendLine($"{emitter.idName} realiza una tirada de da�o: {roll1} y {roll2} (Total: {totalAttackValue})");

            // Validaciones seg�n las reglas que has especificado
            if (totalAttackValue > 70 && totalAttackValue <= 99)
            {
                logMessages.AppendLine($"{emitter.idName} ha fallado su ataque (pifia) con un n�mero de {totalAttackValue} y no realiza ninguna acci�n.");
                LogPanel.Write(logMessages.ToString());
                continue; // No hace da�o y contin�a al siguiente receptor
            }

            if (totalAttackValue < 70)
            {
                if (totalAttackValue > this.emitter.stats.attack)
                {
                    // Aqu� se considera un ataque exitoso, realiza el da�o
                    logMessages.AppendLine($"{emitter.idName} ha tenido �xito en su ataque con un n�mero de {totalAttackValue}.");
                    this.Animate(receiver);

                    // Aqu� es donde calculamos el da�o basado en los dados
                    int totalDamage = CalculateDamageBasedOnDice(logMessages);

                    // Aplicar el da�o al enemigo
                    receiver.ModifyHealth(-totalDamage);

                    // Mensaje que muestra el da�o infligido
                    logMessages.AppendLine($"Da�o infligido: {totalDamage}");
                    this.OnRun(receiver); // Solo se ejecuta si hay �xito
                }
                else
                {
                    logMessages.AppendLine($"{emitter.idName} ha fallado su ataque ya que el valor de ataque es menor que {this.emitter.stats.attack}.");
                }
            }

            // Muestra todos los mensajes acumulados en LogPanel
            LogPanel.Write(logMessages.ToString());
        }

        this.receivers.Clear();
    }

    public void SetEmitter(Fighter _emitter)
    {
        this.emitter = _emitter;
    }

    public void AddReceiver(Fighter _receiver)
    {
        this.receivers.Add(_receiver);
    }

    public string GetNextMessage()
    {
        if (this.messages.Count != 0)
            return this.messages.Dequeue();
        else
            return null;
    }
    public void SetDamage(int _damage)
    {
        this.damage = _damage;
    }

    protected abstract void OnRun(Fighter receiver);

    private int CalculateDamageBasedOnDice(StringBuilder logMessages)
    {
        // Aqu� puedes definir las combinaciones de dados para el ataque
        List<string> diceCombinations = new List<string> { "1D10", "1D4" }; // Ejemplo para H�roe 1, Ataque 1: 1D10 + 1D4

        int totalDamage = 0;
        List<int> results = new List<int>(); // Lista para almacenar resultados de cada tirada

        // Recorrer cada combinaci�n de dados y hacer las tiradas
        foreach (var dice in diceCombinations)
        {
            int diceResult = RollDiceFromString(dice);
            results.Add(diceResult); // Almacenar el resultado
            totalDamage += diceResult; // Sumar el resultado de cada tirada al total
        }

        // Construir el mensaje de da�o infligido
        string damageMessage = string.Join(" + ", results);
        logMessages.AppendLine($"Da�o infligido: {damageMessage} = {totalDamage}");

        return totalDamage;
    }
    private int RollDiceFromString(string dice)
    {
        string[] parts = dice.Split('D'); // Separar el n�mero de dados y las caras
        int numberOfDice = int.Parse(parts[0]); // Ej: "1D10" -> n�mero de dados = 1
        int sides = int.Parse(parts[1]); // Ej: "1D10" -> caras = 10

        return DiceRoller.RollDice(numberOfDice, sides); // Usar DiceRoller para calcular el resultado
    }


}