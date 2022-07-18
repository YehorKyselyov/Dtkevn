using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    public TextMeshProUGUI barText;
    public TextMeshProUGUI speakerNameText;

    public GameControl gameControl;

    private StoryScene currentScene;
    public int sentenceNumber = -1;
    private State state = State.COMPLETED;

    public Dictionary<Speaker, SpriteController> sprites;
    public GameObject spritesPrefab;

    private Animator animator;
    private bool isHidden = false;

    private Coroutine typingCoroutine;
    private float speedFactor = 1f;
    private enum State
    {
        PLAYING,
        SPEED_UP,
        COMPLETED
    }
    private void Start()
    {
        sprites = new Dictionary<Speaker, SpriteController>();
        animator = GetComponent<Animator>();
    }
    public void Hide()
    {
        if (!isHidden)
        {
            animator.SetTrigger("Hide");
            isHidden = true;
        }
    }
    public void Show()
    {
        animator.SetTrigger("Show");
        isHidden = false;
    }
    public void ClearText()
    {
        barText.text = "";
    }
    public void ClearSpeaker()
    {
        speakerNameText.text = "";
    }
    public void PlayScene(StoryScene scene)
    {
        gameControl.SetStateANIMATE();
        currentScene = scene;
        if (scene.playAnimation) sprites = new Dictionary<Speaker, SpriteController>();
        sentenceNumber = -1;
        PlayNextSentence();
    }
    public void LoadScene(GameScene scene, int sentenceIndex)
    {
        DeleteSprites();
        if(currentScene != scene) currentScene = scene as StoryScene;
        if(gameControl.currentScene != scene) gameControl.currentScene = scene;
        sprites = new Dictionary<Speaker, SpriteController>();
        sentenceNumber = sentenceIndex - 1;
        gameControl.backgroundController.SwitchImage((scene as StoryScene).background);
        RecreateSprites();
        PlayNextSentence();
    }
    private void RecreateSprites()
    {
        for (int i = 0; i < sentenceNumber + 1; i++)
        {
            List<StoryScene.Sentence.Action> actions = currentScene.sentences[i].actions;
            for (int j = 0; j < actions.Count; j++)
            {
                ActSpeaker(actions[j]);
            }
        }
    }
    public void PlayNextSentence(bool isAnimated = true)
    {
        speedFactor = 1f;
        typingCoroutine = StartCoroutine(TypeText(currentScene.sentences[++sentenceNumber].text));
        speakerNameText.text = currentScene.sentences[sentenceNumber].speaker.speakerName;
        speakerNameText.color = currentScene.sentences[sentenceNumber].speaker.textColor;
        ActSpeakers(isAnimated);
    }
    public bool IsCompleted()
    {
        return state == State.COMPLETED;
    }
    public void SpeedUp()
    {
        state = State.SPEED_UP;
        speedFactor = 0.15F;
    }

    public bool IsLastSentence()
    {
        return sentenceNumber + 1 == currentScene.sentences.Count;
    }
    private IEnumerator TypeText(string text)
    {
        barText.text = "";
        state = State.PLAYING;
        int wordIndex = 0;
        while(state != State.COMPLETED)
        {
            barText.text += text[wordIndex];
            yield return new WaitForSeconds(speedFactor * 0.05f);
            if (++wordIndex == text.Length)
            {
                state = State.COMPLETED;
                break;
            }
        }
        gameControl.SetStateIDLE();
    }
    private void ActSpeakers(bool isAnimated = true)
    {
        List<StoryScene.Sentence.Action> actions = currentScene.sentences[sentenceNumber].actions;
        for (int i = 0; i < actions.Count; i++)
        {
            ActSpeaker(actions[i],isAnimated);
        }
    }
    private void ActSpeaker(StoryScene.Sentence.Action action, bool isAnimated = true)
    {
        SpriteController controller;
        if (!sprites.ContainsKey(action.speaker))
        {
            controller = Instantiate(action.speaker.prefab.gameObject, spritesPrefab.transform)
                .GetComponent<SpriteController>();
            sprites.Add(action.speaker, controller);
        }
        else
        {
            controller = sprites[action.speaker];
        }
        switch (action.actionType)
        {
            case StoryScene.Sentence.Action.Type.APPEAR:                
                controller.Setup(action.speaker.sprites[action.spriteIndex]);
                controller.Show(action.coords, isAnimated);
                return;
            case StoryScene.Sentence.Action.Type.MOVE:                
                controller.Move(action.coords, action.moveSpeed, isAnimated);             
                break;
            case StoryScene.Sentence.Action.Type.DISAPPEAR:
                controller.Hide(isAnimated);
                break;
            case StoryScene.Sentence.Action.Type.SWITCH:
                controller.SwitchSprite(action.speaker.sprites[action.spriteIndex], isAnimated);
                break;
            case StoryScene.Sentence.Action.Type.NONE:
                controller = sprites[action.speaker];
                break;
        }
        
    }
    private SpriteController GetSpriteController(Speaker speaker)
    {
        SpriteController controller = null;
        sprites.TryGetValue(speaker, out controller);
        return controller;
    }
    public void HideSprites()
    {
        for (int i = 0; i < currentScene.sentences.Count; i++)
        {
            for (int j = 0; j < currentScene.sentences[i].actions.Count; j++)
            {
                SpriteController controller = sprites[currentScene.sentences[i].actions[j].speaker];
                CanvasGroup canvas = controller.gameObject.GetComponent<CanvasGroup>(); 
                if (canvas.alpha != 0)
                {
                    controller.Hide();
                }
            }            
        }
    }
 
    public void DeleteSprites()
    {
        for (var j = spritesPrefab.transform.childCount - 1; j >= 0; j--)
        {
            Object.Destroy(spritesPrefab.transform.GetChild(j).gameObject);
        }
    }
    public int GetSentenceIdex()
    {
        return sentenceNumber;
    }
    public void GoBack()
    {
        DeleteSprites();
        sprites.Clear();
        sentenceNumber -= 2;
        StopTyping();
        RecreateSprites();
        PlayNextSentence(false);
    }
    public void StopTyping()
    {
        state = State.COMPLETED;
        StopCoroutine(typingCoroutine);
    }
    
}
