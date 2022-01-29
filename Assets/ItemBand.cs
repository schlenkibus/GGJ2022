using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBand : MonoBehaviour
{
    public Vector3 velocity;
    public Material mat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Item")
        {
            Rigidbody orb = other.attachedRigidbody;
            if (orb != null)
            {
                Vector2 t = mat.mainTextureScale;
                t.y -= (0.05f * Time.deltaTime);
                mat.mainTextureScale = t;
                orb.velocity = velocity;
            }
        }
    }
}
