using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public TextDisplay bottomBar;
    public GameObject spritesMenu;

    public void SaveData()
    {
        ES3.Save<int>("sentenceIndex", bottomBar.GetSentenceIdex());
        ES3.Save<GameScene>("scene", bottomBar.gameControl.currentScene);
    }
    public void LoadData()
    {
        int index = ES3.Load<int>("sentenceIndex");
        GameScene scene = ES3.Load<GameScene>("scene");
        bottomBar.LoadScene(scene, index);
    }
}
