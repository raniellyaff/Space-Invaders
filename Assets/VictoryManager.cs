using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI titleText;
    
    void Start()
    {
        // Verifica qual cena está carregada
        string sceneName = SceneManager.GetActiveScene().name;
        bool isVictory = sceneName == "Vitoria";
        
        // Atualiza o título
        if (titleText != null)
        {
            if (isVictory)
            {
                titleText.text = "PARABENS!";
                titleText.color = new Color(1f, 0.84f, 0f); // Dourado
            }
            else
            {
                titleText.text = "GAME OVER";
                titleText.color = Color.red;
            }
        }
        
        // Mostra o score final
        if (scoreText != null && ScoreManager.Instance != null)
        {
            scoreText.text = $"Score: {ScoreManager.Instance.GetScore()}";
        }
        
        // Pausa o jogo
        Time.timeScale = 0f;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        
        SceneManager.LoadScene("Scene1");
    }
    
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }
        
        SceneManager.LoadScene("Inicial");
    }
}