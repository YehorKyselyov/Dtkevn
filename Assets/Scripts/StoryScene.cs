using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story Scene", menuName = "Custom/Story Scene")]
[System.Serializable]

public class StoryScene: GameScene
{
    public List<Sentence> sentences;
    public Sprite background;
    public GameScene nextScene;

    [System.Serializable]
    public struct Sentence
    {
        public string text;
        public Speaker speaker;
    }
}
public class GameScene : ScriptableObject
{

}
