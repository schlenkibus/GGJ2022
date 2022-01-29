using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShelfTrigger : MonoBehaviour
{
    public UnityEvent onItemStocked;
    private List<GameObject> itemsSeen = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item" && !itemsSeen.Contains(other.gameObject))
        {
            itemsSeen.Add(other.gameObject);
            onItemStocked.Invoke();
        }
    }
}
