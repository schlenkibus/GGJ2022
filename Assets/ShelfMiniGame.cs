using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfMiniGame : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMesh timer;
    public TextMesh itemsNeeded;
    public TextMesh itemsDone;
    public TextMesh level;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onItemAddedToShelf()
    {
        int oldItems = int.Parse(itemsDone.text);
        oldItems++;
        itemsDone.text = oldItems.ToString();
    }

    public void setLevel(int l)
    {
        Debug.Log(l);
        level.text = l.ToString();
    }

    public void setTimer(int t)
    {
        Debug.Log(t);
        timer.text = t.ToString();
    }

    public void setItemsNeeded(int needed)
    {
        Debug.Log(needed);
        itemsNeeded.text = needed.ToString();
    }

    public void setItemsDone(int done)
    {
        Debug.Log(done);
        itemsDone.text = done.ToString();
    }
}
