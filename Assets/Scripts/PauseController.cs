using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameControl gameControl;
    public GameObject bottomBar;
    public GameObject choicePannel;

    public void SetPause()
    {
        bottomBar.SetActive(false);
        choicePannel.SetActive(false);
        gameObject.SetActive(true);
    }
    public void BreakPause()
    {
        bottomBar.SetActive(true);
        choicePannel.SetActive(true);
        gameObject.SetActive(false);
        gameControl.SetStateIDLE();
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
