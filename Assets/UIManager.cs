using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    
    void Start()
    {
        // Tenta encontrar os textos se não estiverem conectados
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
            if (scoreText != null)
                Debug.Log("✅ ScoreText encontrado automaticamente!");
        }
        
        if (livesText == null)
        {
            livesText = GameObject.Find("LivesText")?.GetComponent<TextMeshProUGUI>();
            if (livesText != null)
                Debug.Log("✅ LivesText encontrado automaticamente!");
        }
        
        UpdateUI();
    }
    
    void Update()
    {
        // Verifica se os textos ainda existem
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        }
        
        if (livesText == null)
        {
            livesText = GameObject.Find("LivesText")?.GetComponent<TextMeshProUGUI>();
        }
        
        UpdateUI();
    }
    
    void UpdateUI()
    {
        if (ScoreManager.Instance == null) return;
        
        if (scoreText != null)
        {
            scoreText.text = $"Score: {ScoreManager.Instance.GetScore()}";
        }
        
        if (livesText != null)
        {
            livesText.text = $"Vidas: {ScoreManager.Instance.GetLives()}";
        }
    }
}