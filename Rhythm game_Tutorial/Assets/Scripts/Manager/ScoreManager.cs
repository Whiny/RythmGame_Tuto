using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]UnityEngine.UI.Text txtScore = null;
    [SerializeField] int increaseScore = 10;
    int currentScore = 0;
    [SerializeField] float[] weight = null;
    [SerializeField] int comboBonusScore = 10;

    Animator animator;
    string animScoreUp = "ScoreUp";

    ComboManager comboManager;

    // Start is called before the first frame update
    void Start()
    {
        comboManager = FindObjectOfType<ComboManager>();
        animator = GetComponent("Animator") as Animator;
        currentScore = 0;
        txtScore.text = "0";
    }

    public void IncreaseScore(int p_JudgementState)
    {
        //콤보 계싼
        comboManager.InmcreaseCombo();

        int t_currentCombo = comboManager.GetCurrentCombo();
        int t_bonusComboScore = (t_currentCombo/10)*comboBonusScore;

        //가중치
        int t_increaseScore = increaseScore + t_bonusComboScore;
        t_increaseScore = (int)(t_increaseScore * weight[p_JudgementState]);
        currentScore+=t_increaseScore;
        txtScore.text = string.Format("{0:#,##0}",currentScore);

        //애니 실행
        animator.SetTrigger(animScoreUp);

    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}
