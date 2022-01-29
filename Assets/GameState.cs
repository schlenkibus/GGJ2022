using UnityEngine;

[System.Serializable]
public class GameState
{
    //shelf
    public bool activated;
    public int level;
    public int timer;
    public int itemsNeeded;
    public int itemsStocked;

    public static GameState CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameState>(jsonString);
    }
    //{"activated": false, "level": 1, "timer": 0, "itemsNeeded": 10, "itemsStocked": 0}
}
