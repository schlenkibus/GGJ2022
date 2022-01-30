using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCounters : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMesh playerM;
    public TextMesh bossM;

    public int foo;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPlayerMoneyChanged(float m)
    {
        playerM.text = "You made: " + m.ToString() + "$";
    }

    public void onBossMoneyChanged(float m)
    {
        bossM.text = "Your Boss made: " + m.ToString() + "$";
    }
}
