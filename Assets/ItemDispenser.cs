using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemDispenser : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] prefabs;
    public Transform spawnPostion;
    public ParticleSystem ps;
    public AudioSource aus;

    public List<GameObject> spawnedItems;

    public List<AudioClip> clips;
    private int idx = 0;
    private int audioIdx = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void dispenseItem()
    {
        ps.Play();
        aus.clip = clips[audioIdx];
        aus.Play();
        Vector3 pos = spawnPostion.transform.position;

        spawnedItems.Add(Instantiate(prefabs[idx], pos, new Quaternion(0, 0, 0, 0), null));
        idx += 1;
        idx %= prefabs.Length;
        audioIdx += 1;
        audioIdx %= clips.Count;
    }
}
