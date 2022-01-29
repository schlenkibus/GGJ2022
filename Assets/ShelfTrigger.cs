using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShelfTrigger : MonoBehaviour
{
    public UnityEvent onItemStocked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
    }

    void onTriggerEntered(Collider other)
    {
        if(other.gameObject.tag == "Item")
        {
            onItemStocked.Invoke();
        }
    }
}