using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodJobSound : MonoBehaviour
{
    public AudioSource player;
    public AudioClip[] clips;

    private int idx = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playOneSound()
    {
        player.clip = clips[idx];
        player.Play();
        idx += 1;
        idx %= clips.Length;
    }
}
