using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    public void OnNextLevelClick()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.NextLevel();
        }
    }
}