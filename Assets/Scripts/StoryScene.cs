using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story Scene", menuName = "Custom/Story Scene")]
[System.Serializable]

public class StoryScene: GameScene
{
    public List<Sentence> sentences;
    public Sprite background;
    public GameScene nextScene;

    public bool playAnimation;

    [System.Serializable]
    public struct Sentence
    {
        public string text;
        public Speaker speaker;
        public List<Action> actions;

        public AudioClip music;
        public AudioClip sound;

        [System.Serializable]
        public struct Action
        {
            public Speaker speaker;
            public int spriteIndex;
            public Type actionType;
            public Vector2 coords;
            public float moveSpeed;

            [System.Serializable]
            public enum Type
            {
                NONE,
                APPEAR,
                MOVE,
                SWITCH,
                DISAPPEAR
            }
        }
    }
}
public class GameScene : ScriptableObject
{

}
