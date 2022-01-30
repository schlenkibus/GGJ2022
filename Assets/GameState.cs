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

    //register
    public bool register_activated;
    public int register_level;
    public int register_timer;
    public int register_itemsNeeded;
    public int register_itemsStocked;

    public float playerMoney;
    public float bossMoney;

    public static GameState CreateFromJSON(string jsonString)
    {
        Debug.Log(jsonString);
        return JsonUtility.FromJson<GameState>(jsonString);
    }
    //{"activated": false, "level": 1, "timer": 0, "itemsNeeded": 10, "itemsStocked": 0, "playerMoney": 0, "bossMoney": 0}
}
