using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RegisterTrigger : MonoBehaviour
{
    public UnityEvent onItemScanned;
    private List<Collider> seen = new List<Collider>();
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
        if (other.gameObject.tag == "Item" && !seen.Contains(other))
        {
            seen.Add(other);
            onItemScanned.Invoke();
        }
    }
}
