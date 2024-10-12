using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum CombatStatus
{
    WAITING_FOR_FIGHTER,
    FIGHTER_ACTION,
    CHECK_ACTION_MESSAGES,
    CHECK_FOR_VICTORY,
    NEXT_TURN,
    CHECK_FIGHTER_STATUS_CONDITION
}

public class CombatManager : MonoBehaviour
{
    private Fighter[] playerTeam;
    private Fighter[] enemyTeam;

    private Fighter[] fighters;
    private int fighterIndex;

    private bool isCombatActive;

    private CombatStatus combatStatus;

    private Skill currentFighterSkill;

    private List<Fighter> returnBuffer;

    public Fighter[] heroes; // Asigna estos en el inspector
    public Fighter[] enemies; // Asigna estos en el inspector



    void Start()
    {
        InitializeCombat();

        this.returnBuffer = new List<Fighter>();

        this.fighters = GameObject.FindObjectsOfType<Fighter>();

        this.SortFightersBySpeed();
        this.MakeTeams();

        LogPanel.Write("Batalla Iniciado.");

        this.combatStatus = CombatStatus.NEXT_TURN;

        this.fighterIndex = -1;
        this.isCombatActive = true;
        StartCoroutine(this.CombatLoop());


        //Cambios de sprite 

        LoadEnemySprite();
    }
    private void InitializeCombat()
    {
        // Reiniciar estadísticas de héroes
       // foreach (var hero in heroes)
       // {
         //   hero.stats = HeroeStats.GetStatsForHero(hero.idName).Clone(); // O inicializa de nuevo
         //   hero.statusPanel.SetStats(hero.idName, hero.stats);
      //  }

        // Reiniciar estadísticas de enemigos
        foreach (var enemy in enemies)
        {
            enemy.stats = EnemyStats.GetStatsForEnemy(enemy.idName).Clone(); // O inicializa de nuevo
            enemy.statusPanel.SetStats(enemy.idName, enemy.stats);
        }
    }

    private void SortFightersBySpeed()
    {
        bool sorted = false;
        while (!sorted)
        {
            sorted = true;

            for (int i = 0; i < this.fighters.Length - 1; i++)
            {
                Fighter a = this.fighters[i];
                Fighter b = this.fighters[i + 1];

                float aSpeed = a.GetCurrentStats().speed;
                float bSpeed = b.GetCurrentStats().speed;

                if (bSpeed > aSpeed)
                {
                    this.fighters[i] = b;
                    this.fighters[i + 1] = a;

                    sorted = false;
                }
            }
        }
    }

    private void MakeTeams()
    {
        List<Fighter> playersBuffer = new List<Fighter>();
        List<Fighter> enemiesBuffer = new List<Fighter>();

        foreach (var fgtr in this.fighters)
        {
            if (fgtr.team == Team.PLAYERS)
            {
                playersBuffer.Add(fgtr);
            }
            else if (fgtr.team == Team.ENEMIES)
            {
                enemiesBuffer.Add(fgtr);
            }

            fgtr.combatManager = this;
        }

        this.playerTeam = playersBuffer.ToArray();
        this.enemyTeam = enemiesBuffer.ToArray();
    }
     
    //
 
