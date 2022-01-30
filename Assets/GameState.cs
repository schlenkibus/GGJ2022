using UnityEngine;

[System.Serializable]
public class GameState
{
    public bool activated;
    public int level;
    public int timer;
    public int itemsNeeded;
    public int itemsStocked;

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
}
