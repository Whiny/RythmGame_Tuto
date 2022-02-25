using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public float bpm = 0;
    double currentTime = 0d;

    bool noteActive = true;
    [SerializeField] Transform tfNoteAppear = null;

    TimingManager timingManager;
    EffectManager effectManager;
    ComboManager comboManager;

    // Update is called once per frame

    void Start() 
    {
        timingManager = GetComponent<TimingManager>();
        effectManager = FindObjectOfType<EffectManager>();
        comboManager = FindObjectOfType<ComboManager>();
    }
    void Update()
    {
        if(noteActive)
        {
            currentTime += Time.deltaTime;
            if(currentTime>=60d / bpm)
            {
                GameObject t_note = ObjectPool.instance.noteQueue.Dequeue();
                t_note.transform.position=tfNoteAppear.position;
                t_note.SetActive(true);
                currentTime -= 60/bpm;

                timingManager.boxNoteList.Add(t_note);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Note"))
        {
            Note note = other.GetComponent("Note") as Note;
            if(note.GetNoteFlag())
            {
                timingManager.MissRecord(); 
                effectManager.JudgementEffect(4);
                comboManager.ResetCombo();
            }
            timingManager.boxNoteList.Remove(other.gameObject);


            ObjectPool.instance.noteQueue.Enqueue(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    public void RemoveNote()
    {
        noteActive = false;
        for(int i=0;i<timingManager.boxNoteList.Count;i++)
        {
            timingManager.boxNoteList[i].SetActive(false);
            ObjectPool.instance.noteQueue.Enqueue(timingManager.boxNoteList[i]);
        }
    }
}