    //
    IEnumerator CombatLoop()
    {
        while (this.isCombatActive)
        {
            switch (this.combatStatus)
            {
                case CombatStatus.WAITING_FOR_FIGHTER:
                    yield return null;
                    break;

                case CombatStatus.FIGHTER_ACTION:
                    LogPanel.Write($"{this.fighters[this.fighterIndex].idName} uses {currentFighterSkill.skillName}.");

                    yield return null;

                    // Executing fighter skill
                    currentFighterSkill.Run();

                    // Wait for fighter skill animation 
                    yield return new WaitForSeconds(currentFighterSkill.animationDuration);
                    this.combatStatus = CombatStatus.CHECK_ACTION_MESSAGES;

                    break;


                case CombatStatus.CHECK_ACTION_MESSAGES:
                    string nextMessage = this.currentFighterSkill.GetNextMessage();

                    if (nextMessage != null)
                    {
                        LogPanel.Write(nextMessage);
                        yield return new WaitForSeconds(3f);
                    }
                    else
                    {
                        this.currentFighterSkill = null;
                        this.combatStatus = CombatStatus.CHECK_FOR_VICTORY;
                        yield return null;
                    }
                    break;

                case CombatStatus.CHECK_FOR_VICTORY:
                    bool arePlayersAlive = false;
                    foreach (var figther in this.playerTeam)
                    {
                        arePlayersAlive |= figther.isAlive;
                    }

                    // if (this.playerTeam[0].isAlive OR this.playerTeam[1].isAlive)

                    bool areEnemiesAlive = false;
                    foreach (var figther in this.enemyTeam)
                    {
                        areEnemiesAlive |= figther.isAlive;
                    }

                    bool victory = areEnemiesAlive == false;
                    bool defeat  = arePlayersAlive == false;

                    if (victory)
                    {
                        LogPanel.Write("Victoria!");
                        this.isCombatActive = false;

                        // Cambiar a la escena "" después de la victoria
                        EndCombat(true);
                    }

                    if (defeat)
                    {
                        LogPanel.Write("Derrota!");
                        this.isCombatActive = false;
                        // Cambiar a la escena "" después de la derrota

                        EndCombat(false);

                    }

                    if (this.isCombatActive)
                    {
                        this.combatStatus = CombatStatus.NEXT_TURN;
                    }

                    yield return null;
                    break;
                case CombatStatus.NEXT_TURN:
                    yield return new WaitForSeconds(1f);

                    Fighter current = null;

                    do {
                        this.fighterIndex = (this.fighterIndex + 1) % this.fighters.Length;

                        current = this.fighters[this.fighterIndex];
                    } while (current.isAlive == false);

                    this.combatStatus = CombatStatus.CHECK_FIGHTER_STATUS_CONDITION;

                    break;

                case CombatStatus.CHECK_FIGHTER_STATUS_CONDITION:
                    var currentFighter = this.fighters[this.fighterIndex];

                    // Comprobar si el nombre del luchador actual es "Lupus" y "Helena" sigue viva
                    if (currentFighter.idName == "Lupus")
                    {
                        bool isHelenaAlive = false;

                        // Verificar si Helena está viva
                        foreach (var enemy in this.enemyTeam)
                        {
                            if (enemy.idName == "Helena" && enemy.isAlive)
                            {
                                isHelenaAlive = true;
                                break;
                            }
                        }

                        // Si Helena sigue viva, saltamos el turno de Lupus
                        if (isHelenaAlive)
                        {
                            LogPanel.Write("Lupus no puede atacar mientras Helena esté viva.");
                            this.combatStatus = CombatStatus.NEXT_TURN;
                            yield return null;
                            break;
                        }
                    }

                    var statusCondition = currentFighter.GetCurrentStatusCondition();

                    if (statusCondition != null)
                    {
                        statusCondition.Apply();

                        while (true)
                        {
                            string nextSCMessage = statusCondition.GetNextMessage();
                            if (nextSCMessage == null)
                            {
                                break;
                            }

                            LogPanel.Write(nextSCMessage);
                            yield return new WaitForSeconds(2f);
                        }

                        if (statusCondition.BlocksTurn())
                        {
                            this.combatStatus = CombatStatus.CHECK_FOR_VICTORY;
                            break;
                        }
                    }

                    LogPanel.Write($"{currentFighter.idName} has the turn.");
                    currentFighter.InitTurn();

                    this.combatStatus = CombatStatus.WAITING_FOR_FIGHTER;
                    break;
            }
        }
    }


    public Fighter[] FilterJustAlive(Fighter[] team)
    {
        this.returnBuffer.Clear();

        foreach (var fgtr in team)
        {
            if (fgtr.isAlive)
            {
                this.returnBuffer.Add(fgtr);
            }
        }

        return this.returnBuffer.ToArray();
    }

    public Fighter[] GetOpposingTeam()
    {
        Fighter currentFighter = this.fighters[this.fighterIndex];

        Fighter[] team = null;
        if (currentFighter.team == Team.PLAYERS)
        {
            team = this.enemyTeam;
        }
        else if (currentFighter.team == Team.ENEMIES)
        {
            team = this.playerTeam;
        }

        return this.FilterJustAlive(team);
    }

    public Fighter[] GetAllyTeam()
    {
        Fighter currentFighter = this.fighters[this.fighterIndex];

        Fighter[] team = null;
        if (currentFighter.team == Team.PLAYERS)
        {
            team = this.playerTeam;
        }
        else
        {
            team = this.enemyTeam;
        }

        return this.FilterJustAlive(team);
    }


