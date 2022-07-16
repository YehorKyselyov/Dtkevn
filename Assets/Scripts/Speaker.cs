using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Speaker", menuName = "Custom/Speaker")]
[System.Serializable]
public class Speaker : ScriptableObject
{
    public string speakerName;
    public Color textColor;

    public List<Sprite> sprites;
    public SpriteController prefab;
}