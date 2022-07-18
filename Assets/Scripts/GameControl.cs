using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public GameScene currentScene;
    public TextDisplay bottomBar;
    public BackgroundController backgroundController;
    public ChoiceController choiceController;
    public AudioController audioController;
    public PauseController pauseController;
    public SaveManager saveManager;


    private State state = State.IDLE;
    private enum State
    {
        IDLE,
        ANIMATE,
        DELETESPRITES,
        PAUSE,
        CHOOSE
    }
    void Start()
    {

        if (!SaveData.load)
        {
            if (currentScene is StoryScene)
            {
                StoryScene storyScene = currentScene as StoryScene;
                bottomBar.PlayScene(storyScene);
                backgroundController.SetImage(storyScene.background);
                PlayAudio(storyScene.sentences[0]);
            }
        }
        else
        {
            saveManager.LoadData();
            PlayAudio((currentScene as StoryScene).sentences[0]);
        }
    }

    void Update()
    {
        if(state == State.IDLE)
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            {
                if (bottomBar.IsCompleted())
                {
                    if (bottomBar.IsLastSentence())
                    {
                        PlayScene((currentScene as StoryScene).nextScene);
                    }
                    else
                    {
                        bottomBar.PlayNextSentence();
                        PlayAudio((currentScene as StoryScene).sentences[bottomBar.GetSentenceIdex()]);
                    }
                }
                else
                {
                    bottomBar.SpeedUp();
                }
            }
            if(bottomBar.IsCompleted())
            {
                if (Input.GetMouseButton(1))
                {

                    state = State.PAUSE;
                    pauseController.SetPause();
                }
                
            }
            
        }
        if (state == State.DELETESPRITES) bottomBar.DeleteSprites();
        if (Input.GetKeyDown(KeyCode.Backspace) && bottomBar.sentenceNumber != 0)
        {
            bottomBar.GoBack();
        }

    }

    public void PlayScene(GameScene scene)
    {
        StartCoroutine(SwitchScene(scene));
    }
    private IEnumerator SwitchScene(GameScene scene)
    {
        state = State.ANIMATE;       
        currentScene = scene;
        bottomBar.Hide();
        yield return new WaitForSeconds(1f);
        if (scene is StoryScene && (scene as StoryScene).playAnimation)
        {
            bottomBar.HideSprites();
            yield return new WaitForSeconds(1f);
            StoryScene storyScene = scene as StoryScene;
            backgroundController.SwitchImage(storyScene.background);
            PlayAudio(storyScene.sentences[0]);
            yield return new WaitForSeconds(1f);
            bottomBar.ClearText();
            bottomBar.ClearSpeaker();
            state = State.DELETESPRITES;
            bottomBar.Show();
            yield return new WaitForSeconds(1f);
            bottomBar.PlayScene(storyScene);
            state = State.IDLE;
        }
        else if (scene is StoryScene && !(scene as StoryScene).playAnimation)
        {
            bottomBar.ClearText();
            bottomBar.ClearSpeaker();
            bottomBar.Show();
            yield return new WaitForSeconds(1f);
            bottomBar.PlayScene(scene as StoryScene);
            state = State.IDLE;
        }
        else if (scene is ChooseScene)
        {
            state = State.CHOOSE;
            choiceController.SetupChoose(scene as ChooseScene);
        }
    }

    public void PlayAudio(StoryScene.Sentence sentence)
    {
        audioController.PlayAudio(sentence.music, sentence.sound);
    }
    
    public void SetStateIDLE()
    {
        state = State.IDLE;
    }
    public void SetStateANIMATE()
    {
        state = State.ANIMATE;
    }
}