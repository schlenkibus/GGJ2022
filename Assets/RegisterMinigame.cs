using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterMinigame : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMesh timer;
    public TextMesh itemsNeeded;
    public TextMesh itemsDone;
    public TextMesh level;
    public AudioSource tickPlayer;
    public Light redLight;
    public AudioSource beep;

    private bool isActive = false;

    public GameObject[] items;
    public Transform spawnPoint;

    private int idx = 0;
    private List<GameObject> createdObjects = new List<GameObject>();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator spawnItems(int numItems)
    {
        Debug.Log("I am spawing Stuff");

        float timePerItem = 60 / numItems;
        for(int i = 0; i < numItems; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.3f, timePerItem));
            Debug.Log("I am spawing Stuff");
            Vector3 pos = spawnPoint.position;
            createdObjects.Add(Instantiate(items[idx], pos, new Quaternion(0, 0, 0, 0), null));
            idx += 1;
            idx %= items.Length;
        }
    }

    public void onActiveStatusChanged(bool status)
    {
        if (status == true && isActive == false)
        {
            StartCoroutine(spawnItems(itemsNeededAmt));
        }

        isActive = status;
    }

    private IEnumerator turnLightOff()
    {
        yield return new WaitForSeconds(0.2f);
        redLight.enabled = false;
    }

    public void onItemScanned()
    {
        Debug.Log("Item Scanned on Client Side!");
        redLight.enabled = true;
        beep.Play();
        int oldItems = int.Parse(itemsDone.text);
        oldItems++;
        itemsDone.text = oldItems.ToString();
        StartCoroutine(turnLightOff());
    }

    public void setLevel(int l)
    {
        if (l == -1)
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
        if (t < oldTime)
        {
            tickPlayer.Play();
        }
        timer.text = t.ToString();
    }

    private int itemsNeededAmt = 15;

    public void setItemsNeeded(int needed)
    {
        itemsNeededAmt = needed;
        itemsNeeded.text = needed.ToString();
    }

    public void setItemsDone(int done)
    {
        itemsDone.text = done.ToString();
    }
}
