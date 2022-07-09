using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public StoryScene currentScene;
    public TextDisplay bottomBar;
    public BackgroundController backgroundController;

    private State state = State.IDLE;
    private enum State
    {
        IDLE,
        ANIMATE,
        CHOOSE
    }
    void Start()
    {
        bottomBar.PlayScene(currentScene);
        backgroundController.SetImage(currentScene.background);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (bottomBar.IsCompleted())
            {
                if (state == State.IDLE && bottomBar.IsLastSentence())
                {
                    PlayScene(currentScene.nextScene);
                }
                else bottomBar.PlayNextSentence();
            }


        }
    }
    private void PlayScene(StoryScene scene)
    {
        StartCoroutine(SwitchScene(scene)); 
    }
    private IEnumerator SwitchScene(StoryScene scene)
    {
        state = State.ANIMATE;
        currentScene = scene;
        bottomBar.Hide();
        yield return new WaitForSeconds(1f);
        backgroundController.SwitchImage(scene.background);
        yield return new WaitForSeconds(1f);
        bottomBar.ClearText();
        bottomBar.Show();
        yield return new WaitForSeconds(1f);
        bottomBar.PlayScene(scene);
        state = State.IDLE;
    }
}