    public void OnFighterSkill(Skill skill)
    {
        this.currentFighterSkill = skill;
        this.combatStatus = CombatStatus.FIGHTER_ACTION;
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="playerWon"></param>  //cosas que agrego para funciones de codigo 
    private void EndCombat(bool playerWon)
    {
        if (playerWon)
        {
            // Obtener la escena y el punto de contacto donde ocurrió la colisión
            string lastScene = PlayerPrefs.GetString("LastScene", "bosque"); // Predeterminado "bosque" si no encuentra nada
            float contactPointX = PlayerPrefs.GetFloat("ContactPointX", 0f);
            float contactPointY = PlayerPrefs.GetFloat("ContactPointY", 0f);

            // Cargar la escena original
            SceneManager.LoadScene(lastScene);

            // Reposicionar al jugador en el punto de contacto
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = new Vector3(contactPointX, contactPointY, player.transform.position.z);
            }
        }
    }

    private void LoadEnemySprite()
    {
        // Obtener el nombre del sprite del enemigo desde PlayerPrefs
        string enemySpriteName = PlayerPrefs.GetString("EnemySprite", "");

        // Si hay un sprite guardado, asignarlo a un GameObject
        if (!string.IsNullOrEmpty(enemySpriteName))
        {
            GameObject enemyGFX = GameObject.Find("Enemy 1/GFX"); // Cambia el nombre según tu jerarquía
            GameObject enemyGFX2 = GameObject.Find("Enemy2/GFX"); // Cambia el nombre según tu jerarquía

            // Cargar el prefab correspondiente
            GameObject enemyPrefab = Resources.Load<GameObject>("Sprites/" + enemySpriteName); // Cambia la ruta según la ubicación de tus prefabs

            if (enemyGFX != null)
            {
                SpriteRenderer enemySpriteRenderer = enemyGFX.GetComponent<SpriteRenderer>();

                // Verificar si se ha cargado un prefab y asignar su sprite
                if (enemyPrefab != null)
                {
                    SpriteRenderer prefabSpriteRenderer = enemyPrefab.GetComponentInChildren<SpriteRenderer>();
                    if (prefabSpriteRenderer != null)
                    {
                        enemySpriteRenderer.sprite = prefabSpriteRenderer.sprite; // Asignar el sprite del prefab
                    }
                    else
                    {
                        Debug.LogError("No se encontró un SpriteRenderer en el prefab: " + enemySpriteName);
                    }
                }
                else
                {
                    // Buscar el sprite por su nombre en los recursos
                    Sprite loadedSprite = Resources.Load<Sprite>("Sprites/" + enemySpriteName); // Cambia la ruta según la ubicación de tus sprites

                    if (loadedSprite != null)
                    {
                        enemySpriteRenderer.sprite = loadedSprite; // Asignar el sprite cargado
                    }
                    else
                    {
                        Debug.LogError("Sprite no encontrado: " + enemySpriteName);
                    }
                }
            }
            else
            {
                Debug.LogError("No se encontró el GameObject 'Enemy 1/GFX'.");
            }

            if (enemyGFX2 != null)
            {
                SpriteRenderer enemySpriteRenderer2 = enemyGFX2.GetComponent<SpriteRenderer>();
                if (enemyPrefab != null)
                {
                    SpriteRenderer prefabSpriteRenderer = enemyPrefab.GetComponentInChildren<SpriteRenderer>();
                    if (prefabSpriteRenderer != null)
                    {
                        enemySpriteRenderer2.sprite = prefabSpriteRenderer.sprite; // Asignar el sprite del prefab
                    }
                    else
                    {
                        Debug.LogError("No se encontró un SpriteRenderer en el prefab: " + enemySpriteName);
                    }
                }
                else
                {
                    Sprite loadedSprite2 = Resources.Load<Sprite>("Sprites/" + enemySpriteName); // Cambia la ruta según la ubicación de tus sprites

                    if (loadedSprite2 != null)
                    {
                        enemySpriteRenderer2.sprite = loadedSprite2; // Asignar el sprite cargado
                    }
                    else
                    {
                        Debug.LogError("Sprite no encontrado: " + enemySpriteName);
                    }
                }
            }
            else
            {
                Debug.LogError("No se encontró el GameObject 'Enemy 2/GFX'.");
            }
        }
    }

}
