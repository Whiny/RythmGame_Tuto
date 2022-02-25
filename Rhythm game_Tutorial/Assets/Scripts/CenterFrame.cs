using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterFrame : MonoBehaviour
{
    public AudioSource myAudio;
    bool musicStart = false;
    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(!musicStart)
        {
            if(other.CompareTag("Note"))
            {
                myAudio.Play();
                musicStart = true;
            }
        }
    }
}
