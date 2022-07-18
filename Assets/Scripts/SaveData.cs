using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData
{
    public static bool load;
    public static void SetLoad()
    {
        load = true;
    }
    public static void SetNewGame()
    {
        load = false;
    }
}
