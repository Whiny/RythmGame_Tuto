using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject goUI = null;
    [SerializeField] UnityEngine.UI.Text[] txtCount = null;
    [SerializeField] UnityEngine.UI.Text txtCoin = null;
    [SerializeField] UnityEngine.UI.Text txtScore = null;
    [SerializeField] UnityEngine.UI.Text txtMaxCombo = null;

    ScoreManager scoreManager;
    ComboManager comboManager;
    TimingManager timingManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        comboManager = FindObjectOfType<ComboManager>();
        timingManager = FindObjectOfType<TimingManager>();
    }

    public void ShowResult()
    {
        goUI.SetActive(true);
        
        for(int i = 0;i<txtCoin.text.Length; i++)
        {
            txtCount[i].text = "0";
        }

        txtCoin.text = "0";
        txtScore.text = "0";
        txtMaxCombo.text = "0";

        int[] t_judgementRecord = timingManager.GetJedgementRecord();
        int t_currentScore = scoreManager.GetCurrentScore();
        int t_maxCombo = comboManager.GetMaxCombo();
        int t_coin = t_currentScore / 50;

        for(int i = 0;i<txtCount.Length; i++)
            txtCount[i].text = string.Format("{0:#,##0}",t_judgementRecord[i]);

        txtScore.text = string.Format("{0:#,##0}",t_currentScore);
        txtMaxCombo.text = string.Format("{0:#,##0}",t_maxCombo);
        txtCoin.text = string.Format("{0:#,##0}",t_coin);
    }
}
