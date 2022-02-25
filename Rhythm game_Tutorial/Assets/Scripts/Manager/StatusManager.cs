using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    int maxHP = 3;
    int currentHP = 3;

    int maxShield = 3;
    int currentShield = 0;

    bool isDead = false;
    bool isBlink = false;

    [SerializeField] UnityEngine.UI.Image[] hpImage = null;
    [SerializeField] UnityEngine.UI.Image[] shieldImage = null;

    [SerializeField] int shieldIncreaseCombo = 5;
    int currentShieldCombo = 0;
    [SerializeField] UnityEngine.UI.Image shieldGuage = null;

    [SerializeField] float blickSpeed = 0.1f;
    [SerializeField] int blinkCount = 10;
    int currentBlinkCount = 0;

    Result result;
    NoteManager noteManager;
    [SerializeField] MeshRenderer playerMesh = null;

    void Start() 
    {
        result = FindObjectOfType<Result>();
        noteManager = FindObjectOfType<NoteManager>();
    }

    public void CheckShield()
    {
        currentShieldCombo++;

        if(currentShieldCombo >= shieldIncreaseCombo)
        {
            currentShieldCombo = 0;
            IncreaseShield();
        }

        shieldGuage.fillAmount = (float)currentShieldCombo / shieldIncreaseCombo;
    }

    public void IncreaseShield()
    {
        currentShield++;
        Debug.Log(currentShield);

        if(currentShield>=maxShield)
            currentShield = maxShield;

        SettingShield();
    }

    public void DrecreaseShield(int p_num)
    {
        currentShield -= p_num;

        if(currentShield <=0)
            currentShield = 0;

        SettingShield();
    }

    public void ResetShieldCombo()
    {
        currentShieldCombo = 0;
        shieldGuage.fillAmount = (float)currentShieldCombo / shieldIncreaseCombo;
    }

    public void IncreaseHP(int p_num)
    {
        currentHP += p_num;

        if(currentHP>=maxHP)
            currentHP = maxHP;

        SettingHP();
    }

    public void DecreaseHP(int p_num)
    {
        if(!isBlink)
        {
            if(currentShield >0)
                DrecreaseShield(p_num);
            else
            { 
                currentHP -= p_num;

                if(currentHP <= 0)
                {
                    result.ShowResult();
                    noteManager.RemoveNote();
                }
                else
                    StartCoroutine(BlinkCo());

                SettingHP();
            }
        }
    }

    void SettingHP()
    {
        for(int i=0;i<hpImage.Length;i++)
        {
            if(i<currentHP)
                hpImage[i].gameObject.SetActive(true);
            else
                hpImage[i].gameObject.SetActive(false);
        }
    }

    void SettingShield()
    {
        for(int i=0;i<shieldImage.Length;i++)
        {
            if(i<currentShield)
                shieldImage[i].gameObject.SetActive(true);
            else
                shieldImage[i].gameObject.SetActive(false);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    IEnumerator BlinkCo()
    {
        isBlink = true;
        while(currentBlinkCount<=blinkCount)
        {
            playerMesh.enabled = !playerMesh.enabled;
            yield return new WaitForSeconds(blickSpeed);
            currentBlinkCount++;
        }
        playerMesh.enabled = true;
        currentBlinkCount = 0;
        isBlink = false;
    }
}