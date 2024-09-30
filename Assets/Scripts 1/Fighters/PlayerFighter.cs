using UnityEngine;

public class PlayerFighter : Fighter
{
    [Header("UI")]
    public PlayerSkillPanel skillPanel;
    public EnemiesPanel enemiesPanel;
    public Animator animator;
    private Skill skillToBeExecuted;

    private readonly Stats[] heroStats = new Stats[4]; // Array to store hero stats

    void Awake()
    {
        // Initialize hero stats (assuming you have defined the Stats class)
        heroStats[0] = new Stats(21, 60, 50, 45, 20, 20); // Hero 1 stats
        heroStats[1] = new Stats(25, 50, 40, 50, 15, 25); // Hero 2 stats (example)
        heroStats[2] = new Stats(18, 70, 60, 30, 25, 15); // Hero 3 stats (example)
        heroStats[3] = new Stats(28, 45, 35, 55, 20, 30); // Hero 4 stats (example)

        this.stats = heroStats[0]; // Assign default stats to current player (can be changed)
    }
    public override void InitTurn()
    {
        this.skillPanel.ShowForPlayer(this);

        for (int i = 0; i < this.skills.Length; i++)
        {
            this.skillPanel.ConfigureButton(i, this.skills[i].skillName);
        }
    }

    /// ================================================
    /// <summary>
    /// Se llama desde EnemiesPanel.
    /// </summary>
    /// <param name="index"></param>
    public void ExecuteSkill(int index)
    {
        this.skillToBeExecuted = this.skills[index];
        this.skillToBeExecuted.SetEmitter(this);

        if (this.skillToBeExecuted.needsManualTargeting)
        {
            Fighter[] receivers = this.GetSkillTargets(this.skillToBeExecuted);
            this.enemiesPanel.Show(this, receivers);
        }
        else
        {
            this.AutoConfigureSkillTargeting(this.skillToBeExecuted);
            
            this.combatManager.OnFighterSkill(this.skillToBeExecuted);
            this.skillPanel.Hide();
        }
    }

    public void SetTargetAndAttack(Fighter enemyFigther)
    {
        this.skillToBeExecuted.AddReceiver(enemyFigther);

        this.combatManager.OnFighterSkill(this.skillToBeExecuted);

        this.skillPanel.Hide();
        this.enemiesPanel.Hide();
    }
}
