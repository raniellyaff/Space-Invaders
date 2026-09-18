using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PresentationManager : MonoBehaviour
{
    public float displayTime = 2.5f; // Tempo que a tela fica visível
    public TextMeshProUGUI presentationText;
    public string levelText = "Boa sorte!";
    
    void Start()
    {
        if (presentationText != null)
        {
            presentationText.text = levelText;
        }
        
        // Carrega a Scene1 após o tempo definido
        Invoke("LoadGame", displayTime);
    }
    
    void LoadGame()
    {
        SceneManager.LoadScene("Scene1");
    }
}