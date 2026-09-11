using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void OnStartClick()
    {
        if (InvaderFormation.Instance != null)
        {
            InvaderFormation.Instance.StartGame();
        }

        gameObject.SetActive(false);
    }
}