using UnityEngine;
using System.Collections.Generic;

public class InvaderFormation : MonoBehaviour
{
    public static InvaderFormation Instance;

    [Header("Invasores (arraste TODOS os invasores já posicionados na cena aqui)")]
    public List<GameObject> invaders = new List<GameObject>();

    [Header("Movimento (opcional - deixe desmarcado para ficarem parados)")]
    public bool invadersMoveAcrossScreen = false;
    public float stepDistance = 0.3f;
    public int stepsBeforeTurn = 10;
    public float moveInterval = 0.5f;
    public float dropDistance = 0.5f;
    public float speedIncreasePerInvaderLost = 0.02f;

    [Header("Tiro dos Invasores")]
    [Tooltip("Objeto da própria cena, DESATIVADO, usado como molde para o Instantiate. Não é um prefab.")]
    public GameObject invaderMissileTemplate;
    public float minShootInterval = 1.5f;
    public float maxShootInterval = 4f;

    [Header("Nave Chefe (30~50s)")]
    [Tooltip("Objeto da própria cena, DESATIVADO, usado como molde para o Instantiate. Não é um prefab.")]
    public GameObject bossTemplate;
    public float minBossInterval = 30f;
    public float maxBossInterval = 50f;
    public Vector2 bossSpawnPosition = new Vector2(-5.5f, 4.8f);

    private int initialInvaderCount;
    private int direction = 1;
    private int stepsTaken = 0;
    private float moveTimer = 0f;
    private float shootTimer = 0f;
    private float bossTimer = 0f;
    private float nextShootInterval;
    private float nextBossInterval;
    private bool isActive = false;
    private bool gameOverTriggered = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Não instancia nada: os invasores já existem na cena, só limpamos
        // possíveis entradas vazias da lista arrastada no Inspector.
        invaders.RemoveAll(inv => inv == null);
        initialInvaderCount = invaders.Count;

        nextShootInterval = Random.Range(minShootInterval, maxShootInterval);
        nextBossInterval = Random.Range(minBossInterval, maxBossInterval);
    }

    // Chamado pelo StartButton
    public void StartGame()
    {
        isActive = true;
    }

    public bool IsActive
    {
        get { return isActive; }
    }

    void Update()
    {
        if (!isActive || gameOverTriggered) return;

        invaders.RemoveAll(inv => inv == null);

        if (invaders.Count == 0) return; // LevelManager detecta a vitória

        if (invadersMoveAcrossScreen)
        {
            MoveFormation();
        }

        HandleShooting();
        HandleBoss();
    }

    void MoveFormation()
    {
        moveTimer += Time.deltaTime;

        int destroyedCount = Mathf.Max(0, initialInvaderCount - invaders.Count);
        float currentInterval = Mathf.Max(0.08f, moveInterval - (speedIncreasePerInvaderLost * destroyedCount));

        if (moveTimer < currentInterval) return;
        moveTimer = 0f;

        bool shouldTurn = stepsTaken >= stepsBeforeTurn;

        if (shouldTurn)
        {
            foreach (GameObject invader in invaders)
            {
                if (invader == null) continue;

                Vector3 pos = invader.transform.position;
                pos.y -= dropDistance;
                invader.transform.position = pos;

                CheckReachedBottom(pos);
            }

            direction *= -1;
            stepsTaken = 0;
        }
        else
        {
            foreach (GameObject invader in invaders)
            {
                if (invader == null) continue;

                Vector3 pos = invader.transform.position;
                pos.x += direction * stepDistance;
                invader.transform.position = pos;
            }

            stepsTaken++;
        }
    }

    void CheckReachedBottom(Vector3 pos)
    {
        if (gameOverTriggered) return;

        GameObject bottomWall = GameObject.FindGameObjectWithTag("BottomWall");
        if (bottomWall == null) return;

        float bottomEdge = bottomWall.transform.position.y + (bottomWall.transform.localScale.y / 2);

        if (pos.y <= bottomEdge + 0.5f)
        {
            gameOverTriggered = true;

            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.TriggerGameOver();
            }
        }
    }

    void HandleShooting()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer < nextShootInterval) return;

        shootTimer = 0f;
        nextShootInterval = Random.Range(minShootInterval, maxShootInterval);

        if (invaders.Count == 0 || invaderMissileTemplate == null) return;

        GameObject shooter = invaders[Random.Range(0, invaders.Count)];
        if (shooter == null) return;

        GameObject missile = Instantiate(invaderMissileTemplate, shooter.transform.position, Quaternion.identity);
        missile.SetActive(true); // o molde fica desativado na cena, então ativamos a cópia
    }

    void HandleBoss()
    {
        bossTimer += Time.deltaTime;
        if (bossTimer < nextBossInterval) return;

        bossTimer = 0f;
        nextBossInterval = Random.Range(minBossInterval, maxBossInterval);

        if (bossTemplate == null) return;

        GameObject boss = Instantiate(bossTemplate, bossSpawnPosition, Quaternion.identity);
        boss.SetActive(true); // o molde fica desativado na cena, então ativamos a cópia
    }

    public int GetRemainingInvaders()
    {
        invaders.RemoveAll(inv => inv == null);
        return invaders.Count;
    }
}