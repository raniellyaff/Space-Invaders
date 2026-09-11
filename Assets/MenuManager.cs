using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // Vai para a tela de apresentação
        SceneManager.LoadScene("Apresentacao");
    }
    
    public void GoToRules()
    {
        // Vai para a tela de regras
        SceneManager.LoadScene("Regras");
    }
    
    public void GoToMenu()
    {
        // Volta para o menu inicial
        SceneManager.LoadScene("Inicial");
    }
    
    public void GoToPresentation()
    {
        // Vai para a tela de apresentação (se precisar chamar de outro lugar)
        SceneManager.LoadScene("Apresentacao");
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}