using UnityEngine;
using System.Collections.Generic;

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
        foreach (var receiver in this.receivers)
        {
            // Realizar la tirada de ataque
            int roll = Random.Range(0, 10); // Genera un número entre 0 y 9
            LogPanel.Write($"{emitter.idName} realiza una tirada de ataque: {roll}");

            if (roll <= 3) // Pifia
            {
                LogPanel.Write($"{emitter.idName} ha fallado su ataque (pifia) con un número de {roll} y no realiza ninguna acción.");
                // Aquí puedes decidir si quieres hacer algo más en caso de pifia
                continue; // No ejecuta la habilidad si hay pifia
            }

            LogPanel.Write($"{emitter.idName} ha tenido éxito en su ataque con un número de {roll}.");
            this.Animate(receiver);
            this.OnRun(receiver); // Solo se ejecuta si hay éxito
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