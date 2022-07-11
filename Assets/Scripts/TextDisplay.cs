using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    public TextMeshProUGUI barText;
    public TextMeshProUGUI speakerNameText;

    private StoryScene currentScene;
    private int sentenceNumber = -1;
    private State state = State.COMPLETED;

    public Dictionary<Speaker, SpriteController> sprites;
    public GameObject spritesPrefab;

    private Animator animator;
    private bool isHidden = false;
    private enum State
    {
        PLAYING, 
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
    public void PlayScene(StoryScene scene)
    {
        currentScene = scene;
        sentenceNumber = -1;
        PlayNextSentence();
    }
    public void PlayNextSentence()
    {
        StartCoroutine(TypeText(currentScene.sentences[++sentenceNumber].text));
        speakerNameText.text = currentScene.sentences[sentenceNumber].speaker.speakerName;
        speakerNameText.color = currentScene.sentences[sentenceNumber].speaker.textColor;
        ActSpeakers();
    }
    public bool IsCompleted()
    {
        return state == State.COMPLETED;
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
            yield return new WaitForSeconds(0.05f);
            if (++wordIndex == text.Length)
            {
                state = State.COMPLETED;
                break;
            }
        }
    }
    private void ActSpeakers()
    {
        List<StoryScene.Sentence.Action> actions = currentScene.sentences[sentenceNumber].actions;
        for (int i = 0; i < actions.Count; i++)
        {
            ActSpeaker(actions[i]);
        }
    }
    private void ActSpeaker(StoryScene.Sentence.Action action)
    {
        SpriteController controller = null;
        switch (action.actionType)
        {
            case StoryScene.Sentence.Action.Type.APPEAR:
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
                controller.Setup(action.speaker.sprites[action.spriteIndex]);
                controller.Show(action.coords);
                return;
            case StoryScene.Sentence.Action.Type.MOVE:
                if (sprites.ContainsKey(action.speaker))
                {
                    controller = sprites[action.speaker];
                    controller.Move(action.coords, action.moveSpeed);
                }
                break;
            case StoryScene.Sentence.Action.Type.DISAPPEAR:
                if (sprites.ContainsKey(action.speaker))
                {
                    controller = sprites[action.speaker];
                    controller.Hide();
                }
                break;
            case StoryScene.Sentence.Action.Type.SWITCH:
                if (sprites.ContainsKey(action.speaker))
                {
                    controller = sprites[action.speaker];
                    controller.SwitchSprite(action.speaker.sprites[action.spriteIndex]);
                }
                break;
            case StoryScene.Sentence.Action.Type.NONE:
                if (sprites.ContainsKey(action.speaker))
                {
                    controller = sprites[action.speaker];
                }
                break;
        }
        if (controller != null)
        {
            controller.SwitchSprite(action.speaker.sprites[action.spriteIndex]);
        }
    }
}
