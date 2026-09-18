using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<ScoreManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("ScoreManager");
                    _instance = go.AddComponent<ScoreManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    private int score = 0;
    private int lives = 3;
    private int maxLives = 5;

    [Header("Configurações")]
    public int initialLives = 3;

    [Header("Game Over - UI")]
    public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI gameOverScoreText;

    private bool isGameOver = false;
    private bool isInitialized = false;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            ResetScore();
        }

        FindAndConnectUI();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAndConnectUI();

        if (scene.name == "Scene1" || scene.name == "Scene2")
        {
            lives = initialLives;
            isGameOver = false;
            Time.timeScale = 1f;

            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);
        }
    }

    void FindAndConnectUI()
    {
        if (gameOverPanel == null)
        {
            GameObject panel = GameObject.Find("GameOverPanel");
            if (panel != null)
                gameOverPanel = panel;
        }

        if (gameOverScoreText == null && gameOverPanel != null)
        {
            foreach (var t in gameOverPanel.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
            {
                if (t.name == "GameOverScoreText" || t.name == "FinalScoreText" || t.name.Contains("Score"))
                {
                    gameOverScoreText = t;
                    break;
                }
            }
        }
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;
        score += points;
    }

    public void LoseLife()
    {
        if (isGameOver) return;

        lives--;

        if (lives <= 0)
        {
            lives = 0;
            GameOver();
        }
    }

    public void AddLife()
    {
        if (isGameOver) return;
        if (lives < maxLives) lives++;
    }

    public int GetLives() { return lives; }
    public int GetScore() { return score; }

    public void ResetScore()
    {
        score = 0;
        lives = initialLives;
        isGameOver = false;
        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void ResetLivesForNewLevel()
    {
        lives = initialLives;
        isGameOver = false;
        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Derrota");
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        GameOver();
    }

    public void KillPlayerInstant()
    {
        if (isGameOver) return;

        lives = 0;
        GameOver();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        ResetScore();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        SceneManager.LoadScene("Scene1");
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        ResetScore();
        SceneManager.LoadScene("Inicial");
    }
}