using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();

    int[] judgementRecord = new int[5];

    [SerializeField] Transform Center = null;
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBoxs = null;

    EffectManager effect;
    ScoreManager scoreManager;
    ComboManager comboManager;
    StageManager stageManager;
    PlayerController playerController;
    StatusManager statusManager;
    // Start is called before the first frame update
    void Start()
    {
        effect = FindObjectOfType<EffectManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        comboManager = FindObjectOfType<ComboManager>();
        stageManager = FindObjectOfType<StageManager>();
        playerController = FindObjectOfType<PlayerController>();
        statusManager = FindObjectOfType<StatusManager>();

        timingBoxs = new Vector2[timingRect.Length];

        for(int i = 0;i<timingRect.Length;i++)
        {
            timingBoxs[i].Set(Center.localPosition.x - timingRect[i].rect.width /2,
                              Center.localPosition.x + timingRect[i].rect.width /2);
        }
    }

    public bool CheckTiming()
    {
        for(int i=0;i<boxNoteList.Count;i++)
        {
            float t_notePosX = boxNoteList[i].transform.localPosition.x;

            for(int x=0;x<timingBoxs.Length;x++)
            {
                if(timingBoxs[x].x<=t_notePosX && t_notePosX<=timingBoxs[x].y)
                {
                    //노트제거 (오브젝트 풀링)
                    Note note = boxNoteList[i].GetComponent("Note") as Note;
                    note.HideNote();
                    boxNoteList.RemoveAt(i);

                    //이펙트
                    if(x<timingBoxs.Length-1)
                        effect.NoteHitEffect();
                    effect.JudgementEffect(x);

                    //굿 이하 일 때 콤보 리셋
                    if(x>1)
                        comboManager.ResetCombo();
                    
                    if(!CheckCanNextPlate()) //틀린 방향으로
                    {
                        effect.JudgementEffect(5);
                        comboManager.ResetCombo();
                    }
                    else //옳은 방향으로
                    {
                        scoreManager.IncreaseScore(x);//점수 증가
                        stageManager.ShowNextPlate();//플레이트 생성
                        judgementRecord[x]++; //판정 기록
                        statusManager.CheckShield();
                    }
                
                    return true;
                }
            }
        }
        comboManager.ResetCombo();
        effect.JudgementEffect(timingBoxs.Length);
        MissRecord();
        return false;
    }

    
    bool CheckCanNextPlate()
    {
        if(Physics.Raycast(playerController.destPos, Vector3.down, out RaycastHit t_hitInfo, 1.1f))
        {
            if(t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate t_plate = t_hitInfo.transform.GetComponent<BasicPlate>();
                if(t_plate.flag)
                {
                    t_plate.flag = false;
                    return true;
                }
            }
        }
        return false;
    }

    public int[] GetJedgementRecord()
    {
        return judgementRecord;
    }

    public void MissRecord()
    {
        judgementRecord[4]++;
        statusManager.ResetShieldCombo();
    }
}
