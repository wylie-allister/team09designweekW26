using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    [Header("Win Backgrounds")]
    [SerializeField] private GameObject bunnyWinBG;
    [SerializeField] private GameObject foxWinBG;

    private void Start()
    {
        bunnyWinBG.SetActive(false);
        foxWinBG.SetActive(false);

        // Activate based on winner
        if (GameManager.LastWinner == GameManager.Winner.Bunnies)
        {
            bunnyWinBG.SetActive(true);
        }
        else if (GameManager.LastWinner == GameManager.Winner.Foxes)
        {
            foxWinBG.SetActive(true);
        }
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Dual Monitor Scene");
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Team Select");
    }
}
