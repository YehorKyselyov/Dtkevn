using UnityEngine;

[CreateAssetMenu(fileName = "Speaker", menuName = "Custom/Speaker")]
[System.Serializable]
public class Speaker : ScriptableObject
{
    public string speakerName;
    public Color textColor;
}
