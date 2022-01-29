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
    public AudioSource tickPlayer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onItemAddedToShelf()
    {
        Debug.Log("Item Added on Client Side!");
        int oldItems = int.Parse(itemsDone.text);
        oldItems++;
        itemsDone.text = oldItems.ToString();
    }

    public void setLevel(int l)
    {
        level.text = l.ToString();
    }

    public void setTimer(int t)
    {
        int oldTime = int.Parse(timer.text);
        if(t < oldTime)
        {
            tickPlayer.Play();
        }
        timer.text = t.ToString();
    }

    public void setItemsNeeded(int needed)
    {
        itemsNeeded.text = needed.ToString();
    }

    public void setItemsDone(int done)
    {
        itemsDone.text = done.ToString();
    }
}
