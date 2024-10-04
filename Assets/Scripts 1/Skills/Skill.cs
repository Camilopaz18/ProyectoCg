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
            int initialRoll = Random.Range(0, 10); // Genera un número entre 0 y 9
            logMessages.AppendLine($"{emitter.idName} realiza una tirada de ataque: {initialRoll}");

            if (initialRoll <= 3) // Pifia
            {
                logMessages.AppendLine($"{emitter.idName} ha fallado su ataque (pifia) con un número de {initialRoll} y no realiza ninguna acción.");
                LogPanel.Write(logMessages.ToString());
                continue; // No ejecuta la habilidad si hay pifia
            }

            logMessages.AppendLine($"{emitter.idName} ha tenido éxito en su ataque con un número de {initialRoll}.");

            // Si el ataque inicial es exitoso, se realizan dos tiradas para calcular el daño
            int roll1 = Random.Range(0, 10); // Primera tirada
            int roll2 = Random.Range(0, 10); // Segunda tirada
            int totalAttackValue = roll1 * 10 + roll2; // Valor total del ataque
            logMessages.AppendLine($"{emitter.idName} realiza una tirada de daño: {roll1} y {roll2} (Total: {totalAttackValue})");

            // Validaciones según las reglas que has especificado
            if (totalAttackValue > 70 && totalAttackValue <= 99)
            {
                logMessages.AppendLine($"{emitter.idName} ha fallado su ataque (pifia) con un número de {totalAttackValue} y no realiza ninguna acción.");
                LogPanel.Write(logMessages.ToString());
                continue; // No hace daño y continúa al siguiente receptor
            }

            if (totalAttackValue < 70)
            {
                if (totalAttackValue > this.emitter.stats.attack)
                {
                    // Aquí se considera un ataque exitoso, realiza el daño
                    logMessages.AppendLine($"{emitter.idName} ha tenido éxito en su ataque con un número de {totalAttackValue}.");
                    this.Animate(receiver);
                    this.OnRun(receiver); // Solo se ejecuta si hay éxito
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
    
}