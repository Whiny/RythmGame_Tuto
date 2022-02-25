using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    AudioSource audioSource;
    NoteManager noteManager;
    CenterFrame centerFrame;
    Result result;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent("AudioSource") as AudioSource;
        noteManager = FindObjectOfType<NoteManager>();
        result = FindObjectOfType<Result>();
        centerFrame = FindObjectOfType<CenterFrame>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            audioSource.Play();
            PlayerController.s_canPresskey = false;
            noteManager.RemoveNote();
            result.ShowResult();
            centerFrame.myAudio.Stop();
        }
    }
}
