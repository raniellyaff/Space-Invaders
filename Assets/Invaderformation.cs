using UnityEngine;
using System.Collections.Generic;

public class InvaderFormation : MonoBehaviour
{
    public static InvaderFormation Instance;

    [Header("Formação")]
    public GameObject invaderPrefabType1; // linha de cima  - 10 pts
    public GameObject invaderPrefabType2; // linhas do meio - 20 pts
    public GameObject invaderPrefabType3; // linhas de baixo - 30 pts
    public int rows = 5;
    public int columns = 10;
    public float spacingX = 1f;
    public float spacingY = 0.8f;
    public Vector2 startPosition = new Vector2(-4.5f, 4f);

    [Header("Movimento (passo a passo, igual ao PDF)")]
    public float stepDistance = 0.3f;
    public int stepsBeforeTurn = 10; // 10 passos numa direção, 10 na outra
    public float moveInterval = 0.5f;
    public float dropDistance = 0.5f; // desce quando muda de direção
    public float speedIncreasePerInvaderLost = 0.02f;

    [Header("Tiro dos Invasores")]
    public GameObject invaderMissilePrefab;
    public float minShootInterval = 1.5f;
    public float maxShootInterval = 4f;

    [Header("Nave Chefe (30~50s)")]
    public GameObject bossPrefab;
    public float minBossInterval = 30f;
    public float maxBossInterval = 50f;
    public Vector2 bossSpawnPosition = new Vector2(-5.5f, 4.8f);

    private List<GameObject> invaders = new List<GameObject>();
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
        BuildFormation();
        nextShootInterval = Random.Range(minShootInterval, maxShootInterval);
        nextBossInterval = Random.Range(minBossInterval, maxBossInterval);
    }

    void BuildFormation()
    {
        for (int row = 0; row < rows; row++)
        {
            GameObject prefab = invaderPrefabType1;
            if (row == 1 || row == 2) prefab = invaderPrefabType2;
            if (row >= 3) prefab = invaderPrefabType3;

            if (prefab == null) continue;

            for (int col = 0; col < columns; col++)
            {
                Vector2 pos = startPosition + new Vector2(col * spacingX, -row * spacingY);
                GameObject invader = Instantiate(prefab, pos, Quaternion.identity, transform);
                invaders.Add(invader);
            }
        }
    }

    // Chamado pelo StartButton (igual ao ball.StartBall() no Arkanoid)
    public void StartGame()
    {
        isActive = true;
    }

    // Permite que outros scripts (como o player.cs) saibam se o jogo já começou
    public bool IsActive
    {
        get { return isActive; }
    }

    void Update()
    {
        if (!isActive || gameOverTriggered) return;

        invaders.RemoveAll(inv => inv == null);

        if (invaders.Count == 0) return; // LevelManager detecta a vitória

        MoveFormation();
        HandleShooting();
        HandleBoss();
    }

    void MoveFormation()
    {
        moveTimer += Time.deltaTime;

        int destroyedCount = (rows * columns) - invaders.Count;
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

        // Se um invasor tocou a parede inferior, o jogo acaba (regra do PDF)
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

        if (invaders.Count == 0 || invaderMissilePrefab == null) return;

        GameObject shooter = invaders[Random.Range(0, invaders.Count)];
        if (shooter == null) return;

        Instantiate(invaderMissilePrefab, shooter.transform.position, Quaternion.identity);
    }

    void HandleBoss()
    {
        bossTimer += Time.deltaTime;
        if (bossTimer < nextBossInterval) return;

        bossTimer = 0f;
        nextBossInterval = Random.Range(minBossInterval, maxBossInterval);

        if (bossPrefab == null) return;

        Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);
    }

    public int GetRemainingInvaders()
    {
        invaders.RemoveAll(inv => inv == null);
        return invaders.Count;
    }
}