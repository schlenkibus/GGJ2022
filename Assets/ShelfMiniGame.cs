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
    public AudioSource alarmPlayer;

    private bool isActive;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onActiveStatusChanged(bool status)
    {
        if(status == true && isActive == false)
        {
            alarmPlayer.Play();
        }

        isActive = status;
    }

    public void onItemAddedToShelf()
    {
        if(isActive)
        {
            int oldItems = int.Parse(itemsDone.text);
            oldItems++;
            itemsDone.text = oldItems.ToString();
        }
    }

    public void setLevel(int l)
    {
        if(l == -1)
        {
            level.text = "Task Failed!";
        }
        else
        {
            level.text = l.ToString();
        }
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
