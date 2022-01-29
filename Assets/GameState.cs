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
    public float playerMoney;
    public float bossMoney;

    public static GameState CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameState>(jsonString);
    }
    //{"activated": false, "level": 1, "timer": 0, "itemsNeeded": 10, "itemsStocked": 0, "playerMoney": 0, "bossMoney": 0}
}
