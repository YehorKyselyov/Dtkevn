using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D gameCursor;
    void Start()
    {
        Cursor.SetCursor(gameCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}
