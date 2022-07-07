using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    public TextMeshProUGUI barText;
    public TextMeshProUGUI speakerNameText;

    public StoryScene currentScene;
    private int sentenceNumber = -1;
    private State state = State.COMPLETED;
    private enum State
    {
        PLAYING, 
        COMPLETED
    }
    void Start()
    {
        StartCoroutine(TypeText(currentScene.sentences[++sentenceNumber].text));
        speakerNameText.text = currentScene.sentences[sentenceNumber].speaker.speakerName;
        speakerNameText.color = currentScene.sentences[sentenceNumber].speaker.textColor;
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
}
